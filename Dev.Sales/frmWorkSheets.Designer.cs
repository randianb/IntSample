namespace Dev.Sales
{
    partial class frmWorkSheets
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.pgWorkSheet = new Telerik.WinControls.UI.RadPageView();
            this.telerikMetroTouchTheme1 = new Telerik.WinControls.Themes.TelerikMetroTouchTheme();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.radCommandBar1 = new Telerik.WinControls.UI.RadCommandBar();
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElement1 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.btnNewWorksheet = new Telerik.WinControls.UI.CommandBarButton();
            this.btnNewPage = new Telerik.WinControls.UI.CommandBarButton();
            this.btnRemovePage = new Telerik.WinControls.UI.CommandBarButton();
            this.tableLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pgWorkSheet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 1;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.pgWorkSheet, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.radPanel1, 0, 0);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 48F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1524, 772);
            this.tableLayoutPanel4.TabIndex = 4;
            // 
            // pgWorkSheet
            // 
            this.pgWorkSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgWorkSheet.Location = new System.Drawing.Point(3, 51);
            this.pgWorkSheet.Name = "pgWorkSheet";
            this.pgWorkSheet.Size = new System.Drawing.Size(1518, 718);
            this.pgWorkSheet.TabIndex = 4;
            this.pgWorkSheet.Text = "radPageView1";
            this.pgWorkSheet.ThemeName = "TelerikMetroTouch";
            this.pgWorkSheet.ViewMode = Telerik.WinControls.UI.PageViewMode.Backstage;
            this.pgWorkSheet.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.radPageView1_MouseDoubleClick);
            ((Telerik.WinControls.UI.RadPageViewBackstageElement)(this.pgWorkSheet.GetChildAt(0))).ItemAlignment = Telerik.WinControls.UI.StripViewItemAlignment.Near;
            ((Telerik.WinControls.UI.RadPageViewBackstageElement)(this.pgWorkSheet.GetChildAt(0))).StripAlignment = Telerik.WinControls.UI.StripViewAlignment.Left;
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.radCommandBar1);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(3, 3);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(1518, 42);
            this.radPanel1.TabIndex = 0;
            // 
            // radCommandBar1
            // 
            this.radCommandBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radCommandBar1.Location = new System.Drawing.Point(0, 0);
            this.radCommandBar1.Name = "radCommandBar1";
            this.radCommandBar1.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement1});
            this.radCommandBar1.Size = new System.Drawing.Size(1518, 44);
            this.radCommandBar1.TabIndex = 0;
            this.radCommandBar1.Text = "radCommandBar1";
            this.radCommandBar1.ThemeName = "TelerikMetroTouch";
            // 
            // commandBarRowElement1
            // 
            this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement1.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElement1});
            // 
            // commandBarStripElement1
            // 
            this.commandBarStripElement1.DisplayName = "commandBarStripElement1";
            this.commandBarStripElement1.EnableDragging = false;
            this.commandBarStripElement1.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.btnNewWorksheet,
            this.btnNewPage,
            this.btnRemovePage});
            this.commandBarStripElement1.Name = "commandBarStripElement1";
            this.commandBarStripElement1.VisibleInCommandBar = true;
            // 
            // btnNewWorksheet
            // 
            this.btnNewWorksheet.DisplayName = "commandBarButton1";
            this.btnNewWorksheet.DrawText = true;
            this.btnNewWorksheet.Image = global::Dev.Sales.Properties.Resources.BT_copy_20;
            this.btnNewWorksheet.ImageAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnNewWorksheet.Name = "btnNewWorksheet";
            this.btnNewWorksheet.Text = "New Worksheet";
            this.btnNewWorksheet.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNewWorksheet.Click += new System.EventHandler(this.btnNewWorksheet_Click);
            // 
            // btnNewPage
            // 
            this.btnNewPage.DisplayName = "commandBarButton2";
            this.btnNewPage.DrawText = true;
            this.btnNewPage.Image = global::Dev.Sales.Properties.Resources._1480109121_new_241;
            this.btnNewPage.Name = "btnNewPage";
            this.btnNewPage.Text = "New Page";
            this.btnNewPage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNewPage.Click += new System.EventHandler(this.btnNewPage_Click);
            // 
            // btnRemovePage
            // 
            this.btnRemovePage.DisplayName = "commandBarButton3";
            this.btnRemovePage.DrawText = true;
            this.btnRemovePage.Image = global::Dev.Sales.Properties.Resources.Cancel_20;
            this.btnRemovePage.Name = "btnRemovePage";
            this.btnRemovePage.Text = "Remove Page";
            this.btnRemovePage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            // 
            // frmWorkSheets
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1524, 772);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Name = "frmWorkSheets";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Worksheets";
            this.ThemeName = "TelerikMetroTouch";
            this.Load += new System.EventHandler(this.frmPatternRequest_Load);
            this.tableLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pgWorkSheet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private Telerik.WinControls.UI.RadPageView pgWorkSheet;
        private Telerik.WinControls.Themes.TelerikMetroTouchTheme telerikMetroTouchTheme1;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadCommandBar radCommandBar1;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement1;
        private Telerik.WinControls.UI.CommandBarButton btnNewWorksheet;
        private Telerik.WinControls.UI.CommandBarButton btnNewPage;
        private Telerik.WinControls.UI.CommandBarButton btnRemovePage;
    }
}