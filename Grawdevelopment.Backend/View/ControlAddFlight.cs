using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Grawdevelopment.Backend.View;
using GrawDevelopment.GeneralTypeDefinition;
using GrawDevelopment.GeneralTypeDefinition.Interface;
using GrawDevelopment.GeneralTypeDefinition.Localization;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Windows.Forms;

namespace Grawdevelopment.Backend
{
    public partial class ControlAddFlight : DevExpress.XtraEditors.XtraUserControl
    {
        private static ILocalization _localization = Container<ILocalization>.Value;

        public FirebaseLib FireBase { get; set; }
        public ISoundingDataCollection Sounding { get; set; }
        public string Temp100FileName { get; set; }
        public string TempEndFileName { get; set; }

        public int StationId { get; set; }

        public ControlAddFlight()
        {
            InitializeComponent();

        }

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            if (Sounding?.InnerList?.Count == 0)
            {
                XtraMessageBox.Show(_localization.LoadText(1061), _localization.LoadText(1037), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            Sounding.UpdateData(true, true, false, 5);
            var serializedJson = JsonConvert.SerializeObject(Sounding.InnerList);
            string fileName = Path.Combine(Path.GetTempFileName());

            using (StreamWriter myWriter = File.CreateText(fileName))
            {
                myWriter.WriteLine(serializedJson);
            }
            if (File.Exists(fileName))
            {
                SplashScreenManager.ShowForm(this, typeof(ProgressControl), true, true);
                SplashScreenManager.Default.SetWaitFormCaption(_localization.LoadText(3700));
                FireBase.SetProgress += (s, arg) => SplashScreenManager.Default.SetWaitFormDescription(_localization.LoadText(1062) + ": " + arg + "% " + _localization.LoadText(1063));
                await FireBase.AddFlight(fileName, StationId, Sounding.StartTime, Temp100FileName, TempEndFileName);
                SplashScreenManager.CloseForm(false);
            }
        }

        private void ControlAddFlight_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            simpleButton1.Text = _localization.LoadText(1022);
        }
    }
}
