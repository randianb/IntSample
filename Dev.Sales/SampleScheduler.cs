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

namespace Dev.Sales
{
    /// <summary>
    /// 샘플오더 관리화면: 오더입력수정 및 작업지시등
    /// </summary>
    public partial class SampleScheduler : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                             // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, int> _searchKey;                // 쿼리값 value, 쿼리항목 key로 전달 
        public DataRow InsertedOrderRow = null;

        private bool _bRtn;                                                     // 쿼리결과 리턴
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        private DataTable _dt = null;                                           // 기본 데이터테이블
        private RadContextMenu contextMenu;                                     // 컨텍스트 메뉴
        private List<CodeContents> lstStatus = new List<CodeContents>();        // 오더상태
        private List<CustomerName> custName = new List<CustomerName>();         // 거래처
        private List<CodeContents> codeName = new List<CodeContents>();         // 코드
        private List<CodeContents> sizeName = new List<CodeContents>();
        private int _orderIdx; 
        
        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public SampleScheduler(InheritMDI main, int OrderIdx)
        {
            base.InitializeComponent(); // parent 컴포넌트에 접근하기 위해 컨트롤을 public으로 지정 > 향후 수정 필요 (todo) 
            InitializeComponent();
            __main__ = main;            // MDI 연결 
            _orderIdx = OrderIdx;

            this.gvWork.DragDropService.PreviewDragDrop += DragDropService_PreviewDragDrop;
            this.gvWork.DragDropService.Stopped += DragDropService_Stopped;
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
            GV1_Create_Columns(gvWork);

            _searchKey = new Dictionary<CommonValues.KeyName, int>();
            _searchKey.Add(CommonValues.KeyName.OrderIdx, _orderIdx);
            _searchKey.Add(CommonValues.KeyName.OperationIdx, 0);
            _searchKey.Add(CommonValues.KeyName.Status, 0);

            DataBinding_GV1(_searchKey, "", "");   // 중앙 그리드뷰 데이터 
            
            // 다른 폼으로부터 전달된 Work ID가 있을 경우, 해당 ID로 조회 
            //if (!string.IsNullOrEmpty(_workOrderIdx))
            //{
            //    _searchKey = new Dictionary<CommonValues.KeyName, int>();
            //    _searchKey.Add(CommonValues.KeyName.CustIdx, 0);
            //    _searchKey.Add(CommonValues.KeyName.Status, 0);
            //    _searchKey.Add(CommonValues.KeyName.Size, 0);

            //    DataBinding_GV1(2, _searchKey, _workOrderIdx, txtStyle.Text.Trim());
            //}
        }
        
        /// <summary>
        /// 상단 검색을 위한 Dropdownlist 생성
        /// </summary>
        private void Config_DropDownList()
        {
            // 사이즈 
            ddlSize.DataSource = sizeName;
            ddlSize.DisplayMember = "Contents";
            ddlSize.ValueMember = "CodeIdx";
            ddlSize.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlSize.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 바이어
            ddlCust.DataSource = custName;
            ddlCust.DisplayMember = "CustName";
            ddlCust.ValueMember = "CustIdx";
            ddlCust.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlCust.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 오더 상태 
            ddlStatus.DataSource = lstStatus;
            ddlStatus.DisplayMember = "Contents";
            ddlStatus.ValueMember = "CodeIdx";
            ddlStatus.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlStatus.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            
        }
        
        
        #endregion

        #region 3. 컨트롤 초기 설정
        
        private void GV1_Create_Columns(RadGanttView gv)
        {
            gv.GanttViewElement.GraphicalViewElement.TimelineStart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //this.radGanttView1.GanttViewElement.GraphicalViewElement.TimelineEnd = new DateTime(2017, 11, 30);

            GanttViewTextViewColumn titleColumn = new GanttViewTextViewColumn("Title", "Scheduled Work");
            titleColumn.Width = 200;
            GanttViewTextViewColumn startColumn = new GanttViewTextViewColumn("Start");
            startColumn.FormatString = "{0:d}";
            GanttViewTextViewColumn endColumn = new GanttViewTextViewColumn("End");
            endColumn.FormatString = "{0:d}";
                        
            gv.GanttViewElement.Columns.Add(titleColumn);
            gv.GanttViewElement.Columns.Add(startColumn);
            gv.GanttViewElement.Columns.Add(endColumn);
            gv.GanttViewElement.GraphicalViewElement.TimelineRange = TimeRange.Month;
            gv.GanttViewElement.GraphicalViewElement.LinksHandlesSize = new Size(0, 0);

            //this.radGanttView1.ReadOnly = true; 
            GanttViewTodayIndicatorElement todayIndicator = gv.GanttViewElement.GraphicalViewElement.TodayIndicatorElement;
            todayIndicator.BackColor = Color.Red;
            todayIndicator.BackColor2 = Color.Red;
            GanttViewTimelineTodayIndicatorElement timelineTodayIndicator = gv.GanttViewElement.GraphicalViewElement.TimelineTodayIndicatorElement;
            timelineTodayIndicator.HorizontalLineWidth = 1;
            timelineTodayIndicator.BackColor = Color.Red;
            timelineTodayIndicator.BackColor2 = Color.Red;
                        
            gv.GanttViewBehavior = new MyBaseGanttViewBehavior();
            
        }

