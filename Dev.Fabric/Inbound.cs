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

namespace Dev.Fabric 
{
    /// <summary>
    /// 샘플오더 관리화면: 오더입력수정 및 작업지시등
    /// </summary>
    public partial class Inbound : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                     // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, string> _searchString;  // 쿼리값 value, 쿼리항목 key로 전달 
        public Dictionary<CommonValues.KeyName, int> _searchKey;        // 쿼리값 value, 쿼리항목 key로 전달 
        private bool _bRtn;                                             // 쿼리결과 리턴
        private DataSet _ds1 = null;                                    // 기본 데이터셋
        private DataTable _dt = null;                                   // 기본 데이터테이블
        private Controller.Inbound _obj1 = null;                        // 현재 생성된 객체 
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
        private List<CodeContents> lstRack1 = new List<CodeContents>();       // 
        private List<CodeContents> lstRack2 = new List<CodeContents>();        // 
        private List<CodeContents> lstRack3 = new List<CodeContents>();        // 
        private List<CodeContents> lstRack21 = new List<CodeContents>();       // 
        private List<CodeContents> lstRack22 = new List<CodeContents>();        // 
        private List<CodeContents> lstRack23 = new List<CodeContents>();        //
        private List<CodeContents> codeName = new List<CodeContents>();         // 코드
        private List<CustomerName> lstDept = new List<CustomerName>();     // 
        
