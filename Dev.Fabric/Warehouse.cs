using Int.Code;
using Dev.Options;
using System;
using System.IO;
using System.Data;
using System.Drawing;
using System.Collections.Generic;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Windows.Forms;
using Int.Customer;
using Int.Department;
using Int.Costcenter;
using System.Globalization;

namespace Dev.Fabric 
{
    /// <summary>
    /// 원단창고 원단 적재위치
    /// </summary>
    public partial class Warehouse : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                     // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, string> _searchString;  // 쿼리값 value, 쿼리항목 key로 전달 
        public Dictionary<CommonValues.KeyName, int> _searchKey;        // 쿼리값 value, 쿼리항목 key로 전달 
        private bool _bRtn;                                             // 쿼리결과 리턴
        private DataSet _ds1 = null;                                    // 기본 데이터셋
        private DataTable _dt = null;                                   // 기본 데이터테이블
        private Controller.Inbound _obj1 = null;                        // 현재 생성된 객체 
        private RadContextMenu contextMenu;                             // 컨텍스트 메뉴
        private List<CodeContents> lstStatus = new List<CodeContents>();        // 
        private List<CodeContents> lstStatus2 = new List<CodeContents>();        // 
        private List<CodeContents> lstOutStatus = new List<CodeContents>();        // 
        private List<CodeContents> lstOutStatus2 = new List<CodeContents>();        // 

        private List<CustomerName> lstBuyer = new List<CustomerName>();        // 
        private List<CustomerName> lstBuyer2 = new List<CustomerName>();        // 
        private List<CodeContents> lstColor = new List<CodeContents>();        // 
        private List<CodeContents> lstColor2 = new List<CodeContents>();        // 
        private List<Controller.Fabric> lstFabric = new List<Controller.Fabric>();        // 
        private List<Controller.Fabric> lstFabric2 = new List<Controller.Fabric>();        // 
        private List<CodeContents> lstFabricType = new List<CodeContents>();        // 
        private List<CodeContents> lstFabricType2 = new List<CodeContents>();        // 
        private List<CodeContents> lstRack1 = new List<CodeContents>();       // 
        private List<CodeContents> lstRack2 = new List<CodeContents>();        // 
        private List<CodeContents> lstRack3 = new List<CodeContents>();        // 
        private List<CodeContents> lstRack21 = new List<CodeContents>();       // 
        private List<CodeContents> lstRack22 = new List<CodeContents>();        // 
        private List<CodeContents> lstRack23 = new List<CodeContents>();        //
        private List<CodeContents> codeName = new List<CodeContents>();         // 코드
        private List<CostcenterName> lstCenter = new List<CostcenterName>();     // 
        private List<CustomerName> lstDept = new List<CustomerName>();     // 
        private List<CostcenterName> lstCenter2 = new List<CostcenterName>();     // 
        private List<CustomerName> lstDept2 = new List<CustomerName>();     // 
        private List<Controller.FabricMapInfo> lstFabricMapInfo = null; 

        private const int ROWCOUNT = 30;
        private const int COLUMNCOUNT = 40;

