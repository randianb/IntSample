namespace Dev.Sales
{
    partial class frmScheduler
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
            Telerik.WinControls.UI.SchedulerDailyPrintStyle schedulerDailyPrintStyle1 = new Telerik.WinControls.UI.SchedulerDailyPrintStyle();
            Telerik.WinControls.UI.AppointmentMappingInfo appointmentMappingInfo1 = new Telerik.WinControls.UI.AppointmentMappingInfo();
            Telerik.WinControls.UI.ResourceMappingInfo resourceMappingInfo1 = new Telerik.WinControls.UI.ResourceMappingInfo();
            this.scNavigator = new Telerik.WinControls.UI.RadSchedulerNavigator();
            this.scDelivery = new Telerik.WinControls.UI.RadScheduler();
            this.schedulerBindingDataSource1 = new Telerik.WinControls.UI.SchedulerBindingDataSource();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.telerikMetroTouchTheme1 = new Telerik.WinControls.Themes.TelerikMetroTouchTheme();
            ((System.ComponentModel.ISupportInitialize)(this.scNavigator)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scDelivery)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerBindingDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerBindingDataSource1.EventProvider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerBindingDataSource1.ResourceProvider)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // scNavigator
            // 
            this.scNavigator.AssociatedScheduler = this.scDelivery;
            this.scNavigator.DateFormat = "MMMM";
            this.scNavigator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scNavigator.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.scNavigator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.scNavigator.Location = new System.Drawing.Point(3, 3);
            this.scNavigator.Name = "scNavigator";
            this.scNavigator.NavigationStep = 30;
            this.scNavigator.NavigationStepType = Telerik.WinControls.UI.NavigationStepTypes.Month;
            // 
            // 
            // 
            this.scNavigator.RootElement.StretchVertically = false;
            this.scNavigator.Size = new System.Drawing.Size(1278, 89);
            this.scNavigator.TabIndex = 1;
            this.scNavigator.Text = "radSchedulerNavigator1";
            ((Telerik.WinControls.UI.RadToggleButtonElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(0))).Font = new System.Drawing.Font("Segoe UI", 9F);
            ((Telerik.WinControls.UI.RadToggleButtonElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(0))).AutoSize = true;
            ((Telerik.WinControls.UI.LightVisualElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(4))).Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.RadItem)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1))).Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.RadItem)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1))).AutoSize = true;
            ((Telerik.WinControls.RadItem)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1))).SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(1))).AutoSize = true;
            ((Telerik.WinControls.Layouts.DockLayoutPanel)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2))).AutoSize = true;
            ((Telerik.WinControls.Layouts.DockLayoutPanel)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            ((Telerik.WinControls.UI.RadButtonElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(0))).AutoSize = false;
            ((Telerik.WinControls.UI.RadButtonElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(0))).Margin = new System.Windows.Forms.Padding(6, 4, 4, 11);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(0).GetChildAt(0))).AutoSize = true;
            ((Telerik.WinControls.UI.RadButtonElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(1))).ImageAlignment = System.Drawing.ContentAlignment.MiddleRight;
            ((Telerik.WinControls.UI.RadButtonElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(1))).AutoSize = false;
            ((Telerik.WinControls.UI.RadButtonElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(1))).Padding = new System.Windows.Forms.Padding(5, 4, 5, 5);
            ((Telerik.WinControls.UI.RadButtonElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(1))).Margin = new System.Windows.Forms.Padding(10, 4, 10, 11);
            ((Telerik.WinControls.UI.RadLabelElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(2))).TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            ((Telerik.WinControls.UI.RadLabelElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(2))).TextImageRelation = System.Windows.Forms.TextImageRelation.Overlay;
            ((Telerik.WinControls.UI.RadLabelElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(2))).Text = "3월 - 4월";
            ((Telerik.WinControls.UI.RadLabelElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(2))).Font = new System.Drawing.Font("Malgun Gothic", 20F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.UI.RadLabelElement)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(2))).Margin = new System.Windows.Forms.Padding(30, 1, 0, 1);
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.scNavigator.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(2).GetChildAt(2).GetChildAt(1))).TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // scDelivery
            // 
            this.scDelivery.ActiveViewType = Telerik.WinControls.UI.SchedulerViewType.Month;
            this.scDelivery.Culture = new System.Globalization.CultureInfo("");
            this.scDelivery.DataSource = this.schedulerBindingDataSource1;
            this.scDelivery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scDelivery.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.scDelivery.Location = new System.Drawing.Point(3, 98);
            this.scDelivery.Name = "scDelivery";
            schedulerDailyPrintStyle1.AppointmentFont = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            schedulerDailyPrintStyle1.DateEndRange = new System.DateTime(2017, 3, 21, 0, 0, 0, 0);
            schedulerDailyPrintStyle1.DateHeadingFont = new System.Drawing.Font("Gulim", 9F, System.Drawing.FontStyle.Bold);
            schedulerDailyPrintStyle1.DateStartRange = new System.DateTime(2017, 3, 16, 0, 0, 0, 0);
            schedulerDailyPrintStyle1.PageHeadingFont = new System.Drawing.Font("Gulim", 22F, System.Drawing.FontStyle.Bold);
            this.scDelivery.PrintStyle = schedulerDailyPrintStyle1;
            this.scDelivery.Size = new System.Drawing.Size(1278, 677);
            this.scDelivery.TabIndex = 0;
            this.scDelivery.Text = "radScheduler1";
            this.scDelivery.ThemeName = "TelerikMetroTouch";
            this.scDelivery.CellFormatting += new System.EventHandler<Telerik.WinControls.UI.SchedulerCellEventArgs>(this.radScheduler1_CellFormatting);
            this.scDelivery.AppointmentEditDialogShowing += new System.EventHandler<Telerik.WinControls.UI.AppointmentEditDialogShowingEventArgs>(this.scDelivery_AppointmentEditDialogShowing);
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(0))).Text = "Sunday";
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(0))).ToolTipText = "Sunday";
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(0))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            ((Telerik.WinControls.UI.MonthViewVerticalHeader)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1))).Font = new System.Drawing.Font("Segoe UI", 12F);
            ((Telerik.WinControls.UI.SchedulerCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(0))).Font = new System.Drawing.Font("Segoe UI", 12F);
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(1))).GradientAngle = 0F;
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(1))).TextOrientation = System.Windows.Forms.Orientation.Vertical;
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(1))).Text = "Mar 26 - 01";
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(1))).ToolTipText = "Mar 26 - 01";
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(1))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(1))).Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(2))).GradientAngle = 0F;
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(2))).TextOrientation = System.Windows.Forms.Orientation.Vertical;
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(2))).Text = "Apr 02 - 08";
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(2))).ToolTipText = "Apr 02 - 08";
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(2))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(2))).Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(3))).GradientAngle = 0F;
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(3))).TextOrientation = System.Windows.Forms.Orientation.Vertical;
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(3))).Text = "Apr 09 - 15";
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(3))).ToolTipText = "Apr 09 - 15";
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(3))).ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            ((Telerik.WinControls.UI.SchedulerHeaderCellElement)(this.scDelivery.GetChildAt(0).GetChildAt(0).GetChildAt(1).GetChildAt(3))).Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            // 
            // schedulerBindingDataSource1
            // 
            // 
            // 
            // 
            appointmentMappingInfo1.BackgroundId = "BackgroundId";
            appointmentMappingInfo1.Description = "Description";
            appointmentMappingInfo1.End = "End";
            appointmentMappingInfo1.MasterEventId = "MasterEventId";
            appointmentMappingInfo1.RecurrenceRule = "RecurrenceRule";
            appointmentMappingInfo1.ResourceId = "ResourceID";
            appointmentMappingInfo1.Start = "Start";
            appointmentMappingInfo1.Summary = "Summary";
            this.schedulerBindingDataSource1.EventProvider.Mapping = appointmentMappingInfo1;
            // 
            // 
            // 
            resourceMappingInfo1.Id = "ID";
            resourceMappingInfo1.Image = "Image";
            resourceMappingInfo1.Name = "Name";
            this.schedulerBindingDataSource1.ResourceProvider.Mapping = resourceMappingInfo1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.scNavigator, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.scDelivery, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1284, 763);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // frmScheduler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1284, 763);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "frmScheduler";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.Text = "frmScheduler";
            this.Load += new System.EventHandler(this.frmScheduler_Load);
            ((System.ComponentModel.ISupportInitialize)(this.scNavigator)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scDelivery)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerBindingDataSource1.EventProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerBindingDataSource1.ResourceProvider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.schedulerBindingDataSource1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadScheduler scDelivery;
        private Telerik.WinControls.UI.RadSchedulerNavigator scNavigator;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private Telerik.WinControls.UI.SchedulerBindingDataSource schedulerBindingDataSource1;
        private Telerik.WinControls.Themes.TelerikMetroTouchTheme telerikMetroTouchTheme1;
    }
}