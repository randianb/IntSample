namespace Dev.Fabric
{
    partial class frmPrintOutbound
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.reportViewer1 = new Telerik.ReportViewer.WinForms.ReportViewer();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.toggleShip = new Telerik.WinControls.UI.RadToggleSwitch();
            this.ddlDept = new Telerik.WinControls.UI.RadDropDownList();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.dtToDate = new Telerik.WinControls.UI.RadDateTimePicker();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.btnSearch = new Telerik.WinControls.UI.RadButton();
            this.dtFromDate = new Telerik.WinControls.UI.RadDateTimePicker();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toggleShip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlDept)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.reportViewer1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.radPanel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1050, 646);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.Location = new System.Drawing.Point(3, 53);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ShowHistoryButtons = false;
            this.reportViewer1.Size = new System.Drawing.Size(1044, 590);
            this.reportViewer1.TabIndex = 5;
            this.reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.btnSearch);
            this.radPanel1.Controls.Add(this.toggleShip);
            this.radPanel1.Controls.Add(this.ddlDept);
            this.radPanel1.Controls.Add(this.radLabel3);
            this.radPanel1.Controls.Add(this.dtToDate);
            this.radPanel1.Controls.Add(this.radLabel2);
            this.radPanel1.Controls.Add(this.radLabel1);
            this.radPanel1.Controls.Add(this.dtFromDate);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(3, 3);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(1044, 44);
            this.radPanel1.TabIndex = 4;
            // 
            // toggleShip
            // 
            this.toggleShip.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.toggleShip.Location = new System.Drawing.Point(678, 10);
            this.toggleShip.Name = "toggleShip";
            this.toggleShip.OffText = "Order Detail";
            this.toggleShip.OnText = "Incl. Shipment";
            this.toggleShip.Size = new System.Drawing.Size(111, 24);
            this.toggleShip.TabIndex = 31;
            this.toggleShip.Text = "radToggleSwitch1";
            this.toggleShip.ThemeName = "ControlDefault";
            this.toggleShip.Value = false;
            this.toggleShip.Visible = false;
            // 
            // ddlDept
            // 
            this.ddlDept.DisplayMember = "custName";
            this.ddlDept.DropDownHeight = 20;
            this.ddlDept.Location = new System.Drawing.Point(84, 13);
            this.ddlDept.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ddlDept.Name = "ddlDept";
            this.ddlDept.Size = new System.Drawing.Size(111, 20);
            this.ddlDept.TabIndex = 30;
            this.ddlDept.Text = "radDropDownList1";
            this.ddlDept.ValueMember = "customerID";
            this.ddlDept.Visible = false;
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(12, 14);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(66, 18);
            this.radLabel3.TabIndex = 4;
            this.radLabel3.Text = "Department";
            this.radLabel3.Visible = false;
            // 
            // dtToDate
            // 
            this.dtToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtToDate.Location = new System.Drawing.Point(420, 12);
            this.dtToDate.Name = "dtToDate";
            this.dtToDate.Size = new System.Drawing.Size(103, 20);
            this.dtToDate.TabIndex = 3;
            this.dtToDate.TabStop = false;
            this.dtToDate.Text = "2016-12-19";
            this.dtToDate.Value = new System.DateTime(2016, 12, 19, 15, 8, 23, 806);
            this.dtToDate.Visible = false;
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(400, 13);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(14, 18);
            this.radLabel2.TabIndex = 2;
            this.radLabel2.Text = "~";
            this.radLabel2.Visible = false;
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(221, 13);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(62, 18);
            this.radLabel1.TabIndex = 2;
            this.radLabel1.Text = "Order Date";
            this.radLabel1.Visible = false;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(12, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(110, 24);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dtFromDate
            // 
            this.dtFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFromDate.Location = new System.Drawing.Point(294, 12);
            this.dtFromDate.Name = "dtFromDate";
            this.dtFromDate.Size = new System.Drawing.Size(103, 20);
            this.dtFromDate.TabIndex = 0;
            this.dtFromDate.TabStop = false;
            this.dtFromDate.Text = "2014-01-01";
            this.dtFromDate.Value = new System.DateTime(2014, 1, 1, 0, 0, 0, 0);
            this.dtFromDate.Visible = false;
            // 
            // rptFabricCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1050, 646);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "rptFabricCode";
            this.Text = "Report Viewer Form";
            this.Load += new System.EventHandler(this.rptFabricCode_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.toggleShip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ddlDept)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSearch)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtFromDate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.ReportViewer.WinForms.ReportViewer reportViewer1;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadToggleSwitch toggleShip;
        private Telerik.WinControls.UI.RadDropDownList ddlDept;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadDateTimePicker dtToDate;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadButton btnSearch;
        private Telerik.WinControls.UI.RadDateTimePicker dtFromDate;
    }
}