        //private string _layoutfile = "/GVLayoutFabricWarehouse.xml";
        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 35, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public Warehouse(InheritMDI main)
        {
            base.InitializeComponent(); // parent 컴포넌트에 접근하기 위해 컨트롤을 public으로 지정 > 향후 수정 필요 (todo) 
            InitializeComponent();
            __main__ = main;            // MDI 연결 
            _gv1 = this.gvMain;  // 그리드뷰 일반화를 위해 변수 별도 저장
            
            //this.Width = 2030;
            //this.Height = 1160;
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCodeSize_Load(object sender, EventArgs e)
        {
            //this.WindowState = FormWindowState.Maximized;  // 창 최대화
            DataLoading_DDL();          // DDL 데이터 로딩
            Config_DropDownList();      // 상단 DDL 생성 설정

            
            //GV1_CreateColumn(_gv1);     // 그리드뷰 생성
            //GV1_LayoutSetting(_gv1);    // 중앙 그리드뷰 설정 

            ToolTip toolTip = this._gv1.Behavior.ToolTip;
            toolTip.AutoPopDelay = 15000;
            toolTip.UseAnimation = true;
            
        }
        
        /// <summary>
        /// 상단 검색을 위한 Dropdownlist 생성
        /// </summary>
        private void Config_DropDownList()
        {
            // 
            ddlStatus.DataSource = lstStatus; 
            ddlStatus.DisplayMember = "Contents";
            ddlStatus.ValueMember = "CodeIdx";
            ddlStatus.AutoCompleteMode = AutoCompleteMode.Suggest; 
            ddlStatus.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlStatus.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 
            ddlBuyer.DataSource = lstBuyer;
            ddlBuyer.DisplayMember = "CustName";
            ddlBuyer.ValueMember = "CustIdx";
            ddlBuyer.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlBuyer.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlBuyer.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
            
            // 
            ddlColor.DataSource = lstColor;
            ddlColor.DisplayMember = "Contents";
            ddlColor.ValueMember = "Contents";
            ddlColor.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlColor.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlColor.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 
            ddlFabric.DataSource = lstFabric;
            ddlFabric.DisplayMember = "LongName";
            ddlFabric.ValueMember = "Idx";
            ddlFabric.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlFabric.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlFabric.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 
            ddlFabricType.DataSource = lstFabricType;
            ddlFabricType.DisplayMember = "Contents";
            ddlFabricType.ValueMember = "CodeIdx";
            ddlFabricType.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlFabricType.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlFabricType.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
            
            // 
            ddlRack1.DataSource = lstRack1;
            ddlRack1.DisplayMember = "Contents";
            ddlRack1.ValueMember = "CodeIdx";
            ddlRack1.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlRack1.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlRack1.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
            // 
            ddlRack2.DataSource = lstRack2;
            ddlRack2.DisplayMember = "Contents";
            ddlRack2.ValueMember = "CodeIdx";
            ddlRack2.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlRack2.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlRack2.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
            // 
            ddlRack3.DataSource = lstRack3;
            ddlRack3.DisplayMember = "Contents";
            ddlRack3.ValueMember = "CodeIdx";
            ddlRack3.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlRack3.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlRack3.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
        }

        /// <summary>
        /// 입고 그리드뷰 컬럼 생성
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV1_CreateColumn(RadGridView gv)
        {
            #region Columns 생성
            
            GridViewComboBoxColumn Status = new GridViewComboBoxColumn();
            Status.Name = "Status";
            Status.DataSource = lstStatus2;
            Status.DisplayMember = "Contents";
            Status.ValueMember = "CodeIdx";
            Status.FieldName = "Status";
            Status.HeaderText = "Status";
            Status.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Status.DropDownStyle = RadDropDownStyle.DropDown;
            Status.Width = 100;
            gv.Columns.Add(Status);

            GridViewDateTimeColumn IDate = new GridViewDateTimeColumn();
            IDate.Name = "IDate";
            IDate.Width = 100;
            IDate.TextAlignment = ContentAlignment.MiddleCenter;
            IDate.FormatString = "{0:d}";
            IDate.FieldName = "IDate";
            IDate.HeaderText = "Date";
            IDate.ReadOnly = true;
            gv.Columns.Add(IDate);


            GridViewComboBoxColumn BuyerIdx = new GridViewComboBoxColumn();
            BuyerIdx.Name = "BuyerIdx";
            BuyerIdx.DataSource = lstBuyer2;
            BuyerIdx.ValueMember = "CustIdx";
            BuyerIdx.DisplayMember = "CustName";
            BuyerIdx.FieldName = "BuyerIdx";
            BuyerIdx.HeaderText = "Buyer";
            BuyerIdx.Width = 100;
            gv.Columns.Insert(3, BuyerIdx);

            GridViewComboBoxColumn ColorIdx = new GridViewComboBoxColumn();
            ColorIdx.Name = "ColorIdx";
            ColorIdx.DataSource = lstColor2;
            ColorIdx.DisplayMember = "Contents";
            ColorIdx.ValueMember = "Contents";
            ColorIdx.FieldName = "ColorIdx";
            ColorIdx.HeaderText = "Color";
            ColorIdx.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ColorIdx.DropDownStyle = RadDropDownStyle.DropDown;
            ColorIdx.Width = 200;
            gv.Columns.Add(ColorIdx);
                        
            GridViewComboBoxColumn FabricIdx = new GridViewComboBoxColumn();
            FabricIdx.Name = "FabricIdx";
            FabricIdx.DataSource = lstFabric2;
            FabricIdx.DisplayMember = "LongName";
            FabricIdx.ValueMember = "Idx";
            FabricIdx.FieldName = "FabricIdx";
            FabricIdx.HeaderText = "Fabric";
            FabricIdx.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            FabricIdx.DropDownStyle = RadDropDownStyle.DropDown;
            FabricIdx.Width = 150; 
            gv.Columns.Add(FabricIdx);

            GridViewComboBoxColumn FabricType = new GridViewComboBoxColumn();
            FabricType.Name = "FabricType";
            FabricType.DataSource = lstFabricType2;
            FabricType.DisplayMember = "Contents";
            FabricType.ValueMember = "CodeIdx";
            FabricType.FieldName = "FabricType";
            FabricType.HeaderText = "FabricType";
            FabricType.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            FabricType.DropDownStyle = RadDropDownStyle.DropDown;
            FabricType.Width = 100;
            gv.Columns.Add(FabricType);
                        
            GridViewTextBoxColumn Artno = new GridViewTextBoxColumn();
            Artno.Name = "Artno";
            Artno.FieldName = "Artno";
            Artno.HeaderText = "Art";
            Artno.Width = 100;
            Artno.TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns.Add(Artno);

            GridViewTextBoxColumn Lotno = new GridViewTextBoxColumn();
            Lotno.Name = "Lotno";
            Lotno.FieldName = "Lotno";
            Lotno.HeaderText = "Lot#";
            Lotno.TextAlignment = ContentAlignment.MiddleLeft;
            Lotno.Width = 100;
            gv.Columns.Add(Lotno);

            GridViewTextBoxColumn Roll = new GridViewTextBoxColumn();
            Roll.DataType = typeof(int);
            Roll.Name = "Roll";
            Roll.FieldName = "Roll";
            Roll.HeaderText = "Roll";
            Roll.Width = 60;
            Roll.TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns.Add(Roll);

            GridViewTextBoxColumn Width = new GridViewTextBoxColumn();
            Width.DataType = typeof(int);
            Width.Name = "Width";
            Width.FieldName = "Width";
            Width.HeaderText = "Width";
            Width.Width = 60;
            Width.TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns.Add(Width);

            GridViewTextBoxColumn Kgs = new GridViewTextBoxColumn();
            Kgs.DataType = typeof(double);
            Kgs.Name = "Kgs";
            Kgs.FieldName = "Kgs";
            Kgs.HeaderText = "Kgs";
            Kgs.Width = 60;
            Kgs.TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns.Add(Kgs);

            GridViewTextBoxColumn Yds = new GridViewTextBoxColumn();
            Yds.DataType = typeof(double);
            Yds.Name = "Yds";
            Yds.FieldName = "Yds";
            Yds.HeaderText = "Yds";
            Yds.Width = 60;
            Yds.TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns.Add(Yds);

            GridViewTextBoxColumn IOCenterIdx = new GridViewTextBoxColumn();
            IOCenterIdx.Name = "IOCenterIdx";
            IOCenterIdx.FieldName = "IOCenterIdx";
            IOCenterIdx.IsVisible = false; 
            gv.Columns.Add(IOCenterIdx);
            
            GridViewComboBoxColumn IODeptIdx = new GridViewComboBoxColumn();
            IODeptIdx.Name = "IODeptIdx";
            IODeptIdx.DataSource = lstDept;
            IODeptIdx.DisplayMember = "CustName";
            IODeptIdx.ValueMember = "CustIdx";
            IODeptIdx.FieldName = "IODeptIdx";
            IODeptIdx.HeaderText = "In";
            IODeptIdx.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            IODeptIdx.DropDownStyle = RadDropDownStyle.DropDown;
            IODeptIdx.Width = 150;
            gv.Columns.Add(IODeptIdx);
            
            GridViewComboBoxColumn RackNo = new GridViewComboBoxColumn();
            RackNo.Name = "RackNo";
            RackNo.DataSource = lstRack21;
            RackNo.DisplayMember = "Contents";
            RackNo.ValueMember = "CodeIdx";
            RackNo.FieldName = "RackNo";
            RackNo.HeaderText = "Rack#";
            RackNo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            RackNo.DropDownStyle = RadDropDownStyle.DropDown;
            RackNo.Width = 70;
            gv.Columns.Add(RackNo);

            GridViewComboBoxColumn Floorno = new GridViewComboBoxColumn();
            Floorno.Name = "Floorno";
            Floorno.DataSource = lstRack22;
            Floorno.DisplayMember = "Contents";
            Floorno.ValueMember = "CodeIdx";
            Floorno.FieldName = "Floorno";
            Floorno.HeaderText = "Floor#";
            Floorno.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Floorno.DropDownStyle = RadDropDownStyle.DropDown;
            Floorno.Width = 70;
            gv.Columns.Add(Floorno);

            GridViewComboBoxColumn RackPos = new GridViewComboBoxColumn();
            RackPos.Name = "RackPos";
            RackPos.DataSource = lstRack23;
            RackPos.DisplayMember = "Contents";
            RackPos.ValueMember = "CodeIdx";
            RackPos.FieldName = "RackPos";
            RackPos.HeaderText = "Rack\nPosition";
            RackPos.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            RackPos.DropDownStyle = RadDropDownStyle.DropDown;
            RackPos.Width = 70;
            gv.Columns.Add(RackPos);

            GridViewTextBoxColumn PosX = new GridViewTextBoxColumn();
            PosX.DataType = typeof(int);
            PosX.Name = "PosX";
            PosX.FieldName = "PosX";
            PosX.HeaderText = "Location\n(X)";
            PosX.TextAlignment = ContentAlignment.MiddleCenter;
            PosX.Width = 70;
            gv.Columns.Add(PosX);

            GridViewTextBoxColumn PosY = new GridViewTextBoxColumn();
            PosY.DataType = typeof(int);
            PosY.Name = "PosY";
            PosY.FieldName = "PosY";
            PosY.HeaderText = "Location\n(Y)";
            PosY.TextAlignment = ContentAlignment.MiddleCenter;
            PosY.Width = 70;
            gv.Columns.Add(PosY);

            GridViewTextBoxColumn Qrcode = new GridViewTextBoxColumn();
            Qrcode.DataType = typeof(int);
            Qrcode.Name = "Qrcode";
            Qrcode.FieldName = "Qrcode";
            Qrcode.HeaderText = "QR Code";
            Qrcode.IsVisible = false;
            Qrcode.ReadOnly = true;
            Qrcode.TextAlignment = ContentAlignment.MiddleLeft;
            Qrcode.Width = 70;
            gv.Columns.Add(Qrcode);

            GridViewTextBoxColumn filenm1 = new GridViewTextBoxColumn();
            filenm1.Name = "filenm1";
            filenm1.FieldName = "filenm1";
            filenm1.IsVisible = false; 
            gv.Columns.Add(filenm1);

            GridViewTextBoxColumn filenm2 = new GridViewTextBoxColumn();
            filenm2.Name = "filenm2";
            filenm2.FieldName = "filenm2";
            filenm2.IsVisible = false;
            gv.Columns.Add(filenm2);

            GridViewTextBoxColumn fileurl1 = new GridViewTextBoxColumn();
            fileurl1.Name = "fileurl1";
            fileurl1.FieldName = "fileurl1";
            fileurl1.IsVisible = false;
            gv.Columns.Add(fileurl1);

            GridViewTextBoxColumn fileurl2 = new GridViewTextBoxColumn();
            fileurl2.Name = "fileurl2";
            fileurl2.FieldName = "fileurl2";
            fileurl2.IsVisible = false;
            gv.Columns.Add(fileurl2);

            GridViewTextBoxColumn Comments = new GridViewTextBoxColumn();
            Comments.Name = "Comments";
            Comments.FieldName = "Comments";
            Comments.HeaderText = "Comments";
            Comments.TextAlignment = ContentAlignment.MiddleLeft;
            Comments.Width = 100;
            gv.Columns.Add(Comments);

            GridViewTextBoxColumn WorkOrderIdx = new GridViewTextBoxColumn();
            WorkOrderIdx.Name = "WorkOrderIdx";
            WorkOrderIdx.FieldName = "WorkOrderIdx";
            WorkOrderIdx.HeaderText = "Inbound#";
            WorkOrderIdx.ReadOnly = true;
            WorkOrderIdx.TextAlignment = ContentAlignment.MiddleLeft;
            WorkOrderIdx.Width = 100;
            gv.Columns.Add(WorkOrderIdx);

            #endregion
        }
        
        #endregion

        #region 3. 컨트롤 초기 설정
        
        /// <summary>
        /// 그리드뷰 설정
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV1_LayoutSetting(RadGridView gv)
        {
            #region Config Gridview 

            gv.Dock = DockStyle.Fill;
            gv.AllowAddNewRow = false;
            gv.AllowCellContextMenu = true;
            gv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
            gv.AllowColumnHeaderContextMenu = false;
            gv.EnableGrouping = false;
            gv.MasterView.TableHeaderRow.MinHeight = 50;
            
            gv.GridViewElement.PagingPanelElement.NumericButtonsCount = 15;
            gv.GridViewElement.PagingPanelElement.ShowFastBackButton = false;
            gv.GridViewElement.PagingPanelElement.ShowFastForwardButton = false;

            gv.MultiSelect = true;

            #endregion

            #region Config Cell Conditions: 상태에 따른 행스타일 변경

            // 캔슬오더 색상변경
            //Font f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Strikeout);
            //ConditionalFormattingObject obj = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "2", "", true);
            //obj.RowForeColor = Color.Black;
            //obj.RowBackColor = Color.FromArgb(255, 255, 230, 230);
            //obj.RowFont = f;
            //gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj);

            //// 마감오더 색상변경
            //f = new Font(new FontFamily("Segoe UI"), 8.25f);
            //ConditionalFormattingObject obj2 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "3", "", true);
            //obj2.RowForeColor = Color.Black;
            //obj2.RowBackColor = Color.FromArgb(255, 220, 255, 240);
            //obj2.RowFont = f;
            //gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj2);

            //// 선적완료 색상변경
            //f = new Font(new FontFamily("Segoe UI"), 8.25f);
            //ConditionalFormattingObject obj3 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "1", "", true);
            //obj3.RowForeColor = Color.Black;
            //obj3.RowFont = f;
            //gv.Columns["ShipCompleted"].ConditionalFormattingObjectList.Add(obj3);

            //f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            //ConditionalFormattingObject obj4 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "0", "", true);
            //obj4.RowForeColor = Color.Black;
            //obj4.RowFont = f;
            //gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj4);

            //f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            //ConditionalFormattingObject obj5 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "1", "", true);
            //obj5.RowForeColor = Color.Black;
            //obj5.RowFont = f;
            //gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj5);

            #endregion

        }

        #endregion
        
        #region 5. 데이터 조회 (바인딩 테스트후, 무겁고 너무느려서 직접 쿼리로 제어)

        /// <summary>
        /// Dropdownlist 데이터 로딩
        /// </summary>
        private void DataLoading_DDL()
        {
            // Status 
            /* -- 1:대기상태, 2:입고(생산처), 3:입고(사용가능 잔량회수), 4:입고(구매처)
               --5:정상출고, 6:출고(원단불량), 7:판매, 8:폐기, 9:악성재고, 10:공장용보관 */
            lstStatus.Add(new CodeContents(0, CommonValues.DicFabricInStatus[0], ""));
            lstStatus.Add(new CodeContents(1, CommonValues.DicFabricInStatus[1], ""));
            lstStatus.Add(new CodeContents(2, CommonValues.DicFabricInStatus[2], ""));
            lstStatus.Add(new CodeContents(3, CommonValues.DicFabricInStatus[3], ""));
            lstStatus.Add(new CodeContents(4, CommonValues.DicFabricInStatus[4], ""));
            lstStatus.Add(new CodeContents(9, CommonValues.DicFabricInStatus[9], ""));
            lstStatus.Add(new CodeContents(10, CommonValues.DicFabricInStatus[10], ""));

            lstStatus2.Add(new CodeContents(0, CommonValues.DicFabricInStatus[0], ""));
            lstStatus2.Add(new CodeContents(1, CommonValues.DicFabricInStatus[1], ""));
            lstStatus2.Add(new CodeContents(2, CommonValues.DicFabricInStatus[2], ""));
            lstStatus2.Add(new CodeContents(3, CommonValues.DicFabricInStatus[3], ""));
            lstStatus2.Add(new CodeContents(4, CommonValues.DicFabricInStatus[4], ""));
            lstStatus2.Add(new CodeContents(9, CommonValues.DicFabricInStatus[9], ""));
            lstStatus2.Add(new CodeContents(10, CommonValues.DicFabricInStatus[10], ""));

            lstOutStatus.Add(new CodeContents(0, CommonValues.DicFabricOutStatus[0], ""));
            lstOutStatus.Add(new CodeContents(5, CommonValues.DicFabricOutStatus[5], ""));
            lstOutStatus.Add(new CodeContents(6, CommonValues.DicFabricOutStatus[6], ""));
            lstOutStatus.Add(new CodeContents(7, CommonValues.DicFabricOutStatus[7], ""));
            lstOutStatus.Add(new CodeContents(8, CommonValues.DicFabricOutStatus[8], ""));
            lstOutStatus.Add(new CodeContents(11, CommonValues.DicFabricOutStatus[11], ""));

            lstOutStatus2.Add(new CodeContents(0, CommonValues.DicFabricOutStatus[0], ""));
            lstOutStatus2.Add(new CodeContents(5, CommonValues.DicFabricOutStatus[5], ""));
            lstOutStatus2.Add(new CodeContents(6, CommonValues.DicFabricOutStatus[6], ""));
            lstOutStatus2.Add(new CodeContents(7, CommonValues.DicFabricOutStatus[7], ""));
            lstOutStatus2.Add(new CodeContents(8, CommonValues.DicFabricOutStatus[8], ""));
            lstOutStatus2.Add(new CodeContents(11, CommonValues.DicFabricOutStatus[11], ""));


            // Buyer
            _dt = CommonController.Getlist(CommonValues.KeyName.CustIdx).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                lstBuyer.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
                lstBuyer2.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
            }

            // Color 
            _dt = Codes.Controller.Color.GetUselist().Tables[0];
            lstColor.Add(new CodeContents(0, "", ""));
            lstColor2.Add(new CodeContents(0, "", ""));
            foreach (DataRow row in _dt.Rows)
            {
                lstColor.Add(new CodeContents(Convert.ToInt32(row["ColorIdx"]),
                                            row["ColorName"].ToString(),
                                            ""));
                lstColor2.Add(new CodeContents(Convert.ToInt32(row["ColorIdx"]),
                                            row["ColorName"].ToString(),
                                            ""));
            }

            // Fabric IN
            _dt = CommonController.Getlist(CommonValues.KeyName.Wash).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                lstDept.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
            }

