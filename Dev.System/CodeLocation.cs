using Int.Code;
using Int.Customer;
using Dev.Options;
using System;
using System.IO; 
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Telerik.WinControls;
using Telerik.WinControls.UI; 

namespace Dev.Codes
{
    /// <summary>
    /// 각 비용센터/부서/창고별 적재 장소관리 
    /// 원단/부자재/완성창고 등 구분은 Code에 등록한후 불러온다 
    /// </summary>
    public partial class CodeLocation : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                     // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, int> _searchKey;        // 쿼리값 value, 쿼리항목 key로 전달 
        private bool _bRtn;                                             // 쿼리결과 리턴
        private DataSet _ds1 = null;                                    // 기본 데이터셋
        private DataTable _dt = null;                                   // 기본 데이터테이블
        private RadContextMenu contextMenu;                             // 컨텍스트 메뉴
        private List<CodeContents> lstRack1 = new List<CodeContents>();       // 
        private List<CodeContents> lstRack2 = new List<CodeContents>();        // 
        private List<CodeContents> lstRack3 = new List<CodeContents>();        // 
        private List<CodeContents> lstRack21 = new List<CodeContents>();       // 
        private List<CodeContents> lstRack22 = new List<CodeContents>();        // 
        private List<CodeContents> lstRack23 = new List<CodeContents>();        //
        private List<CodeContents> lstWarehouse = new List<CodeContents>();         // 코드
        private List<CodeContents> lstWarehouse2 = new List<CodeContents>();         // 코드
        private List<CustomerName> lstDept = new List<CustomerName>();     // 

