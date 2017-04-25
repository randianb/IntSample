namespace Dev.Options
{
    public partial class InheritMDI
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
        public void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InheritMDI));
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.lblServerInfo = new Telerik.WinControls.UI.RadLabelElement();
            this.commandBarSeparator1 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.lblRows = new Telerik.WinControls.UI.RadLabelElement();
            this.commandBarSeparator2 = new Telerik.WinControls.UI.CommandBarSeparator();
            this.lblDescription = new Telerik.WinControls.UI.RadLabelElement();
            this.radRibbonBar1 = new Telerik.WinControls.UI.RadRibbonBar();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.lblServerInfo,
            this.commandBarSeparator1,
            this.lblRows,
            this.commandBarSeparator2,
            this.lblDescription});
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 269);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(292, 26);
            this.radStatusStrip1.TabIndex = 1;
            this.radStatusStrip1.Text = "radStatusStrip1";
            // 
            // lblServerInfo
            // 
            this.lblServerInfo.Name = "lblServerInfo";
            this.radStatusStrip1.SetSpring(this.lblServerInfo, false);
            this.lblServerInfo.Text = "Server";
            this.lblServerInfo.TextWrap = true;
            // 
            // commandBarSeparator1
            // 
            this.commandBarSeparator1.Name = "commandBarSeparator1";
            this.radStatusStrip1.SetSpring(this.commandBarSeparator1, false);
            this.commandBarSeparator1.VisibleInOverflowMenu = false;
            // 
            // lblRows
            // 
            this.lblRows.Name = "lblRows";
            this.radStatusStrip1.SetSpring(this.lblRows, false);
            this.lblRows.Text = "Rows";
            this.lblRows.TextWrap = true;
            // 
            // commandBarSeparator2
            // 
            this.commandBarSeparator2.Name = "commandBarSeparator2";
            this.radStatusStrip1.SetSpring(this.commandBarSeparator2, false);
            this.commandBarSeparator2.VisibleInOverflowMenu = false;
            // 
            // lblDescription
            // 
            this.lblDescription.Name = "lblDescription";
            this.radStatusStrip1.SetSpring(this.lblDescription, false);
            this.lblDescription.Text = "None";
            this.lblDescription.TextWrap = true;
            // 
            // radRibbonBar1
            // 
            this.radRibbonBar1.ApplicationMenuStyle = Telerik.WinControls.UI.ApplicationMenuStyle.BackstageView;
            // 
            // 
            // 
            this.radRibbonBar1.ExitButton.Text = "Exit";
            this.radRibbonBar1.Location = new System.Drawing.Point(0, 0);
            this.radRibbonBar1.Name = "radRibbonBar1";
            // 
            // 
            // 
            this.radRibbonBar1.OptionsButton.Text = "Options";
            this.radRibbonBar1.Size = new System.Drawing.Size(292, 148);
            this.radRibbonBar1.TabIndex = 2;
            this.radRibbonBar1.Text = "InheritMDI";
            this.radRibbonBar1.Visible = false;
            // 
            // InheritMDI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 295);
            this.Controls.Add(this.radStatusStrip1);
            this.Controls.Add(this.radRibbonBar1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = null;
            this.Name = "InheritMDI";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "InheritMDI";
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radRibbonBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        public Telerik.WinControls.UI.RadLabelElement lblServerInfo;
        public Telerik.WinControls.UI.RadLabelElement lblRows;
        public Telerik.WinControls.UI.RadLabelElement lblDescription;
        public Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator1;
        public Telerik.WinControls.UI.CommandBarSeparator commandBarSeparator2;
        public Telerik.WinControls.UI.RadRibbonBar radRibbonBar1;
    }
}