            // Fabric 
            _searchString = new Dictionary<CommonValues.KeyName, string>();
            _searchString.Add(CommonValues.KeyName.Remark, "");
            _searchString.Add(CommonValues.KeyName.IsUse, "1");
            _dt = Controller.Fabric.Getlist(_searchString).Tables[0];

            lstFabric.Add(new Controller.Fabric(0, "", ""));
            lstFabric2.Add(new Controller.Fabric(0, "", ""));
            foreach (DataRow row in _dt.Rows)
            {
                lstFabric.Add(new Controller.Fabric(Convert.ToInt32(row["Idx"]),
                                            row["LongName"].ToString(),
                                            row["ShortName"].ToString()));
                lstFabric2.Add(new Controller.Fabric(Convert.ToInt32(row["Idx"]),
                                            row["LongName"].ToString(),
                                            row["ShortName"].ToString()));
            }

            // 코드
            _dt = CommonController.Getlist(CommonValues.KeyName.Codes).Tables[0];

            codeName.Add(new CodeContents(0, "", "Fabric Type")); 
            foreach (DataRow row in _dt.Rows)
            {
                codeName.Add(new CodeContents(Convert.ToInt32(row["Idx"]),
                                            row["Contents"].ToString(),
                                            row["Classification"].ToString()));
            }