        private string _layoutfile = "/GVLayoutCodeLocation.xml";
        RadMenuItem mnuNew, mnuDel, mnuHide, mnuShow = null;

        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 59, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        private int _centerIdx = 4;     // 개발실 고정
        private int _deptIdx = 7;       // 사무실 고정
        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public CodeLocation(InheritMDI main)
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
        private void CodeLocation_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;  // 창 최대화
            DataLoading_DDL();
            Config_DropDownList(); 
            GV1_CreateColumn(_gv1);     // 그리드뷰 생성
            GV1_LayoutSetting(_gv1);    // 중앙 그리드뷰 설정 
            Config_ContextMenu();       // 중앙 그리드뷰 컨텍스트 생성 설정 
            LoadGVLayout();             // 그리드뷰 레이아웃 복구 
            //DataBinding_GV1(0, null, "", "");   // 중앙 그리드뷰 데이터 
        }
              

        /// <summary>
        /// 그리드뷰 컨텍스트 메뉴 생성  
        /// </summary>
        private void Config_ContextMenu()
        {
            contextMenu = new RadContextMenu();
            Clear_Shortcuts(); 

            // 오더 신규 입력
            mnuNew = new RadMenuItem("New Location");
            // mnuNew.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.N));
            mnuNew.Click += new EventHandler(mnuNew_Click);

            // 오더 삭제
            mnuDel = new RadMenuItem("Remove Location");
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
            contextMenu.Items.Add(mnuNew);
            contextMenu.Items.Add(mnuDel);
            contextMenu.Items.Add(separator);

            contextMenu.Items.Add(mnuHide);
            contextMenu.Items.Add(mnuShow);
            
        }

        /// <summary>
        /// 상단 검색을 위한 Dropdownlist 생성
        /// </summary>
        private void Config_DropDownList()
        {
            
            // 
            ddlWarehouse.DataSource = lstWarehouse2;
            ddlWarehouse.DisplayMember = "Contents";
            ddlWarehouse.ValueMember = "CodeIdx";
            ddlWarehouse.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlWarehouse.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlWarehouse.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
            
            ddlRack1.DataSource = lstRack21;
            ddlRack1.DisplayMember = "Contents";
            ddlRack1.ValueMember = "CodeIdx";
            ddlRack1.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlRack1.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlRack1.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
            // 
            ddlRack2.DataSource = lstRack22;
            ddlRack2.DisplayMember = "Contents";
            ddlRack2.ValueMember = "CodeIdx";
            ddlRack2.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlRack2.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlRack2.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
            // 
            ddlRack3.DataSource = lstRack23;
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
            Idx.Width = 50;
            Idx.TextAlignment = ContentAlignment.MiddleLeft;
            gv.Columns.Add(Idx);

            
            GridViewComboBoxColumn Warehouse = new GridViewComboBoxColumn();
            Warehouse.Name = "Warehouse";
            Warehouse.DataSource = lstWarehouse;
            Warehouse.DisplayMember = "Contents";
            Warehouse.ValueMember = "CodeIdx";
            Warehouse.FieldName = "Warehouse";
            Warehouse.HeaderText = "Warehouse";
            Warehouse.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            Warehouse.DropDownStyle = RadDropDownStyle.DropDown;
            Warehouse.Width = 100;
            gv.Columns.Add(Warehouse);

            GridViewComboBoxColumn RackNo = new GridViewComboBoxColumn();
            RackNo.Name = "RackNo";
            RackNo.DataSource = lstRack1;
            RackNo.DisplayMember = "Contents";
            RackNo.ValueMember = "CodeIdx";
            RackNo.FieldName = "RackNo";
            RackNo.HeaderText = "RackNo";
            RackNo.TextAlignment = ContentAlignment.MiddleCenter; 
            RackNo.AutoCompleteMode = AutoCompleteMode.Suggest;
            RackNo.DropDownStyle = RadDropDownStyle.DropDownList;
            RackNo.Width = 70;
            gv.Columns.Add(RackNo);

            GridViewComboBoxColumn Floorno = new GridViewComboBoxColumn();
            Floorno.Name = "Floorno";
            Floorno.DataSource = lstRack2;
            Floorno.DisplayMember = "Contents";
            Floorno.ValueMember = "CodeIdx";
            Floorno.FieldName = "Floorno";
            Floorno.HeaderText = "Floorno";
            Floorno.TextAlignment = ContentAlignment.MiddleCenter;
            Floorno.AutoCompleteMode = AutoCompleteMode.Suggest;
            Floorno.DropDownStyle = RadDropDownStyle.DropDownList;
            Floorno.Width = 70;
            gv.Columns.Add(Floorno);

            GridViewComboBoxColumn RackPos = new GridViewComboBoxColumn();
            RackPos.Name = "RackPos";
            RackPos.DataSource = lstRack3;
            RackPos.DisplayMember = "Contents";
            RackPos.ValueMember = "CodeIdx";
            RackPos.FieldName = "RackPos";
            RackPos.HeaderText = "RackPos";
            RackPos.TextAlignment = ContentAlignment.MiddleCenter;
            RackPos.AutoCompleteMode = AutoCompleteMode.Suggest;
            RackPos.DropDownStyle = RadDropDownStyle.DropDownList;
            RackPos.Width = 70;
            gv.Columns.Add(RackPos);

            GridViewTextBoxColumn PosX = new GridViewTextBoxColumn();
            PosX.DataType = typeof(int);
            PosX.Name = "PosX";
            PosX.FieldName = "PosX";
            PosX.HeaderText = "PosX";
            PosX.Width = 60;
            PosX.TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns.Add(PosX);

            GridViewTextBoxColumn PosY = new GridViewTextBoxColumn();
            PosY.DataType = typeof(int);
            PosY.Name = "PosY";
            PosY.FieldName = "PosY";
            PosY.HeaderText = "PosY";
            PosY.Width = 60;
            PosY.TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns.Add(PosY);
            
            GridViewTextBoxColumn Remark = new GridViewTextBoxColumn();
            Remark.Name = "Remark";
            Remark.FieldName = "Remark";
            Remark.HeaderText = "Remark";
            Remark.Width = 200;
            Remark.TextAlignment = ContentAlignment.MiddleLeft;
            gv.Columns.Add(Remark);

            GridViewTextBoxColumn LocationName = new GridViewTextBoxColumn();
            LocationName.Name = "LocationName";
            LocationName.FieldName = "LocationName";
            LocationName.HeaderText = "Location";
            LocationName.Width = 100;
            LocationName.TextAlignment = ContentAlignment.MiddleLeft;
            gv.Columns.Add(LocationName);

            GridViewCheckBoxColumn IsUse = new GridViewCheckBoxColumn();
            IsUse.DataType = typeof(int);
            IsUse.Name = "IsUse";
            IsUse.FieldName = "IsUse";
            IsUse.HeaderText = "Use";
            IsUse.WrapText = true;
            IsUse.Width = 40;
            gv.Columns.Add(IsUse);
            
            GridViewTextBoxColumn Qrcode = new GridViewTextBoxColumn();
            Qrcode.Name = "Qrcode";
            Qrcode.FieldName = "Qrcode";
            Qrcode.HeaderText = "Qrcode";
            Qrcode.Width = 130;
            Qrcode.TextAlignment = ContentAlignment.MiddleLeft;
            gv.Columns.Add(Qrcode);

            GridViewCommandColumn CreateCode = new GridViewCommandColumn();
            CreateCode.Name = "CreateCode";
            CreateCode.UseDefaultText = true;
            CreateCode.DefaultText = "Create";
            CreateCode.FieldName = "CreateCode";
            CreateCode.HeaderText = "Create QR";
            CreateCode.TextAlignment = ContentAlignment.MiddleCenter; 
            CreateCode.Width = 80;
            CreateCode.Tag = "CreateCode";
            gv.MasterTemplate.Columns.Add(CreateCode);
            gv.CommandCellClick += new CommandCellClickEventHandler(GV1_CommandCellClick);

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
            ConditionalFormattingObject obj = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "0", "", true);
            obj.RowForeColor = Color.Black;
            obj.RowBackColor = Color.FromArgb(255, 255, 230, 230);
            obj.RowFont = f;
            gv.Columns["IsUse"].ConditionalFormattingObjectList.Add(obj);

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

                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                int _mode_ = 2;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
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
                        _bRtn = Controller.Location.Delete(str);
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
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                int _mode_ = 1;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    DataRow row = Controller.Location.Insert(_centerIdx, _deptIdx);

                    RefleshWithCondition();
                    SetCurrentRow(_gv1, Convert.ToInt32(row["LastIdx"]));   // 신규입력된 행번호로 이동
                }
                
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


        #endregion

        #region 5. 데이터 조회 (바인딩 테스트후, 무겁고 너무느려서 직접 쿼리로 제어)

        /// <summary>
        /// Dropdownlist 데이터 로딩
        /// </summary>
        private void DataLoading_DDL()
        {
            
            // 코드
            _dt = Code.Getlist("Warehouse").Tables[0];

            lstWarehouse.Add(new CodeContents(0, "", ""));
            lstWarehouse2.Add(new CodeContents(0, "", ""));
            foreach (DataRow row in _dt.Rows)
            {
                lstWarehouse.Add(new CodeContents(Convert.ToInt32(row["Idx"]),
                                            row["Contents"].ToString(),
                                            row["Classification"].ToString()));
                lstWarehouse2.Add(new CodeContents(Convert.ToInt32(row["Idx"]),
                                            row["Contents"].ToString(),
                                            row["Classification"].ToString()));
            }

            #region Rack 리스트 생성 

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

            // Rack Floor 추가 또는 수정시 PrintQRCode.cs도 같이 수정필요 !!! 
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

            #endregion
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
                DataBinding_GV1(_centerIdx, _deptIdx, Convert.ToInt32(ddlWarehouse.SelectedValue), Convert.ToInt32(ddlRack1.SelectedValue),
                                Convert.ToInt32(ddlRack2.SelectedValue), Convert.ToInt32(ddlRack3.SelectedValue),
                                Convert.ToInt32(txtLocationX.Value), Convert.ToInt32(txtLocationY.Value), txtRemark.Text.Trim());
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        /// <summary>
        /// 메인 그리드뷰 데이터 로딩
        /// </summary>
        /// <param name="ColorName">컬러명</param>
        private void DataBinding_GV1(int CostcenterIdx, int DeptIdx, int Warehouse, int RackNo, int Floorno,
                                     int RackPos, int PosX, int PosY, string Remark)
        {
            try
            {
                //조회시간
                //Stopwatch sw = new Stopwatch();
                //sw.Start(); 

                _gv1.DataSource = null;

                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                int _mode_ = 0;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_); 
                else
                {
                    _ds1 = Controller.Location.Getlist(CostcenterIdx, DeptIdx, Warehouse, RackNo, Floorno,
                                     RackPos, PosX, PosY, Remark);

                    if (_ds1 != null)
                    {
                        _gv1.DataSource = _ds1.Tables[0].DefaultView;
                        // 조회 후, 상태알림 및 설정적용
                        __main__.lblRows.Text = _gv1.RowCount.ToString() + " Rows";
                        _gv1.EnablePaging = CommonValues.enablePaging;
                        _gv1.AllowSearchRow = CommonValues.enableSearchRow;
                    }
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

        private void GV1_CommandCellClick(object sender, GridViewCellEventArgs e)
        {
            try
            {
                _gv1.EndEdit();
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                int _mode_ = 1;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);
                    // 코드리딩시 적재위치 구분용 문자 3자리 추가 
                    string __QRCode__ = "LOC," + Convert.ToInt32(row.Cells["Idx"].Value).ToString() + "," + _centerIdx + "," + _deptIdx + "," +
                                                            Convert.ToInt32(row.Cells["Warehouse"].Value).ToString() + "," +
                                                            Convert.ToInt32(row.Cells["PosX"].Value).ToString() + "," +
                                                            Convert.ToInt32(row.Cells["PosY"].Value).ToString() + "," +
                                                            Convert.ToInt32(row.Cells["RackNo"].Value).ToString() + "," +
                                                            Convert.ToInt32(row.Cells["Floorno"].Value).ToString() + "," +
                                                            Convert.ToInt32(row.Cells["RackPos"].Value).ToString();
                    e.Row.Cells["Qrcode"].Value = __QRCode__;
                }
                    
            }
            catch (Exception ex)
            {
                Console.WriteLine("GV1_CommandCellClick: " + ex.Message.ToString());
            }
        }

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
                    _bRtn = Controller.Location.Update(Convert.ToInt32(row.Cells["Idx"].Value),
                                                    row.Cells["LocationName"].Value.ToString(),
                                                    _centerIdx, _deptIdx,
                                                    Convert.ToInt32(row.Cells["Warehouse"].Value),
                                                    Convert.ToInt32(row.Cells["PosX"].Value),
                                                    Convert.ToInt32(row.Cells["PosY"].Value),
                                                    Convert.ToInt32(row.Cells["RackNo"].Value),
                                                    Convert.ToInt32(row.Cells["Floorno"].Value),
                                                    Convert.ToInt32(row.Cells["RackPos"].Value),
                                                    row.Cells["Remark"].Value.ToString(),
                                                    Convert.ToInt32(row.Cells["IsUse"].Value),
                                                    row.Cells["Qrcode"].Value.ToString()
                                                    );
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
            }
            
        }

        /// <summary>
        /// 그리드뷰 셀 생성시 DDL의 높이,출력항목수 설정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterTemplate_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            //// DDL 높이, 출력항목수 설정
            //RadDropDownListEditor editor = this._gv1.ActiveEditor as RadDropDownListEditor;
            //if (editor != null)
            //{
            //    ((RadDropDownListEditorElement)((RadDropDownListEditor)this._gv1.ActiveEditor).EditorElement).DefaultItemsCountInDropDown
            //        = CommonValues.DDL_DefaultItemsCountInDropDown;
            //    ((RadDropDownListEditorElement)((RadDropDownListEditor)this._gv1.ActiveEditor).EditorElement).DropDownHeight
            //        = CommonValues.DDL_DropDownHeight; 
            //}

            //// 날짜컬럼의 달력크기 설정
            //RadDateTimeEditor dtEditor = e.ActiveEditor as RadDateTimeEditor;
            //if (dtEditor != null)
            //{
            //    RadDateTimeEditorElement el = dtEditor.EditorElement as RadDateTimeEditorElement;
            //    el.CalendarSize = new Size(500, 400);
            //}

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

        /// <summary>
        /// 행 선택시 - 오더캔슬, 마감일때 편집 불가능하도록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_SelectionChanged(object sender, EventArgs e)
        {
            CommonValues.ListWorkID.Clear();
            CommonValues.WorkOperation = "Location";

            // 선택된 모든 행에 대해 
            foreach (GridViewRowInfo row in _gv1.SelectedRows)
            {
                if (string.IsNullOrEmpty(row.Cells["Qrcode"].Value.ToString().Trim()))
                {
                }
                else
                {
                    CommonValues.ListWorkID.Add(row.Cells["Qrcode"].Value.ToString().Trim());
                }
            }
        }

        private void gvMain_MouseEnter(object sender, EventArgs e)
        {
            Config_ContextMenu(); 
        }

        private void CodeColor_Activated(object sender, EventArgs e)
        {
            Config_ContextMenu(); 
        }

        private void CodeColor_Deactivate(object sender, EventArgs e)
        {
            Clear_Shortcuts(); 
        }

        private void gvMain_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement.ColumnInfo.HeaderText == "RackNo" || e.CellElement.ColumnInfo.HeaderText == "Floorno" ||
                    e.CellElement.ColumnInfo.HeaderText == "RackPos")
            {
                e.CellElement.DrawFill = true;
                e.CellElement.BackColor = Color.LemonChiffon;
                e.CellElement.GradientStyle = Telerik.WinControls.GradientStyles.Solid;
            }
            else if (e.CellElement.ColumnInfo.HeaderText == "PosX" || e.CellElement.ColumnInfo.HeaderText == "PosY")
            {
                e.CellElement.DrawFill = true;
                e.CellElement.BackColor = Color.LightGreen;
                e.CellElement.GradientStyle = Telerik.WinControls.GradientStyles.Solid;
            }
            else
            {
                e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, Telerik.WinControls.ValueResetFlags.Local);
                e.CellElement.ResetValue(VisualElement.BackColorProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
            }
        }

        #endregion

        #region 7. 기능 멤버

        /// <summary>
        /// 단축키 초기화 
        /// </summary>
        private void Clear_Shortcuts()
        {
            if (mnuNew != null) { mnuNew.Shortcuts.Clear(); }
            if (mnuDel != null) { mnuDel.Shortcuts.Clear(); }
            if (mnuHide != null) { mnuHide.Shortcuts.Clear(); }
            if (mnuShow != null) { mnuShow.Shortcuts.Clear(); }

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
