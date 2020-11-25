using Grawdevelopment.Backend;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrawGoConsole.Controller
{
    public class StationDatasource
    {
        private List<Station> _stations;

        public StationDatasource()
        {

            //var item1 = new Station();
            //item1.Id = item.Id;
            //item1.Name = item.Name;
            //item1.City = item.City;
            //item1.Country = item.Country;
            //item1.Latitude = item.Latitude;
            //item1.Longitude = item.Longitude;
            //item1.Altitude = item.Altitude;
            //item1.IsPublic = item.IsPublic;
            //_stations.Add(item1);

            //var item2 = new Station();
            //item2.Id = 2;
            //item2.Name = "Station 2";
            //item2.City = "Wendelstein";
            //item2.Country = "Deutschland";
            //item2.Latitude = "49.25";
            //item2.Longitude = "11.987";
            //item2.Altitude = "450";
            //item2.IsPublic = false;
            //_stations.Add(item2);
        }


        public async Task GetData()
        {
            _stations = new List<Station>();
            var fireBase = new FirebaseLib();
            var result = await fireBase.LoginAsync();

            if (result)
            {
                var stations = await fireBase.GetStations();
                foreach (var item in stations)
                {
                    _stations.Add(item);
                }
            }
        }


        public List<Station> GetDataSource()
        {
            return _stations;
        }
    }
}
