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
using System.Diagnostics;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Telerik.WinForms.Documents.Model;

namespace Dev.Pattern
{
    /// <summary>
    /// 작지리스트 보기화면 - 수정불가 > 추가등록
    /// </summary>
    public partial class WorksheetMain : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                             // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, int> _searchKey;                // 쿼리값 value, 쿼리항목 key로 전달 
        public DataRow InsertedOrderRow = null;
        
        private bool _bRtn;                                                     // 쿼리결과 리턴
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        private DataTable _dt = null;                                           // 기본 데이터테이블
        private Controller.Worksheet _obj1 = null;                                // 현재 생성된 객체 
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
        private string _layoutfile = "/GVLayoutWorksheet.xml";
        private string _workOrderIdx;
        private GridViewRowInfo _currentRow = null; 
        private List<string> lstFiles = new List<string>();
        private List<string> lstFileUrls = new List<string>();

        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 19, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        //RadMenuItem mnuNew, mnuDel, mnuHide, mnuShow, menuItem2, menuItem3, menuItem4, mnuWorksheet, mnuCutting, mnuOutsourcing, mnuSewing, mnuInspecting = null;

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public WorksheetMain(InheritMDI main, string WorkOrderIdx)
        {
            base.InitializeComponent(); // parent 컴포넌트에 접근하기 위해 컨트롤을 public으로 지정 > 향후 수정 필요 (todo) 
            InitializeComponent();
            __main__ = main;            // MDI 연결 
            _gv1 = this.gvWorksheet;  // 그리드뷰 일반화를 위해 변수 별도 저장
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
            //LoadGVLayout();             // 그리드뷰 레이아웃 복구 

            //TD확인 
            if ((Options.UserInfo.DeptIdx == 12 || Options.UserInfo.DeptIdx == 11) && 
                (Options.UserInfo.Idx!=98 && Options.UserInfo.Idx != 99)) // 3D인원 제외 
            {
                toggTD.Value = true;
            }
            else
            {
                toggTD.Value = false;
            }

            WorksheetOpCheck1.Checked = CommonValues.WorksheetOpCheck1;
            WorksheetOpCheck2.Checked = CommonValues.WorksheetOpCheck2;
            WorksheetOpCheck3.Checked = CommonValues.WorksheetOpCheck3;
            WorksheetOpCheck4.Checked = CommonValues.WorksheetOpCheck4;
            WorksheetOpCheck5.Checked = CommonValues.WorksheetOpCheck5;

            // 다른 폼으로부터 전달된 Work ID가 있을 경우, 해당 ID로 조회 
            if (!string.IsNullOrEmpty(_workOrderIdx))
            {
                DataBinding_GV1(0, 0, 0, 0, "", "", _workOrderIdx, 1,1,1,1,1);
            }

            if (Options.UserInfo.Idx == 18)             // 개발실 원단담당자일 경우, 저장버튼 활성화 
                btnSaveFabricComment.Enabled = true; 
            else
                btnSaveFabricComment.Enabled = false;

            // Period Time이 설정되어 있는 경우, 타이머 동작시켜 주기적으로 데이터 갱신한다 
            if (CommonValues.PeriodTime > 0)
            {
                DataTable dt = Options.Data.CommonData.GetCodeValue(CommonValues.PeriodTime).Tables[0];
                if (dt.Rows.Count>0)
                {
                    Timer searchTimer = new System.Windows.Forms.Timer();
                    searchTimer.Interval = Convert.ToInt32(dt.Rows[0]["SubCode1"]);
                    this.Text = this.Text + "-----Auto refreshing " + dt.Rows[0]["Contents"].ToString(); 
                    searchTimer.Tick += new EventHandler(searchTimer_Tick);
                    searchTimer.Start(); 
                }
                RefleshWithCondition(); 
            }
        }

        void searchTimer_Tick(object sender, EventArgs e)
        {
            // UI 쓰레드에서 실행. 
            // UI 컨트롤 직접 엑세스 가능
            RefleshWithCondition();
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

            // 담당자
            ddlHandler.DataSource = lstUser;
            ddlHandler.DisplayMember = "CustName";
            ddlHandler.ValueMember = "CustIdx";
            ddlHandler.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlHandler.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 오더 상태 
            ddlStatus.DataSource = lstStatus;
            ddlStatus.DisplayMember = "Contents";
            ddlStatus.ValueMember = "CodeIdx";
            ddlStatus.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlStatus.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 정해진 바이어가 있는 경우, 해당 바이어로 dropdown 미리 선택
            if (CommonValues.NewOrderBuyerIdx > 0)
                ddlCust.SelectedValue = Convert.ToInt32(CommonValues.NewOrderBuyerIdx);
        }
        

        /// <summary>
        /// 그리드뷰 컬럼 생성 (메인)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV1_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            GridViewTextBoxColumn Idx = new GridViewTextBoxColumn();
            Idx.Name = "Idx";
            Idx.FieldName = "Idx";
            Idx.Width = 50;
            Idx.TextAlignment = ContentAlignment.MiddleCenter;
            Idx.HeaderText = "ID";
            Idx.ReadOnly = true;
            gv.Columns.Add(Idx);

            GridViewTextBoxColumn OrderIdx = new GridViewTextBoxColumn();
            OrderIdx.Name = "OrderIdx";
            OrderIdx.FieldName = "OrderIdx";
            OrderIdx.IsVisible = false;
            gv.Columns.Add(OrderIdx);
            
            GridViewTextBoxColumn DeptIdx = new GridViewTextBoxColumn();
            DeptIdx.Name = "DeptIdx";
            DeptIdx.FieldName = "DeptIdx";
            DeptIdx.ReadOnly = true;
            DeptIdx.Width = 50;
            DeptIdx.TextAlignment = ContentAlignment.MiddleLeft;
            DeptIdx.HeaderText = "Dept.";
            gv.Columns.Add(DeptIdx);

            GridViewTextBoxColumn Buyer = new GridViewTextBoxColumn();
            Buyer.Name = "Buyer";
            Buyer.FieldName = "Buyer";
            Buyer.ReadOnly = true;
            Buyer.Width = 130;
            Buyer.TextAlignment = ContentAlignment.MiddleLeft;
            Buyer.HeaderText = "Buyer";
            gv.Columns.Add(Buyer);

            GridViewHyperlinkColumn Fileno = new GridViewHyperlinkColumn();
            Fileno.Name = "Fileno";
            Fileno.FieldName = "Fileno";
            Fileno.ReadOnly = true;
            Fileno.Width = 80;
            Fileno.HeaderText = "File#";
            Fileno.ReadOnly = true;
            gv.Columns.Add(Fileno);
            
            GridViewTextBoxColumn Styleno = new GridViewTextBoxColumn();
            Styleno.Name = "Styleno";
            Styleno.FieldName = "Styleno";
            Styleno.ReadOnly = true;
            Styleno.Width = 120;
            Styleno.TextAlignment = ContentAlignment.MiddleLeft;
            Styleno.HeaderText = "Style#";
            gv.Columns.Add(Styleno);

            GridViewDateTimeColumn DeliveryDate = new GridViewDateTimeColumn();
            DeliveryDate.Name = "DeliveryDate";
            DeliveryDate.FieldName = "DeliveryDate";
            DeliveryDate.Width = 80;
            DeliveryDate.TextAlignment = ContentAlignment.MiddleCenter;
            DeliveryDate.FormatInfo = new System.Globalization.CultureInfo("en-US");
            DeliveryDate.FormatString = "{0:d}";
            DeliveryDate.HeaderText = "Delivery";
            DeliveryDate.ReadOnly = true;
            gv.Columns.Add(DeliveryDate);

            GridViewDateTimeColumn RegDate = new GridViewDateTimeColumn();
            RegDate.Name = "RegDate";
            RegDate.FieldName = "RegDate";
            RegDate.Width = 120;
            RegDate.TextAlignment = ContentAlignment.MiddleCenter;
            RegDate.FormatInfo = new System.Globalization.CultureInfo("en-US");
            RegDate.FormatString = "{0:g}";
            RegDate.HeaderText = "Sent Date";
            RegDate.ReadOnly = true;
            gv.Columns.Add(RegDate);

            GridViewTextBoxColumn Handler = new GridViewTextBoxColumn();
            Handler.Name = "Handler";
            Handler.FieldName = "Handler";
            Handler.ReadOnly = true;
            Handler.Width = 90;
            Handler.TextAlignment = ContentAlignment.MiddleLeft;
            Handler.HeaderText = "Order\nHandler";
            gv.Columns.Add(Handler);

