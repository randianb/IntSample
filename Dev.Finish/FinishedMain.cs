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
using System.IO;
using System.Globalization;

namespace Dev.Out
{
    /// <summary>
    /// 나염 외주오더현황 
    /// </summary>
    public partial class FinishedMain : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                             // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, string> _searchString;  // 쿼리값 value, 쿼리항목 key로 전달 
        public Dictionary<CommonValues.KeyName, int> _searchKey;        // 쿼리값 value, 쿼리항목 key로 전달 
        public DataRow InsertedOrderRow = null;
        
        private bool _bRtn;                                                     // 쿼리결과 리턴
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        private DataTable _dt = null;                                           // 기본 데이터테이블
        private Controller.OutFinished _obj1 = null;                                // 현재 생성된 객체 
        private RadContextMenu contextMenu;                                     // 컨텍스트 메뉴
        private List<CodeContents> lstWorkStatus = new List<CodeContents>();        // 오더상태
        private List<CustomerName> custName = new List<CustomerName>();         // 거래처
        private List<CodeContents> sizeName = new List<CodeContents>();

        private List<CustomerName> lstOut = new List<CustomerName>();        // 
        private List<CustomerName> lstOut2 = new List<CustomerName>();        // 
        private List<CustomerName> lstReceived = new List<CustomerName>();        // 
        private List<CustomerName> lstReceived2 = new List<CustomerName>();        // 

        private string _layoutfile = "/GVLayoutOutFinished.xml";
        private string _workOrderIdx;

