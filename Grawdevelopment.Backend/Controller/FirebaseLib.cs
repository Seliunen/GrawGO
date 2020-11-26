using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using GrawDevelopment.Configuration.ConfigurationObjects.Backend;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Grawdevelopment.Backend
{
    public class FirebaseLib
    {
        private const string Station = "station";
        private const string Flights = "flights";
        private const string RawData = "rawdata";
        private const string Users = "users";

        public string ApiKey { get; set; } = "AIzaSyAGn_Rw60zDbMxa9QNK7N5f2tyVPMfBp9Y";
        public string UserName { get; set; } = "123@aa.com";
        public string Password { get; set; } = "123456";
        public string Url { get; set; } = "https://grawapp.firebaseio.com/";
        public string Bucket { get; set; } = "grawapp.appspot.com";

        public Station CurrentStation { get; private set; }
        public string StationKey { get; private set; }


        public event EventHandler<int> SetProgress;
        protected virtual void OnSetProgress(object sender, int e)
        {
            SetProgress?.Invoke(sender, e);
        }

        //    FirebaseClient Client;
        public FirebaseAuthLink Authlink { get; set; } = null;
        protected FirebaseAuthProvider Auth = null;
        protected string Token = String.Empty;
        public AuthErrorReason ErrorCode { get; private set; } = AuthErrorReason.Undefined;

        public string ErrorText { get; private set; }

        public FirebaseLib()
        {

        }

        public FirebaseLib(BackendSettings settings)
        {
            ApiKey = settings.ApiKey;
            UserName = settings.User;
            Password = settings.PasswordDecrypted;
            Url = settings.Url;
            Bucket = settings.Bucket;
        }
        public FirebaseLib(string apiKey, string userName, string password)
        {
            ApiKey = apiKey;
            UserName = userName;
            Password = password;
        }

        public async Task<bool> LoginAsync()
        {
            var retvalue = await LoginAsync(UserName, Password);
            return retvalue;
        }



        public async Task<bool> LoginAsync(string username = "", string password = "")
        {

            try
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    UserName = username;
                    Password = password;
                }
                Auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                Authlink = await Auth.SignInWithEmailAndPasswordAsync(UserName, Password);
                await Authlink.RefreshUserDetails();
                Token = Authlink.FirebaseToken;
                return true;
            }
            catch (FirebaseAuthException e)
            {
                Console.WriteLine($"Error {e.Reason}");
                ErrorCode = e.Reason;
                // JObject obj = JObject.Parse(e.ResponseData);
                //var oMycustomclassname = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(e.ResponseData);
                // var text = obj["error"]["errors"]["message"];

            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error {ex.Message}");

                //JObject obj = JObject.Parse(ex.Message);
            }
            return false;
        }

        public User GetUser() => Authlink?.User;


        public async Task<bool> SignIn(string email, string password, string displayName = "",
            bool sendVerificationEmail = false)
        {
            try
            {
                Auth = new FirebaseAuthProvider(new FirebaseConfig(ApiKey));
                var auth = await Auth.CreateUserWithEmailAndPasswordAsync(email, password, displayName, sendVerificationEmail);

                return true;
            }
            catch (FirebaseAuthException ex)
            {
                ErrorCode = ex.Reason;
            }
            return false;
        }

        public async Task<bool> AddStation(Station item)
        {
            try
            {
                var firebaseClient = GetClient();
                var result = await firebaseClient?.Child("station").
                    PostAsync<Station>(item);
                return true;
            }
            catch (FirebaseException ex)
            {
                ErrorText = ex.Message;
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            return false;
        }

        public async Task<bool> AddUser(User item)
        {
            try
            {
                var firebaseClient = GetClient();
                if (firebaseClient != null && item.LocalId != null)
                {
                    await firebaseClient.Child(Users).Child(item.LocalId).PutAsync(item);
                    return true;
                }

                return false;
            }
            catch (FirebaseException ex)
            {
                ErrorText = ex.Message;
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            return false;
        }

        public async Task<bool> ChangeStation(Station station, String key)
        {
            try
            {
                var firebaseClient = GetClient();
                if (firebaseClient != null)
                    await firebaseClient?.Child("station")
                        .Child(key)
                        .PatchAsync<Station>(station);
                return true;
            }
            catch (FirebaseException ex)
            {
                ErrorText = ex.Message;
            }
            catch (Exception ex)
            {
                ErrorText = ex.Message;
            }
            return false;
        }

        public async Task<FirebaseObject<Station>> GetStation(int id)
        {
            var client = GetClient();
            var stations = await client?.Child("station").OnceAsync<Station>();

            var theStation = stations?.FirstOrDefault(x => x.Object.Id == id);

            CurrentStation = theStation?.Object;
            StationKey = theStation?.Key;
            return theStation;
        }

        public async Task<Station> GetStationObject(int id)
        {
            var client = GetClient();
            var stations = await client?.Child("station").OnceAsync<Station>();

            var theStation = stations?.FirstOrDefault(x => x.Object.Id == id);

            CurrentStation = theStation?.Object;
            StationKey = theStation?.Key;
            return CurrentStation;
        }

        public async Task<IEnumerable<StationDataObject>> GetStations()
        {
            var client = GetClient();
            var stations = await client?.Child(Station)?.OnceAsync<Station>();

            return stations.Select(item => new StationDataObject
            {
                Id = item.Object.Id,
                Name = item.Object.Name,
                City = item.Object.City,
                Country = item.Object.Country,
                Altitude = item.Object.Altitude,
                Longitude = item.Object.Longitude,
                Latitude = item.Object.Latitude,
                ImageName = item.Object.ImageName,
                ImageUrl = item.Object.ImageUrl,
                Key = item.Key
            })
                .ToList();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var client = GetClient();
            var users = await client?.Child(Users)?.OnceAsync<User>();

            return users.Select(item => new User
            {
                Email = item.Object.Email,
                DisplayName = item.Object.DisplayName,
                LocalId = item.Object.LocalId,
                IsEmailVerified = item.Object.IsEmailVerified,
            })
                .ToList();
        }

        public async Task<IEnumerable<FlightDataObject>> GetFlightData(string key)
        {
            var client = GetClient();
            var flighMetaData = await client?.Child(Station)?.Child(key).Child(Flights).OnceAsync<Flight>();
            var flightData = flighMetaData.Select(c => new FlightDataObject
            {
                Date = c.Object.EpochTime.FromUnixTime().ToShortDateString(),
                Time = c.Object.EpochTime.FromUnixTime().ToShortTimeString(),
                Key = c.Key,
                FileName = c.Object.FileName,
                Url = c.Object.Url,
                Url100 = c.Object.Url100,
                UrlEnd = c.Object.UrlEnd,
                IsRealTimeDataAvailable = c.Object.IsRealTimeDataAvailable,
                EpochTime = c.Object.EpochTime
            }).ToList();
            return flightData;
        }

        public async Task AddFlight(string fileName, int stationId,
            DateTime date, string fileWmo100 = "", string fileWmoEnd = "",
            string serialNumber = "", double duration = 0)
        {
            var station = await GetStation(stationId);
            var stream = File.Open(fileName, FileMode.Open);
            var outputName = Path.GetRandomFileName();
            //var uri = new System.Uri(Url);

            var task = new FirebaseStorage(Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(Authlink.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })
               .Child("data")
               .Child(outputName)
               .PutAsync(stream);
            task.Progress.ProgressChanged += (s, e) => OnSetProgress(this, e.Percentage);

            FirebaseStorageTask wmo100Task = null;
            if (!string.IsNullOrEmpty(fileWmo100))
            {
                var stream100 = File.Open(fileWmo100, FileMode.Open);
                var outputName100 = Path.GetRandomFileName();

                wmo100Task = new FirebaseStorage(Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(Authlink.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })
               .Child("data")
               .Child(outputName100)
               .PutAsync(stream100);
                task.Progress.ProgressChanged += (s, e) => OnSetProgress(this, e.Percentage);
            }

            FirebaseStorageTask wmoEndTask = null;
            if (!string.IsNullOrEmpty(fileWmoEnd))
            {
                var streamEnd = File.Open(fileWmoEnd, FileMode.Open);
                var outputNameEnd = Path.GetRandomFileName();

                wmoEndTask = new FirebaseStorage(Bucket,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(Authlink.FirebaseToken),
                    ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                })
               .Child("data")
               .Child(outputNameEnd)
               .PutAsync(streamEnd);
                task.Progress.ProgressChanged += (s, e) => OnSetProgress(this, e.Percentage);
            }

            try
            {
                // error during upload will be thrown when you await the task
                var url = await task;
                var url100 = "";
                if (wmo100Task != null)
                {
                    url100 = await wmo100Task ?? "";
                }
                var urlEnd = "";
                if (wmoEndTask != null)
                {
                    urlEnd = await wmoEndTask ?? "";
                }
                Console.WriteLine($"Download link:\n {url} \n {url100} \n {urlEnd} \n");

                var client = GetClient();
                var epochTime = date.GetUnixEpoch();
                var flightDb = await client.Child("station").
                    Child(station.Key).Child("flights").
                    PostAsync<Flight>(new Flight
                    {
                        Date = date.ToShortDateString(),
                        Time = date.ToLongTimeString(),
                        FileName = outputName,
                        Url = url,
                        Url100 = url100,
                        UrlEnd = urlEnd,
                        EpochTime = epochTime,
                        SerialNumber = serialNumber,
                        Duration = duration
                    });
            }
            catch (FirebaseStorageException e)
            {
                Console.WriteLine("Exception was thrown: {0}", e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}", ex);
            }

        }

        public async Task<string> AddImage(string imageFile)
        {
            var stream = File.Open(imageFile, FileMode.Open);
            var outputName = Path.GetRandomFileName();
            var task = new FirebaseStorage(Bucket,
                        new FirebaseStorageOptions
                        {
                            AuthTokenAsyncFactory = () => Task.FromResult(Authlink.FirebaseToken),
                            ThrowOnCancel = true // when you cancel the upload, exception is thrown. By default no exception is thrown
                        })
                    .Child("stationImages")
                    .Child(outputName)
                    .PutAsync(stream);
            task.Progress.ProgressChanged += (s, e) => OnSetProgress(this, e.Percentage);
            try
            {
                var url = await task;
                return url;
            }
            catch (FirebaseStorageException e)
            {
                Console.WriteLine("Exception was thrown: {0}", e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception was thrown: {0}", ex);
            }

            return string.Empty;
        }

        public async Task DeleteFromBucket(string fileName)
        {
            var task = new FirebaseStorage(Bucket,
                    new FirebaseStorageOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(Authlink.FirebaseToken),
                        ThrowOnCancel =
                            true // when you cancel the upload, exception is thrown. By default no exception is thrown
                    })
                .Child("data")
                .Child(fileName)
                .DeleteAsync();
            await task;
        }


        public string ConvertObjectToJsonFile(object item)
        {
            var serializedJson = JsonConvert.SerializeObject(item);
            var fileName = Path.Combine(Path.GetTempFileName());

            using (var myWriter = File.CreateText(fileName))
            {
                myWriter.WriteLine(serializedJson);
            }
            return fileName;
        }

        protected FirebaseClient GetClient()
        {
            try
            {
                if (Authlink == null)
                {
                    return null;
                }

                var firebaseClient = new FirebaseClient(Url,
                  new FirebaseOptions
                  {
                      //AuthTokenAsyncFactory = () => Task.FromResult(Authlink.FirebaseToken)
                      AuthTokenAsyncFactory = async () => (await Authlink.GetFreshAuthAsync()).FirebaseToken
                  });
                return firebaseClient;
            }
            catch (FirebaseException e)
            {
                Console.WriteLine("Exception was thrown: {0}", e.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return null;
        }


        public async Task<IReadOnlyCollection<FirebaseObject<object>>> GetStationUsers(string uid)
        {
            var client = GetClient();
            var stations = await client?.Child(Users)
                .Child(uid)
                .Child(Station)
                .OnceAsync<object>();


            return stations;
        }

        public async Task<string> GrantUserToStation(int stationId, User user)
        {
            var errorMessage = string.Empty;
            try
            {
                var stationObject = await GetStation(stationId);
                var client = GetClient();

                await client.Child(Users)
                    .Child(user.LocalId)
                    .Child(Station)
                    .Child(stationObject.Key)
                    .PutAsync(stationObject.Object.Id);

            }
            catch (FirebaseException e)
            {
                errorMessage = e.Message;
            }
            return errorMessage;

            //await client.Child(Station)
            //    .Child(stationObject.Key)
            //    .Child(Users)
            //    .Child(user.LocalId)
            //    .PutAsync(user);

        }
    }

    public static class ExtensionMethods
    {
        public static double GetUnixEpoch(this DateTime dateTime)
        {
            var unixTime = dateTime.ToUniversalTime() -
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return unixTime.TotalSeconds;
        }

        public static DateTime FromUnixTime(this double unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddSeconds(unixTime);
        }
    }

}
