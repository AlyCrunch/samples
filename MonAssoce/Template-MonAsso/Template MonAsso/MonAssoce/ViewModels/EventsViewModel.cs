using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonAssoce.ViewModels.Items;
using MonAssoce.Data;
using MonAssoce.Data.Models;

namespace MonAssoce.ViewModels
{
    public class EventsViewModel : Common.CategoryViewModel<EventItemViewModel>
    {
        public EventsViewModel()
        {
            this.Items = new ObservableCollection<EventItemViewModel>();
        }

        public async Task LoadData()
        {
            if (this.Items.Count == 0)
            {
                //LoadSampleData();
                await this.LoadProcessedData();
            }
        }

        public async Task LoadProcessedData()
        {
            await ProcessData.LoadSettings();
            List<Event> tempList = ProcessData.GetEvents();

            foreach (Event eventModel in tempList)
            {
                EventItemViewModel eventItem = new EventItemViewModel();
                eventItem.ModelToItem(eventModel);
                this.Items.Add(eventItem);
            }
        }

        public void LoadSampleData()
        {
            for (int i = 1; i <= 20; i++)
            {
                EventItemViewModel item = new EventItemViewModel();
                item.Image = "/Content/Images/Events/default.png";
                item.Title = "Event title " + i;
                item.Subtitle = "20 avril 2012";
                item.Content = "Pastrami biltong capicola swine, shankle spare ribs" +
                " ham leberkas. Swine short ribs venison chuck, prosciutto beef boudin" +
                " leberkas pork chop kielbasa pork belly bresaola spare ribs t-bone" +
                " shank. Sausage biltong pancetta, ham hamburger t-bone leberkas" +
                " spare ribs andouille short ribs sirloin ball tip short loin" +
                " prosciutto rump. Chuck bacon bresaola tenderloin, prosciutto" +
                " leberkas rump jerky cow pork loin sirloin. Strip steak short loin" +
                " andouille, pancetta pork flank brisket pork chop hamburger shank" +
                " meatloaf rump pig filet mignon leberkas. Bresaola brisket leberkas" +
                " filet mignon pork belly rump, ball tip andouille. Rump pork chicken" +
                " beef meatloaf, short loin swine shankle. Bresaola leberkas brisket" +
                " strip steak, chicken ham hock spare ribs kielbasa shankle ribeye" +
                " venison pork pastrami pork loin corned beef. Beef turkey drumstick" +
                " cow strip steak. Capicola shankle shank, prosciutto jowl pork belly" +
                " bresaola hamburger. Shank turducken short ribs jowl sausage chuck" +
                " fatback. Tail hamburger chicken, fatback beef chuck sirloin drumstick" +
                " pancetta ribeye. Drumstick short loin prosciutto sausage, brisket" +
                " tongue meatball chicken corned beef. Tri-tip beef ribs capicola" +
                " chicken, corned beef hamburger boudin kielbasa cow pig flank" +
                " andouille. Bresaola swine beef ribs, shank shankle spare ribs tri-tip" +
                " tongue. Ham hock tenderloin spare ribs, short loin drumstick hamburger" +
                " frankfurter beef ribs fatback leberkas. Bacon prosciutto pork chop," +
                " short ribs venison pig kielbasa shank capicola spare ribs bresaola" +
                " boudin. Swine sausage jerky cow ham hock. Turducken frankfurter" +
                " sausage ball tip fatback biltong. Meatball jerky fatback, strip steak" +
                " beef leberkas pork loin frankfurter short loin biltong ground round ball" +
                " tip jowl pork chop salami. Pork chop filet mignon brisket, capicola chuck" +
                " pancetta bresaola short ribs beef ribs flank ground round pork belly cow" +
                " salami. Frankfurter beef boudin filet mignon, turkey tenderloin" +
                " prosciutto rump drumstick pork swine tri-tip ham hock pancetta chuck." +
                " Strip steak sirloin pork loin, ribeye tongue meatball short ribs kielbasa" +
                " drumstick brisket. Pig short loin pork loin boudin pastrami tail, andouille" +
                " drumstick. Cow short ribs beef ribs shoulder venison chicken. Filet mignon" +
                " bresaola chicken, turducken pork chop venison bacon.";
                this.Items.Add(item);
            }
        }

        public async void RefreshData()
        {
            this.Items = new ObservableCollection<EventItemViewModel>();
            ProcessData.AllEvents = null;
            await ProcessData.LoadEventsImages();
        }
    }
}