        //RadMenuItem mnuNew, mnuDel, mnuHide, mnuShow, menuItem2, menuItem3, menuItem4, mnuWorksheet, mnuCutting, mnuOutsourcing, mnuSewing, mnuInspecting = null;
        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 46, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public FinishedMain(InheritMDI main, string WorkOrderIdx)
        {
            base.InitializeComponent(); // parent 컴포넌트에 접근하기 위해 컨트롤을 public으로 지정 > 향후 수정 필요 (todo) 
            InitializeComponent();
            __main__ = main;            // MDI 연결 
            _gv1 = this.gvMain;  // 그리드뷰 일반화를 위해 변수 별도 저장
            _workOrderIdx = WorkOrderIdx;
            //dtOutFrom.Value = Convert.ToDateTime(DateTime.Now.Year + "-01-01");
            //dtRcvdFrom.Value = Convert.ToDateTime(DateTime.Now.Year + "-01-01");
            //dtOutTo.Value = Convert.ToDateTime(DateTime.Now.Year + "-12-31");
            //dtRcvdTo.Value = Convert.ToDateTime(DateTime.Now.Year + "-12-31");
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

                _searchKey.Add(CommonValues.KeyName.BuyerIdx, 0);
                _searchKey.Add(CommonValues.KeyName.Size, 0);
                _searchKey.Add(CommonValues.KeyName.CustIdx, 0);
                _searchKey.Add(CommonValues.KeyName.User, 0);

                _searchString.Add(CommonValues.KeyName.OrderIdx, "");
                _searchString.Add(CommonValues.KeyName.Styleno, "");
                _searchString.Add(CommonValues.KeyName.WorkOrderIdx, _workOrderIdx);
                _searchString.Add(CommonValues.KeyName.ColorIdx, "");
                _searchString.Add(CommonValues.KeyName.Remark, "");

                _searchString.Add(CommonValues.KeyName.StartDate, "2000-01-01");

                DataBinding_GV1(_searchKey, _searchString, "2000-01-01", "2900-01-01", "2000-01-01", "2900-01-01");
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
            
            // 사이즈 
            ddlSize.DataSource = sizeName;
            ddlSize.DisplayMember = "Contents";
            ddlSize.ValueMember = "CodeIdx";
            ddlSize.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlSize.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
                        
            // 출고처  
            ddlOut.DataSource = lstOut2;
            ddlOut.DisplayMember = "CustName";
            ddlOut.ValueMember = "CustIdx";
            ddlOut.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlOut.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // Received
            ddlReceived.DataSource = lstReceived2;
            ddlReceived.DisplayMember = "CustName";
            ddlReceived.ValueMember = "CustIdx";
            ddlReceived.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlReceived.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlReceived.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

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

            GridViewTextBoxColumn Idx = new GridViewTextBoxColumn();
            Idx.DataType = typeof(int);
            Idx.Name = "Idx";
            Idx.FieldName = "Idx";
            Idx.HeaderText = "ID";
            Idx.Width = 40;
            Idx.ReadOnly = true; 
            Idx.TextAlignment = ContentAlignment.MiddleLeft;
            gv.Columns.Add(Idx);
            
            GridViewTextBoxColumn WorkOrderIdx = new GridViewTextBoxColumn();
            WorkOrderIdx.Name = "WorkOrderIdx";
            WorkOrderIdx.FieldName = "WorkOrderIdx";
            WorkOrderIdx.HeaderText = "Work ID (Out)";
            WorkOrderIdx.Width = 100;
            WorkOrderIdx.ReadOnly = true;
            WorkOrderIdx.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(WorkOrderIdx);

            GridViewComboBoxColumn Out1 = new GridViewComboBoxColumn();
            Out1.Name = "Out1";
            Out1.DataSource = lstOut;
            Out1.DisplayMember = "CustName";
            Out1.ValueMember = "CustIdx";
            Out1.FieldName = "Out1";
            Out1.HeaderText = "Out";
            Out1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Out1.DropDownStyle = RadDropDownStyle.DropDown;
            Out1.Width = 130;
            gv.Columns.Add(Out1);

            GridViewTextBoxColumn Out2 = new GridViewTextBoxColumn();
            Out2.Name = "Out2";
            Out2.FieldName = "Out2";
            Out2.IsVisible = false;
            Out2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(Out2);
            
            GridViewTextBoxColumn Delivered = new GridViewTextBoxColumn();
            Delivered.Name = "Delivered";
            Delivered.FieldName = "Delivered";
            Delivered.HeaderText = "Delivered";
            Delivered.Width = 130;
            Delivered.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(Delivered);
            
            GridViewDateTimeColumn OutDate = new GridViewDateTimeColumn();
            OutDate.Name = "OutDate";
            OutDate.FieldName = "OutDate";
            OutDate.Width = 90;
            OutDate.TextAlignment = ContentAlignment.MiddleCenter;
            OutDate.FormatInfo = new System.Globalization.CultureInfo("ko-KR");
            OutDate.FormatString = "{0:d}";
            OutDate.HeaderText = "Out Date"; 
            gv.Columns.Add(OutDate);
            
            GridViewCommandColumn Confirm = new GridViewCommandColumn();
            Confirm.Name = "Confirm";
            Confirm.FieldName = "Confirm";
            Confirm.UseDefaultText = true;
            Confirm.DefaultText = "Receive";
            Confirm.HeaderText = "Recevie";
            Confirm.TextAlignment = ContentAlignment.MiddleCenter;
            Confirm.Width = 60;
            gv.Columns.Add(Confirm);

            GridViewComboBoxColumn Received1 = new GridViewComboBoxColumn();
            Received1.Name = "Received1";
            Received1.DataSource = lstReceived;
            Received1.DisplayMember = "CustName";
            Received1.ValueMember = "CustIdx";
            Received1.FieldName = "Received1";
            Received1.HeaderText = "Received";
            Received1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Received1.DropDownStyle = RadDropDownStyle.DropDown;
            Received1.Width = 130;
            gv.Columns.Add(Received1);

            GridViewTextBoxColumn Received2 = new GridViewTextBoxColumn();
            Received2.Name = "Received2";
            Received2.FieldName = "Received2";
            Received2.IsVisible = false;
            Received2.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(Received2);

            GridViewDateTimeColumn RcvdDate = new GridViewDateTimeColumn();
            RcvdDate.Name = "RcvdDate";
            RcvdDate.FieldName = "RcvdDate";
            RcvdDate.Width = 90;
            RcvdDate.FormatInfo = new System.Globalization.CultureInfo("ko-KR");
            RcvdDate.TextAlignment = ContentAlignment.MiddleCenter;
            RcvdDate.FormatString = "{0:d}";
            RcvdDate.HeaderText = "Rcvd Date";
            gv.Columns.Add(RcvdDate);

            GridViewTextBoxColumn Status = new GridViewTextBoxColumn();
            Status.Name = "Status";
            Status.FieldName = "Status";
            Status.HeaderText = "Status";
            Status.Width = 80;
            Status.ReadOnly = true; 
            gv.Columns.Add(Status);

            GridViewCommandColumn viewInv = new GridViewCommandColumn();
            viewInv.Name = "viewInv";
            viewInv.FieldName = "viewInv";
            viewInv.Width = 60;
            viewInv.DefaultText = "Print";
            viewInv.UseDefaultText = true;
            viewInv.TextAlignment = ContentAlignment.MiddleCenter;
            viewInv.HeaderText = "Invoice";
            gv.Columns.Add(viewInv);

            GridViewTextBoxColumn Remarks = new GridViewTextBoxColumn();
            Remarks.Name = "Remarks";
            Remarks.FieldName = "Remarks";
            Remarks.HeaderText = "Remarks";
            Remarks.Width = 220;
            gv.Columns.Add(Remarks);

            #endregion
        }