            // Fabric Type 
            
            lstFabricType.Clear();
            lstFabricType = codeName.FindAll(
                delegate (CodeContents code)
                {
                    return code.Classification == "Fabric Type";
                });

            lstFabricType2.Clear();
            lstFabricType2 = codeName.FindAll(
                delegate (CodeContents code)
                {
                    return code.Classification == "Fabric Type";
                });

            // Lot# 

            // Inbound (WorkOrderIdx)

            // Location X

            // Location Y

            // Rack# 
            lstRack1.Add(new CodeContents(0, "", ""));
            lstRack1.Add(new CodeContents(1, "1", ""));
            lstRack1.Add(new CodeContents(2, "2", ""));
            lstRack1.Add(new CodeContents(3, "3", ""));
            lstRack1.Add(new CodeContents(4, "4", ""));
            lstRack1.Add(new CodeContents(5, "5", ""));
            lstRack1.Add(new CodeContents(6, "6", ""));
            lstRack1.Add(new CodeContents(7, "7", ""));
            lstRack1.Add(new CodeContents(8, "8", ""));
            lstRack1.Add(new CodeContents(9, "9", ""));
            lstRack1.Add(new CodeContents(10, "10", ""));

            lstRack2.Add(new CodeContents(0, "", ""));
            lstRack2.Add(new CodeContents(1, "A", ""));
            lstRack2.Add(new CodeContents(2, "B", ""));
            lstRack2.Add(new CodeContents(3, "C", ""));
            lstRack2.Add(new CodeContents(4, "D", ""));
            lstRack2.Add(new CodeContents(5, "E", ""));
            lstRack2.Add(new CodeContents(6, "F", ""));
            lstRack2.Add(new CodeContents(7, "G", ""));
            lstRack2.Add(new CodeContents(8, "H", ""));
            lstRack2.Add(new CodeContents(9, "I", ""));

