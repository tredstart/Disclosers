using Disclosers_Monitor.Services;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Windows.UI.Xaml.Controls;

namespace Disclosers_Monitor.Views
{
    public sealed partial class BandsMapPage : Page, INotifyPropertyChanged
    {
        CrossBase crossbase = new CrossBase("crossuniverse");
        public BandsMapPage()
        {
            InitializeComponent();

        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
            {
                return;
            }

            storage = value;
            OnPropertyChanged(propertyName);
        }

        private void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));



        private void SetAP(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string AP_X = XPoint.Text;
            string AP_Y = YPoint.Text;
            string AP_Z = ZPoint.Text;


            crossbase.setData("AP-X", AP_X);
            crossbase.setData("AP-Y", AP_Y);
            crossbase.setData("AP-Z", AP_Z);
            successed();
        }
        public async void successed()
        {
            ContentDialog success = new ContentDialog
            {
                Title = "Set successfully",
                Content = "You can close it",
                CloseButtonText = "Ok"
            };
            ContentDialogResult result = await success.ShowAsync();
        }



        private void setMinDistance(object sender, Windows.UI.Xaml.Input.ManipulationCompletedRoutedEventArgs e)
        {
            string minimumDistance = DistancePicker.Value.ToString();
            crossbase.setData("min_distance", minimumDistance);
        }
    }
}
