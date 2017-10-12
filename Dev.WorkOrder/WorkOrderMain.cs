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
using System.IO;
using Dev.Pattern;
using Dev.Production;
using Dev.Sales;
using Dev.Out;

namespace Dev.WorkOrder
{
    /// <summary>
    /// 작업 관리화면: 작업지시번호 확인 및 QR처리
    /// </summary>
    public partial class WorkOrderMain : InheritForm
    {
        
        #region 1. 변수 설정

        public InheritMDI __main__;                                             // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, int> _searchKey;                // 쿼리값 value, 쿼리항목 key로 전달 
        public Dictionary<CommonValues.KeyName, string> _searchString;                // 쿼리값 value, 쿼리항목 key로 전달 
        public DataRow InsertedOrderRow = null;

        private enum OrderStatus { Normal, Progress, Cancel, Close };           // 오더상태값
        private bool _bRtn;                                                     // 쿼리결과 리턴
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        private DataTable _dt = null;                                           // 기본 데이터테이블
        private Controller.Pattern _obj1 = null;                                // 현재 생성된 객체 
        private Controller.OrderType _obj2 = null;                           // 현재 생성된 객체 
        private RadContextMenu contextMenu;                                     // 컨텍스트 메뉴
        private List<CodeContents> lstStatus = new List<CodeContents>();        // 오더상태
        private List<CodeContents> lstWorkStatus = new List<CodeContents>();        // 작업진행상태
        private List<CodeContents> lstWorkStatus2 = new List<CodeContents>();        // 작업진행상태
        private List<DepartmentName> deptName = new List<DepartmentName>();     // 부서
        private List<CustomerName> custName = new List<CustomerName>();         // 거래처
        private List<CodeContents> codeName = new List<CodeContents>();         // 코드
        private List<CustomerName> lstUser = new List<CustomerName>();          // 유저명
        private List<CodeContents> lstOperation = new List<CodeContents>();     // 공정명
        private string _layoutfile = "/GVLayoutWorkOrder.xml";
        private string _workOrderIdx;
        private string __QRCode__ = "";
        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 20, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public WorkOrderMain(InheritMDI main, string WorkOrderIdx)
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
                _searchString = new Dictionary<CommonValues.KeyName, string>();

                _searchKey.Add(CommonValues.KeyName.CustIdx, 0);
                _searchKey.Add(CommonValues.KeyName.OperationIdx, 0);
                _searchString.Add(CommonValues.KeyName.Fileno, "");
                _searchKey.Add(CommonValues.KeyName.Status, 0);
                _searchString.Add(CommonValues.KeyName.Styleno, "");
                _searchString.Add(CommonValues.KeyName.WorkOrderIdx, "");
                _searchKey.Add(CommonValues.KeyName.WorkStatus, 0);
                _searchString.Add(CommonValues.KeyName.TicketDate, "");
                _searchString.Add(CommonValues.KeyName.StartDate, "");

