using Microsoft.OneDrive.Sdk;
using Microsoft.OneDrive.Sdk.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeePassWin
{
    class Onedrive
    {
        public async void connect()
        {
            

            var msaAuthProvider = new MsaAuthenticationProvider(
                App.Config.applicationId.Value,
                    "https://login.live.com/oauth20_desktop.srf",
                    new string[] {
                        "onedrive.readwrite"
                        ,"wl.offline_access"
                        ,"wl.signin"
                    }
            );

            await msaAuthProvider.AuthenticateUserAsync();
            OneDriveClient oneDriveClient = new OneDriveClient("https://api.onedrive.com/v1.0", msaAuthProvider);
            string RefreshToken = msaAuthProvider.CurrentAccountSession.RefreshToken;
            string ClientID = msaAuthProvider.CurrentAccountSession.ClientId;
            var rootItem = await oneDriveClient
                            .Drive
                            .Root
                            .Request()
                            .GetAsync();




            // -------------------  AFTER
            AccountSession session = new AccountSession();
            session.ClientId = ClientID;
            session.RefreshToken = RefreshToken;
            var _msaAuthenticationProvider = new MsaAuthenticationProvider(
                App.Config.applicationId.Value,
                    "https://login.live.com/oauth20_desktop.srf",
                    new string[] {
                        "onedrive.readwrite"
                        ,"wl.offline_access"
                        ,"wl.signin"
                    }
            );

            var _oneDriveClient = new OneDriveClient("https://api.onedrive.com/v1.0", _msaAuthenticationProvider);
            _msaAuthenticationProvider.CurrentAccountSession = session;
            await _msaAuthenticationProvider.AuthenticateUserAsync();
            rootItem = await oneDriveClient
                .Drive
                .Root
                .Request()
                .GetAsync();

        }
    }
}