        /// <summary>
        /// 그리드뷰 컬럼 생성 (상세)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV2_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            gv.MasterTemplate.Columns.Clear();
            gv.DataSource = null;

            GridViewTextBoxColumn Idx = new GridViewTextBoxColumn();
            Idx.Name = "Idx";
            Idx.FieldName = "Idx";
            Idx.Width = 60;
            Idx.TextAlignment = ContentAlignment.MiddleLeft;
            Idx.HeaderText = "ID";
            Idx.ReadOnly = true;
            gv.Columns.Add(Idx);

            GridViewTextBoxColumn pIdx = new GridViewTextBoxColumn();
            pIdx.Name = "pIdx";
            pIdx.FieldName = "pIdx";
            pIdx.Width = 60;
            pIdx.TextAlignment = ContentAlignment.MiddleLeft;
            pIdx.HeaderText = "PID";
            pIdx.ReadOnly = true;
            gv.Columns.Add(pIdx);

            GridViewTextBoxColumn OrderIdx = new GridViewTextBoxColumn();
            OrderIdx.Name = "OrderIdx";
            OrderIdx.FieldName = "OrderIdx";
            OrderIdx.Width = 60;
            OrderIdx.TextAlignment = ContentAlignment.MiddleLeft;
            OrderIdx.HeaderText = "OID";
            OrderIdx.ReadOnly = true;
            gv.Columns.Add(OrderIdx);

            GridViewTextBoxColumn DeptIdx = new GridViewTextBoxColumn();
            DeptIdx.Name = "DeptIdx";
            DeptIdx.FieldName = "DeptIdx";
            DeptIdx.Width = 70;
            DeptIdx.TextAlignment = ContentAlignment.MiddleLeft;
            DeptIdx.HeaderText = "Dept.";
            DeptIdx.ReadOnly = true;
            gv.Columns.Add(DeptIdx);

            GridViewTextBoxColumn Buyer = new GridViewTextBoxColumn();
            Buyer.Name = "Buyer";
            Buyer.FieldName = "Buyer";
            Buyer.Width = 150;
            Buyer.TextAlignment = ContentAlignment.MiddleLeft;
            Buyer.HeaderText = "Buyer";
            Buyer.ReadOnly = true;
            gv.Columns.Add(Buyer);

            GridViewTextBoxColumn Fileno = new GridViewTextBoxColumn();
            Fileno.Name = "Fileno";
            Fileno.FieldName = "Fileno";
            Fileno.Width = 100;
            Fileno.TextAlignment = ContentAlignment.MiddleLeft;
            Fileno.HeaderText = "File#";
            Fileno.ReadOnly = true;
            gv.Columns.Add(Fileno);

