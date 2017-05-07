using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace KeePassWin
{
    class Storage
    {
        private static string extension = "kpw";

        public static async void saveFile(string path, string content)
        {
            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync(getRealName(path), Windows.Storage.CreationCollisionOption.FailIfExists);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, content);
        }

        public static async Task<IReadOnlyList<StorageFile>> getFiles()
        {
            StorageFolder picturesFolder = ApplicationData.Current.LocalFolder;
            IReadOnlyList<StorageFile> fileList = await picturesFolder.GetFilesAsync();
            return fileList;
        }

        public static async Task<string> getContentFile( string name )
        {
            string body = "";
            name = getRealName(name);

            IReadOnlyList<StorageFile> fileList = await getFiles();
            foreach (StorageFile file in fileList)
            {
                if (file.Name == name) {
                    body = await Windows.Storage.FileIO.ReadTextAsync(file);
                }
            }

            return body;
        }

        private static string getRealName(string name)
        {
            return name + "." + extension;
        }
    }
}
