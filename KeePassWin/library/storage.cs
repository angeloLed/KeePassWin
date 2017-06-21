using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace KeePassWin
{
    delegate void FileSaved(StorageFile sender);
    delegate void FileDeleted(StorageFile sender);

    class Storage
    {
        public static event FileSaved FileSavedEvent;
        public static event FileDeleted FileDeletedEvent;

        private static string extension = "kpw";

        public static async Task<bool> deleteFile(string name)
        {
            IReadOnlyList<StorageFile> files = await getFiles();
            StorageFile fileDeleted = null;

            foreach (StorageFile file in files) {
                if (file.Name == mergeExtension(name)) {
                    fileDeleted = file;
                    await file.DeleteAsync();
                }
            }

            FileDeletedEvent?.Invoke(fileDeleted);

            return true;
        }

        public static async void saveFile(string path, string content)
        {
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file = await storageFolder.CreateFileAsync(mergeExtension(path), CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(file, content);

                FileSavedEvent?.Invoke(file);
            }
            catch (Exception ex)
            {
                //TODO: trapping better
            }
        }

        public static async Task<IReadOnlyList<StorageFile>> getFiles()
        {
            StorageFolder appFolder = ApplicationData.Current.LocalFolder;
            IReadOnlyList<StorageFile> fileList = await appFolder.GetFilesAsync();
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
