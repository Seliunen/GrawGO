using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Grawdevelopment.Backend;

namespace GrawGoConsole
{
    public partial class Form1 : DevExpress.XtraEditors.XtraForm
    {
        List<string> test = new List<string>();

        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)

        {
            navigationPage1.Caption = "Station";
            navigationPage2.Caption = "Flights";
            navigationPage3.Caption = "Users";

            var fireBase = new FirebaseLib();
            var result = await fireBase.LoginAsync();
            if (result)
            {
                Console.WriteLine(fireBase.GetUser().Email);
                var stations = await fireBase.GetStations();
                foreach (var item in stations)
                {
                    Console.WriteLine(item.Name);
                }
            }

        }
        



    }
}