            GridViewTextBoxColumn Styleno = new GridViewTextBoxColumn();
            Styleno.Name = "Styleno";
            Styleno.FieldName = "Styleno";
            Styleno.Width = 120;
            Styleno.TextAlignment = ContentAlignment.MiddleLeft;
            Styleno.HeaderText = "Style#";
            Styleno.ReadOnly = true;
            gv.Columns.Add(Styleno);

            GridViewTextBoxColumn Handler = new GridViewTextBoxColumn();
            Handler.Name = "Handler";
            Handler.FieldName = "Handler";
            Handler.Width = 120;
            Handler.TextAlignment = ContentAlignment.MiddleLeft;
            Handler.HeaderText = "Handler";
            Handler.ReadOnly = true;
            gv.Columns.Add(Handler);

            GridViewTextBoxColumn WorkOrderIdx = new GridViewTextBoxColumn();
            WorkOrderIdx.Name = "WorkOrderIdx";
            WorkOrderIdx.FieldName = "WorkOrderIdx";
            WorkOrderIdx.Width = 110;
            WorkOrderIdx.TextAlignment = ContentAlignment.MiddleLeft;
            WorkOrderIdx.HeaderText = "WorkOrder#";
            WorkOrderIdx.ReadOnly = true;
            gv.Columns.Add(WorkOrderIdx);


            GridViewTextBoxColumn OrdColorIdx = new GridViewTextBoxColumn();
            OrdColorIdx.Name = "OrdColorIdx";
            OrdColorIdx.FieldName = "OrdColorIdx";
            OrdColorIdx.Width = 200;
            OrdColorIdx.TextAlignment = ContentAlignment.MiddleLeft;
            OrdColorIdx.HeaderText = "Color";
            OrdColorIdx.ReadOnly = true;
            gv.Columns.Add(OrdColorIdx);

            GridViewTextBoxColumn OrdSizeIdx = new GridViewTextBoxColumn();
            OrdSizeIdx.Name = "SizeNm";
            OrdSizeIdx.FieldName = "SizeNm";
            OrdSizeIdx.Width = 90;
            OrdSizeIdx.TextAlignment = ContentAlignment.MiddleCenter;
            OrdSizeIdx.HeaderText = "Size";
            OrdSizeIdx.ReadOnly = true; 
            gv.Columns.Add(OrdSizeIdx);
            
            GridViewTextBoxColumn OutQty = new GridViewTextBoxColumn();
            OutQty.Name = "OutQty";
            OutQty.FieldName = "OutQty";
            OutQty.Width = 70;
            OutQty.TextAlignment = ContentAlignment.MiddleRight;
            OutQty.HeaderText = "Out Q'ty";
            gv.Columns.Add(OutQty);

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
            //Font f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            //ConditionalFormattingObject obj = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "4", "", true);
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
            
            //f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            //ConditionalFormattingObject obj4 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "0", "", true);
            //obj4.RowForeColor = Color.Black;
            //obj4.RowFont = f;
            //gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj4);

            //f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            //ConditionalFormattingObject obj5 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "2", "", true);
            //obj5.RowForeColor = Color.Black;
            //obj5.RowBackColor = Color.Lavender; 
            //obj5.RowFont = f;
            //gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj5);

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


            // Sewing Vendor 
            _dt = CommonController.Getlist(CommonValues.KeyName.Vendor).Tables[0];

            lstOut.Add(new CustomerName(0, "", 0));
            lstOut2.Add(new CustomerName(0, "", 0));

            foreach (DataRow row in _dt.Rows)
            {
                lstOut.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
                lstOut2.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
            }

            // Username
            lstReceived.Add(new CustomerName(0, "", 0));
            lstReceived2.Add(new CustomerName(0, "", 0));

            _dt = CommonController.Getlist(CommonValues.KeyName.AllUser).Tables[0];
            