            lstRack3.Add(new CodeContents(0, "", ""));
            lstRack3.Add(new CodeContents(1, "1", ""));
            lstRack3.Add(new CodeContents(2, "2", ""));
            lstRack3.Add(new CodeContents(3, "3", ""));
            lstRack3.Add(new CodeContents(4, "4", ""));
            lstRack3.Add(new CodeContents(5, "5", ""));
            lstRack3.Add(new CodeContents(6, "6", ""));
            lstRack3.Add(new CodeContents(7, "7", ""));
            lstRack3.Add(new CodeContents(8, "8", ""));
            lstRack3.Add(new CodeContents(9, "9", ""));
            lstRack3.Add(new CodeContents(10, "10", ""));

            lstRack21.Add(new CodeContents(0, "", ""));
            lstRack21.Add(new CodeContents(1, "1", ""));
            lstRack21.Add(new CodeContents(2, "2", ""));
            lstRack21.Add(new CodeContents(3, "3", ""));
            lstRack21.Add(new CodeContents(4, "4", ""));
            lstRack21.Add(new CodeContents(5, "5", ""));
            lstRack21.Add(new CodeContents(6, "6", ""));
            lstRack21.Add(new CodeContents(7, "7", ""));
            lstRack21.Add(new CodeContents(8, "8", ""));
            lstRack21.Add(new CodeContents(9, "9", ""));
            lstRack21.Add(new CodeContents(10, "10", ""));

