using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonAssoce.Data.Models;
using Windows.UI.Notifications;
using MonAssoce.NotificationsExtensions.BadgeContent;
using MonAssoce.NotificationsExtensions.TileContent;
using NotificationsExtensions.TileContent;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace MonAssoce.Data.Libs
{
    public class LiveTilesHelper
    {
        public async static Task GetLiveTiles()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);

            await ProcessData.LoadSettings();
            List<Event> events = ProcessData.GetEvents();
            

            ITileWideBlockAndText01 tileContent = TileContentFactory.CreateTileWideBlockAndText01();
            //ITileSquareText02 squareContent = TileContentFactory.CreateTileSquareText02();
            ITileSquarePeekImageAndText02 squareContent = TileContentFactory.CreateTileSquarePeekImageAndText02();
            if (events.Count > 5)
            {
                events = events.GetRange(0, 5);
            }
            

            for(int i = events.Count - 1; i >= 0; i--) 
            {
                tileContent.StrictValidation = true;
                tileContent.TextBlock.Text = events[i].Date.Day.ToString();
                tileContent.TextSubBlock.Text = events[i].Date.ToString("MMMM");
                tileContent.TextBody1.Text = events[i].Title;

                try
                {
                    tileContent.TextBody2.Text = events[i].Description.Substring(20, 20);
                    tileContent.TextBody3.Text = events[i].Description.Substring(40, 20);
                    tileContent.TextBody4.Text = events[i].Description.Substring(60, events[i].Description.Length - 60);
                }
                catch { }

                squareContent.TextHeading.Text = events[i].Date.ToString("M/d/yy");
                squareContent.TextBodyWrap.Text = events[i].Title;
                if (events[i].PictureURI != "")
                    squareContent.Image.Src = events[i].PictureURI;
                else
                    squareContent.Image.Src = "ms-appx://Content/Images/Events/default.png";
                tileContent.SquareContent = squareContent;

                TileNotification tileNotification = tileContent.CreateNotification();

                TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
            }
        }

        public static void GetLiveTilesTest()
        {
            TileUpdateManager.CreateTileUpdaterForApplication().Clear();
            TileUpdateManager.CreateTileUpdaterForApplication().EnableNotificationQueue(true);
            
            ITileWideBlockAndText01 tileContent = TileContentFactory.CreateTileWideBlockAndText01();
            ITileSquarePeekImageAndText02 squareContent = TileContentFactory.CreateTileSquarePeekImageAndText02();

            tileContent.StrictValidation = true;
            tileContent.TextBlock.Text = "OK";
            tileContent.TextSubBlock.Text = "Test";
            tileContent.TextBody1.Text = "Ca marche";
            tileContent.TextBody2.Text = "Background task ok";

            squareContent.TextHeading.Text = "OK";
            squareContent.TextBodyWrap.Text = "Background task ok";
            squareContent.Image.Src = "http://static.freepik.com/photos-libre/ok-icone_17-1009133509.jpg";

            tileContent.SquareContent = squareContent;

            TileNotification tileNotification = tileContent.CreateNotification();

            TileUpdateManager.CreateTileUpdaterForApplication().Update(tileNotification);
        }
    }
}
