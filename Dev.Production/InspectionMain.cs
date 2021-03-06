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
using Telerik.WinForms.Documents.Model;

namespace Dev.Production
{
    /// <summary>
    /// 나염 외주오더현황 
    /// </summary>
    public partial class InspectionMain : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                             // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, string> _searchString;  // 쿼리값 value, 쿼리항목 key로 전달 
        public Dictionary<CommonValues.KeyName, int> _searchKey;        // 쿼리값 value, 쿼리항목 key로 전달 
        public DataRow InsertedOrderRow = null;
        
        private bool _bRtn;                                                     // 쿼리결과 리턴
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        private DataTable _dt = null;                                           // 기본 데이터테이블
        private Controller.Inspecting _obj1 = null;                                // 현재 생성된 객체 
        private RadContextMenu contextMenu;                                     // 컨텍스트 메뉴
        private List<CodeContents> lstStatus = new List<CodeContents>();        // 오더상태
        private List<CodeContents> lstStatus2 = new List<CodeContents>();        // 오더상태
        private List<CustomerName> custName = new List<CustomerName>();         // 거래처
        private List<CodeContents> sizeName = new List<CodeContents>();

        private List<CustomerName> lstUser = new List<CustomerName>();         // TD

        private string _layoutfile = "/GVLayoutInspection.xml";
        private string _workOrderIdx;