            lstRack22.Add(new CodeContents(0, "", ""));
            lstRack22.Add(new CodeContents(1, "A", ""));
            lstRack22.Add(new CodeContents(2, "B", ""));
            lstRack22.Add(new CodeContents(3, "C", ""));
            lstRack22.Add(new CodeContents(4, "D", ""));
            lstRack22.Add(new CodeContents(5, "E", ""));
            lstRack22.Add(new CodeContents(6, "F", ""));
            lstRack22.Add(new CodeContents(7, "G", ""));
            lstRack22.Add(new CodeContents(8, "H", ""));
            lstRack22.Add(new CodeContents(9, "I", ""));

            lstRack23.Add(new CodeContents(0, "", ""));
            lstRack23.Add(new CodeContents(1, "1", ""));
            lstRack23.Add(new CodeContents(2, "2", ""));
            lstRack23.Add(new CodeContents(3, "3", ""));
            lstRack23.Add(new CodeContents(4, "4", ""));
            lstRack23.Add(new CodeContents(5, "5", ""));
            lstRack23.Add(new CodeContents(6, "6", ""));
            lstRack23.Add(new CodeContents(7, "7", ""));
            lstRack23.Add(new CodeContents(8, "8", ""));
            lstRack23.Add(new CodeContents(9, "9", ""));
            lstRack23.Add(new CodeContents(10, "10", ""));
        }

        /// <summary>
        /// 검색버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefleshWithCondition();
        }

        /// <summary>
        /// 검색조건을 이용한 자료 새로고침
        /// </summary>
        private void RefleshWithCondition()
        {
            try
            {
                if (ddlStatus.SelectedValue != null || ddlBuyer.SelectedValue != null || ddlColor.SelectedValue != null
                    || ddlFabric.SelectedValue != null || ddlFabricType.SelectedValue != null
                    || !string.IsNullOrEmpty(txtLotno.Text) || !string.IsNullOrEmpty(txtInboundno.Text)
                    || !string.IsNullOrEmpty(txtLocationX.Value.ToString()) || !string.IsNullOrEmpty(txtLocationY.Value.ToString())
                    || ddlRack1.SelectedValue != null || ddlRack2.SelectedValue != null || ddlRack3.SelectedValue != null
                    )
                {
                    _searchString = new Dictionary<CommonValues.KeyName, string>();
                    _searchString.Add(CommonValues.KeyName.Lotno, txtLotno.Text.Trim());
                    _searchString.Add(CommonValues.KeyName.WorkOrderIdx, txtInboundno.Text.Trim());
                    _searchString.Add(CommonValues.KeyName.ColorIdx, Convert.ToString(ddlColor.SelectedValue));

                    _searchKey = new Dictionary<CommonValues.KeyName, int>();
                    _searchKey.Add(CommonValues.KeyName.Status, Convert.ToInt32(ddlStatus.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.BuyerIdx, Convert.ToInt32(ddlBuyer.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.FabricIdx, Convert.ToInt32(ddlFabric.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.FabricType, Convert.ToInt32(ddlFabricType.SelectedValue));

                    _searchKey.Add(CommonValues.KeyName.RackNo, Convert.ToInt32(ddlRack1.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Floorno, Convert.ToInt32(ddlRack2.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.RackPos, Convert.ToInt32(ddlRack3.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.PosX, Convert.ToInt32(txtLocationX.Value));
                    _searchKey.Add(CommonValues.KeyName.PosY, Convert.ToInt32(txtLocationY.Value));
                    
                    ApplyColors(); 
                    DataBinding_GV1();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

        }

        /// <summary>
        /// 메인 그리드뷰 데이터 로딩
        /// </summary>
        /// <param name="SizeName">사이즈명</param>
        private void DataBinding_GV1()
        {
            try
            {
                _gv1.DataSource = null;

                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                if (Convert.ToInt16(__AUTHCODE__.Substring(0, 1).Trim()) > 0)
                {
                    CultureInfo ci = new CultureInfo("ko-KR");
                    _ds1 = Controller.Inbound.GetlistWareHouse(_searchString, _searchKey, dtInboundDate.Value.ToString("d", ci));

                    if (_ds1 != null)
                    {
                        UpdateColors(); 
                        //_gv1.DataSource = _ds1.Tables[0].DefaultView;
                        //__main__.lblRows.Text = _gv1.RowCount.ToString() + " Rows";
                        //_gv1.EnablePaging = CommonValues.enablePaging;
                        //_gv1.AllowSearchRow = CommonValues.enableSearchRow;
                    }
                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBindingGv1: " + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 조회하기전 배경 및 데이터 초기화 
        /// </summary>
        private void ApplyColors()
        {
            _gv1.Rows.Clear();

            this._gv1.ShowGroupPanel = false;
            this._gv1.VirtualMode = true;
            this._gv1.RowCount = ROWCOUNT;
            this._gv1.ColumnCount = COLUMNCOUNT;
            this._gv1.BackColor = Color.Transparent;
            this._gv1.ReadOnly = true;
            this._gv1.TableElement.DrawFill = false;
            this._gv1.TableElement.RowHeight = 25;
            this._gv1.TableElement.CellSpacing = 0;
            this._gv1.TableElement.RowSpacing = 0;
            this._gv1.MasterTemplate.ShowRowHeaderColumn = false;
            this._gv1.MasterTemplate.ShowColumnHeaders = false;
            this._gv1.MasterTemplate.AllowAddNewRow = false;
            
            //Random r = new Random();
            for (int y = 0; y < this._gv1.RowCount; y++)
            {
                for (int x = 0; x < this._gv1.ColumnCount; x++)
                {
                    this._gv1.Rows[y].Cells[x].Style.CustomizeFill = true;
                    this._gv1.Rows[y].Cells[x].Style.DrawFill = true;
                    this._gv1.Rows[y].Cells[x].ColumnInfo.Width = 35; 
                    this._gv1.Rows[y].Cells[x].Style.BackColor = Color.FromArgb(255, 255, 255, 255);
                    //this._gv1.Rows[y].Cells[x].Style.BackColor = Color.FromArgb(r.Next(255), r.Next(255), r.Next(255));
                }
            }
        }
        
        // 조회 및 툴팁에 표시할 데이터 저장 
        private void UpdateColors()
        {
            lstFabricMapInfo = new List<Controller.FabricMapInfo>();
            Controller.FabricMapInfo mapInfo = null; 
            int rowInfo = 0; 

            foreach(DataRow row in _ds1.Tables[0].Rows)
            {
                if (row["FabricIdx"]!=DBNull.Value && !string.IsNullOrEmpty(row["FabricIdx"].ToString()) &&
                    row["PosY"] !=DBNull.Value && row["PosX"] != DBNull.Value && row["Status"] != DBNull.Value &&
                    Convert.ToInt32(row["PosY"])>0 && Convert.ToInt32(row["PosX"]) > 0 && Convert.ToInt32(row["Status"]) > 0)
                {
                    // FabricMapInfo에 값 저장 
                    mapInfo = new Controller.FabricMapInfo();
                    mapInfo.IDate = "Inbound Date: " + Convert.ToDateTime(row["IDate"]).ToString(); 
                    mapInfo.Buyer = "Buyer: " + row["BuyerIdx"].ToString();
                    mapInfo.Color = "Color: " + row["ColorIdx"].ToString();
                    mapInfo.Fabric = "Fabric: " + row["FabricIdx"].ToString();
                    mapInfo.FabricType = "FabricType: " + row["FabricType"].ToString();
                    mapInfo.LotNo = "Lot#: " + row["Lotno"].ToString();
                    mapInfo.ArtNo = "Art#: " + row["Artno"].ToString();
                    mapInfo.InCust = "Inbound: " + row["IODeptIdx"].ToString();
                    mapInfo.Width = "Width: " + Convert.ToInt32(row["Width"]).ToString();
                    mapInfo.Kgs = "Kgs: " + Convert.ToDouble(row["Kgs"]).ToString() + " kgs";
                    mapInfo.Yds = "Yds: " + Convert.ToDouble(row["Yds"]).ToString() + " yds";
                    mapInfo.Coords = "X, Y: " + Convert.ToInt32(row["PosX"]).ToString() + ", " + Convert.ToInt32(row["PosY"]).ToString();
                    if (Convert.ToInt32(row["RackNo"]) <= 0) mapInfo.CoordsRack = ""; 
                    else mapInfo.CoordsRack = Convert.ToInt32(row["RackNo"]).ToString() + "" + Convert.ToChar(Convert.ToInt32(row["Floorno"]) + 64) + ""
                                            + Convert.ToInt32(row["RackPos"]).ToString();

                    lstFabricMapInfo.Add(mapInfo);

                    // Cell포맷팅에서 해당 데이터를 찾기위해 태그에 인덱스를 저장해둔다 
                    this._gv1.Rows[Convert.ToInt32(row["PosY"]) - 1].Cells[COLUMNCOUNT - Convert.ToInt32(row["PosX"])].Tag = rowInfo;
                    rowInfo++; 

                    switch (Convert.ToInt32(row["Status"]))
                    {
                        case 2:
                            this._gv1.Rows[Convert.ToInt32(row["PosY"]) - 1].Cells[COLUMNCOUNT - Convert.ToInt32(row["PosX"])].Style.BackColor = Color.YellowGreen;
                            break;
                        case 3:
                            this._gv1.Rows[Convert.ToInt32(row["PosY"]) - 1].Cells[COLUMNCOUNT - Convert.ToInt32(row["PosX"])].Style.BackColor = Color.DeepSkyBlue;
                            break;
                        case 4:
                            this._gv1.Rows[Convert.ToInt32(row["PosY"]) - 1].Cells[COLUMNCOUNT - Convert.ToInt32(row["PosX"])].Style.BackColor = Color.Gold;
                            break;
                        case 9:
                            this._gv1.Rows[Convert.ToInt32(row["PosY"]) - 1].Cells[COLUMNCOUNT - Convert.ToInt32(row["PosX"])].Style.BackColor = Color.Salmon;
                            break;
                        case 10:
                            this._gv1.Rows[Convert.ToInt32(row["PosY"]) - 1].Cells[COLUMNCOUNT - Convert.ToInt32(row["PosX"])].Style.BackColor = Color.Violet;
                            break;

                    }
                    
                }
            }

            
        }

        private void GV1_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            e.RowElement.DrawBorder = false;
            e.RowElement.DrawFill = false;
        }

        private void GV1_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            e.CellElement.Text = "";
            e.CellElement.ToolTipText = ""; 

            // 태그값으로 리스트에서 데이터 불러와 툴팁에 표기한다 
            if (e.Row is GridViewDataRowInfo && _gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag != null)
            {
                e.CellElement.ToolTipText = lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].IDate + "\n" +
                                            lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].Buyer + "\n" +
                                            lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].Color + "\n" +
                                            lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].Fabric + "\n" +
                                            lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].FabricType + "\n" +
                                            lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].LotNo + "\n" +
                                            lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].ArtNo + "\n" +
                                            lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].InCust + "\n" +
                                            lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].Width + "\n" +
                                            lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].Kgs + "\n" +
                                            lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].Yds + "\n" +
                                            lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].Coords + "\n" +
                                            "Rack#: " + lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].CoordsRack;

                e.CellElement.Text = lstFabricMapInfo[Convert.ToInt32(_gv1.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag)].CoordsRack;
                e.CellElement.TextAlignment = ContentAlignment.MiddleCenter; 
            }


            if (e.CellElement.IsCurrent)
            {
                e.CellElement.DrawBorder = true;
                e.CellElement.BorderWidth = 2;
                e.CellElement.BorderColor = Color.Yellow;
                e.CellElement.BorderBoxStyle = BorderBoxStyle.SingleBorder;
            }
            else
            {
                e.CellElement.DrawBorder = true;
                e.CellElement.BorderWidth = 1;
                e.CellElement.BorderColor = Color.Honeydew;
                e.CellElement.BorderColor2 = Color.Honeydew;
                e.CellElement.BorderColor3 = Color.Honeydew;
                e.CellElement.BorderColor4 = Color.Honeydew;

                e.CellElement.BorderBoxStyle = BorderBoxStyle.OuterInnerBorders;
                //e.CellElement.BackColor = Color.FromArgb(255, 255, 255, 255);
            }
        }

        #endregion

        #region 6. 컨트롤 이벤트 및 기타 설정

        /// <summary>
        /// 그리드뷰 데이터 변경시 자료 업데이트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GV1_Update(object sender, GridViewCellEventArgs e)
        {

        }

        /// <summary>
        /// 그리드뷰 셀포맷팅 (각 이벤트시 수시작동) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            
        }

        /// <summary>
        /// 그리드뷰 셀 생성시 DDL의 높이,출력항목수 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterTemplate_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {

        }
        
        /// <summary>
        /// 그리드뷰 행 클릭 (다중선택시 오더복사 disable)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_Click(object sender, EventArgs e)
        {
            
        }

        private void ddlBuyer_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {

        }

        private void gvMain_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            
        }
            

        /// <summary>
        /// 행 선택시 - 오더캔슬, 마감일때 편집 불가능하도록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void radLabel12_DoubleClick(object sender, EventArgs e)
        {
            dtInboundDate.Value = Convert.ToDateTime("2000-01-01"); 
        }
        

        /// <summary>
        /// 컬러 드롭다운 입력받은 텍스트로 처리하기 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_EditorRequired(object sender, EditorRequiredEventArgs e)
        {

        }

        private void gvMain_CreateCell(object sender, GridViewCreateCellEventArgs e)
        {

        }

        private void ddlCenter_SelectedValueChanged(object sender, EventArgs e)
        {
                       
        }



        #endregion

        private void Warehouse_Resize(object sender, EventArgs e)
        {
            //Console.WriteLine(this.Width.ToString() + ", " + this.Height.ToString()); 
        }

        private void btnSearch1_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            try
            {
                _gv1.DataSource = null;
                
                ApplyColors();

                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                if (Convert.ToInt16(__AUTHCODE__.Substring(0, 1).Trim()) > 0)
                {
                    CultureInfo ci = new CultureInfo("ko-KR");
                    _ds1 = Data.InboundData.GetlistWareHouse2(_searchString, _searchKey, dtInboundDate.Value.ToString("d", ci));

                    if (_ds1 != null)
                    {
                        UpdateColors();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("btnSearch2: " + ex.Message.ToString());
            }
    }
    }

}
