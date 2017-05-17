﻿using Int.Code;
using Int.Customer;
using Int.Department;
using Dev.Options;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Dev.Pattern;

namespace Dev.Sales
{
    /// <summary>
    /// 샘플오더 관리화면: 오더입력수정 및 작업지시등
    /// </summary>
    public partial class OrderMain : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                             // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, int> _searchKey;                // 쿼리값 value, 쿼리항목 key로 전달 
        public DataRow InsertedOrderRow = null;

        private enum OrderStatus { Normal, Progress, Cancel, Close };           // 오더상태값
        
        private bool _bRtn;                                                     // 쿼리결과 리턴
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        private DataTable _dt = null;                                           // 기본 데이터테이블
        private Controller.Orders _obj1 = null;                                 // 현재 생성된 객체 
        private Controller.OrderColor _obj2 = null;                             // 현재 생성된 객체 
        private Controller.Operation _obj3 = null;                             // 현재 생성된 객체 
        private RadContextMenu contextMenu;                                     // 컨텍스트 메뉴
        private List<CodeContents> lstStatus = new List<CodeContents>();        // 오더상태
        private List<CodeContents> lstProduction = new List<CodeContents>();    // 생산진행상태 (패턴포함) 

        private List<DepartmentName> deptName = new List<DepartmentName>();     // 부서
        private List<CustomerName> custName = new List<CustomerName>();         // 거래처
        private List<CustomerName> lstVendor = new List<CustomerName>();        // vendor
        private List<CustomerName> embName = new List<CustomerName>();          // 나염업체
        private List<CodeContents> codeName = new List<CodeContents>();         // 코드
        private List<CodeContents> lstOrigin = new List<CodeContents>();        // Origin country 
        private List<CodeContents> lstBrand = new List<CodeContents>();         // 브랜드
        private List<CodeContents> lstIsPrinting = new List<CodeContents>();    // 나염여부
        private List<CustomerName> lstUser = new List<CustomerName>();          // 유저명
        private List<CustomerName> lstSewthread = new List<CustomerName>();     // sewthread
        private List<CodeContents> lstColor = new List<CodeContents>();         // 컬러명
        private List<CodeContents> lstOperation = new List<CodeContents>();         // 공정명
        private List<Codes.Controller.Sizes> lstSize = new List<Codes.Controller.Sizes>(); 
        private string _layoutfile = "/GVLayoutOrders.xml";
                
        RadMenuItem mnuNew, mnuDel, mnuHide, mnuShow, menuItem2, menuItem3, menuItem4, mnuWorksheet, 
            mnuCutting, mnuOutsourcing, mnuSewing, mnuInspecting, mnuPattern = null;

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public OrderMain(InheritMDI main)
        {
            base.InitializeComponent(); // parent 컴포넌트에 접근하기 위해 컨트롤을 public으로 지정 > 향후 수정 필요 (todo) 
            InitializeComponent();
            __main__ = main;            // MDI 연결 
            _gv1 = this.gvOrderActual;  // 그리드뷰 일반화를 위해 변수 별도 저장
            
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmOrderActual_Load(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;  // 창 최대화
            DataLoading_DDL();          // DDL 데이터 로딩
            Config_DropDownList();      // 상단 DDL 생성 설정
            GV1_CreateColumn(_gv1);     // 그리드뷰 생성
            GV1_LayoutSetting(_gv1);    // 중앙 그리드뷰 설정 
            //Config_ContextMenu();       // 중앙 그리드뷰 컨텍스트 생성 설정 
            LoadGVLayout();             // 그리드뷰 레이아웃 복구 
            // DataBinding_GV1(0, null, "", "");   // 중앙 그리드뷰 데이터 
            GV3_CreateColumn(gvOperation);  // 공정 제목
            GV4_CreateColumn(gvProduction); // 현황 
        }
        
        /// <summary>
        /// 상단 검색을 위한 Dropdownlist 생성
        /// </summary>
        private void Config_DropDownList()
        {
            // 부서 
            ddlDept.DataSource = deptName;
            ddlDept.DisplayMember = "DeptName";
            ddlDept.ValueMember = "DeptIdx";
            ddlDept.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlDept.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 바이어
            ddlCust.DataSource = custName;
            ddlCust.DisplayMember = "CustName";
            ddlCust.ValueMember = "CustIdx";
            ddlCust.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlCust.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 오더 상태 
            ddlStatus.DataSource = lstStatus;
            ddlStatus.DisplayMember = "Contents";
            ddlStatus.ValueMember = "CodeIdx";
            ddlStatus.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlStatus.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 나염
            //embName.Add(new CustomerName(0, "All", 0));
            ddlPrinting.DataSource = embName;
            ddlPrinting.DisplayMember = "CustName";
            ddlPrinting.ValueMember = "CustIdx";
            ddlPrinting.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlPrinting.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
        }

        /// <summary>
        /// 컨텍스트 메뉴 생성 (메인)
        /// </summary>
        private void Config_ContextMenu()
        {
            contextMenu = new RadContextMenu();

            // 단축키 초기화 
            Clear_Shortcuts(); 

            // 오더 신규 입력
            mnuNew = new RadMenuItem("New Order");
            //mnuNew.Image = Properties.Resources._20_20;
            mnuNew.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.N));
            mnuNew.Click += new EventHandler(mnuNew_Click);

