using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
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


        private async void getLength(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            
            await BackgroundCalulating();
           

        }
        public double toDistance(double rssi)
        {
            double distance = 0;
            rssi = -rssi-6;
            double rssi2 = rssi * rssi;
            double rssi3 = rssi2 * rssi;
            double rssi4 = rssi3 * rssi;
            double rssi5 = rssi4 * rssi;

            distance = (-0.043 * rssi5 - 4.92 * rssi4 - 171.5 * rssi3 - 600.8 * rssi2 + 41.41 * rssi - 0.84) / (rssi4 + 250.4 * rssi3 + 14780 * rssi2 - 455.9 * rssi + 12.24);

            return distance;
        }
        public double calculateRSSI(Dictionary<string, int> bandOne, Dictionary<string, int> bandTwo)
        {

            double[] a = triangularSystem(bandOne);
            double[] b = triangularSystem(bandTwo);

            double l = Math.Sqrt(Math.Pow(a[0] - b[0], 2) + Math.Pow(a[1] - b[1], 2));

            return l;
        }

        public double[] triangularSystem(Dictionary<string, int> band)
        {
            double r1 = toDistance(band["X"]);
            double r2 = toDistance(band["Y"]);
            double r3 = toDistance(band["Z"]);
            double x2 = 2;
            double y3 = 3.5;
            double x3 = 1;

            double x = (r1 * r1 - r2 * r2 + x2 * x2) /( 2 * x2);
            double y = (r1 * r1 - r3 * r3 + x3 * x3 + y3 * y3 - 2 * x * x3) / (2 * y3);

            double[] coordinates = { x, y };

            return coordinates;
        }

        public async Task BackgroundCalulating()
        {
            while (true)
            {
                Dictionary<int, Dictionary<string, int>> dataDict = await Average();
                double rssi = calculateRSSI(dataDict[1], dataDict[2]);
                if (rssi <  int.Parse(crossbase.getData("min_distance")))
                {
                    crossbase.setData("id-1-vibrate", "true");
                    crossbase.setData("id-2-vibrate", "true");

                }
                
                LengthBox.Text = (rssi*10).ToString();
            }
            

            
        }
        public async Task<Dictionary<int, Dictionary<string, int>>> Average()
        {
            string[] coordinates = { "X", "Y", "Z" };
            Dictionary<int, Dictionary<string, int>> dataDict = new Dictionary<int, Dictionary<string, int>>();
            
            await Task.Run(()=>
            {
                
                double times = 1;
                int max_amount = Int32.Parse(crossbase.getData("max_amount"));
                for (int j = 0; j < times; j++)
                {
                    for (int i = 1; i <= max_amount; i++)
                    {
                        if (j == 0)
                        {
                            Dictionary<string, int> data = new Dictionary<string, int>();
                            foreach (var coordinate in coordinates)
                            {
                                string v = "id-" + i.ToString() + "-";
                                data.Add(coordinate, int.Parse(crossbase.getData(v + coordinate)));
                            }
                            dataDict.Add(i, data);
                        }
                        else
                        {
                            foreach (var data in dataDict)
                            {
                                var d = data.Value;
                                foreach (var coordinate in coordinates)
                                {
                                    string v = "id-" + i.ToString() + "-";
                                    d[coordinate] += int.Parse(crossbase.getData(v + coordinate));

                                }
                            }
                        }

                    }
                }
                for (int i = 1; i <= max_amount; ++i)
                {
                    foreach (var data in dataDict)
                    {
                        var d = data.Value;
                        foreach (var coordinate in coordinates)
                        {
                            string v = "id-" + i.ToString() + "-";
                            d[coordinate] = (int)(d[coordinate] / times);

                        }
                    }
                }
            }

            );

            await Task.Delay(5000);


            return dataDict;
        }
    }
}
