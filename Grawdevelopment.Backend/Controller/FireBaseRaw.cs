using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Storage;

namespace Grawdevelopment.Backend.Controller
{
    public class FireBaseRaw:FirebaseLib
    {

        string _flightKey;
        string _rawDataKey;
        string _stationKey;
        private string _lastFile = String.Empty;
        IDisposable _subscription;

        public event EventHandler<RawData> SetRawData;

        protected virtual void OnSetRawData( RawData e)
        {
            SetRawData?.Invoke(this, e);
        }

        public FireBaseRaw()
        {
            _rawDataKey = String.Empty;
            
        }

        //public FireBaseRaw(BackendSettings settings)
        //{
        //    ApiKey = settings.ApiKey;
        //    UserName = settings.User;
        //    Password = settings.PasswordDecrypted;
        //    Url = settings.Url;
        //    Bucket = settings.Bucket;
        //}
        //public FireBaseRaw(string apiKey, string userName, string password)
        //{
        //    ApiKey = apiKey;
        //    UserName = userName;
        //    Password = password;
        //}

        public async Task<bool> AddRawFlight(int id,string serialNumber)
        {
            try
            {
                var client = GetClient();
                var station = await GetStation(id);
                if (station != null)
                {
                    var epochTime = DateTime.UtcNow.GetUnixEpoch();
                    var flight = await client.Child("station").
                           Child(station.Key).Child("flights").
                           PostAsync<Flight>(new Flight
                           {
                               Date = DateTime.UtcNow.ToString("MM/dd/yyyy"),
                               Time = DateTime.UtcNow.ToLongTimeString(),
                               FileName = "",
                               Url = "",
                               IsRealTimeDataAvailable = true,
                               EpochTime = epochTime,
                               SerialNumber = serialNumber
                           });
                    _flightKey = flight.Key;
                    _stationKey = station.Key;

                    _subscription = client.Child("station").
                      Child(_stationKey).Child("flights").
                      Child(_flightKey).Child("rawdata").AsObservable<RawData>()
                      .Subscribe(x => OnSetRawData(x.Object));

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;

        }
        public async Task<bool> AddRawData(RawData data)
        {
            try
            {
                await Authlink.RefreshUserDetails();
                var client = GetClient();
                
                if (!string.IsNullOrEmpty(_rawDataKey))
                {
                    await client.Child("station").
                         Child(_stationKey).Child("flights").
                         Child(_flightKey).Child("rawdata").Child(_rawDataKey).DeleteAsync();
                }

                var rawdata = await client.Child("station").
                       Child(_stationKey).Child("flights").
                       Child(_flightKey).Child("rawdata").
                       PostAsync<RawData>(data);
                _rawDataKey = rawdata.Key;
                return true;
            }
            catch (FirebaseException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        public async Task DeleteRawFlight()
        {
            try
            {
                var client = GetClient();
                await client.Child("station").
                        Child(_stationKey).Child("flights").
                        Child(_flightKey).DeleteAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<string> AddFlightProfile(string fileName)
        {
            var stream = File.Open(fileName, FileMode.Open);
            var outputName = Path.GetRandomFileName();
           
            var url = await new FirebaseStorage(Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(Authlink.FirebaseToken),
                        ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                    })
                .Child("data")
                .Child(outputName)
                .PutAsync(stream);
            await DeleteFlightProfile();
            _lastFile = outputName;
            return url;
        }

        private async Task DeleteFlightProfile()
        {
            if (!String.IsNullOrEmpty(_lastFile))
            {
                await new FirebaseStorage(Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(Authlink.FirebaseToken),
                            ThrowOnCancel =
                                true // when you cancel the upload, exception is thrown. By default no exception is thrown
                        })
                    .Child("data")
                    .Child(_lastFile)
                    .DeleteAsync();
            }
        }
    }
}
