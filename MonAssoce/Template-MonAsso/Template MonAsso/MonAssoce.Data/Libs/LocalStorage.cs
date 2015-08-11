using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace MonAssoce.Data.Libs
{
    public class LocalStorage
    {
        Helpers help = new Helpers();
        private static LocalStorage _instance;

        private LocalStorage() { }

        public static LocalStorage Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LocalStorage();
                }
                return _instance;
            }
        }

        public async Task<string> NewURI(string fileName, string URI)
        {
            //Get the Local Folder
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            try
            {
                if (NetworkInterface.GetIsNetworkAvailable())
                {
                    XDocument doc = XDocument.Load(URI);


                    StorageFile myFile = await localFolder.CreateFileAsync(fileName + ".xml", CreationCollisionOption.ReplaceExisting);
                    string Content = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>\n" + ReplacePhoto(fileName, doc.ToString()).ToString(); ;
                    
                    await FileIO.WriteTextAsync(myFile, Content);
                    return myFile.Path;
                }
                else
                {
                    try
                    {
                        var file = await StorageFile.GetFileFromPathAsync(localFolder.Path + "\\" + fileName + ".xml");
                        XDocument doc = XDocument.Load(file.Path);
                        this.ReplacePhoto(fileName, doc.ToString());
                        return file.Path;
                    }
                    catch
                    {
                        return fileName;
                    }
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return fileName;
        }

        public string ReplacePhoto(string folderName, string Content)
        {
            var localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            List<string> picsList = new List<string>();

            string[] smallpics = Content.Split(new string[] { "<pictureURI>", "</pictureURI>" }, System.StringSplitOptions.None);
            string[] bigpics = Content.Split(new string[] { "<bigPictureURI>", "</bigPictureURI>" }, System.StringSplitOptions.None);
            string[] pics = Content.Split(new string[] { "<image>", "</image>" }, System.StringSplitOptions.None);

            foreach (string pic in smallpics)
            {
                if (!pic.Contains("<") && pic != "")
                    picsList.Add(pic);
            }

            foreach (string pic in bigpics)
            {
                if (!pic.Contains("<") && pic != "")
                    picsList.Add(pic);
            }

            foreach (string pic in pics)
            {
                if (!pic.Contains("<") && pic != "")
                    picsList.Add(pic);
            }

            foreach (string pic in picsList)
            {
                if (help.IsRemoteURI(pic))
                {
                    string[] fileNameRemote = pic.Split('/');
                    if (NetworkInterface.GetIsNetworkAvailable())
                    {
                        //this.GetImage(pic, fileNameRemote[fileNameRemote.Length - 1], folderName);
                        this.GetImageUpdated(pic, fileNameRemote[fileNameRemote.Length - 1], folderName);
                    }
                    string newPic = localFolder.Path + "\\" + folderName + "\\" + fileNameRemote[fileNameRemote.Length - 1];
                    newPic = newPic.Replace('\\', '/');
                    Content = Content.Replace(pic, newPic);
                    Content = Content.Replace("<remotePictureURI>" + newPic + "</remotePictureURI>", "<remotePictureURI>" + pic + "</remotePictureURI>");
                }
            }
            return Content;
        }

        public async Task GetImage(string url, string fileName, string folderName)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            byte[] img = await response.Content.ReadAsByteArrayAsync();
            InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream(); 

            StorageFile file = null;
            StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

            file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            
            IRandomAccessStream writeStream = await file.OpenAsync(FileAccessMode.ReadWrite);
            Stream outStream = Task.Run(() => writeStream.AsStreamForWrite()).Result;
            await outStream.WriteAsync(img, 0, img.Length);

            await outStream.FlushAsync();
            writeStream.Dispose();
        }

        public List<DownloadOperation> activeDownloads = new List<DownloadOperation>();

        private async Task GetImageUpdated(string url, string fileName, string folderName)
        {
            try
            {
                Uri source = new Uri(url);
                string destination = fileName;
                StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
                StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
                BackgroundDownloader downloader = new BackgroundDownloader();
                DownloadOperation download = downloader.CreateDownload(source, file);

                await HandleDownloadAsync(download);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("------------------------" + Environment.NewLine +
                                "Download Error" + Environment.NewLine + 
                                "Filename : " + fileName + Environment.NewLine +
                                "Folder : " + folderName + Environment.NewLine +
                                "------------------------", ex);
            }
        }

        private async Task HandleDownloadAsync(DownloadOperation download)
        {
            try
            {
                activeDownloads.Add(download);
                await download.StartAsync().AsTask();
                Debug.WriteLine("------------------------" + Environment.NewLine +
                "Download Started" + Environment.NewLine +
                "Filename : " + download.ResultFile.Name + Environment.NewLine +
                "Folder : " + download.ResultFile.Path + Environment.NewLine +
                "------------------------");
                ResponseInformation response = download.GetResponseInformation();
            }
            finally
            {
                activeDownloads.Remove(download);
                Debug.WriteLine("------------------------" + Environment.NewLine +
                "Download Finished" + Environment.NewLine +
                "Filename : " + download.ResultFile.Name + Environment.NewLine +
                "Folder : " + download.ResultFile.Path + Environment.NewLine +
                "------------------------");
            }
        }

        private async Task ReplaceImageByDefault(string url, string fileName, string folderName)
        {
            StorageFolder folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);
            StorageFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
        }
    }
}
