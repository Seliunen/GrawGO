using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using Grawdevelopment.Backend.Properties;
using GrawDevelopment.Core.Common;
using GrawDevelopment.GeneralTypeDefinition;
using GrawDevelopment.GeneralTypeDefinition.Extension;
using GrawDevelopment.GeneralTypeDefinition.Localization;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grawdevelopment.Backend
{
    public partial class ControlStation : DevExpress.XtraEditors.XtraUserControl
    {
        private static ILocalization _localization = Container<ILocalization>.Value;
        public Firebase.Database.FirebaseObject<Station> StationObject { get; set; }
        public FirebaseLib FireBase { get; set; }
        public Station StationSetting { get; set; }

        private bool _imageChanged;
        public ControlStation()
        {
            InitializeComponent();



        }

        private async void ControlStation_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                checkEdit1.Properties.Caption = _localization.LoadText(1019);
                simpleButton1.Text = _localization.LoadText(2205);
                layoutControlItem1.Text = _localization.LoadText(0478);
                layoutControlItem2.Text = _localization.LoadText(1016);
                layoutControlItem3.Text = _localization.LoadText(1017);
                layoutControlItem4.Text = _localization.LoadText(1015);
                layoutControlItem5.Text = _localization.LoadText(0643);
                layoutControlItem6.Text = _localization.LoadText(0642);
                layoutControlItem7.Text = _localization.LoadText(2127);
                layoutControlItem10.Text = _localization.LoadText(1018);
                layoutControlItem11.Text = _localization.LoadText(1012);

                _imageChanged = false;
                buttonEdit1.Click += (s, args) =>
                {
                    var openFileDialog = new XtraOpenFileDialog();
                    //openFileDialog.Filter = "Image Files(*.PNG; *.BMP; *.JPG; *.GIF)| *.PNG; *.BMP; *.JPG; *.GIF";
                    openFileDialog.Filter = "Image Files(*.png)| *.png";
                    openFileDialog.Multiselect = false;
                    openFileDialog.CheckFileExists = true;
                    openFileDialog.FilterIndex = 1;
                    openFileDialog.RestoreDirectory = true;

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        buttonEdit1.EditValue = openFileDialog.FileName;
                        _imageChanged = true;
                        pictureEdit1.Image = Image.FromFile(openFileDialog.FileName);
                    }
                };

                await GetStation();

                //await UpdateUi();
            }
        }

        private async Task UpdateUi()
        {
            textEdit1.Text = (StationObject != null) ? StationObject.Object.Id.ToString() : StationSetting?.Id.ToString();
            textEdit2.Text = (StationObject != null) ? StationObject.Object.Name : StationSetting?.Name;
            textEdit3.Text = (StationObject != null) ? StationObject.Object.City : "";
            textEdit4.Text = (StationObject != null) ? StationObject.Object.Country : "";
            textEdit5.Text = (StationObject != null) ? StationObject.Object.Latitude : StationSetting?.Latitude;
            textEdit6.Text = (StationObject != null) ? StationObject.Object.Longitude : StationSetting?.Longitude;
            textEdit7.Text = (StationObject != null) ? StationObject.Object.Altitude : StationSetting?.Altitude;
            checkEdit1.Checked = StationObject?.Object.IsPublic ?? false;
            if (string.IsNullOrEmpty(StationObject?.Object.ImageUrl))
            {
                pictureEdit1.Image = Resources.placeholder;
            }
            else
            {
                pictureEdit1.Image = await GetImageFromURL(StationObject?.Object.ImageUrl);
            }
        }

        private async Task<Image> GetImageFromURL(string url)
        {
            var httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            var httpWebReponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
            var stream = httpWebReponse.GetResponseStream();
            return Image.FromStream(stream);
        }

        public async Task GetStation()
        {
            if (StationSetting != null)
            {
                SplashScreenManager.ShowDefaultWaitForm(_localization.LoadText(3700), _localization.LoadText(1055));
                StationObject = await FireBase.GetStation(StationSetting?.Id ?? 0) ;
                SplashScreenManager.CloseDefaultWaitForm();
                await UpdateUi();
            }
        }

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textEdit1.Text) || string.IsNullOrEmpty(textEdit2.Text) ||
                string.IsNullOrEmpty(textEdit3.Text) || string.IsNullOrEmpty(textEdit4.Text) ||
                string.IsNullOrEmpty(textEdit5.Text) || string.IsNullOrEmpty(textEdit6.Text) ||
                string.IsNullOrEmpty(textEdit7.Text))
            {
                XtraMessageBox.Show(_localization.LoadText(1036), _localization.LoadText(1037), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            var station = new Station()
            {
                Id = textEdit1.Text.ToInteger(),
                Name = textEdit2.Text,
                City = textEdit3.Text,
                Country = textEdit4.Text,
                Latitude = textEdit5.Text,
                Longitude = textEdit6.Text,
                Altitude = textEdit7.Text,
                IsPublic = checkEdit1.Checked
            };

            var fileName = $"{station.Id}_{station.Name}.png";

            //if (!string.IsNullOrEmpty(StationObject.Object.ImageName) && !string.Equals(fileName, StationObject.Object.ImageName))
            //{
            //    await FireBase.AddImage(StationObject.Object.ImageName);
            //}

            if (!FormulaLib.IsNumeric(station.Id))
            {

                XtraMessageBox.Show(_localization.LoadText(1060),
                                        _localization.LoadText(3738), MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            else
            {
                if (_imageChanged)
                {
                    pictureEdit1.Image.Save(fileName, ImageFormat.Png);

                    var pictureUrl = await FireBase.AddImage(fileName);
                    File.Delete(fileName);
                    if (!string.IsNullOrEmpty(pictureUrl))
                    {
                        station.ImageName = fileName;
                        station.ImageUrl = pictureUrl;
                    }
                }
                else
                {
                    station.ImageName = StationObject.Object.ImageName;
                    station.ImageUrl = StationObject.Object.ImageUrl;
                }

                if (StationObject != null && StationObject.Object.Id.Equals(station.Id))
                {
                    SplashScreenManager.ShowDefaultWaitForm(_localization.LoadText(3700), _localization.LoadText(1056));
                    var r = await FireBase.ChangeStation(station, StationObject.Key);
                    SplashScreenManager.CloseDefaultWaitForm();
                }
                else
                {
                    SplashScreenManager.ShowDefaultWaitForm(_localization.LoadText(3700), _localization.LoadText(1057));
                    var result = await FireBase.AddStation(station);
                    SplashScreenManager.CloseDefaultWaitForm();
                    if (!result)
                    {
                        XtraMessageBox.Show(_localization.LoadText(1046) + " " + FireBase.ErrorText, _localization.LoadText(1037), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                }
                SplashScreenManager.ShowDefaultWaitForm(_localization.LoadText(3700), _localization.LoadText(1058));
                var s = await FireBase.GetStation(station.Id);
                SplashScreenManager.CloseDefaultWaitForm();
                if (s != null)
                {
                    XtraMessageBox.Show(_localization.LoadText(1561) + " " + s.Object.Name + " " + _localization.LoadText(1059), _localization.LoadText(3841), MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}