            // 오더 삭제
            //contextMenu = new RadContextMenu();
            mnuDel = new RadMenuItem("Remove Order");
            //mnuNew.Image = Properties.Resources._20_20;
            mnuDel.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.D));
            mnuDel.Click += new EventHandler(mnuDel_Click);

            // 열 숨기기
            //contextMenu = new RadContextMenu();
            mnuHide = new RadMenuItem("Hide Column");
            mnuHide.Click += new EventHandler(mnuHide_Click);

            // 열 보이기
            //contextMenu = new RadContextMenu();
            mnuShow = new RadMenuItem("Show all Columns");
            mnuShow.Click += new EventHandler(mnuShow_Click);
            
            // 오더복사
            menuItem2 = new RadMenuItem("Copy Order");
            menuItem2.Click += new EventHandler(mnuCopyOrder_Click);
            menuItem2.Image = Properties.Resources.copy;
            menuItem2.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.C));

            // 오더캔슬
            menuItem3 = new RadMenuItem("Cancel Order");
            menuItem3.Image = Properties.Resources.cancel;
            menuItem3.ForeColor = Color.Red;
            menuItem3.Click += new EventHandler(mnuCancelOrder_Click);
            menuItem3.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.D));

            // 오더종료
            menuItem4 = new RadMenuItem("Close Order");
            menuItem4.Image = Properties.Resources.locked;
            menuItem4.ForeColor = Color.Blue;
            menuItem4.Click += new EventHandler(mnuCloseOrder_Click);
            menuItem4.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.C));

            // 패턴
            mnuPattern = new RadMenuItem("Request Pattern");
            // mnuShipment.Image = Properties.Resources._20_20;
            mnuPattern.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.P));
            mnuPattern.Click += new EventHandler(mnuPattern_Click);

            // 작업지시서
            mnuWorksheet = new RadMenuItem("Edit Worksheets");
            // mnuShipment.Image = Properties.Resources._20_20;
            mnuWorksheet.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.S));
            mnuWorksheet.Click += new EventHandler(mnuWorksheet_Click);

            // 생산 작업지시 (Cutting)
            mnuCutting = new RadMenuItem("Cutting Order");
            //mnuCutting.Image = Properties.Resources._20_20;
            //mnuCutting.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.S));
            mnuCutting.Click += new EventHandler(mnuCutting_Click);

            // 생산 작업지시 (Outsourcing)
            mnuOutsourcing = new RadMenuItem("Outsourcing Order");
            //mnuCutting.Image = Properties.Resources._20_20;
            //mnuCutting.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.S));
            mnuOutsourcing.Click += new EventHandler(mnuOutsourcing_Click);

            // 생산 작업지시 (Sewing)
            mnuSewing = new RadMenuItem("Sewing Order");
            //mnuCutting.Image = Properties.Resources._20_20;
            //mnuCutting.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.S));
            mnuSewing.Click += new EventHandler(mnuSewing_Click);

            // 생산 작업지시 (Inspecting)
            mnuInspecting = new RadMenuItem("Inspecting Order");
            //mnuCutting.Image = Properties.Resources._20_20;
            //mnuCutting.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.S));
            mnuInspecting.Click += new EventHandler(mnuInspecting_Click);

            // 분리선
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            separator.Tag = "seperator";

            // 컨텍스트 추가 
            contextMenu.Items.Add(mnuNew);
            contextMenu.Items.Add(mnuDel);
            contextMenu.Items.Add(separator);

            contextMenu.Items.Add(mnuHide);
            contextMenu.Items.Add(mnuShow);
            separator = new RadMenuSeparatorItem();
            separator.Tag = "seperator";
            contextMenu.Items.Add(separator);

            contextMenu.Items.Add(menuItem2);
            contextMenu.Items.Add(menuItem3);
            contextMenu.Items.Add(menuItem4);
            separator = new RadMenuSeparatorItem();
            separator.Tag = "seperator";
            contextMenu.Items.Add(separator);

            //contextMenu.Items.Add(mnuShipment);
            contextMenu.Items.Add(mnuWorksheet);
            contextMenu.Items.Add(mnuPattern);
            
            separator = new RadMenuSeparatorItem();
            separator.Tag = "seperator";
            contextMenu.Items.Add(separator);

            contextMenu.Items.Add(mnuCutting);
            contextMenu.Items.Add(mnuOutsourcing);
            contextMenu.Items.Add(mnuSewing);
            contextMenu.Items.Add(mnuInspecting);

        }
        
        /// <summary>
        /// 컨텍스트 메뉴 (컬러사이즈)
        /// </summary>
        private void Config_ContextMenu_Color()
        {
            contextMenu = new RadContextMenu();

            // 단축키 초기화 
            Clear_Shortcuts(); 

            // 오더 신규 입력
            mnuNew = new RadMenuItem("New");
            //mnuNew.Image = Properties.Resources._20_20;
            mnuNew.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.N));
            mnuNew.Click += new EventHandler(mnuNewColor_Click);

            // 오더 삭제
            //contextMenu = new RadContextMenu();
            mnuDel = new RadMenuItem("Remove");
            //mnuNew.Image = Properties.Resources._20_20;
            mnuDel.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.D));
            mnuDel.Click += new EventHandler(mnuDelColor_Click);
            
            // 컨텍스트 추가 
            contextMenu.Items.Add(mnuNew);
            contextMenu.Items.Add(mnuDel);
        }

        /// <summary>
        /// 컨텍스트 메뉴 (공정)
        /// </summary>
        private void Config_ContextMenu_Operation()
        {
            contextMenu = new RadContextMenu();

            // 단축키 초기화 
            Clear_Shortcuts();

            // 오더 신규 입력
            mnuNew = new RadMenuItem("New");
            //mnuNew.Image = Properties.Resources._20_20;
            mnuNew.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.N));
            mnuNew.Click += new EventHandler(mnuNewOperation_Click);

            // 오더 삭제
            //contextMenu = new RadContextMenu();
            mnuDel = new RadMenuItem("Remove");
            //mnuNew.Image = Properties.Resources._20_20;
            mnuDel.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.D));
            mnuDel.Click += new EventHandler(mnuDelOperation_Click);

            // 컨텍스트 추가 
            contextMenu.Items.Add(mnuNew);
            contextMenu.Items.Add(mnuDel);
        }

        /// <summary>
        /// 그리드뷰 컬럼 생성 (메인)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV1_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            gv.Columns["Idx"].Width = 50;
            gv.Columns["Idx"].TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns["Idx"].HeaderText = "ID";

            GridViewComboBoxColumn cboDept = new GridViewComboBoxColumn();
            cboDept.Name = "DeptIdx";
            cboDept.DataSource = deptName;
            cboDept.ValueMember = "DeptIdx";
            cboDept.DisplayMember = "DeptName";
            cboDept.FieldName = "DeptIdx";
            cboDept.HeaderText = "Department";
            cboDept.Width = 70;
            gv.Columns.Insert(1, cboDept);

            gv.Columns["Indate"].Width = 90;
            gv.Columns["Indate"].TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns["Indate"].FormatString = "{0:d}";
            gv.Columns["Indate"].HeaderText = "Date";

            GridViewComboBoxColumn cboBuyer = new GridViewComboBoxColumn();
            cboBuyer.Name = "Buyer";
            cboBuyer.DataSource = custName;
            cboBuyer.ValueMember = "CustIdx";
            cboBuyer.DisplayMember = "CustName";
            cboBuyer.FieldName = "Buyer";
            cboBuyer.HeaderText = "Buyer";
            cboBuyer.Width = 100;
            gv.Columns.Insert(3, cboBuyer);
            
            gv.Columns["Pono"].Width = 120;
            gv.Columns["Pono"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns["Pono"].HeaderText = "PO#";

            gv.Columns["Styleno"].Width = 130;
            gv.Columns["Styleno"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns["Styleno"].HeaderText = "Style#";

            gv.Columns["Fileno"].Width = 90;
            gv.Columns["Fileno"].HeaderText = "INT File #";
            gv.Columns["Fileno"].ReadOnly = true;

            GridViewTextBoxColumn reorder = new GridViewTextBoxColumn();
            reorder.Name = "reorder";
            reorder.FieldName = "reorder";
            reorder.HeaderText = "Re#";
            reorder.ReadOnly = true;
            reorder.Width = 35;
            reorder.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Insert(7, reorder);

            GridViewTextBoxColumn reason = new GridViewTextBoxColumn();
            reason.Name = "ReorderReason";
            reason.FieldName = "ReorderReason";
            reason.HeaderText = "Reason";
            reason.IsVisible = false;
            reason.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Insert(8, reason);
            
            gv.Columns["Season"].Width = 60;
            gv.Columns["Season"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns["Season"].HeaderText = "Season";

            gv.Columns["Description"].Width = 200;
            gv.Columns["Description"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns["Description"].HeaderText = "Description";
            
            gv.Columns["DeliveryDate"].Width = 100;
            gv.Columns["DeliveryDate"].TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns["DeliveryDate"].FormatString = "{0:d}";
            gv.Columns["DeliveryDate"].HeaderText = "Delivery";
            
            GridViewComboBoxColumn cboIsPrinting = new GridViewComboBoxColumn();
            cboIsPrinting.Name = "IsPrinting";

            lstIsPrinting.Clear();
            lstIsPrinting = codeName.FindAll(
                delegate (CodeContents code)
                {
                    return code.Classification == "IsPrinting";
                });

            cboIsPrinting.DataSource = lstIsPrinting;
            cboIsPrinting.ValueMember = "CodeIdx";
            cboIsPrinting.DisplayMember = "Contents";
            cboIsPrinting.FieldName = "IsPrinting";
            cboIsPrinting.HeaderText = "Screen Printing\n(Yes No)";
            cboIsPrinting.Width = 100;
            gv.Columns.Insert(13, cboIsPrinting);

            GridViewComboBoxColumn cboEmblelishId1 = new GridViewComboBoxColumn();
            cboEmblelishId1.Name = "EmbelishId1";
            cboEmblelishId1.DataSource = embName;
            cboEmblelishId1.ValueMember = "CustIdx";
            cboEmblelishId1.DisplayMember = "CustName";
            cboEmblelishId1.FieldName = "EmbelishId1";
            cboEmblelishId1.HeaderText = "Embelishment1";
            cboEmblelishId1.Width = 100;
            gv.Columns.Insert(14, cboEmblelishId1);

            GridViewComboBoxColumn cboEmblelishId2 = new GridViewComboBoxColumn();
            cboEmblelishId2.Name = "EmbelishId2";
            cboEmblelishId2.DataSource = embName;
            cboEmblelishId2.ValueMember = "CustIdx";
            cboEmblelishId2.DisplayMember = "CustName";
            cboEmblelishId2.FieldName = "EmbelishId2";
            cboEmblelishId2.HeaderText = "Embelishment2";
            cboEmblelishId2.Width = 100;
            gv.Columns.Insert(15, cboEmblelishId2);

            GridViewMultiComboBoxColumn cboSizeGrp = new GridViewMultiComboBoxColumn();
            cboSizeGrp.Name = "SizeGroupIdx";
            cboSizeGrp.DataSource = dataSetSizeGroup.DataTableSizeGroup;
            cboSizeGrp.ValueMember = "SizeGroupIdx";
            cboSizeGrp.DisplayMember = "SizeGroupName";
            cboSizeGrp.FieldName = "SizeGroupIdx";
            cboSizeGrp.HeaderText = "SizeGroup";
            cboSizeGrp.AutoSizeMode = BestFitColumnMode.AllCells;
            cboSizeGrp.Width = 200;
            gv.Columns.Insert(16, cboSizeGrp);
                        
            GridViewMultiComboBoxColumn cboSewThread = new GridViewMultiComboBoxColumn();
            cboSewThread.Name = "SewThreadIdx";
            cboSewThread.DataSource = dataSetSizeGroup.DataTableSewThread;
            cboSewThread.ValueMember = "SewThreadIdx";
            cboSewThread.DisplayMember = "SewThreadName";
            cboSewThread.FieldName = "SewThreadIdx";
            cboSewThread.HeaderText = "SewThread";
            cboSewThread.AutoSizeMode = BestFitColumnMode.AllCells;
            cboSewThread.Width = 100;
            gv.Columns.Insert(17, cboSewThread);
            
            GridViewComboBoxColumn cHandler = new GridViewComboBoxColumn();
            cHandler.Name = "Handler";
            cHandler.DataSource = lstUser;
            cHandler.ValueMember = "CustIdx";
            cHandler.DisplayMember = "CustName";
            cHandler.FieldName = "Handler";
            cHandler.HeaderText = "Handler";
            cHandler.Width = 80;
            gv.Columns.Insert(18, cHandler);

            gv.Columns["OrderQty"].Width = 80;
            gv.Columns["OrderQty"].FormatString = "{0:N0}";
            gv.Columns["OrderQty"].HeaderText = "Q'ty(pcs)";

            gv.Columns["OrderPrice"].Width = 50;
            gv.Columns["OrderPrice"].FormatString = "{0:N2}";
            gv.Columns["OrderPrice"].HeaderText = "U/Price\n($)";

            gv.Columns["OrderAmount"].Width = 100;
            gv.Columns["OrderAmount"].FormatString = "{0:N2}";
            gv.Columns["OrderAmount"].HeaderText = "Amount($)";

            GridViewTextBoxColumn remark = new GridViewTextBoxColumn();
            remark.Name = "Remark";
            remark.FieldName = "Remark";
            remark.HeaderText = "Remark";
            remark.Width = 100;
            remark.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Insert(22, remark);

            GridViewDateTimeColumn dtRequested = new GridViewDateTimeColumn();
            dtRequested.Name = "TeamRequestedDate";
            dtRequested.Width = 100;
            dtRequested.TextAlignment = ContentAlignment.MiddleCenter;
            dtRequested.FormatString = "{0:d}";
            dtRequested.FieldName = "TeamRequestedDate";
            dtRequested.HeaderText = "Requested";
            dtRequested.ReadOnly = true; 
            gv.Columns.Insert(23, dtRequested);

            GridViewDateTimeColumn dtConfirmed = new GridViewDateTimeColumn();
            dtConfirmed.Name = "SplConfirmedDate";
            dtConfirmed.Width = 100;
            dtConfirmed.TextAlignment = ContentAlignment.MiddleCenter;
            dtConfirmed.FormatString = "{0:d}";
            dtConfirmed.FieldName = "SplConfirmedDate";
            dtConfirmed.HeaderText = "Confirmed";
            dtConfirmed.ReadOnly = true;
            gv.Columns.Insert(24, dtConfirmed);


            GridViewComboBoxColumn status = new GridViewComboBoxColumn();
            status.Name = "Status";
            status.DataSource = lstStatus;
            status.DisplayMember = "Contents";
            status.ValueMember = "CodeIdx";
            status.FieldName = "Status";
            status.HeaderText = "Status";
            status.Width = 70;
            status.ReadOnly = true;
            gv.Columns.Insert(25, status);

            GridViewComboBoxColumn cboVendor = new GridViewComboBoxColumn();
            cboVendor.Name = "Vendor";
            cboVendor.DataSource = lstVendor;
            cboVendor.ValueMember = "CustIdx";
            cboVendor.DisplayMember = "CustName";
            cboVendor.FieldName = "Vendor";
            cboVendor.HeaderText = "Vendor";
            cboVendor.Width = 100;
            gv.Columns.Insert(26, cboVendor);

            // Country (코드의 Origin으로 연결) 
            GridViewComboBoxColumn cboOrigin = new GridViewComboBoxColumn();
            cboOrigin.Name = "Country";

            lstOrigin.Clear();
            lstOrigin = codeName.FindAll(
                delegate (CodeContents code)
                {
                    return code.CodeIdx == 0 || code.Classification == "Origin";
                });
            
            cboOrigin.DataSource = lstOrigin;
            cboOrigin.ValueMember = "CodeIdx";
            cboOrigin.DisplayMember = "Contents";
            cboOrigin.FieldName = "Country";
            cboOrigin.HeaderText = "Country";
            cboOrigin.Width = 100;
            gv.Columns.Insert(27, cboOrigin);

            GridViewTextBoxColumn SampleType = new GridViewTextBoxColumn();
            SampleType.Name = "SampleType";
            SampleType.FieldName = "SampleType";
            SampleType.HeaderText = "SampleType";
            SampleType.IsVisible = false; 
            SampleType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Insert(28, SampleType);

            GridViewTextBoxColumn InspType = new GridViewTextBoxColumn();
            InspType.Name = "InspType";
            InspType.FieldName = "InspType";
            InspType.HeaderText = "Inspection Type";
            InspType.IsVisible = false;
            InspType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Insert(29, InspType);
            
            #endregion
        }

        /// <summary>
        /// 그리드뷰 컬럼 생성 (컬러사이즈)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV2_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            gv.MasterTemplate.Columns.Clear(); 
            gv.DataSource = null; 

            GridViewTextBoxColumn cIdx = new GridViewTextBoxColumn();
            cIdx.Name = "Idx";
            cIdx.FieldName = "Idx";
            cIdx.Width = 50;
            cIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cIdx.HeaderText = "ID";
            cIdx.ReadOnly = true; 
            gv.Columns.Add(cIdx); 

            GridViewTextBoxColumn cOrderIdx = new GridViewTextBoxColumn();
            cOrderIdx.Name = "OrderIdx";
            cOrderIdx.FieldName = "OrderIdx";
            cOrderIdx.Width = 60;
            cOrderIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cOrderIdx.HeaderText = "Order ID";
            cOrderIdx.ReadOnly = true;
            gv.Columns.Add(cOrderIdx);

            GridViewComboBoxColumn cColorIdx = new GridViewComboBoxColumn();
            cColorIdx.Name = "ColorIdx";
            cColorIdx.FieldName = "ColorIdx";
            cColorIdx.DataSource = lstColor;
            cColorIdx.DisplayMember = "Contents";
            cColorIdx.ValueMember = "CodeIdx";
            cColorIdx.Width = 120;
            cColorIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cColorIdx.HeaderText = "Color";
            gv.Columns.Add(cColorIdx);
            

            #region Sizes 

            GridViewTextBoxColumn cSizeIdx1 = new GridViewTextBoxColumn();
            cSizeIdx1.Name = "SizeIdx1";
            cSizeIdx1.FieldName = "SizeIdx1";
            cSizeIdx1.IsVisible = false;
            gv.Columns.Add(cSizeIdx1);

            GridViewTextBoxColumn cSizeIdx2 = new GridViewTextBoxColumn();
            cSizeIdx2.Name = "SizeIdx2";
            cSizeIdx2.FieldName = "SizeIdx2";
            cSizeIdx2.IsVisible = false;
            gv.Columns.Add(cSizeIdx2);

            GridViewTextBoxColumn cSizeIdx3 = new GridViewTextBoxColumn();
            cSizeIdx3.Name = "SizeIdx3";
            cSizeIdx3.FieldName = "SizeIdx3";
            cSizeIdx3.IsVisible = false;
            gv.Columns.Add(cSizeIdx3);

            GridViewTextBoxColumn cSizeIdx4 = new GridViewTextBoxColumn();
            cSizeIdx4.Name = "SizeIdx4";
            cSizeIdx4.FieldName = "SizeIdx4";
            cSizeIdx4.IsVisible = false;
            gv.Columns.Add(cSizeIdx4);

            GridViewTextBoxColumn cSizeIdx5 = new GridViewTextBoxColumn();
            cSizeIdx5.Name = "SizeIdx5";
            cSizeIdx5.FieldName = "SizeIdx5";
            cSizeIdx5.IsVisible = false;
            gv.Columns.Add(cSizeIdx5);

            GridViewTextBoxColumn cSizeIdx6 = new GridViewTextBoxColumn();
            cSizeIdx6.Name = "SizeIdx6";
            cSizeIdx6.FieldName = "SizeIdx6";
            cSizeIdx6.IsVisible = false;
            gv.Columns.Add(cSizeIdx6);

            GridViewTextBoxColumn cSizeIdx7 = new GridViewTextBoxColumn();
            cSizeIdx7.Name = "SizeIdx7";
            cSizeIdx7.FieldName = "SizeIdx7";
            cSizeIdx7.IsVisible = false;
            gv.Columns.Add(cSizeIdx7);

            GridViewTextBoxColumn cSizeIdx8 = new GridViewTextBoxColumn();
            cSizeIdx8.Name = "SizeIdx8";
            cSizeIdx8.FieldName = "SizeIdx8";
            cSizeIdx8.IsVisible = false;
            gv.Columns.Add(cSizeIdx8);

            #endregion 

            #region Pcs 

            GridViewDecimalColumn cPcs1 = new GridViewDecimalColumn();
            cPcs1.Name = "pcs1";
            cPcs1.FieldName = "pcs1";
            cPcs1.Width = 70;
            cPcs1.FormatString = "{0:N0}"; 
            cPcs1.HeaderText = lstSize[1].SizeName; 
            gv.Columns.Add(cPcs1);

            GridViewDecimalColumn cPcs2 = new GridViewDecimalColumn();
            cPcs2.Name = "pcs2";
            cPcs2.FieldName = "pcs2";
            cPcs2.Width = 70;
            cPcs2.FormatString = "{0:N0}";
            cPcs2.HeaderText = lstSize[2].SizeName;
            gv.Columns.Add(cPcs2);

            GridViewDecimalColumn cPcs3 = new GridViewDecimalColumn();
            cPcs3.Name = "pcs3";
            cPcs3.FieldName = "pcs3";
            cPcs3.Width = 70;
            cPcs3.FormatString = "{0:N0}";
            cPcs3.HeaderText = lstSize[3].SizeName;
            gv.Columns.Add(cPcs3);

            GridViewDecimalColumn cPcs4 = new GridViewDecimalColumn();
            cPcs4.Name = "pcs4";
            cPcs4.FieldName = "pcs4";
            cPcs4.Width = 70;
            cPcs4.FormatString = "{0:N0}";
            cPcs4.HeaderText = lstSize[4].SizeName;
            gv.Columns.Add(cPcs4);

            GridViewDecimalColumn cPcs5 = new GridViewDecimalColumn();
            cPcs5.Name = "pcs5";
            cPcs5.FieldName = "pcs5";
            cPcs5.Width = 70;
            cPcs5.FormatString = "{0:N0}";
            cPcs5.HeaderText = lstSize[5].SizeName;
            gv.Columns.Add(cPcs5);

            GridViewDecimalColumn cPcs6 = new GridViewDecimalColumn();
            cPcs6.Name = "pcs6";
            cPcs6.FieldName = "pcs6";
            cPcs6.Width = 70;
            cPcs6.FormatString = "{0:N0}";
            cPcs6.HeaderText = lstSize[6].SizeName;
            gv.Columns.Add(cPcs6);

            GridViewDecimalColumn cPcs7 = new GridViewDecimalColumn();
            cPcs7.Name = "pcs7";
            cPcs7.FieldName = "pcs7";
            cPcs7.Width = 70;
            cPcs7.FormatString = "{0:N0}";
            cPcs7.HeaderText = lstSize[7].SizeName;
            gv.Columns.Add(cPcs7);

            GridViewDecimalColumn cPcs8 = new GridViewDecimalColumn();
            cPcs8.Name = "pcs8";
            cPcs8.FieldName = "pcs8";
            cPcs8.Width = 70;
            cPcs8.FormatString = "{0:N0}";
            cPcs8.HeaderText = lstSize[8].SizeName;
            gv.Columns.Add(cPcs8);

            #endregion

            #endregion
        }

        /// <summary>
        /// 그리드뷰 컬럼 생성 (공정)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV3_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            gv.MasterTemplate.Columns.Clear();
            gv.DataSource = null;

            GridViewTextBoxColumn cIdx = new GridViewTextBoxColumn();
            cIdx.Name = "Idx";
            cIdx.FieldName = "Idx";
            cIdx.Width = 50;
            cIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cIdx.HeaderText = "ID";
            cIdx.ReadOnly = true;
            gv.Columns.Add(cIdx);

            GridViewTextBoxColumn cOrderIdx = new GridViewTextBoxColumn();
            cOrderIdx.Name = "OrderIdx";
            cOrderIdx.FieldName = "OrderIdx";
            cOrderIdx.Width = 60;
            cOrderIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cOrderIdx.HeaderText = "Order ID";
            cOrderIdx.ReadOnly = true;
            gv.Columns.Add(cOrderIdx);

            lstOperation.Clear();
            lstOperation = codeName.FindAll(
                delegate (CodeContents code)
                {
                    return code.Classification == "Operation";
                });
            
            GridViewComboBoxColumn cOperIdx = new GridViewComboBoxColumn();
            cOperIdx.Name = "OperationIdx";
            cOperIdx.FieldName = "OperationIdx";
            cOperIdx.DataSource = lstOperation;
            cOperIdx.DisplayMember = "Contents";
            cOperIdx.ValueMember = "CodeIdx";
            cOperIdx.Width = 120;
            cOperIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cOperIdx.HeaderText = "Operation";
            gv.Columns.Add(cOperIdx);

            GridViewTextBoxColumn cPriority = new GridViewTextBoxColumn();
            cPriority.Name = "Priority";
            cPriority.FieldName = "Priority";
            cPriority.Width = 70;
            cPriority.TextAlignment = ContentAlignment.MiddleCenter;
            cPriority.HeaderText = "Priority";
            //cPriority.ReadOnly = true;
            gv.Columns.Add(cPriority);

            GridViewCommandColumn cUpButton = new GridViewCommandColumn();
            cUpButton.Name = "OperationUp";
            cUpButton.HeaderText = "Up";
            cUpButton.Image = Properties.Resources.up_arrow;
            cUpButton.ImageAlignment = ContentAlignment.MiddleCenter; 
            cUpButton.Width = 40;
            gv.Columns.Add(cUpButton);

            GridViewCommandColumn cDownButton = new GridViewCommandColumn();
            cDownButton.Name = "OperationDown";
            cDownButton.HeaderText = "Down";
            cDownButton.Image = Properties.Resources.down_arrow;
            cDownButton.ImageAlignment = ContentAlignment.MiddleCenter;
            cDownButton.Width = 40;
            gv.Columns.Add(cDownButton);

            gv.CommandCellClick += new CommandCellClickEventHandler(GV_CommandCellClick); 


            #endregion
        }

        /// <summary>
        /// 그리드뷰 컬럼 생성 (진행현황)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV4_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            gv.MasterTemplate.Columns.Clear();
            gv.DataSource = null;

            GridViewComboBoxColumn cOperIdx = new GridViewComboBoxColumn();
            cOperIdx.Name = "OperationIdx";
            cOperIdx.FieldName = "OperationIdx";
            cOperIdx.DataSource = lstOperation;
            cOperIdx.DisplayMember = "Contents";
            cOperIdx.ValueMember = "CodeIdx";
            cOperIdx.Width = 70;
            cOperIdx.TextAlignment = ContentAlignment.MiddleLeft;
            cOperIdx.HeaderText = "Operation";
            cOperIdx.ReadOnly = true;
            gv.Columns.Add(cOperIdx);

            GridViewHyperlinkColumn  cWorkIdx = new GridViewHyperlinkColumn();
            cWorkIdx.Name = "WorkOrderIdx";
            cWorkIdx.FieldName = "WorkOrderIdx";
            cWorkIdx.Width = 100;
            cWorkIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cWorkIdx.HeaderText = "Work ID";
            cWorkIdx.ReadOnly = true;
            gv.Columns.Add(cWorkIdx);

            GridViewComboBoxColumn status = new GridViewComboBoxColumn();
            status.Name = "Status";
            status.DataSource = lstProduction;
            status.DisplayMember = "Contents";
            status.ValueMember = "CodeIdx";
            status.FieldName = "Status";
            status.HeaderText = "Status";
            status.Width = 80;
            status.ReadOnly = true;
            gv.Columns.Add(status);

            GridViewTextBoxColumn cTitle = new GridViewTextBoxColumn();
            cTitle.Name = "Title";
            cTitle.FieldName = "Title";
            cTitle.Width = 300;
            cTitle.TextAlignment = ContentAlignment.MiddleLeft;
            cTitle.HeaderText = "Title";
            cTitle.ReadOnly = true;
            gv.Columns.Add(cTitle);

            #endregion
        }

        #endregion

        #region 3. 컨트롤 초기 설정

        /// <summary>
        /// 컨텍스트 메뉴 연결 (메인, 행높이 설정)
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
                else item.MinSize = new Size(0, 22);
            }
        }

        /// <summary>
        /// 컨텍스트 메뉴 연결 (컬러사이즈)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GV_SubContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.ContextMenu = contextMenu.DropDown;
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

            #region Config Cell Conditions: 오더상태에 따라 행스타일 변경

            // 캔슬오더 색상변경
            Font f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Strikeout);
            ConditionalFormattingObject obj = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "2", "", true);
            obj.RowForeColor = Color.Black;
            obj.RowBackColor = Color.FromArgb(255, 255, 230, 230);
            obj.RowFont = f;
            gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj);

            // 마감오더 색상변경
            f = new Font(new FontFamily("Segoe UI"), 8.25f);
            ConditionalFormattingObject obj2 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "3", "", true);
            obj2.RowForeColor = Color.Black;
            obj2.RowBackColor = Color.FromArgb(255, 220, 255, 240);
            obj2.RowFont = f;
            gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj2);
            
            f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            ConditionalFormattingObject obj4 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "0", "", true);
            obj4.RowForeColor = Color.Black;
            obj4.RowFont = f;
            gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj4);

            f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            ConditionalFormattingObject obj5 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "1", "", true);
            obj5.RowForeColor = Color.Black;
            obj5.RowFont = f;
            gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj5);

            #endregion

        }

        #endregion

        #region 4. 컨텍스트 메뉴 기능
        
        private void mnuInspecting_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuSewing_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuOutsourcing_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuCutting_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void mnuWorksheet_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

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
        /// 행 자료삭제 (메인)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDel_Click(object sender, EventArgs e)
        {
            try
            {
                string str = "";
                //_gv1.EndEdit();

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
                    _bRtn = Controller.Orders.Delete(str);
                    RefleshWithCondition();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnInsert_Click: " + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 행 자료삭제 (컬러사이즈)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDelColor_Click(object sender, EventArgs e)
        {
            try
            {
                string str = "";
                //_gv1.EndEdit();

                // 삭제하기전 삭제될 데이터 확인
                foreach (GridViewRowInfo row in gvColorSize.SelectedRows)
                {
                    if (string.IsNullOrEmpty(str)) str = Convert.ToInt32(row.Cells["Idx"].Value).ToString();
                    else str += "," + Convert.ToInt32(row.Cells["Idx"].Value).ToString();
                }

                // 해당 자료 삭제여부 확인후, 
                if (RadMessageBox.Show("Do you want to delete this item?\n(ID: " + str + ")", "Confirm",
                    MessageBoxButtons.YesNo, RadMessageIcon.Question) == DialogResult.Yes)
                {
                    // 삭제후 새로고침
                    _bRtn = Controller.OrderColor.Delete(str);
                    DataBinding_GV2(gvColorSize, Convert.ToInt32(Int.Members.GetCurrentRow(_gv1).Cells["Idx"].Value)); 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnInsert_Click: " + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 행 자료삭제 (공정)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuDelOperation_Click(object sender, EventArgs e)
        {
            try
            {
                string str = "";
                //_gv1.EndEdit();

                // 삭제하기전 삭제될 데이터 확인
                foreach (GridViewRowInfo row in gvOperation.SelectedRows)
                {
                    if (string.IsNullOrEmpty(str)) str = Convert.ToInt32(row.Cells["Idx"].Value).ToString();
                    else str += "," + Convert.ToInt32(row.Cells["Idx"].Value).ToString();
                }

                // 해당 자료 삭제여부 확인후, 
                if (RadMessageBox.Show("Do you want to delete this item?\n(ID: " + str + ")", "Confirm",
                    MessageBoxButtons.YesNo, RadMessageIcon.Question) == DialogResult.Yes)
                {
                    // 삭제후 새로고침
                    _bRtn = Controller.Operation.Delete(str);
                    DataBinding_GV3(gvOperation, Convert.ToInt32(Int.Members.GetCurrentRow(_gv1).Cells["Idx"].Value));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnInsert_Click: " + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 신규입력 (메인) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuNew_Click(object sender, EventArgs e)
        {
            try
            {
                //if (_gv1.RowCount > 0)
                //{
                    frmNewOrder newOrder = new frmNewOrder(this);
                    newOrder.ShowDialog();
                    
                    if (InsertedOrderRow != null)
                    {
                        RefleshWithCondition();
                        SetCurrentRow(_gv1, Convert.ToInt32(InsertedOrderRow["LastIdx"]));   // 신규입력된 행번호로 이동

                    }
                    
                //}
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 신규입력 (컬러사이즈)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuNewColor_Click(object sender, EventArgs e)
        {
            try
            {
                int orderIdx=0;
                int colorIdx=0;
                int sizeIdx1=0;
                int sizeIdx2 = 0;
                int sizeIdx3 = 0;
                int sizeIdx4 = 0;
                int sizeIdx5 = 0;
                int sizeIdx6 = 0;
                int sizeIdx7 = 0;
                int sizeIdx8 = 0;
                
                gvColorSize.EndEdit();
                GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);  // 현재 행번호 확인

                if (row.Cells["Idx"].Value != DBNull.Value) orderIdx = Convert.ToInt32(row.Cells["Idx"].Value.ToString());

                sizeIdx1 = lstSize[1].SizeIdx;
                sizeIdx2 = lstSize[2].SizeIdx;
                sizeIdx3 = lstSize[3].SizeIdx;
                sizeIdx4 = lstSize[4].SizeIdx;
                sizeIdx5 = lstSize[5].SizeIdx;
                sizeIdx6 = lstSize[6].SizeIdx;
                sizeIdx7 = lstSize[7].SizeIdx;
                sizeIdx8 = lstSize[8].SizeIdx;

                DataRow rows = Controller.OrderColor.Insert(orderIdx, colorIdx,
                                                           sizeIdx1, sizeIdx2, sizeIdx3, sizeIdx4, sizeIdx5, sizeIdx6, sizeIdx7, sizeIdx8);

                DataBinding_GV2(gvColorSize, Convert.ToInt32(row.Cells["Idx"].Value.ToString())); 
                //SetCurrentRow(gvColorSize, Convert.ToInt32(rows["LastIdx"]));   // 신규입력된 행번호로 이동
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }
        
        /// <summary>
        /// 신규입력 (공정)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuNewOperation_Click(object sender, EventArgs e)
        {
            try
            {
                int orderIdx = 0;
                
                gvOperation.EndEdit();
                GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);  // 현재 행번호 확인

                if (row.Cells["Idx"].Value != DBNull.Value) orderIdx = Convert.ToInt32(row.Cells["Idx"].Value.ToString());
                
                DataRow rows = Controller.Operation.Insert2(orderIdx);

                DataBinding_GV3(gvOperation, Convert.ToInt32(row.Cells["Idx"].Value.ToString()));
                //SetCurrentRow(gvColorSize, Convert.ToInt32(rows["LastIdx"]));   // 신규입력된 행번호로 이동
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 메인에서 신규자료가 입력된후, 입력된 행을 선택 포커싱한다 
        /// </summary>
        /// <param name="gv">선택하고자하는 그리드뷰</param>
        /// <param name="row">선택 행(Insert시 신규Idx를 리턴받는다)</param>
        private void SetCurrentRow(RadGridView gv, int row)
        {
            try
            {
                row = 0;
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

        /// <summary>
        /// 오더 마감
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCloseOrder_Click(object sender, EventArgs e)
        {
            if (RadMessageBox.Show("Do you want to close this order?", "Confirm", MessageBoxButtons.YesNo, RadMessageIcon.Question)
                == DialogResult.Yes)
            {
                bool result = Data.OrdersData.CloseOrder(Int.Members.GetCurrentRow(_gv1, "Idx"));
                if (result)
                    RadMessageBox.Show("Closed", "Info", MessageBoxButtons.OK, RadMessageIcon.Info);
            }
        }

        /// <summary>
        /// 오더 캔슬
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCancelOrder_Click(object sender, EventArgs e)
        {
            try
            {
                string str = "";
                //_gv1.EndEdit();

                // 삭제하기전 삭제될 데이터 확인
                foreach (GridViewRowInfo row in _gv1.SelectedRows)
                {
                    if (string.IsNullOrEmpty(str)) str = Convert.ToInt32(row.Cells["Idx"].Value).ToString();
                    else str += "," + Convert.ToInt32(row.Cells["Idx"].Value).ToString();
                }

                // 해당 자료 삭제여부 확인후, 
                if (RadMessageBox.Show("Do you want to cancel this item?\n(ID: " + str + ")", "Confirm",
                    MessageBoxButtons.YesNo, RadMessageIcon.Question) == DialogResult.Yes)
                {
                    bool result = Data.OrdersData.CancelOrder(Int.Members.GetCurrentRow(_gv1, "Idx"));
                    if (result)
                        RadMessageBox.Show("Canceled", "Info", MessageBoxButtons.OK, RadMessageIcon.Info);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnInsert_Click: " + ex.Message.ToString());
            }

        }

        /// <summary>
        /// 선적내역 등록수정창 열기 (오더와 별도 테이블 저장 > 본 ERP에서 분리해야됨)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuShipment_Click(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// 패턴 요청
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuPattern_Click(object sender, EventArgs e)
        {
            try
            {
                // 파일번호 입력하지 않았을 경우
                if (string.IsNullOrEmpty(Int.Members.GetCurrentRow(_gv1, "Fileno", "")))
                {
                    RadMessageBox.Show("Please input the file number", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }
                
                // 스타일 입력안되었을 경우
                if (string.IsNullOrEmpty(Int.Members.GetCurrentRow(_gv1, "Styleno", "")))
                {
                    RadMessageBox.Show("Please input the style#", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }
                if (Int.Members.GetCurrentRow(_gv1, "SizeGroupIdx") <= 0)
                {
                    RadMessageBox.Show("Please input the Size Group", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }

                // 파일번호, 오더수량, 금액이 정상 입력되었으면 
                if (!string.IsNullOrEmpty(Int.Members.GetCurrentRow(_gv1, "Fileno", "")) &&
                    !string.IsNullOrEmpty(Int.Members.GetCurrentRow(_gv1, "Styleno", "")) &&
                    Int.Members.GetCurrentRow(_gv1, "SizeGroupIdx") > 0
                    )
                {
                    // 패턴 요청창 열기 
                    frmPatternRequest frm = new frmPatternRequest(Int.Members.GetCurrentRow(_gv1, "Idx"),
                                                    Int.Members.GetCurrentRow(_gv1, "Fileno", ""),
                                                    Int.Members.GetCurrentRow(_gv1, "Styleno", ""),
                                                    Int.Members.GetCurrentRow(_gv1, "SizeGroupIdx"),
                                                    Int.Members.GetCurrentRow(_gv1, "Status")
                                                    );
                    frm.Text = "Pattern";
                    
                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        // RadMessageBox.Show(frm.OrderIdx.ToString());
                        // Production Status 갱신 
                        DataBinding_GV4(gvProduction, Int.Members.GetCurrentRow(_gv1, "Idx"));
                    }
                }
                else
                {
                    // 에러 메시지
                    if (string.IsNullOrEmpty(Int.Members.GetCurrentRow(_gv1, "Fileno", "")))
                    {
                        RadMessageBox.Show("Please input the File#");
                        return;
                    }
                   
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 오더 복사 (복사후 복사수량 알림)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnuCopyOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(Int.Members.GetCurrentRow(_gv1, "Fileno", "")))
                {
                    RadMessageBox.Show("Please input the file number", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }
                if (Int.Members.GetCurrentRow(_gv1, "SizeGroupIdx", 0f) <= 0.0)
                {
                    RadMessageBox.Show("Please input the Order Amount", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }

                // 파일번호, 오더수량, 금액이 정상 입력되어 있으면 
                if (!string.IsNullOrEmpty(Int.Members.GetCurrentRow(_gv1, "Fileno", "")) &&
                    Int.Members.GetCurrentRow(_gv1, "OrderQty") > 0 &&
                    Int.Members.GetCurrentRow(_gv1, "OrderAmount", 0f) > 0f)
                {
                    if (RadMessageBox.Show("Do you want to copy?", "Confirm", MessageBoxButtons.YesNo, RadMessageIcon.Question)
                        == DialogResult.Yes)
                    {
                        // 복사창 열기 
                        frmCopyOrder frm = new frmCopyOrder(Int.Members.GetCurrentRow(_gv1, "Idx"),
                                                        Int.Members.GetCurrentRow(_gv1, "Fileno", ""),
                                                        Int.Members.GetCurrentRow(_gv1, "OrderQty"),
                                                        Int.Members.GetCurrentRow(_gv1, "OrderAmount", 0f),
                                                        Int.Members.GetCurrentRow(_gv1, "ShipCompleted"));
                        frm.Text = "Copy Order";
                        if (frm.ShowDialog(this) == DialogResult.OK)
                        {
                            RadMessageBox.Show(frm.Copied.ToString() + " copied.", "Info", MessageBoxButtons.OK, RadMessageIcon.Info);
                            RefleshWithCondition();
                        }
                    }

                }
                else
                {
                    // 에러 메시지
                    if (string.IsNullOrEmpty(Int.Members.GetCurrentRow(_gv1, "Fileno", "")))
                    {
                        RadMessageBox.Show("Please input the File#");
                        return;
                    }
                    if (Int.Members.GetCurrentRow(_gv1, "OrderQty") <= 0)
                    {
                        RadMessageBox.Show("Please input the order Q'ty");
                        return;
                    }
                    if (Int.Members.GetCurrentRow(_gv1, "OrderAmount", 0f) <= 0f)
                    {
                        RadMessageBox.Show("Please input the order amount");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        #endregion

        #region 5. 데이터 조회 (바인딩 테스트후, 무겁고 느려서 직접 쿼리로 제어)

        /// <summary>
        /// Dropdownlist 데이터 로딩
        /// </summary>
        private void DataLoading_DDL()
        {

            // 부서    
            _dt = CommonController.Getlist(CommonValues.KeyName.DeptIdx).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                // 관리부와 임원은 모든 부서에 접근가능
                if (UserInfo.DeptIdx == 5 || UserInfo.DeptIdx == 6)
                {
                    deptName.Add(new DepartmentName(Convert.ToInt32(row["DeptIdx"]),
                                                row["DeptName"].ToString(),
                                                Convert.ToInt32(row["CostcenterIdx"])));
                }
                // 영업부는 해당 부서 데이터만 접근가능
                else
                {
                    if (Convert.ToInt32(row["DeptIdx"]) <= 0 || Convert.ToInt32(row["DeptIdx"]) == UserInfo.DeptIdx)
                    {
                        deptName.Add(new DepartmentName(Convert.ToInt32(row["DeptIdx"]),
                                                row["DeptName"].ToString(),
                                                Convert.ToInt32(row["CostcenterIdx"])));
                    }
                }
            }

            // 바이어
            _dt = CommonController.Getlist(CommonValues.KeyName.CustIdx).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                custName.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
            }

            // 나염업체
            embName.Add(new CustomerName(0, "", 0));
            _dt = CommonController.Getlist(CommonValues.KeyName.EmbelishId1).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                embName.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
            }

            // Vendor (구분 Sew업체로 연결) 
            embName.Add(new CustomerName(0, "", 0));
            _dt = CommonController.Getlist(CommonValues.KeyName.Vendor).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                lstVendor.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
            }

            // 코드
            _dt = CommonController.Getlist(CommonValues.KeyName.Codes).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                codeName.Add(new CodeContents(Convert.ToInt32(row["Idx"]),
                                            row["Contents"].ToString(),
                                            row["Classification"].ToString()));
            }

            

            // 사이즈 그룹 데이터로딩 
            _dt = Codes.Controller.SizeGroup.GetlistName().Tables[0]; //Getlist(0).Tables[0];
            foreach (DataRow row in _dt.Rows)
            {
                dataSetSizeGroup.DataTableSizeGroup.Rows.Add(Convert.ToInt32(row["SizeGroupIdx"]),
                                            row["Client"].ToString(),
                                            row["SizeGroupName"].ToString(),
                                            row["SizeIdx1"].ToString(),
                                            row["SizeIdx2"].ToString(),
                                            row["SizeIdx3"].ToString(),
                                            row["SizeIdx4"].ToString(),
                                            row["SizeIdx5"].ToString(),
                                            row["SizeIdx6"].ToString(),
                                            row["SizeIdx7"].ToString(),
                                            row["SizeIdx8"].ToString());
            }
            
            // Sewthread
            //lstSewthread.Add(new CustomerName(0, "", 0));
            _dt = Codes.Controller.SewThread.GetUsablelist(); // CommonController.Getlist(CommonValues.KeyName.SewThread).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                dataSetSizeGroup.DataTableSewThread.Rows.Add(Convert.ToInt32(row["SewThreadIdx"]),
                                            row["SewThreadCustIdx"].ToString(),
                                            row["SewThreadName"].ToString(),
                                            row["ColorIdx"].ToString());
            }

            // Username
            lstUser.Add(new CustomerName(0, "", 0));
            _dt = CommonController.Getlist(CommonValues.KeyName.User).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                lstUser.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]),
                                            row["UserName"].ToString(),
                                            Convert.ToInt32(row["DeptIdx"])));
            }

            // 컬러명 
            _dt = Codes.Controller.Color.GetUselist().Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                lstColor.Add(new CodeContents(Convert.ToInt32(row["ColorIdx"]),
                                            row["ColorName"].ToString(),
                                            ""));
            }
                        
            // 오더상태
            lstStatus.Add(new CodeContents(0, "", ""));
            lstStatus.Add(new CodeContents(1, "Progress", ""));
            lstStatus.Add(new CodeContents(2, "Canceled", ""));
            lstStatus.Add(new CodeContents(3, "Shipped", ""));
            // 생산진행상태
            lstProduction.Add(new CodeContents(0, "New Work", ""));
            lstProduction.Add(new CodeContents(1, "Printed Ticket", ""));
            lstProduction.Add(new CodeContents(2, "Finished Work", ""));
            lstProduction.Add(new CodeContents(3, "Canceled Work", ""));
        }

        /// <summary>
        /// 메인 검색버튼
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
                if (ddlCust.SelectedValue != null || ddlDept.SelectedValue != null
                    || ddlStatus.SelectedValue != null || ddlPrinting.SelectedValue != null
                    || !string.IsNullOrEmpty(txtFileno.Text) || !string.IsNullOrEmpty(txtStyle.Text))
                {
                    _searchKey = new Dictionary<CommonValues.KeyName, int>();

                    // 영업부인경우, 해당 부서만 조회할수 있도록 제한 
                    if (UserInfo.ReportNo < 9)
                        _searchKey.Add(CommonValues.KeyName.DeptIdx, UserInfo.DeptIdx);
                    else
                        _searchKey.Add(CommonValues.KeyName.DeptIdx, Convert.ToInt32(ddlDept.SelectedValue));

                    _searchKey.Add(CommonValues.KeyName.CustIdx, Convert.ToInt32(ddlCust.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Status, Convert.ToInt32(ddlStatus.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.EmbelishId1, Convert.ToInt32(ddlPrinting.SelectedValue));

                    DataBinding_GV1(2, _searchKey, txtFileno.Text.Trim(), txtStyle.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

        }

        /// <summary>
        /// 데이터 로딩 (메인)
        /// </summary>
        /// <param name="KeyCount">0:전체, 2:조건검색</param>
        /// <param name="SearchKey">RefleshWithCondition()에서 검색조건(key, value) 확인</param>
        /// <param name="fileno">검색조건: 파일번호</param>
        /// <param name="styleno">검색조건: 스타일번호</param>
        private void DataBinding_GV1(int KeyCount, Dictionary<CommonValues.KeyName, int> SearchKey, string fileno, string styleno)
        {
            try
            {
                //조회시간
                //Stopwatch sw = new Stopwatch();
                //sw.Start(); 

                _gv1.DataSource = null;
                
                _ds1 = Controller.Orders.Getlist(KeyCount, SearchKey, fileno, styleno);
                if (_ds1 != null)
                {
                    _gv1.DataSource = _ds1.Tables[0].DefaultView;
                    // 조회 후, 상태알림 및 설정적용
                    __main__.lblRows.Text = _gv1.RowCount.ToString() + " Rows"; 
                    _gv1.EnablePaging = CommonValues.enablePaging;
                    _gv1.AllowSearchRow = CommonValues.enableSearchRow; 
                }
                //sw.Stop();
                //lblTime.Text = sw.Elapsed.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBindingGv1: " + ex.Message.ToString());
            }
        }
        
        /// <summary>
        /// 데이터 로딩 (컬러사이즈) 
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="OrderIdx"></param>
        private void DataBinding_GV2(RadGridView gv, int OrderIdx)
        {
            try
            {
                gv.DataSource = null;

                _ds1 = Controller.OrderColor.Getlist(OrderIdx);
                if (_ds1 != null)
                {
                    gv.DataSource = _ds1.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBinding " + gv.Name + ": " + ex.Message.ToString());
            }
        }
        
        /// <summary>
        /// 데이터 로딩 (공정) 
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="OrderIdx"></param>
        private void DataBinding_GV3(RadGridView gv, int OrderIdx)
        {
            try
            {
                gv.DataSource = null;

                _ds1 = Controller.Operation.Getlist(OrderIdx);
                if (_ds1 != null)
                {
                    gv.DataSource = _ds1.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBinding " + gv.Name + ": " + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 데이터 로딩 (생산 진행현황) 
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="OrderIdx"></param>
        private void DataBinding_GV4(RadGridView gv, int OrderIdx)
        {
            try
            {
                _searchKey = new Dictionary<CommonValues.KeyName, int>();
                _searchKey.Add(CommonValues.KeyName.OrderIdx, OrderIdx);
                _searchKey.Add(CommonValues.KeyName.OperationIdx, 0);
                _searchKey.Add(CommonValues.KeyName.Status, 0);

                //DataBinding_GV4(2, _searchKey, "", null);

                gv.DataSource = null;

                _ds1 = Dev.Controller.WorkOrder.Getlist(_searchKey, "", "");
                if (_ds1 != null)
                {
                    gv.DataSource = _ds1.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBinding " + gv.Name + ": " + ex.Message.ToString());
            }
        }

        #endregion

        #region 6. 컨트롤 이벤트 및 기타 설정

        private void Clear_Shortcuts()
        {
            if (mnuNew != null) mnuNew.Shortcuts.Clear();
            if (mnuDel != null) mnuDel.Shortcuts.Clear();
            if (mnuHide != null) mnuHide.Shortcuts.Clear();
            if (mnuShow != null) mnuShow.Shortcuts.Clear();
            if (menuItem2 != null) menuItem2.Shortcuts.Clear();
            if (menuItem3 != null) menuItem3.Shortcuts.Clear();
            if (menuItem4 != null) menuItem4.Shortcuts.Clear();
            if (mnuWorksheet != null) mnuWorksheet.Shortcuts.Clear();
            if (mnuCutting != null) mnuCutting.Shortcuts.Clear();
            if (mnuOutsourcing != null) mnuOutsourcing.Shortcuts.Clear();
            if (mnuSewing != null) mnuSewing.Shortcuts.Clear();
            if (mnuInspecting != null) mnuInspecting.Shortcuts.Clear();
        }

        /// <summary>
        /// 메인폼 활성화시 컨텍스트 메뉴 로딩 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderMain_Activated(object sender, EventArgs e)
        {
            Config_ContextMenu();
        }

        /// <summary>
        /// 메인폼 비활성화시 생성된 모든 단축키 제거
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderMain_Deactivate(object sender, EventArgs e)
        {
            Clear_Shortcuts(); 
        }
                

        /// <summary>
        /// 데이터 업데이트 (메인)
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

                // 객체생성 및 값 할당
                _obj1           = new Controller.Orders(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value));
                _obj1.Idx       = Convert.ToInt32(row.Cells["Idx"].Value.ToString());
                _obj1.Fileno    = row.Cells["Fileno"].Value.ToString();
                _obj1.DeptIdx   = Convert.ToInt32(row.Cells["DeptIdx"].Value.ToString());
                if (row.Cells["Reorder"].Value != DBNull.Value) _obj1.Reorder = Convert.ToInt32(row.Cells["Reorder"].Value.ToString());
                if (row.Cells["ReorderReason"].Value != DBNull.Value) _obj1.ReorderReason = row.Cells["ReorderReason"].Value.ToString();
                if (row.Cells["Indate"].Value != DBNull.Value) _obj1.Indate    = Convert.ToDateTime(row.Cells["Indate"].Value);

                if (row.Cells["Buyer"].Value != DBNull.Value)           _obj1.Buyer = Convert.ToInt32(row.Cells["Buyer"].Value.ToString());
                if (row.Cells["Vendor"].Value != DBNull.Value) _obj1.Vendor = Convert.ToInt32(row.Cells["Vendor"].Value.ToString());
                if (row.Cells["Country"].Value != DBNull.Value) _obj1.Country = Convert.ToInt32(row.Cells["Country"].Value.ToString());
                if (row.Cells["Pono"].Value != DBNull.Value)            _obj1.Pono = row.Cells["Pono"].Value.ToString();
                if (row.Cells["Styleno"].Value != DBNull.Value)         _obj1.Styleno = row.Cells["Styleno"].Value.ToString();
                if (row.Cells["SampleType"].Value != DBNull.Value) _obj1.SampleType = row.Cells["SampleType"].Value.ToString();
                if (row.Cells["InspType"].Value != DBNull.Value) _obj1.InspType = row.Cells["InspType"].Value.ToString();

                if (row.Cells["Season"].Value != DBNull.Value)          _obj1.Season = row.Cells["Season"].Value.ToString();
                if (row.Cells["Description"].Value != DBNull.Value)     _obj1.Description = row.Cells["Description"].Value.ToString();
                if (row.Cells["DeliveryDate"].Value != DBNull.Value)    _obj1.DeliveryDate = Convert.ToDateTime(row.Cells["DeliveryDate"].Value);
                if (row.Cells["IsPrinting"].Value != DBNull.Value)      _obj1.IsPrinting = Convert.ToInt32(row.Cells["IsPrinting"].Value.ToString());
                if(row.Cells["EmbelishId1"].Value!= DBNull.Value)       _obj1.EmbelishId1 = Convert.ToInt32(row.Cells["EmbelishId1"].Value);
                if (row.Cells["EmbelishId2"].Value != DBNull.Value)     _obj1.EmbelishId2 = Convert.ToInt32(row.Cells["EmbelishId2"].Value);
                if (row.Cells["SizeGroupIdx"].Value != DBNull.Value) _obj1.SizeGroupIdx = Convert.ToInt32(row.Cells["SizeGroupIdx"].Value);
                if (row.Cells["SewThreadIdx"].Value != DBNull.Value) _obj1.SewThreadIdx = Convert.ToInt32(row.Cells["SewThreadIdx"].Value);

                if (row.Cells["OrderQty"].Value != DBNull.Value)        _obj1.OrderQty = Convert.ToInt32(row.Cells["OrderQty"].Value.ToString());
                if (row.Cells["OrderPrice"].Value != DBNull.Value)      _obj1.OrderPrice = Convert.ToDouble(row.Cells["OrderPrice"].Value.ToString());
                if (row.Cells["OrderAmount"].Value != DBNull.Value)     _obj1.OrderAmount = Convert.ToDouble(row.Cells["OrderAmount"].Value.ToString());

                if (row.Cells["Remark"].Value != DBNull.Value) _obj1.Remark = row.Cells["Remark"].Value.ToString();
                if (row.Cells["TeamRequestedDate"].Value != DBNull.Value) _obj1.TeamRequestedDate = Convert.ToDateTime(row.Cells["TeamRequestedDate"].Value);
                if (row.Cells["SplConfirmedDate"].Value != DBNull.Value) _obj1.SplConfirmedDate = Convert.ToDateTime(row.Cells["SplConfirmedDate"].Value);
                
                if (row.Cells["Status"].Value != DBNull.Value)          _obj1.Status = Convert.ToInt32(row.Cells["Status"].Value.ToString());

                // 업데이트 (오더캔슬, 선적완료 상태가 아닐경우)
                if (_obj1.Status != 2 && _obj1.Status != 3) _bRtn=_obj1.Update();

                // 변경된 사이즈 그룹의 사이즈 정보 갱신 
                lstSize.Clear(); // 기존 저장된 사이즈 초기화 
                GetSizes(_gv1); // 하단 Color Size 데이터용 Size 정보 업데이트 
                GV2_CreateColumn(gvColorSize);
            }
            catch (Exception ex)
            {
                Console.WriteLine("GV1_Update: " + ex.Message.ToString());
            }
            
        }

        /// <summary>
        /// 데이터 업데이트 (컬러사이즈)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvColorSize_Update(object sender, GridViewCellEventArgs e)
        {
            _bRtn = false;

            try
            {
                RadGridView gv = gvColorSize;

                gv.EndEdit();
                GridViewRowInfo row = Int.Members.GetCurrentRow(gv);  // 현재 행번호 확인

                // 객체생성 및 값 할당
                _obj2 = new Controller.OrderColor(Convert.ToInt32(row.Cells["Idx"].Value));
                _obj2.Idx = Convert.ToInt32(row.Cells["Idx"].Value.ToString());
                _obj2.OrderIdx = Convert.ToInt32(row.Cells["OrderIdx"].Value.ToString());
                if (row.Cells["ColorIdx"].Value != DBNull.Value) _obj2.ColorIdx = Convert.ToInt32(row.Cells["ColorIdx"].Value.ToString());
                if (row.Cells["SizeIdx1"].Value != DBNull.Value) _obj2.SizeIdx1 = Convert.ToInt32(row.Cells["SizeIdx1"].Value.ToString());
                if (row.Cells["SizeIdx2"].Value != DBNull.Value) _obj2.SizeIdx2 = Convert.ToInt32(row.Cells["SizeIdx2"].Value.ToString());
                if (row.Cells["SizeIdx3"].Value != DBNull.Value) _obj2.SizeIdx3 = Convert.ToInt32(row.Cells["SizeIdx3"].Value.ToString());
                if (row.Cells["SizeIdx4"].Value != DBNull.Value) _obj2.SizeIdx4 = Convert.ToInt32(row.Cells["SizeIdx4"].Value.ToString());
                if (row.Cells["SizeIdx5"].Value != DBNull.Value) _obj2.SizeIdx5 = Convert.ToInt32(row.Cells["SizeIdx5"].Value.ToString());
                if (row.Cells["SizeIdx6"].Value != DBNull.Value) _obj2.SizeIdx6 = Convert.ToInt32(row.Cells["SizeIdx6"].Value.ToString());
                if (row.Cells["SizeIdx7"].Value != DBNull.Value) _obj2.SizeIdx7 = Convert.ToInt32(row.Cells["SizeIdx7"].Value.ToString());
                if (row.Cells["SizeIdx8"].Value != DBNull.Value) _obj2.SizeIdx8 = Convert.ToInt32(row.Cells["SizeIdx8"].Value.ToString());

                if (row.Cells["Pcs1"].Value != DBNull.Value) _obj2.Pcs1 = Convert.ToInt32(row.Cells["Pcs1"].Value.ToString());
                if (row.Cells["Pcs2"].Value != DBNull.Value) _obj2.Pcs2 = Convert.ToInt32(row.Cells["Pcs2"].Value.ToString());
                if (row.Cells["Pcs3"].Value != DBNull.Value) _obj2.Pcs3 = Convert.ToInt32(row.Cells["Pcs3"].Value.ToString());
                if (row.Cells["Pcs4"].Value != DBNull.Value) _obj2.Pcs4 = Convert.ToInt32(row.Cells["Pcs4"].Value.ToString());
                if (row.Cells["Pcs5"].Value != DBNull.Value) _obj2.Pcs5 = Convert.ToInt32(row.Cells["Pcs5"].Value.ToString());
                if (row.Cells["Pcs6"].Value != DBNull.Value) _obj2.Pcs6 = Convert.ToInt32(row.Cells["Pcs6"].Value.ToString());
                if (row.Cells["Pcs7"].Value != DBNull.Value) _obj2.Pcs7 = Convert.ToInt32(row.Cells["Pcs7"].Value.ToString());
                if (row.Cells["Pcs8"].Value != DBNull.Value) _obj2.Pcs8 = Convert.ToInt32(row.Cells["Pcs8"].Value.ToString());

                // 업데이트
                _bRtn = _obj2.Update();
            }
            catch (Exception ex)
            {
                Console.WriteLine("gvColorSize_Update: " + ex.Message.ToString());
            }

        }

        /// <summary>
        /// 공정 그리드뷰 클릭 시, 입력된 내역이 있는지 확인 후, 없으면 신규입력 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOperation_Click(object sender, EventArgs e)
        {
            if (gvOperation.RowCount <= 0)
            {
                if (RadMessageBox.Show("There's no operation data.\nWould you like to input the operation data?", "Confirm", MessageBoxButtons.YesNo, RadMessageIcon.Question) 
                    == DialogResult.Yes)
                {
                    bool result = Controller.Operation.Insert(Int.Members.GetCurrentRow(_gv1, "Idx"));
                    DataBinding_GV3(gvOperation, Int.Members.GetCurrentRow(_gv1, "Idx"));
                }
            }
        }

        /// <summary>
        /// 데이터 업데이트 (공정)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOperation_Update(object sender, GridViewCellEventArgs e)
        {
            _bRtn = false;

            try
            {
                RadGridView gv = gvOperation;

                gv.EndEdit();
                GridViewRowInfo row = Int.Members.GetCurrentRow(gv);  // 현재 행번호 확인

                // 객체생성 및 값 할당
                _obj3 = new Controller.Operation(Convert.ToInt32(row.Cells["Idx"].Value));
                _obj3.Idx = Convert.ToInt32(row.Cells["Idx"].Value.ToString());
                _obj3.OrderIdx = Convert.ToInt32(row.Cells["OrderIdx"].Value.ToString());
                if (row.Cells["OperationIdx"].Value != DBNull.Value) _obj3.OperationIdx = Convert.ToInt32(row.Cells["OperationIdx"].Value.ToString());
                if (row.Cells["Priority"].Value != DBNull.Value) _obj3.Priority = Convert.ToInt32(row.Cells["Priority"].Value.ToString());
                
                // 업데이트
                _bRtn = _obj3.Update();
            }
            catch (Exception ex)
            {
                Console.WriteLine("gvOperation_Update: " + ex.Message.ToString());
            }

        }

        private void gvProduction_HyperlinkOpened(object sender, HyperlinkOpenedEventArgs e)
        {
            CommonController.Close_All_Children(this, "PatternMain");
            PatternMain form = new PatternMain(__main__, e.Cell.Value.ToString());
            form.Text = DateTime.Now.ToLongTimeString();
            form.MdiParent = this.MdiParent;
            form.Show();
        }

        /// <summary>
        /// 컨텍스트 메뉴 업데이트 (메인) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_MouseEnter(object sender, EventArgs e)
        {
            _gv1 = (RadGridView)sender;
            Config_ContextMenu();

            // 멀티행 선택시 컨텍스트 메뉴에서 오더복사 disable
            if (_gv1.SelectedRows.Count > 1)
            {
                contextMenu.Items[6].Enabled = false;
                contextMenu.Items[6].Shortcuts.Clear();
            }
            else
            {
                contextMenu.Items[6].Enabled = true;
                contextMenu.Items[6].Shortcuts.Clear();
                contextMenu.Items[6].Shortcuts.Add(new RadShortcut(Keys.Control, Keys.C));
            }
        }

        /// <summary>
        /// 컨텍스트 메뉴 업데이트 (컬러사이즈) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvColorSize_MouseEnter(object sender, EventArgs e)
        {
            Config_ContextMenu_Color();
        }

        /// <summary>
        /// 컨텍스트 메뉴 업데이트 (공정) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOperation_MouseEnter(object sender, EventArgs e)
        {
            Config_ContextMenu_Operation();
        }

        /// <summary>
        /// 컬러사이즈 그리드뷰의 제목 변경으로 위한 사이즈 셋을 불러온다 
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GetSizes(RadGridView gv)
        {
            _bRtn = false;
            try
            {
                _gv1.EndEdit();
                GridViewRowInfo row = Int.Members.GetCurrentRow(gv);  // 현재 행번호 확인

                DataRow dr = Codes.Controller.SizeGroup.Get(Convert.ToInt32(row.Cells["SizeGroupIdx"].Value)); 

                if (dr != null)
                {
                    lstSize.Add(new Codes.Controller.Sizes(0,""));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx1"]), dr["Size1"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx2"]), dr["Size2"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx3"]), dr["Size3"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx4"]), dr["Size4"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx5"]), dr["Size5"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx6"]), dr["Size6"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx7"]), dr["Size7"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx8"]), dr["Size8"].ToString()));
                }   
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message); 
            }
        }

        /// <summary>
        /// 그리드뷰 셀포맷팅 (메인) 
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
            }
            
        }

        /// <summary>
        /// 그리드뷰 셀 생성후, DDL설정 (메인)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterTemplate_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            RadMultiColumnComboBoxElement meditor = e.ActiveEditor as RadMultiColumnComboBoxElement;
            Console.WriteLine(gvColorSize.RowCount.ToString()); 

            if (gvColorSize.RowCount > 0)
            {
                if (meditor != null && meditor.ValueMember == "SizeGroupIdx")
                {
                    meditor.Enabled = false; 
                }
            }
            else
            {
                if (meditor != null)
                {
                    meditor.Enabled = true;
                    meditor.AutoSizeDropDownToBestFit = true;
                    meditor.AutoSizeDropDownHeight = true;
                    
                    if (meditor.ValueMember == "SizeGroupIdx")
                    {
                        meditor.AutoSizeDropDownColumnMode = BestFitColumnMode.AllCells;
                        meditor.EditorControl.Columns["SizeGroupIdx"].HeaderText = "ID";
                        meditor.EditorControl.Columns["Client"].HeaderText = "Buyer";
                        meditor.EditorControl.Columns["SizeGroupName"].HeaderText = "Size Group";
                        meditor.EditorControl.Columns["SizeIdx1"].HeaderText = "Size1";
                        meditor.EditorControl.Columns["SizeIdx2"].HeaderText = "Size2";
                        meditor.EditorControl.Columns["SizeIdx3"].HeaderText = "Size3";
                        meditor.EditorControl.Columns["SizeIdx4"].HeaderText = "Size4";
                        meditor.EditorControl.Columns["SizeIdx5"].HeaderText = "Size5";
                        meditor.EditorControl.Columns["SizeIdx6"].HeaderText = "Size6";
                        meditor.EditorControl.Columns["SizeIdx7"].HeaderText = "Size7";
                        meditor.EditorControl.Columns["SizeIdx8"].HeaderText = "Size8";

                        //meditor.EditorControl.Columns["SizeGroupIdx"].Width = 500;

                    }
                    else if (meditor.ValueMember == "SewThreadIdx")
                    {
                        meditor.AutoSizeDropDownColumnMode = BestFitColumnMode.AllCells;
                        meditor.EditorControl.Columns["SewThreadIdx"].HeaderText = "ID";
                        meditor.EditorControl.Columns["SewThreadCustIdx"].HeaderText = "Customer";
                        meditor.EditorControl.Columns["SewThreadName"].HeaderText = "SewThread";
                        meditor.EditorControl.Columns["ColorIdx"].HeaderText = "Color";
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
        /// 행 선택시 - 오더캔슬, 마감일때 편집 불가능하도록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_SelectionChanged(object sender, EventArgs e)
        {
            lstSize.Clear(); // 기존 저장된 사이즈 초기화 
            GetSizes(_gv1); // 하단 Color Size 데이터용 Size 정보 업데이트 

            if (Int.Members.GetCurrentRow(_gv1, "Status") == 2
                || Int.Members.GetCurrentRow(_gv1, "Status") == 3)
            {
                Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = true;
            }
            else
            {
                Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = false;
            }

            // 오더별 사이즈 그룹이 다르므로 컬러사이즈 제목 갱신후 자료갱신
            GV2_CreateColumn(gvColorSize); 
            DataBinding_GV2(gvColorSize, Int.Members.GetCurrentRow(_gv1, "Idx"));

            // 공정 자료갱신
            DataBinding_GV3(gvOperation, Int.Members.GetCurrentRow(_gv1, "Idx"));

            // 생산 진행현황 갱신 
            DataBinding_GV4(gvProduction, Int.Members.GetCurrentRow(_gv1, "Idx"));
        }

        /// <summary>
        /// 공정 순서(Up/Down) 변경버튼 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GV_CommandCellClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                if (e.Column.Name == "OperationUp")
                {
                    // 해당 row index가 2번째 행부터 
                    if (gvOperation.CurrentRow.Index > 0)
                    {
                        // 행 swap (index from, index to) 
                        SwapPriority(Convert.ToInt32(gvOperation.ChildRows[gvOperation.CurrentRow.Index].Cells["Idx"].Value),
                                    Convert.ToInt32(gvOperation.ChildRows[gvOperation.CurrentRow.Index - 1].Cells["Idx"].Value),
                                    Convert.ToInt32(gvOperation.ChildRows[gvOperation.CurrentRow.Index].Cells["Priority"].Value),
                                    Convert.ToInt32(gvOperation.ChildRows[gvOperation.CurrentRow.Index - 1].Cells["Priority"].Value));
                    }
                }
                else if (e.Column.Name == "OperationDown")
                {
                    // 해당 row index가 끝에서부터 2번째 행전부터 
                    if (gvOperation.CurrentRow.Index < gvOperation.RowCount - 1)
                    {
                        // 행 swap (index from, index to) 
                        SwapPriority(Convert.ToInt32(gvOperation.ChildRows[gvOperation.CurrentRow.Index].Cells["Idx"].Value),
                                    Convert.ToInt32(gvOperation.ChildRows[gvOperation.CurrentRow.Index + 1].Cells["Idx"].Value),
                                    Convert.ToInt32(gvOperation.ChildRows[gvOperation.CurrentRow.Index].Cells["Priority"].Value),
                                    Convert.ToInt32(gvOperation.ChildRows[gvOperation.CurrentRow.Index + 1].Cells["Priority"].Value));
                    }
                }
            }
            catch (Exception ex)
            {
                __main__.lblDescription.Text = ex.Message.ToString();
            }

        }

        /// <summary>
        /// 행 순서변경 (공정)
        /// </summary>
        /// <param name="indexFrom">변경될 행번호</param>
        /// <param name="indexTo">변경할 행번호</param>
        /// <param name="priorityFrom">변경될 우선순위</param>
        /// <param name="priorityTo">변경할 우선순위</param>
        private void SwapPriority(int indexFrom, int indexTo, int priorityFrom, int priorityTo)
        {
            bool result = Controller.Operation.SwapPriority(indexFrom, indexTo, priorityFrom, priorityTo);
            DataBinding_GV3(gvOperation, Int.Members.GetCurrentRow(_gv1, "Idx"));
        }

        #endregion

        #region 7. 기능 멤버 (그리드뷰 레이아웃 저장)

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
                if (System.IO.File.Exists(path))
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
                
    }
}