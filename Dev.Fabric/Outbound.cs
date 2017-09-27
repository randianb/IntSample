using Int.Code;
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
    /// 원단 출고 
    /// </summary>
    public partial class Outbound : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                     // 부모 MDI (하단 상태바 리턴용) 
        private string _searchDate = "";
        private string _outNo = "";
        public Dictionary<CommonValues.KeyName, string> _searchString;  // 쿼리값 value, 쿼리항목 key로 전달 
        public Dictionary<CommonValues.KeyName, int> _searchKey;        // 쿼리값 value, 쿼리항목 key로 전달 
        private bool _bRtn;                                             // 쿼리결과 리턴
        private DataSet _ds1 = null;                                    // 기본 데이터셋
        private DataTable _dt = null;                                   // 기본 데이터테이블
        private Controller.Outbound _obj1 = null;                        // 현재 생성된 객체 
        private RadContextMenu contextMenu;                             // 컨텍스트 메뉴
        private List<CodeContents> lstStatus = new List<CodeContents>();        // 
        private List<CodeContents> lstStatus2 = new List<CodeContents>();        // 
        private List<CustomerName> lstBuyer = new List<CustomerName>();        // 
        private List<CustomerName> lstBuyer2 = new List<CustomerName>();        // 
        private List<CodeContents> lstColor = new List<CodeContents>();        // 
        private List<CodeContents> lstColor2 = new List<CodeContents>();        // 
        private List<Controller.Fabric> lstFabric = new List<Controller.Fabric>();        // 
        private List<Controller.Fabric> lstFabric2 = new List<Controller.Fabric>();        // 
        private List<CodeContents> lstFabricType = new List<CodeContents>();        // 
        private List<CodeContents> lstFabricType2 = new List<CodeContents>();        // 
        private List<CodeContents> codeName = new List<CodeContents>();         // 코드
        private List<CustomerName> lstDept = new List<CustomerName>();     // 
        private List<CustomerName> lstUser = new List<CustomerName>();          // 유저명
        private List<CustomerName> lstUser2 = new List<CustomerName>();          // 유저명

        private string _layoutfile = "/GVLayoutFabricOutbound.xml";
        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 28, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public Outbound(InheritMDI main, string searchDate, string outNo)
        {
            base.InitializeComponent(); // parent 컴포넌트에 접근하기 위해 컨트롤을 public으로 지정 > 향후 수정 필요 (todo) 
            InitializeComponent();
            __main__ = main;            // MDI 연결 
            _gv1 = this.gvMain;  // 그리드뷰 일반화를 위해 변수 별도 저장
            _searchDate = searchDate;
            _outNo = outNo; 
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Outbound_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;  // 창 최대화
            DataLoading_DDL();          // DDL 데이터 로딩
            Config_DropDownList();      // 상단 DDL 생성 설정
            GV1_CreateColumn(_gv1);     // 그리드뷰 생성
            GV1_LayoutSetting(_gv1);    // 중앙 그리드뷰 설정 
            Config_ContextMenu();       // 중앙 그리드뷰 컨텍스트 생성 설정 
            LoadGVLayout();             // 그리드뷰 레이아웃 복구 
            
            // 시작시 초기 날짜가 있는 경우, 데이터 즉시 조회
            if (!string.IsNullOrEmpty(_searchDate) || !string.IsNullOrEmpty(_outNo))
            {
                try
                {
                    _gv1.DataSource = null;

                    /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                    /// 읽기: 0, 쓰기: 1, 삭제: 2
                    int _mode_ = 0;
                    if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                        CheckAuth.ShowMessage(_mode_);
                    else
                    {
                        _searchString = new Dictionary<CommonValues.KeyName, string>();
                        _searchString.Add(CommonValues.KeyName.Lotno, "");
                        _searchString.Add(CommonValues.KeyName.WorkOrderIdx, _outNo);
                        _searchString.Add(CommonValues.KeyName.ColorIdx, "");

                        _searchKey = new Dictionary<CommonValues.KeyName, int>();
                        _searchKey.Add(CommonValues.KeyName.Status, 0);
                        _searchKey.Add(CommonValues.KeyName.BuyerIdx, 0);
                        
                        _searchKey.Add(CommonValues.KeyName.FabricIdx, 0);
                        _searchKey.Add(CommonValues.KeyName.FabricType, 0);

                        _searchKey.Add(CommonValues.KeyName.InIdx, 0);
                        _searchKey.Add(CommonValues.KeyName.Handler, 0);
                        _searchKey.Add(CommonValues.KeyName.OrderIdx, 0);

                        CultureInfo ci = new CultureInfo("ko-KR");
                        _ds1 = Controller.Outbound.Getlist(_searchString, _searchKey, dtOutboundDate.Value.ToString("d", ci));

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
            mnuNew = new RadMenuItem("New Outbound");
            mnuNew.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.N));
            mnuNew.Click += new EventHandler(mnuNew_Click);

            // 오더 삭제
            mnuDel = new RadMenuItem("Cancel Outbound");
            mnuDel.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.D));
            mnuDel.Click += new EventHandler(mnuDel_Click);

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
            //contextMenu.Items.Add(mnuNew);
            contextMenu.Items.Add(mnuDel);
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
            ddlFabric.DataSource = lstFabric;
            ddlFabric.DisplayMember = "LongName";
            ddlFabric.ValueMember = "Idx";
            ddlFabric.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlFabric.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlFabric.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 
            ddlFabricType.DataSource = lstFabricType;
            ddlFabricType.DisplayMember = "Contents";
            ddlFabricType.ValueMember = "CodeIdx";
            ddlFabricType.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlFabricType.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlFabricType.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
            
        }

        /// <summary>
        /// 그리드뷰 컬럼 생성
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
            Status.Width = 150;
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
                        
            GridViewComboBoxColumn FabricIdx = new GridViewComboBoxColumn();
            FabricIdx.Name = "FabricIdx";
            FabricIdx.DataSource = lstFabric2;
            FabricIdx.DisplayMember = "LongName";
            FabricIdx.ValueMember = "Idx";
            FabricIdx.FieldName = "FabricIdx";
            FabricIdx.HeaderText = "Fabric";
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
            IODeptIdx.HeaderText = "In";
            IODeptIdx.Width = 150;
            gv.Columns.Add(IODeptIdx);

            GridViewTextBoxColumn OrderIdx = new GridViewTextBoxColumn();
            OrderIdx.Name = "OrderIdx";
            OrderIdx.FieldName = "OrderIdx";
            OrderIdx.HeaderText = "Order#";
            OrderIdx.Width = 60;
            gv.Columns.Add(OrderIdx);
            
            GridViewComboBoxColumn Handler = new GridViewComboBoxColumn();
            Handler.Name = "Handler";
            Handler.DataSource = lstUser;
            Handler.DisplayMember = "CustName";
            Handler.ValueMember = "CustIdx";
            Handler.FieldName = "Handler";
            Handler.HeaderText = "Handler";
            Handler.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Handler.DropDownStyle = RadDropDownStyle.DropDown;
            Handler.Width = 70;
            gv.Columns.Add(Handler);

            GridViewTextBoxColumn WorkOrderIdx = new GridViewTextBoxColumn();
            WorkOrderIdx.Name = "WorkOrderIdx";
            WorkOrderIdx.FieldName = "WorkOrderIdx";
            WorkOrderIdx.HeaderText = "Outbound#";
            WorkOrderIdx.ReadOnly = true;
            WorkOrderIdx.TextAlignment = ContentAlignment.MiddleLeft;
            WorkOrderIdx.Width = 100;
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
            InIdx.ReadOnly = true;  
            InIdx.Width = 50;
            gv.Columns.Add(InIdx);

            GridViewCheckBoxColumn IsOut = new GridViewCheckBoxColumn();
            IsOut.Name = "IsOut";
            IsOut.FieldName = "IsOut";
            IsOut.HeaderText = "Out";
            IsOut.Width = 40;
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
            Font f = new Font(new FontFamily("Segoe UI"), 8.25f);
            //ConditionalFormattingObject obj = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "2", "", true);
            //obj.RowForeColor = Color.Black;
            //obj.RowBackColor = Color.FromArgb(255, 255, 230, 230);
            //obj.RowFont = f;
            //gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj);

            //// 마감오더 색상변경
            f = new Font(new FontFamily("Segoe UI"), 8.25f);
            ConditionalFormattingObject obj2 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "True", "", true);
            obj2.RowForeColor = Color.Black;
            obj2.RowBackColor = Color.FromArgb(255, 220, 255, 240);
            obj2.RowFont = f;
            gv.Columns["IsOut"].ConditionalFormattingObjectList.Add(obj2);

            // 완료 색상변경
            //f = new Font(new FontFamily("Segoe UI"), 8.25f);
            //ConditionalFormattingObject obj3 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "1", "", true);
            //obj3.RowForeColor = Color.Black;
            //obj3.RowFont = f;
            //gv.Columns["IsOut"].ConditionalFormattingObjectList.Add(obj3);

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
                    if (RadMessageBox.Show("Do you want to cancel this item?\n(ID: " + str + ")", "Confirm",
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
                ///// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                ///// 읽기: 0, 쓰기: 1, 삭제: 2
                //if (Convert.ToInt16(__AUTHCODE__.Substring(1, 1).Trim()) > 0)
                //{
                //    string WorkOrderIdx = Int.Code.Code.GetPrimaryCode(UserInfo.CenterIdx, UserInfo.DeptIdx, 14, "");           // 입고번호 생성 
                //                                                                                                                // 입고번호 생성과 동시에 QR코드 생성
                //    string __QRCode__ = Int.Encryptor.Encrypt(WorkOrderIdx.Trim(), "love1229");
                //    DataRow row = Controller.Inbound.Insert(WorkOrderIdx, __QRCode__, UserInfo.CenterIdx, UserInfo.DeptIdx, UserInfo.Idx);  // 신규 입력
                //    RefleshWithCondition();                                                                                     // 재조회 
                //    SetCurrentRow(_gv1, Convert.ToInt32(row["LastIdx"]));                                                       // 신규입력된 행번호로 이동
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
               --5:정상출고, 6:출고(원단불량), 7:판매, 8:폐기, 9:악성재고, 10:공장용보관, 11: 재고조정 */
            lstStatus.Add(new CodeContents(0, CommonValues.DicFabricOutStatus[0], ""));
            lstStatus.Add(new CodeContents(3, CommonValues.DicFabricOutStatus[3], ""));
            lstStatus.Add(new CodeContents(5, CommonValues.DicFabricOutStatus[5], ""));
            lstStatus.Add(new CodeContents(6, CommonValues.DicFabricOutStatus[6], ""));
            lstStatus.Add(new CodeContents(7, CommonValues.DicFabricOutStatus[7], ""));
            lstStatus.Add(new CodeContents(8, CommonValues.DicFabricOutStatus[8], ""));
            lstStatus.Add(new CodeContents(11, CommonValues.DicFabricOutStatus[11], ""));

            lstStatus2.Add(new CodeContents(0, CommonValues.DicFabricOutStatus[0], ""));
            lstStatus2.Add(new CodeContents(3, CommonValues.DicFabricOutStatus[3], ""));
            lstStatus2.Add(new CodeContents(5, CommonValues.DicFabricOutStatus[5], ""));
            lstStatus2.Add(new CodeContents(6, CommonValues.DicFabricOutStatus[6], ""));
            lstStatus2.Add(new CodeContents(7, CommonValues.DicFabricOutStatus[7], ""));
            lstStatus2.Add(new CodeContents(8, CommonValues.DicFabricOutStatus[8], ""));
            lstStatus2.Add(new CodeContents(11, CommonValues.DicFabricOutStatus[11], ""));

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
                lstDept.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
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

            // Username
            lstUser.Add(new CustomerName(0, "", 0));
            lstUser2.Add(new CustomerName(0, "", 0));
            _dt = CommonController.Getlist(CommonValues.KeyName.User).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                lstUser.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]), row["UserName"].ToString(), Convert.ToInt32(row["DeptIdx"])));
                lstUser2.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]), row["UserName"].ToString(), Convert.ToInt32(row["DeptIdx"])));
            }
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
                    || !string.IsNullOrEmpty(txtLotno.Text) || !string.IsNullOrEmpty(txtOutboundno.Text) || !string.IsNullOrEmpty(txtInboundno.Text)
                    || ddlRack1.SelectedValue != null || ddlRack2.SelectedValue != null || ddlRack3.SelectedValue != null
                    )
                {
                    _searchString = new Dictionary<CommonValues.KeyName, string>();
                    _searchString.Add(CommonValues.KeyName.Lotno, txtLotno.Text.Trim());
                    _searchString.Add(CommonValues.KeyName.WorkOrderIdx, txtOutboundno.Text.Trim());
                    _searchString.Add(CommonValues.KeyName.ColorIdx, "");

                    _searchKey = new Dictionary<CommonValues.KeyName, int>();
                    _searchKey.Add(CommonValues.KeyName.Status, Convert.ToInt32(ddlStatus.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.BuyerIdx, Convert.ToInt32(ddlBuyer.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.FabricIdx, Convert.ToInt32(ddlFabric.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.FabricType, Convert.ToInt32(ddlFabricType.SelectedValue));

                    _searchKey.Add(CommonValues.KeyName.InIdx, Convert.ToInt32(txtInboundno.Text));
                    _searchKey.Add(CommonValues.KeyName.Handler, 0);
                    _searchKey.Add(CommonValues.KeyName.OrderIdx, 0);
                    
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
                    _ds1 = Controller.Outbound.Getlist(_searchString, _searchKey, dtOutboundDate.Value.ToString("d", ci));

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
                    _obj1 = new Controller.Outbound(Convert.ToInt32(row.Cells["Idx"].Value));
                    _obj1.Idx = Convert.ToInt32(row.Cells["Idx"].Value);

                    //if (row.Cells["Status"].Value != DBNull.Value) _obj1.Status = Convert.ToInt32(row.Cells["Status"].Value);
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
                    //if (row.Cells["IOCenterIdx"].Value != DBNull.Value) _obj1.IOCenterIdx = Convert.ToInt32(row.Cells["IOCenterIdx"].Value);
                    //if (row.Cells["IODeptIdx"].Value != DBNull.Value) _obj1.IODeptIdx = Convert.ToInt32(row.Cells["IODeptIdx"].Value);
                    if (row.Cells["OrderIdx"].Value != DBNull.Value) _obj1.OrderIdx = Convert.ToInt32(row.Cells["OrderIdx"].Value); else _obj1.OrderIdx = 0;

                    if (row.Cells["Comments"].Value != DBNull.Value) _obj1.Comments = row.Cells["Comments"].Value.ToString(); else _obj1.Comments = "";

                    if (row.Cells["Handler"].Value != DBNull.Value) _obj1.Handler = Convert.ToInt32(row.Cells["Handler"].Value); else _obj1.Handler = 0;
                    if (row.Cells["IsOut"].Value != DBNull.Value) _obj1.IsOut = Convert.ToInt32(row.Cells["IsOut"].Value); else _obj1.IsOut = 0;
                    
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
        
        /// <summary>
        /// 행 선택시 - 오더캔슬, 마감일때 편집 불가능하도록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_SelectionChanged(object sender, EventArgs e)
        {
            if (Int.Members.GetCurrentRow(_gv1, "IsOut") == 1)
            {
                Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = true;
            }
            else
            {
                Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = false;
            }
        }

        private void radLabel12_DoubleClick(object sender, EventArgs e)
        {
            dtOutboundDate.Value = Convert.ToDateTime("2000-01-01"); 
        }

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
        
    }
    
}
