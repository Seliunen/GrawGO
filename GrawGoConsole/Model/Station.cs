using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrawGoConsole.Model
{
    public class Station
    {
        //xxx
        public int Id { get; set; }
        public string Name { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public string Altitude { get; set; }

        public bool IsPublic { get; set; }

        public string ImageUrl { get; set; }

        public string ImageName { get; set; }

    }
}
