using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GrawGoConsole
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        List<string> test = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)

        {
            navigationPage1.Caption = "Station";
            navigationPage2.Caption = "Flights";
            navigationPage3.Caption = "Users";
            
            


        }
        




    }
}