                DataBinding_GV1(_searchKey, _searchString);
            }
        }
        
        /// <summary>
        /// 상단 검색을 위한 Dropdownlist 생성
        /// </summary>
        private void Config_DropDownList()
        {
            
            // 바이어
            ddlCust.DataSource = custName;
            ddlCust.DisplayMember = "CustName";
            ddlCust.ValueMember = "CustIdx";
            ddlCust.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlCust.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
            
            // 공정
            lstOperation.Clear();
            lstOperation = codeName.FindAll(
                delegate (CodeContents code)
                {
                    return code.Classification == "Operation";
                });
            lstOperation.Insert(0, new CodeContents(0, "", "")); 
            ddlOperation.DataSource = lstOperation;
            ddlOperation.DisplayMember = "Contents";
            ddlOperation.ValueMember = "CodeIdx";
            ddlOperation.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlOperation.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 오더 상태 
            ddlStatus.DataSource = lstStatus;
            ddlStatus.DisplayMember = "Contents";
            ddlStatus.ValueMember = "CodeIdx";
            ddlStatus.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlStatus.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 작업진행상태  
            ddlWorkStatus.DataSource = lstWorkStatus;
            ddlWorkStatus.DisplayMember = "Contents";
            ddlWorkStatus.ValueMember = "CodeIdx";
            ddlWorkStatus.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlWorkStatus.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
        }
        

        /// <summary>
        /// 그리드뷰 컬럼 생성 (메인)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV1_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            GridViewTextBoxColumn Buyer = new GridViewTextBoxColumn();
            Buyer.Name = "Buyer";
            Buyer.FieldName = "CustName";
            Buyer.HeaderText = "Buyer";
            Buyer.Width = 140;
            Buyer.ReadOnly = true;
            Buyer.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(Buyer);

            GridViewTextBoxColumn Operation = new GridViewTextBoxColumn();
            Operation.Name = "Operation";
            Operation.FieldName = "OperationIdx";
            Operation.HeaderText = "Operation";
            Operation.Width = 100;
            Operation.ReadOnly = true;
            Operation.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(Operation);

            GridViewHyperlinkColumn Fileno = new GridViewHyperlinkColumn();
            Fileno.Name = "Fileno";
            Fileno.FieldName = "Fileno";
            Fileno.HeaderText = "File #";
            Fileno.Width = 100;
            Fileno.ReadOnly = true;
            Fileno.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(Fileno);

            GridViewTextBoxColumn OrderType = new GridViewTextBoxColumn();
            OrderType.Name = "OrderType";
            OrderType.FieldName = "OrderType";
            OrderType.HeaderText = "Sample\nType";
            OrderType.Width = 60;
            OrderType.ReadOnly = true;
            OrderType.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            gv.Columns.Add(OrderType);

            GridViewTextBoxColumn Styleno = new GridViewTextBoxColumn();
            Styleno.Name = "Styleno";
            Styleno.FieldName = "Styleno";
            Styleno.HeaderText = "Style#";
            Styleno.Width = 100;
            Styleno.ReadOnly = true;
            Styleno.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(Styleno);
            
            GridViewHyperlinkColumn WorkOrderIdx = new GridViewHyperlinkColumn();
            WorkOrderIdx.Name = "WorkOrderIdx";
            WorkOrderIdx.FieldName = "WorkOrderIdx";
            WorkOrderIdx.Width = 100;
            WorkOrderIdx.TextAlignment = ContentAlignment.MiddleLeft;
            WorkOrderIdx.HeaderText = "Work ID";
            WorkOrderIdx.ReadOnly = true;
            gv.Columns.Add(WorkOrderIdx);

            GridViewTextBoxColumn Title = new GridViewTextBoxColumn();
            Title.Name = "Title";
            Title.FieldName = "Title";
            Title.HeaderText = "Title";
            Title.Width = 300;
            Title.ReadOnly = true;
            Title.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(Title);

            GridViewDateTimeColumn StartDate = new GridViewDateTimeColumn();
            StartDate.Name = "StartDate";
            StartDate.FieldName = "Start";
            StartDate.Width = 100;
            StartDate.TextAlignment = ContentAlignment.MiddleCenter;
            StartDate.FormatString = "{0:d}";
            StartDate.HeaderText = "Start Date";
            StartDate.ReadOnly = true;
            gv.Columns.Add(StartDate);

            GridViewComboBoxColumn WorkStatus = new GridViewComboBoxColumn();
            WorkStatus.Name = "WorkStatus";
            WorkStatus.DataSource = lstWorkStatus2;
            WorkStatus.DisplayMember = "Contents";
            WorkStatus.ValueMember = "CodeIdx";
            WorkStatus.FieldName = "wstatus";
            WorkStatus.HeaderText = "Work Status";
            WorkStatus.Width = 100;
            gv.Columns.Add(WorkStatus);

            GridViewDateTimeColumn TicketDate = new GridViewDateTimeColumn();
            TicketDate.Name = "TicketDate";
            TicketDate.FieldName = "TicketDate";
            TicketDate.Width = 100;
            TicketDate.TextAlignment = ContentAlignment.MiddleCenter;
            TicketDate.FormatString = "{0:d}";
            TicketDate.HeaderText = "Ticket Date";
            gv.Columns.Add(TicketDate);

            GridViewTextBoxColumn Qrcode = new GridViewTextBoxColumn();
            Qrcode.Name = "Qrcode";
            Qrcode.FieldName = "Qrcode";
            Qrcode.HeaderText = "QR-Code";
            Qrcode.Width = 100;
            Qrcode.ReadOnly = true; 
            //Qrcode.IsVisible = false;
            Qrcode.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(Qrcode);

            GridViewComboBoxColumn status = new GridViewComboBoxColumn();
            status.Name = "Status";
            status.DataSource = lstWorkStatus;
            status.DisplayMember = "Contents";
            status.ValueMember = "CodeIdx";
            status.FieldName = "status";
            status.IsVisible = false;
            gv.Columns.Add(status);

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

            //// 캔슬오더 색상변경
            Font f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            ConditionalFormattingObject obj = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "4", "", true);
            obj.RowForeColor = Color.Black;
            obj.RowBackColor = Color.FromArgb(255, 255, 230, 230);
            obj.RowFont = f;
            gv.Columns["WorkStatus"].ConditionalFormattingObjectList.Add(obj);
                        
            //// 마감오더 색상변경
            f = new Font(new FontFamily("Segoe UI"), 8.25f);
            ConditionalFormattingObject obj2 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "3", "", true);
            obj2.RowForeColor = Color.Black;
            obj2.RowBackColor = Color.FromArgb(255, 220, 255, 240);
            obj2.RowFont = f;
            gv.Columns["WorkStatus"].ConditionalFormattingObjectList.Add(obj2);

            f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            ConditionalFormattingObject obj4 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "2", "", true);
            obj4.RowForeColor = Color.Black;
            obj4.RowBackColor = Color.PapayaWhip; 
            obj4.RowFont = f;
            gv.Columns["WorkStatus"].ConditionalFormattingObjectList.Add(obj4);

            //// 입력실수 색상변경
            f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            ConditionalFormattingObject obj5 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "14", "", true);
            obj5.RowForeColor = Color.Black;
            obj5.RowBackColor = Color.FromArgb(255, 255, 210, 210);
            obj5.RowFont = f;
            gv.Columns["WorkStatus"].ConditionalFormattingObjectList.Add(obj5);

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

            // 작업진행상태 (CommonValues정의)
            lstWorkStatus.Add(new CodeContents(0, CommonValues.DicWorkOrderStatus[0], ""));
            lstWorkStatus.Add(new CodeContents(1, CommonValues.DicWorkOrderStatus[1], ""));
            lstWorkStatus.Add(new CodeContents(2, CommonValues.DicWorkOrderStatus[2], ""));
            lstWorkStatus.Add(new CodeContents(3, CommonValues.DicWorkOrderStatus[3], ""));
            lstWorkStatus.Add(new CodeContents(4, CommonValues.DicWorkOrderStatus[4], ""));
            lstWorkStatus.Add(new CodeContents(5, CommonValues.DicWorkOrderStatus[5], ""));
            lstWorkStatus.Add(new CodeContents(6, CommonValues.DicWorkOrderStatus[6], ""));
            lstWorkStatus.Add(new CodeContents(7, CommonValues.DicWorkOrderStatus[7], ""));
            lstWorkStatus.Add(new CodeContents(8, CommonValues.DicWorkOrderStatus[8], ""));
            lstWorkStatus.Add(new CodeContents(10, CommonValues.DicWorkOrderStatus[10], ""));
            lstWorkStatus.Add(new CodeContents(11, CommonValues.DicWorkOrderStatus[11], ""));
            lstWorkStatus.Add(new CodeContents(12, CommonValues.DicWorkOrderStatus[12], ""));
            lstWorkStatus.Add(new CodeContents(13, CommonValues.DicWorkOrderStatus[13], ""));
            lstWorkStatus.Add(new CodeContents(14, CommonValues.DicWorkOrderStatus[14], ""));

            // 작업진행상태 (CommonValues정의)
            lstWorkStatus2.Add(new CodeContents(0, CommonValues.DicWorkOrderStatus[0], ""));
            lstWorkStatus2.Add(new CodeContents(1, CommonValues.DicWorkOrderStatus[1], ""));
            lstWorkStatus2.Add(new CodeContents(2, CommonValues.DicWorkOrderStatus[2], ""));
            lstWorkStatus2.Add(new CodeContents(3, CommonValues.DicWorkOrderStatus[3], ""));
            lstWorkStatus2.Add(new CodeContents(4, CommonValues.DicWorkOrderStatus[4], ""));
            lstWorkStatus2.Add(new CodeContents(5, CommonValues.DicWorkOrderStatus[5], ""));
            lstWorkStatus2.Add(new CodeContents(6, CommonValues.DicWorkOrderStatus[6], ""));
            lstWorkStatus2.Add(new CodeContents(7, CommonValues.DicWorkOrderStatus[7], ""));
            lstWorkStatus2.Add(new CodeContents(8, CommonValues.DicWorkOrderStatus[8], ""));
            lstWorkStatus2.Add(new CodeContents(10, CommonValues.DicWorkOrderStatus[10], ""));
            lstWorkStatus2.Add(new CodeContents(11, CommonValues.DicWorkOrderStatus[11], ""));
            lstWorkStatus2.Add(new CodeContents(12, CommonValues.DicWorkOrderStatus[12], ""));
            lstWorkStatus2.Add(new CodeContents(13, CommonValues.DicWorkOrderStatus[13], ""));
            lstWorkStatus2.Add(new CodeContents(14, CommonValues.DicWorkOrderStatus[14], ""));
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
                if (ddlCust.SelectedValue != null || ddlOperation.SelectedValue != null
                    || ddlStatus.SelectedValue != null || ddlWorkStatus.SelectedValue != null
                    || !string.IsNullOrEmpty(txtFileno.Text) || !string.IsNullOrEmpty(txtStyle.Text) || !string.IsNullOrEmpty(txtWorkno.Text)
                    || dtStart.Value != null || dtTicket.Value != null
                    )
                {
                    _searchKey = new Dictionary<CommonValues.KeyName, int>();
                    _searchString = new Dictionary<CommonValues.KeyName, string>();

                    // 영업부인경우, 해당 부서만 조회할수 있도록 제한 
                    if (UserInfo.ReportNo < 9)
                        _searchKey.Add(CommonValues.KeyName.DeptIdx, UserInfo.DeptIdx);
                    else
                        _searchKey.Add(CommonValues.KeyName.DeptIdx, Convert.ToInt32(ddlWorkStatus.SelectedValue));
                    
                    _searchKey.Add(CommonValues.KeyName.CustIdx, Convert.ToInt32(ddlCust.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.OperationIdx, Convert.ToInt32(ddlOperation.SelectedValue));
                    _searchString.Add(CommonValues.KeyName.Fileno, txtFileno.Text.Trim());
                    _searchKey.Add(CommonValues.KeyName.Status, Convert.ToInt32(ddlStatus.SelectedValue));
                    _searchString.Add(CommonValues.KeyName.Styleno, txtStyle.Text.Trim());
                    _searchString.Add(CommonValues.KeyName.WorkOrderIdx, txtWorkno.Text.Trim());
                    _searchKey.Add(CommonValues.KeyName.WorkStatus, Convert.ToInt32(ddlWorkStatus.SelectedValue));
                    _searchString.Add(CommonValues.KeyName.TicketDate, CommonController.ConvertDate(dtTicket.Value).ToString().Substring(0, 10));
                    _searchString.Add(CommonValues.KeyName.StartDate, CommonController.ConvertDate(dtStart.Value).ToString().Substring(0, 10));

                    //Console.WriteLine(CommonController.ConvertDate(dtTicket.Value).ToString()); 
                    DataBinding_GV1(_searchKey, _searchString);
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
        private void DataBinding_GV1(Dictionary<CommonValues.KeyName, int> SearchKey, 
                                    Dictionary<CommonValues.KeyName, string> SearchString)
        {
            try
            {
                _gv1.DataSource = null;
                
                _ds1 = Controller.WorkOrder.Getlist(SearchKey, SearchString);

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

                //// 객체생성 및 값 할당
                //_obj1           = new Controller.Pattern(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value));
                //_obj1.Idx       = Convert.ToInt32(row.Cells["Idx"].Value.ToString());

                //if (row.Cells["OrdSizeIdx"].Value != DBNull.Value) _obj1.OrdSizeIdx = Convert.ToInt32(row.Cells["OrdSizeIdx"].Value.ToString());

                //if (row.Cells["TechpackDate"].Value != DBNull.Value && row.Cells["TechpackDate"] != null)
                //{
                //    _obj1.TechpackDate = Convert.ToDateTime(row.Cells["TechpackDate"].Value);
                //}
                //else
                //{
                //    _obj1.TechpackDate = new DateTime(2000, 1, 1);
                //}

                //if (row.Cells["RequestedDate"].Value != DBNull.Value) _obj1.RequestedDate = Convert.ToDateTime(row.Cells["RequestedDate"].Value);
                //if (row.Cells["Requested"].Value != DBNull.Value) _obj1.Requested = Convert.ToInt32(row.Cells["Requested"].Value.ToString());

                //if (row.Cells["ConfirmedDate"].Value != DBNull.Value && row.Cells["ConfirmedDate"] != null)
                //{
                //    _obj1.ConfirmedDate = Convert.ToDateTime(row.Cells["ConfirmedDate"].Value);
                //}
                //else
                //{
                //    _obj1.ConfirmedDate = new DateTime(2000, 1, 1);
                //}

                //if (row.Cells["Confirmed"].Value != DBNull.Value) _obj1.Confirmed = Convert.ToInt32(row.Cells["Confirmed"].Value.ToString());

                //if (row.Cells["CompletedDate"].Value != DBNull.Value && row.Cells["CompletedDate"] != null)
                //{
                //    _obj1.CompletedDate = Convert.ToDateTime(row.Cells["CompletedDate"].Value);
                //}
                //else
                //{
                //    _obj1.CompletedDate = new DateTime(2000, 1, 1); 
                //}

                //if (row.Cells["SentDate"].Value != DBNull.Value && row.Cells["SentDate"] != null)
                //{
                //    _obj1.SentDate = Convert.ToDateTime(row.Cells["SentDate"].Value);
                //}
                //else
                //{
                //    _obj1.SentDate = new DateTime(2000, 1, 1);
                //}


                //// 업데이트 (오더캔슬, 선적완료 상태가 아닐경우)
                _bRtn = Controller.WorkOrder.Update(row.Cells["WorkOrderIdx"].Value.ToString().Trim(),
                                                    row.Cells["TicketDate"].Value == DBNull.Value ? new DateTime(2000,1,1) : Convert.ToDateTime(row.Cells["TicketDate"].Value),
                                                    Convert.ToInt32(row.Cells["WorkStatus"].Value),
                                                    row.Cells["Qrcode"].Value.ToString().Trim(),
                                                    UserInfo.Idx); 
                __main__.lblRows.Text = "Updated Work Info"; 
                                
            }
            catch (Exception ex)
            {
                Console.WriteLine("GV1_Update: " + ex.Message.ToString());
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
                el.NullDate = new DateTime(2000, 1, 1); 
                el.CalendarSize = new Size(500, 400);

                //if (el.Value.ToString().Substring(0, 10) == "2000-01-01")
                //{
                //    el.Value = Convert.ToDateTime(null); 
                //}
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
        /// 행 선택시 버튼 상태 변경
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                // 토글 초기화 
                toggleWay.Enabled = false;
                toggleMode.Value = false;
                toggleMode.Enabled = true;
                
                lvQRCode.Items.Clear();
                
                // 선택된 오더상태에 따라 읽기수정 가능여부 설정 
                if (Int.Members.GetCurrentRow(_gv1, "WorkStatus") == 4
                    || Int.Members.GetCurrentRow(_gv1, "WorkStatus") == 3)
                {
                    Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = true;
                }
                else
                {
                    Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = false;
                }

                bool iSame = true;              // 멀티선택된 행들중 QR상태가 다른지 확인하기 위한 용도 
                string TagName = "";            // 각행의 QR상태를 Tag에 저장하여 비교
                string OpName = "";
                btnSaveData.Enabled = true;
                CommonValues.ListWorkID.Clear();
                CommonValues.WorkOperation = ""; 

                // 선택된 모든 행에 대해 
                foreach (GridViewRowInfo row in _gv1.SelectedRows)
                {
                    if (Convert.ToInt32(row.Cells["WorkStatus"].Value) == 3 || Convert.ToInt32(row.Cells["WorkStatus"].Value) == 4)
                    {
                        btnSaveData.Text = "Print QR";
                        btnSaveData.Tag = "qr";
                        btnSaveData.Enabled = false;
                        toggleWay.Enabled = true;
                        toggleMode.Value = true;
                        txtBarcode.Enabled = true;
                    }
                    // 생성된 QR코드가 없으면 발행가능 상태
                    else if (string.IsNullOrEmpty(row.Cells["Qrcode"].Value.ToString().Trim()))
                    {
                        btnSaveData.Text = "Print QR";
                        btnSaveData.Tag = "qr";
                        txtBarcode.Enabled = false;
                    }
                    // 생성된 QR코드가 있으면 발행불가상태 및 QR읽기모드 활성화, 
                    // 저장버튼은 비활성화 해두고 리스트에 읽어들인 WorkID가 있을때 활성화 한다 
                    else
                    {
                        CommonValues.ListWorkID.Add(row.Cells["WorkOrderIdx"].Value.ToString().Trim());
                        CommonValues.WorkOperation = row.Cells["Operation"].Value.ToString().Trim();
                        btnSaveData.Text = "Save QR";
                        btnSaveData.Tag = "save";
                        btnSaveData.Enabled = false;
                        toggleWay.Enabled = true;
                        toggleMode.Value = true;
                        txtBarcode.Enabled = true; 
                    }
                    // 맨처음 태그는 버튼 태그를 저장해두고 
                    if (string.IsNullOrEmpty(TagName))
                    {
                        TagName = btnSaveData.Tag.ToString();
                        OpName = row.Cells["Operation"].Value.ToString().Trim(); 
                    }
                    else if (TagName.Trim() != btnSaveData.Tag.ToString() || OpName.Trim() != row.Cells["Operation"].Value.ToString().Trim())
                    {
                        iSame = false;
                    }

                }
                // 각행간 태그가 일치하지 않는 행이 있으면 모두 비활성화 
                if (!iSame)
                {
                    __main__.lblDescription.Text = "There're different QR Status item in your selected items.";
                    btnSaveData.Text = "Different item selected!!";
                    btnSaveData.Enabled = false;
                    toggleWay.Enabled = false;
                    toggleMode.Value = false;
                    toggleMode.Enabled = false;
                    txtBarcode.Enabled = false; 
                }
            }
            catch(Exception ex)
            {
                RadMessageBox.Show(ex.Message); 
            }
                        
        }
        
        /// <summary>
        /// 업데이트 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            string WorkOrderIdx = "";
            string SQL = ""; 

            try
            {
                #region QR 생성 모드 
                                
                // 선택된 행들에 QR코드 생성 및 티켓출력
                if (btnSaveData.Tag.ToString() == "qr")
                {
                    // http://interp.azurewebsites.net/api/Values

                    try
                    {
                        if (RadMessageBox.Show("Are you sure to create QR Code?", "Confirm", MessageBoxButtons.YesNo, RadMessageIcon.Question)
                            == DialogResult.Yes)
                        {
                            foreach (GridViewRowInfo row in _gv1.SelectedRows)
                            {
                                if (!string.IsNullOrEmpty(row.Cells["Qrcode"].Value.ToString().Trim()))
                                {
                                    RadMessageBox.Show(row.Cells["WorkOrderIdx"].Value.ToString().Trim() +
                                            " already created QR Code.", "Notice", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                                    return;
                                }
                                // 작업완료 또는 취소된 건은 제외 
                                if (Convert.ToInt32(row.Cells["WorkStatus"].Value)==3 || Convert.ToInt32(row.Cells["WorkStatus"].Value) == 4)
                                {
                                    RadMessageBox.Show("This work was finished or canceled", "Error");
                                    return; 
                                }

                                __QRCode__ = row.Cells["WorkOrderIdx"].Value.ToString().Trim(); //Int.Encryptor.Encrypt(row.Cells["WorkOrderIdx"].Value.ToString().Trim(), "love1229");

                                //update QR code
                                _bRtn = Controller.WorkOrder.Update(row.Cells["WorkOrderIdx"].Value.ToString().Trim(),
                                                        row.Cells["TicketDate"].Value == DBNull.Value ? DateTime.Now : Convert.ToDateTime(row.Cells["TicketDate"].Value),
                                                        2, __QRCode__, UserInfo.Idx);
                                __main__.lblRows.Text = "Created QR Codes";
                            }
                            RefleshWithCondition();
                        }
                    }
                    catch (Exception ex)
                    {
                        RadMessageBox.Show("QR: " + ex.Message);
                    }
                }

                #endregion

                #region 리딩된 리스트박스의 QR코드저장, Concept: 출고시 작업물 현황완료 상태로 전환 (2>3), 입고시 외주작업물 현황 >> 170623 현재 구분필요없음 차후 용도 

                else if (btnSaveData.Tag.ToString() == "save")
                {

                    if (lvQRCode.Items.Count > 0)
                    {
                        foreach (ListViewDataItem item in lvQRCode.Items)
                        {
                            // 복호화
                            WorkOrderIdx = item.Value.ToString(); // Decryptor(item.Value.ToString());

                            // 값이없으면
                            if (string.IsNullOrEmpty(WorkOrderIdx.Trim()))
                            {
                                item.Value = "Invalid code - " + item.Value;
                            }
                            // 길이가 14, 12가 아니면
                            else if (WorkOrderIdx.Length != 14 && WorkOrderIdx.Length != 12 && WorkOrderIdx.Length != 11) 
                            {
                                item.Value = "Invalid code - " + item.Value;
                            }
                            // S, D, O로 시작하지 않으면 - S:Sample, D:Development, O:Outbound
                            else if (WorkOrderIdx.Substring(0, 1) != "S" && WorkOrderIdx.Substring(0, 1) != "D" && WorkOrderIdx.Substring(0, 1) != "O")
                            {
                                item.Value = "Invalid code - " + item.Value;
                            }
                            // 쿼리생성 
                            else
                            {
                                SQL += "Update WorkOrder set Status=3 where WorkOrderIdx='" + WorkOrderIdx + "'; ";
                                // Cutting
                                if (WorkOrderIdx.Substring(0, 2) == "DC")
                                {
                                    SQL += "update Cutting set CuttedDate = dbo.GetLocalDate(default), CuttedQty = OrdQty where isnull(WorkOrderIdx,'')='" + WorkOrderIdx + "'; ";
                                }
                                // Printing
                                else if (WorkOrderIdx.Substring(0, 2) == "DP")
                                {
                                    SQL += "update Printing set RcvdDate=dbo.GetLocalDate(default), RcvdQty=OrdQty where isnull(WorkOrderIdx,'')='" + WorkOrderIdx + "'; ";
                                }
                                // Embroidery
                                else if (WorkOrderIdx.Substring(0, 2) == "DE")
                                {
                                    SQL += "update Embroidery set RcvdDate=dbo.GetLocalDate(default), RcvdQty=OrdQty where isnull(WorkOrderIdx,'')='" + WorkOrderIdx + "'; ";
                                }
                                // Sewing
                                else if (WorkOrderIdx.Substring(0, 2) == "DS")
                                {
                                    SQL += "update Sewing set WorkDate=dbo.GetLocalDate(default), WorkQty=OrdQty where isnull(WorkOrderIdx,'')='" + WorkOrderIdx + "'; ";
                                }
                                // Inspection
                                else if (WorkOrderIdx.Substring(0, 2) == "DN")
                                {
                                    SQL += "update Inspecting set InspCompletedDate=dbo.GetLocalDate(default) where isnull(WorkOrderIdx,'')='" + WorkOrderIdx + "'; ";
                                }
                                
                                //if (result) RadMessageBox.Show("Update Succeed");
                            }
                        }
                        // 쿼리가 있으면 작업상태 일괄 업데이트 
                        if (SQL!="")
                        {
                            bool result = Controller.WorkOrder.Update(SQL);
                            if (result) RadMessageBox.Show("Update Succeed");
                            // __main__.lblDescription.Text = "Update Succeed";
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                __main__.lblDescription.Text = ex.Message; 
            }
        }
        
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
            catch(Exception ex)
            {
                return ""; 
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

        private void CheckFolder(string sPath)
        {
            // 폴더 유무확인 및 생성 
            DirectoryInfo di = new DirectoryInfo(sPath);
            if (di.Exists == false)
            {
                di.Create();
            }
        }


        #endregion

        /// <summary>
        /// 토글값이 리딩일때만 리딩방법 변경할수 있도록 한다 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toggleMode_ValueChanged(object sender, EventArgs e)
        {
            if (toggleMode.Value == false)
            {
                toggleWay.Enabled = false;
            }
            else
            {
                toggleWay.Enabled = true;
                btnSaveData.Text = "Save QR";
                btnSaveData.Tag = "save";
                btnSaveData.Enabled = false;
                txtBarcode.Enabled = true; 
            }
            txtBarcode.Select(); 
            
        }

        /// <summary>
        /// 값이 들어왔을때 처리, 저장버튼 활성화 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvQRCode_ItemValueChanged(object sender, ListViewItemValueChangedEventArgs e)
        {
            if (btnSaveData.Tag.ToString() == "save")
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
        }

        private void gvOrderActual_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.Cancel = true; 
        }

        /// <summary>
        /// 토글값이 변경되면 바코드 리딩 상태로 전환
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toggleWay_ValueChanged(object sender, EventArgs e)
        {
            txtBarcode.Select();
        }

        /// <summary>
        /// 바코드 리딩시 Listview에 항목 추가
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter && !string.IsNullOrEmpty(txtBarcode.Text.Trim()))
                {
                    lvQRCode.Items.Add(txtBarcode.Text.Trim());
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
                Console.WriteLine(ex.Message); 
            }
        }

        /// <summary>
        /// 유효하지 않은 코드 색상 다르게 표시 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvQRCode_VisualItemFormatting(object sender, ListViewVisualItemEventArgs e)
        {
            try
            {
                if (e.VisualItem.Text.Substring(0, 5) == "Inval")
                {
                    e.VisualItem.NumberOfColors = 1;
                    e.VisualItem.BackColor = Color.Red;
                    e.VisualItem.ForeColor = Color.White;
                }
                else
                {
                    e.VisualItem.ResetValue(LightVisualElement.NumberOfColorsProperty, Telerik.WinControls.ValueResetFlags.Local);
                    e.VisualItem.ResetValue(LightVisualElement.BackColorProperty, Telerik.WinControls.ValueResetFlags.Local);
                    e.VisualItem.ResetValue(LightVisualElement.ForeColorProperty, Telerik.WinControls.ValueResetFlags.Local);

                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
        }

        private void lvQRCode_Click(object sender, EventArgs e)
        {
            txtBarcode.Select();
        }

        private void gvOrderActual_HyperlinkOpened(object sender, HyperlinkOpenedEventArgs e)
        {
            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                int _mode_ = 0;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    // File번호일 경우, 
                    if (e.Cell.Value.ToString().Trim().Length < 11 && e.Cell.Value.ToString().Trim().Substring(0, 1) == "S")
                    {
                        CommonController.Close_All_Children(this, "OrderMain");
                        Sales.OrderMain form = new Sales.OrderMain(__main__, e.Cell.Value.ToString());
                        form.Text = "Order Main"; // DateTime.Now.ToLongTimeString();
                        form.MdiParent = this.MdiParent;
                        form.Show();
                    }
                    // Worksheet일 경우, 
                    else if (e.Cell.Value.ToString().Trim().Length > 12)
                    {
                        if (e.Cell.Value.ToString().Trim().Substring(11, 1) == "P")
                        {
                            CommonController.Close_All_Children(this, "PatternMain");
                            Pattern.OrderMain form = new Pattern.OrderMain(__main__, e.Cell.Value.ToString());
                            form.Text = "Pattern Main"; // DateTime.Now.ToLongTimeString();
                            form.MdiParent = this.MdiParent;
                            form.Show();
                        }
                        // shipment
                        else if (e.Cell.Value.ToString().Trim().Substring(0, 3) == "ODS")
                        {
                            CommonController.Close_All_Children(this, "FinishedMain");
                            FinishedMain form = new FinishedMain(__main__, e.Cell.Value.ToString().Substring(0, 12));
                            form.Text = "Finished Main";
                            form.MdiParent = this.MdiParent;
                            form.Show();
                        }
                    }
                    // Cutting 
                    else if (e.Cell.Value.ToString().Trim().Substring(1, 1) == "C")
                    {
                        CommonController.Close_All_Children(this, "CuttingMain");
                        CuttingMain form = new CuttingMain(__main__, e.Cell.Value.ToString());
                        form.Text = "Cutting Main";
                        form.MdiParent = this.MdiParent;
                        form.Show();
                    }
                    // Printing 
                    else if (e.Cell.Value.ToString().Trim().Substring(0, 2) == "DP")
                    {
                        CommonController.Close_All_Children(this, "PrintingMain");
                        PrintingMain form = new PrintingMain(__main__, e.Cell.Value.ToString());
                        form.Text = "Printing Main";
                        form.MdiParent = this.MdiParent;
                        form.Show();
                    }
                    // Embroidery 
                    else if (e.Cell.Value.ToString().Trim().Substring(0, 2) == "DE")
                    {
                        CommonController.Close_All_Children(this, "EmbroideryMain");
                        EmbroideryMain form = new EmbroideryMain(__main__, e.Cell.Value.ToString());
                        form.Text = "Embroidery Main";
                        form.MdiParent = this.MdiParent;
                        form.Show();
                    }
                    // Sewing 
                    else if (e.Cell.Value.ToString().Trim().Substring(0, 2) == "DS")
                    {
                        CommonController.Close_All_Children(this, "SewingMain");
                        SewingMain form = new SewingMain(__main__, e.Cell.Value.ToString());
                        form.Text = "Sewing Main";
                        form.MdiParent = this.MdiParent;
                        form.Show();
                    }
                    // Inspection 
                    else if (e.Cell.Value.ToString().Trim().Substring(0, 2) == "DN")
                    {
                        CommonController.Close_All_Children(this, "InspectionMain");
                        InspectionMain form = new InspectionMain(__main__, e.Cell.Value.ToString());
                        form.Text = "Inspection Main";
                        form.MdiParent = this.MdiParent;
                        form.Show();
                    }
                    
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
        }
    }
}
