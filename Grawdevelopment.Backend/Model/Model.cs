using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grawdevelopment.Backend
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

    public class StationDataObject:Station
    {
        public string Key { get; set; }
    }

    public class Flight
    {
        public string Date { get; set; }
        public string Time { get; set; }

        public string FileName { get; set; }
        public string Url { get; set; }
        public string Url100 { get; set; }
        public string UrlEnd { get; set; }

        public bool IsRealTimeDataAvailable { get; set; }

        public double EpochTime { get; set; }
        public string SerialNumber { get; set; }
        public double Duration { get; set; }
    }

    public class FlightDataObject:Flight
    {
        public string Key { get; set; }
    }

    public class RawData
    {
        public String TimeStamp { get; set; }
        public double Pressure { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public double Altitude { get; set; }
        public double WindSpeed { get; set; }
        public double WinDirection { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public double EpochTime { get; set; }
        public int SensorStatus { get; set; }
        public int TelemetryStatus { get; set; }
        public int GpsStatus { get; set; }
        public bool StartDetected { get; set; }
        public double StartTimeEpoch { get; set; }
        public string Url { get; set; }
        public double Battery { get; set; }
        public double Channel { get; set; }
        public double FieldStrength { get; set; }
    }
}