            GridViewTextBoxColumn HandlerIdx = new GridViewTextBoxColumn();
            HandlerIdx.Name = "HandlerIdx";
            HandlerIdx.FieldName = "HandlerIdx";
            HandlerIdx.IsVisible = false;
            gv.Columns.Add(HandlerIdx);

            GridViewDateTimeColumn ConfirmDate = new GridViewDateTimeColumn();
            ConfirmDate.Name = "ConfirmDate";
            ConfirmDate.FieldName = "ConfirmDate";
            ConfirmDate.Width = 120;
            ConfirmDate.TextAlignment = ContentAlignment.MiddleCenter;
            ConfirmDate.FormatInfo = new System.Globalization.CultureInfo("en-US");
            ConfirmDate.FormatString = "{0:g}";
            ConfirmDate.HeaderText = "Confirm Date\n(Office)";
            ConfirmDate.ReadOnly = true;
            gv.Columns.Add(ConfirmDate);

            GridViewTextBoxColumn ConfirmUser = new GridViewTextBoxColumn();
            ConfirmUser.Name = "ConfirmUser";
            ConfirmUser.FieldName = "ConfirmUser";
            ConfirmUser.ReadOnly = true;
            ConfirmUser.Width = 90;
            ConfirmUser.TextAlignment = ContentAlignment.MiddleLeft;
            ConfirmUser.HeaderText = "Confirmed\n(Office)";
            gv.Columns.Add(ConfirmUser);

            GridViewDateTimeColumn ConfirmDateTD = new GridViewDateTimeColumn();
            ConfirmDateTD.Name = "ConfirmDateTD";
            ConfirmDateTD.FieldName = "ConfirmDateTD";
            ConfirmDateTD.Width = 120;
            ConfirmDateTD.TextAlignment = ContentAlignment.MiddleCenter;
            ConfirmDateTD.FormatInfo = new System.Globalization.CultureInfo("en-US");
            ConfirmDateTD.FormatString = "{0:g}";
            ConfirmDateTD.HeaderText = "Confirm Date\n(T/D)";
            ConfirmDateTD.ReadOnly = true;
            gv.Columns.Add(ConfirmDateTD);

            GridViewTextBoxColumn ConfirmUserTD = new GridViewTextBoxColumn();
            ConfirmUserTD.Name = "ConfirmUserTD";
            ConfirmUserTD.FieldName = "ConfirmUserTD";
            ConfirmUserTD.ReadOnly = true;
            ConfirmUserTD.Width = 90;
            ConfirmUserTD.TextAlignment = ContentAlignment.MiddleLeft;
            ConfirmUserTD.HeaderText = "Confirmed\n(T/D)";
            gv.Columns.Add(ConfirmUserTD);

            GridViewDateTimeColumn ConfirmDateLast = new GridViewDateTimeColumn();
            ConfirmDateLast.Name = "ConfirmDateLast";
            ConfirmDateLast.FieldName = "ConfirmDateLast";
            ConfirmDateLast.Width = 120;
            ConfirmDateLast.TextAlignment = ContentAlignment.MiddleCenter;
            ConfirmDateLast.FormatInfo = new System.Globalization.CultureInfo("en-US");
            ConfirmDateLast.FormatString = "{0:g}";
            ConfirmDateLast.HeaderText = "Confirm Date\n(Admin)";
            ConfirmDateLast.ReadOnly = true;
            gv.Columns.Add(ConfirmDateLast);

            GridViewTextBoxColumn ConfirmUserLast = new GridViewTextBoxColumn();
            ConfirmUserLast.Name = "ConfirmUserLast";
            ConfirmUserLast.FieldName = "ConfirmUserLast";
            ConfirmUserLast.ReadOnly = true;
            ConfirmUserLast.Width = 90;
            ConfirmUserLast.TextAlignment = ContentAlignment.MiddleLeft;
            ConfirmUserLast.HeaderText = "Confirmed\n(Admin)";
            gv.Columns.Add(ConfirmUserLast);

            GridViewComboBoxColumn Status = new GridViewComboBoxColumn();
            Status.Name = "Status";
            Status.DataSource = lstStatus;
            Status.DisplayMember = "Contents";
            Status.ValueMember = "CodeIdx";
            Status.FieldName = "Status";
            Status.HeaderText = "Status";
            Status.Width = 100;
            Status.IsVisible = true;
            Status.ReadOnly = true;
            gv.Columns.Add(Status);

            GridViewDateTimeColumn RejectDate = new GridViewDateTimeColumn();
            RejectDate.Name = "RejectDate";
            RejectDate.FieldName = "RejectDate";
            RejectDate.Width = 120;
            RejectDate.TextAlignment = ContentAlignment.MiddleCenter;
            RejectDate.FormatInfo = new System.Globalization.CultureInfo("en-US");
            RejectDate.FormatString = "{0:g}";
            RejectDate.HeaderText = "Rejected/\nCancelled Date";
            RejectDate.ReadOnly = true;
            gv.Columns.Add(RejectDate);

            GridViewTextBoxColumn Rejected = new GridViewTextBoxColumn();
            Rejected.Name = "Rejected";
            Rejected.FieldName = "Rejected";
            Rejected.ReadOnly = true;
            Rejected.Width = 90; 
            Rejected.TextAlignment = ContentAlignment.MiddleLeft;
            Rejected.HeaderText = "Rejected";
            gv.Columns.Add(Rejected);

            GridViewTextBoxColumn WorksheetIdx = new GridViewTextBoxColumn();
            WorksheetIdx.Name = "WorksheetIdx";
            WorksheetIdx.FieldName = "WorksheetIdx";
            WorksheetIdx.ReadOnly = true;
            WorksheetIdx.Width = 100;
            WorksheetIdx.TextAlignment = ContentAlignment.MiddleLeft;
            WorksheetIdx.HeaderText = "Worksheet#";
            gv.Columns.Add(WorksheetIdx);

            GridViewTextBoxColumn Comments = new GridViewTextBoxColumn();
            Comments.Name = "Comments";
            Comments.FieldName = "Comments";
            Comments.IsVisible = false;
            gv.Columns.Add(Comments);

            GridViewTextBoxColumn CommentTD = new GridViewTextBoxColumn();
            CommentTD.Name = "CommentTD";
            CommentTD.FieldName = "CommentTD";
            CommentTD.IsVisible = false;
            gv.Columns.Add(CommentTD);
            
            #region Attachment columns 

            GridViewTextBoxColumn Attached1 = new GridViewTextBoxColumn();
            Attached1.Name = "Attached1";
            Attached1.FieldName = "Attached1";
            Attached1.IsVisible = false;
            gv.Columns.Add(Attached1);

            GridViewTextBoxColumn Attached2 = new GridViewTextBoxColumn();
            Attached2.Name = "Attached2";
            Attached2.FieldName = "Attached2";
            Attached2.IsVisible = false;
            gv.Columns.Add(Attached2);

            GridViewTextBoxColumn Attached3 = new GridViewTextBoxColumn();
            Attached3.Name = "Attached3";
            Attached3.FieldName = "Attached3";
            Attached3.IsVisible = false;
            gv.Columns.Add(Attached3);

            GridViewTextBoxColumn Attached4 = new GridViewTextBoxColumn();
            Attached4.Name = "Attached4";
            Attached4.FieldName = "Attached4";
            Attached4.IsVisible = false;
            gv.Columns.Add(Attached4);

            GridViewTextBoxColumn Attached5 = new GridViewTextBoxColumn();
            Attached5.Name = "Attached5";
            Attached5.FieldName = "Attached5";
            Attached5.IsVisible = false;
            gv.Columns.Add(Attached5);

            GridViewTextBoxColumn Attached6 = new GridViewTextBoxColumn();
            Attached6.Name = "Attached6";
            Attached6.FieldName = "Attached6";
            Attached6.IsVisible = false;
            gv.Columns.Add(Attached6);

            GridViewTextBoxColumn Attached7 = new GridViewTextBoxColumn();
            Attached7.Name = "Attached7";
            Attached7.FieldName = "Attached7";
            Attached7.IsVisible = false;
            gv.Columns.Add(Attached7);

            GridViewTextBoxColumn Attached8 = new GridViewTextBoxColumn();
            Attached8.Name = "Attached8";
            Attached8.FieldName = "Attached8";
            Attached8.IsVisible = false;
            gv.Columns.Add(Attached8);

