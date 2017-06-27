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
using System.Globalization;

namespace Dev.Production
{
    /// <summary>
    /// 샘플오더 관리화면: 오더입력수정 및 작업지시등
    /// </summary>
    public partial class CuttingMain : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                             // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, string> _searchString;  // 쿼리값 value, 쿼리항목 key로 전달 
        public Dictionary<CommonValues.KeyName, int> _searchKey;        // 쿼리값 value, 쿼리항목 key로 전달 
        public DataRow InsertedOrderRow = null;
        
        private bool _bRtn;                                                     // 쿼리결과 리턴
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        private DataTable _dt = null;                                           // 기본 데이터테이블
        private Controller.Cutting _obj1 = null;                                // 현재 생성된 객체 
        private RadContextMenu contextMenu;                                     // 컨텍스트 메뉴
        private List<CodeContents> lstStatus = new List<CodeContents>();        // 오더상태
        private List<CodeContents> lstStatus2 = new List<CodeContents>();        // 오더상태
        private List<CustomerName> custName = new List<CustomerName>();         // 거래처
        private List<CodeContents> sizeName = new List<CodeContents>();
        
        private List<Controller.Fabric> lstFabric = new List<Controller.Fabric>();        // 
        private List<Controller.Fabric> lstFabric2 = new List<Controller.Fabric>();        // 
        
        private string _layoutfile = "/GVLayoutCutting.xml";
        private string _workOrderIdx;

        //RadMenuItem mnuNew, mnuDel, mnuHide, mnuShow, menuItem2, menuItem3, menuItem4, mnuWorksheet, mnuCutting, mnuOutsourcing, mnuSewing, mnuInspecting = null;
        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 39, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public CuttingMain(InheritMDI main, string WorkOrderIdx)
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
                _searchKey.Add(CommonValues.KeyName.FabricIdx, 0);

                _searchString.Add(CommonValues.KeyName.OrderIdx, "");
                _searchString.Add(CommonValues.KeyName.Styleno, "");
                _searchString.Add(CommonValues.KeyName.WorkOrderIdx, _workOrderIdx);
                _searchString.Add(CommonValues.KeyName.ColorIdx, "");
                _searchString.Add(CommonValues.KeyName.Remark, "");
                _searchString.Add(CommonValues.KeyName.StartDate, "2000-01-01");

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

            // 원단
            ddlFabric.DataSource = lstFabric2;
            ddlFabric.DisplayMember = "LongName";
            ddlFabric.ValueMember = "Idx";
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
            cboBuyer.Width = 100;
            cboBuyer.ReadOnly = true;
            gv.Columns.Add(cboBuyer);

            GridViewComboBoxColumn Styleno = new GridViewComboBoxColumn();
            Styleno.Name = "Styleno";
            Styleno.FieldName = "Styleno";
            Styleno.HeaderText = "Style#";
            Styleno.Width = 100;
            Styleno.ReadOnly = true;
            gv.Columns.Add(Styleno);

            GridViewComboBoxColumn Fileno = new GridViewComboBoxColumn();
            Fileno.Name = "Fileno";
            Fileno.FieldName = "Fileno";
            Fileno.HeaderText = "File#";
            Fileno.Width = 80;
            Fileno.ReadOnly = true;
            gv.Columns.Add(Fileno);

            GridViewTextBoxColumn WorkOrderIdx = new GridViewTextBoxColumn();
            WorkOrderIdx.Name = "WorkOrderIdx";
            WorkOrderIdx.FieldName = "WorkOrderIdx";
            WorkOrderIdx.HeaderText = "Work ID";
            WorkOrderIdx.Width = 100;
            WorkOrderIdx.ReadOnly = true;
            WorkOrderIdx.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(WorkOrderIdx);

            GridViewTextBoxColumn OrdColorIdx = new GridViewTextBoxColumn();
            OrdColorIdx.Name = "OrdColorIdx";
            OrdColorIdx.FieldName = "OrdColorIdx";
            OrdColorIdx.HeaderText = "Color";
            OrdColorIdx.Width = 100;
            OrdColorIdx.ReadOnly = true;
            OrdColorIdx.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(OrdColorIdx);

            GridViewComboBoxColumn cboSize = new GridViewComboBoxColumn();
            cboSize.Name = "OrdSizeIdx";
            cboSize.FieldName = "OrdSizeIdx";
            cboSize.HeaderText = "Size";
            cboSize.ReadOnly = true;
            cboSize.Width = 70;
            gv.Columns.Add(cboSize);

