namespace GrawGoConsole
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.navigationPage3 = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.editFormUserControl1 = new DevExpress.XtraGrid.Views.Grid.EditFormUserControl();
            this.navigationPage2 = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.navigationPage1 = new DevExpress.XtraBars.Navigation.NavigationPage();
            this.navigationPane1 = new DevExpress.XtraBars.Navigation.NavigationPane();
            this.controlGrid2 = new GrawGoConsole.View.ControlGrid();
            this.grawGO3 = new GrawGoConsole.GrawGO();
            this.controlGrid1 = new GrawGoConsole.View.ControlGrid();
            this.grawGO1 = new GrawGoConsole.GrawGO();
            this.navigationPage3.SuspendLayout();
            this.navigationPage2.SuspendLayout();
            this.navigationPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.navigationPane1)).BeginInit();
            this.navigationPane1.SuspendLayout();
            this.SuspendLayout();
            // 
            // navigationPage3
            // 
            this.editFormUserControl1.SetBoundPropertyName(this.navigationPage3, "");
            this.navigationPage3.Caption = "navigationPage3";
            this.navigationPage3.Controls.Add(this.controlGrid1);
            this.navigationPage3.Controls.Add(this.grawGO1);
            this.navigationPage3.Controls.Add(this.editFormUserControl1);
            this.navigationPage3.Name = "navigationPage3";
            this.navigationPage3.Size = new System.Drawing.Size(755, 409);
            // 
            // editFormUserControl1
            // 
            this.editFormUserControl1.Location = new System.Drawing.Point(554, 71);
            this.editFormUserControl1.Name = "editFormUserControl1";
            this.editFormUserControl1.Size = new System.Drawing.Size(8, 8);
            this.editFormUserControl1.TabIndex = 0;
            // 
            // navigationPage2
            // 
            this.editFormUserControl1.SetBoundPropertyName(this.navigationPage2, "");
            this.navigationPage2.Caption = "navigationPage2";
            this.navigationPage2.Controls.Add(this.grawGO3);
            this.navigationPage2.Name = "navigationPage2";
            this.navigationPage2.Size = new System.Drawing.Size(755, 409);
            // 
            // navigationPage1
            // 
            this.editFormUserControl1.SetBoundPropertyName(this.navigationPage1, "");
            this.navigationPage1.Caption = "navigationPage1";
            this.navigationPage1.Controls.Add(this.controlGrid2);
            this.navigationPage1.Name = "navigationPage1";
            this.navigationPage1.Size = new System.Drawing.Size(755, 409);
            // 
            // navigationPane1
            // 
            this.navigationPane1.Appearance.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.navigationPane1.Appearance.Options.UseFont = true;
            this.navigationPane1.AppearanceButton.Hovered.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navigationPane1.AppearanceButton.Hovered.Options.UseFont = true;
            this.navigationPane1.AppearanceButton.Normal.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navigationPane1.AppearanceButton.Normal.Options.UseFont = true;
            this.navigationPane1.AppearanceButton.Pressed.Font = new System.Drawing.Font("Segoe UI Black", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navigationPane1.AppearanceButton.Pressed.Options.UseFont = true;
            this.editFormUserControl1.SetBoundPropertyName(this.navigationPane1, "");
            this.navigationPane1.Controls.Add(this.navigationPage1);
            this.navigationPane1.Controls.Add(this.navigationPage2);
            this.navigationPane1.Controls.Add(this.navigationPage3);
            this.navigationPane1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigationPane1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.navigationPane1.Location = new System.Drawing.Point(0, 0);
            this.navigationPane1.Name = "navigationPane1";
            this.navigationPane1.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.navigationPage1,
            this.navigationPage2,
            this.navigationPage3});
            this.navigationPane1.RegularSize = new System.Drawing.Size(934, 482);
            this.navigationPane1.SelectedPage = this.navigationPage1;
            this.navigationPane1.Size = new System.Drawing.Size(934, 482);
            this.navigationPane1.TabIndex = 0;
            this.navigationPane1.Text = "navigationPane1";
            this.navigationPane1.TransitionType = DevExpress.Utils.Animation.Transitions.Fade;
            // 
            // controlGrid2
            // 
            this.editFormUserControl1.SetBoundPropertyName(this.controlGrid2, "");
            this.controlGrid2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlGrid2.Location = new System.Drawing.Point(0, 0);
            this.controlGrid2.Name = "controlGrid2";
            this.controlGrid2.Size = new System.Drawing.Size(755, 409);
            this.controlGrid2.TabIndex = 0;
            // 
            // grawGO3
            // 
            this.editFormUserControl1.SetBoundPropertyName(this.grawGO3, "");
            this.grawGO3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grawGO3.Location = new System.Drawing.Point(0, 0);
            this.grawGO3.Name = "grawGO3";
            this.grawGO3.Size = new System.Drawing.Size(755, 409);
            this.grawGO3.TabIndex = 0;
            // 
            // controlGrid1
            // 
            this.editFormUserControl1.SetBoundPropertyName(this.controlGrid1, "");
            this.controlGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.controlGrid1.Location = new System.Drawing.Point(0, 0);
            this.controlGrid1.Name = "controlGrid1";
            this.controlGrid1.Size = new System.Drawing.Size(755, 409);
            this.controlGrid1.TabIndex = 2;
            // 
            // grawGO1
            // 
            this.editFormUserControl1.SetBoundPropertyName(this.grawGO1, "");
            this.grawGO1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grawGO1.Location = new System.Drawing.Point(0, 0);
            this.grawGO1.Name = "grawGO1";
            this.grawGO1.Size = new System.Drawing.Size(755, 409);
            this.grawGO1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.editFormUserControl1.SetBoundPropertyName(this, "");
            this.ClientSize = new System.Drawing.Size(934, 482);
            this.Controls.Add(this.navigationPane1);
            this.IconOptions.ShowIcon = false;
            this.Name = "Form1";
            this.Text = "GrawGo";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.navigationPage3.ResumeLayout(false);
            this.navigationPage2.ResumeLayout(false);
            this.navigationPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.navigationPane1)).EndInit();
            this.navigationPane1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.NavigationPage navigationPage3;
        private DevExpress.XtraGrid.Views.Grid.EditFormUserControl editFormUserControl1;
        private GrawGO grawGO1;
        private DevExpress.XtraBars.Navigation.NavigationPage navigationPage2;
        private GrawGO grawGO3;
        private DevExpress.XtraBars.Navigation.NavigationPage navigationPage1;
        private DevExpress.XtraBars.Navigation.NavigationPane navigationPane1;
        private View.ControlGrid controlGrid1;
        private View.ControlGrid controlGrid2;
    }
}