        private string _layoutfile = "/GVLayoutFabricInbound.xml";

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public Inbound(InheritMDI main)
        {
            base.InitializeComponent(); // parent 컴포넌트에 접근하기 위해 컨트롤을 public으로 지정 > 향후 수정 필요 (todo) 
            InitializeComponent();
            __main__ = main;            // MDI 연결 
            _gv1 = this.gvMain;  // 그리드뷰 일반화를 위해 변수 별도 저장
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCodeSize_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;  // 창 최대화
            DataLoading_DDL();          // DDL 데이터 로딩
            Config_DropDownList();      // 상단 DDL 생성 설정
            GV1_CreateColumn(_gv1);     // 그리드뷰 생성
            GV1_LayoutSetting(_gv1);    // 중앙 그리드뷰 설정 
            Config_ContextMenu();       // 중앙 그리드뷰 컨텍스트 생성 설정 
            LoadGVLayout();             // 그리드뷰 레이아웃 복구 
            //DataBinding_GV1(0, null, "", "");   // 중앙 그리드뷰 데이터 
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
            mnuNew = new RadMenuItem("New Inbound");
            mnuNew.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.N));
            mnuNew.Click += new EventHandler(mnuNew_Click);

            // 오더 삭제
            //mnuDel = new RadMenuItem("Remove Inbound");
            //mnuDel.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.D));
            //mnuDel.Click += new EventHandler(mnuDel_Click);

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
            contextMenu.Items.Add(mnuNew);
            //contextMenu.Items.Add(mnuDel);
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
            ddlColor.ValueMember = "CodeIdx";
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
            // 
            ddlRack1.DataSource = lstRack1;
            ddlRack1.DisplayMember = "Contents";
            ddlRack1.ValueMember = "CodeIdx";
            ddlRack1.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlRack1.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlRack1.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
            // 
            ddlRack2.DataSource = lstRack2;
            ddlRack2.DisplayMember = "Contents";
            ddlRack2.ValueMember = "CodeIdx";
            ddlRack2.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlRack2.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlRack2.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
            // 
            ddlRack3.DataSource = lstRack3;
            ddlRack3.DisplayMember = "Contents";
            ddlRack3.ValueMember = "CodeIdx";
            ddlRack3.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlRack3.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlRack3.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
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
            Status.Width = 100;
            gv.Columns.Add(Status);

            GridViewDateTimeColumn IDate = new GridViewDateTimeColumn();
            IDate.Name = "IDate";
            IDate.Width = 100;
            IDate.TextAlignment = ContentAlignment.MiddleCenter;
            IDate.FormatString = "{0:d}";
            IDate.FieldName = "IDate";
            IDate.HeaderText = "Date";
            IDate.ReadOnly = true;
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
            ColorIdx.ValueMember = "CodeIdx";
            ColorIdx.FieldName = "ColorIdx";
            ColorIdx.HeaderText = "Color";
            ColorIdx.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ColorIdx.DropDownStyle = RadDropDownStyle.DropDown;
            ColorIdx.Width = 100;
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
            FabricType.Width = 150;
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
            
            GridViewComboBoxColumn IODeptIdx = new GridViewComboBoxColumn();
            IODeptIdx.Name = "IODeptIdx";
            IODeptIdx.DataSource = lstDept;
            IODeptIdx.DisplayMember = "CustName";
            IODeptIdx.ValueMember = "CustIdx";
            IODeptIdx.FieldName = "IODeptIdx";
            IODeptIdx.HeaderText = "In";
            IODeptIdx.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            IODeptIdx.DropDownStyle = RadDropDownStyle.DropDown;
            IODeptIdx.Width = 150;
            gv.Columns.Add(IODeptIdx);

            GridViewTextBoxColumn Comments = new GridViewTextBoxColumn();
            Comments.Name = "Comments";
            Comments.FieldName = "Comments";
            Comments.HeaderText = "Comments";
            Comments.TextAlignment = ContentAlignment.MiddleLeft;
            Comments.Width = 100;
            gv.Columns.Add(Comments);

            GridViewTextBoxColumn WorkOrderIdx = new GridViewTextBoxColumn();
            WorkOrderIdx.Name = "WorkOrderIdx";
            WorkOrderIdx.FieldName = "WorkOrderIdx";
            WorkOrderIdx.HeaderText = "Inbound#";
            WorkOrderIdx.ReadOnly = true; 
            WorkOrderIdx.TextAlignment = ContentAlignment.MiddleLeft;
            WorkOrderIdx.Width = 100;
            gv.Columns.Add(WorkOrderIdx);

            GridViewComboBoxColumn RackNo = new GridViewComboBoxColumn();
            RackNo.Name = "RackNo";
            RackNo.DataSource = lstRack21;
            RackNo.DisplayMember = "Contents";
            RackNo.ValueMember = "CodeIdx";
            RackNo.FieldName = "RackNo";
            RackNo.HeaderText = "Rack#";
            RackNo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            RackNo.DropDownStyle = RadDropDownStyle.DropDown;
            RackNo.Width = 70;
            gv.Columns.Add(RackNo);

            GridViewComboBoxColumn Floorno = new GridViewComboBoxColumn();
            Floorno.Name = "Floorno";
            Floorno.DataSource = lstRack22;
            Floorno.DisplayMember = "Contents";
            Floorno.ValueMember = "CodeIdx";
            Floorno.FieldName = "Floorno";
            Floorno.HeaderText = "Floor#";
            Floorno.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Floorno.DropDownStyle = RadDropDownStyle.DropDown;
            Floorno.Width = 70;
            gv.Columns.Add(Floorno);

            GridViewComboBoxColumn RackPos = new GridViewComboBoxColumn();
            RackPos.Name = "RackPos";
            RackPos.DataSource = lstRack23;
            RackPos.DisplayMember = "Contents";
            RackPos.ValueMember = "CodeIdx";
            RackPos.FieldName = "RackPos";
            RackPos.HeaderText = "Rack Position";
            RackPos.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            RackPos.DropDownStyle = RadDropDownStyle.DropDown;
            RackPos.Width = 70;
            gv.Columns.Add(RackPos);

            GridViewTextBoxColumn PosX = new GridViewTextBoxColumn();
            PosX.DataType = typeof(int);
            PosX.Name = "PosX";
            PosX.FieldName = "PosX";
            PosX.HeaderText = "Location(X)";
            PosX.TextAlignment = ContentAlignment.MiddleCenter;
            PosX.Width = 70;
            gv.Columns.Add(PosX);

            GridViewTextBoxColumn PosY = new GridViewTextBoxColumn();
            PosY.DataType = typeof(int);
            PosY.Name = "PosY";
            PosY.FieldName = "PosY";
            PosY.HeaderText = "Location(Y)";
            PosY.TextAlignment = ContentAlignment.MiddleCenter;
            PosY.Width = 70;
            gv.Columns.Add(PosY);

            GridViewTextBoxColumn Qrcode = new GridViewTextBoxColumn();
            Qrcode.DataType = typeof(int);
            Qrcode.Name = "Qrcode";
            Qrcode.FieldName = "Qrcode";
            Qrcode.HeaderText = "QR Code";
            Qrcode.IsVisible = false;
            Qrcode.ReadOnly = true;
            Qrcode.TextAlignment = ContentAlignment.MiddleLeft;
            Qrcode.Width = 70;
            gv.Columns.Add(Qrcode);

            GridViewTextBoxColumn filenm1 = new GridViewTextBoxColumn();
            filenm1.Name = "filenm1";
            filenm1.FieldName = "filenm1";
            filenm1.IsVisible = false; 
            gv.Columns.Add(filenm1);

            GridViewTextBoxColumn filenm2 = new GridViewTextBoxColumn();
            filenm2.Name = "filenm2";
            filenm2.FieldName = "filenm2";
            filenm2.IsVisible = false;
            gv.Columns.Add(filenm2);

            GridViewTextBoxColumn fileurl1 = new GridViewTextBoxColumn();
            fileurl1.Name = "fileurl1";
            fileurl1.FieldName = "fileurl1";
            fileurl1.IsVisible = false;
            gv.Columns.Add(fileurl1);

            GridViewTextBoxColumn fileurl2 = new GridViewTextBoxColumn();
            fileurl2.Name = "fileurl2";
            fileurl2.FieldName = "fileurl2";
            fileurl2.IsVisible = false;
            gv.Columns.Add(fileurl2);


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
            //Font f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Strikeout);
            //ConditionalFormattingObject obj = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "2", "", true);
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

            //// 선적완료 색상변경
            //f = new Font(new FontFamily("Segoe UI"), 8.25f);
            //ConditionalFormattingObject obj3 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "1", "", true);
            //obj3.RowForeColor = Color.Black;
            //obj3.RowFont = f;
            //gv.Columns["ShipCompleted"].ConditionalFormattingObjectList.Add(obj3);

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
                    _bRtn = Controller.Inbound.Delete(str);
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
                string WorkOrderIdx = Int.Code.Code.GetPrimaryCode(UserInfo.CenterIdx, UserInfo.DeptIdx, 14, "");           // 입고번호 생성 
                string __QRCode__ = Int.Encryptor.Encrypt(WorkOrderIdx.Trim(), "love1229");
                DataRow row = Controller.Inbound.Insert(WorkOrderIdx, __QRCode__, UserInfo.CenterIdx, UserInfo.DeptIdx, UserInfo.Idx);  // 신규 입력
                RefleshWithCondition();                                                                                     // 재조회 
                SetCurrentRow(_gv1, Convert.ToInt32(row["LastIdx"]));                                                       // 신규입력된 행번호로 이동
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
               --5:정상출고, 6:출고(원단불량), 7:판매, 8:폐기, 9:악성재고, 10:공장용보관 */
            lstStatus.Add(new CodeContents(0, CommonValues.DicFabricInStatus[0], ""));
            lstStatus.Add(new CodeContents(1, CommonValues.DicFabricInStatus[1], ""));
            lstStatus.Add(new CodeContents(2, CommonValues.DicFabricInStatus[2], ""));
            lstStatus.Add(new CodeContents(3, CommonValues.DicFabricInStatus[3], ""));
            lstStatus.Add(new CodeContents(9, CommonValues.DicFabricInStatus[9], ""));
            lstStatus.Add(new CodeContents(10, CommonValues.DicFabricInStatus[10], ""));

            lstStatus2.Add(new CodeContents(0, CommonValues.DicFabricInStatus[0], ""));
            lstStatus2.Add(new CodeContents(1, CommonValues.DicFabricInStatus[1], ""));
            lstStatus2.Add(new CodeContents(2, CommonValues.DicFabricInStatus[2], ""));
            lstStatus2.Add(new CodeContents(3, CommonValues.DicFabricInStatus[3], ""));
            lstStatus2.Add(new CodeContents(9, CommonValues.DicFabricInStatus[9], ""));
            lstStatus2.Add(new CodeContents(10, CommonValues.DicFabricInStatus[10], ""));

            // department    
            //_dt = CommonController.Getlist(CommonValues.KeyName.DeptIdx).Tables[0];

            //foreach (DataRow row in _dt.Rows)
            //{
            //    // 관리부와 임원은 모든 부서에 접근가능
            //    if (UserInfo.DeptIdx == 5 || UserInfo.DeptIdx == 6)
            //    {
            //        lstDept.Add(new DepartmentName(Convert.ToInt32(row["DeptIdx"]),
            //                                    row["DeptName"].ToString(),
            //                                    Convert.ToInt32(row["CostcenterIdx"])));
            //    }
            //    // 영업부는 해당 부서 데이터만 접근가능
            //    else
            //    {
            //        if (Convert.ToInt32(row["DeptIdx"]) <= 0 || Convert.ToInt32(row["DeptIdx"]) == UserInfo.DeptIdx)
            //        {
            //            lstDept.Add(new DepartmentName(Convert.ToInt32(row["DeptIdx"]),
            //                                    row["DeptName"].ToString(),
            //                                    Convert.ToInt32(row["CostcenterIdx"])));
            //        }
            //    }
            //}

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

            // Lot# 

            // Inbound (WorkOrderIdx)

            // Location X

            // Location Y

            // Rack# 
            lstRack1.Add(new CodeContents(0, "", ""));
            lstRack1.Add(new CodeContents(1, "1", ""));
            lstRack1.Add(new CodeContents(2, "2", ""));
            lstRack1.Add(new CodeContents(3, "3", ""));
            lstRack1.Add(new CodeContents(4, "4", ""));
            lstRack1.Add(new CodeContents(5, "5", ""));
            lstRack1.Add(new CodeContents(6, "6", ""));
            lstRack1.Add(new CodeContents(7, "7", ""));
            lstRack1.Add(new CodeContents(8, "8", ""));
            lstRack1.Add(new CodeContents(9, "9", ""));
            lstRack1.Add(new CodeContents(10, "10", ""));

            lstRack2.Add(new CodeContents(0, "", ""));
            lstRack2.Add(new CodeContents(1, "A", ""));
            lstRack2.Add(new CodeContents(2, "B", ""));
            lstRack2.Add(new CodeContents(3, "C", ""));
            lstRack2.Add(new CodeContents(4, "D", ""));
            lstRack2.Add(new CodeContents(5, "E", ""));
            lstRack2.Add(new CodeContents(6, "F", ""));
            lstRack2.Add(new CodeContents(7, "G", ""));
            lstRack2.Add(new CodeContents(8, "H", ""));
            lstRack2.Add(new CodeContents(9, "I", ""));

            lstRack3.Add(new CodeContents(0, "", ""));
            lstRack3.Add(new CodeContents(1, "1", ""));
            lstRack3.Add(new CodeContents(2, "2", ""));
            lstRack3.Add(new CodeContents(3, "3", ""));
            lstRack3.Add(new CodeContents(4, "4", ""));
            lstRack3.Add(new CodeContents(5, "5", ""));
            lstRack3.Add(new CodeContents(6, "6", ""));
            lstRack3.Add(new CodeContents(7, "7", ""));
            lstRack3.Add(new CodeContents(8, "8", ""));
            lstRack3.Add(new CodeContents(9, "9", ""));
            lstRack3.Add(new CodeContents(10, "10", ""));

            lstRack21.Add(new CodeContents(0, "", ""));
            lstRack21.Add(new CodeContents(1, "1", ""));
            lstRack21.Add(new CodeContents(2, "2", ""));
            lstRack21.Add(new CodeContents(3, "3", ""));
            lstRack21.Add(new CodeContents(4, "4", ""));
            lstRack21.Add(new CodeContents(5, "5", ""));
            lstRack21.Add(new CodeContents(6, "6", ""));
            lstRack21.Add(new CodeContents(7, "7", ""));
            lstRack21.Add(new CodeContents(8, "8", ""));
            lstRack21.Add(new CodeContents(9, "9", ""));
            lstRack21.Add(new CodeContents(10, "10", ""));

            lstRack22.Add(new CodeContents(0, "", ""));
            lstRack22.Add(new CodeContents(1, "A", ""));
            lstRack22.Add(new CodeContents(2, "B", ""));
            lstRack22.Add(new CodeContents(3, "C", ""));
            lstRack22.Add(new CodeContents(4, "D", ""));
            lstRack22.Add(new CodeContents(5, "E", ""));
            lstRack22.Add(new CodeContents(6, "F", ""));
            lstRack22.Add(new CodeContents(7, "G", ""));
            lstRack22.Add(new CodeContents(8, "H", ""));
            lstRack22.Add(new CodeContents(9, "I", ""));

            lstRack23.Add(new CodeContents(0, "", ""));
            lstRack23.Add(new CodeContents(1, "1", ""));
            lstRack23.Add(new CodeContents(2, "2", ""));
            lstRack23.Add(new CodeContents(3, "3", ""));
            lstRack23.Add(new CodeContents(4, "4", ""));
            lstRack23.Add(new CodeContents(5, "5", ""));
            lstRack23.Add(new CodeContents(6, "6", ""));
            lstRack23.Add(new CodeContents(7, "7", ""));
            lstRack23.Add(new CodeContents(8, "8", ""));
            lstRack23.Add(new CodeContents(9, "9", ""));
            lstRack23.Add(new CodeContents(10, "10", ""));
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
                    || !string.IsNullOrEmpty(txtLotno.Text) || !string.IsNullOrEmpty(txtInboundno.Text)
                    || !string.IsNullOrEmpty(txtLocationX.Value.ToString()) || !string.IsNullOrEmpty(txtLocationY.Value.ToString())
                    || ddlRack1.SelectedValue != null || ddlRack2.SelectedValue != null || ddlRack3.SelectedValue != null
                    )
                {
                    _searchString = new Dictionary<CommonValues.KeyName, string>();
                    _searchString.Add(CommonValues.KeyName.Lotno, txtLotno.Text.Trim());
                    _searchString.Add(CommonValues.KeyName.WorkOrderIdx, txtInboundno.Text.Trim());

                    _searchKey = new Dictionary<CommonValues.KeyName, int>();
                    _searchKey.Add(CommonValues.KeyName.Status, Convert.ToInt32(ddlStatus.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.BuyerIdx, Convert.ToInt32(ddlBuyer.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.ColorIdx, Convert.ToInt32(ddlColor.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.FabricIdx, Convert.ToInt32(ddlFabric.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.FabricType, Convert.ToInt32(ddlFabricType.SelectedValue));

                    _searchKey.Add(CommonValues.KeyName.RackNo, Convert.ToInt32(ddlRack1.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Floorno, Convert.ToInt32(ddlRack2.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.RackPos, Convert.ToInt32(ddlRack3.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.PosX, Convert.ToInt32(txtLocationX.Value));
                    _searchKey.Add(CommonValues.KeyName.PosY, Convert.ToInt32(txtLocationY.Value));
                    
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
                
                _ds1 = Controller.Inbound.Getlist(_searchString, _searchKey);

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
                _obj1 = new Controller.Inbound(Convert.ToInt32(row.Cells["Idx"].Value));
                _obj1.Idx = Convert.ToInt32(row.Cells["Idx"].Value);

                if (row.Cells["Status"].Value != DBNull.Value) _obj1.Status = Convert.ToInt32(row.Cells["Status"].Value);
                if (row.Cells["IDate"].Value != DBNull.Value) _obj1.IDate = Convert.ToDateTime(row.Cells["IDate"].Value);
                if (row.Cells["BuyerIdx"].Value != DBNull.Value) _obj1.BuyerIdx = Convert.ToInt32(row.Cells["BuyerIdx"].Value);
                if (row.Cells["ColorIdx"].Value != DBNull.Value) _obj1.ColorIdx = Convert.ToInt32(row.Cells["ColorIdx"].Value);
                if (row.Cells["FabricType"].Value != DBNull.Value) _obj1.FabricType = Convert.ToInt32(row.Cells["FabricType"].Value);
                if (row.Cells["Artno"].Value != DBNull.Value) _obj1.Artno = row.Cells["Artno"].Value.ToString(); else _obj1.Artno = ""; 
                if (row.Cells["Lotno"].Value != DBNull.Value) _obj1.Lotno = row.Cells["Lotno"].Value.ToString(); else _obj1.Lotno = "";
                if (row.Cells["FabricIdx"].Value != DBNull.Value) _obj1.FabricIdx = Convert.ToInt32(row.Cells["FabricIdx"].Value);
                if (row.Cells["Roll"].Value != DBNull.Value) _obj1.Roll = Convert.ToInt32(row.Cells["Roll"].Value);
                if (row.Cells["Width"].Value != DBNull.Value) _obj1.Width = Convert.ToInt32(row.Cells["Width"].Value);

                if (row.Cells["Kgs"].Value != DBNull.Value) _obj1.Kgs = Convert.ToInt32(row.Cells["Kgs"].Value);
                if (row.Cells["Yds"].Value != DBNull.Value) _obj1.Yds = Convert.ToInt32(row.Cells["Yds"].Value);
                //if (row.Cells["RegCenterIdx"].Value != DBNull.Value) _regCenterIdx = Convert.ToInt32(row.Cells["RegCenterIdx"]);
                //if (row.Cells["RegDeptIdx"].Value != DBNull.Value) _regDeptIdx = Convert.ToInt32(row.Cells["RegDeptIdx"]);
                //if (row.Cells["RegUserIdx"].Value != DBNull.Value) _regUserIdx = Convert.ToInt32(row.Cells["RegUserIdx"]);
                //if (row.Cells["RegDate"].Value != DBNull.Value) _regDate = Convert.ToDateTime(row.Cells["RegDate"]);
                if (row.Cells["IOCenterIdx"].Value != DBNull.Value) _obj1.IOCenterIdx = Convert.ToInt32(row.Cells["IOCenterIdx"].Value);
                if (row.Cells["IODeptIdx"].Value != DBNull.Value) _obj1.IODeptIdx = Convert.ToInt32(row.Cells["IODeptIdx"].Value);
                if (row.Cells["Comments"].Value != DBNull.Value) _obj1.Comments = row.Cells["Comments"].Value.ToString(); else _obj1.Comments = "";

                if (row.Cells["WorkOrderIdx"].Value != DBNull.Value) _obj1.WorkOrderIdx = row.Cells["WorkOrderIdx"].Value.ToString();   
                if (row.Cells["RackNo"].Value != DBNull.Value) _obj1.RackNo = Convert.ToInt32(row.Cells["RackNo"].Value); else _obj1.RackNo = 0;
                if (row.Cells["Floorno"].Value != DBNull.Value) _obj1.Floorno = Convert.ToInt32(row.Cells["Floorno"].Value); else _obj1.Floorno = 0;
                if (row.Cells["RackPos"].Value != DBNull.Value) _obj1.RackPos = Convert.ToInt32(row.Cells["RackPos"].Value); else _obj1.RackPos = 0;
                if (row.Cells["PosX"].Value != DBNull.Value) _obj1.PosX = Convert.ToInt32(row.Cells["PosX"].Value); else _obj1.PosX = 0;
                if (row.Cells["PosY"].Value != DBNull.Value) _obj1.PosY = Convert.ToInt32(row.Cells["PosY"].Value); else _obj1.PosY = 0;
                if (row.Cells["Qrcode"].Value != DBNull.Value) _obj1.Qrcode = row.Cells["Qrcode"].Value.ToString(); else _obj1.Qrcode = "";

                if (row.Cells["filenm1"].Value != DBNull.Value) _obj1.Filenm1 = row.Cells["filenm1"].ToString(); else _obj1.Filenm1 = "";
                if (row.Cells["filenm2"].Value != DBNull.Value) _obj1.Filenm2 = row.Cells["filenm2"].ToString(); else _obj1.Filenm2 = "";
                if (row.Cells["fileurl1"].Value != DBNull.Value) _obj1.Fileurl1 = row.Cells["fileurl1"].ToString(); else _obj1.Fileurl1 = "";
                if (row.Cells["fileurl2"].Value != DBNull.Value) _obj1.Fileurl2 = row.Cells["fileurl2"].ToString(); else _obj1.Fileurl2 = "";

                _bRtn = _obj1.Update();
                if (_bRtn) __main__.lblDescription.Text = "Update Succeed"; 
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

        /// <summary>
        /// 행 선택시 - 오더캔슬, 마감일때 편집 불가능하도록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_SelectionChanged(object sender, EventArgs e)
        {

            //if (Int.Members.GetCurrentRow(_gv1, "ShipCompleted") == 1 || Int.Members.GetCurrentRow(_gv1, "Status") == 2
            //    || Int.Members.GetCurrentRow(_gv1, "Status") == 3)
            //{
            //    Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = true;
            //}
            //else
            //{
            //    Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = false;
            //}
        }

        #endregion

        #region 7. 기능 멤버 (변경필요없음)

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
