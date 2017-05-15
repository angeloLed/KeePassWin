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
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile sampleFile = await storageFolder.CreateFileAsync(mergeExtension(path), CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, content);
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
            name = mergeExtension(name);

            IReadOnlyList<StorageFile> fileList = await getFiles();
            foreach (StorageFile file in fileList)
            {
                if (file.Name == name) {
                    body = await FileIO.ReadTextAsync(file);
                }
            }

            return body;
        }

        private static string mergeExtension(string name)
        {
            if (name.EndsWith(extension)) {
                return name;
            }
            else {
                return name + "." + extension;
            }
        }
    }
}
