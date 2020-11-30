using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Grid;
using GrawGoConsole.Controller;
using System.Drawing.Imaging;
using Grawdevelopment.Backend;
using GrawDevelopment.Common.Splash;

namespace GrawGoConsole.View
{
    public partial class ControlGridStation : DevExpress.XtraEditors.XtraUserControl
    {
        Dictionary<String, Bitmap> iconsCache = new Dictionary<string, Bitmap>();
        public ControlGridStation()
        {
            InitializeComponent();
        }

        private async void ControlGrid_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            gridView1.OptionsCustomization.AllowSort = false;
            gridView1.OptionsCustomization.AllowFilter = false;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsView.ShowIndicator = false;
            gridView1.OptionsView.EnableAppearanceEvenRow = true;
            gridView1.Appearance.EvenRow.BackColor = Color.LightGray;
            gridView1.Columns["Image"].OptionsColumn.FixedWidth = true;
            gridView1.Columns["Image"].Width = 100;
            gridView1.RowHeight = 100;

            gridView1.CustomUnboundColumnData += gridView1_CustomUnboundColumnData;
            ProgressHud.Shared.Initialize(this,"");
            var dataSource = new StationDatasource();
            await dataSource.GetData();
            gridControl1.DataSource = dataSource.GetDataSource();
            ProgressHud.Shared.Stop();
        }

        void gridView1_CustomUnboundColumnData(object sender,
            DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            var item = e.Row as StationDataObject;
            if (item == null)
                return;
            if (string.IsNullOrEmpty(item.ImageUrl))
                return;
            if (iconsCache.ContainsKey(item.ImageUrl))
            {
                e.Value = iconsCache[item.ImageUrl];
                return;
            }

            var request = WebRequest.Create(item.ImageUrl);
            using (var response = request.GetResponse())
            {
                using (var stream = response.GetResponseStream())
                {
                    e.Value = Bitmap.FromStream(stream);
                    iconsCache.Add(item.ImageUrl, (Bitmap)e.Value);
                }
            }
        }
    }
}
