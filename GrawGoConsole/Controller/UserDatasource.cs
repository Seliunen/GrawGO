using GrawGoConsole.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrawGoConsole.Controller
{
    public class UserDatasource
    {

        private List<User> _User;

        public UserDatasource()
        {

            _User = new List<User>();

            var item1 = new User();
            item1.Id = 1;
            item1.EMail = "max.mustermann@mustermann.de";
            item1.Name = "Mustermann";
            item1.firstname = "Max";
            _User.Add(item1);

            var item2 = new User();
            item2.Id = 2;
            item2.EMail = "max.mustermann@mustermann.de";
            item2.Name = "Mustermann";
            item2.firstname = "Max";
            _User.Add(item2);

        }

        public List<User> GetDataSource()
        {
            return _User;
        }

    }
}
