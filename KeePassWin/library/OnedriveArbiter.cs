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

        public async Task FirstKissOnedrive()
        {
            //download database
            await this.downloadFilesProcedure();

            //upload database
            await this.uploadFilesProcedure();

            App.LocalSettings.Values["#OD_firstTimeSync"] = true;
        }

        public async Task Sync()
        {
            //download/update database
            ItemChildrenCollectionPage remoteDbs = await this.downloadFilesProcedure();

            //check dbs to delete
            List<string> localDbsToDelete = new List<string>();
            IReadOnlyList<StorageFile> localDbs = await Storage.getFiles();
            foreach (StorageFile db in localDbs)
            {
                var result = remoteDbs.Where(d => d.Name == db.Name);
                if (result.Count() == 0) {
                    localDbsToDelete.Add(db.Name);
                }
            }

            //procede to delete
            foreach (string dbName in localDbsToDelete)
            {
                await Storage.deleteFile(dbName);
            }

        }

        public async Task Connect()
        {
            try {
                if (App.LocalSettings.Values["#OD_refreshToken"] != null && App.LocalSettings.Values["#OD_userId"] != null)
                {
                    await this.refreshConnect();
                }
                else
                {
                    await this.connect();
                }

                //ItemChildrenCollectionPage dbs = await this.getDbs();

                this.connected = true;
            }
            catch (Exception e) {
                this.connected = false;
            }

        }

        public bool IsConnected()
        {
            return this.connected;
        }

        #endregion

        #region PRIVATE METHODS

        private async Task<ItemChildrenCollectionPage> downloadFilesProcedure()
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

            return dbs;
        }

        private async Task uploadFilesProcedure()
        {
            IReadOnlyList<StorageFile> localDbs = await Storage.getFiles();
            foreach (StorageFile localDb in localDbs)
            {
                await this.updateDb(localDb);
            }
        }

        private void updateFile(StorageFile file)
        {
            if (this.IsConnected()) {
                this.updateDb(file);
            }
        }

        private void deleteFle(StorageFile file)
        {
            if (this.IsConnected()) {
                this.deleteDb(file);
            }
        }

        private async Task deleteDb(StorageFile db)
        {
            await this.oneDriveClient
              .Drive
              .Root
              .ItemWithPath("KeeSync/" + db.Name)
              .Request()
              .DeleteAsync();

        }

        private async Task updateDb(StorageFile db)
        {
            var randomAccessStream = await db.OpenReadAsync();
            Stream stream = randomAccessStream.AsStreamForRead();
            using (stream)
            {
                var createdFolder = await this.oneDriveClient
                    .Drive
                    //.Items[App.LocalSettings.Values["#OD_rootFolderId"].ToString()]
                    .Root
                    .ItemWithPath("KeeSync/" + db.Name)
                    .Content
                    .Request()
                    .PutAsync<Item>(stream);
            }
        }

        private async Task connect()
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
        }

        private async Task refreshConnect()
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
