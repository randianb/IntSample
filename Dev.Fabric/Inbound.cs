﻿using Int.Code;
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
    /// 샘플오더 관리화면: 오더입력수정 및 작업지시등
    /// </summary>
    public partial class Inbound : InheritForm
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

        private string _layoutfile = "/GVLayoutFabricInbound.xml";
        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 27, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public Inbound(InheritMDI main)
        {
            base.InitializeComponent(); // parent 컴포넌트에 접근하기 위해 컨트롤을 public으로 지정 > 향후 수정 필요 (todo) 
            InitializeComponent();
            __main__ = main;            // MDI 연결 
            _gv1 = this.gvMain;  // 그리드뷰 일반화를 위해 변수 별도 저장
            //toolWindow3.AutoHide();

            // 컨트롤 상태 초기화 
            ControlStateReset();
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCodeSize_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;  // 창 최대화
            DataLoading_DDL();          // DDL 데이터 로딩
            Config_DropDownList();      // 상단 DDL 생성 설정
            GV1_CreateColumn(_gv1);     // 그리드뷰 생성
            GV2_CreateColumn(gvOutbound);     // 그리드뷰 생성
            GV1_LayoutSetting(_gv1);    // 중앙 그리드뷰 설정 
            GV2_LayoutSetting(gvOutbound);    // 중앙 그리드뷰 설정 
            Config_ContextMenu();       // 중앙 그리드뷰 컨텍스트 생성 설정 
            LoadGVLayout();             // 그리드뷰 레이아웃 복구 

            
        }

        #region 컨텍스트 메뉴 생성 및 제거 

        
        RadMenuItem mnuNew, mnuDel, mnuHide, mnuShow; 
        private void Form_Activated(object sender, EventArgs e)
        {
            Config_ContextMenu();
        }
        private void Form_Deactivate(object sender, EventArgs e)
        {
            mnuNew.Shortcuts.Clear();
            mnuDel.Shortcuts.Clear();
            mnuHide.Shortcuts.Clear();
            mnuShow.Shortcuts.Clear();
        }
        
        /// <summary>
        /// 그리드뷰 컨텍스트 메뉴 생성  
        /// </summary>
        private void Config_ContextMenu()
        {
            contextMenu = new RadContextMenu();

            // 오더 신규 입력
            mnuNew = new RadMenuItem("New Fabric IN");
            mnuNew.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.N));
            mnuNew.Click += new EventHandler(mnuNew_Click);

            // 오더 삭제
            //mnuDel = new RadMenuItem("Remove Inbound");
            //mnuDel.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.D));
            //mnuDel.Click += new EventHandler(mnuDel_Click);

            // 열 숨기기
            mnuHide = new RadMenuItem("Hide Column");
            mnuHide.Click += new EventHandler(mnuHide_Click);

            // 열 보이기
            mnuShow = new RadMenuItem("Show all Columns");
            mnuShow.Click += new EventHandler(mnuShow_Click);
            
            // 분리선
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            separator.Tag = "seperator";

            // 컨텍스트 추가 
            contextMenu.Items.Add(mnuNew);
            //contextMenu.Items.Add(mnuDel);
            contextMenu.Items.Add(separator);

            contextMenu.Items.Add(mnuHide);
            contextMenu.Items.Add(mnuShow);
            
        }

        #endregion

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
            ddlFabric.DataSource = dataSetSizeGroup.DataTableFabric2;
            ddlFabric.DisplayMember = "ShortName";
            ddlFabric.ValueMember = "Idx";
            ddlFabric.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ddlFabric.AutoSizeDropDownColumnMode = BestFitColumnMode.AllCells;
            ddlFabric.AutoSizeDropDownHeight = true;
            ddlFabric.DropDownStyle = RadDropDownStyle.DropDown;
            ddlFabric.AutoSizeDropDownToBestFit = true; 
            
            // 
            ddlFabricType.DataSource = lstFabricType;
            ddlFabricType.DisplayMember = "Contents";
            ddlFabricType.ValueMember = "CodeIdx";
            ddlFabricType.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlFabricType.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlFabricType.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            ddlOutMode.DataSource = lstOutStatus;
            ddlOutMode.DisplayMember = "Contents";
            ddlOutMode.ValueMember = "CodeIdx";
            ddlOutMode.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlOutMode.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlOutMode.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
            
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
            
            GridViewTextBoxColumn Idx = new GridViewTextBoxColumn();
            Idx.DataType = typeof(int);
            Idx.Name = "Idx";
            Idx.FieldName = "Idx";
            Idx.HeaderText = "ID";
            Idx.Width = 40;
            Idx.TextAlignment = ContentAlignment.MiddleLeft;
            gv.Columns.Add(Idx);

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
            // IDate.ReadOnly = true;
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

            GridViewMultiComboBoxColumn FabricIdx = new GridViewMultiComboBoxColumn();
            FabricIdx.Name = "FabricIdx";
            FabricIdx.DataSource = dataSetSizeGroup.DataTableFabric;
            FabricIdx.ValueMember = "Idx";
            FabricIdx.DisplayMember = "ShortName";
            FabricIdx.FieldName = "FabricIdx";
            FabricIdx.HeaderText = "Fabric";
            FabricIdx.AutoSizeMode = BestFitColumnMode.AllCells;
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
            Artno.TextAlignment = ContentAlignment.MiddleLeft;
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
            IODeptIdx.DataSource = lstDept2;
            IODeptIdx.DisplayMember = "CustName";
            IODeptIdx.ValueMember = "CustIdx";
            IODeptIdx.FieldName = "IODeptIdx";
            IODeptIdx.HeaderText = "IN From";
            IODeptIdx.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            IODeptIdx.DropDownStyle = RadDropDownStyle.DropDown;
            IODeptIdx.Width = 150;
            gv.Columns.Add(IODeptIdx);

            GridViewTextBoxColumn Comments = new GridViewTextBoxColumn();
            Comments.Name = "Comments";
            Comments.FieldName = "Comments";
            Comments.HeaderText = "Comments";
            Comments.TextAlignment = ContentAlignment.MiddleLeft;
            Comments.Width = 100;
            gv.Columns.Add(Comments);

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
            Qrcode.IsVisible = true;
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
            
            GridViewTextBoxColumn WorkOrderIdx = new GridViewTextBoxColumn();
            WorkOrderIdx.Name = "WorkOrderIdx";
            WorkOrderIdx.FieldName = "WorkOrderIdx";
            WorkOrderIdx.HeaderText = "In #";
            WorkOrderIdx.ReadOnly = true;
            WorkOrderIdx.TextAlignment = ContentAlignment.MiddleLeft;
            WorkOrderIdx.Width = 100;
            gv.Columns.Add(WorkOrderIdx);

            #endregion
        }

        /// <summary>
        /// 출고 그리드뷰 컬럼 생성
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV2_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            GridViewTextBoxColumn Idx = new GridViewTextBoxColumn();
            Idx.DataType = typeof(int);
            Idx.Name = "Idx";
            Idx.FieldName = "Idx";
            Idx.HeaderText = "ID";
            Idx.Width = 40;
            Idx.TextAlignment = ContentAlignment.MiddleLeft;
            gv.Columns.Add(Idx);

            GridViewComboBoxColumn Status = new GridViewComboBoxColumn();
            Status.Name = "Status";
            Status.DataSource = lstOutStatus2;
            Status.DisplayMember = "Contents";
            Status.ValueMember = "CodeIdx";
            Status.FieldName = "Status";
            Status.HeaderText = "Status";
            Status.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Status.DropDownStyle = RadDropDownStyle.DropDown;
            Status.Width = 140;
            gv.Columns.Add(Status);

            GridViewDateTimeColumn IDate = new GridViewDateTimeColumn();
            IDate.Name = "IDate";
            IDate.Width = 80;
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

            GridViewTextBoxColumn ColorIdx = new GridViewTextBoxColumn();
            ColorIdx.Name = "ColorIdx";
            ColorIdx.FieldName = "ColorIdx";
            ColorIdx.HeaderText = "Color";
            ColorIdx.Width = 170;
            ColorIdx.TextAlignment = ContentAlignment.MiddleLeft;
            ColorIdx.ReadOnly = true; 
            gv.Columns.Add(ColorIdx);

            //GridViewComboBoxColumn ColorIdx = new GridViewComboBoxColumn();
            //ColorIdx.Name = "ColorIdx";
            //ColorIdx.DataSource = lstColor2;
            //ColorIdx.DisplayMember = "Contents";
            //ColorIdx.ValueMember = "Contents";
            //ColorIdx.FieldName = "ColorIdx";
            //ColorIdx.HeaderText = "Color";
            //ColorIdx.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //ColorIdx.DropDownStyle = RadDropDownStyle.DropDown;
            //ColorIdx.Width = 170;
            //gv.Columns.Add(ColorIdx);

            GridViewTextBoxColumn FabricIdx = new GridViewTextBoxColumn();
            FabricIdx.Name = "FabricIdx";
            //FabricIdx.DataSource = lstFabric2;
            //FabricIdx.DisplayMember = "LongName";
            //FabricIdx.ValueMember = "Idx";
            FabricIdx.FieldName = "FabricNm";
            FabricIdx.HeaderText = "Fabric";
            //FabricIdx.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //FabricIdx.DropDownStyle = RadDropDownStyle.DropDown;
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

            GridViewTextBoxColumn IODeptIdx = new GridViewTextBoxColumn();
            IODeptIdx.Name = "IODeptIdx";
            IODeptIdx.FieldName = "DeptNm";
            IODeptIdx.HeaderText = "Out Place";
            IODeptIdx.Width = 150;
            gv.Columns.Add(IODeptIdx);

            GridViewTextBoxColumn OrderIdx = new GridViewTextBoxColumn();
            OrderIdx.Name = "OrderIdx";
            OrderIdx.FieldName = "OrderIdx";
            OrderIdx.HeaderText = "Order#";
            OrderIdx.Width = 60;
            gv.Columns.Add(OrderIdx);

            //GridViewComboBoxColumn Handler = new GridViewComboBoxColumn();
            //Handler.Name = "Handler";
            //Handler.DataSource = lstUser;
            //Handler.DisplayMember = "CustName";
            //Handler.ValueMember = "CustIdx";
            //Handler.FieldName = "Handler";
            //Handler.HeaderText = "Handler";
            //Handler.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //Handler.DropDownStyle = RadDropDownStyle.DropDown;
            //Handler.Width = 70;
            //gv.Columns.Add(Handler);

            GridViewHyperlinkColumn WorkOrderIdx = new GridViewHyperlinkColumn();
            WorkOrderIdx.Name = "WorkOrderIdx";
            WorkOrderIdx.FieldName = "WorkOrderIdx";
            WorkOrderIdx.Width = 100;
            WorkOrderIdx.TextAlignment = ContentAlignment.MiddleLeft;
            WorkOrderIdx.HeaderText = "Out #";
            gv.Columns.Add(WorkOrderIdx);
            
            GridViewTextBoxColumn Comments = new GridViewTextBoxColumn();
            Comments.Name = "Comments";
            Comments.FieldName = "Comments";
            Comments.HeaderText = "Comments";
            Comments.Width = 120;
            gv.Columns.Add(Comments);

            GridViewTextBoxColumn InIdx = new GridViewTextBoxColumn();
            InIdx.Name = "InIdx";
            InIdx.FieldName = "InIdx";
            InIdx.HeaderText = "In#";
            InIdx.IsVisible = false;
            InIdx.Width = 50;
            gv.Columns.Add(InIdx);

            GridViewTextBoxColumn IsOut = new GridViewTextBoxColumn();
            IsOut.Name = "IsOut";
            IsOut.FieldName = "IsOut";
            IsOut.HeaderText = "IsOut";
            IsOut.IsVisible = false;
            IsOut.Width = 50;
            gv.Columns.Add(IsOut);


            #endregion
        }

        #endregion

        #region 3. 컨트롤 초기 설정

        /// <summary>
        /// 컨텍스트 메뉴 연결 (행높이 설정)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.ContextMenu = contextMenu.DropDown;

            // 분리선이 아닌경우 행 높이 조정
            foreach (RadItem item in e.ContextMenu.Items)
            {
                if (item.Tag != null && item.Tag.ToString() == "seperator") { }
                else item.MinSize = new Size(0, 25);
            }
        }
        
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
        /// <summary>
        /// 그리드뷰 설정
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV2_LayoutSetting(RadGridView gv)
        {
            #region Config Gridview 

            //gv.Dock = DockStyle.Fill;
            //gv.AllowAddNewRow = false;
            //gv.AllowCellContextMenu = true;
            //gv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
            //gv.AllowColumnHeaderContextMenu = false;
            //gv.EnableGrouping = false;
            //gv.MasterView.TableHeaderRow.MinHeight = 50;

            //gv.GridViewElement.PagingPanelElement.NumericButtonsCount = 15;
            //gv.GridViewElement.PagingPanelElement.ShowFastBackButton = false;
            //gv.GridViewElement.PagingPanelElement.ShowFastForwardButton = false;

            //gv.MultiSelect = true;

            #endregion

            #region Config Cell Conditions: 상태에 따른 행스타일 변경

            // 캔슬오더 색상변경
            Font f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            ConditionalFormattingObject obj = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "3", "", true);
            obj.RowForeColor = Color.Blue;
            obj.RowBackColor = Color.White; 
            obj.RowFont = f;
            gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj);

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

        #region 4. 컨텍스트 메뉴 기능

        /// <summary>
        /// 선택된 컬럼 숨기기 (셀내에서 선택하도록 한다)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuHide_Click(object sender, EventArgs e)
        {
            try
            {
                _gv1.Columns[_gv1.CurrentColumn.FieldName].IsVisible = false;
                __main__.lblDescription.Text = "셀 위에서 우측 버튼을 눌러 숨기기하세요";
            }
            catch (Exception ex)
            {
                __main__.lblDescription.Text = ex.Message;
            }
        }

        /// <summary>
        /// 숨겨진 모든 컬럼 보이기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuShow_Click(object sender, EventArgs e)
        {
            foreach (GridViewColumn col in _gv1.Columns)
            {
                col.IsVisible = true;
            }
        }

        /// <summary>
        /// 행 자료삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDel_Click(object sender, EventArgs e)
        {
            try
            {
                string str = "";
                //_gv1.EndEdit();

                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                if (Convert.ToInt16(__AUTHCODE__.Substring(2, 1).Trim()) > 0)
                {
                    // 삭제하기전 삭제될 데이터 확인
                    foreach (GridViewRowInfo row in _gv1.SelectedRows)
                    {
                        if (string.IsNullOrEmpty(str)) str = Convert.ToInt32(row.Cells["Idx"].Value).ToString();
                        else str += "," + Convert.ToInt32(row.Cells["Idx"].Value).ToString();
                    }

                    // 해당 자료 삭제여부 확인후, 
                    if (RadMessageBox.Show("Do you want to delete this item?\n(ID: " + str + ")", "Confirm",
                        MessageBoxButtons.YesNo, RadMessageIcon.Question) == DialogResult.Yes)
                    {
                        // 삭제후 새로고침
                        _bRtn = Controller.Inbound.Delete(str);
                        RefleshWithCondition();
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnInsert_Click: " + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 자료 새로입력 (로그인 사용자의 부서번호로 기본 입력된다) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuNew_Click(object sender, EventArgs e)
        {
            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                if (Convert.ToInt16(__AUTHCODE__.Substring(1, 1).Trim()) > 0)
                {
                    string WorkOrderIdx = Int.Code.Code.GetPrimaryCode(UserInfo.CenterIdx, UserInfo.DeptIdx, 14, "");           // 입고번호 생성 
                                                                                                                                // 입고번호 생성과 동시에 QR코드 생성
                    string __QRCode__ = WorkOrderIdx.Trim(); //Int.Encryptor.Encrypt(WorkOrderIdx.Trim(), "love1229");
                    DataRow row = Controller.Inbound.Insert(WorkOrderIdx, __QRCode__, UserInfo.CenterIdx, UserInfo.DeptIdx, UserInfo.Idx);  // 신규 입력
                    RefleshWithCondition();                                                                                     // 재조회 
                    SetCurrentRow(_gv1, Convert.ToInt32(row["LastIdx"]));                                                       // 신규입력된 행번호로 이동
                }
                
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 신규자료가 입력된후, 입력된 행을 선택 포커싱한다 
        /// </summary>
        /// <param name="gv">선택하고자하는 그리드뷰</param>
        /// <param name="row">선택 행(Insert시 신규Idx를 리턴받는다)</param>
        private void SetCurrentRow(RadGridView gv, int row)
        {
            try
            {
                row = 0;
                Console.WriteLine("SetRow " + row.ToString()); 
                // 페이징 되어 있는 경우, 제일 첫페이지로 이동한후, 
                if (CommonValues.enablePaging == true)
                {
                    gv.MasterTemplate.MoveToFirstPage();
                }
                gv.CurrentRow = gv.Rows[row];
                if (gv.CurrentRow != null)
                {
                    gv.CurrentRow.IsSelected = true;
                }

            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);

            }
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
            lstOutStatus.Add(new CodeContents(3, CommonValues.DicFabricOutStatus[3], ""));
            lstOutStatus.Add(new CodeContents(5, CommonValues.DicFabricOutStatus[5], ""));
            lstOutStatus.Add(new CodeContents(6, CommonValues.DicFabricOutStatus[6], ""));
            lstOutStatus.Add(new CodeContents(7, CommonValues.DicFabricOutStatus[7], ""));
            lstOutStatus.Add(new CodeContents(8, CommonValues.DicFabricOutStatus[8], ""));
            lstOutStatus.Add(new CodeContents(11, CommonValues.DicFabricOutStatus[11], ""));

            lstOutStatus2.Add(new CodeContents(0, CommonValues.DicFabricOutStatus[0], ""));
            lstOutStatus2.Add(new CodeContents(3, CommonValues.DicFabricOutStatus[3], ""));
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
                lstDept.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]), row["CustName"].ToString(), Convert.ToInt32(row["Classification"])));
                lstDept2.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]), row["CustName"].ToString(), Convert.ToInt32(row["Classification"])));
            }

            // Fabric 
            //_searchString = new Dictionary<CommonValues.KeyName, string>();
            //_searchString.Add(CommonValues.KeyName.Remark, "");
            //_searchString.Add(CommonValues.KeyName.IsUse, "1");
            //_dt = Controller.Fabric.Getlist(_searchString).Tables[0];

            //lstFabric.Add(new Controller.Fabric(0, "", ""));
            //lstFabric2.Add(new Controller.Fabric(0, "", ""));
            //foreach (DataRow row in _dt.Rows)
            //{
            //    lstFabric.Add(new Controller.Fabric(Convert.ToInt32(row["Idx"]),
            //                                row["LongName"].ToString(),
            //                                row["ShortName"].ToString()));
            //    lstFabric2.Add(new Controller.Fabric(Convert.ToInt32(row["Idx"]),
            //                                row["LongName"].ToString(),
            //                                row["ShortName"].ToString()));
            //}

            // Fabric 
            _searchString = new Dictionary<CommonValues.KeyName, string>();
            _searchString.Add(CommonValues.KeyName.Remark, "");
            _searchString.Add(CommonValues.KeyName.IsUse, "1");
            _dt = Dev.Controller.Fabric.Getlist(_searchString).Tables[0];

            dataSetSizeGroup.DataTableFabric.Rows.Add(0, "", "", "", 0f, "", 0f, "", 0f, "", 0f, "", 0f);
            dataSetSizeGroup.DataTableFabric2.Rows.Add(0, "", "", "", 0f, "", 0f, "", 0f, "", 0f, "", 0f);
            foreach (DataRow row in _dt.Rows)
            {
                dataSetSizeGroup.DataTableFabric.Rows.Add(Convert.ToInt32(row["Idx"]),
                                            row["LongName"].ToString(), row["ShortName"].ToString(),
                                            row["Yarnnm1"] == DBNull.Value ? "" : row["Yarnnm1"].ToString(), row["Percent1"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent1"]),
                                            row["Yarnnm2"] == DBNull.Value ? "" : row["Yarnnm2"].ToString(), row["Percent2"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent2"]),
                                            row["Yarnnm3"] == DBNull.Value ? "" : row["Yarnnm3"].ToString(), row["Percent3"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent3"]),
                                            row["Yarnnm4"] == DBNull.Value ? "" : row["Yarnnm4"].ToString(), row["Percent4"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent4"]),
                                            row["Yarnnm5"] == DBNull.Value ? "" : row["Yarnnm5"].ToString(), row["Percent5"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent5"])
                                            );

                dataSetSizeGroup.DataTableFabric2.Rows.Add(Convert.ToInt32(row["Idx"]),
                                            row["LongName"].ToString(), row["ShortName"].ToString(),
                                            row["Yarnnm1"] == DBNull.Value ? "" : row["Yarnnm1"].ToString(), row["Percent1"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent1"]),
                                            row["Yarnnm2"] == DBNull.Value ? "" : row["Yarnnm2"].ToString(), row["Percent2"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent2"]),
                                            row["Yarnnm3"] == DBNull.Value ? "" : row["Yarnnm3"].ToString(), row["Percent3"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent3"]),
                                            row["Yarnnm4"] == DBNull.Value ? "" : row["Yarnnm4"].ToString(), row["Percent4"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent4"]),
                                            row["Yarnnm5"] == DBNull.Value ? "" : row["Yarnnm5"].ToString(), row["Percent5"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent5"])
                                            );
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
                    
                    CultureInfo ci = new CultureInfo("ko-KR");
                    Console.WriteLine(dtInboundDate.Value.ToString("d",ci)); 

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
                    _ds1 = Controller.Inbound.Getlist(_searchString, _searchKey, dtInboundDate.Value.ToString("d", ci));

                    if (_ds1 != null)
                    {
                        _gv1.DataSource = _ds1.Tables[0].DefaultView;
                        __main__.lblRows.Text = _gv1.RowCount.ToString() + " Rows";
                        _gv1.EnablePaging = CommonValues.enablePaging;
                        _gv1.AllowSearchRow = CommonValues.enableSearchRow;
                    }
                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBindingGv1: " + ex.Message.ToString());
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
            _bRtn = false;
            try
            {
                _gv1.EndEdit();
                GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);  // 현재 행번호 확인

                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                int _mode_ = 1;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    // 객체생성 및 값 할당
                    _obj1 = new Controller.Inbound(Convert.ToInt32(row.Cells["Idx"].Value));
                    _obj1.Idx = Convert.ToInt32(row.Cells["Idx"].Value);

                    if (row.Cells["Status"].Value != DBNull.Value) _obj1.Status = Convert.ToInt32(row.Cells["Status"].Value);
                    if (row.Cells["IDate"].Value != DBNull.Value) _obj1.IDate = Convert.ToDateTime(row.Cells["IDate"].Value);
                    if (row.Cells["BuyerIdx"].Value != DBNull.Value) _obj1.BuyerIdx = Convert.ToInt32(row.Cells["BuyerIdx"].Value);
                    if (row.Cells["ColorIdx"].Value != DBNull.Value) _obj1.ColorIdx = Convert.ToString(row.Cells["ColorIdx"].Value);
                    if (row.Cells["FabricType"].Value != DBNull.Value) _obj1.FabricType = Convert.ToInt32(row.Cells["FabricType"].Value);
                    if (row.Cells["Artno"].Value != DBNull.Value) _obj1.Artno = row.Cells["Artno"].Value.ToString(); else _obj1.Artno = "";
                    if (row.Cells["Lotno"].Value != DBNull.Value) _obj1.Lotno = row.Cells["Lotno"].Value.ToString(); else _obj1.Lotno = "";
                    if (row.Cells["FabricIdx"].Value != DBNull.Value) _obj1.FabricIdx = Convert.ToInt32(row.Cells["FabricIdx"].Value);
                    if (row.Cells["Roll"].Value != DBNull.Value) _obj1.Roll = Convert.ToInt32(row.Cells["Roll"].Value);
                    if (row.Cells["Width"].Value != DBNull.Value) _obj1.Width = Convert.ToInt32(row.Cells["Width"].Value);

                    if (row.Cells["Kgs"].Value != DBNull.Value) _obj1.Kgs = Convert.ToDouble(row.Cells["Kgs"].Value);
                    if (row.Cells["Yds"].Value != DBNull.Value) _obj1.Yds = Convert.ToDouble(row.Cells["Yds"].Value);
                    //if (row.Cells["RegCenterIdx"].Value != DBNull.Value) _regCenterIdx = Convert.ToInt32(row.Cells["RegCenterIdx"]);
                    //if (row.Cells["RegDeptIdx"].Value != DBNull.Value) _regDeptIdx = Convert.ToInt32(row.Cells["RegDeptIdx"]);
                    //if (row.Cells["RegUserIdx"].Value != DBNull.Value) _regUserIdx = Convert.ToInt32(row.Cells["RegUserIdx"]);
                    //if (row.Cells["RegDate"].Value != DBNull.Value) _regDate = Convert.ToDateTime(row.Cells["RegDate"]);
                    if (row.Cells["IOCenterIdx"].Value != DBNull.Value) _obj1.IOCenterIdx = Convert.ToInt32(row.Cells["IOCenterIdx"].Value);
                    if (row.Cells["IODeptIdx"].Value != DBNull.Value) _obj1.IODeptIdx = Convert.ToInt32(row.Cells["IODeptIdx"].Value);
                    if (row.Cells["Comments"].Value != DBNull.Value) _obj1.Comments = row.Cells["Comments"].Value.ToString(); else _obj1.Comments = "";

                    if (row.Cells["WorkOrderIdx"].Value != DBNull.Value) _obj1.WorkOrderIdx = row.Cells["WorkOrderIdx"].Value.ToString();
                    if (row.Cells["RackNo"].Value != DBNull.Value) _obj1.RackNo = Convert.ToInt32(row.Cells["RackNo"].Value); else _obj1.RackNo = 0;
                    if (row.Cells["Floorno"].Value != DBNull.Value) _obj1.Floorno = Convert.ToInt32(row.Cells["Floorno"].Value); else _obj1.Floorno = 0;
                    if (row.Cells["RackPos"].Value != DBNull.Value) _obj1.RackPos = Convert.ToInt32(row.Cells["RackPos"].Value); else _obj1.RackPos = 0;
                    if (row.Cells["PosX"].Value != DBNull.Value) _obj1.PosX = Convert.ToInt32(row.Cells["PosX"].Value); else _obj1.PosX = 0;
                    if (row.Cells["PosY"].Value != DBNull.Value) _obj1.PosY = Convert.ToInt32(row.Cells["PosY"].Value); else _obj1.PosY = 0;
                    if (row.Cells["Qrcode"].Value != DBNull.Value) _obj1.Qrcode = row.Cells["Qrcode"].Value.ToString(); else _obj1.Qrcode = "";

                    if (row.Cells["filenm1"].Value != DBNull.Value) _obj1.Filenm1 = row.Cells["filenm1"].ToString(); else _obj1.Filenm1 = "";
                    if (row.Cells["filenm2"].Value != DBNull.Value) _obj1.Filenm2 = row.Cells["filenm2"].ToString(); else _obj1.Filenm2 = "";
                    if (row.Cells["fileurl1"].Value != DBNull.Value) _obj1.Fileurl1 = row.Cells["fileurl1"].ToString(); else _obj1.Fileurl1 = "";
                    if (row.Cells["fileurl2"].Value != DBNull.Value) _obj1.Fileurl2 = row.Cells["fileurl2"].ToString(); else _obj1.Fileurl2 = "";

                    _bRtn = _obj1.Update();
                    if (_bRtn) __main__.lblDescription.Text = "Update Succeed";
                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine("GV1_Update: " + ex.Message.ToString());
            }
            
        }
        
        /// <summary>
        /// 그리드뷰 셀포맷팅 (각 이벤트시 수시작동) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font f = new Font(new FontFamily("Segoe UI"), 8.25f);

            // 중앙 그리드뷰 헤더 폰트 변경
            GridHeaderCellElement element = sender as GridHeaderCellElement;
            if (element != null)
            {
                element.Font = f;

                if (e.CellElement.ColumnInfo.Name == "RackNo" || e.CellElement.ColumnInfo.Name == "Floorno" || e.CellElement.ColumnInfo.Name == "RackPos" ||
                    e.CellElement.ColumnInfo.Name == "PosX" || e.CellElement.ColumnInfo.Name == "PosY") 
                {
                    e.CellElement.ForeColor = Color.Blue;
                    e.CellElement.BackColor = Color.LightYellow;
                }
                else
                {
                    e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
                    e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                }
            }
            
        }

        /// <summary>
        /// 그리드뷰 셀 생성시 DDL의 높이,출력항목수 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterTemplate_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadMultiColumnComboBoxElement meditor = e.ActiveEditor as RadMultiColumnComboBoxElement;

            if (_gv1.RowCount > 0)
            {
                if (meditor != null)
                {
                    meditor.Enabled = true;
                    meditor.AutoSizeDropDownToBestFit = true;
                    meditor.AutoSizeDropDownHeight = false;
                    meditor.DropDownHeight = 400;

                    if (meditor.ValueMember == "FabricIdx")
                    {
                        meditor.AutoSizeDropDownColumnMode = BestFitColumnMode.AllCells;
                        meditor.EditorControl.Columns["Idx"].HeaderText = "ID";
                        meditor.EditorControl.Columns["ShortName"].HeaderText = "Fabric";
                        //meditor.EditorControl.Columns["ShortName"].Width = 150;
                        meditor.EditorControl.Columns["Yarnnm1"].HeaderText = "Yarn1";
                        meditor.EditorControl.Columns["Percent1"].HeaderText = "%";
                        meditor.EditorControl.Columns["Yarnnm2"].HeaderText = "Yarn2";
                        meditor.EditorControl.Columns["Percent2"].HeaderText = "%";
                        meditor.EditorControl.Columns["Yarnnm3"].HeaderText = "Yarn3";
                        meditor.EditorControl.Columns["Percent3"].HeaderText = "%";
                        meditor.EditorControl.Columns["Yarnnm4"].HeaderText = "Yarn4";
                        meditor.EditorControl.Columns["Percent4"].HeaderText = "%";
                        meditor.EditorControl.Columns["Yarnnm5"].HeaderText = "Yarn5";
                        meditor.EditorControl.Columns["Percent5"].HeaderText = "%";

                    }

                }
            }

            // DDL 높이, 출력항목수 설정
            RadDropDownListEditor editor = this._gv1.ActiveEditor as RadDropDownListEditor;
            if (editor != null)
            {
                ((RadDropDownListEditorElement)((RadDropDownListEditor)this._gv1.ActiveEditor).EditorElement).DefaultItemsCountInDropDown
                    = CommonValues.DDL_DefaultItemsCountInDropDown;
                ((RadDropDownListEditorElement)((RadDropDownListEditor)this._gv1.ActiveEditor).EditorElement).DropDownHeight
                    = CommonValues.DDL_DropDownHeight;
                ((RadDropDownListEditorElement)((RadDropDownListEditor)this._gv1.ActiveEditor).EditorElement).DropDownWidth
                    = ((RadDropDownListEditorElement)((RadDropDownListEditor)this._gv1.ActiveEditor).EditorElement).DropDownWidth * CommonValues.DDL_DropDownWidth; 
            }

            // 날짜컬럼의 달력크기 설정
            RadDateTimeEditor dtEditor = e.ActiveEditor as RadDateTimeEditor;
            if (dtEditor != null)
            {
                RadDateTimeEditorElement el = dtEditor.EditorElement as RadDateTimeEditorElement;
                el.CalendarSize = new Size(500, 400);
            }

        }
        
        /// <summary>
        /// 현재 윈도우 종료시 그리드뷰 레이아웃 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmOrderActual_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveGVLayout(); 
        }

        /// <summary>
        /// 그리드뷰 행 클릭 (다중선택시 오더복사 disable)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_Click(object sender, EventArgs e)
        {
            // 멀티행 선택시 컨텍스트 메뉴에서 오더복사 disable
            //if (_gv1.SelectedRows.Count > 1)
            //{
            //    contextMenu.Items[6].Enabled = false;
            //    contextMenu.Items[6].Shortcuts.Clear();
            //}
            //else
            //{
            //    contextMenu.Items[6].Enabled = true;
            //    contextMenu.Items[6].Shortcuts.Clear();
            //    contextMenu.Items[6].Shortcuts.Add(new RadShortcut(Keys.Control, Keys.C));
            //}
        }

        private void ddlBuyer_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {

        }

        private void gvMain_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            
        }

        private void lvQRCode_VisualItemFormatting(object sender, ListViewVisualItemEventArgs e)
        {
            
        }

        /// <summary>
        /// 읽어드린 QR가 있을때만 저장버튼 활성화 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvQRCode_ItemValueChanged(object sender, ListViewItemValueChangedEventArgs e)
        {
            if (lvQRCode.Items.Count <= 0)
            {
                btnSaveData.Enabled = false;
            }
            else
            {
                btnSaveData.Enabled = true;
            }
        }

        /// <summary>
        /// 코드를 읽을때마다 코드 유효 체크
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            string CodeType, WorkOrderIdx = "";
            
            try
            {
                if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtBarcode.Text.Trim()))
                {
                    // Location 코드체크 (랙인지만 확인, 암호화 안함)
                    if (txtBarcode.Text.Trim().Substring(0, 3).ToUpper() == "LOC")
                    {
                        CodeType = "Location";
                        WorkOrderIdx = txtBarcode.Text.Trim();
                        lvQRCode.Items.Add(CodeType, WorkOrderIdx, "");
                    }
                    // 원단 코드체크 (암호화, 코드길이, 종류확인) 
                    else
                    {
                        CodeType = "Fabric";
                        // 복호화
                        WorkOrderIdx = txtBarcode.Text.Trim(); //.Replace("-", "/").Replace("`", "-"); //Decryptor(txtBarcode.Text.Trim());
                        WorkOrderIdx = WorkOrderIdx.Substring(0, 3) + "-" + WorkOrderIdx.Substring(4); 
                        //WorkOrderIdx.Replace("`", "-"); 
                        //Console.WriteLine(CodeType + " === " + txtBarcode.Text.Trim() + " === " + WorkOrderIdx); 
                        // 값이없으면
                        if (string.IsNullOrEmpty(WorkOrderIdx.Trim())) { }
                        // 길이가 14, 12가 아니면
                        else if (WorkOrderIdx.Length != 14 && WorkOrderIdx.Length != 12) { }
                        // I로 시작하지 않으면 
                        else if (WorkOrderIdx.Substring(0, 1) != "I") { }
                        // 쿼리생성 
                        else
                        {
                            
                            lvQRCode.Items.Add(CodeType, WorkOrderIdx, "");
                        }
                    }
                    btnSaveData.Enabled = true;
                    txtBarcode.Text = "";
                    txtBarcode.Select();
                }
                else
                {
                    //Console.WriteLine(e.KeyCode.ToString());
                }
            }
            catch(Exception ex)
            {

            }
        }
        
        /// <summary>
        /// 입출고 작업진행 선택 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toggleWay_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                int _mode_ = 1;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    // 컨트롤 상태 초기화 
                    ControlStateReset();
                    lvQRCode.Items.Clear(); 

                    // Inbound
                    if (toggleWay.Value)
                    {
                        btnSaveData.Enabled = false;
                        btnSaveData.Text = "Fabric IN"; 
                        radPanel1.Enabled = false;          // 출고모드, 센터, 부서선택 
                        tableLayoutPanel7.Enabled = false;  // 작업지시시서 조회란
                        toggReadMode.Enabled = false;

                        txtBarcode.Select();
                    }
                    // Outbound
                    else
                    {
                        btnSaveData.Enabled = false;
                        btnSaveData.Text = "Fabric OUT";
                        radPanel1.Enabled = true;          // 출고모드, 센터, 부서선택 
                        tableLayoutPanel7.Enabled = true;  // 작업지시시서 조회란
                        toggReadMode.Enabled = true;
                        ddlOutMode.SelectedValue = 5; 

                        txtBarcode.Select();
                    }
                }
                    
            }
            catch(Exception ex)
            {

            }
            
        }

        /// <summary>
        /// 버튼 상태초기화 
        /// </summary>
        private void ControlStateReset()
        {
            btnSaveData.Enabled = false;
            radPanel1.Enabled = false;          // 출고모드, 센터, 부서선택 
            tableLayoutPanel7.Enabled = false;  // 작업지시시서 조회란
            toggReadMode.Enabled = false; 
        }

        /// <summary>
        /// 저장버튼을 눌렀을때 입고, 출고(스캔), 출고(수정) 옵션에 따른 처리 
        /// </summary>
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            string Category = "", Code = "";
            string startCategory="", startCode = "";
            string FabricCode = "", LocationCode = "";
            string WorkSheetIdx = "";

            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                int _mode_ = 1;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    #region Inbound Mode
                    // Inbound
                    if (toggleWay.Value)
                    {
                        string temp = "";

                        if (lvQRCode.Items.Count > 0)
                        {
                            // 연속으로 중복된 코드제거 
                            foreach (ListViewDataItem item in lvQRCode.Items)
                            {
                                if (item[1].ToString() == temp)
                                {
                                    item[2] = "D";
                                    // continue; 
                                }
                                else
                                {
                                    item[2] = "";
                                    temp = item[1].ToString();
                                }
                            }

                            foreach (ListViewDataItem item in lvQRCode.Items)
                            {
                                // 중복되지 않은 코드라면 정상처리 
                                if (item[2].ToString() != "D")
                                {
                                    Category = item[0].ToString();
                                    Code = item[1].ToString();

                                    // 처음 들어오는 코드일때
                                    if (string.IsNullOrEmpty(startCategory) || string.IsNullOrEmpty(startCode))
                                    {
                                        // 시작코드만 저장하고 
                                        startCategory = item[0].ToString().Trim();
                                        startCode = item[1].ToString().Trim();
                                    }
                                    // 두번째 들어오는 코드일때 
                                    else
                                    {
                                        // 같은 종류의 코드가 연속되면 경고메시지를 출력후, 마지막에 읽어드린 코드로 진행한다
                                        if (startCategory == item[0].ToString().Trim())
                                        {
                                            //RadMessageBox.Show("[" + startCode + "] and [" + item[1].ToString().Trim() + "] are the same category.\n" +
                                            //    "The system will process with the last code [" + item[1].ToString().Trim() + ".", "Warning",
                                            //    MessageBoxButtons.OK, RadMessageIcon.Exclamation);

                                            // 시작코드 저장후 다음 행 이동 
                                            startCategory = item[0].ToString().Trim();
                                            startCode = item[1].ToString().Trim();
                                        }
                                        else
                                        {
                                            // 원단정보가 먼저 스캔되었다면 
                                            if (startCategory == "Fabric")
                                            {
                                                FabricCode = startCode;
                                                LocationCode = item[1].ToString().Trim();
                                            }
                                            // 적재위치정보가 먼저 스캔되었다면 
                                            else if (startCategory == "Location")
                                            {
                                                FabricCode = item[1].ToString().Trim();
                                                LocationCode = startCode;
                                            }

                                            string[] codeStr = LocationCode.Split(',');
                                            if (codeStr.Length > 0)
                                            {
                                                // 입고번호를 통해 적재위치 갱신
                                                bool result = Controller.Inbound.UpdateIn(FabricCode, DateTime.Now, Convert.ToInt32(codeStr[7].Trim()),
                                                                                            Convert.ToInt32(codeStr[8].Trim()), Convert.ToInt32(codeStr[9].Trim()),
                                                                                            Convert.ToInt32(codeStr[5].Trim()), Convert.ToInt32(codeStr[6].Trim()));
                                            }
                                        }
                                    }

                                }
                                __main__.lblDescription.Text = "It's saved the Fabric Inbound data. Please review the data.";
                                dtInboundDate.Value = Convert.ToDateTime(DateTime.Today);
                                RefleshWithCondition();
                            }
                        }
                    }
                    #endregion

                    #region Outbound Mode 
                    // Outbound
                    else
                    {
                        string WorkOrderIdx = "";

                        // 선택된 DDL 확인 
                        if (this.ddlOutMode.SelectedIndex <= -1)
                        {
                            RadMessageBox.Show("Please select the outbound mode.", "Info.", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                            return;
                        }

                        if (this.ddlDept.SelectedIndex <= -1)
                        {
                            RadMessageBox.Show("Please select the outbound place.", "Info.", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                            return;
                        }
                        
                        int codeOutMode = Convert.ToInt32(ddlOutMode.Items[ddlOutMode.SelectedIndex].Value);
                        int codeCenter = 0;
                        if (this.ddlDept.SelectedIndex > -1)
                        {
                            codeCenter = Convert.ToInt32(ddlCenter.Items[ddlCenter.SelectedIndex].Value);
                        }

                        int codeDept = Convert.ToInt32(ddlDept.Items[ddlDept.SelectedIndex].Value);


                        #region 스캔 출고

                        // Read Scan
                        if (toggReadMode.Value)
                        {
                            string temp = "";
                            
                            // 스캔된 리스트가 있는지 확인 
                            if (lvQRCode.Items.Count > 0)
                            {
                                // 연속으로 중복된 코드제거 
                                foreach (ListViewDataItem item in lvQRCode.Items)
                                {
                                    if (item[1].ToString() == temp)
                                    {
                                        item[2] = "D";
                                    }
                                    else
                                    {
                                        item[2] = "";
                                        temp = item[1].ToString();
                                    }
                                }

                                foreach (ListViewDataItem item in lvQRCode.Items)
                                {
                                    // 중복되지 않은 코드라면 정상처리 
                                    if (item[2].ToString() != "D")
                                    {
                                        Category = item[0].ToString();
                                        Code = item[1].ToString();

                                        // 처음 들어오는 코드일때
                                        if (string.IsNullOrEmpty(startCategory) || string.IsNullOrEmpty(startCode))
                                        {
                                            // 시작코드만 저장하고 
                                            startCategory = item[0].ToString().Trim();
                                            startCode = item[1].ToString().Trim();
                                        }
                                        // 두번째 들어오는 코드일때 
                                        else
                                        {
                                            // 같은 종류의 코드가 연속되면 경고메시지를 출력후, 마지막에 읽어드린 코드로 진행한다
                                            if (startCategory == item[0].ToString().Trim())
                                            {
                                                //RadMessageBox.Show("[" + startCode + "] and [" + item[1].ToString().Trim() + "] are the same category.\n" +
                                                //    "The system will process with the last code [" + item[1].ToString().Trim() + ".", "Warning",
                                                //    MessageBoxButtons.OK, RadMessageIcon.Exclamation);

                                                // 시작코드 저장후 다음 행 이동 
                                                startCategory = item[0].ToString().Trim();
                                                startCode = item[1].ToString().Trim();
                                            }
                                            else
                                            {
                                                // 원단정보가 먼저 스캔되었다면 
                                                if (startCategory == "Fabric")
                                                {
                                                    FabricCode = startCode;
                                                    WorkSheetIdx = item[1].ToString().Trim();
                                                }
                                                // 작업지시정보가 먼저 스캔되었다면 
                                                else if (startCategory == "WorkSheet")
                                                {
                                                    FabricCode = item[1].ToString().Trim();
                                                    WorkSheetIdx = startCode;
                                                }
                                                
                                                if (!string.IsNullOrEmpty(FabricCode) && !string.IsNullOrEmpty(WorkSheetIdx))
                                                {
                                                    WorkOrderIdx = Int.Code.Code.GetPrimaryCode(UserInfo.CenterIdx, UserInfo.DeptIdx, 11, "");           // 출고번호 생성

                                                    // 출고내역 등록
                                                    DataRow irow = Controller.Outbound.Insert(FabricCode,
                                                                        codeOutMode, UserInfo.CenterIdx, UserInfo.DeptIdx, UserInfo.Idx,
                                                                        codeCenter, codeDept, 0, WorkOrderIdx);
                                                    
                                                }
                                            }
                                        }
                                    }
                                }
                                
                                __main__.lblDescription.Text = "It's saved the Fabric Outbound data. Please review the data.";

                                CommonController.Close_All_Children(this, "Outbound");
                                CultureInfo ci = new CultureInfo("ko-KR");
                                Outbound form = new Outbound(__main__, DateTime.Today.ToString("d", ci), "");
                                form.Text = DateTime.Now.ToLongTimeString();
                                form.MdiParent = this.MdiParent;
                                form.Show();
                            }
                            
                        }
                        #endregion

                        #region 수동 출고

                        else
                        {
                            // 선택된 입고건이 있는지 확인
                            if (gvMain.SelectedRows.Count > 0)
                            {
                                
                                WorkOrderIdx = Int.Code.Code.GetPrimaryCode(UserInfo.CenterIdx, UserInfo.DeptIdx, 11, "");           // 출고번호 생성 
                                
                                gvMain.EndEdit();
                                GridViewRowInfo row = Int.Members.GetCurrentRow(gvMain);  // 현재 행번호 확인

                                DataRow irow = Controller.Outbound.Insert(Convert.ToString(row.Cells["WorkOrderIdx"].Value),
                                                                        codeOutMode, UserInfo.CenterIdx, UserInfo.DeptIdx, UserInfo.Idx,
                                                                        codeCenter, codeDept, 0, WorkOrderIdx);

                                __main__.lblDescription.Text = "In#" + Convert.ToString(row.Cells["WorkOrderIdx"].Value) + " is shipped out to Out#" + WorkOrderIdx;

                                GridViewRowInfo orow = Int.Members.GetCurrentRow(_gv1);

                                // 시작시 초기 날짜가 있는 경우, 데이터 즉시 조회
                                if (Convert.ToInt32(orow.Cells["Idx"].Value) > 0)
                                {
                                    try
                                    {
                                        gvOutbound.DataSource = null;
                                                                                
                                        _searchString = new Dictionary<CommonValues.KeyName, string>();
                                        _searchString.Add(CommonValues.KeyName.Lotno, "");
                                        _searchString.Add(CommonValues.KeyName.WorkOrderIdx, "");
                                        _searchString.Add(CommonValues.KeyName.ColorIdx, "");

                                        _searchKey = new Dictionary<CommonValues.KeyName, int>();
                                        _searchKey.Add(CommonValues.KeyName.Status, 0);
                                        _searchKey.Add(CommonValues.KeyName.BuyerIdx, 0);

                                        _searchKey.Add(CommonValues.KeyName.FabricIdx, 0);
                                        _searchKey.Add(CommonValues.KeyName.FabricType, 0);

                                        _searchKey.Add(CommonValues.KeyName.InIdx, Convert.ToInt32(orow.Cells["Idx"].Value));
                                        _searchKey.Add(CommonValues.KeyName.Handler, 0);
                                        _searchKey.Add(CommonValues.KeyName.OrderIdx, 0);

                                        CultureInfo ci = new CultureInfo("ko-KR");
                                        _ds1 = Controller.Outbound.Getlist(_searchString, _searchKey, "");

                                        if (_ds1 != null)
                                        {
                                            gvOutbound.DataSource = _ds1.Tables[0].DefaultView;
                                            gvOutbound.EnablePaging = CommonValues.enablePaging;
                                            gvOutbound.AllowSearchRow = CommonValues.enableSearchRow;
                                        }
                                        
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("DataBindingGv1: " + ex.Message.ToString());
                                    }
                                }
                                AddSummaryRows();

                            }
                            else
                            {
                                RadMessageBox.Show("Please select a fabric inbound item.", "Info");
                                return; 
                            }
                        }
                        #endregion
                    }
                    
                    #endregion
                }
                    
            }
            catch (Exception ex)
            {
                __main__.lblDescription.Text = ex.Message;
            }
        }


        private void lvQRCode_Click(object sender, EventArgs e)
        {
            txtBarcode.Select();
        }

        /// <summary>
        /// 행 선택시 - 오더캔슬, 마감일때 편집 불가능하도록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_SelectionChanged(object sender, EventArgs e)
        {
            CommonValues.ListWorkID.Clear();
            CommonValues.WorkOperation = "Fabric";

            // 선택된 모든 행에 대해 
            foreach (GridViewRowInfo row in _gv1.SelectedRows)
            {
                if (string.IsNullOrEmpty(row.Cells["Qrcode"].Value.ToString().Trim()))
                {
                }
                else
                {
                    //Console.WriteLine(row.Cells["WorkOrderIdx"].Value.ToString().Trim()); 
                    CommonValues.ListWorkID.Add(row.Cells["WorkOrderIdx"].Value.ToString().Trim());
                }
            }

            GridViewRowInfo orow = Int.Members.GetCurrentRow(_gv1); 

            // 시작시 초기 날짜가 있는 경우, 데이터 즉시 조회
            if (Convert.ToInt32(orow.Cells["Idx"].Value)>0)
            {
                try
                {
                    gvOutbound.DataSource = null;

                    /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                    /// 읽기: 0, 쓰기: 1, 삭제: 2
                    int _mode_ = 0;
                    if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                        CheckAuth.ShowMessage(_mode_);
                    else
                    {
                        _searchString = new Dictionary<CommonValues.KeyName, string>();
                        _searchString.Add(CommonValues.KeyName.Lotno, "");
                        _searchString.Add(CommonValues.KeyName.WorkOrderIdx, "");
                        _searchString.Add(CommonValues.KeyName.ColorIdx, "");

                        _searchKey = new Dictionary<CommonValues.KeyName, int>();
                        _searchKey.Add(CommonValues.KeyName.Status, 0);
                        _searchKey.Add(CommonValues.KeyName.BuyerIdx, 0);
                        
                        _searchKey.Add(CommonValues.KeyName.FabricIdx, 0);
                        _searchKey.Add(CommonValues.KeyName.FabricType, 0);

                        _searchKey.Add(CommonValues.KeyName.InIdx, Convert.ToInt32(orow.Cells["Idx"].Value));
                        _searchKey.Add(CommonValues.KeyName.Handler, 0);
                        _searchKey.Add(CommonValues.KeyName.OrderIdx, 0);

                        CultureInfo ci = new CultureInfo("ko-KR");
                        _ds1 = Controller.Outbound.Getlist(_searchString, _searchKey, "");
                        
                        if (_ds1 != null)
                        {
                            gvOutbound.DataSource = _ds1.Tables[0].DefaultView;
                            gvOutbound.EnablePaging = CommonValues.enablePaging;
                            gvOutbound.AllowSearchRow = CommonValues.enableSearchRow;
                            
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DataBindingGv1: " + ex.Message.ToString());
                }
            }
            AddSummaryRows();
        }

        private void radLabel12_DoubleClick(object sender, EventArgs e)
        {
            dtInboundDate.Value = Convert.ToDateTime("2000-01-01"); 
        }

        /// <summary>
        /// 출고 모드 선택
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toggReadMode_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Read Scan
                if (toggReadMode.Value)
                {
                    btnSaveData.Enabled = false;
                    btnSaveData.Text = "Outbound Fabric";
                    radPanel1.Enabled = true;          // 출고모드, 센터, 부서선택 
                    tableLayoutPanel7.Enabled = true;  // 작업지시시서 조회란
                    
                    txtBarcode.Select();
                }
                // Manual 
                else
                {
                    btnSaveData.Enabled = true;
                    btnSaveData.Text = "Outbound Manually";
                    radPanel1.Enabled = true;          // 출고모드, 센터, 부서선택 
                    tableLayoutPanel7.Enabled = true;  // 작업지시시서 조회란

                }
                lvQRCode.Items.Clear();

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// 컬러 드롭다운 입력받은 텍스트로 처리하기 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvMain_EditorRequired(object sender, EditorRequiredEventArgs e)
        {
            if (e.EditorType == typeof(RadDropDownListEditor))
            {
                e.EditorType = typeof(Dev.Controller.MyDDLEditor);
            }

        }

        private void gvMain_CreateCell(object sender, GridViewCreateCellEventArgs e)
        {
            if (e.CellType == typeof(GridComboBoxCellElement) && e.Column.Name == "ColorIdx")
            {
                e.CellElement = new Dev.Controller.MyCombBoxCellElement(e.Column as GridViewDataColumn, e.Row);
            }

        }

        private void ddlCenter_SelectedValueChanged(object sender, EventArgs e)
        {
                       
        }

        /// <summary>
        /// 출고모드 변경시 DDL 설정 변경 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlOutMode_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {
                if (this.ddlOutMode.SelectedIndex > -1 && ddlOutMode.Items[ddlOutMode.SelectedIndex].Value.GetType() != typeof(CodeContents))
                {
                    int code = Convert.ToInt32(ddlOutMode.Items[ddlOutMode.SelectedIndex].Value);

                    lstCenter.Clear();
                    lstDept.Clear();
                    ddlCenter.DataSource = null;
                    ddlDept.DataSource = null;

                    if (code == 3 || code == 5 || code == 6 || code == 11)
                    {
                        _dt = Int.Costcenter.Costcenter.Getlist().Tables[0];

                        lstCenter.Add(new CostcenterName(0, ""));
                        foreach (DataRow row in _dt.Rows)
                        {
                            lstCenter.Add(new CostcenterName(Convert.ToInt32(row["CostcenterIdx"]), row["CostcenterName"].ToString()));
                        }
                        
                        ddlCenter.DataSource = lstCenter;
                        ddlCenter.ValueMember = "CostcenterIdx";
                        ddlCenter.DisplayMember = "Name";

                        ddlCenter.SelectedValue = 4;
                    }
                    else if (code == 7 || code == 8)
                    {
                        lstCenter.Add(new CostcenterName(0, ""));
                        
                        ddlCenter.DataSource = lstCenter;
                        ddlCenter.ValueMember = "CostcenterIdx";
                        ddlCenter.DisplayMember = "Name";

                        // 업체 리스트도 변경 
                        _dt = Int.Customer.Customer.Getlist(146).Tables[0];

                        lstDept.Clear();
                        lstDept.Add(new CustomerName(0, "", 0));
                        foreach (DataRow row in _dt.Rows)
                        {
                            lstDept.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]), row["CustName"].ToString(), 0));
                        }
                        ddlDept.DataSource = null;
                        ddlDept.DataSource = lstDept;
                        ddlDept.ValueMember = "CustIdx";
                        ddlDept.DisplayMember = "CustName";
                    }

                    ddlCenter.AutoCompleteMode = AutoCompleteMode.Suggest;
                    ddlCenter.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
                    ddlCenter.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
                }

            }
            catch (Exception ex)
            {

            }
            
            
        }
        /// <summary>
        /// 코스트센터 변경시 DDL 설정 변경 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlCenter_SelectedIndexChanged(object sender, Telerik.WinControls.UI.Data.PositionChangedEventArgs e)
        {
            try
            {
                if (this.ddlCenter.SelectedIndex > -1)
                {
                    int code = Convert.ToInt32(ddlCenter.Items[ddlCenter.SelectedIndex].Value);

                    
                    
                    // 센터가 파트너가 아닌 경우(INT)
                    if (code != 6)
                    {
                        _dt = Int.Department.Dept.Getlist(code).Tables[0];

                        lstDept.Clear();
                        
                        lstDept.Add(new CustomerName(0, "", 0));
                        foreach (DataRow row in _dt.Rows)
                        {
                            lstDept.Add(new CustomerName(Convert.ToInt32(row["DeptIdx"]), row["DeptName"].ToString(), 0));
                        }

                        ddlDept.DataSource = null;
                        ddlDept.DataSource = lstDept;
                        ddlDept.ValueMember = "CustIdx";
                        ddlDept.DisplayMember = "CustName";

                        if(code==4) ddlDept.SelectedValue = 9;  // 원단선택
                    }
                    else if (code == 6)
                    {
                        _dt = Int.Customer.Customer.Getlist(146).Tables[0];

                        lstDept.Clear();
                        lstDept.Add(new CustomerName(0, "", 0));
                        foreach (DataRow row in _dt.Rows)
                        {
                            lstDept.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]), row["CustName"].ToString(), 0));
                        }
                        ddlDept.DataSource = null;
                        ddlDept.DataSource = lstDept;
                        ddlDept.ValueMember = "CustIdx";
                        ddlDept.DisplayMember = "CustName";
                    }

                    ddlDept.AutoCompleteMode = AutoCompleteMode.Suggest;
                    ddlDept.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
                    ddlDept.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnWorksheetSearch_Click(object sender, EventArgs e)
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
                    _searchString.Add(CommonValues.KeyName.Lotno, "");
                    _searchString.Add(CommonValues.KeyName.WorkOrderIdx, "");
                    _searchString.Add(CommonValues.KeyName.ColorIdx, "");

                    _searchKey = new Dictionary<CommonValues.KeyName, int>();
                    _searchKey.Add(CommonValues.KeyName.Status, 0);
                    _searchKey.Add(CommonValues.KeyName.BuyerIdx, 0);
                    _searchKey.Add(CommonValues.KeyName.FabricIdx, 0);
                    _searchKey.Add(CommonValues.KeyName.FabricType, 0);

                    _searchKey.Add(CommonValues.KeyName.RackNo, 0);
                    _searchKey.Add(CommonValues.KeyName.Floorno, 0);
                    _searchKey.Add(CommonValues.KeyName.RackPos, 0);
                    _searchKey.Add(CommonValues.KeyName.PosX, 0);
                    _searchKey.Add(CommonValues.KeyName.PosY, 0);

                    CultureInfo ci = new CultureInfo("ko-KR");
                    Console.WriteLine(dtInboundDate.Value.ToString("d", ci));

                    DataBinding_GV1();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        private void gvOutbound_HyperlinkOpened(object sender, HyperlinkOpenedEventArgs e)
        {
            CommonController.Close_All_Children(this, "Outbound");
            Outbound form = new Outbound(__main__, "", e.Cell.Value.ToString().Trim());
            form.Text = DateTime.Now.ToLongTimeString();
            form.MdiParent = this.MdiParent;
            form.Show();
        }

        #endregion

        #region 7. 기능 멤버 (변경필요없음)

        /// <summary>
        // QR읽은후 복호화 
        /// </summary>
        /// <param name="origin"></param>
        /// <returns></returns>
        private string Decryptor(string origin)
        {
            string result = "";

            try
            {
                return result = Int.Encryptor.Decrypt(origin, "love1229");
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private void gvOutbound_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement is GridSummaryCellElement)
            {
                // e.CellElement.TextAlignment = ContentAlignment.MiddleRight;

                // 0미만일 경우 컬러 변경
                GridSummaryCellElement summaryCell = e.CellElement as GridSummaryCellElement;
                decimal summaryValue = 0m;
                if (summaryCell != null && decimal.TryParse(e.CellElement.Value + "", out summaryValue) &&
                    summaryValue < 0)
                {
                    summaryCell.ForeColor = Color.Red;
                }
                else
                {
                    summaryCell.ForeColor = Color.Black;
                }
                
            }
        }

        /// <summary>
        /// 그리드뷰 레이아웃 복구 (/conf에 그리드별로 저장함) 
        /// </summary>
        private void LoadGVLayout()
        {
            // 레이아웃 xml 파일 위치 설정 
            string url = Environment.CurrentDirectory + "/conf";
            string path = url + _layoutfile;

            try
            {
                if (File.Exists(path))
                {
                    _gv1.LoadLayout(path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 현재 폼의 그리드뷰 레이아웃 저장 (form closed) 
        /// </summary>
        private void SaveGVLayout()
        {
            string url = Environment.CurrentDirectory + "/conf";
            string path = url + _layoutfile;

            try
            {
                _gv1.SaveLayout(path);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }


        #endregion

        private void AddSummaryRows()
        {
            gvOutbound.SummaryRowsBottom.Clear();
            gvOutbound.BottomPinnedRowsMode = GridViewBottomPinnedRowsMode.Fixed; 

            GridViewSummaryRowItem summaryRowItem = new GridViewSummaryRowItem();
            GridViewSummaryItem summaryItem = null;
            
            summaryItem = new GridViewSummaryItem("Lotno", "Total Q'ty ", GridAggregateFunction.Max);
            summaryRowItem.Add(summaryItem);

            summaryItem = new GridViewSummaryItem();
            summaryItem.Name = "Kgs";
            summaryItem.Aggregate = GridAggregateFunction.Sum;
            summaryRowItem.Add(summaryItem);

            summaryItem = new GridViewSummaryItem();
            summaryItem.Name = "Yds";
            summaryItem.Aggregate = GridAggregateFunction.Sum;
            summaryRowItem.Add(summaryItem);

            gvOutbound.SummaryRowsBottom.Add(summaryRowItem);

            string summaryTitle = "";

            summaryTitle = "Stock Q'ty ";

            // 미선적/Over/Shortage 
            summaryRowItem = new GridViewSummaryRowItem();
            summaryItem = null;

            summaryItem = new GridViewSummaryItem("Lotno", summaryTitle, GridAggregateFunction.Max);
            summaryRowItem.Add(summaryItem);

            GridViewRowInfo row = Int.Members.GetCurrentRow(gvMain);

            summaryItem = new GridViewSummaryItem();
            summaryItem.Name = "Kgs";
            // Status=3(Return remained fabric) 
            summaryItem.AggregateExpression = Convert.ToDouble(row.Cells["Kgs"].Value == DBNull.Value ? 0 : row.Cells["Kgs"].Value) + "-Sum(IIF(Status=3, Kgs*-1.0, Kgs))";
            summaryItem.FormatString = "{0:N2}";
            summaryRowItem.Add(summaryItem);

            summaryItem = new GridViewSummaryItem();
            summaryItem.Name = "Yds";
            summaryItem.AggregateExpression = Convert.ToDouble(row.Cells["Yds"].Value == DBNull.Value ? 0 : row.Cells["Yds"].Value) + "-Sum(IIF(Status=3, Yds*-1.0, Yds))";
            summaryItem.FormatString = "{0:N2}";
            summaryRowItem.Add(summaryItem);

            gvOutbound.SummaryRowsBottom.Add(summaryRowItem);

        }

    }

}
