using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace KeePassWin
{
    public class OnedriveArbiter
    {
        private MsaAuthenticationProvider msaAuthProvider;
        private OneDriveClient oneDriveClient;
        private bool connected = false;

        public OnedriveArbiter()
        {
            Storage.FileSavedEvent += updateFile;
            Storage.FileDeletedEvent += deleteFle;
        }

        #region PUBLIC METHODS

        public bool HasFirstTimeSyncComplete()
        {
            return App.LocalSettings.Values["#OD_firstTimeSync"] != null;
        }

        public async Task<bool> FirstKissOnedrive()
        {

            //download database
            ItemChildrenCollectionPage dbs = await this.getDbs();
            foreach (Item file in dbs)
            {
                string content = "";
                Stream stream = await this.oneDriveClient
                              .Drive
                              .Items[file.Id]
                              .Content
                              .Request()
                              .GetAsync();
                StreamReader reader = new StreamReader(stream);
                using (reader)
                {
                    content = reader.ReadToEnd();
                }
                Storage.saveFile(file.Name, content);
            }

            //upload database
            IReadOnlyList<StorageFile> localDbs = await Storage.getFiles();
            foreach (StorageFile localDb in localDbs)
            {
                await this.updateDb(localDb);
            }

            return true;
        }

        public async Task<bool> Connect()
        {
            if (App.LocalSettings.Values["#OD_refreshToken"] != null && App.LocalSettings.Values["#OD_userId"] != null)
            {
                await this.refreshConnect();
            }
            else
            {
                await this.connect();
            }

            ItemChildrenCollectionPage dbs = await this.getDbs();

            this.connected = true;

            return true;
        }

        public bool IsConnected()
        {
            return this.connected;
        }

        #endregion

        #region PRIVATE METHODS

        private async void updateFile(StorageFile file)
        {
            if (this.IsConnected()) {
                await this.updateDb(file);
            }
        }

        private async void deleteFle(StorageFile file)
        {
            if (this.IsConnected()) {
                await this.deleteDb(file);
            }
        }

        private async Task<bool> deleteDb(StorageFile db)
        {
            await this.oneDriveClient
              .Drive
              .Root
              .ItemWithPath("KeeSync/" + db.Name)
              .Request()
              .DeleteAsync();

            return true;
        }

        private async Task<bool> updateDb(StorageFile db)
        {
            var randomAccessStream = await db.OpenReadAsync();
            Stream stream = randomAccessStream.AsStreamForRead();
            using (stream)
            {
                var createdFolder = await this.oneDriveClient
                    .Drive
                    //.Items[App.LocalSettings.Values["#OD_rootFolderId"].ToString()]
                    .Root
                    .ItemWithPath("KeeSync/"+db.Name)
                    .Content
                    .Request() 
                    .PutAsync<Item>(stream);
            }

            return true;
        }

        private async Task<bool> connect()
        {
            this.msaAuthProvider = new MsaAuthenticationProvider(
                App.Config.applicationId.Value,
                    "https://login.live.com/oauth20_desktop.srf",
                    new string[] {
                        "onedrive.readwrite"
                        ,"wl.offline_access"
                        ,"wl.signin"
                    }
            );

            await this.msaAuthProvider.AuthenticateUserAsync();
            this.oneDriveClient = new OneDriveClient("https://api.onedrive.com/v1.0", this.msaAuthProvider);
            App.LocalSettings.Values["#OD_refreshToken"] = this.msaAuthProvider.CurrentAccountSession.RefreshToken;
            App.LocalSettings.Values["#OD_userId"] = this.msaAuthProvider.CurrentAccountSession.UserId;

            return true;
        }

        private async Task<bool> refreshConnect()
        {
            AccountSession session = new AccountSession();
            session.ClientId = App.LocalSettings.Values["#OD_userId"].ToString();
            session.RefreshToken = App.LocalSettings.Values["#OD_refreshToken"].ToString();
            this.msaAuthProvider = new MsaAuthenticationProvider(
                App.Config.applicationId.Value,
                    "https://login.live.com/oauth20_desktop.srf",
                    new string[] {
                        "onedrive.readwrite"
                        ,"wl.offline_access"
                        ,"wl.signin"
                    }
            );

            this.oneDriveClient = new OneDriveClient("https://api.onedrive.com/v1.0", this.msaAuthProvider);
            this.msaAuthProvider.CurrentAccountSession = session;
            await this.msaAuthProvider.AuthenticateUserAsync();

            return true;
        }

        private async Task<ItemChildrenCollectionPage> getDbs()
        {
            if (App.LocalSettings.Values["#OD_rootFolderId"] == null)
            {
                await this.creteRootFolder();
            }

            bool exists = await this.existsFolder(App.LocalSettings.Values["#OD_rootFolderId"].ToString());
            if ( !exists ) {
                await this.creteRootFolder();
            }
            
            ItemChildrenCollectionPage dbs = (ItemChildrenCollectionPage)await oneDriveClient
                .Drive
               .Items[App.LocalSettings.Values["#OD_rootFolderId"].ToString()]
               .Children 
               .Request()
                .GetAsync();

            return dbs;
        }

        private async Task<bool> existsFolder(string id)
        {
            try {
                var dbs = await oneDriveClient
               .Drive
              .Items[id]
              .Request()
               .GetAsync();

                return true;
            }
            catch(Exception e){
                return false;
            }
        }

        private async Task<string> creteRootFolder()
        {
            var root = await oneDriveClient
                .Drive
                .Root
                .Request()
                .GetAsync();

            var rootFolder = await oneDriveClient
                .Drive
                .Items[root.Id]
                .Children
                .Request()
                .AddAsync(new Item { Name = "KeeSync", Folder = new Folder() });

            App.LocalSettings.Values["#OD_rootFolderId"] = rootFolder.Id;
            return rootFolder.Id;

        }

        #endregion
    }
}