            foreach (DataRow row in _dt.Rows)
            {
                lstReceived.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]),
                                            row["UserName"].ToString(),
                                            Convert.ToInt32(row["DeptIdx"])));
                lstReceived2.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]),
                                            row["UserName"].ToString(),
                                            Convert.ToInt32(row["DeptIdx"])));
            }

            // 작업진행상태
            lstWorkStatus.Add(new CodeContents(0, "", ""));
            lstWorkStatus.Add(new CodeContents(1, "New Work", ""));
            lstWorkStatus.Add(new CodeContents(2, "Printed Ticket", ""));
            lstWorkStatus.Add(new CodeContents(3, "Completed", ""));
            lstWorkStatus.Add(new CodeContents(4, "Canceled", ""));
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
                //    || ddlOut.SelectedValue != null || ddlFabric.SelectedValue != null
                //    || !string.IsNullOrEmpty(txtFileno.Text) || !string.IsNullOrEmpty(txtStyle.Text) 
                //    || !string.IsNullOrEmpty(txtColor.Text) || !string.IsNullOrEmpty(dtOutFrom.Text))
                //{
                    _searchKey = new Dictionary<CommonValues.KeyName, int>();
                    _searchString = new Dictionary<CommonValues.KeyName, string>();
                                    
                    _searchKey.Add(CommonValues.KeyName.BuyerIdx, Convert.ToInt32(ddlCust.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Size, Convert.ToInt32(ddlSize.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.CustIdx, Convert.ToInt32(ddlOut.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.User, Convert.ToInt32(ddlReceived.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.WorkStatus, Convert.ToInt32(ddlWorkStatus.SelectedValue));

                    _searchString.Add(CommonValues.KeyName.OrderIdx, txtFileno.Text.ToString().Trim());
                    _searchString.Add(CommonValues.KeyName.Styleno, txtStyle.Text.ToString().Trim());
                    _searchString.Add(CommonValues.KeyName.WorkOrderIdx, txtWorkorder.Text.ToString().Trim());
                    _searchString.Add(CommonValues.KeyName.ColorIdx, txtColor.Text.ToString().Trim());
                    _searchString.Add(CommonValues.KeyName.Remark, txtDelivered.Text.ToString().Trim());
                
                    CultureInfo ci = new CultureInfo("ko-KR");
                    DataBinding_GV1(_searchKey, _searchString,
                                    dtOutFrom.Value.ToString("d", ci).Substring(0, 10),
                                    dtOutTo.Value.ToString("d", ci).Substring(0, 10),
                                    dtRcvdFrom.Value.ToString("d", ci).Substring(0, 10),
                                    dtRcvdTo.Value.ToString("d", ci).Substring(0, 10));
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
        private void DataBinding_GV1(Dictionary<CommonValues.KeyName, int> SearchKey,
                                    Dictionary<CommonValues.KeyName, string> SearchString,
                                    string OutDateFrom, string OutDateTo,
                                      string RcvdDateFrom, string RcvdDateTo)
        {
            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
                int _mode_ = 0;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    _gv1.DataSource = null;

                    _ds1 = Controller.OutFinished.Getlist(SearchKey, SearchString, OutDateFrom, OutDateTo,
                                       RcvdDateFrom, RcvdDateTo);
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

        private void DataBinding_GV2(int pIdx) 
        {
            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
                int _mode_ = 0;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    gvDetail.DataSource = null;

                    _ds1 = Controller.OutFinishedD.Getlist(pIdx);
                    if (_ds1 != null)
                    {
                        gvDetail.DataSource = _ds1.Tables[0].DefaultView;
                        //__main__.lblRows.Text = gvDetail.RowCount.ToString() + " Rows";
                        //gvDetail.EnablePaging = CommonValues.enablePaging;
                        //gvDetail.AllowSearchRow = CommonValues.enableSearchRow;
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
        /// 데이터 업데이트 (메인)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GV1_Update(object sender, GridViewCellEventArgs e)
        {
            _bRtn = false;
            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
                int _mode_ = 1;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0 && 
                    Convert.ToInt16(__AUTHCODE__.Substring(4, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    //_gv1.EndEdit();
                    GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);  // 현재 행번호 확인

                    // 객체생성 및 값 할당
                    _obj1 = new Controller.OutFinished(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value));
                    _obj1.Idx = Convert.ToInt32(row.Cells["Idx"].Value.ToString());

                    if (row.Cells["WorkOrderIdx"].Value != null && row.Cells["WorkOrderIdx"].Value != DBNull.Value)
                        _obj1.WorkOrderIdx = row.Cells["WorkOrderIdx"].Value.ToString();
                    else _obj1.WorkOrderIdx = "";
                    if (row.Cells["Out1"].Value != DBNull.Value) _obj1.Out1 = Convert.ToInt32(row.Cells["Out1"].Value.ToString()); else _obj1.Out1 = 0;
                    if (row.Cells["Out2"].Value != DBNull.Value) _obj1.Out2 = Convert.ToInt32(row.Cells["Out2"].Value.ToString()); else _obj1.Out2 = 0;
                    if (row.Cells["Delivered"].Value != DBNull.Value) _obj1.Delivered = row.Cells["Delivered"].Value.ToString(); else _obj1.Delivered = "";

                    if (row.Cells["Received1"].Value != DBNull.Value) _obj1.Received1 = Convert.ToInt32(row.Cells["Received1"].Value.ToString()); else _obj1.Received1 = 0;
                    if (row.Cells["Received2"].Value != DBNull.Value) _obj1.Received2 = Convert.ToInt32(row.Cells["Received2"].Value.ToString()); else _obj1.Received2 = 0;

                    if (row.Cells["OutDate"].Value != DBNull.Value && row.Cells["OutDate"] != null)
                    {
                        _obj1.OutDate = Convert.ToDateTime(row.Cells["OutDate"].Value);
                    }
                    else
                    {
                        _obj1.OutDate = new DateTime(2000, 1, 1);
                    }
                    if (row.Cells["RcvdDate"].Value != DBNull.Value && row.Cells["RcvdDate"] != null)
                    {
                        _obj1.RcvdDate = Convert.ToDateTime(row.Cells["RcvdDate"].Value);
                    }
                    else
                    {
                        _obj1.RcvdDate = new DateTime(2000, 1, 1);
                    }
                    if (row.Cells["Remarks"].Value != DBNull.Value) _obj1.Remarks = row.Cells["Remarks"].Value.ToString(); else _obj1.Remarks = "";


                    // 업데이트 (오더캔슬, 선적완료 상태가 아닐경우)
                    _bRtn = _obj1.Update();
                    __main__.lblRows.Text = "Updated Out Info";
                }
                

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
                    
                    el.CalendarSize = new Size(500, 400);

                    //if (el.Value.ToString().Length > 10)
                    //{
                    //    Console.WriteLine(el.Value.ToString().Substring(0, 10));
                    //    if (el.Value.ToString().Substring(0, 10) == "2000-01-01")
                    //    {
                    //        el.Value = Convert.ToDateTime(null);
                    //    }
                    //}

                    el.NullDate = new DateTime(2000, 1, 1);
                    el.NullText = "2000-01-01";
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
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
                //lstSize.Clear(); // 기존 저장된 사이즈 초기화 
                //GetSizes(gvOrder); // 하단 Color Size 데이터용 Size 정보 업데이트 

                // 오더별 사이즈 그룹이 다르므로 컬러사이즈 제목 갱신후 자료갱신
                GV2_CreateColumn(gvDetail);
                DataBinding_GV2(Int.Members.GetCurrentRow(gvMain, "Idx"));

                CommonValues.ListWorkID.Clear();
                CommonValues.WorkOperation = "Shipment";

                // 선택된 모든 행에 대해 
                foreach (GridViewRowInfo row in gvMain.SelectedRows)
                {
                    CommonValues.ListWorkID.Add(row.Cells["WorkOrderIdx"].Value.ToString().Trim());
                }
            }
            catch(Exception ex)
            {

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

        private void radLabel8_DoubleClick(object sender, EventArgs e)
        {
            dtOutFrom.Value = Convert.ToDateTime("2000-01-01");
        }

        private void gvMain_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.Cancel = true; 
        }

        private void gvMain_Click(object sender, EventArgs e)
        {

        }

        private void btnOutFinishedAdd_Click(object sender, EventArgs e)
        {
            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
                int _mode_ = 1;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0 && Convert.ToInt16(__AUTHCODE__.Substring(4, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    string NewCode = Code.GetPrimaryCode(UserInfo.CenterIdx, UserInfo.DeptIdx, 13, "");    // create finished code 
                    
                    if (!string.IsNullOrEmpty(NewCode))
                    {
                        DataRow row = Dev.Controller.OutFinished.Insert(NewCode);

                        RefleshWithCondition();
                        // SetCurrentRow(_gv1, Convert.ToInt32(row["LastIdx"]));   // 신규입력된 행번호로 이동


                        // 데이터 DB저장 
                        if (row != null)
                        {
                            // 입력완료 후 그리드뷰 갱신
                            DialogResult = System.Windows.Forms.DialogResult.OK;
                        }
                        else
                        {
                            //lblResult.Text = "Failed to input the data.";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }

        private void radLabel12_DoubleClick(object sender, EventArgs e)
        {
            dtOutTo.Value = Convert.ToDateTime("2900-01-01");
        }

        private void radLabel7_DoubleClick(object sender, EventArgs e)
        {
            dtRcvdFrom.Value = Convert.ToDateTime("2000-01-01");
        }

        private void radLabel13_DoubleClick(object sender, EventArgs e)
        {
            dtRcvdTo.Value = Convert.ToDateTime("2900-01-01");
        }

        private void dtOutTo_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtRcvdTo_ValueChanged(object sender, EventArgs e)
        {

        }

        private void btnOutFinishedDAdd_Click(object sender, EventArgs e)
        {
            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                int _mode_ = 1;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0 &&
                    Convert.ToInt16(__AUTHCODE__.Substring(4, 1).Trim()) <= 0)
                {
                    CheckAuth.ShowMessage(_mode_);
                } 
                else
                {
                    // 워크시트 열기 
                    frmOutRequest frm = new frmOutRequest(Int.Members.GetCurrentRow(_gv1, "Idx"), 
                                                          Int.Members.GetCurrentRow(_gv1, "WorkOrderIdx", "worder")
                                                    );
                    frm.Text = "Out WorkOrder";

                    if (frm.ShowDialog(this) == DialogResult.OK)
                    {
                        // Production Status 갱신 
                        DataBinding_GV2(Int.Members.GetCurrentRow(_gv1, "Idx"));
                    }
                    
                }

            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteDetail_Click(object sender, EventArgs e)
        {
            try
            {
                string str = "";
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                int _mode_ = 2;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0 &&
                    Convert.ToInt16(__AUTHCODE__.Substring(4, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    // 삭제하기전 삭제될 데이터 확인
                    foreach (GridViewRowInfo row in gvDetail.SelectedRows)
                    {
                        if (string.IsNullOrEmpty(str)) str = Convert.ToInt32(row.Cells["Idx"].Value).ToString();
                        else str += "," + Convert.ToInt32(row.Cells["Idx"].Value).ToString();
                    }

                    // 해당 자료 삭제여부 확인후, 
                    if (RadMessageBox.Show("Do you want to delete this item?\n(ID: " + str + ")", "Confirm",
                        MessageBoxButtons.YesNo, RadMessageIcon.Question) == DialogResult.Yes)
                    {
                        // 삭제후 새로고침
                        _bRtn = Controller.OutFinishedD.Delete(str);
                        DataBinding_GV2(Int.Members.GetCurrentRow(_gv1, "Idx"));
                    }
                    
                }

            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }

        private void gvMain_CommandCellClick(object sender, GridViewCellEventArgs e)
        {
            if (e.Column.Name=="Confirm")
            {
                if (RadMessageBox.Show("Did you received it?", "Confirm", MessageBoxButtons.YesNo, RadMessageIcon.Question)
                == DialogResult.Yes)
                {
                    e.Row.Cells["Received1"].Value = UserInfo.Idx;
                    e.Row.Cells["RcvdDate"].Value = DateTime.Now;

                }
            }
            else if (e.Column.Name == "viewInv")
            {
                PrintInvoice pInv = new PrintInvoice(e.Row.Cells["WorkOrderIdx"].Value.ToString().Trim());
                pInv.Show(); 
            }
        }
    }
}
