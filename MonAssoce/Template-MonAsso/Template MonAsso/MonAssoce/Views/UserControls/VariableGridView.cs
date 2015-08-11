using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonAssoce.ViewModels.Items;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MonAssoce.Views.UserControls
{
    public class VariableGridView : GridView
    {
        private int rowVal;
        private int colVal;
        private Random _rand;
        private List<Size> _sequence;
        private List<Size> _sequenceOther;

        public VariableGridView()
        {
            _rand = new Random();
            _sequence = new List<Size> { 
                LayoutSizes.PrimaryPhoto,
                LayoutSizes.SecondaryPhotoItem, LayoutSizes.SecondaryPhotoItem
            };

            _sequenceOther = new List<Size> { 
                LayoutSizes.OtherSmallItem, LayoutSizes.OtherSmallItem,
                LayoutSizes.OtherSmallItem, LayoutSizes.OtherSmallItem
            };

        }
        protected override void PrepareContainerForItemOverride(Windows.UI.Xaml.DependencyObject element, object item)
        {
           base.PrepareContainerForItemOverride(element, item);

           MainItemViewModel dataItem = item as MainItemViewModel;
            int index =-1;
            int SecondIndx = -1;

            if (dataItem != null)
            {
                index = App.MainPageViewModel.Hubs[0].IndexOf(dataItem);
            }

            if (index == -1)
            {
                SecondIndx = App.MainPageViewModel.Hubs[1].IndexOf(dataItem);
                if (SecondIndx == -1)
                {
                    SecondIndx = App.MainPageViewModel.Hubs[2].IndexOf(dataItem);
                    if (SecondIndx == -1)
                    {
                        SecondIndx = App.MainPageViewModel.Hubs[3].IndexOf(dataItem);
                    }
                }
            }

            if (index >= 0 && dataItem.IsDescription)
            {

                (element as UIElement).IsHitTestVisible = false;
                if (index < _sequence.Count)
                {
                    colVal = (int)_sequence[index].Width;
                    rowVal = (int)_sequence[index].Height;
                }
                else
                {
                    colVal = (int)_sequence[1].Width;
                    rowVal = (int)_sequence[1].Height;
                }

            }
            else
            {
                colVal = (int)_sequenceOther[0].Width;
                rowVal = (int)_sequenceOther[0].Height;

            }

            VariableSizedWrapGrid.SetRowSpan(element as UIElement, rowVal);
            VariableSizedWrapGrid.SetColumnSpan(element as UIElement, colVal);
        }
    }

    public static class LayoutSizes {
        public static Size PrimaryPhoto = new Size(9, 6);
        public static Size SecondaryPhotoItem = new Size(3, 2);
        public static Size OtherSmallItem = new Size(3,3);
    }
}
