using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetroFrance.Data.Models;
using MetroFrance.ViewModels.Items;
using MetroFrance.Libs;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Media;
using System.Diagnostics;
using System.Threading;

namespace MetroFrance.ViewModels
{
    public class HomeViewModel : MetroFrance.Common.BindableBase
    {
        #region Constants
        public const string ARTICLE_URL = "http://www.metrofrance.com/rss-mobile-fr-apps.xml?";
        public const string LANDSCAPE = "landscape";
        public const string TV = "WatTV";
        public const string METRO_DIAPO = "metro.diaporama";
        public const string GALLERIES = "GetGalleries";
        public const int HEIGHT_FIRST = 340;
        public const int WIDTH_FIRST = 570;
        public const int HEIGHT_OTHER = 170;
        public const int WIDTH_OTHER = 285;
        public const int HEIGHT_IMAGE = 170;
        public const int WIDTH_IMAGE = 380; 
        #endregion

        #region Properties

        private ObservableCollection<HubHomeViewModel> _hubs;

        private bool _dataLoaded = false;

        private int _nbItemsHeight = 6;

        private bool _isLandscape = true;

        private bool _isSnapped = false;

        private bool _alreadySet = false;

        //private bool _isLoadingData = false;

        private string _lastUpdateDate = string.Empty;

        private int NbMaxArticle;

        private DateTime BeginTime = DateTime.Now;

        private DateTime timer = new DateTime();

        private String resultTrace = String.Empty;

        #endregion

        #region Getters / Setters

        public ObservableCollection<HubHomeViewModel> Hubs
        {
            get { return _hubs; }
            set { this.SetProperty(ref this._hubs, value); }
        }

        public bool AlreadySet
        {
            get { return _alreadySet; }
            set { this.SetProperty(ref this._alreadySet, value); }
        }

        public int NbItemsHeight
        {
            get { return _nbItemsHeight; }
            set { this.SetProperty(ref this._nbItemsHeight, value); }
        }

        public bool DataLoaded
        {
            get { return _dataLoaded; }
            set { this.SetProperty(ref this._dataLoaded, value); }
        }

        public bool IsLandscape
        {
            get { return _isLandscape; }
            set { this.SetProperty(ref this._isLandscape, value); }
        }

        public bool IsSnapped
        {
            get { return _isSnapped; }
            set { this.SetProperty(ref this._isSnapped, value); }
        }

        public string LastUpdateDate
        {
            get { return _lastUpdateDate; }
            set { this.SetProperty(ref this._lastUpdateDate, value); }
        }

        #endregion

        private static readonly SemaphoreSlim _buildView = new SemaphoreSlim(initialCount: 1);

        public HomeViewModel()
        {
            Hubs = new ObservableCollection<HubHomeViewModel>();
        }

