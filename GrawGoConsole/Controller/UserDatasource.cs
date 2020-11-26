using Firebase.Auth;
using Grawdevelopment.Backend;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrawGoConsole.Controller
{
    public class UserDatasource
    {

        private List<User> _user;


        public UserDatasource()
        {
            //_user = new List<User>();

            //var item1 = new User();
            //item1.Id = 1;
            //item1.EMail = "max.mustermann@mustermann.de";
            //item1.Name = "Mustermann";
            //item1.firstname = "Max";
            //_user.Add(item1);

            //var item2 = new User();
            //item2.Id = 2;
            //item2.EMail = "max.mustermann@mustermann.de";
            //item2.Name = "Mustermann";
            //item2.firstname = "Max";
            //_user.Add(item2);

        }

        public async Task GetData()
        {
            _user = new List<User>();
            var fireBase = new FirebaseLib();
            var result = await fireBase.LoginAsync();

            if (result)
            {

                var user = await fireBase.GetUsers();

                foreach (var item in user)
                {
                    _user.Add(item);
                }
            }
        }

        public List<User> GetDataSource()
        {
            return _user;
        }

    }
}
