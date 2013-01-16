using System;
using System.IO;
using Windows.Security.Cryptography.Core;
using Windows.Security.Cryptography;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Streams;
using Windows.Storage.FileProperties;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;

namespace GoogleCalendar
{
    /// <summary>
    /// Class used for writing and reading files
    /// </summary>
    public static class FileManager
    {
        private static StorageFolder GetDirectory()
        {
            return ApplicationData.Current.RoamingFolder;
        }
        public static async Task<string> ReadFile(string file_name)
        {
            try
            {
                StorageFolder storageFolder = GetDirectory();
                StorageFile file = await storageFolder.GetFileAsync(file_name);
                IRandomAccessStream readStream = await file.OpenAsync(FileAccessMode.Read);
                IInputStream inputStream = readStream.GetInputStreamAt(0);
                DataReader dataReader = new DataReader(inputStream);
                uint numBytesLoaded = await dataReader.LoadAsync((uint)readStream.Size);
                readStream.Dispose();
                return dataReader.ReadString(numBytesLoaded);
            }
            catch (Exception)
            {
                return null;
            }
        }
        
        public static void WriteFileAsync(string file_name, string text)
        {
            WriteFileAsync(file_name, text, CreationCollisionOption.ReplaceExisting);
        }

        public static async void WriteFileAsync(string file_name, string text, CreationCollisionOption collisionOption)
        {         
            try
            {
                StorageFolder folder = GetDirectory();
                StorageFile file = await folder.CreateFileAsync(file_name, collisionOption);
                IRandomAccessStream stream = await file.OpenAsync(FileAccessMode.ReadWrite);
                IOutputStream outputStream = stream.GetOutputStreamAt(stream.Size);
                DataWriter dataWriter = new DataWriter(outputStream);
                dataWriter.WriteString(text);
                await dataWriter.StoreAsync();
                await outputStream.FlushAsync();
                stream.Dispose();
            }
            catch (Exception)
            {}
        }

        public static async void RemoveFileAsync(string file_name)
        {
            try
            {
                StorageFolder folder = GetDirectory();
                StorageFile file = await folder.GetFileAsync(file_name);
                await file.DeleteAsync(StorageDeleteOption.PermanentDelete);
            }
            catch (Exception)
            { }
        }
        
        public static void Log(string message)
        {
            WriteFileAsync("system.log", String.Format("{0} : {1}\r\n", DateTime.Now, message), CreationCollisionOption.OpenIfExists);
        }
    }
}
