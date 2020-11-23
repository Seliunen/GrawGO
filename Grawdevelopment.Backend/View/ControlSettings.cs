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
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using GrawDevelopment.Configuration;
using GrawDevelopment.Configuration.ConfigurationObjects.Backend;
using GrawDevelopment.GeneralTypeDefinition.Enum;
using PropertySort = DevExpress.XtraVerticalGrid.PropertySort;

namespace Grawdevelopment.Backend.View
{
    public partial class ControlSettings : DevExpress.XtraEditors.XtraUserControl
    {
        private BackendSettings _backendSettings;

        
        public ControlSettings()
        {
            InitializeComponent();
        }

        private void ControlSettings_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            _backendSettings = new BackendSettings();
            _backendSettings.Load(ConfigurationStorage.LocalAndGlobal);

            propertyGridControl1.SelectedObject = _backendSettings;
            propertyDescriptionControl1.PropertyGrid = propertyGridControl1;
            var generalCheckBox = new RepositoryItemCheckEdit();

            propertyGridControl1.OptionsBehavior.PropertySort = PropertySort.NoSort;
            propertyGridControl1.DefaultEditors.Add(typeof(bool), generalCheckBox);
            var passwordField = new RepositoryItemTextEdit { PasswordChar = '*' };
            propertyGridControl1
                .Rows[$"row{AttributeNamesBackend.Password}"]
                .Properties
                .RowEdit = passwordField;

            propertyGridControl1
                .Rows[$"row{AttributeNamesBackend.TimeSpan}"]
                .Properties
                .RowEdit = GetSpinEdit(10,60,5);

            propertyGridControl1.CustomRecordCellEdit += (o, ev) =>
            {
                Console.WriteLine(ev.Row.Name);
            };


        }

        RepositoryItemSpinEdit GetSpinEdit(int minValue, int maxValue, int increment = 1)
        {
            var spinEdit = new RepositoryItemSpinEdit
            {
                IsFloatValue = false,
                TextEditStyle = TextEditStyles.DisableTextEditor,
                Increment = increment,
                MinValue = minValue,
                MaxValue = maxValue
            };
            return spinEdit;
        }

        public void Save()
        {
            Config.BackendSettings = (BackendSettings) _backendSettings?.Clone();
            _backendSettings?.SaveLocal();
            if (_backendSettings?.IsGlobalAvailable ?? false)
                _backendSettings?.SaveGlobal();
        }
    }
}
