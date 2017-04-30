using Int.Code;
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

namespace Dev.Sales
{
    /// <summary>
    /// 샘플오더 관리화면: 오더입력수정 및 작업지시등
    /// </summary>
    public partial class OrderMain : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                     // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, int> _searchKey;        // 쿼리값 value, 쿼리항목 key로 전달 
        private enum OrderStatus { Normal, Progress, Cancel, Close };   // 오더상태값
        private bool _bRtn;                                             // 쿼리결과 리턴
        private DataSet _ds1 = null;                                    // 기본 데이터셋
        private DataTable _dt = null;                                   // 기본 데이터테이블
        private Controller.Orders _obj1 = null;                               // 현재 생성된 객체 
        private RadContextMenu contextMenu;                             // 컨텍스트 메뉴
        private List<CodeContents> lstStatus = new List<CodeContents>();        // 오더상태
        private List<DepartmentName> deptName = new List<DepartmentName>();     // 부서
        private List<CustomerName> custName = new List<CustomerName>();         // 거래처
        private List<CustomerName> lstVendor = new List<CustomerName>();         // vendor
        private List<CustomerName> embName = new List<CustomerName>();          // 나염업체
        private List<CodeContents> codeName = new List<CodeContents>();         // 코드
        private List<CodeContents> lstOrigin = new List<CodeContents>();         // Origin country 
        private List<CodeContents> lstBrand = new List<CodeContents>();         // 브랜드
        private List<CodeContents> lstIsPrinting = new List<CodeContents>();    // 나염여부
        private List<CustomerName> lstUser = new List<CustomerName>();         // 유저명
        private List<CustomerName> lstSewthread = new List<CustomerName>();         // sewthread