            GridViewTextBoxColumn Attached9 = new GridViewTextBoxColumn();
            Attached9.Name = "Attached9";
            Attached9.FieldName = "Attached9";
            Attached9.IsVisible = false;
            gv.Columns.Add(Attached9);

            GridViewTextBoxColumn AttachedUrl1 = new GridViewTextBoxColumn();
            AttachedUrl1.Name = "AttachedUrl1";
            AttachedUrl1.FieldName = "AttachedUrl1";
            AttachedUrl1.IsVisible = false;
            gv.Columns.Add(AttachedUrl1);

            GridViewTextBoxColumn AttachedUrl2 = new GridViewTextBoxColumn();
            AttachedUrl2.Name = "AttachedUrl2";
            AttachedUrl2.FieldName = "AttachedUrl2";
            AttachedUrl2.IsVisible = false;
            gv.Columns.Add(AttachedUrl2);

            GridViewTextBoxColumn AttachedUrl3 = new GridViewTextBoxColumn();
            AttachedUrl3.Name = "AttachedUrl3";
            AttachedUrl3.FieldName = "AttachedUrl3";
            AttachedUrl3.IsVisible = false;
            gv.Columns.Add(AttachedUrl3);

            GridViewTextBoxColumn AttachedUrl4 = new GridViewTextBoxColumn();
            AttachedUrl4.Name = "AttachedUrl4";
            AttachedUrl4.FieldName = "AttachedUrl4";
            AttachedUrl4.IsVisible = false;
            gv.Columns.Add(AttachedUrl4);

            GridViewTextBoxColumn AttachedUrl5 = new GridViewTextBoxColumn();
            AttachedUrl5.Name = "AttachedUrl5";
            AttachedUrl5.FieldName = "AttachedUrl5";
            AttachedUrl5.IsVisible = false;
            gv.Columns.Add(AttachedUrl5);

            GridViewTextBoxColumn AttachedUrl6 = new GridViewTextBoxColumn();
            AttachedUrl6.Name = "AttachedUrl6";
            AttachedUrl6.FieldName = "AttachedUrl6";
            AttachedUrl6.IsVisible = false;
            gv.Columns.Add(AttachedUrl6);

            GridViewTextBoxColumn AttachedUrl7 = new GridViewTextBoxColumn();
            AttachedUrl7.Name = "AttachedUrl7";
            AttachedUrl7.FieldName = "AttachedUrl7";
            AttachedUrl7.IsVisible = false;
            gv.Columns.Add(AttachedUrl7);

            GridViewTextBoxColumn AttachedUrl8 = new GridViewTextBoxColumn();
            AttachedUrl8.Name = "AttachedUrl8";
            AttachedUrl8.FieldName = "AttachedUrl8";
            AttachedUrl8.IsVisible = false;
            gv.Columns.Add(AttachedUrl8);

            GridViewTextBoxColumn AttachedUrl9 = new GridViewTextBoxColumn();
            AttachedUrl9.Name = "AttachedUrl9";
            AttachedUrl9.FieldName = "AttachedUrl9";
            AttachedUrl9.IsVisible = false;
            gv.Columns.Add(AttachedUrl9);

            #endregion

            GridViewTextBoxColumn CommentFabric = new GridViewTextBoxColumn();
            CommentFabric.Name = "CommentFabric";
            CommentFabric.FieldName = "CommentFabric";
            CommentFabric.IsVisible = false;
            gv.Columns.Add(CommentFabric);

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
            Font f = new Font(new FontFamily("Segoe UI"), 8.25f);
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
                //// 페이징 되어 있는 경우, 제일 첫페이지로 이동한후, 
                //if (CommonValues.enablePaging == true)
                //{
                //    gv.MasterTemplate.MoveToFirstPage();
                //}
                //gv.CurrentRow = gv.Rows[row];
                //if (gv.CurrentRow != null)
                //{
                //    gv.CurrentRow.IsSelected = true;
                //}
                if (_currentRow != null)
                {
                    gv.CurrentRow = _currentRow;
                    gv.CurrentRow.IsSelected = true;
                    gv.TableElement.ScrollToRow(_currentRow); 
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
            // 부서    
            _dt = CommonController.Getlist(CommonValues.KeyName.DeptIdx).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                // 관리부와 임원은 모든 부서에 접근가능
                if (Options.UserInfo.CenterIdx != 1 || Options.UserInfo.DeptIdx == 5 || Options.UserInfo.DeptIdx == 6 || 
                    Options.UserInfo.ExceptionGroup == 233)
                {
                    deptName.Add(new DepartmentName(Convert.ToInt32(row["DeptIdx"]),
                                                row["DeptName"].ToString(),
                                                Convert.ToInt32(row["CostcenterIdx"])));
                }
                // 영업부는 해당 부서 데이터만 접근가능
                else
                {
                    if (Convert.ToInt32(row["DeptIdx"]) <= 0 || Convert.ToInt32(row["DeptIdx"]) == Options.UserInfo.DeptIdx)
                    {
                        deptName.Add(new DepartmentName(Convert.ToInt32(row["DeptIdx"]),
                                                row["DeptName"].ToString(),
                                                Convert.ToInt32(row["CostcenterIdx"])));
                    }
                }

                //if (UserInfo.ExceptionGroup == 233)     // 팀장급 이상일 경우, 
            }

