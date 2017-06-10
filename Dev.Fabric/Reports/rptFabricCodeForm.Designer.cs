namespace Dev.Fabric.Reports
{
    partial class rptFabricCodeForm
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector1 = new Telerik.Reporting.Drawing.DescendantSelector();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.DescendantSelector descendantSelector2 = new Telerik.Reporting.Drawing.DescendantSelector();
            this.detailSection1 = new Telerik.Reporting.DetailSection();
            this.textBox27 = new Telerik.Reporting.TextBox();
            this.pageHeaderSection1 = new Telerik.Reporting.PageHeaderSection();
            this.textBox28 = new Telerik.Reporting.TextBox();
            this.pageFooterSection1 = new Telerik.Reporting.PageFooterSection();
            this.sqlDataSource1 = new Telerik.Reporting.SqlDataSource();
            this.textBox29 = new Telerik.Reporting.TextBox();
            this.textBox30 = new Telerik.Reporting.TextBox();
            this.textBox35 = new Telerik.Reporting.TextBox();
            this.textBox36 = new Telerik.Reporting.TextBox();
            this.textBox37 = new Telerik.Reporting.TextBox();
            this.textBox38 = new Telerik.Reporting.TextBox();
            this.textBox39 = new Telerik.Reporting.TextBox();
            this.textBox40 = new Telerik.Reporting.TextBox();
            this.textBox41 = new Telerik.Reporting.TextBox();
            this.textBox46 = new Telerik.Reporting.TextBox();
            this.textBox47 = new Telerik.Reporting.TextBox();
            this.textBox48 = new Telerik.Reporting.TextBox();
            this.textBox49 = new Telerik.Reporting.TextBox();
            this.textBox50 = new Telerik.Reporting.TextBox();
            this.textBox51 = new Telerik.Reporting.TextBox();
            this.textBox52 = new Telerik.Reporting.TextBox();
            this.textBox53 = new Telerik.Reporting.TextBox();
            this.textBox54 = new Telerik.Reporting.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // detailSection1
            // 
            this.detailSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.50020056962966919D);
            this.detailSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox29,
            this.textBox30,
            this.textBox41,
            this.textBox40,
            this.textBox39,
            this.textBox38,
            this.textBox37,
            this.textBox36,
            this.textBox35});
            this.detailSection1.Name = "detailSection1";
            // 
            // textBox27
            // 
            this.textBox27.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.00010002215276472271D));
            this.textBox27.Name = "ReportNameTextBox";
            this.textBox27.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(13.09999942779541D), Telerik.Reporting.Drawing.Unit.Cm(0.800000011920929D));
            this.textBox27.Style.Font.Bold = true;
            this.textBox27.Style.Font.Name = "Segoe UI";
            this.textBox27.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(14D);
            this.textBox27.Value = "Code List";
            // 
            // pageHeaderSection1
            // 
            this.pageHeaderSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(1.4614583253860474D);
            this.pageHeaderSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox27,
            this.textBox53,
            this.textBox46,
            this.textBox47,
            this.textBox48,
            this.textBox49,
            this.textBox50,
            this.textBox51,
            this.textBox52,
            this.textBox54});
            this.pageHeaderSection1.Name = "pageHeaderSection1";
            // 
            // textBox28
            // 
            this.textBox28.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(18.01298713684082D), Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D));
            this.textBox28.Name = "ReportPageNumberTextBox";
            this.textBox28.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.9899253845214844D), Telerik.Reporting.Drawing.Unit.Cm(0.59990006685256958D));
            this.textBox28.Style.Font.Name = "Segoe UI";
            this.textBox28.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox28.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.textBox28.Value = "Page: {PageNumber}";
            // 
            // pageFooterSection1
            // 
            this.pageFooterSection1.Height = Telerik.Reporting.Drawing.Unit.Cm(0.60000020265579224D);
            this.pageFooterSection1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox28});
            this.pageFooterSection1.Name = "pageFooterSection1";
            // 
            // sqlDataSource1
            // 
            this.sqlDataSource1.ConnectionString = "interp.intsample.dbo";
            this.sqlDataSource1.Name = "sqlDataSource1";
            this.sqlDataSource1.Parameters.AddRange(new Telerik.Reporting.SqlDataSourceParameter[] {
            new Telerik.Reporting.SqlDataSourceParameter("@FabricName", System.Data.DbType.String, null),
            new Telerik.Reporting.SqlDataSourceParameter("@IsUse", System.Data.DbType.String, null)});
            this.sqlDataSource1.SelectCommand = "dbo.up_Fabrics_List2";
            this.sqlDataSource1.SelectCommandType = Telerik.Reporting.SqlDataSourceCommandType.StoredProcedure;
            // 
            // textBox29
            // 
            this.textBox29.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.2065502405166626D), Telerik.Reporting.Drawing.Unit.Cm(0.00019984244136139751D));
            this.textBox29.Name = "textBox29";
            this.textBox29.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2999997138977051D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox29.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox29.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox29.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox29.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox29.StyleName = "Office.TableBody";
            this.textBox29.Value = "= Fields.LongName";
            // 
            // textBox30
            // 
            this.textBox30.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox30.Name = "textBox30";
            this.textBox30.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.2062500715255737D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox30.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox30.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox30.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox30.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox30.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox30.StyleName = "Office.TableBody";
            this.textBox30.Value = "= Fields.Idx";
            // 
            // textBox35
            // 
            this.textBox35.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox35.Name = "textBox35";
            this.textBox35.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox35.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox35.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox35.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox35.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox35.StyleName = "Office.TableBody";
            this.textBox35.Value = "= Fields.Percent3";
            // 
            // textBox36
            // 
            this.textBox36.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19.999799728393555D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox36.Name = "textBox36";
            this.textBox36.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox36.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox36.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox36.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox36.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox36.StyleName = "Office.TableBody";
            this.textBox36.Value = "= Fields.Yarn3";
            // 
            // textBox37
            // 
            this.textBox37.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.999597549438477D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox37.Name = "textBox37";
            this.textBox37.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox37.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox37.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox37.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox37.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox37.StyleName = "Office.TableBody";
            this.textBox37.Value = "= Fields.Percent2";
            // 
            // textBox38
            // 
            this.textBox38.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.991426467895508D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox38.Name = "textBox38";
            this.textBox38.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox38.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox38.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox38.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox38.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox38.StyleName = "Office.TableBody";
            this.textBox38.Value = "= Fields.Yarn2";
            // 
            // textBox39
            // 
            this.textBox39.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.991220474243164D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox39.Name = "textBox39";
            this.textBox39.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox39.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox39.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox39.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox39.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox39.StyleName = "Office.TableBody";
            this.textBox39.Value = "= Fields.Percent1";
            // 
            // textBox40
            // 
            this.textBox40.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.991020202636719D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox40.Name = "textBox40";
            this.textBox40.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox40.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox40.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox40.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox40.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox40.StyleName = "Office.TableBody";
            this.textBox40.Value = "= Fields.Yarn1";
            // 
            // textBox41
            // 
            this.textBox41.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.5001993179321289D), Telerik.Reporting.Drawing.Unit.Cm(0.00020024616969749332D));
            this.textBox41.Name = "textBox41";
            this.textBox41.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.4906210899353027D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox41.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox41.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox41.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox41.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox41.StyleName = "Office.TableBody";
            this.textBox41.Value = "= Fields.ShortName";
            // 
            // textBox46
            // 
            this.textBox46.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(22.002910614013672D), Telerik.Reporting.Drawing.Unit.Cm(0.96145838499069214D));
            this.textBox46.Name = "textBox46";
            this.textBox46.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox46.Style.BackgroundColor = System.Drawing.Color.Azure;
            this.textBox46.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox46.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox46.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox46.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox46.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox46.StyleName = "Office.TableBody";
            this.textBox46.Value = "%";
            // 
            // textBox47
            // 
            this.textBox47.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(19.992076873779297D), Telerik.Reporting.Drawing.Unit.Cm(0.96145838499069214D));
            this.textBox47.Name = "textBox47";
            this.textBox47.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox47.Style.BackgroundColor = System.Drawing.Color.Azure;
            this.textBox47.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox47.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox47.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox47.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox47.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox47.StyleName = "Office.TableBody";
            this.textBox47.Value = "Yarn";
            // 
            // textBox48
            // 
            this.textBox48.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(17.981245040893555D), Telerik.Reporting.Drawing.Unit.Cm(0.96145838499069214D));
            this.textBox48.Name = "textBox48";
            this.textBox48.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox48.Style.BackgroundColor = System.Drawing.Color.Azure;
            this.textBox48.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox48.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox48.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox48.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox48.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox48.StyleName = "Office.TableBody";
            this.textBox48.Value = "%";
            // 
            // textBox49
            // 
            this.textBox49.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.970408439636231D), Telerik.Reporting.Drawing.Unit.Cm(0.96145838499069214D));
            this.textBox49.Name = "textBox49";
            this.textBox49.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox49.Style.BackgroundColor = System.Drawing.Color.Azure;
            this.textBox49.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox49.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox49.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox49.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox49.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox49.StyleName = "Office.TableBody";
            this.textBox49.Value = "Yarn";
            // 
            // textBox50
            // 
            this.textBox50.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.986028671264648D), Telerik.Reporting.Drawing.Unit.Cm(0.96145838499069214D));
            this.textBox50.Name = "textBox50";
            this.textBox50.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox50.Style.BackgroundColor = System.Drawing.Color.Azure;
            this.textBox50.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox50.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox50.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox50.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox50.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox50.StyleName = "Office.TableBody";
            this.textBox50.Value = "%";
            // 
            // textBox51
            // 
            this.textBox51.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.975194931030273D), Telerik.Reporting.Drawing.Unit.Cm(0.96145838499069214D));
            this.textBox51.Name = "textBox51";
            this.textBox51.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox51.Style.BackgroundColor = System.Drawing.Color.Azure;
            this.textBox51.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox51.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox51.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox51.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox51.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox51.StyleName = "Office.TableBody";
            this.textBox51.Value = "Yarn";
            // 
            // textBox52
            // 
            this.textBox52.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(6.5001993179321289D), Telerik.Reporting.Drawing.Unit.Cm(0.96145838499069214D));
            this.textBox52.Name = "textBox52";
            this.textBox52.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.4906210899353027D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox52.Style.BackgroundColor = System.Drawing.Color.Azure;
            this.textBox52.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox52.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox52.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox52.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox52.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox52.StyleName = "Office.TableBody";
            this.textBox52.Value = "Short Name";
            // 
            // textBox53
            // 
            this.textBox53.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010012308484874666D), Telerik.Reporting.Drawing.Unit.Cm(0.96145838499069214D));
            this.textBox53.Name = "textBox53";
            this.textBox53.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(1.2062500715255737D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox53.Style.BackgroundColor = System.Drawing.Color.Azure;
            this.textBox53.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox53.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox53.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox53.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox53.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox53.StyleName = "Office.TableBody";
            this.textBox53.Value = "ID";
            // 
            // textBox54
            // 
            this.textBox54.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(1.1999999284744263D), Telerik.Reporting.Drawing.Unit.Cm(0.96145838499069214D));
            this.textBox54.Name = "textBox54";
            this.textBox54.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(5.2999997138977051D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox54.Style.BackgroundColor = System.Drawing.Color.Azure;
            this.textBox54.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox54.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox54.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(7D);
            this.textBox54.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.textBox54.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox54.StyleName = "Office.TableBody";
            this.textBox54.Value = "Long Name";
            // 
            // rptFabricCodeForm
            // 
            this.DataSource = this.sqlDataSource1;
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.detailSection1,
            this.pageHeaderSection1,
            this.pageFooterSection1});
            this.Name = "rptFabricCodeForm";
            this.PageSettings.ContinuousPaper = false;
            this.PageSettings.Landscape = true;
            this.PageSettings.Margins = new Telerik.Reporting.Drawing.MarginsU(Telerik.Reporting.Drawing.Unit.Mm(4D), Telerik.Reporting.Drawing.Unit.Mm(4D), Telerik.Reporting.Drawing.Unit.Mm(5D), Telerik.Reporting.Drawing.Unit.Mm(5D));
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.Letter;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.TextItemBase)),
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.HtmlTextBox))});
            styleRule1.Style.Padding.Left = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule1.Style.Padding.Right = Telerik.Reporting.Drawing.Unit.Point(2D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.Table), "Office.TableNormal")});
            styleRule2.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule2.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule2.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(34)))), ((int)(((byte)(77)))));
            styleRule2.Style.Font.Name = "Calibri";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            descendantSelector1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Office.TableBody")});
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector1});
            styleRule3.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule3.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule3.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule3.Style.Font.Name = "Calibri";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            descendantSelector2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.TypeSelector(typeof(Telerik.Reporting.Table)),
            new Telerik.Reporting.Drawing.StyleSelector(typeof(Telerik.Reporting.ReportItem), "Office.TableHeader")});
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            descendantSelector2});
            styleRule4.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(121)))), ((int)(((byte)(167)))), ((int)(((byte)(227)))));
            styleRule4.Style.BorderColor.Default = System.Drawing.Color.Black;
            styleRule4.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            styleRule4.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            styleRule4.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(34)))), ((int)(((byte)(77)))));
            styleRule4.Style.Font.Name = "Calibri";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(24.002914428710938D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.DetailSection detailSection1;
        private Telerik.Reporting.TextBox textBox27;
        private Telerik.Reporting.PageHeaderSection pageHeaderSection1;
        private Telerik.Reporting.TextBox textBox28;
        private Telerik.Reporting.PageFooterSection pageFooterSection1;
        private Telerik.Reporting.SqlDataSource sqlDataSource1;
        private Telerik.Reporting.TextBox textBox29;
        private Telerik.Reporting.TextBox textBox30;
        private Telerik.Reporting.TextBox textBox41;
        private Telerik.Reporting.TextBox textBox40;
        private Telerik.Reporting.TextBox textBox39;
        private Telerik.Reporting.TextBox textBox38;
        private Telerik.Reporting.TextBox textBox37;
        private Telerik.Reporting.TextBox textBox36;
        private Telerik.Reporting.TextBox textBox35;
        private Telerik.Reporting.TextBox textBox53;
        private Telerik.Reporting.TextBox textBox46;
        private Telerik.Reporting.TextBox textBox47;
        private Telerik.Reporting.TextBox textBox48;
        private Telerik.Reporting.TextBox textBox49;
        private Telerik.Reporting.TextBox textBox50;
        private Telerik.Reporting.TextBox textBox51;
        private Telerik.Reporting.TextBox textBox52;
        private Telerik.Reporting.TextBox textBox54;
    }
}