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

namespace Grawdevelopment.Backend.View
{
    public partial class FormRegister : DevExpress.XtraEditors.XtraForm
    {
        public (string UserName, string Password) Credential { get; set; }
        public FormRegister()
        {
            InitializeComponent();
        }

        private void FormRegister_Load(object sender, EventArgs e)
        {
            controlSignup1.SetCredentials += (o, tuple) =>
            {
                Credential = tuple;
                DialogResult = DialogResult.OK;
                Close();
            };
        }
    }
}