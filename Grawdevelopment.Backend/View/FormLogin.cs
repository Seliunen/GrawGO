using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraTreeList.Painter;
using GrawDevelopment.Configuration.ConfigurationObjects.Backend;
using Grawdevelopment.Backend.Controller;
using GrawDevelopment.Common.Splash;
using GrawDevelopment.Configuration.Languages;
using GrawDevelopment.GeneralTypeDefinition.Localization;
using GrawDevelopment.GeneralTypeDefinition;

namespace Grawdevelopment.Backend.View
{
    public partial class FormLogin : DevExpress.XtraEditors.XtraForm,ILoginResults
    {
        private static ILocalization _localization = Container<ILocalization>.Value;
        public FirebaseLib Fire { get; set; }
        BackendSettings _settings;
        private ProgressHud _progressHud = ProgressHud.Shared;
        public bool IsLoggedIn { get; set; }
        public BackendSettings Settings
        {
            get => _settings;
            set
            {
                _settings = value;
                textEdit1.Text = Settings?.User;
                textEdit2.Text = Settings?.PasswordDecrypted;
                checkEdit1.Checked = Settings?.KeepSignIn ?? false;

            }
        }
        Action<FirebaseLib> _resultAction;
        private FirebaseLoginController _firebaseLoginController;

        public FormLogin()
        {
            InitializeComponent();
        }

        public FormLogin(FirebaseLib fireBase, BackendSettings settings):this()
        {
            Fire = fireBase;
            Settings = settings;
            IsLoggedIn = false;
            //_resultAction = action;
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            textEdit2.EditValue = Settings?.PasswordDecrypted ?? "";
            checkEdit1.Checked = Settings?.KeepSignIn ?? false;
            checkEdit1.Properties.Caption = _localization?.LoadText(1031);
            simpleButton1.Text = _localization?.LoadText(0462);
            linkLabel1.Text = _localization?.LoadText(2607);
            _firebaseLoginController =new FirebaseLoginController(Fire,this);
        }

        

        private async void simpleButton1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textEdit1.Text) || string.IsNullOrEmpty(textEdit2.Text))
            {
                XtraMessageBox.Show(_localization.LoadText(1036), _localization.LoadText(1037), MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return;
            }
            //_progressHud.Initialize(this,string.Empty);//, _localization.LoadText(1045));
            await _firebaseLoginController.LoginUser(textEdit1.Text, textEdit2.Text);
            

        }

        public void GetError(string errorCode)
        {
            XtraMessageBox.Show(_localization.LoadText(1046) + " " + errorCode, _localization.LoadText(1047), MessageBoxButtons.OK,
                MessageBoxIcon.Stop);
            //_progressHud.Stop();
        }

        public void GetFirebaseResult(FirebaseLib result)
        {
            Settings.User = result.UserName;
            Settings.PasswordDecrypted = result.Password;
            Settings.KeepSignIn = checkEdit1.Checked;
            Fire = result;
            IsLoggedIn = true;
            //_progressHud.Stop();
            Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var f = new FormRegister();
            var result = f.ShowDialog();
            if (result != DialogResult.Cancel)
            {
                var credential = f.Credential;
                textEdit1.Text = credential.UserName;
                textEdit2.Text = credential.Password;
            }
        }
    }
}