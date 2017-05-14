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
using System.Diagnostics;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

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
        
        private bool _bRtn;                                                     // 쿼리결과 리턴
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        private DataTable _dt = null;                                           // 기본 데이터테이블
        private Controller.Pattern _obj1 = null;                                // 현재 생성된 객체 
        private Controller.OrderType _obj2 = null;                           // 현재 생성된 객체 
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
            LoadGVLayout();             // 그리드뷰 레이아웃 복구 

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
            gv.Columns["Idx"].ReadOnly = true; 

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
            cboBuyer.ReadOnly = true;
            gv.Columns.Insert(2, cboBuyer);
            
            gv.Columns["Styleno"].Width = 130;
            gv.Columns["Styleno"].TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns["Styleno"].HeaderText = "Style#";
            gv.Columns["Styleno"].ReadOnly = true;

            gv.Columns["Fileno"].Width = 90;
            gv.Columns["Fileno"].HeaderText = "INT File #";
            gv.Columns["Fileno"].ReadOnly = true;

            GridViewTextBoxColumn WorkOrderIdx = new GridViewTextBoxColumn();
            WorkOrderIdx.Name = "WorkOrderIdx";
            WorkOrderIdx.FieldName = "WorkOrderIdx";
            WorkOrderIdx.HeaderText = "Work ID";
            WorkOrderIdx.Width = 100;
            WorkOrderIdx.ReadOnly = true;
            WorkOrderIdx.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Insert(5, WorkOrderIdx);

            GridViewComboBoxColumn cboSize = new GridViewComboBoxColumn();
            cboSize.Name = "OrdSizeIdx";
            cboSize.DataSource = sizeName;
            cboSize.ValueMember = "CodeIdx";
            cboSize.DisplayMember = "Contents";
            cboSize.FieldName = "OrdSizeIdx";
            cboSize.HeaderText = "Size";
            cboSize.ReadOnly = true;
            cboSize.Width = 70;
            gv.Columns.Insert(6, cboSize);

            GridViewDateTimeColumn TechpackDate = new GridViewDateTimeColumn();
            TechpackDate.Name = "TechpackDate";
            TechpackDate.FieldName = "TechpackDate";
            TechpackDate.Width = 100;
            TechpackDate.TextAlignment = ContentAlignment.MiddleCenter;
            TechpackDate.FormatString = "{0:d}";
            TechpackDate.HeaderText = "Techpack Date";
            TechpackDate.ReadOnly = true;
            gv.Columns.Insert(7, TechpackDate);

            GridViewDateTimeColumn RequestedDate = new GridViewDateTimeColumn();
            RequestedDate.Name = "RequestedDate";
            RequestedDate.FieldName = "RequestedDate";
            RequestedDate.Width = 100;
            RequestedDate.TextAlignment = ContentAlignment.MiddleCenter;
            RequestedDate.FormatString = "{0:d}";
            RequestedDate.HeaderText = "RequestedDate";
            RequestedDate.ReadOnly = true;
            gv.Columns.Insert(8, RequestedDate);
            
            GridViewComboBoxColumn Requested = new GridViewComboBoxColumn();
            Requested.Name = "Requested";
            Requested.DataSource = lstUser;
            Requested.ValueMember = "CustIdx";
            Requested.DisplayMember = "CustName";
            Requested.FieldName = "Requested";
            Requested.HeaderText = "Requested";
            Requested.ReadOnly = true;
            Requested.Width = 100;
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
            Confirmed.Width = 100;
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
            Received.Width = 100;
            gv.Columns.Insert(14, Received);

            GridViewTextBoxColumn Remarks = new GridViewTextBoxColumn();
            Remarks.Name = "Remarks";
            Remarks.FieldName = "Remarks";
            Remarks.HeaderText = "Remarks";
            Remarks.Width = 200;
            Remarks.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Insert(15, Remarks);

            GridViewTextBoxColumn Attached1 = new GridViewTextBoxColumn();
            Attached1.Name = "Attached1";
            Attached1.FieldName = "Attached1";
            Attached1.IsVisible = false;
            gv.Columns.Insert(16, Attached1);

            GridViewTextBoxColumn Attached2 = new GridViewTextBoxColumn();
            Attached2.Name = "Attached2";
            Attached2.FieldName = "Attached2";
            Attached2.IsVisible = false;
            gv.Columns.Insert(17, Attached2);

            GridViewTextBoxColumn Attached3 = new GridViewTextBoxColumn();
            Attached3.Name = "Attached3";
            Attached3.FieldName = "Attached3";
            Attached3.IsVisible = false;
            gv.Columns.Insert(18, Attached3);

            GridViewTextBoxColumn Attached4 = new GridViewTextBoxColumn();
            Attached4.Name = "Attached4";
            Attached4.FieldName = "Attached4";
            Attached4.IsVisible = false;
            gv.Columns.Insert(19, Attached4);

            GridViewTextBoxColumn Attached5 = new GridViewTextBoxColumn();
            Attached5.Name = "Attached5";
            Attached5.FieldName = "Attached5";
            Attached5.IsVisible = false;
            gv.Columns.Insert(20, Attached5);
            
            GridViewComboBoxColumn status = new GridViewComboBoxColumn();
            status.Name = "Status";
            status.DataSource = lstStatus;
            status.DisplayMember = "Contents";
            status.ValueMember = "CodeIdx";
            status.FieldName = "Status";
            status.HeaderText = "Status";
            status.Width = 70;
            status.IsVisible = false; 
            status.ReadOnly = true;
            gv.Columns.Insert(21, status);

            GridViewTextBoxColumn Comments = new GridViewTextBoxColumn();
            Comments.Name = "Comments";
            Comments.FieldName = "Comments";
            Comments.IsVisible = false;
            gv.Columns.Insert(22, Comments);

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
            lstUser.Add(new CustomerName(0, "", 0));
            _dt = CommonController.Getlist(CommonValues.KeyName.User).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                lstUser.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]),
                                            row["UserName"].ToString(),
                                            Convert.ToInt32(row["DeptIdx"])));
            }

            // 오더상태 (CommonValues정의)
            lstStatus.Add(new CodeContents(0, CommonValues.DicOrderStatus[0], ""));
            lstStatus.Add(new CodeContents(1, CommonValues.DicOrderStatus[1], ""));
            lstStatus.Add(new CodeContents(2, CommonValues.DicOrderStatus[2], ""));
            lstStatus.Add(new CodeContents(3, CommonValues.DicOrderStatus[3], ""));

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
        /// 데이터 로딩 (샘플타입) 
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="OrderIdx"></param>
        private void DataBinding_GV2(int OrderIdx, int OrdSizeIdx)
        {
            try
            {
                // 조회하기전에 초기화한후, 
                OrderType_Clear();
                
                _ds1 = Controller.OrderType.Getlist(OrderIdx, OrdSizeIdx);
                if (_ds1 != null)
                {
                    DataRow row = _ds1.Tables[0].Rows[0];
                    btnSaveData.Tag = row["Idx"].ToString(); 
                    chkType101.Checked = Convert.ToBoolean(row["Type101"]); 
                    chkType102.Checked = Convert.ToBoolean(row["Type102"]); 
                    chkType103.Checked = Convert.ToBoolean(row["Type103"]); 

                    chkType201.Checked = Convert.ToBoolean(row["Type201"]); 
                    chkType202.Checked = Convert.ToBoolean(row["Type202"]); 
                    chkType203.Checked = Convert.ToBoolean(row["Type203"]); 
                    chkType204.Checked = Convert.ToBoolean(row["Type204"]); 
                    chkType205.Checked = Convert.ToBoolean(row["Type205"]); 
                    chkType206.Checked = Convert.ToBoolean(row["Type206"]); 
                    chkType207.Checked = Convert.ToBoolean(row["Type207"]); 
                    chkType208.Checked = Convert.ToBoolean(row["Type208"]); 
                    chkType209.Checked = Convert.ToBoolean(row["Type209"]); 
                    chkType210.Checked = Convert.ToBoolean(row["Type210"]); 

                    chkType211.Checked = Convert.ToBoolean(row["Type211"]); 
                    chkType212.Checked = Convert.ToBoolean(row["Type212"]); 
                    chkType213.Checked = Convert.ToBoolean(row["Type213"]); 
                    chkType214.Checked = Convert.ToBoolean(row["Type214"]); 
                    chkType215.Checked = Convert.ToBoolean(row["Type215"]); 
                    chkType216.Checked = Convert.ToBoolean(row["Type216"]); 
                    chkType217.Checked = Convert.ToBoolean(row["Type217"]); 
                    chkType218.Checked = Convert.ToBoolean(row["Type218"]); 
                    chkType219.Checked = Convert.ToBoolean(row["Type219"]); 
                    chkType220.Checked = Convert.ToBoolean(row["Type220"]);
                    
                    __main__.lblRows.Text = "1 Rows of Sample Type";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBinding " + ": " + ex.Message.ToString());
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

                if (row.Cells["OrdSizeIdx"].Value != DBNull.Value) _obj1.OrdSizeIdx = Convert.ToInt32(row.Cells["OrdSizeIdx"].Value.ToString());
                
                if (row.Cells["TechpackDate"].Value != DBNull.Value && row.Cells["TechpackDate"] != null)
                {
                    _obj1.TechpackDate = Convert.ToDateTime(row.Cells["TechpackDate"].Value);
                }
                else
                {
                    _obj1.TechpackDate = new DateTime(2000, 1, 1);
                }

                if (row.Cells["RequestedDate"].Value != DBNull.Value) _obj1.RequestedDate = Convert.ToDateTime(row.Cells["RequestedDate"].Value);
                if (row.Cells["Requested"].Value != DBNull.Value) _obj1.Requested = Convert.ToInt32(row.Cells["Requested"].Value.ToString());

                if (row.Cells["ConfirmedDate"].Value != DBNull.Value && row.Cells["ConfirmedDate"] != null)
                {
                    _obj1.ConfirmedDate = Convert.ToDateTime(row.Cells["ConfirmedDate"].Value);
                }
                else
                {
                    _obj1.ConfirmedDate = new DateTime(2000, 1, 1);
                }

                if (row.Cells["Confirmed"].Value != DBNull.Value) _obj1.Confirmed = Convert.ToInt32(row.Cells["Confirmed"].Value.ToString());
                         
                if (row.Cells["CompletedDate"].Value != DBNull.Value && row.Cells["CompletedDate"] != null)
                {
                    _obj1.CompletedDate = Convert.ToDateTime(row.Cells["CompletedDate"].Value);
                }
                else
                {
                    _obj1.CompletedDate = new DateTime(2000, 1, 1); 
                }

                if (row.Cells["SentDate"].Value != DBNull.Value && row.Cells["SentDate"] != null)
                {
                    _obj1.SentDate = Convert.ToDateTime(row.Cells["SentDate"].Value);
                }
                else
                {
                    _obj1.SentDate = new DateTime(2000, 1, 1);
                }
                
                if (row.Cells["Received"].Value != DBNull.Value) _obj1.Received = Convert.ToInt32(row.Cells["Received"].Value.ToString());
                if (row.Cells["Remarks"].Value != DBNull.Value) _obj1.Remarks = row.Cells["Remarks"].Value.ToString();

                //if (row.Cells["Attached1"].Value != DBNull.Value) _obj1.Attached1 = row.Cells["Attached1"].Value.ToString();
                //if (row.Cells["Attached2"].Value != DBNull.Value) _obj1.Attached2 = row.Cells["Attached2"].Value.ToString();
                //if (row.Cells["Attached3"].Value != DBNull.Value) _obj1.Attached3 = row.Cells["Attached3"].Value.ToString();
                //if (row.Cells["Attached4"].Value != DBNull.Value) _obj1.Attached4 = row.Cells["Attached4"].Value.ToString();
                //if (row.Cells["Attached5"].Value != DBNull.Value) _obj1.Attached5 = row.Cells["Attached5"].Value.ToString();

                //if (row.Cells["AttachedUrl1"].Value != DBNull.Value) _obj1.AttachedUrl1 = row.Cells["AttachedUrl1"].Value.ToString();
                //if (row.Cells["AttachedUrl2"].Value != DBNull.Value) _obj1.AttachedUrl2 = row.Cells["AttachedUrl2"].Value.ToString();
                //if (row.Cells["AttachedUrl3"].Value != DBNull.Value) _obj1.AttachedUrl3 = row.Cells["AttachedUrl3"].Value.ToString();
                //if (row.Cells["AttachedUrl4"].Value != DBNull.Value) _obj1.AttachedUrl4 = row.Cells["AttachedUrl4"].Value.ToString();
                //if (row.Cells["AttachedUrl5"].Value != DBNull.Value) _obj1.AttachedUrl5 = row.Cells["AttachedUrl5"].Value.ToString();

                // 업데이트 (오더캔슬, 선적완료 상태가 아닐경우)
                _bRtn = _obj1.Update();
                __main__.lblRows.Text = "Updated Pattern Info"; 
                                
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
        private void OrderType_Update()
        {
            _bRtn = false;

            try
            {
                // 객체생성 및 값 할당
                _obj2 = new Controller.OrderType(Convert.ToInt32(btnSaveData.Tag));
                _obj2.Idx = Convert.ToInt32(btnSaveData.Tag);
                
                _obj2.Type101 = Convert.ToInt32(chkType101.Checked);
                _obj2.Type102 = Convert.ToInt32(chkType102.Checked);
                _obj2.Type103 = Convert.ToInt32(chkType103.Checked);

                _obj2.Type201 = Convert.ToInt32(chkType201.Checked);
                _obj2.Type202 = Convert.ToInt32(chkType202.Checked);
                _obj2.Type203 = Convert.ToInt32(chkType203.Checked);
                _obj2.Type204 = Convert.ToInt32(chkType204.Checked);
                _obj2.Type205 = Convert.ToInt32(chkType205.Checked);
                _obj2.Type206 = Convert.ToInt32(chkType206.Checked);
                _obj2.Type207 = Convert.ToInt32(chkType207.Checked);
                _obj2.Type208 = Convert.ToInt32(chkType208.Checked);
                _obj2.Type209 = Convert.ToInt32(chkType209.Checked);
                _obj2.Type210 = Convert.ToInt32(chkType210.Checked);
                
                _obj2.Type211 = Convert.ToInt32(chkType211.Checked);
                _obj2.Type212 = Convert.ToInt32(chkType212.Checked);
                _obj2.Type213 = Convert.ToInt32(chkType213.Checked);
                _obj2.Type214 = Convert.ToInt32(chkType214.Checked);
                _obj2.Type215 = Convert.ToInt32(chkType215.Checked);
                _obj2.Type216 = Convert.ToInt32(chkType216.Checked);
                _obj2.Type217 = Convert.ToInt32(chkType217.Checked);
                _obj2.Type218 = Convert.ToInt32(chkType218.Checked);
                _obj2.Type219 = Convert.ToInt32(chkType219.Checked);
                _obj2.Type220 = Convert.ToInt32(chkType220.Checked);

                // 업데이트
                _bRtn = _obj2.Update();

                if (_bRtn)
                {
                    __main__.lblRows.Text = "Updated Sample Type";
                    DataBinding_GV2(Int.Members.GetCurrentRow(_gv1, "OrderIdx"), Int.Members.GetCurrentRow(_gv1, "OrdSizeIdx"));

                    GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);

                    if (row.Cells["Attached1"].Value != DBNull.Value) linkLabel1.Text = row.Cells["Attached1"].Value.ToString();
                    if (row.Cells["Attached2"].Value != DBNull.Value) linkLabel2.Text = row.Cells["Attached2"].Value.ToString();
                    if (row.Cells["Attached3"].Value != DBNull.Value) linkLabel3.Text = row.Cells["Attached3"].Value.ToString();
                    if (row.Cells["Attached4"].Value != DBNull.Value) linkLabel4.Text = row.Cells["Attached4"].Value.ToString();
                    if (row.Cells["Attached5"].Value != DBNull.Value) linkLabel5.Text = row.Cells["Attached5"].Value.ToString();
                    if (row.Cells["Comments"].Value != DBNull.Value) txtComments.Text = row.Cells["Comments"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("gvColorSize_Update: " + ex.Message.ToString());
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
                //el.NullDate = new DateTime(2000, 1, 1);
                //el.NullText = "";
                el.CalendarSize = new Size(500, 400);

                if (el.Value.ToString().Substring(0, 10) == "2000-01-01")
                {
                    el.Value = Convert.ToDateTime(null); 
                }
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
            if (Int.Members.GetCurrentRow(_gv1, "Status") == 2
                || Int.Members.GetCurrentRow(_gv1, "Status") == 3)
            {
                Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = true;
            }
            else
            {
                Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = false;
            }

            // 행변경시 우측 샘플타입도 변경해준다 
            DataBinding_GV2(Int.Members.GetCurrentRow(_gv1, "OrderIdx"), Int.Members.GetCurrentRow(_gv1, "OrdSizeIdx"));

            GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);
            
            if (row.Cells["Attached1"].Value != DBNull.Value) linkLabel1.Text = row.Cells["Attached1"].Value.ToString();
            if (row.Cells["Attached2"].Value != DBNull.Value) linkLabel2.Text = row.Cells["Attached2"].Value.ToString();
            if (row.Cells["Attached3"].Value != DBNull.Value) linkLabel3.Text = row.Cells["Attached3"].Value.ToString();
            if (row.Cells["Attached4"].Value != DBNull.Value) linkLabel4.Text = row.Cells["Attached4"].Value.ToString();
            if (row.Cells["Attached5"].Value != DBNull.Value) linkLabel5.Text = row.Cells["Attached5"].Value.ToString();
            if (row.Cells["Comments"].Value != DBNull.Value) txtComments.Text = row.Cells["Comments"].Value.ToString();
            //linkLabel1.Tag = "";
            //linkLabel2.Tag = "";
            //linkLabel3.Tag = "";
            //linkLabel4.Tag = "";
            //linkLabel5.Tag = "";
        }

        /// <summary>
        /// 샘플타입 조회하기 전에 모든 체크박스 초기화 
        /// </summary>
        private void OrderType_Clear()
        {
            chkType101.Checked = false;
            chkType102.Checked = false;
            chkType103.Checked = false;

            chkType201.Checked = false;
            chkType202.Checked = false;
            chkType203.Checked = false;
            chkType204.Checked = false;
            chkType205.Checked = false;
            chkType206.Checked = false;
            chkType207.Checked = false;
            chkType208.Checked = false;
            chkType209.Checked = false;
            chkType210.Checked = false;

            chkType211.Checked = false;
            chkType212.Checked = false;
            chkType213.Checked = false;
            chkType214.Checked = false;
            chkType215.Checked = false;
            chkType216.Checked = false;
            chkType217.Checked = false;
            chkType218.Checked = false;
            chkType219.Checked = false;
            chkType220.Checked = false;

            linkLabel1.Text = "";
            linkLabel2.Text = "";
            linkLabel3.Text = "";
            linkLabel4.Text = "";
            linkLabel5.Text = "";

            linkLabel1.Tag = "";
            linkLabel2.Tag = "";
            linkLabel3.Tag = "";
            linkLabel4.Tag = "";
            linkLabel5.Tag = "";

            txtComments.Text = ""; 
        }

        /// <summary>
        /// 업데이트 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(btnSaveData.Tag) > 0) OrderType_Update();
        }

        /// <summary>
        /// 샘플타입 체크(해제)시 스타일 변경 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Type_ToggleStateChanged(object sender, StateChangedEventArgs args)
        {
            Font fr = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            Font fb = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Bold);

            if (args.ToggleState == Telerik.WinControls.Enumerations.ToggleState.On)
            {
                ((RadCheckBox)sender).ButtonElement.TextElement.Font = fb;
                ((RadCheckBox)sender).ButtonElement.TextElement.ForeColor = Color.Blue;
                ((RadCheckBox)sender).ButtonElement.CheckMarkPrimitive.CheckElement.ForeColor = Color.Blue;
            }
            else
            {
                ((RadCheckBox)sender).ButtonElement.TextElement.Font = fr;
                ((RadCheckBox)sender).ButtonElement.TextElement.ForeColor = Color.Black;
                ((RadCheckBox)sender).ButtonElement.CheckMarkPrimitive.CheckElement.ForeColor = Color.Black;
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

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CheckFolder(@"C:\INT\Data\updateintsample");
            Download_File("updateintsample", ((System.Windows.Forms.LinkLabel)sender).Text);
            Process process = new Process();
            process.StartInfo.FileName = @"C:\INT\Data\updateintsample\" + ((System.Windows.Forms.LinkLabel)sender).Text.Trim();
            process.Start();
        }
        
        private void Download_File(string containerurl, string filename)
        {
            // Retrieve storage account from connection string.
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client.
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve reference to a previously created container.
            CloudBlobContainer container = blobClient.GetContainerReference(containerurl);

            // Retrieve reference to a blob named "photo1.jpg".
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

            // Save blob contents to a file.
            using (var fileStream = System.IO.File.OpenWrite(@"C:\INT\Data\" + containerurl + @"\" + filename))
            {
                blockBlob.DownloadToStream(fileStream);
            }
        }

        private void CheckFolder(string sPath)
        {
            // 폴더 유무확인 및 생성 
            DirectoryInfo di = new DirectoryInfo(sPath);
            if (di.Exists == false)
            {
                di.Create();
            }
        }
    }
}
