using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grawdevelopment.Backend.Controller
{
    public class FirebaseLoginController
    {
        private FirebaseLib _firebaseLib;

        private ILoginResults _loginResults;

        public FirebaseLoginController(FirebaseLib firebaseLib, ILoginResults loginResults)
        {
            _firebaseLib = firebaseLib;
            _loginResults = loginResults;
        }

        public async Task LoginUser(string userName, string password)
        {
            

            // SplashScreenManager.ShowDefaultWaitForm("please wait....", "login in progress");
            var result = await _firebaseLib.LoginAsync(userName, password);


            if (!result)
            {
                _loginResults?.GetError($"{_firebaseLib.ErrorCode}");
                return;
            }

            var user = _firebaseLib.GetUser();

            var users = await _firebaseLib.GetUsers();

            var exits = users?.Count(x => x.LocalId == user.LocalId) ?? 0;
            if (exits == 0)
            {
                await _firebaseLib.AddUser(user);
            }
            
            _loginResults?.GetFirebaseResult(_firebaseLib);
        }
    }

    public interface ILoginResults
    {
        void GetError(string errorCode);
        void GetFirebaseResult(FirebaseLib result);
    }
}
