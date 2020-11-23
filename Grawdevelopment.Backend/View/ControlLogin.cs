using DevExpress.XtraEditors;
using GrawDevelopment.Common.Splash;
using GrawDevelopment.Configuration.ConfigurationObjects.Backend;
using GrawDevelopment.GeneralTypeDefinition;
using GrawDevelopment.GeneralTypeDefinition.Localization;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Grawdevelopment.Backend
{
    public partial class ControlLogin : DevExpress.XtraEditors.XtraUserControl
    {
        private static ILocalization _localization = Container<ILocalization>.Value;

        public FirebaseLib FireBase { get; set; }
        BackendSettings settings;
        private ProgressHud _progressHud = ProgressHud.Shared;
        public BackendSettings Settings
        {
            get => settings;
            set
            {
                settings = value;
                textEdit1.Text = Settings?.User;
                textEdit2.Text = Settings?.PasswordDecrypted;
                checkEdit1.Checked = Settings?.KeepSignIn ?? false;

            }
        }

        public event EventHandler<FirebaseLib> SetLogin;
        public ControlLogin()
        {
            InitializeComponent();
        }

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            await LoginUser();
        }

        private async Task LoginUser()
        {
            if (string.IsNullOrEmpty(textEdit1.Text) || string.IsNullOrEmpty(textEdit2.Text))
            {
                XtraMessageBox.Show(_localization.LoadText(1036), _localization.LoadText(1037), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }

            // SplashScreenManager.ShowDefaultWaitForm("please wait....", "login in progress");
            _progressHud.Initialize(this, _localization.LoadText(1045));
            var result = await FireBase.LoginAsync(textEdit1.Text, textEdit2.Text);
            _progressHud.Stop();


            if (!result)
            {
                XtraMessageBox.Show(_localization.LoadText(1046) + " " + FireBase.ErrorCode, _localization.LoadText(1047), MessageBoxButtons.OK,
                    MessageBoxIcon.Stop);
                return;
            }

            var user = FireBase.GetUser();

            var users = await FireBase.GetUsers();

            var exits = users?.Count(x => x.LocalId == user.LocalId) ?? 0;
            if (exits == 0)
            {
                await FireBase.AddUser(user);
            }


            XtraMessageBox.Show(_localization.LoadText(2537) + " " + _localization.LoadText(1048), _localization.LoadText(3841), MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            OnSetLogin(this, FireBase);
        }

        protected virtual void OnSetLogin(object sender, FirebaseLib e)
        {
            SetLogin?.Invoke(sender, e);
        }

        private async void ControlLogin_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {

                textEdit1.Text = Settings?.User;
                textEdit2.Text = Settings?.PasswordDecrypted;

                textEdit2.EditValue = _localization.LoadText(2511);

                checkEdit1.Checked = Settings?.KeepSignIn ?? false;
                checkEdit1.Properties.Caption = _localization.LoadText(1031);
                simpleButton1.Text = _localization.LoadText(0462);
                textEdit2.EditValue = _localization.LoadText(2511);
                checkEdit1.Properties.Caption = _localization.LoadText(1031);




                checkEdit1.CheckStateChanged += async (o, args) =>
                {
                    if (Settings != null)
                    {
                        Settings.KeepSignIn = checkEdit1.Checked;
                        Settings.SaveLocal();
                        if (Settings?.KeepSignIn ?? false)
                        {
                            await LoginUser();
                        }
                    }
                };

            }
        }
    }
}
