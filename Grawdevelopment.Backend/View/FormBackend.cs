using DevExpress.XtraBars.Navigation;
using DevExpress.XtraEditors;
using GrawDevelopment.Configuration.ConfigurationObjects.Backend;
using GrawDevelopment.GeneralTypeDefinition;
using GrawDevelopment.GeneralTypeDefinition.Interface;
using GrawDevelopment.GeneralTypeDefinition.Localization;
using System;

namespace Grawdevelopment.Backend
{
    public partial class FormBackend : DevExpress.XtraEditors.XtraForm
    {
        private static ILocalization _localization = Container<ILocalization>.Value;
        FirebaseLib FireBase;
        public Station StationSetting { get; set; }
        public BackendSettings Settings { get; set; }

        public ISoundingDataCollection Sounding { get; set; }

        public string Temp100FileName { get; set; }
        public string TempEndFileName { get; set; }

        public IProtection Protection { get; set; }

        private int _oldIndex = -1;
        public FormBackend()
        {
            InitializeComponent();
        }

        public FormBackend(BackendSettings settings, ISoundingDataCollection sounding,
            string temp100FileName = "", string tempEndFileName = "") : this()
        {
            Settings = settings;
            Sounding = sounding;
            Temp100FileName = temp100FileName;
            TempEndFileName = tempEndFileName;
            eployeesTileBarItem.Text = _localization.LoadText(1026);
            customersTileBarItem.Text = _localization.LoadText(1020);
            tileBarItem1.Text = _localization.LoadText(1021);
            tileBarItem2.Text = _localization.LoadText(1022);
            tileBarItem3.Text = _localization.LoadText(1023);
            tileBarItem4.Text = _localization.LoadText(1024);
            tileBarItem5.Text = _localization.LoadText(1025);
        }


        private void tileBar_SelectedItemChanged(object sender, TileItemEventArgs e)
        {
            navigationFrame.SelectedPageIndex = tileBarGroupTables.Items.IndexOf(e.Item);
        }

        private void tileBarItem1_ItemClick(object sender, TileItemEventArgs e)
        {

        }

        private void FormBackend_Load(object sender, EventArgs e)
        {
            FireBase = new FirebaseLib(Settings);
            controlLogin1.FireBase = FireBase;
            controlLogin1.Settings = Settings;
            controlStation1.FireBase = FireBase;
            controlStation1.StationSetting = StationSetting;
            controlLogin1.SetLogin += ControlLogin1SetLogin;
            controlAddFlight1.FireBase = FireBase;
            controlAddFlight1.Sounding = Sounding;
            controlAddFlight1.Temp100FileName = Temp100FileName;
            controlAddFlight1.TempEndFileName = TempEndFileName;
            controlAddUserStation1.FireBase = FireBase;
            controlAddUserStation1.Settings = Settings;
            controlAddUserStation1.StationSetting = StationSetting;
            controlLicence1.Protection = Protection;

            navigationFrame.SelectedPageChanged += (o, args) =>
            {
                var frame = args.Page.Parent as NavigationFrame;
                var page = frame.SelectedPage;
                var index = frame.SelectedPageIndex;

                if (_oldIndex == 6)
                {
                    Console.WriteLine("Catched");
                    controlSettings1.Save();
                }

                _oldIndex = index;
            };

            navigationFrame.QueryControl += (o, args) =>
            {
                Console.WriteLine("test");

            };
        }

        private async void ControlLogin1SetLogin(object sender, FirebaseLib e)
        {
            Settings.User = e.UserName;
            Settings.PasswordDecrypted = e.Password;
            Settings.SaveLocal();
            tileBarItem1.Enabled = true;
            tileBarItem3.Enabled = true;
            controlStation1.FireBase = e;
            await controlStation1.GetStation();
            controlAddFlight1.FireBase = e;
            if (controlStation1.StationObject != null)
            {
                tileBarItem2.Enabled = true;
                controlAddFlight1.StationId = controlStation1.StationObject?.Object?.Id ?? 0;
                navigationFrame.SelectedPageIndex = 3;
            }
            else
            {
                navigationFrame.SelectedPageIndex = 2;
            }
        }
    }
}