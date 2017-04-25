namespace Dev.Sales
{
    partial class frmShipment
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
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.lblFileno = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.lblQty = new Telerik.WinControls.UI.RadLabel();
            this.lblAmount = new Telerik.WinControls.UI.RadLabel();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.lblStatus = new Telerik.WinControls.UI.RadLabel();
            this.AllowOvership = new Telerik.WinControls.UI.RadCheckBox();
            this.gvShipment = new Telerik.WinControls.UI.RadGridView();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.radCommandBar1 = new Telerik.WinControls.UI.RadCommandBar();
            this.commandBarRowElement1 = new Telerik.WinControls.UI.CommandBarRowElement();
            this.commandBarStripElement1 = new Telerik.WinControls.UI.CommandBarStripElement();
            this.btnRefresh = new Telerik.WinControls.UI.CommandBarButton();
            this.btnInsert = new Telerik.WinControls.UI.CommandBarButton();
            this.btnDelete = new Telerik.WinControls.UI.CommandBarButton();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFileno)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AllowOvership)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvShipment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvShipment.MasterTemplate)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.radLabel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblFileno, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.radLabel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.radLabel3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblQty, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblAmount, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.radLabel4, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.AllowOvership, 3, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(442, 74);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(3, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(53, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "INT File #";
            // 
            // lblFileno
            // 
            this.lblFileno.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblFileno.Location = new System.Drawing.Point(3, 27);
            this.lblFileno.Name = "lblFileno";
            this.lblFileno.Size = new System.Drawing.Size(59, 18);
            this.lblFileno.TabIndex = 1;
            this.lblFileno.Text = "radLabel2";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(113, 3);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(59, 18);
            this.radLabel2.TabIndex = 0;
            this.radLabel2.Text = "Order Q\'ty";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(223, 3);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(79, 18);
            this.radLabel3.TabIndex = 0;
            this.radLabel3.Text = "Order Amount";
            // 
            // lblQty
            // 
            this.lblQty.Location = new System.Drawing.Point(113, 27);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(55, 18);
            this.lblQty.TabIndex = 1;
            this.lblQty.Text = "radLabel2";
            // 
            // lblAmount
            // 
            this.lblAmount.Location = new System.Drawing.Point(223, 27);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(55, 18);
            this.lblAmount.TabIndex = 1;
            this.lblAmount.Text = "radLabel2";
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(333, 3);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(37, 18);
            this.radLabel4.TabIndex = 0;
            this.radLabel4.Text = "Status";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(333, 27);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(55, 18);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "radLabel2";
            // 
            // AllowOvership
            // 
            this.AllowOvership.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.AllowOvership.Location = new System.Drawing.Point(333, 51);
            this.AllowOvership.Name = "AllowOvership";
            this.AllowOvership.Size = new System.Drawing.Size(91, 18);
            this.AllowOvership.TabIndex = 2;
            this.AllowOvership.Text = "Allow Overship";
            // 
            // gvShipment
            // 
            this.gvShipment.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvShipment.Cursor = System.Windows.Forms.Cursors.Default;
            this.gvShipment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvShipment.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.gvShipment.ForeColor = System.Drawing.Color.Black;
            this.gvShipment.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.gvShipment.Location = new System.Drawing.Point(3, 118);
            // 
            // 
            // 
            this.gvShipment.MasterTemplate.AllowAddNewRow = false;
            this.gvShipment.MasterTemplate.ShowRowHeaderColumn = false;
            this.gvShipment.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.gvShipment.Name = "gvShipment";
            this.gvShipment.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gvShipment.Size = new System.Drawing.Size(442, 424);
            this.gvShipment.TabIndex = 1;
            this.gvShipment.Text = "radGridView1";
            this.gvShipment.ViewCellFormatting += new Telerik.WinControls.UI.CellFormattingEventHandler(this.GV1_ViewCellFormatting);
            this.gvShipment.UserAddedRow += new Telerik.WinControls.UI.GridViewRowEventHandler(this.GV1_AddedRow);
            this.gvShipment.CellValueChanged += new Telerik.WinControls.UI.GridViewCellEventHandler(this.GV1_Update);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.radCommandBar1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.gvShipment, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.radButton1, 0, 3);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(448, 575);
            this.tableLayoutPanel2.TabIndex = 2;
            // 
            // radCommandBar1
            // 
            this.radCommandBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.radCommandBar1.Location = new System.Drawing.Point(3, 83);
            this.radCommandBar1.Name = "radCommandBar1";
            this.radCommandBar1.Rows.AddRange(new Telerik.WinControls.UI.CommandBarRowElement[] {
            this.commandBarRowElement1});
            this.radCommandBar1.Size = new System.Drawing.Size(442, 30);
            this.radCommandBar1.TabIndex = 13;
            this.radCommandBar1.Text = "radCommandBar1";
            // 
            // commandBarRowElement1
            // 
            this.commandBarRowElement1.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarRowElement1.MinSize = new System.Drawing.Size(25, 25);
            this.commandBarRowElement1.Strips.AddRange(new Telerik.WinControls.UI.CommandBarStripElement[] {
            this.commandBarStripElement1});
            this.commandBarRowElement1.Text = "";
            this.commandBarRowElement1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // commandBarStripElement1
            // 
            this.commandBarStripElement1.BorderWidth = 0F;
            this.commandBarStripElement1.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.commandBarStripElement1.DisplayName = "commandBarStripElement1";
            // 
            // 
            // 
            this.commandBarStripElement1.Grip.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            this.commandBarStripElement1.Items.AddRange(new Telerik.WinControls.UI.RadCommandBarBaseItem[] {
            this.btnRefresh,
            this.btnInsert,
            this.btnDelete});
            this.commandBarStripElement1.Name = "commandBarStripElement1";
            // 
            // 
            // 
            this.commandBarStripElement1.OverflowButton.Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            this.commandBarStripElement1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            ((Telerik.WinControls.UI.RadCommandBarGrip)(this.commandBarStripElement1.GetChildAt(0))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            ((Telerik.WinControls.UI.RadCommandBarOverflowButton)(this.commandBarStripElement1.GetChildAt(2))).Visibility = Telerik.WinControls.ElementVisibility.Hidden;
            // 
            // btnRefresh
            // 
            this.btnRefresh.AutoSize = true;
            this.btnRefresh.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btnRefresh.DisplayName = "commandBarButton1";
            this.btnRefresh.Image = global::Dev.Sales.Properties.Resources._43_201;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Padding = new System.Windows.Forms.Padding(3);
            this.btnRefresh.Text = "";
            this.btnRefresh.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnInsert
            // 
            this.btnInsert.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btnInsert.DisplayName = "commandBarButton2";
            this.btnInsert.Image = global::Dev.Sales.Properties.Resources._07_plus_201;
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Padding = new System.Windows.Forms.Padding(3);
            this.btnInsert.Text = "";
            this.btnInsert.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btnDelete.DisplayName = "commandBarButton3";
            this.btnDelete.Image = global::Dev.Sales.Properties.Resources._07_minus_201;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Padding = new System.Windows.Forms.Padding(3);
            this.btnDelete.Text = "";
            this.btnDelete.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // radButton1
            // 
            this.radButton1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radButton1.Location = new System.Drawing.Point(3, 548);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(442, 24);
            this.radButton1.TabIndex = 14;
            this.radButton1.Text = "Close Window";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // frmShipment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 575);
            this.Controls.Add(this.tableLayoutPanel2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmShipment";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "frmShipment";
            this.Load += new System.EventHandler(this.frmShipment_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFileno)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AllowOvership)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvShipment.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvShipment)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radCommandBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel lblFileno;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel lblQty;
        private Telerik.WinControls.UI.RadLabel lblAmount;
        private Telerik.WinControls.UI.RadGridView gvShipment;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Telerik.WinControls.UI.RadCommandBar radCommandBar1;
        private Telerik.WinControls.UI.CommandBarRowElement commandBarRowElement1;
        private Telerik.WinControls.UI.CommandBarStripElement commandBarStripElement1;
        private Telerik.WinControls.UI.CommandBarButton btnRefresh;
        private Telerik.WinControls.UI.CommandBarButton btnInsert;
        private Telerik.WinControls.UI.CommandBarButton btnDelete;
        private Telerik.WinControls.UI.RadButton radButton1;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadLabel lblStatus;
        private Telerik.WinControls.UI.RadCheckBox AllowOvership;
    }
}