        private string _layoutfile = "/GVLayoutOrders.xml";
        public DataRow InsertedOrderRow = null;

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
            Config_ContextMenu();       // 중앙 그리드뷰 컨텍스트 생성 설정 
            LoadGVLayout();             // 그리드뷰 레이아웃 복구 
            // DataBinding_GV1(0, null, "", "");   // 중앙 그리드뷰 데이터 
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
        /// 그리드뷰 컨텍스트 메뉴 생성  
        /// </summary>
        private void Config_ContextMenu()
        {
            contextMenu = new RadContextMenu();

            // 오더 신규 입력
            RadMenuItem mnuNew = new RadMenuItem("New Order");
            //mnuNew.Image = Properties.Resources._20_20;
            mnuNew.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.N));
            mnuNew.Click += new EventHandler(mnuNew_Click);

            // 오더 삭제
            //contextMenu = new RadContextMenu();
            RadMenuItem mnuDel = new RadMenuItem("Remove Order");
            //mnuNew.Image = Properties.Resources._20_20;
            mnuDel.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.D));
            mnuDel.Click += new EventHandler(mnuDel_Click);

            // 열 숨기기
            //contextMenu = new RadContextMenu();
            RadMenuItem mnuHide = new RadMenuItem("Hide Column");
            mnuHide.Click += new EventHandler(mnuHide_Click);

            // 열 보이기
            //contextMenu = new RadContextMenu();
            RadMenuItem mnuShow = new RadMenuItem("Show all Columns");
            mnuShow.Click += new EventHandler(mnuShow_Click);


            // 오더복사
            RadMenuItem menuItem2 = new RadMenuItem("Copy Order");
            menuItem2.Click += new EventHandler(mnuCopyOrder_Click);
            menuItem2.Image = Properties.Resources.copy;
            menuItem2.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.C));

            // 오더캔슬
            RadMenuItem menuItem3 = new RadMenuItem("Cancel Order");
            menuItem3.Image = Properties.Resources.cancel;
            menuItem3.ForeColor = Color.Red;
            menuItem3.Click += new EventHandler(mnuCancelOrder_Click);
            menuItem3.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.D));

            // 오더종료
            RadMenuItem menuItem4 = new RadMenuItem("Close Order");
            menuItem4.Image = Properties.Resources.locked;
            menuItem4.ForeColor = Color.Blue;
            menuItem4.Click += new EventHandler(mnuCloseOrder_Click);
            menuItem4.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.C));

            // 선적 입력, 수정 
            RadMenuItem mnuShipment = new RadMenuItem("Edit Shipment Data");
            mnuShipment.Image = Properties.Resources._20_20;
            mnuShipment.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.S));
            mnuShipment.Click += new EventHandler(mnuShipment_Click);

            // 작업지시서
            RadMenuItem mnuWorksheet = new RadMenuItem("Edit Worksheets");
            // mnuShipment.Image = Properties.Resources._20_20;
            mnuWorksheet.Shortcuts.Add(new RadShortcut(Keys.Alt, Keys.S));
            mnuWorksheet.Click += new EventHandler(mnuShipment_Click);

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

            contextMenu.Items.Add(mnuShipment);
            contextMenu.Items.Add(mnuWorksheet);

        }

        /// <summary>
        /// 그리드뷰 컨텍스트 메뉴 생성 (Color, Size) 
        /// </summary>
        private void Config_ContextMenu_ColorSize()
        {
            contextMenu = new RadContextMenu();

            // 신규 입력
            RadMenuItem mnuNewColor = new RadMenuItem("New Color");
            mnuNewColor.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.N));
            mnuNewColor.Click += new EventHandler(mnuNew_Click);

            // 삭제
            RadMenuItem mnuDelColor = new RadMenuItem("Remove Color");
            mnuDelColor.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.D));
            mnuDelColor.Click += new EventHandler(mnuDel_Click);
            
            // 분리선
            RadMenuSeparatorItem separator = new RadMenuSeparatorItem();
            separator.Tag = "seperator";

            // 컨텍스트 추가 
            contextMenu.Items.Add(mnuNewColor);
            contextMenu.Items.Add(mnuDelColor);
            contextMenu.Items.Add(separator);
                      
        }

        /// <summary>
        /// 그리드뷰 컬럼 생성
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
            cboSizeGrp.Width = 200;
            gv.Columns.Insert(16, cboSizeGrp);
            
            GridViewMultiComboBoxColumn cboSewThread = new GridViewMultiComboBoxColumn();
            cboSewThread.Name = "SewThreadIdx";
            cboSewThread.DataSource = dataSetSizeGroup.DataTableSewThread;
            cboSewThread.ValueMember = "SewThreadIdx";
            cboSewThread.DisplayMember = "SewThreadName";
            cboSewThread.FieldName = "SewThreadIdx";
            cboSewThread.HeaderText = "SewThread";
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
        /// 자료 새로입력 (로그인 사용자의 부서번호로 기본 입력된다) 
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
        /// 신규자료가 입력된후, 입력된 행을 선택 포커싱한다 
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
            try
            {
                // 파일번호 입력하지 않았을 경우
                if (string.IsNullOrEmpty(Int.Members.GetCurrentRow(_gv1, "Fileno", "")))
                {
                    RadMessageBox.Show("Please input the file number", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }

                // 오더 금액 입력안되었을 경우
                if (Int.Members.GetCurrentRow(_gv1, "OrderAmount", 0f) <= 0.0)
                {
                    RadMessageBox.Show("Please input the Order Amount", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }

                // 파일번호, 오더수량, 금액이 정상 입력되었으면 
                if (!string.IsNullOrEmpty(Int.Members.GetCurrentRow(_gv1, "Fileno", "")) &&
                    Int.Members.GetCurrentRow(_gv1, "OrderQty") > 0 &&
                    Int.Members.GetCurrentRow(_gv1, "OrderAmount", 0f) > 0f)
                {
                    // shipment내역 입력창 열기 
                    frmShipment frm = new frmShipment(Int.Members.GetCurrentRow(_gv1, "Idx"),
                                                    Int.Members.GetCurrentRow(_gv1, "Fileno", ""),
                                                    Int.Members.GetCurrentRow(_gv1, "OrderQty"),
                                                    Int.Members.GetCurrentRow(_gv1, "OrderAmount", 0f),
                                                    Int.Members.GetCurrentRow(_gv1, "ShipCompleted"),
                                                    Int.Members.GetCurrentRow(_gv1, "Status")
                                                    );
                    frm.Text = "Shipment";
                    frm.ShowDialog(this);
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
                if (Int.Members.GetCurrentRow(_gv1, "OrderAmount", 0f) <= 0.0)
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

            // 오더상태
            lstStatus.Add(new CodeContents(0, "", ""));
            lstStatus.Add(new CodeContents(1, "Progress", ""));
            lstStatus.Add(new CodeContents(2, "Canceled", ""));
            lstStatus.Add(new CodeContents(3, "Shipped", ""));
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
        /// 메인 그리드뷰 데이터 로딩
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
            if (meditor != null)
            {
                meditor.AutoSizeDropDownToBestFit = true;
                meditor.AutoSizeDropDownHeight = true;

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
        /// 그리드뷰 행 클릭 (다중선택시 오더복사 disable)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GV_Click(object sender, EventArgs e)
        {
            _gv1 = (RadGridView)sender;

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
        /// 행 선택시 - 오더캔슬, 마감일때 편집 불가능하도록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_SelectionChanged(object sender, EventArgs e)
        {

            if (Int.Members.GetCurrentRow(_gv1, "Status") == 2
                || Int.Members.GetCurrentRow(_gv1, "Status") == 3)
            {
                Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = true;
            }
            else
            {
                Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = false;
            }
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

        private void twColorSize_Enter(object sender, EventArgs e)
        {
            //GV1_CreateColumn(_gv1);     // 그리드뷰 생성
            //GV1_LayoutSetting(_gv1);    // 중앙 그리드뷰 설정 
            //Config_ContextMenu();       // 중앙 그리드뷰 컨텍스트 생성 설정 
            
            // 데이터 조회 

        }
    }

}
