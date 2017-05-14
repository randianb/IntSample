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

namespace Dev.Fabric
{
    /// <summary>
    /// 샘플오더 관리화면: 오더입력수정 및 작업지시등
    /// </summary>
    public partial class FabricCode : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                     // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, string> _searchString;        // 쿼리값 value, 쿼리항목 key로 전달 
        private bool _bRtn;                                             // 쿼리결과 리턴
        private DataSet _ds1 = null;                                    // 기본 데이터셋
        private DataTable _dt = null;                                   // 기본 데이터테이블
        private Controller.Fabric _obj1 = null;                          // 현재 생성된 객체 
        private RadContextMenu contextMenu;                             // 컨텍스트 메뉴
        private List<CodeContents> lstIsUse = new List<CodeContents>();        // 
        private List<Controller.Yarn> lstYarn1 = new List<Controller.Yarn>();        // 
        private List<Controller.Yarn> lstYarn2 = new List<Controller.Yarn>();        // 
        private List<Controller.Yarn> lstYarn3 = new List<Controller.Yarn>();        // 
        private List<Controller.Yarn> lstYarn4 = new List<Controller.Yarn>();        // 
        private List<Controller.Yarn> lstYarn5 = new List<Controller.Yarn>();        // 

        private string _layoutfile = "/GVLayoutFabricCode.xml";

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public FabricCode(InheritMDI main)
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
            mnuNew = new RadMenuItem("New Yarn");
            mnuNew.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.N));
            mnuNew.Click += new EventHandler(mnuNew_Click);

            // 오더 삭제
            mnuDel = new RadMenuItem("Remove Yarn");
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

        #endregion

        /// <summary>
        /// 상단 검색을 위한 Dropdownlist 생성
        /// </summary>
        private void Config_DropDownList()
        {
            // 
            ddlUse.DataSource = lstIsUse;
            ddlUse.DisplayMember = "Contents";
            ddlUse.ValueMember = "Classification";
            ddlUse.AutoCompleteMode = AutoCompleteMode.Suggest;
            ddlUse.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlUse.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;
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

            GridViewTextBoxColumn LongName = new GridViewTextBoxColumn();
            LongName.Name = "LongName";
            LongName.FieldName = "LongName";
            LongName.HeaderText = "Long Name";
            LongName.TextAlignment = ContentAlignment.MiddleLeft;
            LongName.Width = 300;
            gv.Columns.Add(LongName);

            GridViewTextBoxColumn ShortName = new GridViewTextBoxColumn();
            ShortName.Name = "ShortName";
            ShortName.FieldName = "ShortName";
            ShortName.HeaderText = "Short Name";
            ShortName.TextAlignment = ContentAlignment.MiddleLeft;
            ShortName.Width = 150;
            gv.Columns.Add(ShortName);

            GridViewMultiComboBoxColumn Yarn1 = new GridViewMultiComboBoxColumn();
            Yarn1.Name = "Yarn1";
            Yarn1.DataSource = lstYarn1;
            Yarn1.DisplayMember = "YarnName";
            Yarn1.ValueMember = "Idx";
            Yarn1.FieldName = "Yarn1";
            Yarn1.HeaderText = "Yarn1";
            Yarn1.AutoCompleteMode = AutoCompleteMode.Suggest;
            Yarn1.DropDownStyle = RadDropDownStyle.DropDownList;
            Yarn1.AutoSizeMode = BestFitColumnMode.AllCells;
            //Yarn1.MinWidth = 500; 
            Yarn1.Width = 150;
            gv.Columns.Add(Yarn1);

            GridViewTextBoxColumn Percent1 = new GridViewTextBoxColumn();
            Percent1.DataType = typeof(double);
            Percent1.Name = "Percent1";
            Percent1.FieldName = "Percent1";
            Percent1.HeaderText = "%";
            Percent1.Width = 60;
            Percent1.TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns.Add(Percent1);

            GridViewMultiComboBoxColumn Yarn2 = new GridViewMultiComboBoxColumn();
            Yarn2.Name = "Yarn2";
            Yarn2.DataSource = lstYarn2;
            Yarn2.DisplayMember = "YarnName";
            Yarn2.ValueMember = "Idx";
            Yarn2.FieldName = "Yarn2";
            Yarn2.HeaderText = "Yarn2";
            Yarn2.AutoCompleteMode = AutoCompleteMode.Suggest;
            Yarn2.DropDownStyle = RadDropDownStyle.DropDownList;
            Yarn1.AutoSizeMode = BestFitColumnMode.AllCells;
            Yarn2.Width = 150;
            gv.Columns.Add(Yarn2);

            GridViewTextBoxColumn Percent2 = new GridViewTextBoxColumn();
            Percent2.DataType = typeof(double);
            Percent2.Name = "Percent2";
            Percent2.FieldName = "Percent2";
            Percent2.HeaderText = "%";
            Percent2.Width = 60;
            Percent2.TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns.Add(Percent2);

            GridViewMultiComboBoxColumn Yarn3 = new GridViewMultiComboBoxColumn();
            Yarn3.Name = "Yarn3";
            Yarn3.DataSource = lstYarn3;
            Yarn3.DisplayMember = "YarnName";
            Yarn3.ValueMember = "Idx";
            Yarn3.FieldName = "Yarn3";
            Yarn3.HeaderText = "Yarn3";
            Yarn3.AutoCompleteMode = AutoCompleteMode.Suggest;
            Yarn3.DropDownStyle = RadDropDownStyle.DropDownList;
            Yarn1.AutoSizeMode = BestFitColumnMode.AllCells;
            Yarn3.Width = 150;
            gv.Columns.Add(Yarn3);

            GridViewTextBoxColumn Percent3 = new GridViewTextBoxColumn();
            Percent3.DataType = typeof(double);
            Percent3.Name = "Percent3";
            Percent3.FieldName = "Percent3";
            Percent3.HeaderText = "%";
            Percent3.Width = 60;
            Percent3.TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns.Add(Percent3);

            GridViewMultiComboBoxColumn Yarn4 = new GridViewMultiComboBoxColumn();
            Yarn4.Name = "Yarn4";
            Yarn4.DataSource = lstYarn4;
            Yarn4.DisplayMember = "YarnName";
            Yarn4.ValueMember = "Idx";
            Yarn4.FieldName = "Yarn4";
            Yarn4.HeaderText = "Yarn4";
            Yarn4.AutoCompleteMode = AutoCompleteMode.Suggest;
            Yarn4.DropDownStyle = RadDropDownStyle.DropDownList;
            Yarn1.AutoSizeMode = BestFitColumnMode.AllCells;
            Yarn4.Width = 150;
            gv.Columns.Add(Yarn4);

            GridViewTextBoxColumn Percent4 = new GridViewTextBoxColumn();
            Percent4.DataType = typeof(double);
            Percent4.Name = "Percent4";
            Percent4.FieldName = "Percent4";
            Percent4.HeaderText = "%";
            Percent4.Width = 60;
            Percent4.TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns.Add(Percent4);

            GridViewMultiComboBoxColumn Yarn5 = new GridViewMultiComboBoxColumn();
            Yarn5.Name = "Yarn5";
            Yarn5.DataSource = lstYarn5;
            Yarn5.DisplayMember = "YarnName";
            Yarn5.ValueMember = "Idx";
            Yarn5.FieldName = "Yarn5";
            Yarn5.HeaderText = "Yarn5";
            Yarn5.AutoCompleteMode = AutoCompleteMode.Suggest;
            Yarn5.DropDownStyle = RadDropDownStyle.DropDownList;
            Yarn1.AutoSizeMode = BestFitColumnMode.AllCells;
            Yarn5.Width = 150;
            gv.Columns.Add(Yarn5);

            GridViewTextBoxColumn Percent5 = new GridViewTextBoxColumn();
            Percent5.DataType = typeof(double);
            Percent5.Name = "Percent5";
            Percent5.FieldName = "Percent5";
            Percent5.HeaderText = "%";
            Percent5.Width = 60;
            Percent5.TextAlignment = ContentAlignment.MiddleCenter;
            gv.Columns.Add(Percent5);

            GridViewCheckBoxColumn cIsUse = new GridViewCheckBoxColumn();
            cIsUse.DataType = typeof(int);
            cIsUse.Name = "IsUse";
            cIsUse.FieldName = "IsUse";
            cIsUse.HeaderText = "Use";
            cIsUse.Width = 40;
            gv.Columns.Add(cIsUse);
            
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
                    _bRtn = Controller.Fabric.Delete(str);
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
                DataRow row = Dev.Controller.Fabric.Insert();

                RefleshWithCondition();
                SetCurrentRow(_gv1, Convert.ToInt32(row["LastIdx"]));   // 신규입력된 행번호로 이동
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

            // YarnType
            Dictionary<CommonValues.KeyName, string> _searchString = new Dictionary<CommonValues.KeyName, string>();
            _searchString.Add(CommonValues.KeyName.Composition, "");
            _searchString.Add(CommonValues.KeyName.BurnCount, "");
            _searchString.Add(CommonValues.KeyName.Contents, "");
            _searchString.Add(CommonValues.KeyName.YarnType, "");
            _searchString.Add(CommonValues.KeyName.IsUse, "1");
            _searchString.Add(CommonValues.KeyName.Remark, "");
            _ds1 = Controller.Yarn.Getlist(_searchString); 

            if (_ds1 != null && _ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in _ds1.Tables[0].Rows)
                {
                    lstYarn1.Add(new Controller.Yarn(Convert.ToInt32(row["Idx"]),
                                                    row["YarnName"].ToString(),
                                                    row["Composition"].ToString(),
                                                    row["BurnCount"].ToString(),
                                                    row["YarnType"].ToString(),
                                                    row["Contents1"].ToString(),
                                                    Convert.ToDouble(row["Percent1"]),
                                                    row["Contents2"].ToString(),
                                                    row["Contents3"].ToString(),
                                                    row["Contents4"].ToString(),
                                                    Convert.ToDouble(row["Percent2"]),
                                                    Convert.ToDouble(row["Percent3"]),
                                                    Convert.ToDouble(row["Percent4"]),
                                                    row["Remark"].ToString()));
                    lstYarn2.Add(new Controller.Yarn(Convert.ToInt32(row["Idx"]),
                                                    row["YarnName"].ToString(),
                                                    row["Composition"].ToString(),
                                                    row["BurnCount"].ToString(),
                                                    row["YarnType"].ToString(),
                                                    row["Contents1"].ToString(),
                                                    Convert.ToDouble(row["Percent1"]),
                                                    row["Contents2"].ToString(),
                                                    row["Contents3"].ToString(),
                                                    row["Contents4"].ToString(),
                                                    Convert.ToDouble(row["Percent2"]),
                                                    Convert.ToDouble(row["Percent3"]),
                                                    Convert.ToDouble(row["Percent4"]),
                                                    row["Remark"].ToString()));
                    lstYarn3.Add(new Controller.Yarn(Convert.ToInt32(row["Idx"]),
                                                    row["YarnName"].ToString(),
                                                    row["Composition"].ToString(),
                                                    row["BurnCount"].ToString(),
                                                    row["YarnType"].ToString(),
                                                    row["Contents1"].ToString(),
                                                    Convert.ToDouble(row["Percent1"]),
                                                    row["Contents2"].ToString(),
                                                    row["Contents3"].ToString(),
                                                    row["Contents4"].ToString(),
                                                    Convert.ToDouble(row["Percent2"]),
                                                    Convert.ToDouble(row["Percent3"]),
                                                    Convert.ToDouble(row["Percent4"]),
                                                    row["Remark"].ToString()));
                    lstYarn4.Add(new Controller.Yarn(Convert.ToInt32(row["Idx"]),
                                                    row["YarnName"].ToString(),
                                                    row["Composition"].ToString(),
                                                    row["BurnCount"].ToString(),
                                                    row["YarnType"].ToString(),
                                                    row["Contents1"].ToString(),
                                                    Convert.ToDouble(row["Percent1"]),
                                                    row["Contents2"].ToString(),
                                                    row["Contents3"].ToString(),
                                                    row["Contents4"].ToString(),
                                                    Convert.ToDouble(row["Percent2"]),
                                                    Convert.ToDouble(row["Percent3"]),
                                                    Convert.ToDouble(row["Percent4"]),
                                                    row["Remark"].ToString()));
                    lstYarn5.Add(new Controller.Yarn(Convert.ToInt32(row["Idx"]),
                                                    row["YarnName"].ToString(),
                                                    row["Composition"].ToString(),
                                                    row["BurnCount"].ToString(),
                                                    row["YarnType"].ToString(),
                                                    row["Contents1"].ToString(),
                                                    Convert.ToDouble(row["Percent1"]),
                                                    row["Contents2"].ToString(),
                                                    row["Contents3"].ToString(),
                                                    row["Contents4"].ToString(),
                                                    Convert.ToDouble(row["Percent2"]),
                                                    Convert.ToDouble(row["Percent3"]),
                                                    Convert.ToDouble(row["Percent4"]),
                                                    row["Remark"].ToString()));

                }
            }

            //// Composition
            //_dt = Data.YarnData.Getlist_Composition();

            //if (_dt != null)
            //{
            //    foreach (DataRow row in _dt.Rows)
            //    {
            //        lstComposition.Add(new CodeContents(0, row["Composition"].ToString(), ""));
            //        lstComposition2.Add(new CodeContents(0, row["Composition"].ToString(), ""));
            //    }
            //}

            //// Burncount
            //_dt = Data.YarnData.Getlist_Burncount();

            //if (_dt != null)
            //{
            //    foreach (DataRow row in _dt.Rows)
            //    {
            //        lstBurnCount.Add(new CodeContents(0, row["Burncount"].ToString(), ""));
            //        lstBurnCount2.Add(new CodeContents(0, row["Burncount"].ToString(), ""));
            //    }
            //}

            //// Yarn Contents
            //_dt = Data.YarnData.Getlist_YarnContents();

            //if (_dt != null)
            //{
            //    foreach (DataRow row in _dt.Rows)
            //    {
            //        lstContents.Add(new CodeContents(0, row["Contents"].ToString(), ""));
            //        lstContents1.Add(new CodeContents(0, row["Contents"].ToString(), ""));
            //        lstContents2.Add(new CodeContents(0, row["Contents"].ToString(), ""));
            //        lstContents3.Add(new CodeContents(0, row["Contents"].ToString(), ""));
            //        lstContents4.Add(new CodeContents(0, row["Contents"].ToString(), ""));

            //    }
            //}

            // 오더상태 (CommonValues정의)
            lstIsUse.Add(new CodeContents(2, "", ""));
            lstIsUse.Add(new CodeContents(1, "Yes", "1"));
            lstIsUse.Add(new CodeContents(0, "No", "0"));
            
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
                if (ddlUse.SelectedValue != null
                    || !string.IsNullOrEmpty(txtFabric.Text) 
                    )
                {
                    _searchString = new Dictionary<CommonValues.KeyName, string>();
                    _searchString.Add(CommonValues.KeyName.IsUse, ddlUse.SelectedValue.ToString());
                    _searchString.Add(CommonValues.KeyName.Remark, txtFabric.Text.Trim());

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
        private void DataBinding_GV1()
        {
            try
            {
                _gv1.DataSource = null;
                
                _ds1 = Dev.Controller.Fabric.Getlist(_searchString);

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
                _obj1 = new Controller.Fabric(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value));
                _obj1.Idx = Convert.ToInt32(row.Cells["Idx"].Value.ToString());
                if (row.Cells["LongName"].Value != DBNull.Value) _obj1.LongName = row.Cells["LongName"].Value.ToString();
                if (row.Cells["ShortName"].Value != DBNull.Value) _obj1.ShortName = row.Cells["ShortName"].Value.ToString();

                if (row.Cells["Yarn1"].Value != DBNull.Value) _obj1.Yarn1 = Convert.ToInt32(row.Cells["Yarn1"].Value.ToString());
                if (row.Cells["Yarn2"].Value != DBNull.Value) _obj1.Yarn2 = Convert.ToInt32(row.Cells["Yarn2"].Value.ToString());
                if (row.Cells["Yarn3"].Value != DBNull.Value) _obj1.Yarn3 = Convert.ToInt32(row.Cells["Yarn3"].Value.ToString());
                if (row.Cells["Yarn4"].Value != DBNull.Value) _obj1.Yarn4 = Convert.ToInt32(row.Cells["Yarn4"].Value.ToString());
                if (row.Cells["Yarn5"].Value != DBNull.Value) _obj1.Yarn5 = Convert.ToInt32(row.Cells["Yarn5"].Value.ToString());

                if (row.Cells["Percent1"].Value != DBNull.Value) _obj1.Percent1 = Convert.ToDouble(row.Cells["Percent1"].Value.ToString());
                if (row.Cells["Percent2"].Value != DBNull.Value) _obj1.Percent2 = Convert.ToDouble(row.Cells["Percent2"].Value.ToString());
                if (row.Cells["Percent3"].Value != DBNull.Value) _obj1.Percent3 = Convert.ToDouble(row.Cells["Percent3"].Value.ToString());
                if (row.Cells["Percent4"].Value != DBNull.Value) _obj1.Percent4 = Convert.ToDouble(row.Cells["Percent4"].Value.ToString());
                if (row.Cells["Percent5"].Value != DBNull.Value) _obj1.Percent5 = Convert.ToDouble(row.Cells["Percent5"].Value.ToString());

                if (row.Cells["IsUse"].Value != DBNull.Value) _obj1.IsUse = Convert.ToInt32(row.Cells["IsUse"].Value);

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
            RadMultiColumnComboBoxElement meditor = e.ActiveEditor as RadMultiColumnComboBoxElement;
            if (meditor != null)
            {
                meditor.AutoSizeDropDownToBestFit = true;
                meditor.AutoSizeDropDownHeight = true;

                if (meditor.ValueMember == "Idx")
                {
                    meditor.EditorControl.Columns["Idx"].HeaderText = "ID";
                    meditor.EditorControl.Columns["YarnName"].HeaderText = "Yarn";
                    meditor.EditorControl.Columns["YarnName"].IsVisible = false;
                    meditor.EditorControl.Columns["Composition"].HeaderText = "Composition";
                    meditor.EditorControl.Columns["BurnCount"].HeaderText = "Burn";
                    meditor.EditorControl.Columns["YarnType"].HeaderText = "Type";
                    meditor.EditorControl.Columns["Contents1"].HeaderText = "Contents1";
                    meditor.EditorControl.Columns["Percent1"].HeaderText = "%";
                    
                    meditor.EditorControl.Columns["Contents2"].HeaderText = "Contents2";
                    meditor.EditorControl.Columns["Percent2"].HeaderText = "%";
                    meditor.EditorControl.Columns["Contents3"].HeaderText = "Contents3";
                    meditor.EditorControl.Columns["Percent3"].HeaderText = "%";
                    meditor.EditorControl.Columns["Contents4"].HeaderText = "Contents4";
                    meditor.EditorControl.Columns["Percent4"].HeaderText = "%";
                    meditor.EditorControl.Columns["Remark"].HeaderText = "Remark";
                    meditor.EditorControl.Columns["IsUse"].IsVisible = false;

                    meditor.EditorControl.Columns.Move(10, 6);
                    meditor.EditorControl.Columns.Move(11, 8);
                    meditor.EditorControl.Columns.Move(12, 10);
                }
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
