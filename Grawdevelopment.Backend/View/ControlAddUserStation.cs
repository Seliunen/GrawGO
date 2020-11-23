using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Firebase.Auth;
using Firebase.Database;
using GrawDevelopment.Configuration.ConfigurationObjects.Backend;
using GrawDevelopment.Configuration.Languages;
using GrawDevelopment.GeneralTypeDefinition.Localization;
using GrawDevelopment.GeneralTypeDefinition;

namespace Grawdevelopment.Backend.View
{
    public partial class ControlAddUserStation : DevExpress.XtraEditors.XtraUserControl
    {
        private static ILocalization _localization = Container<ILocalization>.Value;

        public FirebaseLib FireBase { get; set; }
        public Station StationSetting { get; set; }

        BackendSettings settings;

        private User _user;
        public BackendSettings Settings
        {
            get => settings;
            set
            {
                settings = value;
                textEdit1.Text = Settings?.User;
                //textEdit2.Text = Settings?.PasswordDecrypted;
            }
        }
        public ControlAddUserStation()
        {
            InitializeComponent();
        }

        private void ControlAddUserStation_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            simpleButton1.Text = _localization.LoadText(1034);
            
            simpleButton2.Enabled = false;
            simpleButton2.Text = _localization.LoadText(1033);

            layoutControlItem1.Text = _localization.LoadText(1035);
        }

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textEdit1.Text) )
            {
                XtraMessageBox.Show(_localization.LoadText(1036), _localization.LoadText(1037), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            SplashScreenManager.ShowDefaultWaitForm(_localization.LoadText(3700), _localization.LoadText(3700));
            //var result = await FireBase.LoginAsync(textEdit1.Text, textEdit2.Text);

            var users = await FireBase.GetUsers();
            _user = users?.FirstOrDefault(x => x.Email == textEdit1.Text);
            SplashScreenManager.CloseDefaultWaitForm();
            if (_user == null)
            {
                //XtraMessageBox.Show($"User {textEdit1.Text} does not exist in the cloud service. Please subscribe user to cloud service", "user not found", MessageBoxButtons.OK,
                //    MessageBoxIcon.Stop);
                XtraMessageBox.Show(_localization.LoadText(2537) + " " + textEdit1.Text  + " " + _localization.LoadText(1039), _localization.LoadText(1040), MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);

                return;
            }

           


            //if (!result)
            //{
            //    XtraMessageBox.Show($"Errorcode: {FireBase.ErrorCode}", "login error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            //    return;
            //}

           

            ////var exits = users?.Count(x => x.LocalId == _user.LocalId) ?? 0;
            
            simpleButton2.Enabled = true;

        }

         async Task<IReadOnlyCollection<FirebaseObject<object>>> GetStationUsers(string uid)
        {
            return await FireBase.GetStationUsers(uid);
        }

        private async void simpleButton2_Click(object sender, EventArgs e)
        {

            //var stations = await FireBase.GetStations();
           


            var stations = await GetStationUsers(_user.LocalId);
            var count = stations.Count(x => Convert.ToInt32(x.Object) == StationSetting.Id);

            if (count == 0)
            {
                var errorMessage = await FireBase.GrantUserToStation(StationSetting.Id, _user);
                if (string.IsNullOrEmpty(errorMessage))
                {
                    XtraMessageBox.Show(_localization.LoadText(2537) + " " + _user.Email + " " + _localization.LoadText(1041) + " " + StationSetting.Id,
                        _localization.LoadText(3841), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    XtraMessageBox.Show(_localization.LoadText(2537) + " " + _user.Email + _localization.LoadText(1043) + " " + StationSetting.Id + ": " + errorMessage,
                        _localization.LoadText(3738), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            else
            {
                XtraMessageBox.Show(_localization.LoadText(2537) + " " + _user.Email + " " + _localization.LoadText(1042) + " " + StationSetting.Id, _localization.LoadText(3841), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            //users = await GetStationUsers();

            //if (users != null)
            //{
            Console.Write("User not null");
            //}

            
        }
    }
}
