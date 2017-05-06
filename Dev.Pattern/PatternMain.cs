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

namespace Dev.Pattern
{
    /// <summary>
    /// 샘플오더 관리화면: 오더입력수정 및 작업지시등
    /// </summary>
    public partial class PatternMain : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                             // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, int> _searchKey;                // 쿼리값 value, 쿼리항목 key로 전달 
        public DataRow InsertedOrderRow = null;

        private enum OrderStatus { Normal, Progress, Cancel, Close };           // 오더상태값
        private bool _bRtn;                                                     // 쿼리결과 리턴
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        private DataTable _dt = null;                                           // 기본 데이터테이블
        private Controller.Pattern _obj1 = null;                                // 현재 생성된 객체 
        //private Controller.OrderColor _obj2 = null;                           // 현재 생성된 객체 
        private RadContextMenu contextMenu;                                     // 컨텍스트 메뉴
        private List<CodeContents> lstStatus = new List<CodeContents>();        // 오더상태
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
        private List<CodeContents> lstOperation = new List<CodeContents>();     // 공정명
        private List<CodeContents> sizeName = new List<CodeContents>();
        private string _layoutfile = "/GVLayoutPattern.xml";
        private string _workOrderIdx; 

        //RadMenuItem mnuNew, mnuDel, mnuHide, mnuShow, menuItem2, menuItem3, menuItem4, mnuWorksheet, mnuCutting, mnuOutsourcing, mnuSewing, mnuInspecting = null;

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public PatternMain(InheritMDI main, string WorkOrderIdx)
        {
            base.InitializeComponent(); // parent 컴포넌트에 접근하기 위해 컨트롤을 public으로 지정 > 향후 수정 필요 (todo) 
            InitializeComponent();
            __main__ = main;            // MDI 연결 
            _gv1 = this.gvOrderActual;  // 그리드뷰 일반화를 위해 변수 별도 저장
            _workOrderIdx = WorkOrderIdx; 
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

            // 다른 폼으로부터 전달된 Work ID가 있을 경우, 해당 ID로 조회 
            if (!string.IsNullOrEmpty(_workOrderIdx))
            {
                _searchKey = new Dictionary<CommonValues.KeyName, int>();
                _searchKey.Add(CommonValues.KeyName.CustIdx, 0);
                _searchKey.Add(CommonValues.KeyName.Status, 0);
                _searchKey.Add(CommonValues.KeyName.Size, 0);

                DataBinding_GV1(2, _searchKey, _workOrderIdx, txtStyle.Text.Trim());
            }
        }
        
