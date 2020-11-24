using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraBars.Objects;
using FirebaseNet.Messaging;
using Newtonsoft.Json;
using Grawdevelopment.Backend;

namespace Grawdevelopment.Backend.Controller
{
    public enum PushNotificationMessageType
    {
        StartDetected,
        FlightEnded,
        LevelReached,
        BurstDetected
    }

   
    public class FireBaseMessagingService
    {
        private readonly string _ApiKey = "AIzaSyAmolliz0UXriDjoosREDAReK9bhFT7eRk";
        private readonly string _stationNameKey = "station_name_key";
        private readonly string _stationIdKey = "station_Id_key";
        private readonly string _dateKey = "date_key";

        private readonly string _notificationFrom = "[notification_from]";
        private readonly string _flightEnded = "[flight_ended]";
        private readonly string _startDetected = "[start_detected]";
        private readonly string _burstDetected = "[burst_detected]";



        private FireBaseMessagingService()
        {
            
        }
        public static FireBaseMessagingService Shared = new FireBaseMessagingService();

        private Dictionary<string, string> GetPayload(DateTime date, Station station,
            string title, string body)
        {
            var dateString = date.GetUnixEpoch().ToString(CultureInfo.InvariantCulture);
            var payload = new Dictionary<string,string>
            {
                {_stationNameKey,station.Name },
                {_stationIdKey,station.Id.ToString() },
                {_dateKey,dateString },
                {"title",title },
                {"body",body},

            };
            return payload;
        }

        public async Task SendMessage( PushNotificationMessageType messageType,
            Station station,string topic, DateTime date)
        {
            var client = new FCMClient(_ApiKey);
            var context = GetTitleAndBodyPlaceHolder(messageType);
            var payload = GetPayload(date, station,context.title,context.body);
            
            //var topic = stationKey;// "news";
            var message = new Message()
            {

                To = $"/topics/{topic}_iOS",
                Data = payload,
                Notification = new IOSNotificationExtented()
                {
                    Body = context.body,
                    Title = context.title,
                    MutableContent = "true"
                    //Icon = "myIcon"
                }

            };
            
            var result = await client.SendMessageAsync(message);

            message = new Message()
            {

                To = $"/topics/{topic}_Android",
                Data = payload
                //Notification = new IOSNotificationExtented()
                //{
                //    Body = context.body,
                //    Title = context.title,
                //    MutableContent = "true"
                //    //Icon = "myIcon"
                //}

            };

            result = await client.SendMessageAsync(message);
        }

        private (string title, string body) GetTitleAndBodyPlaceHolder(PushNotificationMessageType type)
        {
            switch (type)
            {
                case PushNotificationMessageType.StartDetected:
                    return (_notificationFrom, _startDetected);
                case PushNotificationMessageType.FlightEnded:
                    return (_notificationFrom, _flightEnded);
                case PushNotificationMessageType.BurstDetected:
                    return (_notificationFrom, _burstDetected);
                default:
                    return (_notificationFrom, "not supported");

            }
        }

    }

    public class IOSNotificationExtented : IOSNotification
    {
        [JsonProperty(PropertyName = "mutable_content")]
        public string MutableContent { get; set; }
    }

    public class MessageExtended : Message
    {
        [JsonProperty(PropertyName = "mutable_content")]
        public string MutableContent { get; set; }
    }
}