        //RadMenuItem mnuNew, mnuDel, mnuHide, mnuShow, menuItem2, menuItem3, menuItem4, mnuWorksheet, mnuCutting, mnuOutsourcing, mnuSewing, mnuInspecting = null;
        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 43, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public InspectionMain(InheritMDI main, string WorkOrderIdx)
        {
            base.InitializeComponent(); // parent 컴포넌트에 접근하기 위해 컨트롤을 public으로 지정 > 향후 수정 필요 (todo) 
            InitializeComponent();
            __main__ = main;            // MDI 연결 
            _gv1 = this.gvMain;  // 그리드뷰 일반화를 위해 변수 별도 저장
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

                _searchKey.Add(CommonValues.KeyName.BuyerIdx, 0);
                _searchKey.Add(CommonValues.KeyName.Status, 0);
                _searchKey.Add(CommonValues.KeyName.Size, 0);
                _searchKey.Add(CommonValues.KeyName.User, 0);
                _searchKey.Add(CommonValues.KeyName.WorkStatus, 0);

                _searchString.Add(CommonValues.KeyName.OrderIdx, "");
                _searchString.Add(CommonValues.KeyName.Styleno, "");
                _searchString.Add(CommonValues.KeyName.WorkOrderIdx, _workOrderIdx);
                _searchString.Add(CommonValues.KeyName.ColorIdx, "");
                _searchString.Add(CommonValues.KeyName.RequestDate, "2000-01-01");
                _searchString.Add(CommonValues.KeyName.CompleteDate, "2000-01-01");

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
            
            // 사이즈 
            ddlSize.DataSource = sizeName;
            ddlSize.DisplayMember = "Contents";
            ddlSize.ValueMember = "CodeIdx";
            ddlSize.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlSize.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
                        
            // 오더 상태 
            ddlStatus.DataSource = lstStatus2;
            ddlStatus.DisplayMember = "Contents";
            ddlStatus.ValueMember = "CodeIdx";
            ddlStatus.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlStatus.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            //ddlStatus2.DataSource = lstStatus;
            //ddlStatus2.DisplayMember = "Contents";
            //ddlStatus2.ValueMember = "CodeIdx";
            //ddlStatus2.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            //ddlStatus2.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // TD
            ddlFabric.DataSource = lstUser;
            ddlFabric.DisplayMember = "CustName";
            ddlFabric.ValueMember = "CustIdx";
            ddlFabric.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlFabric.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlFabric.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
            
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

            GridViewTextBoxColumn orderidx = new GridViewTextBoxColumn();
            orderidx.Name = "OrderIdx";
            orderidx.FieldName = "OrderIdx";
            orderidx.IsVisible = false;
            gv.Columns.Add(orderidx);
                                    
            GridViewComboBoxColumn cboBuyer = new GridViewComboBoxColumn();
            cboBuyer.Name = "Buyer";
            cboBuyer.FieldName = "Buyer";
            cboBuyer.HeaderText = "Buyer";
            cboBuyer.Width = 140;
            cboBuyer.ReadOnly = true;
            gv.Columns.Add(cboBuyer);

            GridViewComboBoxColumn Styleno = new GridViewComboBoxColumn();
            Styleno.Name = "Styleno";
            Styleno.FieldName = "Styleno";
            Styleno.HeaderText = "Style#";
            Styleno.Width = 80;
            Styleno.ReadOnly = true;
            gv.Columns.Add(Styleno);

            GridViewComboBoxColumn Fileno = new GridViewComboBoxColumn();
            Fileno.Name = "Fileno";
            Fileno.FieldName = "Fileno";
            Fileno.HeaderText = "File#";
            Fileno.Width = 80;
            Fileno.ReadOnly = true;
            gv.Columns.Add(Fileno);
            
            GridViewTextBoxColumn OrdColorIdx = new GridViewTextBoxColumn();
            OrdColorIdx.Name = "OrdColorIdx";
            OrdColorIdx.FieldName = "OrdColorIdx";
            OrdColorIdx.HeaderText = "Color";
            OrdColorIdx.Width = 150;
            OrdColorIdx.ReadOnly = true;
            OrdColorIdx.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(OrdColorIdx);

            GridViewComboBoxColumn cboSize = new GridViewComboBoxColumn();
            cboSize.Name = "OrdSizeIdx";
            cboSize.FieldName = "OrdSizeIdx";
            cboSize.HeaderText = "Size";
            cboSize.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            cboSize.ReadOnly = true;
            cboSize.Width = 70;
            gv.Columns.Add(cboSize);

            GridViewTextBoxColumn WorkQty = new GridViewTextBoxColumn();
            WorkQty.Name = "WorkQty";
            WorkQty.FieldName = "WorkQty";
            WorkQty.HeaderText = "Q'ty";
            WorkQty.Width = 50;
            WorkQty.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            WorkQty.ReadOnly = true;
            gv.Columns.Add(WorkQty);

            GridViewTextBoxColumn TDName = new GridViewTextBoxColumn();
            TDName.Name = "TDName";
            TDName.FieldName = "TDName";
            TDName.HeaderText = "Allocated T/D";
            TDName.Width = 130;
            TDName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            TDName.ReadOnly = true;
            gv.Columns.Add(TDName);

            GridViewDateTimeColumn InspRequestedDate = new GridViewDateTimeColumn();
            InspRequestedDate.Name = "InspRequestedDate";
            InspRequestedDate.FieldName = "InspRequestedDate";
            InspRequestedDate.Width = 90;
            InspRequestedDate.TextAlignment = ContentAlignment.MiddleCenter;
            InspRequestedDate.FormatInfo = new System.Globalization.CultureInfo("ko-KR");
            InspRequestedDate.FormatString = "{0:d}";
            InspRequestedDate.HeaderText = "Requested";
            InspRequestedDate.ReadOnly = true;
            gv.Columns.Add(InspRequestedDate);
            
            GridViewDateTimeColumn InspDate = new GridViewDateTimeColumn();
            InspDate.Name = "InspDate";
            InspDate.FieldName = "InspDate";
            InspDate.Width = 100;
            InspDate.TextAlignment = ContentAlignment.MiddleCenter;
            InspDate.FormatInfo = new System.Globalization.CultureInfo("ko-KR");
            InspDate.CustomFormat = "{d}";
            InspDate.FormatString = "{0:d}";
            InspDate.HeaderText = "Started(Confirm)";
            InspDate.IsVisible = false; 
            gv.Columns.Add(InspDate);

            GridViewDateTimeColumn InspCompletedDate = new GridViewDateTimeColumn();
            InspCompletedDate.Name = "InspCompletedDate";
            InspCompletedDate.FieldName = "InspCompletedDate";
            InspCompletedDate.Width = 100;
            InspCompletedDate.TextAlignment = ContentAlignment.MiddleCenter;
            InspCompletedDate.FormatInfo = new System.Globalization.CultureInfo("ko-KR");
            InspCompletedDate.CustomFormat = "{d}";
            InspCompletedDate.FormatString = "{0:d}";
            InspCompletedDate.HeaderText = "Confirmed Date";
            InspCompletedDate.ReadOnly = true;
            gv.Columns.Add(InspCompletedDate);

            GridViewTextBoxColumn Confirmed = new GridViewTextBoxColumn();
            Confirmed.Name = "Confirmed";
            Confirmed.FieldName = "Confirmed";
            Confirmed.ReadOnly = true;
            Confirmed.Width = 130;
            Confirmed.TextAlignment = ContentAlignment.MiddleLeft;
            Confirmed.HeaderText = "Confirmed";
            gv.Columns.Add(Confirmed);
            
            GridViewComboBoxColumn status = new GridViewComboBoxColumn();
            status.Name = "Status";
            status.DataSource = lstStatus;
            status.DisplayMember = "Contents";
            status.ValueMember = "CodeIdx";
            status.FieldName = "Status";
            status.HeaderText = "Work Status";
            status.ReadOnly = true; 
            status.Width = 110;
            gv.Columns.Add(status);

            GridViewDateTimeColumn RejectDate = new GridViewDateTimeColumn();
            RejectDate.Name = "RejectDate";
            RejectDate.FieldName = "RejectDate";
            RejectDate.Width = 110;
            RejectDate.TextAlignment = ContentAlignment.MiddleCenter;
            RejectDate.FormatInfo = new System.Globalization.CultureInfo("ko-KR");
            RejectDate.FormatString = "{0:g}";
            RejectDate.HeaderText = "Rejected Date";
            RejectDate.ReadOnly = true;
            gv.Columns.Add(RejectDate);

            GridViewTextBoxColumn Rejected = new GridViewTextBoxColumn();
            Rejected.Name = "Rejected";
            Rejected.FieldName = "Rejected";
            Rejected.ReadOnly = true;
            Rejected.Width = 130;
            Rejected.TextAlignment = ContentAlignment.MiddleLeft;
            Rejected.HeaderText = "Rejected";
            gv.Columns.Add(Rejected);

            GridViewTextBoxColumn SewIdx = new GridViewTextBoxColumn();
            SewIdx.Name = "SewIdx";
            SewIdx.FieldName = "SewIdx";
            SewIdx.HeaderText = "Work ID\n(Sew)";
            SewIdx.Width = 100;
            SewIdx.ReadOnly = true;
            SewIdx.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(SewIdx);

            GridViewTextBoxColumn WorkOrderIdx = new GridViewTextBoxColumn();
            WorkOrderIdx.Name = "WorkOrderIdx";
            WorkOrderIdx.FieldName = "WorkOrderIdx";
            WorkOrderIdx.HeaderText = "Work ID\n(Inspect)";
            WorkOrderIdx.Width = 90;
            WorkOrderIdx.ReadOnly = true;
            WorkOrderIdx.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
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

            #region Config Cell Conditions: 오더상태에 따라 행스타일 변경

            // 캔슬오더 색상변경
            Font f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            ConditionalFormattingObject obj = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "4", "", true);
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
            ConditionalFormattingObject obj5 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "2", "", true);
            obj5.RowForeColor = Color.Black;
            obj5.RowBackColor = Color.Lavender; 
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
            _dt = CommonController.Getlist(CommonValues.KeyName.TDUser).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                lstUser.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]), row["UserName"].ToString(), Convert.ToInt32(row["DeptIdx"])));
            }

            // 오더상태 (CommonValues정의)
            lstStatus.Add(new CodeContents(0, CommonValues.DicWorkOrderStatus[0], ""));
            lstStatus.Add(new CodeContents(1, CommonValues.DicWorkOrderStatus[1], ""));
            lstStatus.Add(new CodeContents(2, CommonValues.DicWorkOrderStatus[2], ""));
            lstStatus.Add(new CodeContents(3, CommonValues.DicWorkOrderStatus[3], ""));
            lstStatus.Add(new CodeContents(4, CommonValues.DicWorkOrderStatus[4], ""));
            lstStatus.Add(new CodeContents(15, CommonValues.DicWorkOrderStatus[15], ""));
            lstStatus.Add(new CodeContents(16, CommonValues.DicWorkOrderStatus[16], ""));
            
            lstStatus2.Add(new CodeContents(0, CommonValues.DicWorkOrderStatus[0], ""));
            lstStatus2.Add(new CodeContents(1, CommonValues.DicWorkOrderStatus[1], ""));
            lstStatus2.Add(new CodeContents(2, CommonValues.DicWorkOrderStatus[2], ""));
            lstStatus2.Add(new CodeContents(3, CommonValues.DicWorkOrderStatus[3], ""));
            lstStatus2.Add(new CodeContents(4, CommonValues.DicWorkOrderStatus[4], ""));
            lstStatus2.Add(new CodeContents(15, CommonValues.DicWorkOrderStatus[15], ""));
            lstStatus2.Add(new CodeContents(16, CommonValues.DicWorkOrderStatus[16], ""));
            
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
                    || ddlStatus.SelectedValue != null || ddlFabric.SelectedValue != null
                    || !string.IsNullOrEmpty(txtFileno.Text) || !string.IsNullOrEmpty(txtStyle.Text) 
                    || !string.IsNullOrEmpty(txtColor.Text) || !string.IsNullOrEmpty(dtRequested.Text) || !string.IsNullOrEmpty(dtCompleted.Text))
                {
                    _searchKey = new Dictionary<CommonValues.KeyName, int>();
                    _searchString = new Dictionary<CommonValues.KeyName, string>();

                    //// 영업부인경우, 해당 부서만 조회할수 있도록 제한 
                    //if (UserInfo.ReportNo < 9)
                    //    _searchKey.Add(CommonValues.KeyName.DeptIdx, UserInfo.DeptIdx);
                    //else
                    //    _searchKey.Add(CommonValues.KeyName.DeptIdx, Convert.ToInt32(ddlSize.SelectedValue));

                    _searchKey.Add(CommonValues.KeyName.BuyerIdx, Convert.ToInt32(ddlCust.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.WorkStatus, Convert.ToInt32(ddlStatus.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Size, Convert.ToInt32(ddlSize.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.User, Convert.ToInt32(ddlFabric.SelectedValue));

                    _searchString.Add(CommonValues.KeyName.OrderIdx, txtFileno.Text.ToString().Trim());
                    _searchString.Add(CommonValues.KeyName.Styleno, txtStyle.Text.ToString().Trim());
                    _searchString.Add(CommonValues.KeyName.WorkOrderIdx, "");
                    _searchString.Add(CommonValues.KeyName.ColorIdx, txtColor.Text.ToString().Trim());
                    
                    CultureInfo ci = new CultureInfo("ko-KR");
                    _searchString.Add(CommonValues.KeyName.RequestDate, dtRequested.Value.ToString("d", ci).Substring(0,10));
                    _searchString.Add(CommonValues.KeyName.CompleteDate, dtCompleted.Value.ToString("d", ci).Substring(0, 10));

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
                
                _ds1 = Controller.Inspecting.Getlist(SearchKey, SearchString);
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

        private void DataBinding_GV2(int Idx)
        {
            try
            {
                txtResult.Text = "";
                txtAction.Text = "";
                
                //ddlStatus2.SelectedValue = 0;
                
                _ds1 = Controller.Inspecting.Getlist(Idx);

                if (_ds1 != null)
                {
                    txtResult.Text = _ds1.Tables[0].Rows[0]["Result"].ToString();
                    txtAction.Text = _ds1.Tables[0].Rows[0]["Action"].ToString();
                
                    //ddlStatus2.SelectedValue = Convert.ToInt32(_ds1.Tables[0].Rows[0]["Status"].ToString());

                    SetDefaultFontPropertiesToEditor(txtResult);
                    SetDefaultFontPropertiesToEditor(txtAction);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBindingGv2: " + ex.Message.ToString());
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
                //_gv1.EndEdit();
                GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);  // 현재 행번호 확인

                // 객체생성 및 값 할당
                _obj1 = new Controller.Inspecting(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value));
                _obj1.Idx = Convert.ToInt32(row.Cells["Idx"].Value.ToString());

                //if (row.Cells["OutOrderIdx"].Value != null && row.Cells["OutOrderIdx"].Value != DBNull.Value)
                //    _obj1.OutOrderIdx = row.Cells["OutOrderIdx"].Value.ToString();
                //else _obj1.OutOrderIdx = "";
                //if (row.Cells["RcvdQty"].Value != DBNull.Value) _obj1.RcvdQty = Convert.ToInt32(row.Cells["RcvdQty"].Value.ToString()); else _obj1.RcvdQty = 0;
                //if (row.Cells["RcvdFrom"].Value != DBNull.Value) _obj1.RcvdFrom = Convert.ToInt32(row.Cells["RcvdFrom"].Value.ToString()); else _obj1.RcvdFrom = 0;
                //if (row.Cells["Remarks"].Value != DBNull.Value) _obj1.Remarks = row.Cells["Remarks"].Value.ToString(); else _obj1.Remarks = "";

                if (row.Cells["InspDate"].Value != DBNull.Value && row.Cells["InspDate"] != null)
                {
                    _obj1.InspDate = Convert.ToDateTime(row.Cells["InspDate"].Value);
                }
                else
                {
                    _obj1.InspDate = new DateTime(2000, 1, 1);
                }

                if (row.Cells["InspCompletedDate"].Value != DBNull.Value && row.Cells["InspCompletedDate"] != null)
                {
                    _obj1.InspCompletedDate = Convert.ToDateTime(row.Cells["InspCompletedDate"].Value);
                }
                else
                {
                    _obj1.InspCompletedDate = new DateTime(2000, 1, 1);
                }

                // 업데이트 (오더캔슬, 선적완료 상태가 아닐경우)
                _bRtn = _obj1.Update();
                __main__.lblRows.Text = "Updated Inspection Info";

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

        public void SetDefaultFontPropertiesToEditor(RadRichTextEditor editor)
        {
            editor.Document.Selection.SelectAll();
            editor.RichTextBoxElement.ChangeFontFamily(new Telerik.WinControls.RichTextEditor.UI.FontFamily("Segoe UI"));
            editor.RichTextBoxElement.ChangeFontSize(Unit.PointToDip(9));
            editor.RichTextBoxElement.ChangeFontStyle(Telerik.WinControls.RichTextEditor.UI.FontStyles.Normal);
            editor.RichTextBoxElement.ChangeFontWeight(Telerik.WinControls.RichTextEditor.UI.FontWeights.Normal);

            editor.RichTextBoxElement.ChangeParagraphLineSpacingType(LineSpacingType.Auto);
            editor.RichTextBoxElement.ChangeParagraphLineSpacing(0.5);
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

                    if (el.Value.ToString().Length > 10)
                    {
                        Console.WriteLine(el.Value.ToString().Substring(0, 10));
                        if (el.Value.ToString().Substring(0, 10) == "2000-01-01")
                        {
                            el.Value = Convert.ToDateTime(null);
                        }
                    }

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
                if (Int.Members.GetCurrentRow(_gv1, "Status") == 3
                || Int.Members.GetCurrentRow(_gv1, "Status") == 4)
                {
                    Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = true;
                }
                else
                {
                    Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = false;
                }

                // 행변경시 우측 샘플타입도 변경해준다 
                DataBinding_GV2(Int.Members.GetCurrentRow(_gv1, "Idx"));

                if (Options.UserInfo.DeptIdx == 12)
                {
                    radScrollablePanel1.Enabled = true; 
                }
                else
                {
                    radScrollablePanel1.Enabled = false; 
                }

                if (Int.Members.GetCurrentRow(_gv1, "Status") >4)
                {
                    btnConfirm.Enabled = false; 
                }
                else
                {
                    btnConfirm.Enabled = true; 
                }

                //GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);
                //if (row.Cells["Attached1"].Value != DBNull.Value) linkLabel1.Text = row.Cells["Attached1"].Value.ToString();
                //if (row.Cells["Attached2"].Value != DBNull.Value) linkLabel2.Text = row.Cells["Attached2"].Value.ToString();
                //if (row.Cells["Attached3"].Value != DBNull.Value) linkLabel3.Text = row.Cells["Attached3"].Value.ToString();

            }
            catch (Exception ex)
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
            dtCompleted.Value = Convert.ToDateTime("2000-01-01");
        }

        private void gvMain_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.Cancel = true; 
        }

        private void gvMain_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveData_Click(object sender, EventArgs e)
        {
            _bRtn = false;
            try
            {
                //_gv1.EndEdit();
                if (_gv1.SelectedRows.Count <= 0)
                {
                    RadMessageBox.Show("You must select a row. Please search the data."); return;
                }

                GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);  // 현재 행번호 확인

                if (Convert.ToInt32(row.Cells["Status"].Value) == 3 || Convert.ToInt32(row.Cells["Status"].Value) == 4)
                {
                    RadMessageBox.Show("You can't modify canceled or completed data.", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }

                // 업데이트 (오더캔슬, 선적완료 상태가 아닐경우)
                _bRtn = Controller.Inspecting.Update2(Convert.ToInt32(row.Cells["Idx"].Value),
                                    row.Cells["WorkOrderIdx"].Value.ToString(),
                                    txtResult.Text.Trim(),
                                    txtAction.Text.Trim(), 0);

                __main__.lblRows.Text = "Updated Inspection Info";

            }
            catch (Exception ex)
            {
                Console.WriteLine("GV2_Update: " + ex.Message.ToString());
            }
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            _bRtn = false;
            int status = 0;

            try
            {
                if (_gv1.SelectedRows.Count <= 0)
                {
                    RadMessageBox.Show("You must select a row. Please search the data."); return;
                }

                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
                int _mode_ = 1;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    _gv1.EndEdit();
                    GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);

                    if (Convert.ToInt32(row.Cells["Status"].Value) == 3 || Convert.ToInt32(row.Cells["Status"].Value) == 4)
                    {
                        RadMessageBox.Show("You can't modify canceled or completed data.", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                        return;
                    }

                    if (opReorder.Checked == false && opModifiable.Checked == false)
                    {
                        RadMessageBox.Show("Please select the reject option", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                        return;
                    }
                    else
                    {
                        if (opReorder.Checked) status = 16;
                        else if (opModifiable.Checked) status = 15; 
                        
                    }

                    // 업데이트 (오더캔슬, 선적완료 상태가 아닐경우)
                    _bRtn = Controller.Inspecting.Update2(Convert.ToInt32(row.Cells["Idx"].Value),
                                        row.Cells["WorkOrderIdx"].Value.ToString(),
                                        txtResult.Text.Trim(),
                                        txtAction.Text.Trim(), 0);

                    if (Options.UserInfo.DeptIdx == 12)     // TD
                    {
                        _bRtn = Data.InspectingData.RejectTD(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value),
                                        Options.UserInfo.Idx, status, 
                                        _gv1.Rows[row.Index].Cells["WorkOrderIdx"].Value.ToString().Trim());

                        if (_bRtn)
                        {
                            __main__.lblRows.Text = "Rejected by TD";
                            RadMessageBox.Show("Rejected by TD.", "Rejected TD");
                        }

                        // 오더핸들러 전화번호가 등록되어 있는 경우
                        DataRow dr = Dev.Options.Data.CommonData.GetPhoneNumberbyOrderID(Convert.ToInt32(_gv1.Rows[row.Index].Cells["OrderIdx"].Value.ToString()));
                        if (dr != null && !string.IsNullOrEmpty(dr["Phone"].ToString().Trim()))
                        {
                            // 결과 메시지 송신
                            Controller.TelegramMessageSender msgSender = new Controller.TelegramMessageSender();
                            msgSender.sendMessage(dr["Phone"].ToString().Trim(), "[검사반려] " +
                                        "Buyer: " + _gv1.Rows[row.Index].Cells["Buyer"].Value.ToString() + ", " +
                                        "File: " + _gv1.Rows[row.Index].Cells["Fileno"].Value.ToString() + ", " +
                                        "Style: " + _gv1.Rows[row.Index].Cells["Styleno"].Value.ToString() + ", " +
                                        "Color: " + _gv1.Rows[row.Index].Cells["OrdColorIdx"].Value.ToString() + ", " +
                                        "Q'ty: " + _gv1.Rows[row.Index].Cells["WorkQty"].Value.ToString() + "\n" +
                                        "Rejected Date: " + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd HH:mm") + ", " +
                                        "Rejected by " + Options.UserInfo.Userfullname.ToString() + "\n" +
                                        "Result: " + txtResult.Text.Trim()
                                        );
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GV2_Reject: " + ex.Message.ToString());
            }
        }

        private void gvMain_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            //  
            if (e.CellElement.ColumnInfo.Name == "Confirmed" || e.CellElement.ColumnInfo.Name == "InspCompletedDate")
            {
                e.CellElement.BackColor = Color.PaleGreen;
                e.CellElement.ForeColor = Color.Black;
                e.CellElement.GradientStyle = GradientStyles.Solid;
                e.CellElement.DrawFill = true;
                e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
            }
            else if (e.CellElement.ColumnInfo.Name == "RejectDate" || e.CellElement.ColumnInfo.Name == "Rejected")
            {
                e.CellElement.BackColor = Color.White;
                e.CellElement.ForeColor = Color.Red;
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

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            _bRtn = false;

            try
            {
                if (_gv1.SelectedRows.Count <= 0)
                {
                    RadMessageBox.Show("You must select a row. Please search the data."); return;
                }

                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
                int _mode_ = 1;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    _gv1.EndEdit();
                    GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);

                    if (Convert.ToInt32(row.Cells["Status"].Value) == 3 || Convert.ToInt32(row.Cells["Status"].Value) == 4)
                    {
                        RadMessageBox.Show("You can't modify canceled or completed data.", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                        return;
                    }

                    // 업데이트 (오더캔슬, 선적완료 상태가 아닐경우)
                    _bRtn = Controller.Inspecting.Update2(Convert.ToInt32(row.Cells["Idx"].Value),
                                        row.Cells["WorkOrderIdx"].Value.ToString(),
                                        txtResult.Text.Trim(),
                                        txtAction.Text.Trim(), 0);

                    if (Options.UserInfo.DeptIdx == 12)     // TD
                    {
                        _bRtn = Data.InspectingData.ConfirmTD(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value),
                                        Options.UserInfo.Idx, _gv1.Rows[row.Index].Cells["WorkOrderIdx"].Value.ToString().Trim());

                        if (_bRtn)
                        {
                            __main__.lblRows.Text = "Confirmed by TD";
                            RadMessageBox.Show("Confirmed by TD.", "Confirmed TD");
                        }
                        // 오더핸들러 전화번호가 등록되어 있는 경우
                        DataRow dr = Dev.Options.Data.CommonData.GetPhoneNumberbyOrderID(Convert.ToInt32(_gv1.Rows[row.Index].Cells["OrderIdx"].Value.ToString()));
                        if (dr != null && !string.IsNullOrEmpty(dr["Phone"].ToString().Trim()))
                        {
                            // 결과 메시지 송신
                            Controller.TelegramMessageSender msgSender = new Controller.TelegramMessageSender();
                            msgSender.sendMessage(dr["Phone"].ToString().Trim(), "[검사완료] " +
                                        "Buyer: " + _gv1.Rows[row.Index].Cells["Buyer"].Value.ToString() + ", " +
                                        "File: " + _gv1.Rows[row.Index].Cells["Fileno"].Value.ToString() + ", " +
                                        "Style: " + _gv1.Rows[row.Index].Cells["Styleno"].Value.ToString() + ", " +
                                        "Color: " + _gv1.Rows[row.Index].Cells["OrdColorIdx"].Value.ToString() + ", " +
                                        "Q'ty: " + _gv1.Rows[row.Index].Cells["WorkQty"].Value.ToString() + "\n" +
                                        "Confirmed by " + Options.UserInfo.Userfullname.ToString() 
                                        );
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GV2_Confirm: " + ex.Message.ToString());
            }
        }
    }
}
