using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrawGoConsole.Model;

namespace GrawGoConsole.Controller
{
    public class StationDatasource
    {
        private List<Station> _stations;

        public StationDatasource()
        {
            _stations = new List<Station>();

            var item1 = new Station();
            item1.Id = 1;
            item1.Name = "Station 1";
            item1.City = "Nürnberg";
            item1.Country = "Deutschland";
            item1.Latitude = "49.12";
            item1.Longitude = "11.345";
            item1.Altitude = "309";
            item1.IsPublic = true;
            _stations.Add(item1);


            var item2 = new Station();
            item2.Id = 2;
            item2.Name = "Station 2";
            item2.City = "Wendelstein";
            item2.Country = "Deutschland";
            item2.Latitude = "49.25";
            item2.Longitude = "11.987";
            item2.Altitude = "450";
            item2.IsPublic = false;
            _stations.Add(item2);
        }

        public List<Station> GetDataSource()
        {
            return _stations;
        }
    }
}
