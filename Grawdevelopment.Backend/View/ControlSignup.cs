using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using GrawDevelopment.GeneralTypeDefinition;
using GrawDevelopment.GeneralTypeDefinition.Localization;
using System;
using System.Windows.Forms;

namespace Grawdevelopment.Backend
{
    public partial class ControlSignup : DevExpress.XtraEditors.XtraUserControl
    {
        private static ILocalization _localization = Container<ILocalization>.Value;

        public event EventHandler<bool> SetSignUpStatus;
        public event EventHandler<(string userName, string password)> SetCredentials;
       



        public (string userName, string password) Credential { get; set; }

        public ControlSignup()
        {
            InitializeComponent();


        }

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            FirebaseLib firebase = new FirebaseLib();
            SplashScreenManager.ShowDefaultWaitForm(_localization.LoadText(3700), _localization.LoadText(1049));

            try
            {
                if (textPassword.Text.Length < 6)
                {
                    XtraMessageBox.Show(_localization.LoadText(1050), _localization.LoadText(1037), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (!textPassword.Text.Equals(textPasswordRepeated.Text))
                {
                    XtraMessageBox.Show(_localization.LoadText(2505), _localization.LoadText(1037), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                if (string.IsNullOrEmpty(textName.Text) || string.IsNullOrEmpty(textEmail.Text))
                {
                    XtraMessageBox.Show(_localization.LoadText(1036), _localization.LoadText(1037), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                var result = await firebase.SignIn(textEmail.Text, textPassword.Text, textName.Text);
                if (!result)
                {
                    var errorCode = firebase.ErrorCode;
                    XtraMessageBox.Show(_localization.LoadText(1046) + " " + errorCode, _localization.LoadText(1052), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                else
                {
                    XtraMessageBox.Show(_localization.LoadText(2537) + " " + textName.Text + " " + _localization.LoadText(1053), _localization.LoadText(1054), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Credential = (textEmail.Text, textPassword.Text);
                    OnSetCredentials((textEmail.Text, textPassword.Text));
                    OnSetSignUpStatus(this, true);
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                SplashScreenManager.CloseDefaultWaitForm();
            }

        }
        protected virtual void OnSetSignUpStatus(object sender, bool e)
        {
            SetSignUpStatus?.Invoke(sender, e);
        }

        private void ControlSignup_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            simpleButton1.Text = _localization.LoadText(1020);
            layoutControlItem1.Text = _localization.LoadText(1032);
            layoutControlItem2.Text = _localization.LoadText(0449);
            layoutControlItem3.Text = _localization.LoadText(2511);
            layoutControlItem6.Text = _localization.LoadText(0452);

        }

        protected virtual void OnSetCredentials((string userName, string password) e)
        {
            SetCredentials?.Invoke(this, e);
        }
    }
}
