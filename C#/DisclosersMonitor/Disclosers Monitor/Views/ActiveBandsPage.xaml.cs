using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using Disclosers_Monitor.Core.Models;
using Disclosers_Monitor.Core.Services;
using Disclosers_Monitor.Services;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Disclosers_Monitor.Views
{
    public sealed partial class ActiveBandsPage : Page, INotifyPropertyChanged
    {
        CrossBase crossbase = new CrossBase("crossuniverse");

        // TODO WTS: Change the grid as appropriate to your app, adjust the column definitions on ActiveBandsPage.xaml.
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public ActiveBandsPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // TODO WTS: Replace this with your actual data

            
            DataGrid dt = new DataGrid();
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


        private void getLength(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            string[] coordinates = { "X", "Y", "Z", "V" };
            Dictionary<int, Dictionary<string, int>> dataDict = new Dictionary<int, Dictionary<string, int>>();
            for(int i = 1; i <= Int32.Parse(crossbase.getData("max_amount")); i++)
            {
                Dictionary<string, int> data = new Dictionary<string, int>();
                foreach(var coordinate in coordinates)
                {
                    string v = "id-" + i.ToString() + "-";
                    data.Add(coordinate, int.Parse(crossbase.getData(v + coordinate)));
                }
                dataDict.Add(i, data);
            }
            double rssi = calculateRSSI(dataDict[1], dataDict[2]);
            double distance = toDistance(rssi);
            if(distance < 1)
            {
                crossbase.setData("id-1-vibrate", "true");
                crossbase.setData("id-2-vibrate", "true");

            }
            LengthBox.Text = distance.ToString();

        }
        public double toDistance(double rssi)
        {
            double distance = 0;
            double power10 = 0;

            power10 = (rssi - 20 * Math.Log10((4 * 3.14 * 1) / (0.125))) / (10 * 1.7);
            distance = Math.Pow(10, power10);

            return distance;
        }
        public double calculateRSSI(Dictionary<string, int> bandOne, Dictionary<string, int> bandTwo)
        {
            double rssi = 0;
            int bX = bandOne["X"] - bandTwo["X"];
            int bY = bandOne["Y"] - bandTwo["Y"];
            int bZ = bandOne["Z"] - bandTwo["Z"];
            int bV = bandOne["V"] - bandTwo["V"];
            
            rssi = Math.Pow(bX, 2 )+ Math.Pow(bY, 2)+ Math.Pow(bZ, 2)+ Math.Pow(bV, 2);
            rssi = Math.Sqrt(rssi);
            return rssi;
        }
    }
}
