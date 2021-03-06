﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GrawGoConsole.Controller;

namespace GrawGoConsole.View
{
    public partial class ControlGridUser : DevExpress.XtraEditors.XtraUserControl
    {
        public ControlGridUser()
        {
            InitializeComponent();
        }

        private async void ControlGridUser_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;
            gridView1.OptionsCustomization.AllowSort = false;
            gridView1.OptionsCustomization.AllowFilter = false;
            gridView1.OptionsView.ShowGroupPanel = false;
            gridView1.OptionsBehavior.Editable = false;
            gridView1.OptionsView.ShowIndicator = false;
            //gridView1.OptionsView.EnableAppearanceEvenRow = true;
            //gridView1.Appearance.EvenRow.BackColor = Color.DimGray;
            var dataSource = new UserDatasource();
            await dataSource.GetData();
            gridControl1.DataSource = dataSource.GetDataSource();
        }
    }
}
