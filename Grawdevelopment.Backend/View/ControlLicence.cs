using DevExpress.Dialogs.Core.View;
using DevExpress.XtraEditors;
using DevExpress.XtraLayout;
using DevExpress.XtraLayout.Utils;
using GrawDevelopment.GeneralTypeDefinition;
using GrawDevelopment.GeneralTypeDefinition.Interface;
using GrawDevelopment.GeneralTypeDefinition.Localization;
using System;
using System.Windows.Forms;

namespace Grawdevelopment.Backend.View
{
    public partial class ControlLicence : DevExpress.XtraEditors.XtraUserControl
    {
        private static ILocalization _localization = Container<ILocalization>.Value;
        public IProtection Protection { get; set; }
        string _fileName = String.Empty;
        private string _code = string.Empty;

        public ControlLicence()
        {
            InitializeComponent();
        }

        private void ButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            var dialog = new XtraOpenFileDialog();
            dialog.DefaultViewMode = ViewMode.List;
            dialog.Filter = "Key file (*.key)|*.key";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                buttonEdit1.EditValue = dialog.FileName;
                _fileName = dialog.FileName;

            }

        }

        private void ControlLicence_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            layoutControlItem1.Text = _localization.LoadText(1029);

            simpleButton1.Text = _localization.LoadText(1030);
            simpleButton2.Text = _localization.LoadText(1066);

            simpleLabelItem1.Text = _localization.LoadText(1027);
            simpleLabelItem3.Text = _localization.LoadText(0180);
            simpleLabelItem5.Text = _localization.LoadText(1028);

            simpleButton1.Text = _localization.LoadText(1030);

            layoutControlItem3.Visibility = LayoutVisibility.Never;

            if (Protection != null)
            {
                // Protection.RemoveData(1);

                if (Protection.IsRegistred)
                {
                    labelLicenseKey.Text = Protection.GetLicenseKey();
                    LabelDaysLeft.Text =
                        (Protection.GetDaysLeft() > 0) ? Protection.GetDaysLeft().ToString() : _localization.LoadText(1064); ;
                    labelLicenseTo.Text = Protection.GetUserData(0);
                    //if(!Protection.IsExpired)
                }

                layoutControlItem1.Enabled = (Protection.IsExpired || !Protection.IsRegistred);
                layoutControlItem2.Enabled = (Protection.IsExpired || !Protection.IsRegistred);
            }

            simpleButton1.Click += (o, args) =>
            {
                if (string.IsNullOrEmpty(_fileName))
                {
                    XtraMessageBox.Show(_localization.LoadText(1065));
                    return;
                }

                Protection.RemoveData(1);
                //Protection.Initialize(_fileName);
                if (Protection.Initialize(_fileName))
                {
                    if (Protection.IsRegistred)
                    {
                        XtraMessageBox.Show(_localization.LoadText(2600));

                        labelLicenseKey.Text = Protection.GetLicenseKey();
                        LabelDaysLeft.Text = (Protection.GetDaysLeft() > 0)
                            ? Protection.GetDaysLeft().ToString()
                            : _localization.LoadText(1064);
                        labelLicenseTo.Text = Protection.GetUserData(0);
                    }
                }

            };

        }

        private void SimpleButton2_Click(object sender, EventArgs e)
        {
            Protection?.RemoveData(1);
        }

        private void LayoutControlItem1_DoubleClick(object sender, EventArgs e)
        {
            if (sender is LayoutControlItem item)
            {
                var tag = item.Tag;

                if (tag != null)
                {
                    if (_code.Length < 4)
                    {
                        _code = $"{_code}{tag.ToString()}";
                        return;
                    }

                    if (string.Equals("2019", _code))
                    {
                        layoutControlItem3.Visibility = LayoutVisibility.Always;
                        return;
                    }
                    _code = String.Empty;

                }
            }
        }
    }
}
