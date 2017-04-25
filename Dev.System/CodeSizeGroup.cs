using Int.Code;
using Int.Customer;
using Int.Department;
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
    /// 샘플오더 관리화면: 오더입력수정 및 작업지시등
    /// </summary>
    public partial class CodeSizeGroup : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                     // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, int> _searchKey;        // 쿼리값 value, 쿼리항목 key로 전달 
        private bool _bRtn;                                             // 쿼리결과 리턴
        private DataSet _ds1 = null;                                    // 기본 데이터셋
        private DataTable _dt = null;                                   // 기본 데이터테이블
        private Controller.SizeGroup _obj1 = null;                               // 현재 생성된 객체 
        private RadContextMenu contextMenu;                             // 컨텍스트 메뉴
        private List<CustomerName> custName = new List<CustomerName>();     // 제조사
        private List<CustomerName> custNameDDL = new List<CustomerName>();     // 제조사
        //private List<CodeContents> sizeName = new List<CodeContents>();     // 컬러명
        private Dictionary<int, List<CodeContents>> sizeName =
            new Dictionary<int, List<CodeContents>>(); 
        private string _layoutfile = "/GVLayoutSizeGroup.xml";

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public CodeSizeGroup(InheritMDI main)
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
        private void frmCodeSizeGroup_Load(object sender, EventArgs e)
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
        /// 상단 검색을 위한 Dropdownlist 생성
        /// </summary>
        private void Config_DropDownList()
        {
            // 바이어
            ddlCust.DataSource = custNameDDL;
            ddlCust.DisplayMember = "CustName";
            ddlCust.ValueMember = "CustIdx";
            ddlCust.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlCust.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

        }

        /// <summary>
        /// 그리드뷰 컨텍스트 메뉴 생성  
        /// </summary>
        private void Config_ContextMenu()
        {
            contextMenu = new RadContextMenu();

            // 오더 신규 입력
            RadMenuItem mnuNew = new RadMenuItem("New Size Group");
            mnuNew.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.N));
            mnuNew.Click += new EventHandler(mnuNew_Click);

            // 오더 삭제
            RadMenuItem mnuDel = new RadMenuItem("Remove Size Group");
            mnuDel.Shortcuts.Add(new RadShortcut(Keys.Control, Keys.D));
            mnuDel.Click += new EventHandler(mnuDel_Click);

            // 열 숨기기
            RadMenuItem mnuHide = new RadMenuItem("Hide Column");
            mnuHide.Click += new EventHandler(mnuHide_Click);

            // 열 보이기
            RadMenuItem mnuShow = new RadMenuItem("Show all Columns");
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
        /// 그리드뷰 컬럼 생성
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV1_CreateColumn(RadGridView gv)
        {
            #region Columns 생성
            
            GridViewTextBoxColumn cSizeGroupIdx = new GridViewTextBoxColumn();
            cSizeGroupIdx.DataType = typeof(int);
            cSizeGroupIdx.Name = "SizeGroupIdx";
            cSizeGroupIdx.FieldName = "SizeGroupIdx";
            cSizeGroupIdx.HeaderText = "ID";
            cSizeGroupIdx.Width = 50;
            cSizeGroupIdx.TextAlignment = ContentAlignment.MiddleLeft;
            gv.Columns.Add(cSizeGroupIdx);

            GridViewComboBoxColumn cClient = new GridViewComboBoxColumn();
            cClient.Name = "Client";
            cClient.DataSource = custName;
            cClient.ValueMember = "CustIdx";
            cClient.DisplayMember = "CustName";
            cClient.FieldName = "Client";
            cClient.HeaderText = "Buyer";
            cClient.Width = 100;
            cClient.TextAlignment = ContentAlignment.MiddleLeft;
            gv.Columns.Add(cClient);

            GridViewTextBoxColumn cSizeGroupName = new GridViewTextBoxColumn();
            cSizeGroupName.Name = "SizeGroupName";
            cSizeGroupName.FieldName = "SizeGroupName";
            cSizeGroupName.HeaderText = "Size Group";
            cSizeGroupName.Width = 200;
            cSizeGroupName.TextAlignment = ContentAlignment.MiddleLeft;
            gv.Columns.Add(cSizeGroupName);

            for (int i = 1; i <= 8; i++)
            {
                GridViewComboBoxColumn cSize = new GridViewComboBoxColumn();
                cSize.Name = "SizeIdx" + i;
                cSize.DataSource = sizeName[i];
                cSize.ValueMember = "CodeIdx";
                cSize.DisplayMember = "Contents";
                cSize.FieldName = "SizeIdx" + i;
                cSize.HeaderText = "Size" + i;
                cSize.Width = 100;
                cSize.TextAlignment = ContentAlignment.MiddleLeft;
                gv.Columns.Add(cSize);
            }
                            
            GridViewCheckBoxColumn cIsUse = new GridViewCheckBoxColumn();
            cIsUse.DataType = typeof(int);
            cIsUse.Name = "IsUse";
            cIsUse.FieldName = "IsUse";
            cIsUse.HeaderText = "Use";
            cIsUse.WrapText = true;
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
                    if (string.IsNullOrEmpty(str)) str = Convert.ToInt32(row.Cells["SizeGroupIdx"].Value).ToString();
                    else str += "," + Convert.ToInt32(row.Cells["SizeGroupIdx"].Value).ToString();
                }

                // 해당 자료 삭제여부 확인후, 
                if (RadMessageBox.Show("Do you want to delete this item?\n(ID: " + str + ")", "Confirm",
                    MessageBoxButtons.YesNo, RadMessageIcon.Question) == DialogResult.Yes)
                {
                    // 삭제후 새로고침
                    _bRtn = Controller.SizeGroup.Delete(str);
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
                DataRow row = Controller.SizeGroup.Insert(0,"", 0, 0, 0, 0, 0, 0, 0, 0, 1);

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
            try
            {
                // 제조사 
                _dt = CommonController.Getlist(CommonValues.KeyName.CustIdx).Tables[0];
                
                foreach (DataRow row in _dt.Rows)
                {
                    custName.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                                row["CustName"].ToString(),
                                                Convert.ToInt32(row["Classification"])));
                    custNameDDL.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                                row["CustName"].ToString(),
                                                Convert.ToInt32(row["Classification"])));
                }

                // 사이즈명 
                _dt = Controller.Sizes.GetUselist().Tables[0];
                for (int i = 1; i <= 8; i++)
                {
                    sizeName[i] = new List<CodeContents>(); 

                    foreach (DataRow row in _dt.Rows)
                    {
                        
                        sizeName[i].Add(new CodeContents(Convert.ToInt32(row["SizeIdx"]),
                                                row["SizeName"].ToString(),
                                                ""));
                    }
                }
                
            }
            catch(Exception ex)
            {
                RadMessageBox.Show(ex.Message); 
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
                DataBinding_GV1(Convert.ToInt32(ddlCust.SelectedValue));
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
        private void DataBinding_GV1(int CustIdx)
        {
            try
            {
                //조회시간
                //Stopwatch sw = new Stopwatch();
                //sw.Start(); 

                _gv1.DataSource = null;
                
                _ds1 = Controller.SizeGroup.Getlist(CustIdx);
                
                if (_ds1 != null)
                {
                    _gv1.DataSource = _ds1.Tables[0].DefaultView;
                    // 조회 후, 상태알림 및 설정적용
                    __main__.lblRows.Text = _gv1.RowCount.ToString() + " Rows"; 
                    _gv1.EnablePaging = CommonValues.enablePaging;
                    _gv1.AllowSearchRow = CommonValues.enableSearchRow; 
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
                _obj1           = new Controller.SizeGroup(Convert.ToInt32(row.Cells["SizeGroupIdx"].Value));
                _obj1.SizeGroupIdx       = Convert.ToInt32(row.Cells["SizeGroupIdx"].Value);
                if (row.Cells["Client"].Value != DBNull.Value) _obj1.Client = Convert.ToInt32(row.Cells["Client"].Value);
                _obj1.SizeGroupName = row.Cells["SizeGroupName"].Value.ToString();
                if (row.Cells["SizeIdx1"].Value != DBNull.Value)  _obj1.SizeIdx1 = Convert.ToInt32(row.Cells["SizeIdx1"].Value);
                if (row.Cells["SizeIdx2"].Value != DBNull.Value) _obj1.SizeIdx2 = Convert.ToInt32(row.Cells["SizeIdx2"].Value);
                if (row.Cells["SizeIdx3"].Value != DBNull.Value) _obj1.SizeIdx3 = Convert.ToInt32(row.Cells["SizeIdx3"].Value);
                if (row.Cells["SizeIdx4"].Value != DBNull.Value) _obj1.SizeIdx4 = Convert.ToInt32(row.Cells["SizeIdx4"].Value);
                if (row.Cells["SizeIdx5"].Value != DBNull.Value) _obj1.SizeIdx5 = Convert.ToInt32(row.Cells["SizeIdx5"].Value);
                if (row.Cells["SizeIdx6"].Value != DBNull.Value) _obj1.SizeIdx6 = Convert.ToInt32(row.Cells["SizeIdx6"].Value);
                if (row.Cells["SizeIdx7"].Value != DBNull.Value) _obj1.SizeIdx7 = Convert.ToInt32(row.Cells["SizeIdx7"].Value);
                if (row.Cells["SizeIdx8"].Value != DBNull.Value) _obj1.SizeIdx8 = Convert.ToInt32(row.Cells["SizeIdx8"].Value);

                _obj1.IsUse    = Convert.ToInt32(row.Cells["IsUse"].Value);
                
                _bRtn=_obj1.Update();
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
            }

            // 날짜컬럼의 달력크기 설정
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

        #region 7. 기능 멤버

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
