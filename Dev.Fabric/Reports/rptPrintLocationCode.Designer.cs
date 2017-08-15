namespace Dev.Fabric.Reports
{
    partial class rptPrintLocationCode
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Barcodes.QRCodeEncoder qrCodeEncoder1 = new Telerik.Reporting.Barcodes.QRCodeEncoder();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            this.detail = new Telerik.Reporting.DetailSection();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.textBox3 = new Telerik.Reporting.TextBox();
            this.barcode1 = new Telerik.Reporting.Barcode();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detail
            // 
            this.detail.CanShrink = false;
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Inch(2D);
            this.detail.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox2,
            this.textBox3,
            this.barcode1,
            this.textBox1});
            this.detail.Name = "detail";
            this.detail.PageBreak = Telerik.Reporting.PageBreak.None;
            // 
            // textBox2
            // 
            this.textBox2.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.1000001430511475D), Telerik.Reporting.Drawing.Unit.Cm(2.1000001430511475D));
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.6000003814697266D), Telerik.Reporting.Drawing.Unit.Cm(0.38833341002464294D));
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(14D);
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            this.textBox2.Value = "= Fields.Warehouse + \" X:\" + Fields.PosX + \", Y:\" + Fields.PosY";
            // 
            // textBox3
            // 
            this.textBox3.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.1000001430511475D), Telerik.Reporting.Drawing.Unit.Cm(1.38937509059906D));
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.6000003814697266D), Telerik.Reporting.Drawing.Unit.Cm(0.38833341002464294D));
            this.textBox3.Style.Font.Bold = true;
            this.textBox3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(14D);
            this.textBox3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox3.Value = "= Fields.CenterIdx";
            // 
            // barcode1
            // 
            this.barcode1.BarAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            qrCodeEncoder1.ECI = Telerik.Reporting.Barcodes.QRCode.ECIMode.ISO8859_3;
            qrCodeEncoder1.ErrorCorrectionLevel = Telerik.Reporting.Barcodes.QRCode.ErrorCorrectionLevel.Q;
            qrCodeEncoder1.FNC1 = Telerik.Reporting.Barcodes.QRCode.FNC1Mode.SecondPosition;
            qrCodeEncoder1.Version = 8;
            this.barcode1.Encoder = qrCodeEncoder1;
            this.barcode1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941D), Telerik.Reporting.Drawing.Unit.Cm(1.0893751382827759D));
            this.barcode1.Module = Telerik.Reporting.Drawing.Unit.Point(4D);
            this.barcode1.Name = "barcode1";
            this.barcode1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.88364577293396D), Telerik.Reporting.Drawing.Unit.Cm(2.6852083206176758D));
            this.barcode1.Stretch = true;
            this.barcode1.Value = "= Fields.Qrcode";
            // 
            // textBox1
            // 
            this.textBox1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(3.1000001430511475D), Telerik.Reporting.Drawing.Unit.Cm(2.8106250762939453D));
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(6.6000003814697266D), Telerik.Reporting.Drawing.Unit.Cm(0.38833341002464294D));
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(14D);
            this.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Top;
            this.textBox1.Value = "= \"Rack# \" + Fields.RackNo + \" - \" + Fields.Floorno + \" - \" + Fields.RackPos";
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.Name = "sqlDataSource1";
            this.sqlDataSource1.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@WorkOrderIdx", System.Data.DbType.String, null)});
            this.sqlDataSource1.SelectCommand = "dbo.up_FabricIn_List3";
            this.sqlDataSource1.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // rptPrintLocationCode
            // 
            this.DataSource = this.sqlDataSource1;
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detail});
            this.Name = "rptPrintQRCode";
            this.PageSettings.ContinuousPaper = false;
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Custom;
            this.PageSettings.PaperSize = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Inch(4D), Telerik.Reporting.Drawing.Unit.Inch(2D));
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1});
            this.Width = Telerik.Reporting.Drawing.Unit.Mm(99D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.SqlDataSource sqlDataSource1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox3;
        private Telerik.Reporting.Barcode barcode1;
        private Telerik.Reporting.TextBox textBox1;
    }
}