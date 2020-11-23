using DevExpress.XtraEditors;
using Grawdevelopment.Backend.Controller;
using GrawDevelopment.Common.Splash;
using GrawDevelopment.Configuration.ConfigurationObjects.Backend;
using GrawDevelopment.GeneralTypeDefinition;
using GrawDevelopment.GeneralTypeDefinition.Enum;
using GrawDevelopment.GeneralTypeDefinition.Interface;
using GrawDevelopment.GeneralTypeDefinition.Localization;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using GrawDevelopment.Configuration;

namespace Grawdevelopment.Backend.View
{
    public partial class FormBackendNavigation : DevExpress.XtraEditors.XtraForm, ILoginResults
    {
        private static ILocalization _localization = Container<ILocalization>.Value;

        private FormLogin _formLogin;
        FirebaseLib FireBase;
        public Station StationSetting { get; set; }
        public BackendSettings Settings { get; set; }

        public ISoundingDataCollection Sounding { get; set; }

        public string Temp100FileName { get; set; }
        public string TempEndFileName { get; set; }

        public IProtection Protection { get; set; }
        private ProgressHud _progressHud = ProgressHud.Shared;


        public FormBackendNavigation()
        {
            InitializeComponent();
        }

        public FormBackendNavigation(BackendSettings settings, ISoundingDataCollection sounding,
            string temp100FileName = "", string tempEndFileName = "") : this()
        {
            Settings = settings;
            Settings.Load(ConfigurationStorage.LocalAndGlobal);
            Sounding = sounding;
            Temp100FileName = temp100FileName;
            TempEndFileName = tempEndFileName;
        }

        private void navigationPane1_Click(object sender, EventArgs e)
        {

        }

        private async void FormBackendNavigation_Load(object sender, EventArgs e)
        {
            FireBase = new FirebaseLib(Settings);
            Show();
            navigationPane1.StateChanging += (o, args) =>
            {
                args.Cancel = true;
            };
            if (Settings.KeepSignIn)
            {
                var firebaseController = new FirebaseLoginController(FireBase, this);
                await firebaseController.LoginUser(Settings.User, Settings.PasswordDecrypted);
                return;
            }
            //show up login page
            await ShowLogin();
        }

        private async Task ShowLogin()
        {
            _formLogin = new FormLogin(FireBase, Settings);
            _formLogin.ShowDialog();
            if (!_formLogin.IsLoggedIn)
            {
                SetControls(false, true);
                return;
            }
            Settings.User = _formLogin.Settings.User;
            Settings.PasswordDecrypted = _formLogin.Settings.PasswordDecrypted;
            Settings.KeepSignIn = _formLogin.Settings.KeepSignIn;
            Settings.SaveLocal();

            Config.BackendSettings = (BackendSettings)Settings?.Clone();
            Settings?.SaveLocal();
            if (Settings?.IsGlobalAvailable ?? false)
                Settings?.SaveGlobal();

            FireBase = _formLogin.Fire;

            await SetLoginSuccessfully();
        }

        private async Task SetLoginSuccessfully()
        {
            controlAddFlight1.FireBase = FireBase;
            controlAddFlight1.Sounding = Sounding;
            controlAddFlight1.Temp100FileName = Temp100FileName;
            controlAddFlight1.TempEndFileName = TempEndFileName;


            controlStation1.FireBase = FireBase;
            controlStation1.StationSetting = StationSetting;
            controlAddUserStation1.FireBase = FireBase;
            controlAddUserStation1.Settings = Settings;
            controlAddUserStation1.StationSetting = StationSetting;
            controlLicence1.Protection = Protection;
            _progressHud.Initialize(this, string.Empty);
            await Task.Delay(200);
            await controlStation1.GetStation();
            _progressHud.Stop();
            controlAddFlight1.FireBase = FireBase;
            navigationPane1.SelectedPageIndex = 0;
            if (controlStation1.StationObject != null)
            {
                navigationPage1.Enabled = true;
                controlAddFlight1.StationId = controlStation1.StationObject?.Object?.Id ?? 0;
                SetControls(true);
            }
            else
            {
                SetControls(false);
            }
        }

        private void SetControls(bool status, bool firstPage = false)
        {
            navigationPage2.Enabled = status;
            navigationPage3.Enabled = status;
            navigationPage4.Enabled = status;
            navigationPage5.Enabled = status;
            navigationPage6.Enabled = status;
            if (firstPage)
                navigationPage1.Enabled = status;
        }

        private async void navButton2_ElementClick(object sender, DevExpress.XtraBars.Navigation.NavElementEventArgs e)
        {
            await ShowLogin();
        }

        private void FormBackendNavigation_FormClosing(object sender, FormClosingEventArgs e)
        {
            controlSettings1.Save();
        }

        public void GetError(string errorCode)
        {
            XtraMessageBox.Show(_localization.LoadText(1046) + " " + errorCode, _localization.LoadText(1047), MessageBoxButtons.OK,
                MessageBoxIcon.Stop);
        }

        public async void GetFirebaseResult(FirebaseLib result)
        {
            FireBase = result;
            await SetLoginSuccessfully();
        }
    }
}