            GridViewTextBoxColumn OrdQty = new GridViewTextBoxColumn();
            OrdQty.Name = "OrdQty";
            OrdQty.FieldName = "OrdQty";
            OrdQty.HeaderText = "Order Q'ty";
            OrdQty.Width = 70;
            OrdQty.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            OrdQty.ReadOnly = true;
            gv.Columns.Add(OrdQty);

            GridViewDateTimeColumn OrdDate = new GridViewDateTimeColumn();
            OrdDate.Name = "OrdDate";
            OrdDate.FieldName = "OrdDate";
            OrdDate.Width = 100;
            OrdDate.TextAlignment = ContentAlignment.MiddleCenter;
            OrdDate.FormatString = "{0:d}";
            OrdDate.HeaderText = "Requested";
            OrdDate.ReadOnly = true;
            gv.Columns.Add(OrdDate);

            GridViewComboBoxColumn FabricIdx = new GridViewComboBoxColumn();
            FabricIdx.Name = "FabricIdx";
            FabricIdx.DataSource = lstFabric;
            FabricIdx.DisplayMember = "LongName";
            FabricIdx.ValueMember = "Idx";
            FabricIdx.FieldName = "FabricIdx";
            FabricIdx.HeaderText = "Fabric";
            FabricIdx.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            FabricIdx.DropDownStyle = RadDropDownStyle.DropDown;
            FabricIdx.Width = 200;
            gv.Columns.Add(FabricIdx);

            GridViewDateTimeColumn CuttedDate = new GridViewDateTimeColumn();
            CuttedDate.Name = "CuttedDate";
            CuttedDate.FieldName = "CuttedDate";
            CuttedDate.Width = 100;
            CuttedDate.TextAlignment = ContentAlignment.MiddleCenter;
            CuttedDate.CustomFormat = "{d}"; 
            CuttedDate.FormatString = "{0:d}";
            CuttedDate.HeaderText = "Cutted";
            gv.Columns.Add(CuttedDate);

            GridViewTextBoxColumn CuttedNo = new GridViewTextBoxColumn();
            CuttedNo.Name = "CuttedNo";
            CuttedNo.FieldName = "CuttedNo";
            CuttedNo.HeaderText = "Cut#";
            CuttedNo.MaxLength = 2; 
            gv.Columns.Add(CuttedNo);

            GridViewTextBoxColumn CuttedQty = new GridViewTextBoxColumn();
            CuttedQty.Name = "CuttedQty";
            CuttedQty.FieldName = "CuttedQty";
            CuttedQty.HeaderText = "Cutted Q'ty";
            CuttedQty.Width = 70;
            CuttedQty.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            gv.Columns.Add(CuttedQty);

            GridViewTextBoxColumn Balance = new GridViewTextBoxColumn();
            Balance.Name = "Balance";
            Balance.FieldName = "Balance";
            Balance.HeaderText = "Balance";
            Balance.Width = 70;
            Balance.EnableExpressionEditor = true;
            Balance.Expression = "OrdQty-CuttedQty"; 
            Balance.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            gv.Columns.Add(Balance);
            
            GridViewTextBoxColumn CuttedPQty = new GridViewTextBoxColumn();
            CuttedPQty.Name = "CuttedPQty";
            CuttedPQty.FieldName = "CuttedPQty";
            CuttedPQty.HeaderText = "Part Q'ty";
            CuttedPQty.Width = 70;
            CuttedPQty.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            gv.Columns.Add(CuttedPQty);

            GridViewComboBoxColumn status = new GridViewComboBoxColumn();
            status.Name = "Status";
            status.DataSource = lstStatus;
            status.DisplayMember = "Contents";
            status.ValueMember = "CodeIdx";
            status.FieldName = "Status";
            status.HeaderText = "Status";
            status.ReadOnly = true; 
            status.Width = 100;
            gv.Columns.Add(status);

            GridViewTextBoxColumn Remarks = new GridViewTextBoxColumn();
            Remarks.Name = "Remarks";
            Remarks.FieldName = "Remarks";
            Remarks.HeaderText = "Remarks";
            Remarks.Width = 150;
            gv.Columns.Add(Remarks);
                        
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

            // 오더상태 (CommonValues정의)
            lstStatus.Add(new CodeContents(0, CommonValues.DicWorkOrderStatus[0], ""));
            lstStatus.Add(new CodeContents(1, CommonValues.DicWorkOrderStatus[1], ""));
            lstStatus.Add(new CodeContents(2, CommonValues.DicWorkOrderStatus[2], ""));
            lstStatus.Add(new CodeContents(3, CommonValues.DicWorkOrderStatus[3], ""));
            lstStatus.Add(new CodeContents(4, CommonValues.DicWorkOrderStatus[4], ""));

