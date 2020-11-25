using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrawGoConsole.Model
{
    public class User
    {
        public string Id { get; set; }

        public string EMail { get; set; }

        public string Name { get; set; }

        public string firstname { get; set; }

        public bool IsEmailVerified { get; set; }
    }
}
