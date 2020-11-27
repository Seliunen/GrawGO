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

namespace GrawGoConsole.View
{
    public partial class ControlGridStation : DevExpress.XtraEditors.XtraUserControl
    {
        RepositoryItemPictureEdit ri;
        private string _url = "";

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
            // gridView1.OptionsView.RowAutoHeight = true;
            gridView1.Columns["Image"].OptionsColumn.FixedWidth = true;
            gridView1.Columns["Image"].Width = 100;
            gridView1.RowHeight = 100;
            //gridView1.CustomRowCellEdit += new DevExpress.XtraGrid.Views.Grid.CustomRowCellEditEventHandler(gridView1_CustomRowCellEdit);

            gridView1.CustomUnboundColumnData += gridView1_CustomUnboundColumnData;

            var dataSource = new StationDatasource();
            await dataSource.GetData();
            gridControl1.DataSource = dataSource.GetDataSource();
        }

        async void gridView1_CustomUnboundColumnData(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDataEventArgs e)
        {
            //throw new NotImplementedException();
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

            // e.Value = await GetImageFromURL(item.ImageUrl);
            //iconsCache.Add(item.ImageUrl, (Bitmap)e.Value);
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

        private void gridView1_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            //var view = sender as GridView;
            //if (view == null) return;
            //if (e.Column.Caption != "FirstName") return;
            //var cellValue = e.Value.ToString() + " " + view.GetRowCellValue(e.RowHandle, view.Columns["LastName"]).ToString();
            //view.SetRowCellValue(e.RowHandle, view.Columns["FullName"], cellValue);
        }

        

        private async void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (e.Column.FieldName == "ImageUrl")
            {
                _url = e.CellValue as string;
               
            }

            //if (e.Column.FieldName == "Image")
            //{
            //    if (string.IsNullOrEmpty(_url))
            //        return;
            //    var pictureEdit = new RepositoryItemImageEdit();
            //    pictureEdit.SizeMode = PictureSizeMode.Stretch;
            //    pictureEdit.NullText = " ";
            //    pictureEdit.Images = await GetImageFromURL(_url);
            //    e.CellValue = pictureEdit;
            //    gridControl1.RepositoryItems.Add(pictureEdit);
            //}
        }

        private async Task<Image> GetImageFromURL(string url)
        {
            var httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(url);
            var httpWebReponse = (HttpWebResponse)await httpWebRequest.GetResponseAsync();
            var stream = httpWebReponse.GetResponseStream();
            return System.Drawing.Image.FromStream(stream);
        }
    }
}