            lstStatus2.Add(new CodeContents(0, CommonValues.DicWorkOrderStatus[0], ""));
            lstStatus2.Add(new CodeContents(1, CommonValues.DicWorkOrderStatus[1], ""));
            lstStatus2.Add(new CodeContents(2, CommonValues.DicWorkOrderStatus[2], ""));
            lstStatus2.Add(new CodeContents(3, CommonValues.DicWorkOrderStatus[3], ""));
            lstStatus2.Add(new CodeContents(4, CommonValues.DicWorkOrderStatus[4], ""));
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
                    || !string.IsNullOrEmpty(txtColor.Text) || !string.IsNullOrEmpty(dtCutted.Text))
                {
                    _searchKey = new Dictionary<CommonValues.KeyName, int>();
                    _searchString = new Dictionary<CommonValues.KeyName, string>();

                    //// 영업부인경우, 해당 부서만 조회할수 있도록 제한 
                    //if (UserInfo.ReportNo < 9)
                    //    _searchKey.Add(CommonValues.KeyName.DeptIdx, UserInfo.DeptIdx);
                    //else
                    //    _searchKey.Add(CommonValues.KeyName.DeptIdx, Convert.ToInt32(ddlSize.SelectedValue));

                    _searchKey.Add(CommonValues.KeyName.BuyerIdx, Convert.ToInt32(ddlCust.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Status, Convert.ToInt32(ddlStatus.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Size, Convert.ToInt32(ddlSize.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.FabricIdx, Convert.ToInt32(ddlFabric.SelectedValue));

                    _searchString.Add(CommonValues.KeyName.OrderIdx, txtFileno.Text.ToString().Trim());
                    _searchString.Add(CommonValues.KeyName.Styleno, txtStyle.Text.ToString().Trim());
                    _searchString.Add(CommonValues.KeyName.WorkOrderIdx, "");
                    _searchString.Add(CommonValues.KeyName.ColorIdx, txtColor.Text.ToString().Trim());
                    _searchString.Add(CommonValues.KeyName.Remark, "");
                    CultureInfo ci = new CultureInfo("ko-KR");
                    _searchString.Add(CommonValues.KeyName.StartDate, dtCutted.Value.ToString("d", ci).Substring(0,10));

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
                
                _ds1 = Controller.Cutting.Getlist(SearchKey, SearchString);
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
                //_gv1.EndEdit();
                GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);  // 현재 행번호 확인

                // 객체생성 및 값 할당
                _obj1 = new Controller.Cutting(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value));
                _obj1.Idx = Convert.ToInt32(row.Cells["Idx"].Value.ToString());

                if (row.Cells["CuttedNo"].Value != DBNull.Value) _obj1.CuttedNo = row.Cells["CuttedNo"].Value.ToString(); else _obj1.CuttedNo = ""; 
                if (row.Cells["CuttedQty"].Value != DBNull.Value) _obj1.CuttedQty = Convert.ToInt32(row.Cells["CuttedQty"].Value.ToString()); else _obj1.CuttedQty = 0;
                if (row.Cells["CuttedPQty"].Value != DBNull.Value) _obj1.CuttedPQty = Convert.ToInt32(row.Cells["CuttedPQty"].Value.ToString()); else _obj1.CuttedPQty = 0;
                if (row.Cells["FabricIdx"].Value != DBNull.Value) _obj1.FabricIdx = Convert.ToInt32(row.Cells["FabricIdx"].Value.ToString()); else _obj1.FabricIdx = 0;
                if (row.Cells["Remarks"].Value != DBNull.Value) _obj1.Remarks = row.Cells["Remarks"].Value.ToString(); else _obj1.Remarks = "";

                if (row.Cells["CuttedDate"].Value != DBNull.Value && row.Cells["CuttedDate"] != null)
                {
                    _obj1.CuttedDate = Convert.ToDateTime(row.Cells["CuttedDate"].Value);
                }
                else
                {
                    _obj1.CuttedDate = new DateTime(2000, 1, 1);
                }
                
                // 업데이트
                _bRtn = _obj1.Update();
                __main__.lblRows.Text = "Updated Cutting Info";

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
                //if (Int.Members.GetCurrentRow(_gv1, "Status") == 2
                //|| Int.Members.GetCurrentRow(_gv1, "Status") == 3)
                //{
                //    Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = true;
                //}
                //else
                //{
                //    Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = false;
                //}

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
            dtCutted.Value = Convert.ToDateTime("2000-01-01");
        }

        private void gvMain_ContextMenuOpening(object sender, ContextMenuOpeningEventArgs e)
        {
            e.Cancel = true; 
        }

        private void gvMain_Click(object sender, EventArgs e)
        {

        }
    }
}
