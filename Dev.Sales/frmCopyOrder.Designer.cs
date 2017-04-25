namespace Dev.Sales
{
    partial class frmCopyOrder
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
            this.components = new System.ComponentModel.Container();
            Telerik.WinControls.UI.GridViewTextBoxColumn gridViewTextBoxColumn2 = new Telerik.WinControls.UI.GridViewTextBoxColumn();
            Telerik.WinControls.UI.TableViewDefinition tableViewDefinition2 = new Telerik.WinControls.UI.TableViewDefinition();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.lblFileno = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel4 = new Telerik.WinControls.UI.RadLabel();
            this.lblQty = new Telerik.WinControls.UI.RadLabel();
            this.lblAmount = new Telerik.WinControls.UI.RadLabel();
            this.radLabel5 = new Telerik.WinControls.UI.RadLabel();
            this.lblStatus = new Telerik.WinControls.UI.RadLabel();
            this.radButton1 = new Telerik.WinControls.UI.RadButton();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.btnCopy = new Telerik.WinControls.UI.RadButton();
            this.gvCopy = new Telerik.WinControls.UI.RadGridView();
            //this.dsOrderPlan = new Dev.Sales.dsOrderPlan();
            this.destOrdersToCopyBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFileno)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblQty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnCopy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCopy)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCopy.MasterTemplate)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.dsOrderPlan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.destOrdersToCopyBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radLabel1.Location = new System.Drawing.Point(3, 64);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(337, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "Please Input the file number to copy and click the confirm button. ";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.radPanel1, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.radLabel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.gvCopy, 0, 2);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 4;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 61F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 343F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 57F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(454, 485);
            this.tableLayoutPanel2.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.radLabel2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblFileno, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.radLabel3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.radLabel4, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblQty, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblAmount, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.radLabel5, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblStatus, 3, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(448, 55);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(3, 3);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(53, 18);
            this.radLabel2.TabIndex = 0;
            this.radLabel2.Text = "INT File #";
            // 
            // lblFileno
            // 
            this.lblFileno.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblFileno.Location = new System.Drawing.Point(3, 30);
            this.lblFileno.Name = "lblFileno";
            this.lblFileno.Size = new System.Drawing.Size(59, 18);
            this.lblFileno.TabIndex = 1;
            this.lblFileno.Text = "radLabel2";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(115, 3);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(59, 18);
            this.radLabel3.TabIndex = 0;
            this.radLabel3.Text = "Order Q\'ty";
            // 
            // radLabel4
            // 
            this.radLabel4.Location = new System.Drawing.Point(227, 3);
            this.radLabel4.Name = "radLabel4";
            this.radLabel4.Size = new System.Drawing.Size(79, 18);
            this.radLabel4.TabIndex = 0;
            this.radLabel4.Text = "Order Amount";
            // 
            // lblQty
            // 
            this.lblQty.Location = new System.Drawing.Point(115, 30);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(55, 18);
            this.lblQty.TabIndex = 1;
            this.lblQty.Text = "radLabel2";
            // 
            // lblAmount
            // 
            this.lblAmount.Location = new System.Drawing.Point(227, 30);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(55, 18);
            this.lblAmount.TabIndex = 1;
            this.lblAmount.Text = "radLabel2";
            // 
            // radLabel5
            // 
            this.radLabel5.Location = new System.Drawing.Point(339, 3);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(37, 18);
            this.radLabel5.TabIndex = 0;
            this.radLabel5.Text = "Status";
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(339, 30);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(55, 18);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "radLabel2";
            // 
            // radButton1
            // 
            this.radButton1.Location = new System.Drawing.Point(227, 9);
            this.radButton1.Name = "radButton1";
            this.radButton1.Size = new System.Drawing.Size(107, 24);
            this.radButton1.TabIndex = 14;
            this.radButton1.Text = "Cancel";
            this.radButton1.Click += new System.EventHandler(this.radButton1_Click);
            // 
            // radPanel1
            // 
            this.radPanel1.BackColor = System.Drawing.Color.Transparent;
            this.radPanel1.Controls.Add(this.btnCopy);
            this.radPanel1.Controls.Add(this.radButton1);
            this.radPanel1.Location = new System.Drawing.Point(3, 442);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(448, 50);
            this.radPanel1.TabIndex = 15;
            ((Telerik.WinControls.Primitives.BorderPrimitive)(this.radPanel1.GetChildAt(0).GetChildAt(1))).ForeColor = System.Drawing.Color.Transparent;
            // 
            // btnCopy
            // 
            this.btnCopy.Location = new System.Drawing.Point(102, 9);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(107, 24);
            this.btnCopy.TabIndex = 14;
            this.btnCopy.Text = "Confirm";
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // gvCopy
            // 
            this.gvCopy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.gvCopy.Cursor = System.Windows.Forms.Cursors.Default;
            this.gvCopy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gvCopy.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.gvCopy.ForeColor = System.Drawing.Color.Black;
            this.gvCopy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.gvCopy.Location = new System.Drawing.Point(3, 99);
            // 
            // 
            // 
            gridViewTextBoxColumn2.EnableExpressionEditor = false;
            gridViewTextBoxColumn2.FieldName = "FileNo";
            gridViewTextBoxColumn2.HeaderText = "FileNo";
            gridViewTextBoxColumn2.IsAutoGenerated = true;
            gridViewTextBoxColumn2.Name = "FileNo";
            gridViewTextBoxColumn2.Width = 178;
            this.gvCopy.MasterTemplate.Columns.AddRange(new Telerik.WinControls.UI.GridViewDataColumn[] {
            gridViewTextBoxColumn2});
            this.gvCopy.MasterTemplate.DataSource = this.destOrdersToCopyBindingSource;
            this.gvCopy.MasterTemplate.ViewDefinition = tableViewDefinition2;
            this.gvCopy.Name = "gvCopy";
            this.gvCopy.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.gvCopy.ShowGroupPanel = false;
            this.gvCopy.Size = new System.Drawing.Size(448, 337);
            this.gvCopy.TabIndex = 16;
            this.gvCopy.Text = "radGridView1";
            // 
            // dsOrderPlan
            // 
            //this.dsOrderPlan.DataSetName = "dsOrderPlan";
            //this.dsOrderPlan.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // destOrdersToCopyBindingSource
            // 
            //this.destOrdersToCopyBindingSource.DataMember = "DestOrdersToCopy";
            //this.destOrdersToCopyBindingSource.DataSource = this.dsOrderPlan;
            // 
            // frmCopyOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 485);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Name = "frmCopyOrder";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "Copy Order";
            this.Load += new System.EventHandler(this.frmCopyOrder_Load);
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblFileno)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblQty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lblStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radButton1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnCopy)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCopy.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gvCopy)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this.dsOrderPlan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.destOrdersToCopyBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadButton btnCopy;
        private Telerik.WinControls.UI.RadButton radButton1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel lblFileno;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadLabel radLabel4;
        private Telerik.WinControls.UI.RadLabel lblQty;
        private Telerik.WinControls.UI.RadLabel lblAmount;
        private Telerik.WinControls.UI.RadLabel radLabel5;
        private Telerik.WinControls.UI.RadLabel lblStatus;
        private Telerik.WinControls.UI.RadGridView gvCopy;
        private System.Windows.Forms.BindingSource destOrdersToCopyBindingSource;
        //private dsOrderPlan dsOrderPlan;
    }
}