        /// <summary>
        /// 상단 검색을 위한 Dropdownlist 생성
        /// </summary>
        private void Config_DropDownList()
        {
            // 사이즈 
            ddlSize.DataSource = sizeName;
            ddlSize.DisplayMember = "Contents";
            ddlSize.ValueMember = "CodeIdx";
            ddlSize.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlSize.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

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

            GridViewTextBoxColumn orderidx = new GridViewTextBoxColumn();
            orderidx.Name = "OrderIdx";
            orderidx.FieldName = "OrderIdx";
            orderidx.IsVisible = false; 
            gv.Columns.Insert(1, orderidx);

            GridViewComboBoxColumn cboBuyer = new GridViewComboBoxColumn();
            cboBuyer.Name = "Buyer";
            cboBuyer.DataSource = custName;
            cboBuyer.ValueMember = "CustIdx";
            cboBuyer.DisplayMember = "CustName";
            cboBuyer.FieldName = "Buyer";
            cboBuyer.HeaderText = "Buyer";
            cboBuyer.Width = 100;
            gv.Columns.Insert(2, cboBuyer);
            
            gv.Columns["Styleno"].Width = 130;
            gv.Columns["Styleno"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns["Styleno"].HeaderText = "Style#";

            gv.Columns["Fileno"].Width = 90;
            gv.Columns["Fileno"].HeaderText = "INT File #";
            gv.Columns["Fileno"].ReadOnly = true;

            GridViewTextBoxColumn WorkOrderIdx = new GridViewTextBoxColumn();
            WorkOrderIdx.Name = "WorkOrderIdx";
            WorkOrderIdx.FieldName = "WorkOrderIdx";
            WorkOrderIdx.HeaderText = "Work ID";
            WorkOrderIdx.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Insert(5, WorkOrderIdx);

            GridViewComboBoxColumn cboSize = new GridViewComboBoxColumn();
            cboSize.Name = "OrdSizeIdx";
            cboSize.DataSource = sizeName;
            cboSize.ValueMember = "CodeIdx";
            cboSize.DisplayMember = "Contents";
            cboSize.FieldName = "OrdSizeIdx";
            cboSize.HeaderText = "Size";
            cboSize.Width = 100;
            gv.Columns.Insert(6, cboSize);

            GridViewDateTimeColumn TechpackDate = new GridViewDateTimeColumn();
            TechpackDate.Name = "TechpackDate";
            TechpackDate.FieldName = "TechpackDate";
            TechpackDate.Width = 100;
            TechpackDate.TextAlignment = ContentAlignment.MiddleCenter;
            TechpackDate.FormatString = "{0:d}";
            TechpackDate.HeaderText = "Techpack Date";
            gv.Columns.Insert(7, TechpackDate);

            GridViewDateTimeColumn RequestedDate = new GridViewDateTimeColumn();
            RequestedDate.Name = "RequestedDate";
            RequestedDate.FieldName = "RequestedDate";
            RequestedDate.Width = 100;
            RequestedDate.TextAlignment = ContentAlignment.MiddleCenter;
            RequestedDate.FormatString = "{0:d}";
            RequestedDate.HeaderText = "RequestedDate";
            gv.Columns.Insert(8, RequestedDate);
            
            GridViewComboBoxColumn Requested = new GridViewComboBoxColumn();
            Requested.Name = "Requested";
            Requested.DataSource = lstUser;
            Requested.ValueMember = "CustIdx";
            Requested.DisplayMember = "CustName";
            Requested.FieldName = "Requested";
            Requested.HeaderText = "Requested";
            Requested.Width = 80;
            gv.Columns.Insert(9, Requested);

            GridViewDateTimeColumn ConfirmedDate = new GridViewDateTimeColumn();
            ConfirmedDate.Name = "ConfirmedDate";
            ConfirmedDate.FieldName = "ConfirmedDate";
            ConfirmedDate.Width = 100;
            ConfirmedDate.TextAlignment = ContentAlignment.MiddleCenter;
            ConfirmedDate.FormatString = "{0:d}";
            ConfirmedDate.HeaderText = "ConfirmedDate";
            gv.Columns.Insert(10, ConfirmedDate);

            GridViewComboBoxColumn Confirmed = new GridViewComboBoxColumn();
            Confirmed.Name = "Confirmed";
            Confirmed.DataSource = lstUser;
            Confirmed.ValueMember = "CustIdx";
            Confirmed.DisplayMember = "CustName";
            Confirmed.FieldName = "Confirmed";
            Confirmed.HeaderText = "Confirmed";
            Confirmed.Width = 80;
            gv.Columns.Insert(11, Confirmed);
            
            GridViewDateTimeColumn CompletedDate = new GridViewDateTimeColumn();
            CompletedDate.Name = "CompletedDate";
            CompletedDate.FieldName = "CompletedDate";
            CompletedDate.Width = 100;
            CompletedDate.TextAlignment = ContentAlignment.MiddleCenter;
            CompletedDate.FormatString = "{0:d}";
            CompletedDate.HeaderText = "CompletedDate";
            gv.Columns.Insert(12, CompletedDate);

            GridViewDateTimeColumn SentDate = new GridViewDateTimeColumn();
            SentDate.Name = "SentDate";
            SentDate.FieldName = "SentDate";
            SentDate.Width = 100;
            SentDate.TextAlignment = ContentAlignment.MiddleCenter;
            SentDate.FormatString = "{0:d}";
            SentDate.HeaderText = "SentDate";
            gv.Columns.Insert(13, SentDate);

            GridViewComboBoxColumn Received = new GridViewComboBoxColumn();
            Received.Name = "Received";
            Received.DataSource = lstUser;
            Received.ValueMember = "CustIdx";
            Received.DisplayMember = "CustName";
            Received.FieldName = "Received";
            Received.HeaderText = "Received";
            Received.Width = 80;
            gv.Columns.Insert(14, Received);

            GridViewComboBoxColumn status = new GridViewComboBoxColumn();
            status.Name = "Status";
            status.DataSource = lstStatus;
            status.DisplayMember = "Contents";
            status.ValueMember = "CodeIdx";
            status.FieldName = "Status";
            status.HeaderText = "Status";
            status.Width = 70;
            status.ReadOnly = true;
            gv.Columns.Insert(15, status);
            
            //GridViewTextBoxColumn SampleType = new GridViewTextBoxColumn();
            //SampleType.Name = "SampleType";
            //SampleType.FieldName = "SampleType";
            //SampleType.HeaderText = "SampleType";
            //SampleType.IsVisible = false; 
            //SampleType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            //gv.Columns.Insert(20, SampleType);

            //GridViewTextBoxColumn InspType = new GridViewTextBoxColumn();
            //InspType.Name = "InspType";
            //InspType.FieldName = "InspType";
            //InspType.HeaderText = "Inspection Type";
            //InspType.IsVisible = false;
            //InspType.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            //gv.Columns.Insert(21, InspType);
            
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
        
        #region 5. 데이터 조회 (바인딩 테스트후, 무겁고 느려서 직접 쿼리로 제어)

        /// <summary>
        /// Dropdownlist 데이터 로딩
        /// </summary>
        private void DataLoading_DDL()
        {
            
            // 바이어
            _dt = CommonController.Getlist(CommonValues.KeyName.CustAll).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                custName.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
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

            // 사이즈명 
            _dt = Dev.Codes.Controller.Sizes.GetUselist().Tables[0];
            
            sizeName = new List<CodeContents>();

            sizeName.Add(new CodeContents(0, "", "")); 
            foreach (DataRow row in _dt.Rows)
            {

                sizeName.Add(new CodeContents(Convert.ToInt32(row["SizeIdx"]),
                                        row["SizeName"].ToString(),
                                        ""));
            }
            

            // Username
            //lstUser.Add(new CustomerName(0, "", 0));
            //_dt = CommonController.Getlist(CommonValues.KeyName.User).Tables[0];

            //foreach (DataRow row in _dt.Rows)
            //{
            //    lstUser.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]),
            //                                row["UserName"].ToString(),
            //                                Convert.ToInt32(row["DeptIdx"])));
            //}

            // 오더상태
            lstStatus.Add(new CodeContents(0, "", ""));
            lstStatus.Add(new CodeContents(1, "Progress", ""));
            lstStatus.Add(new CodeContents(2, "Canceled", ""));
            lstStatus.Add(new CodeContents(3, "Shipped", ""));
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
                if (ddlCust.SelectedValue != null || ddlSize.SelectedValue != null
                    || ddlStatus.SelectedValue != null || ddlSize.SelectedValue != null
                    || !string.IsNullOrEmpty(txtFileno.Text) || !string.IsNullOrEmpty(txtStyle.Text))
                {
                    _searchKey = new Dictionary<CommonValues.KeyName, int>();

                    // 영업부인경우, 해당 부서만 조회할수 있도록 제한 
                    if (UserInfo.ReportNo < 9)
                        _searchKey.Add(CommonValues.KeyName.DeptIdx, UserInfo.DeptIdx);
                    else
                        _searchKey.Add(CommonValues.KeyName.DeptIdx, Convert.ToInt32(ddlSize.SelectedValue));

                    _searchKey.Add(CommonValues.KeyName.CustIdx, Convert.ToInt32(ddlCust.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Status, Convert.ToInt32(ddlStatus.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Size, Convert.ToInt32(ddlSize.SelectedValue));

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
                _gv1.DataSource = null;
                
                _ds1 = Controller.Pattern.Getlist(KeyCount, SearchKey, fileno, styleno);
                if (_ds1 != null)
                {
                    _gv1.DataSource = _ds1.Tables[0].DefaultView;
                    __main__.lblRows.Text = _gv1.RowCount.ToString() + " Rows"; 
                    _gv1.EnablePaging = CommonValues.enablePaging;
                    _gv1.AllowSearchRow = CommonValues.enableSearchRow; 
                }
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
        

        #endregion

        #region 6. 컨트롤 이벤트 및 기타 설정
        
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
                _obj1           = new Controller.Pattern(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value));
                _obj1.Idx       = Convert.ToInt32(row.Cells["Idx"].Value.ToString());
                //_obj1.Fileno    = row.Cells["Fileno"].Value.ToString();
                //_obj1.DeptIdx   = Convert.ToInt32(row.Cells["DeptIdx"].Value.ToString());
                //if (row.Cells["Reorder"].Value != DBNull.Value) _obj1.Reorder = Convert.ToInt32(row.Cells["Reorder"].Value.ToString());
                //if (row.Cells["ReorderReason"].Value != DBNull.Value) _obj1.ReorderReason = row.Cells["ReorderReason"].Value.ToString();
                //if (row.Cells["Indate"].Value != DBNull.Value) _obj1.Indate    = Convert.ToDateTime(row.Cells["Indate"].Value);

                //if (row.Cells["Buyer"].Value != DBNull.Value)           _obj1.Buyer = Convert.ToInt32(row.Cells["Buyer"].Value.ToString());
                //if (row.Cells["Vendor"].Value != DBNull.Value) _obj1.Vendor = Convert.ToInt32(row.Cells["Vendor"].Value.ToString());
                //if (row.Cells["Country"].Value != DBNull.Value) _obj1.Country = Convert.ToInt32(row.Cells["Country"].Value.ToString());
                //if (row.Cells["Pono"].Value != DBNull.Value)            _obj1.Pono = row.Cells["Pono"].Value.ToString();
                //if (row.Cells["Styleno"].Value != DBNull.Value)         _obj1.Styleno = row.Cells["Styleno"].Value.ToString();
                //if (row.Cells["SampleType"].Value != DBNull.Value) _obj1.SampleType = row.Cells["SampleType"].Value.ToString();
                //if (row.Cells["InspType"].Value != DBNull.Value) _obj1.InspType = row.Cells["InspType"].Value.ToString();

                //if (row.Cells["Season"].Value != DBNull.Value)          _obj1.Season = row.Cells["Season"].Value.ToString();
                //if (row.Cells["Description"].Value != DBNull.Value)     _obj1.Description = row.Cells["Description"].Value.ToString();
                //if (row.Cells["DeliveryDate"].Value != DBNull.Value)    _obj1.DeliveryDate = Convert.ToDateTime(row.Cells["DeliveryDate"].Value);
                //if (row.Cells["IsPrinting"].Value != DBNull.Value)      _obj1.IsPrinting = Convert.ToInt32(row.Cells["IsPrinting"].Value.ToString());
                //if(row.Cells["EmbelishId1"].Value!= DBNull.Value)       _obj1.EmbelishId1 = Convert.ToInt32(row.Cells["EmbelishId1"].Value);
                //if (row.Cells["EmbelishId2"].Value != DBNull.Value)     _obj1.EmbelishId2 = Convert.ToInt32(row.Cells["EmbelishId2"].Value);
                //if (row.Cells["SizeGroupIdx"].Value != DBNull.Value) _obj1.SizeGroupIdx = Convert.ToInt32(row.Cells["SizeGroupIdx"].Value);
                //if (row.Cells["SewThreadIdx"].Value != DBNull.Value) _obj1.SewThreadIdx = Convert.ToInt32(row.Cells["SewThreadIdx"].Value);

                //if (row.Cells["OrderQty"].Value != DBNull.Value)        _obj1.OrderQty = Convert.ToInt32(row.Cells["OrderQty"].Value.ToString());
                //if (row.Cells["OrderPrice"].Value != DBNull.Value)      _obj1.OrderPrice = Convert.ToDouble(row.Cells["OrderPrice"].Value.ToString());
                //if (row.Cells["OrderAmount"].Value != DBNull.Value)     _obj1.OrderAmount = Convert.ToDouble(row.Cells["OrderAmount"].Value.ToString());

                //if (row.Cells["Remark"].Value != DBNull.Value) _obj1.Remark = row.Cells["Remark"].Value.ToString();
                //if (row.Cells["TeamRequestedDate"].Value != DBNull.Value) _obj1.TeamRequestedDate = Convert.ToDateTime(row.Cells["TeamRequestedDate"].Value);
                //if (row.Cells["SplConfirmedDate"].Value != DBNull.Value) _obj1.SplConfirmedDate = Convert.ToDateTime(row.Cells["SplConfirmedDate"].Value);
                
                //if (row.Cells["Status"].Value != DBNull.Value)          _obj1.Status = Convert.ToInt32(row.Cells["Status"].Value.ToString());

                //// 업데이트 (오더캔슬, 선적완료 상태가 아닐경우)
                //if (_obj1.Status != 2 && _obj1.Status != 3) _bRtn=_obj1.Update();

                // 변경된 사이즈 그룹의 사이즈 정보 갱신 
                
                
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
            //_bRtn = false;

            //try
            //{
            //    RadGridView gv = gvColorSize;

            //    gv.EndEdit();
            //    GridViewRowInfo row = Int.Members.GetCurrentRow(gv);  // 현재 행번호 확인

            //    // 객체생성 및 값 할당
            //    _obj2 = new Controller.Pattern(Convert.ToInt32(row.Cells["Idx"].Value));
            //    _obj2.Idx = Convert.ToInt32(row.Cells["Idx"].Value.ToString());
            //    _obj2.OrderIdx = Convert.ToInt32(row.Cells["OrderIdx"].Value.ToString());
            //    if (row.Cells["ColorIdx"].Value != DBNull.Value) _obj2.ColorIdx = Convert.ToInt32(row.Cells["ColorIdx"].Value.ToString());
            //    if (row.Cells["SizeIdx1"].Value != DBNull.Value) _obj2.SizeIdx1 = Convert.ToInt32(row.Cells["SizeIdx1"].Value.ToString());
            //    if (row.Cells["SizeIdx2"].Value != DBNull.Value) _obj2.SizeIdx2 = Convert.ToInt32(row.Cells["SizeIdx2"].Value.ToString());
            //    if (row.Cells["SizeIdx3"].Value != DBNull.Value) _obj2.SizeIdx3 = Convert.ToInt32(row.Cells["SizeIdx3"].Value.ToString());
            //    if (row.Cells["SizeIdx4"].Value != DBNull.Value) _obj2.SizeIdx4 = Convert.ToInt32(row.Cells["SizeIdx4"].Value.ToString());
            //    if (row.Cells["SizeIdx5"].Value != DBNull.Value) _obj2.SizeIdx5 = Convert.ToInt32(row.Cells["SizeIdx5"].Value.ToString());
            //    if (row.Cells["SizeIdx6"].Value != DBNull.Value) _obj2.SizeIdx6 = Convert.ToInt32(row.Cells["SizeIdx6"].Value.ToString());
            //    if (row.Cells["SizeIdx7"].Value != DBNull.Value) _obj2.SizeIdx7 = Convert.ToInt32(row.Cells["SizeIdx7"].Value.ToString());
            //    if (row.Cells["SizeIdx8"].Value != DBNull.Value) _obj2.SizeIdx8 = Convert.ToInt32(row.Cells["SizeIdx8"].Value.ToString());

            //    if (row.Cells["Pcs1"].Value != DBNull.Value) _obj2.Pcs1 = Convert.ToInt32(row.Cells["Pcs1"].Value.ToString());
            //    if (row.Cells["Pcs2"].Value != DBNull.Value) _obj2.Pcs2 = Convert.ToInt32(row.Cells["Pcs2"].Value.ToString());
            //    if (row.Cells["Pcs3"].Value != DBNull.Value) _obj2.Pcs3 = Convert.ToInt32(row.Cells["Pcs3"].Value.ToString());
            //    if (row.Cells["Pcs4"].Value != DBNull.Value) _obj2.Pcs4 = Convert.ToInt32(row.Cells["Pcs4"].Value.ToString());
            //    if (row.Cells["Pcs5"].Value != DBNull.Value) _obj2.Pcs5 = Convert.ToInt32(row.Cells["Pcs5"].Value.ToString());
            //    if (row.Cells["Pcs6"].Value != DBNull.Value) _obj2.Pcs6 = Convert.ToInt32(row.Cells["Pcs6"].Value.ToString());
            //    if (row.Cells["Pcs7"].Value != DBNull.Value) _obj2.Pcs7 = Convert.ToInt32(row.Cells["Pcs7"].Value.ToString());
            //    if (row.Cells["Pcs8"].Value != DBNull.Value) _obj2.Pcs8 = Convert.ToInt32(row.Cells["Pcs8"].Value.ToString());

            //    // 업데이트
            //    _bRtn = _obj2.Update();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("gvColorSize_Update: " + ex.Message.ToString());
            //}

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

            if (meditor != null )
            {
                meditor.Enabled = false;
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
            //lstSize.Clear(); // 기존 저장된 사이즈 초기화 
            //GetSizes(_gv1); // 하단 Color Size 데이터용 Size 정보 업데이트 

            if (Int.Members.GetCurrentRow(_gv1, "Status") == 2
                || Int.Members.GetCurrentRow(_gv1, "Status") == 3)
            {
                Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = true;
            }
            else
            {
                Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = false;
            }

            // 컬러사이즈 제목 갱신후 자료갱신
            //GV2_CreateColumn(gvColorSize);  
            //DataBinding_GV2(gvColorSize, Int.Members.GetCurrentRow(_gv1, "Idx"));
            
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