        public async Task<bool> LoadData(bool refresh, bool cache)
        {
            DataLoaded = false;

            try
            {
                if (this.IsSnapped)
                    Hubs = await DataToViewModelSnapped(refresh, cache);
                else
                {
                    //Hubs = await DataToViewModel(refresh, cache);
                    await DataToViewModel(refresh, cache);
                    await this.InsertMediaHub(refresh, cache);
                }

                //await App.DataAccess.LoadDataComplete();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

            // Initialize / Update Live Tile
            await App.HomeViewModel.UpdateLiveTile();

            DataLoaded = true;
            return true;
        }

        /// <summary>
        /// Update / Initialize Live Tile
        /// </summary>
        public async Task UpdateLiveTile()
        {
            try
            {
                await MetroFrance.Data.Libs.Helpers.LiveTile.Update(false, App.DataAccess); // CHANGED TO False because if all updates have been made, that's useless to ask to update a second time here.
            }
            catch { }
        }


        private async Task<ObservableCollection<HubHomeViewModel>> DataToViewModel(bool refresh, bool cache)
        {
            
            /*while (_isLoadingData)
            {
                // Do nothing. This is to avoid loading Data 2 times...
            }
            _isLoadingData = true;*/
            //Hubs = new ObservableCollection<HubHomeViewModel>();
            ArticleItemViewModel errorItem = new ArticleItemViewModel()
                        {
                            ErrorVisibility = Windows.UI.Xaml.Visibility.Visible,
                            HeightBorder = HEIGHT_FIRST,
                            WidthBorder = WIDTH_FIRST
                        };

            // Initialize local variables.
            ObservableCollection<HubHomeViewModel> hubs = new ObservableCollection<HubHomeViewModel>();
            ObservableCollection<HubHomeViewModel> hubsToDisplay = new ObservableCollection<HubHomeViewModel>();

            int NbMaxItemsALaUne = 0;

            if (IsLandscape)
                NbMaxItemsALaUne = ((((NbItemsHeight - 4) * 2) + NbItemsHeight) / 2) + 1;
            else
                NbMaxItemsALaUne = ((((NbItemsHeight - 4) * 2)) / 2) + 1;

            int NbMaxOthers = (((NbItemsHeight - 4) * 2) / 2) + 1;



            await _buildView.WaitAsync();

            List<Category> Categories = new List<Category>();

            try
            {
                Categories = await App.DataAccess.GetCategoriesAsync(refresh, cache);
                refresh = false;
            }
            catch
            {
                if (Hubs.Count == 0)
                {
                    _buildView.Release();
                    return new ObservableCollection<HubHomeViewModel>();
                }
                else return Hubs;
            }

            foreach (Category catego in Categories)
            {
                hubs.Add(new HubHomeViewModel()
                {
                    ID = ARTICLE_URL + catego.Guid,
                    Color = new SolidColorBrush(MetroFrance.Libs.ConvertHelpers.HexToColor(catego.Color)),
                    HubName = catego.Name
                });
            }

            int i = 0;

            if (hubs[2].HubName == App.ResourceLoader.GetString("NewsImages"))
                hubs.RemoveAt(2);

            Windows.UI.Xaml.Visibility filInfoVisibility = Windows.UI.Xaml.Visibility.Collapsed;
            Windows.UI.Xaml.Visibility normalVisibility = Windows.UI.Xaml.Visibility.Collapsed;

            foreach (HubHomeViewModel hub in hubs)
            {
                if (hub.HubName == App.ResourceLoader.GetString("TopNews"))
                {
                    NbMaxArticle = NbMaxItemsALaUne;
                    filInfoVisibility = Windows.UI.Xaml.Visibility.Collapsed;
                    normalVisibility = Windows.UI.Xaml.Visibility.Visible;
                }
                else
                {
                    if (hub.HubName == App.ResourceLoader.GetString("FilInfoTitle"))
                    {
                        NbMaxArticle = NbItemsHeight;
                        filInfoVisibility = Windows.UI.Xaml.Visibility.Visible;
                        normalVisibility = Windows.UI.Xaml.Visibility.Collapsed;
                    }
                    else
                    {
                        NbMaxArticle = NbMaxOthers;
                        filInfoVisibility = Windows.UI.Xaml.Visibility.Collapsed;
                        normalVisibility = Windows.UI.Xaml.Visibility.Visible;
                    }
                }


                try
                {
                    List<Article> Articles = await App.DataAccess.GetArticlesForCategoryAsync(hub.ID, refresh, cache);

                    if (Articles.Count == 0)
                        throw new System.ArgumentException("WSExeption", "No data");

                    int j = 0;
                    SolidColorBrush Color;

                    foreach (Article art in Articles)
                    {
                        if (j < NbMaxArticle)
                        {
                            Stretch stretch = (art.ImgOrientation == LANDSCAPE) ? Stretch.UniformToFill : Stretch.Uniform;

                            int sw = (j == 0) ? WIDTH_FIRST : WIDTH_OTHER;
                            int sh = (j == 0) ? HEIGHT_FIRST : HEIGHT_OTHER;

                            if (j % 2 == 0)
                                Color = new SolidColorBrush(MetroFrance.Libs.ConvertHelpers.HexToColor("#EAEBDD"));
                            else
                                Color = new SolidColorBrush(MetroFrance.Libs.ConvertHelpers.HexToColor("#E4F8D7"));

                            ArticleItemViewModel myArticle = new ArticleItemViewModel()
                            {
                                Guid = art.Guid,
                                ContentLight = art.DescrTXT,
                                Title = art.Title,
                                Author = art.Author,
                                FirstHub = (hub.HubName == App.ResourceLoader.GetString("TopNews")) ? true : false,
                                FilActu = (hub.HubName == App.ResourceLoader.GetString("FilInfoTitle")) ? true : false,
                                PublicationDate = ConvertHelpers.DateTimeToString(art.UpdateDate),
                                FilActuVisibility = filInfoVisibility,
                                NormalVisibility = normalVisibility,
                                ShareLink = art.Link,
                                Tag = (hub.HubName == App.ResourceLoader.GetString("FilInfoTitle")) ? art.ImgThumbnail : art.ImgBig,
                                HeightBorder = sh,
                                WidthBorder = sw,
                                URL = art.Link,
                                Category = hub.ID,
                                Color = Color,
                                Stretch = stretch,
                                Parent = hub,
                                HubName = hub.HubName
                            };

                            string img = (hub.HubName == App.ResourceLoader.GetString("FilInfoTitle")) ? art.ImgThumbnail : art.ImgBig;
                            MetroFrance.Data.DataAccess.ResizeParams rp = new Data.DataAccess.ResizeParams();
                            if (j == 0)
                            {
                                rp = ImageResizeBuilder.GenericResizeBuilder(ImageResizeBuilder.PictureType.BIG_ARTICLE_PICTURE, img);
                                this.LoadImage(myArticle, rp, true);
                            }
                            else
                            {
                                rp = ImageResizeBuilder.GenericResizeBuilder(ImageResizeBuilder.PictureType.MEDIUM_ARTICLE_PICTURE, img);
                                this.LoadImage(myArticle, rp, true);
                            }
                            
                            hub.Add(myArticle);
                            j++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Debug.WriteLine("Article de " + hub.HubName + " error : " + e.Message);
                    hub.Add(errorItem);
                }
                i++;

                hubsToDisplay.Add(hub);
                Hubs = hubsToDisplay;
            }

            Debug.WriteLine(resultTrace);

            _buildView.Release();
            //_isLoadingData = false;

            return hubs;
        }

        private async Task<ObservableCollection<HubHomeViewModel>> DataToViewModelSnapped(bool refresh, bool cache)
        {
            ArticleItemViewModel errorItem = new ArticleItemViewModel()
            {
                ErrorVisibility = Windows.UI.Xaml.Visibility.Visible,
                HeightBorder = HEIGHT_FIRST,
                WidthBorder = WIDTH_FIRST
            };

            int NbMaxItemsALaUne = 4;
            int NbMaxOthers = 1;
            int MaxItems = 0;
            int NbItems = 0;
            int i = 0;

            ObservableCollection<HubHomeViewModel> hubs = new ObservableCollection<HubHomeViewModel>();
            ObservableCollection<HubHomeViewModel> hubsToDisplay = new ObservableCollection<HubHomeViewModel>();

            List<MetroFrance.Data.Models.Category> Categories = new List<MetroFrance.Data.Models.Category>();

            try
            {
                Categories = await App.DataAccess.GetCategoriesAsync(refresh, cache);
                refresh = false;
            }
            catch
            {
                if (Hubs.Count == 0) return new ObservableCollection<HubHomeViewModel>();
                else return Hubs;
            }

            foreach (MetroFrance.Data.Models.Category catego in Categories)
            {
                if (catego.Name != App.ResourceLoader.GetString("NewsImages") && catego.Name != App.ResourceLoader.GetString("FilInfoTitle"))
                {
                    hubs.Add(new HubHomeViewModel()
                    {
                        ID = ARTICLE_URL + catego.Guid,
                        Color = new SolidColorBrush(MetroFrance.Libs.ConvertHelpers.HexToColor(catego.Color)),
                        HubName = catego.Name
                    });
                }
            }

            foreach (HubHomeViewModel hub in hubs)
            {
                try
                {
                    if (i == 0)
                    {
                        MaxItems = NbMaxItemsALaUne;
                    }
                    else
                    {
                        MaxItems = NbMaxOthers;
                    }
                    NbItems = 0;

                    List<MetroFrance.Data.Models.Article> Articles = await App.DataAccess.GetArticlesForCategoryAsync(hub.ID, refresh, cache);

                    foreach (MetroFrance.Data.Models.Article art in Articles)
                    {
                        Stretch stretch = (art.ImgOrientation == LANDSCAPE) ? Stretch.UniformToFill : Stretch.Uniform;

                        if (NbItems < MaxItems)
                        {

                            ArticleItemViewModel myArticle = new ArticleItemViewModel()
                            {
                                Guid = art.Guid,
                                NormalVisibility = Windows.UI.Xaml.Visibility.Visible,
                                Author = art.Author,
                                PublicationDate = ConvertHelpers.DateTimeToString(art.UpdateDate),
                                Title = art.Title,
                                HeightBorder = WIDTH_OTHER,
                                WidthBorder = HEIGHT_OTHER,
                                Tag = art.ImgBig,
                                Category = hub.ID,
                                ShareLink = art.Link,
                                URL = art.Link,
                                Stretch = stretch,
                                Parent = hub,
                                HubName = hub.HubName
                            };

                            MetroFrance.Data.DataAccess.ResizeParams rp = ImageResizeBuilder.GenericResizeBuilder(ImageResizeBuilder.PictureType.MEDIUM_ARTICLE_PICTURE, art.ImgBig);
                            this.LoadImage(myArticle, rp, true);
                            hub.Add(myArticle);
                        }
                        else
                        {
                            break;
                        }
                        NbItems++;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    hub.Add(errorItem);
                }
                i++;

                hubsToDisplay.Add(hub);
                Hubs = hubsToDisplay;
            }

            return hubs;
        }

        private async Task InsertMediaHub(bool refresh, bool cache)
        {
            try
            {
                Hubs.Insert(2, await MediaHub(refresh, cache));
            }
            catch (Exception)
            {
            }

            Hubs = await this.PhotosHubs(Hubs);
        }

        private async Task<HubHomeViewModel> MediaHub(bool refresh, bool cache)
        {
            ArticleItemViewModel errorItem = new ArticleItemViewModel()
            {
                ErrorVisibility = Windows.UI.Xaml.Visibility.Visible,
                HeightBorder = HEIGHT_FIRST,
                WidthBorder = WIDTH_FIRST
            };

            HubHomeViewModel Media = new HubHomeViewModel();
            bool video = false;
            Media.HubName = App.ResourceLoader.GetString("NewsImages");
            Media.Color = new SolidColorBrush(MetroFrance.Libs.ConvertHelpers.HexToColor("#098644"));
            int i = 0;
            List<Gallery> GalleriesItem = new List<Gallery>();
            int NbMaxItems = NbItemsHeight / 2;

            try
            {
                this.TraceBegin(GALLERIES);
                GalleriesItem = await App.DataAccess.GetGalleriesAsync(refresh, cache);
                this.TraceEnd(GALLERIES);
            }
            catch
            {

                Media.Add(errorItem);
                return Media;
            }

            GalleriesItem.Sort(delegate(Gallery p1, Gallery p2) { return p1.Order.CompareTo(p2.Order); });
            try
            {
                foreach (Gallery gal in GalleriesItem)
                {
                    //if (gal.CType == "videos" && !video)
                    if (gal.Guid == "wat.tv" && !video)
                    {
                        this.TraceBegin(TV);
                        List<WatTvItem> watTv = await App.DataAccess.GetWatTvItemAsync(gal.Url, refresh, cache);
                        this.TraceEnd(TV);
                        video = true;

                        ArticleItemViewModel myArticle = new ArticleItemViewModel()
                        {
                            Guid = watTv[0].Guid,
                            NormalVisibility = Windows.UI.Xaml.Visibility.Visible,
                            ImageHub = true,
                            PublicationDate = ConvertHelpers.DateTimeToString(watTv[0].PubDate),
                            Title = watTv[0].Title,
                            Tag = watTv[0].Image.Url,
                            VideoVisibility = Windows.UI.Xaml.Visibility.Visible,
                            IsVideo = true,
                            WidthBorder = WIDTH_IMAGE,
                            HeightBorder = HEIGHT_IMAGE,
                            Category = App.ResourceLoader.GetString("VideoCategoryTitle"),
                            Parent = Media,
                            URL = watTv[0].EmbededVideoUrl,
                            ContentLight = MetroFrance.Libs.ConvertHelpers.ParseAndDeleteHtml(watTv[0].DescrXML)
                        };

                        Media.Add(myArticle);

                        MetroFrance.Data.DataAccess.ResizeParams rp = ImageResizeBuilder.GenericResizeBuilder(ImageResizeBuilder.PictureType.MEDIUM_ARTICLE_PICTURE, watTv[0].Image.Url);
                        this.LoadImage(myArticle, rp, true);

                    }
                    if (gal.Guid == METRO_DIAPO && video && i < NbMaxItems)
                    {

                        this.TraceBegin(App.ResourceLoader.GetString("Diapo"));
                        List<Diaporama> Diapos = await App.DataAccess.GetDiaporamaAsync(gal.Url, refresh, cache);
                        this.TraceEnd(App.ResourceLoader.GetString("Diapo"));

                        foreach (Diaporama diap in Diapos)
                        {
                            if (i < NbMaxItems - 1)
                            {

                                ArticleItemViewModel myArticle = new ArticleItemViewModel()
                                {
                                    URL = diap.Link,
                                    Guid = diap.Guid,
                                    NormalVisibility = Windows.UI.Xaml.Visibility.Visible,
                                    ImageHub = true,
                                    ShareLink = diap.Sharelink,
                                    Title = diap.Title,
                                    GalleryVisibility = Windows.UI.Xaml.Visibility.Visible,
                                    WidthBorder = WIDTH_IMAGE,
                                    MarginText = new Windows.UI.Xaml.Thickness(5, 0, 55, 0),
                                    Tag = diap.ImgThumbnail,
                                    HeightBorder = HEIGHT_IMAGE,
                                    Stretch = Windows.UI.Xaml.Media.Stretch.Uniform,
                                    Category = METRO_DIAPO,
                                    PublicationDate = ConvertHelpers.DateTimeToString(diap.PubDate)
                                };


                                Media.Add(myArticle);
                                MetroFrance.Data.DataAccess.ResizeParams rp = ImageResizeBuilder.GenericResizeBuilder(ImageResizeBuilder.PictureType.MEDIUM_ARTICLE_PICTURE, diap.ImgThumbnail);
                                this.LoadImage(myArticle, rp, true);

                            }
                            i++;
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("Gallery + WaTV error : " + e.Message);
                Media.Add(errorItem);
            }

            return Media;
        }

        public async Task ChangeLocal()
        {
            List<Category> Categories = await App.DataAccess.GetCategoriesAsync(true, false);
            Hubs.RemoveAt(Hubs.Count - 2);
            int NbMaxOthers = (((NbItemsHeight - 4) * 2) / 2) + 1;

            List<Article> Articles = await App.DataAccess.GetArticlesForCategoryAsync(ARTICLE_URL + Categories[Categories.Count - 2].Guid, true, false);

            Hubs.Insert(Hubs.Count - 1, new HubHomeViewModel()
            {
                ID = ARTICLE_URL + Categories[Categories.Count - 2].Guid,
                Color = new SolidColorBrush(MetroFrance.Libs.ConvertHelpers.HexToColor(Categories[Categories.Count - 2].Color)),                
                HubName = Categories[Categories.Count - 2].Name
            });

            int j = 0;


            foreach (Article art in Articles)
            {
                Stretch stretch = (art.ImgOrientation == LANDSCAPE) ? Stretch.UniformToFill : Stretch.Uniform;
                int sw = (j == 0) ? WIDTH_FIRST : WIDTH_OTHER;
                int sh = (j == 0) ? HEIGHT_FIRST : HEIGHT_OTHER;

                if (j < NbMaxOthers)
                {
                    ArticleItemViewModel article = new ArticleItemViewModel()
                    {
                        Guid = art.Guid,
                        ContentLight = art.DescrTXT,
                        Title = art.Title,
                        Author = art.Author,
                        PublicationDate = ConvertHelpers.DateTimeToString(art.UpdateDate),
                        NormalVisibility = Windows.UI.Xaml.Visibility.Visible,
                        FilActu = false,
                        HeightBorder = sh,
                        WidthBorder = sw,
                        Tag = art.ImgBig,
                        ShareLink = art.Link,
                        URL = art.Link,
                        Category = Hubs[Hubs.Count - 2].ID,
                        Stretch = stretch,
                        Parent = Hubs[Hubs.Count - 2],
                        HubName = Hubs[Hubs.Count - 2].HubName
                    };

                    MetroFrance.Data.DataAccess.ResizeParams rp = new Data.DataAccess.ResizeParams();
                    if (j == 0)
                    {
                        rp = ImageResizeBuilder.GenericResizeBuilder(ImageResizeBuilder.PictureType.BIG_ARTICLE_PICTURE, art.ImgBig);
                    }
                    else
                    {
                        rp = ImageResizeBuilder.GenericResizeBuilder(ImageResizeBuilder.PictureType.MEDIUM_ARTICLE_PICTURE, art.ImgBig);
                    }
                    this.LoadImage(article, rp, true);
                    Hubs[Hubs.Count - 2].Add(article);
                }
                else
                {
                    break;
                }
                j++;
            }
            Hubs = await this.PhotosHubs(Hubs);
        }

        private async Task<ObservableCollection<HubHomeViewModel>> PhotosHubs(ObservableCollection<HubHomeViewModel> hubs)
        {
            List<List<string>> photos = new List<List<string>>();

            foreach (HubHomeViewModel hub in hubs)
            {
                List<string> pics = new List<string>();
                foreach (ArticleItemViewModel art in hub)
                {
                    pics.Add(art.Tag);
                }
                photos.Add(pics);
            }

            try
            {
                for (int i = 0; i < photos.Count - 1; i++)
                {
                    for (int j = i + 1; j < photos.Count; j++)
                    {
                        try
                        {
                            photos[j].Remove(photos[i][0]);
                        }
                        catch { }
                    }
                }

                int cpt = 0;

                foreach (HubHomeViewModel hub in hubs)
                {
                    MetroFrance.Data.DataAccess.ResizeParams rp = ImageResizeBuilder.GenericResizeBuilder(ImageResizeBuilder.PictureType.BIG_ARTICLE_PICTURE, photos[cpt][0]);

                    BitmapImage img = new BitmapImage();
                    img.SetSource(await App.DataAccess.GetBitmapSourceAsync(rp));
                    hub.Img = img;

                    cpt++;
                }
            }
            catch
            {
            }
            return hubs;
        }

        public String GetIDHubName(string HubName)
        {
            foreach (HubHomeViewModel hub in Hubs)
            {
                if (hub.HubName == HubName)
                    return hub.ID;
            }
            return String.Empty;
        }

        private void TraceBegin(String NameHub)
        {
            timer = DateTime.Now;
        }

        private void TraceEnd(String NameHub)
        {
            string repere = (DateTime.Now - BeginTime).Seconds.ToString() + "." + (DateTime.Now - BeginTime).Milliseconds.ToString();
            string timeOfMethod = (DateTime.Now - timer).Seconds.ToString() + "." + (DateTime.Now - timer).Milliseconds.ToString();
            resultTrace += repere + "; " + NameHub + "; " + timeOfMethod + "\n";
        }

        public async Task<bool> InitializeDatas()
        {
            List<Category> Categories = new List<Category>();
            try
            {
                Categories = await App.DataAccess.GetCategoriesAsync(true, false);
            }
            catch
            {
                return false;
            }

            foreach (Category cat in Categories)
            {
                try
                {
                    List<Article> Articles = await App.DataAccess.GetArticlesForCategoryAsync(ARTICLE_URL + cat.Guid, true, false);
                }
                catch { }
            }

            List<Gallery> GalleriesItem = new List<Gallery>();

            try
            {
                GalleriesItem = await App.DataAccess.GetGalleriesAsync(true, false);
            }
            catch
            {
                return true;
            }

            foreach (Gallery gal in GalleriesItem)
            {
                if (gal.CType == "videos")
                {
                    try
                    {
                        List<WatTvItem> watTv = await App.DataAccess.GetWatTvItemAsync(gal.Url, true, false);
                    }
                    catch { }
                }
                if (gal.CType != "videos")
                {
                    try
                    {
                        List<Diaporama> Diapos = await App.DataAccess.GetDiaporamaAsync(gal.Url, true, false);
                    }
                    catch { }
                }
            }

            return true;
        }

    }
}