            // 바이어
            _dt = CommonController.Getlist(CommonValues.KeyName.CustAll).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                custName.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
            }
            
            //// 코드
            //_dt = CommonController.Getlist(CommonValues.KeyName.Codes).Tables[0];

            //foreach (DataRow row in _dt.Rows)
            //{
            //    codeName.Add(new CodeContents(Convert.ToInt32(row["Idx"]),
            //                                row["Contents"].ToString(),
            //                                row["Classification"].ToString()));
            //}

            //// 사이즈명 
            //_dt = Dev.Codes.Controller.Sizes.GetUselist().Tables[0];
            
            //sizeName = new List<CodeContents>();

            //sizeName.Add(new CodeContents(0, "", "")); 
            //foreach (DataRow row in _dt.Rows)
            //{

            //    sizeName.Add(new CodeContents(Convert.ToInt32(row["SizeIdx"]),
            //                            row["SizeName"].ToString(),
            //                            ""));
            //}
            
            // Username
            lstUser.Add(new CustomerName(0, "", 0));

            if (Options.UserInfo.CenterIdx != 1 || Options.UserInfo.DeptIdx == 5 || Options.UserInfo.DeptIdx == 6)
            {
                _dt = CommonController.Getlist(CommonValues.KeyName.AllUser).Tables[0];
            }
            else
            {
                _dt = CommonController.Getlist(CommonValues.KeyName.User).Tables[0];
            }

            foreach (DataRow row in _dt.Rows)
            {
                lstUser.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]),
                                            row["UserName"].ToString(),
                                            Convert.ToInt32(row["DeptIdx"])));
            }

            // 오더상태 (CommonValues정의)
            lstStatus.Add(new CodeContents(0, CommonValues.DicWorkOrderStatus[0], ""));
            lstStatus.Add(new CodeContents(1, CommonValues.DicWorkOrderStatus[1], ""));
            lstStatus.Add(new CodeContents(2, CommonValues.DicWorkOrderStatus[2], ""));
            lstStatus.Add(new CodeContents(3, CommonValues.DicWorkOrderStatus[3], ""));
            lstStatus.Add(new CodeContents(4, CommonValues.DicWorkOrderStatus[4], ""));
            lstStatus.Add(new CodeContents(5, CommonValues.DicWorkOrderStatus[5], ""));
            lstStatus.Add(new CodeContents(6, CommonValues.DicWorkOrderStatus[6], ""));
            lstStatus.Add(new CodeContents(7, CommonValues.DicWorkOrderStatus[7], ""));
            lstStatus.Add(new CodeContents(8, CommonValues.DicWorkOrderStatus[8], ""));
            lstStatus.Add(new CodeContents(10, CommonValues.DicWorkOrderStatus[10], ""));
            lstStatus.Add(new CodeContents(11, CommonValues.DicWorkOrderStatus[11], ""));
            lstStatus.Add(new CodeContents(12, CommonValues.DicWorkOrderStatus[12], ""));
            lstStatus.Add(new CodeContents(13, CommonValues.DicWorkOrderStatus[13], ""));
            lstStatus.Add(new CodeContents(14, CommonValues.DicWorkOrderStatus[14], ""));
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
                //if (ddlCust.SelectedValue != null || ddlSize.SelectedValue != null
                //    || ddlStatus.SelectedValue != null || ddlSize.SelectedValue != null
                //    || !string.IsNullOrEmpty(txtFileno.Text) || !string.IsNullOrEmpty(txtStyle.Text))
                //{
                _searchKey = new Dictionary<CommonValues.KeyName, int>();

                // 영업부인경우, 해당 부서만 조회할수 있도록 제한 
                if (Options.UserInfo.ReportNo < 9 && Options.UserInfo.ExceptionGroup != 233)
                    _searchKey.Add(CommonValues.KeyName.DeptIdx, Options.UserInfo.DeptIdx);
                else
                    _searchKey.Add(CommonValues.KeyName.DeptIdx, Convert.ToInt32(ddlDept.SelectedValue));

                //_searchKey.Add(CommonValues.KeyName.CustIdx, Convert.ToInt32(ddlCust.SelectedValue));
                //_searchKey.Add(CommonValues.KeyName.Status, Convert.ToInt32(ddlStatus.SelectedValue));
                //_searchKey.Add(CommonValues.KeyName.Size, Convert.ToInt32(ddlSize.SelectedValue));

                //int OrderIdx, string WorksheetIdx, int Handler, int ConfirmUser, int WorkStatus

                DataBinding_GV1(_searchKey[CommonValues.KeyName.DeptIdx], Convert.ToInt32(ddlCust.SelectedValue), 
                                Convert.ToInt32(ddlHandler.SelectedValue), Convert.ToInt32(ddlStatus.SelectedValue), 
                                txtFileno.Text.Trim(), txtStyle.Text.Trim(), txtWorksheet.Text.Trim(),
                                WorksheetOpCheck1.Checked ? 1 : 0,
                                WorksheetOpCheck2.Checked ? 1 : 0,
                                WorksheetOpCheck3.Checked ? 1 : 0,
                                WorksheetOpCheck4.Checked ? 1 : 0,
                                WorksheetOpCheck5.Checked ? 1 : 0
                                );
                //}
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
        private void DataBinding_GV1(int DeptIdx, int CustIdx, int Handler, int WorkStatus, string Fileno, string Styleno, string WorksheetIdx,
                                    int OptionCheck1, int OptionCheck2, int OptionCheck3, int OptionCheck4, int OptionCheck5)
        {
            try
            {
                _gv1.DataSource = null;
                
                if (toggTD.Value)
                {
                    // TD일땐 해당 바이어만 
                    _ds1 = Controller.Worksheet.Getlist(DeptIdx, CustIdx, Handler, WorkStatus, Fileno, Styleno, WorksheetIdx, Options.UserInfo.Idx,
                                    OptionCheck1, OptionCheck2, OptionCheck3, OptionCheck4, OptionCheck5);
                }
                else
                {
                    // TD아니면 모든 바이어 
                    _ds1 = Controller.Worksheet.Getlist(DeptIdx, CustIdx, Handler, WorkStatus, Fileno, Styleno, WorksheetIdx,
                                    OptionCheck1, OptionCheck2, OptionCheck3, OptionCheck4, OptionCheck5);
                }
                
                if (_ds1 != null)
                {
                    _gv1.DataSource = _ds1.Tables[0].DefaultView;
                    __main__.lblRows.Text = _gv1.RowCount.ToString() + " Rows"; 
                    _gv1.EnablePaging = CommonValues.enablePaging;
                    _gv1.AllowSearchRow = CommonValues.enableSearchRow;
                    //SetCurrentRow(_gv1, 0);

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
                ///// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                ///// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
                //int _mode_ = 1;
                //if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                //    CheckAuth.ShowMessage(_mode_);
                //else
                //{

                //    _gv1.EndEdit();
                //    GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);  // 현재 행번호 확인

                //    // 객체생성 및 값 할당
                //    _obj1 = new Controller.Worksheet(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value));
                //    _obj1.Idx = Convert.ToInt32(row.Cells["Idx"].Value.ToString());

                //    if (row.Cells["OrdSizeIdx"].Value != DBNull.Value) _obj1.OrdSizeIdx = Convert.ToInt32(row.Cells["OrdSizeIdx"].Value.ToString());

                //    if (row.Cells["TechpackDate"].Value != DBNull.Value && row.Cells["TechpackDate"] != null)
                //    {
                //        _obj1.TechpackDate = Convert.ToDateTime(row.Cells["TechpackDate"].Value);
                //    }
                //    else
                //    {
                //        _obj1.TechpackDate = new DateTime(2000, 1, 1);
                //    }

                //    if (row.Cells["RequestedDate"].Value != DBNull.Value) _obj1.RequestedDate = Convert.ToDateTime(row.Cells["RequestedDate"].Value);
                //    if (row.Cells["Requested"].Value != DBNull.Value) _obj1.Requested = Convert.ToInt32(row.Cells["Requested"].Value.ToString());

                //    if (row.Cells["ConfirmedDate"].Value != DBNull.Value && row.Cells["ConfirmedDate"] != null)
                //    {
                //        _obj1.ConfirmedDate = Convert.ToDateTime(row.Cells["ConfirmedDate"].Value);
                //    }
                //    else
                //    {
                //        _obj1.ConfirmedDate = new DateTime(2000, 1, 1);
                //    }

                //    if (row.Cells["Confirmed"].Value != DBNull.Value) _obj1.Confirmed = Convert.ToInt32(row.Cells["Confirmed"].Value.ToString());

                //    if (row.Cells["CompletedDate"].Value != DBNull.Value && row.Cells["CompletedDate"] != null)
                //    {
                //        _obj1.CompletedDate = Convert.ToDateTime(row.Cells["CompletedDate"].Value);
                //    }
                //    else
                //    {
                //        _obj1.CompletedDate = new DateTime(2000, 1, 1);
                //    }

                //    if (row.Cells["SentDate"].Value != DBNull.Value && row.Cells["SentDate"] != null)
                //    {
                //        _obj1.SentDate = Convert.ToDateTime(row.Cells["SentDate"].Value);
                //    }
                //    else
                //    {
                //        _obj1.SentDate = new DateTime(2000, 1, 1);
                //    }

                //    if (row.Cells["Received"].Value != DBNull.Value) _obj1.Received = Convert.ToInt32(row.Cells["Received"].Value.ToString());
                //    if (row.Cells["Remarks"].Value != DBNull.Value) _obj1.Remarks = row.Cells["Remarks"].Value.ToString();

                //    //if (row.Cells["Attached1"].Value != DBNull.Value) _obj1.Attached1 = row.Cells["Attached1"].Value.ToString();
                //    //if (row.Cells["Attached2"].Value != DBNull.Value) _obj1.Attached2 = row.Cells["Attached2"].Value.ToString();
                //    //if (row.Cells["Attached3"].Value != DBNull.Value) _obj1.Attached3 = row.Cells["Attached3"].Value.ToString();
                //    //if (row.Cells["Attached4"].Value != DBNull.Value) _obj1.Attached4 = row.Cells["Attached4"].Value.ToString();
                //    //if (row.Cells["Attached5"].Value != DBNull.Value) _obj1.Attached5 = row.Cells["Attached5"].Value.ToString();

                //    //if (row.Cells["AttachedUrl1"].Value != DBNull.Value) _obj1.AttachedUrl1 = row.Cells["AttachedUrl1"].Value.ToString();
                //    //if (row.Cells["AttachedUrl2"].Value != DBNull.Value) _obj1.AttachedUrl2 = row.Cells["AttachedUrl2"].Value.ToString();
                //    //if (row.Cells["AttachedUrl3"].Value != DBNull.Value) _obj1.AttachedUrl3 = row.Cells["AttachedUrl3"].Value.ToString();
                //    //if (row.Cells["AttachedUrl4"].Value != DBNull.Value) _obj1.AttachedUrl4 = row.Cells["AttachedUrl4"].Value.ToString();
                //    //if (row.Cells["AttachedUrl5"].Value != DBNull.Value) _obj1.AttachedUrl5 = row.Cells["AttachedUrl5"].Value.ToString();

                //    // 업데이트 (오더캔슬, 선적완료 상태가 아닐경우)
                //    _bRtn = _obj1.Update();
                //    __main__.lblRows.Text = "Updated Pattern Info";

                //}

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
            try
            {
                RadMultiColumnComboBoxElement meditor = e.ActiveEditor as RadMultiColumnComboBoxElement;

                if (meditor != null)
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
            catch(Exception ex)
            {
                
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
            try
            {
                _currentRow = Int.Members.GetCurrentRow(_gv1);

                if (Int.Members.GetCurrentRow(_gv1, "Status") == 2
                || Int.Members.GetCurrentRow(_gv1, "Status") == 3)
                {
                    Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = true;
                }
                else
                {
                    Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = false;
                }
                
                GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);
                
                if (row.Cells["Attached1"].Value != DBNull.Value) linkLabel1.Text = row.Cells["Attached1"].Value.ToString();
                if (row.Cells["Attached2"].Value != DBNull.Value) linkLabel2.Text = row.Cells["Attached2"].Value.ToString();
                if (row.Cells["Attached3"].Value != DBNull.Value) linkLabel3.Text = row.Cells["Attached3"].Value.ToString();
                if (row.Cells["Attached4"].Value != DBNull.Value) linkLabel4.Text = row.Cells["Attached4"].Value.ToString();
                if (row.Cells["Attached5"].Value != DBNull.Value) linkLabel5.Text = row.Cells["Attached5"].Value.ToString();
                if (row.Cells["Attached6"].Value != DBNull.Value) linkLabel6.Text = row.Cells["Attached6"].Value.ToString();
                if (row.Cells["Attached7"].Value != DBNull.Value) linkLabel7.Text = row.Cells["Attached7"].Value.ToString();
                if (row.Cells["Attached8"].Value != DBNull.Value) linkLabel8.Text = row.Cells["Attached8"].Value.ToString();
                if (row.Cells["Attached9"].Value != DBNull.Value) linkLabel9.Text = row.Cells["Attached9"].Value.ToString();

                // 컨트롤 초기화 
                txtComments.Enabled = true;
                txtCommentTD.Enabled = true;
                btnCancel.Visible = true;
                btnSaveData.Visible = true;
                btnReject.Visible = true;
                btnSaveData.Enabled = true;
                btnReject.Enabled = true;
                
                if (Options.UserInfo.DeptIdx == 7 && Options.UserInfo.IsLeader != 1) // 사무실
                {
                    txtComments.Enabled = true;
                    txtCommentTD.Enabled = true;
                    btnCancel.Visible = false;
                    btnSaveData.Text = "Confirmed";

                    if (!string.IsNullOrEmpty(row.Cells["ConfirmUser"].Value.ToString()) ||
                        !string.IsNullOrEmpty(row.Cells["Rejected"].Value.ToString()))
                    {
                        btnSaveData.Enabled = false;
                    }
                }
                else if (Options.UserInfo.DeptIdx == 12)  // TD
                {
                    txtComments.Enabled = true;
                    txtCommentTD.Enabled = true;
                    btnCancel.Visible = false;
                    btnSaveData.Text = "Confirmed";

                    if (!string.IsNullOrEmpty(row.Cells["ConfirmUserTD"].Value.ToString()) ||
                        !string.IsNullOrEmpty(row.Cells["Rejected"].Value.ToString()))
                    {
                        btnSaveData.Enabled = false;
                        btnReject.Enabled = false;
                    }
                }
                else if (Options.UserInfo.DeptIdx == 7 && Options.UserInfo.IsLeader == 1) // 개발실 총괄
                {
                    txtComments.Enabled = true;
                    txtCommentTD.Enabled = true;
                    btnCancel.Visible = false;
                    btnSaveData.Text = "Confirmed";

                    if (!string.IsNullOrEmpty(row.Cells["ConfirmUserLast"].Value.ToString()) ||
                        !string.IsNullOrEmpty(row.Cells["Rejected"].Value.ToString()))
                    {
                        btnSaveData.Enabled = false;
                    }
                } 
                else // 영업부 
                {
                    txtCommentTD.Enabled = true;
                    btnSaveData.Visible = false;
                    btnReject.Visible = false;
                    txtComments.Enabled = true;
                    
                    if ((!string.IsNullOrEmpty(row.Cells["ConfirmUserTD"].Value.ToString())) ||
                        (Options.UserInfo.Idx!=Convert.ToInt32(row.Cells["HandlerIdx"].Value)) ||
                        (!string.IsNullOrEmpty(row.Cells["ConfirmUserLast"].Value.ToString())) ||
                        (!string.IsNullOrEmpty(row.Cells["Rejected"].Value.ToString()))) {

                        btnCancel.Visible = false;
                    }
                }

                txtComments.Text = "";
                txtCommentTD.Text = "";
                txtFabricComment.Text = "";
                if (row.Cells["Comments"].Value != DBNull.Value) txtComments.Text = row.Cells["Comments"].Value.ToString();
                if (row.Cells["CommentTD"].Value != DBNull.Value) txtCommentTD.Text = row.Cells["CommentTD"].Value.ToString();
                if (row.Cells["CommentFabric"].Value != DBNull.Value) txtFabricComment.Text = row.Cells["CommentFabric"].Value.ToString();

                // 캔슬또는 완료 건이면 승인, 취소 locking
                if (Convert.ToInt32(row.Cells["Status"].Value)==3 || Convert.ToInt32(row.Cells["Status"].Value) == 4 ||
                    Options.UserInfo.DeptIdx==5 || Options.UserInfo.DeptIdx==6) // 관리부, 임원 버튼 접근불가 
                {
                    txtComments.Enabled = true;
                    txtCommentTD.Enabled = true;
                    btnCancel.Visible = false;
                    btnSaveData.Visible = false;
                    btnReject.Visible = false;
                }

                // 원단담당자 버튼 비활성화 
                if (Options.UserInfo.Idx == 18)
                {
                    btnCancel.Visible = false;
                    btnSaveData.Visible = false;
                    btnReject.Visible = false;
                }

                SetDefaultFontPropertiesToEditor(txtComments);
                SetDefaultFontPropertiesToEditor(txtCommentTD);
                SetDefaultFontPropertiesToEditor(txtFabricComment);
            }
            catch(Exception ex)
            {

            }

        }

        public void SetDefaultFontPropertiesToEditor(RadRichTextEditor editor)
        {
            editor.Document.Selection.SelectAll();
            editor.RichTextBoxElement.ChangeFontFamily(new Telerik.WinControls.RichTextEditor.UI.FontFamily("Segoe UI"));
            editor.RichTextBoxElement.ChangeFontSize(Unit.PointToDip(9));
            editor.RichTextBoxElement.ChangeFontStyle(Telerik.WinControls.RichTextEditor.UI.FontStyles.Normal);
            editor.RichTextBoxElement.ChangeFontWeight(Telerik.WinControls.RichTextEditor.UI.FontWeights.Normal);

            editor.RichTextBoxElement.ChangeParagraphLineSpacingType(LineSpacingType.Auto);
            editor.RichTextBoxElement.ChangeParagraphLineSpacing(1);
            editor.RichTextBoxElement.ChangeParagraphSpacingAfter(12);

            editor.DocumentInheritsDefaultStyleSettings = true;

            Telerik.WinForms.Documents.DocumentPosition startPosition = editor.Document.CaretPosition;
            Telerik.WinForms.Documents.DocumentPosition endPosition = new Telerik.WinForms.Documents.DocumentPosition(startPosition);
            startPosition.MoveToCurrentWordEnd();
            endPosition.MoveToCurrentWordEnd();
            editor.Document.Selection.AddSelectionStart(startPosition);
            editor.Document.Selection.AddSelectionEnd(endPosition);
        }

        /// <summary>
        /// 업데이트 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
                int _mode_ = 1;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    _gv1.EndEdit();
                    GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);

                    if (Options.UserInfo.DeptIdx == 12)     // TD
                    {
                        _bRtn = Data.WorksheetData.ConfirmTD(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value),
                        Options.UserInfo.Idx, txtCommentTD.Text.Trim(),
                        Convert.ToInt32(_gv1.Rows[row.Index].Cells["Status"].Value));

                        if (_bRtn)
                        {
                            __main__.lblRows.Text = "Confirmed Worksheet by TD";
                            RadMessageBox.Show("Confirmed Worksheet by TD.", "Confirmed TD");
                        }
                    } 
                    else if (Options.UserInfo.DeptIdx == 7 && Options.UserInfo.IsLeader==1)     // 개발 총괄
                    {
                        _bRtn = Data.WorksheetData.ConfirmAdmin(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value),
                        Options.UserInfo.Idx, txtCommentTD.Text.Trim());

                        if (_bRtn)
                        {
                            __main__.lblRows.Text = "Confirmed Worksheet by Admin";
                            RadMessageBox.Show("Confirmed Worksheet by Admin.", "Confirmed");
                        }
                    }
                    else if (Options.UserInfo.DeptIdx == 7 && Options.UserInfo.IsLeader != 1)     // 사무실
                    {
                        _bRtn = Data.WorksheetData.ConfirmOffice(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value),
                        Options.UserInfo.Idx, txtCommentTD.Text.Trim(),
                        Convert.ToInt32(_gv1.Rows[row.Index].Cells["Status"].Value));

                        if (_bRtn)
                        {
                            __main__.lblRows.Text = "Confirmed Worksheet by Office";
                            RadMessageBox.Show("Confirmed Worksheet by Office.", "Confirmed");
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("btnUpdate_Click: " + ex.Message.ToString());
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
            // Tackpack 첨부파일 링크 
            try
            {
                string container = "intsampleworksheet";
                string processfname = @"C:\INT\Data\intsampleworksheet\"; 
                string filename = ((System.Windows.Forms.LinkLabel)sender).Text.Trim();

                CheckFolder(@"C:\INT\Data\intsampleworksheet");
                CheckFolder(@"C:\INT\Data\intsampleworksheet\Development\temp");
                
                if (filename.ToString().PadLeft(16) == "Development/temp")
                {
                    container = "intsampleworksheet/Development/temp";
                    processfname = @"C:\INT\Data\intsampleworksheet\Development\temp";
                    filename = filename.Substring(15); 
                }

                Download_File(container, filename);
                Process process = new Process();
                process.StartInfo.FileName = processfname + filename;
                process.Start();
            }
            catch(Exception ex) {  }
        }
        
        private void Download_File(string containerurl, string filename)
        {
            try
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
            catch(Exception ex)
            {

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

        private void gvOrderActual_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            // Overdue 
            if (e.CellElement.ColumnInfo.Name == "RequestedDate" || e.CellElement.ColumnInfo.Name == "ConfirmedDate" || 
                e.CellElement.ColumnInfo.Name == "CompletedDate")
            {
                e.CellElement.BackColor = Color.LightYellow;
                e.CellElement.ForeColor = Color.Black;
                e.CellElement.GradientStyle = GradientStyles.Solid;
                e.CellElement.DrawFill = true;
                e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
            }
            else
            {
                e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);


            }
            
        }

        private void LinkLabel_LinkClicked2(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 요척 첨부파일 링크 
            //try
            //{
            //    CheckFolder(@"C:\INT\Data\intsampleconsum");
            //    Download_File("intsampleconsum", ((System.Windows.Forms.LinkLabel)sender).Text);
            //    Process process = new Process();
            //    process.StartInfo.FileName = @"C:\INT\Data\intsampleconsum\" + ((System.Windows.Forms.LinkLabel)sender).Text.Trim();
            //    process.Start();
            //}
            //catch (Exception ex) { }
        }

        /// <summary>
        /// azure storage 파일 업로드
        /// </summary>
        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                int tagNo = Convert.ToInt16(((RadBrowseEditor)sender).Tag); 
                //bool result = Data.UpdateData.DeleteAll(_selectedNode);

                // 스토리지 설정 
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // 선택된 패키지의 폴더안의 blob 리스트 조회 (updateinterp, updateintsample, ...) 
                CloudBlobContainer container = blobClient.GetContainerReference(CommonValues.packageName + "consum");

                string[] fileNames = GetFiles();

                if (fileNames != null)
                {
                    foreach (string filename in fileNames)
                    {
                        // 업데이트 파일 storage저장 
                        using (var fileStream = System.IO.File.OpenRead(filename))
                        {
                            // blob명은 파일명과 같도록 생성
                            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename.Substring(filename.LastIndexOf("\\") + 1));

                            blockBlob.UploadFromStream(fileStream);

                            lstFiles[tagNo] = filename.Substring(filename.LastIndexOf("\\") + 1);
                            lstFileUrls[tagNo] =blockBlob.StorageUri.PrimaryUri.ToString();

                        }

                    }

                }
                RadMessageBox.Show("Uploaded completed.", "Saved"); 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// 다중 파일 선택
        /// </summary>
        /// <returns>string[] filenames</returns>
        private string[] GetFiles()
        {
            string[] fileNames;
            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.Filter = "All files|*.*";
            openDialog.Title = "Select files to upload";
            openDialog.RestoreDirectory = true;
            openDialog.Multiselect = false;
            openDialog.CheckFileExists = true;

            try
            {
                DialogResult result = openDialog.ShowDialog();

                if (result == DialogResult.OK && openDialog.FileNames.Length <= 1)
                {
                    //listFiles.Items.Clear();

                    //for (int i = 0; i < openDialog.FileNames.Length; i++)
                    //{
                    //    FileOpen_ListView(openDialog.FileNames[i], listFiles);
                    //}

                    return fileNames = openDialog.FileNames;

                }
                else if (result == DialogResult.Cancel)
                {
                    return null;
                }
                else
                {
                    if (MessageBox.Show("Too many files were Selected. Please select files less than 5.",
                        "Too many files...", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                return null;
            }

        }

        private void gvWorksheet_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            try
            {
                RadMultiColumnComboBoxElement meditor = e.ActiveEditor as RadMultiColumnComboBoxElement;

                if (meditor != null)
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
            catch (Exception ex)
            {

            }
        }

        private void gvWorksheet_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (Options.UserInfo.DeptIdx != 7 && Options.UserInfo.IsLeader != 1)
            {
                if (e.CellElement.ColumnInfo.Name == "ConfirmUser" || e.CellElement.ColumnInfo.Name == "ConfirmDate")
                {
                    e.CellElement.BackColor = Color.LightYellow;
                    e.CellElement.ForeColor = Color.Black;
                    e.CellElement.GradientStyle = GradientStyles.Solid;
                    e.CellElement.DrawFill = true;
                    //e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
                }
                else if (e.CellElement.ColumnInfo.Name == "ConfirmUserTD" || e.CellElement.ColumnInfo.Name == "ConfirmDateTD")
                {
                    e.CellElement.BackColor = Color.PaleGreen;
                    e.CellElement.ForeColor = Color.Black;
                    e.CellElement.GradientStyle = GradientStyles.Solid;
                    e.CellElement.DrawFill = true;
                    //e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
                }
                else if (e.CellElement.ColumnInfo.Name == "RejectDate" || e.CellElement.ColumnInfo.Name == "Rejected")
                {
                    e.CellElement.BackColor = Color.White;
                    e.CellElement.ForeColor = Color.Red;
                    e.CellElement.GradientStyle = GradientStyles.Solid;
                    e.CellElement.DrawFill = true;
                    //e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
                }
                else if (e.CellElement.ColumnInfo.Name == "RegDate" || e.CellElement.ColumnInfo.Name == "Handler")
                {
                    e.CellElement.BackColor = Color.SeaShell;
                    e.CellElement.ForeColor = Color.Black;
                    e.CellElement.GradientStyle = GradientStyles.Solid;
                    e.CellElement.DrawFill = true;
                    //e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
                }
                else if (e.CellElement.ColumnInfo.Name == "ConfirmUserLast" || e.CellElement.ColumnInfo.Name == "ConfirmDateLast")
                {
                    e.CellElement.BackColor = Color.AliceBlue;
                    e.CellElement.ForeColor = Color.Black;
                    e.CellElement.GradientStyle = GradientStyles.Solid;
                    e.CellElement.DrawFill = true;
                    //e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
                }
                else
                {
                    e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                    e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                    e.CellElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                    e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);


                }
            }
                
        }

        private void gvWorksheet_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
            //_bRtn = false;
            //try
            //{
            //    /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
            //    /// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
            //    int _mode_ = 1;
            //    if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
            //        CheckAuth.ShowMessage(_mode_);
            //    else
            //    {

            //        _gv1.EndEdit();
            //        GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);  // 현재 행번호 확인

            //        // 객체생성 및 값 할당
            //        _obj1 = new Controller.Pattern(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value));
            //        _obj1.Idx = Convert.ToInt32(row.Cells["Idx"].Value.ToString());

            //        if (row.Cells["OrdSizeIdx"].Value != DBNull.Value) _obj1.OrdSizeIdx = Convert.ToInt32(row.Cells["OrdSizeIdx"].Value.ToString());

            //        if (row.Cells["TechpackDate"].Value != DBNull.Value && row.Cells["TechpackDate"] != null)
            //        {
            //            _obj1.TechpackDate = Convert.ToDateTime(row.Cells["TechpackDate"].Value);
            //        }
            //        else
            //        {
            //            _obj1.TechpackDate = new DateTime(2000, 1, 1);
            //        }

            //        if (row.Cells["RequestedDate"].Value != DBNull.Value) _obj1.RequestedDate = Convert.ToDateTime(row.Cells["RequestedDate"].Value);
            //        if (row.Cells["Requested"].Value != DBNull.Value) _obj1.Requested = Convert.ToInt32(row.Cells["Requested"].Value.ToString());

            //        if (row.Cells["ConfirmedDate"].Value != DBNull.Value && row.Cells["ConfirmedDate"] != null)
            //        {
            //            _obj1.ConfirmedDate = Convert.ToDateTime(row.Cells["ConfirmedDate"].Value);
            //        }
            //        else
            //        {
            //            _obj1.ConfirmedDate = new DateTime(2000, 1, 1);
            //        }

            //        if (row.Cells["Confirmed"].Value != DBNull.Value) _obj1.Confirmed = Convert.ToInt32(row.Cells["Confirmed"].Value.ToString());

            //        if (row.Cells["CompletedDate"].Value != DBNull.Value && row.Cells["CompletedDate"] != null)
            //        {
            //            _obj1.CompletedDate = Convert.ToDateTime(row.Cells["CompletedDate"].Value);
            //        }
            //        else
            //        {
            //            _obj1.CompletedDate = new DateTime(2000, 1, 1);
            //        }

            //        if (row.Cells["SentDate"].Value != DBNull.Value && row.Cells["SentDate"] != null)
            //        {
            //            _obj1.SentDate = Convert.ToDateTime(row.Cells["SentDate"].Value);
            //        }
            //        else
            //        {
            //            _obj1.SentDate = new DateTime(2000, 1, 1);
            //        }

            //        if (row.Cells["Received"].Value != DBNull.Value) _obj1.Received = Convert.ToInt32(row.Cells["Received"].Value.ToString());
            //        if (row.Cells["Remarks"].Value != DBNull.Value) _obj1.Remarks = row.Cells["Remarks"].Value.ToString();

            //        //if (row.Cells["Attached1"].Value != DBNull.Value) _obj1.Attached1 = row.Cells["Attached1"].Value.ToString();
            //        //if (row.Cells["Attached2"].Value != DBNull.Value) _obj1.Attached2 = row.Cells["Attached2"].Value.ToString();
            //        //if (row.Cells["Attached3"].Value != DBNull.Value) _obj1.Attached3 = row.Cells["Attached3"].Value.ToString();
            //        //if (row.Cells["Attached4"].Value != DBNull.Value) _obj1.Attached4 = row.Cells["Attached4"].Value.ToString();
            //        //if (row.Cells["Attached5"].Value != DBNull.Value) _obj1.Attached5 = row.Cells["Attached5"].Value.ToString();

            //        //if (row.Cells["AttachedUrl1"].Value != DBNull.Value) _obj1.AttachedUrl1 = row.Cells["AttachedUrl1"].Value.ToString();
            //        //if (row.Cells["AttachedUrl2"].Value != DBNull.Value) _obj1.AttachedUrl2 = row.Cells["AttachedUrl2"].Value.ToString();
            //        //if (row.Cells["AttachedUrl3"].Value != DBNull.Value) _obj1.AttachedUrl3 = row.Cells["AttachedUrl3"].Value.ToString();
            //        //if (row.Cells["AttachedUrl4"].Value != DBNull.Value) _obj1.AttachedUrl4 = row.Cells["AttachedUrl4"].Value.ToString();
            //        //if (row.Cells["AttachedUrl5"].Value != DBNull.Value) _obj1.AttachedUrl5 = row.Cells["AttachedUrl5"].Value.ToString();

            //        // 업데이트 (오더캔슬, 선적완료 상태가 아닐경우)
            //        _bRtn = _obj1.Update();
            //        __main__.lblRows.Text = "Updated Pattern Info";

            //    }

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("GV1_Update: " + ex.Message.ToString());
            //}
        }

        private void gvWorksheet_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font f = new Font(new FontFamily("Segoe UI"), 8.25f);

            // 중앙 그리드뷰 헤더 폰트 변경
            GridHeaderCellElement element = sender as GridHeaderCellElement;
            if (element != null)
            {
                element.Font = f;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
                int _mode_ = 1;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {

                    if (string.IsNullOrEmpty(txtComments.Text.Trim()))
                    {
                        RadMessageBox.Show("Please input cancel reason.", "Cancel Worksheet");
                        return; 
                    }

                    _gv1.EndEdit();
                    GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);

                    _bRtn = Data.WorksheetData.CancelWorksheetTeam(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value),
                        Options.UserInfo.Idx, txtComments.Text.Trim());

                    if (_bRtn)
                    {
                        __main__.lblRows.Text = "Cancelled Worksheet";
                        RadMessageBox.Show("Cancelled Worksheet.", "Cancelled");
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("btnUpdate_Click: " + ex.Message.ToString());
            }
            
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
                int _mode_ = 1;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    _gv1.EndEdit();
                    GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);

                    if (string.IsNullOrEmpty(txtCommentTD.Text.Trim()))
                    {
                        RadMessageBox.Show("Please input cancel reason.", "Cancel Worksheet");
                        return;
                    }

                    if (Options.UserInfo.DeptIdx == 12)     // TD
                    {
                        _bRtn = Data.WorksheetData.RejectTD(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value),
                                                            Options.UserInfo.Idx, txtCommentTD.Text.Trim());

                        if (_bRtn)
                        {
                            __main__.lblRows.Text = "Rejected Worksheet by TD";
                            RadMessageBox.Show("Rejected Worksheet by TD.", "Rejected");
                            
                        }

                        // 오더핸들러 전화번호가 등록되어 있는 경우
                        DataRow dr = Dev.Options.Data.CommonData.GetPhoneNumberbyOrderID(Convert.ToInt32(_gv1.Rows[row.Index].Cells["OrderIdx"].Value.ToString()));
                        if (dr != null && !string.IsNullOrEmpty(dr["Phone"].ToString().Trim()))
                        {
                            // 결과 메시지 송신
                            Controller.TelegramMessageSender msgSender = new Controller.TelegramMessageSender();
                            msgSender.sendMessage(dr["Phone"].ToString().Trim(), "[작지반려] " +
                                        "Buyer: " + _gv1.Rows[row.Index].Cells["Buyer"].Value.ToString() + ", " +
                                        "File: " + _gv1.Rows[row.Index].Cells["Fileno"].Value.ToString() + ", " +
                                        "Style: " + _gv1.Rows[row.Index].Cells["Styleno"].Value.ToString() + ", " +
                                        "Worksheet#: " + _gv1.Rows[row.Index].Cells["WorksheetIdx"].Value.ToString() + ", " +
                                        "Rejected Date: " + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm") + ", " +
                                        "Rejected by " + Options.UserInfo.Userfullname.ToString() + "\n" +
                                         "Comment: " + txtCommentTD.Text.ToString()
                                        );
                        }
                    }
                    else if (Options.UserInfo.DeptIdx == 7 && Options.UserInfo.IsLeader==1)     // 개발 총괄
                    {
                        _bRtn = Data.WorksheetData.RejectAdmin(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value),
                        Options.UserInfo.Idx, txtCommentTD.Text.Trim());

                        if (_bRtn)
                        {
                            __main__.lblRows.Text = "Rejected Worksheet by Admin";
                            RadMessageBox.Show("Rejected Worksheet by Admin.", "Rejected");
                        }

                        // 오더핸들러 전화번호가 등록되어 있는 경우
                        DataRow dr = Dev.Options.Data.CommonData.GetPhoneNumberbyOrderID(Convert.ToInt32(_gv1.Rows[row.Index].Cells["OrderIdx"].Value.ToString()));
                        if (dr != null && !string.IsNullOrEmpty(dr["Phone"].ToString().Trim()))
                        {
                            // 결과 메시지 송신
                            Controller.TelegramMessageSender msgSender = new Controller.TelegramMessageSender();
                            msgSender.sendMessage(dr["Phone"].ToString().Trim(), "[작지반려] " +
                                        "Buyer: " + _gv1.Rows[row.Index].Cells["Buyer"].Value.ToString() + ", " +
                                        "File: " + _gv1.Rows[row.Index].Cells["Fileno"].Value.ToString() + ", " +
                                        "Style: " + _gv1.Rows[row.Index].Cells["Styleno"].Value.ToString() + ", " +
                                        "Worksheet#: " + _gv1.Rows[row.Index].Cells["WorksheetIdx"].Value.ToString() + ", " +
                                        "Rejected Date: " + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm") + ", " +
                                        "Rejected by " + Options.UserInfo.Userfullname.ToString() + "\n" +
                                         "Comment: " + txtCommentTD.Text.ToString()
                                        );
                        }
                    }
                    else if (Options.UserInfo.DeptIdx == 7 && Options.UserInfo.IsLeader != 1)     // 사무실
                    {
                        _bRtn = Data.WorksheetData.RejectOffice(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value),
                        Options.UserInfo.Idx, txtCommentTD.Text.Trim());

                        if (_bRtn)
                        {
                            __main__.lblRows.Text = "Rejected Worksheet by Office";
                            RadMessageBox.Show("Rejected Worksheet by Office.", "Rejected");
                        }

                        // 오더핸들러 전화번호가 등록되어 있는 경우
                        DataRow dr = Dev.Options.Data.CommonData.GetPhoneNumberbyOrderID(Convert.ToInt32(_gv1.Rows[row.Index].Cells["OrderIdx"].Value.ToString()));
                        if (dr != null && !string.IsNullOrEmpty(dr["Phone"].ToString().Trim()))
                        {
                            // 결과 메시지 송신
                            Controller.TelegramMessageSender msgSender = new Controller.TelegramMessageSender();
                            msgSender.sendMessage(dr["Phone"].ToString().Trim(), "[작지반려] " +
                                        "Buyer: " + _gv1.Rows[row.Index].Cells["Buyer"].Value.ToString() + ", " +
                                        "File: " + _gv1.Rows[row.Index].Cells["Fileno"].Value.ToString() + ", " +
                                        "Style: " + _gv1.Rows[row.Index].Cells["Styleno"].Value.ToString() + ", " +
                                        "Worksheet#: " + _gv1.Rows[row.Index].Cells["WorksheetIdx"].Value.ToString() + ", " +
                                        "Rejected Date: " + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm") + ", " +
                                        "Rejected by " + Options.UserInfo.Userfullname.ToString() + "\n" +
                                         "Comment: " + txtCommentTD.Text.ToString()
                                        );
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("btnUpdate_Click: " + ex.Message.ToString());
            }
        }

        private void gvWorksheet_RowFormatting(object sender, RowFormattingEventArgs e)
        {
            try
            { 
                _gv1.EndEdit();
                GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);

                if (Options.UserInfo.DeptIdx == 12)     // TD
                {
                    if (!string.IsNullOrEmpty(e.RowElement.RowInfo.Cells["ConfirmDateTD"].Value.ToString()))
                    {
                        e.RowElement.DrawFill = true;
                        e.RowElement.GradientStyle = GradientStyles.Solid;
                        e.RowElement.BackColor = Color.LightSkyBlue;
                    }
                    else if (!string.IsNullOrEmpty(e.RowElement.RowInfo.Cells["RejectDate"].Value.ToString()))
                    {
                        e.RowElement.DrawFill = true;
                        e.RowElement.GradientStyle = GradientStyles.Solid;
                        e.RowElement.BackColor = Color.LightPink;
                    }
                    else
                    {
                        e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                        e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                        e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                    }
                }
                else if (Options.UserInfo.DeptIdx == 7 && Options.UserInfo.IsLeader == 1)     // 개발 총괄
                {
                    if (!string.IsNullOrEmpty(e.RowElement.RowInfo.Cells["ConfirmDateLast"].Value.ToString()))
                    {
                        e.RowElement.DrawFill = true; 
                        e.RowElement.GradientStyle = GradientStyles.Solid;
                        e.RowElement.BackColor = Color.LightSkyBlue;
                    }
                    else if (!string.IsNullOrEmpty(e.RowElement.RowInfo.Cells["RejectDate"].Value.ToString()))
                    {
                        e.RowElement.DrawFill = true;
                        e.RowElement.GradientStyle = GradientStyles.Solid;
                        e.RowElement.BackColor = Color.LightPink;
                    }
                    else
                    {
                        e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                        e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                        e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                    }
                }
                else if (Options.UserInfo.DeptIdx == 7 && Options.UserInfo.IsLeader != 1)     // 사무실
                {
                    if (!string.IsNullOrEmpty(e.RowElement.RowInfo.Cells["ConfirmDate"].Value.ToString()))
                    {
                        e.RowElement.DrawFill = true;
                        e.RowElement.GradientStyle = GradientStyles.Solid;
                        e.RowElement.BackColor = Color.LightSkyBlue;
                    }
                    else if (!string.IsNullOrEmpty(e.RowElement.RowInfo.Cells["RejectDate"].Value.ToString()))
                    {
                        e.RowElement.DrawFill = true;
                        e.RowElement.GradientStyle = GradientStyles.Solid;
                        e.RowElement.BackColor = Color.LightPink;
                    }
                    else
                    {
                        e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                        e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                        e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                    }
                }
                else      // 영업부
                {
                    if (!string.IsNullOrEmpty(e.RowElement.RowInfo.Cells["ConfirmDateLast"].Value.ToString()))
                    {
                        e.RowElement.DrawFill = true;
                        e.RowElement.GradientStyle = GradientStyles.Solid;
                        e.RowElement.BackColor = Color.LightSkyBlue;
                    }
                    else if (!string.IsNullOrEmpty(e.RowElement.RowInfo.Cells["RejectDate"].Value.ToString()))
                    {
                        e.RowElement.DrawFill = true;
                        e.RowElement.GradientStyle = GradientStyles.Solid;
                        e.RowElement.BackColor = Color.LightPink;
                    }
                    else
                    {
                        e.RowElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                        e.RowElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                        e.RowElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("RowFormatting: " + ex.Message.ToString());
            }
        }

        private void gvWorksheet_HyperlinkOpened(object sender, HyperlinkOpenedEventArgs e)
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
                    CommonController.Close_All_Children(this, "WorksheetMain");
                    OrderMain form = new OrderMain(__main__, e.Hyperlink.ToString());
                    form.Text = "Order Main"; // DateTime.Now.ToLongTimeString();
                    //form.MdiParent = this; 
                    form.WindowState = FormWindowState.Normal;
                    form.Show();
                    
                }
            }
            catch(Exception ex)
            {
                RadMessageBox.Show(ex.Message); 
            }
        }

        private void gvWorksheet_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.Cancel = true; 
        }

        private void btnSaveFabricComment_Click(object sender, EventArgs e)
        {
            try
            {
                _gv1.EndEdit();
                GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);

                if (Options.UserInfo.Idx == 18)     // Sample Fabric Bryan
                {
                    _bRtn = Data.WorksheetData.SaveFabricComments(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value),
                                                                txtFabricComment.Text.Trim());

                    if (_bRtn)
                    {
                        __main__.lblRows.Text = "Guardado Comentario";
                        RadMessageBox.Show("Guardado Comentario", "Guardar");
                    }
                }

                // 오더핸들러 전화번호가 등록되어 있는 경우
                DataRow dr = Dev.Options.Data.CommonData.GetPhoneNumberbyOrderID(Convert.ToInt32(_gv1.Rows[row.Index].Cells["OrderIdx"].Value.ToString()));
                if (dr != null && !string.IsNullOrEmpty(dr["Phone"].ToString().Trim()))
                {
                    // 결과 메시지 송신
                    Controller.TelegramMessageSender msgSender = new Controller.TelegramMessageSender();
                    msgSender.sendMessage(dr["Phone"].ToString().Trim(), "[원단담당자 의견] " +
                                "Buyer: " + _gv1.Rows[row.Index].Cells["Buyer"].Value.ToString() + ", " +
                                "File: " + _gv1.Rows[row.Index].Cells["Fileno"].Value.ToString() + ", " +
                                "Style: " + _gv1.Rows[row.Index].Cells["Styleno"].Value.ToString() + ", " +
                                "Worksheet#: " + _gv1.Rows[row.Index].Cells["WorksheetIdx"].Value.ToString() + ", " +
                                "Date: " + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm") + ", " +
                                "Commented by " + Options.UserInfo.Userfullname.ToString() + "\n" +
                                 "Comment: " + txtFabricComment.Text.ToString()
                                );
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnSaveFabricComment_Click: " + ex.Message.ToString());
            }
        }

        private void txtFileno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        private void txtStyle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        private void txtWorksheet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }
    }
}
