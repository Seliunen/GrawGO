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
using Grawdevelopment.Backend;
using GrawGoConsole.Controller;

namespace GrawGoConsole.View
{
    public partial class ControlFlights : DevExpress.XtraEditors.XtraUserControl
    {
        public ControlFlights()
        {
            InitializeComponent();

            Load += async (sender, args) =>
            {
                if (DesignMode)
                    return;
                gridView1.OptionsCustomization.AllowSort = false;
                gridView1.OptionsCustomization.AllowFilter = false;
                gridView1.OptionsView.ShowGroupPanel = false;
                gridView1.OptionsBehavior.Editable = false;
                gridView1.OptionsView.ShowIndicator = false;

                var dataSource = new StationDatasource();
                await dataSource.GetData();
                
                lookUpEdit1.Properties.DisplayMember = "Name";
                lookUpEdit1.Properties.ValueMember = "Id";
                lookUpEdit1.Properties.DataSource = dataSource.GetDataSource();
                lookUpEdit1.Properties.PopulateColumns();
                for (var i = 0; i < lookUpEdit1.Properties.Columns.Count; i++)
                {
                    if (lookUpEdit1.Properties.Columns[i].FieldName != "Name" &&
                        lookUpEdit1.Properties.Columns[i].FieldName != "Id")
                    {
                        lookUpEdit1.Properties.Columns[i].Visible = false;
                    }
                }
                lookUpEdit1.ItemIndex = 0;


                lookUpEdit1.EditValueChanged += (o, eventArgs) =>
                {
                    if (!(o is LookUpEdit item))
                        return;
                    var currentList = (List<Station>)item.Properties.DataSource;
                    var stationItem = currentList[item.ItemIndex];

                    //Flüge laden!!!


                };
                //gridControl1.DataSource = dataSource.GetDataSource();

            };
        }
    }
}