        #endregion


        #region 5. 데이터 조회

        private void GV1_Create_Rows(RadGanttView gv)
        {
            gvWork.Items.Clear();

            if (_ds1 != null)
            {
                GanttViewDataItem item = null;
                Console.WriteLine(_ds1.Tables[0].Rows.Count.ToString()); 

                foreach (DataRow row in _ds1.Tables[0].Rows)
                {
                    item = new GanttViewDataItem();
                    item.Start = Convert.ToDateTime(row["Start"]);
                    if (row["End"] != DBNull.Value) item.End =  Convert.ToDateTime(row["End"]);
                    if (row["Progress"] != DBNull.Value) item.Progress = Convert.ToDecimal(row["Progress"]); 
                    item.Title =  row["Title"].ToString() + " / " + row["WorkOrderIdx"].ToString();
                    item.Tag = row["OperationIdx"].ToString();

                    this.gvWork.Items.Add(item); 
                }
            }
        }

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


            // 오더상태 (CommonValues정의)
            lstStatus.Add(new CodeContents(0, CommonValues.DicOrderStatus[0], ""));
            lstStatus.Add(new CodeContents(1, CommonValues.DicOrderStatus[1], ""));
            lstStatus.Add(new CodeContents(2, CommonValues.DicOrderStatus[2], ""));
            lstStatus.Add(new CodeContents(3, CommonValues.DicOrderStatus[3], ""));

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
                    || ddlStatus.SelectedValue != null || ddlSize.SelectedValue != null
                    || !string.IsNullOrEmpty(txtFileno.Text) || !string.IsNullOrEmpty(txtStyle.Text))
                {
                    _searchKey = new Dictionary<CommonValues.KeyName, int>();

                    // 영업부인경우, 해당 부서만 조회할수 있도록 제한 
                    if (UserInfo.ReportNo < 9)
                        _searchKey.Add(CommonValues.KeyName.DeptIdx, UserInfo.DeptIdx);
                    else
                        _searchKey.Add(CommonValues.KeyName.DeptIdx, Convert.ToInt32(ddlSize.SelectedValue));

                    _searchKey.Add(CommonValues.KeyName.CustIdx, Convert.ToInt32(ddlCust.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Status, Convert.ToInt32(ddlStatus.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Size, Convert.ToInt32(ddlSize.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.OrderIdx, 0);
                    _searchKey.Add(CommonValues.KeyName.OperationIdx, 0);

                    DataBinding_GV1(_searchKey, txtFileno.Text.Trim(), txtStyle.Text.Trim());
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
        private void DataBinding_GV1(Dictionary<CommonValues.KeyName, int> SearchKey, string fileno, string TicketDate)
        {
            try
            {
                gvWork.DataSource = null;

                _ds1 = Dev.Controller.WorkOrder.Getlist2(SearchKey, fileno, TicketDate);
                if (_ds1 != null)
                {
                    //_gv1.DataSource = _ds1.Tables[0].DefaultView;
                    //__main__.lblRows.Text = _gv1.RowCount.ToString() + " Rows";
                    //_gv1.EnablePaging = CommonValues.enablePaging;
                    //_gv1.AllowSearchRow = CommonValues.enableSearchRow;

                    GV1_Create_Rows(gvWork);
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

                // 객체생성 및 값 할당
                //_obj1 = new Controller.Pattern(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value));
                //_obj1.Idx = Convert.ToInt32(row.Cells["Idx"].Value.ToString());
                //_obj1.Fileno = row.Cells["Fileno"].Value.ToString();
                //_obj1.DeptIdx = Convert.ToInt32(row.Cells["DeptIdx"].Value.ToString());
                //if (row.Cells["Reorder"].Value != DBNull.Value) _obj1.Reorder = Convert.ToInt32(row.Cells["Reorder"].Value.ToString());
                //if (row.Cells["ReorderReason"].Value != DBNull.Value) _obj1.ReorderReason = row.Cells["ReorderReason"].Value.ToString();
                //if (row.Cells["Indate"].Value != DBNull.Value) _obj1.Indate = Convert.ToDateTime(row.Cells["Indate"].Value);
                
                //if (row.Cells["Remark"].Value != DBNull.Value) _obj1.Remark = row.Cells["Remark"].Value.ToString();
                //if (row.Cells["TeamRequestedDate"].Value != DBNull.Value) _obj1.TeamRequestedDate = Convert.ToDateTime(row.Cells["TeamRequestedDate"].Value);
                //if (row.Cells["SplConfirmedDate"].Value != DBNull.Value) _obj1.SplConfirmedDate = Convert.ToDateTime(row.Cells["SplConfirmedDate"].Value);

                //if (row.Cells["Status"].Value != DBNull.Value) _obj1.Status = Convert.ToInt32(row.Cells["Status"].Value.ToString());

                //// 업데이트 (오더캔슬, 선적완료 상태가 아닐경우)
                //if (_obj1.Status != 2 && _obj1.Status != 3) _bRtn = _obj1.Update();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("GV1_Update: " + ex.Message.ToString());
            }
        }

        #region GanttView Task Drag and drop 

        private void DragDropService_Stopped(object sender, EventArgs e)
        {
            if (data != null)
            {
                string[] strTitle = data.Title.Split('/');
                bool result = Dev.Controller.WorkOrder.Update(strTitle[2].ToString().Trim(), data.Start, data.End, 0.1, Dev.Options.UserInfo.Idx);
            }
        }
        GanttViewDataItem data;
        private void DragDropService_PreviewDragDrop(object sender, RadDropEventArgs e)
        {
            GanttViewTaskElement draggedTaskElement = e.DragInstance as GanttViewTaskElement;
            if (draggedTaskElement != null)
            {
                data = ((GanttGraphicalViewBaseItemElement)draggedTaskElement.Parent).Data;
            }
        }

        #endregion 

        #endregion

        #region 7. 기능 멤버 (그리드뷰 레이아웃 저장)

        #endregion

        private void gvWork_TextViewItemFormatting(object sender, GanttViewTextViewItemFormattingEventArgs e)
        {
            if (e.Item.Tag.ToString() == "110")
            {
                e.ItemElement.DrawFill = true;
                e.ItemElement.BackColor = Color.LightGreen;
                e.ItemElement.GradientStyle = GradientStyles.Solid;
            }
            else if (e.Item.Tag.ToString() == "111")
            {
                e.ItemElement.DrawFill = true;
                e.ItemElement.BackColor = Color.Yellow;
                e.ItemElement.GradientStyle = GradientStyles.Solid;
            }
            else
            {
                e.ItemElement.ResetValue(LightVisualElement.DrawBorderProperty, ValueResetFlags.Local);
                e.ItemElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                e.ItemElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
            }
        }

        private void gvWork_SelectedItemChanged(object sender, GanttViewSelectedItemChangedEventArgs e)
        {
            
        }

        private void gvWork_SelectedItemChanging(object sender, GanttViewSelectedItemChangingEventArgs e)
        {
            
        }

        private void gvWork_DragDrop(object sender, DragEventArgs e)
        {
            //string[] strTitle = gvWork.SelectedItem.Title.Split('/');
            //Console.WriteLine(strTitle[1] + ", " + gvWork.SelectedItem.Start + ", " + gvWork.SelectedItem.End);
        }
        
        /// <summary>
        /// 스케쥴러 아이템 변경시 업데이트
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvWork_ItemChanged(object sender, GanttViewItemChangedEventArgs e)
        {
            try
            {
                string[] strTitle = gvWork.SelectedItem.Title.Split('/');

                // 패턴 
                if (gvWork.SelectedItem.Tag.ToString().Trim() == "110")
                {
                    bool result = Dev.Controller.WorkOrder.Update(strTitle[2].ToString().Trim(),
                            gvWork.SelectedItem.Start, gvWork.SelectedItem.End, 0.1, Dev.Options.UserInfo.Idx);
                }
                // 재단
                else if (gvWork.SelectedItem.Tag.ToString().Trim() == "111")
                {
                    bool result = Dev.Controller.WorkOrder.Update(strTitle[3].ToString().Trim(),
                            gvWork.SelectedItem.Start, gvWork.SelectedItem.End, 0.1, Dev.Options.UserInfo.Idx);
                }

            }
            catch(Exception ex)
            {
                RadMessageBox.Show(ex.Message); 
            }
        }

        private void gvWork_ContextMenuOpening(object sender, GanttViewContextMenuOpeningEventArgs e)
        {
            e.Cancel = true; 
        }
    }
}
