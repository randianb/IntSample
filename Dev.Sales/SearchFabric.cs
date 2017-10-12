using Dev.Options;
using Int.Code;
using Int.Customer;
using Int.Department;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Dev.Sales
{
    public partial class SearchFabric : Telerik.WinControls.UI.RadForm
    {
        #region 변수 선언

        private RadGridView _gv1 = null; 
        private DataTable _dt = null;                                           // 기본 데이터테이블
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        public Dictionary<CommonValues.KeyName, string> _searchString;        // 쿼리값 value, 쿼리항목 key로 전달 

        public Dictionary<CommonValues.KeyName, int> _searchKey;                // 쿼리값 value, 쿼리항목 key로 전달 
        private List<DepartmentName> deptName = new List<DepartmentName>();     // 부서
        private List<CodeContents> lstIsUse = new List<CodeContents>();        // 
        
        private string _worderIdx, _fileNo, _styleNo = "";
        private int _orderIdx, _orderStatus, _sizeGroup = 0;
        private bool _bRtn = false;
        private List<Codes.Controller.Sizes> lstSize = new List<Codes.Controller.Sizes>();
        private List<CustomerName> custName = new List<CustomerName>();         // 거래처

        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 18, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        #endregion

        #region 초기화

        /// <summary>
        /// parent로부터 order 정보 받아오기 
        /// </summary>
        /// <param name="idx">고유번호</param>
        /// <param name="fileNo">파일번호</param>
        /// <param name="qty">수량</param>
        /// <param name="amount">금액</param>
        /// <param name="shipCompleted">선적완료 여부</param>
        public SearchFabric(int idx)
        {
            InitializeComponent();
            _orderIdx = idx;
            
            this.Text += " (ID#" + _orderIdx.ToString() + ")"; 
            this.StartPosition = FormStartPosition.CenterScreen;
            _gv1 = this.gvMain;  // 그리드뷰 일반화를 위해 변수 별도 저장

            // 선적완료, 오더마감 여부에 따라 편집 disable 
            if (_orderStatus==2 || _orderStatus==3)
            {
                tableLayoutPanel2.Enabled = false; 
            }
        }

        private void frmPatternRequest_Load(object sender, EventArgs e)
        {
            DataLoading_DDL();          // DDL 데이터 로딩
            Config_DropDownList();      // 상단 DDL 생성 설정
            GV1_CreateColumn(_gv1);     // 그리드뷰 생성
            GV1_LayoutSetting(_gv1);    // 중앙 그리드뷰 설정 
            txtFabric.Select(); 
        }

        /// <summary>
        /// 상단 검색을 위한 Dropdownlist 생성
        /// </summary>
        private void Config_DropDownList()
        {
            //// 부서 
            //ddlDept.DataSource = deptName;
            //ddlDept.DisplayMember = "DeptName";
            //ddlDept.ValueMember = "DeptIdx";
            //ddlDept.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            //ddlDept.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            //// 바이어
            //ddlCust.DataSource = custName;
            //ddlCust.DisplayMember = "CustName";
            //ddlCust.ValueMember = "CustIdx";
            //ddlCust.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            //ddlCust.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 오더상태 (CommonValues정의)
            lstIsUse.Add(new CodeContents(2, "", ""));
            lstIsUse.Add(new CodeContents(1, "Yes", "1"));
            lstIsUse.Add(new CodeContents(0, "No", "0"));

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
            Idx.ReadOnly = true;
            Idx.TextAlignment = ContentAlignment.MiddleLeft;
            gv.Columns.Add(Idx);

            GridViewTextBoxColumn LongName = new GridViewTextBoxColumn();
            LongName.Name = "LongName";
            LongName.FieldName = "LongName";
            LongName.HeaderText = "Long Name";
            LongName.TextAlignment = ContentAlignment.MiddleLeft;
            LongName.Width = 300;
            LongName.ReadOnly = true;
            gv.Columns.Add(LongName);

            GridViewTextBoxColumn ShortName = new GridViewTextBoxColumn();
            ShortName.Name = "ShortName";
            ShortName.FieldName = "ShortName";
            ShortName.HeaderText = "Short Name";
            ShortName.TextAlignment = ContentAlignment.MiddleLeft;
            ShortName.Width = 150;
            ShortName.ReadOnly = true;
            gv.Columns.Add(ShortName);
            
            GridViewCheckBoxColumn cIsUse = new GridViewCheckBoxColumn();
            cIsUse.DataType = typeof(int);
            cIsUse.Name = "IsUse";
            cIsUse.FieldName = "IsUse";
            cIsUse.HeaderText = "Use";
            cIsUse.Width = 40;
            cIsUse.ReadOnly = true; 
            gv.Columns.Add(cIsUse);

            #endregion
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

        /// <summary>
        /// Dropdownlist 데이터 로딩
        /// </summary>
        private void DataLoading_DDL()
        {
            try
            {
                // 오더상태 (CommonValues정의)
                lstIsUse.Add(new CodeContents(2, "", ""));
                lstIsUse.Add(new CodeContents(1, "Yes", "1"));
                lstIsUse.Add(new CodeContents(0, "No", "0"));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

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
                //if (!string.IsNullOrEmpty(txtFabric.Text))
                //{
                    _searchString = new Dictionary<CommonValues.KeyName, string>();
                    _searchString.Add(CommonValues.KeyName.IsUse, "");
                    _searchString.Add(CommonValues.KeyName.Remark, txtFabric.Text.Trim());

                    DataBinding_GV1();
                //}
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

        #region 바인딩 & 이벤트

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
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
                //RadMultiColumnComboBoxElement meditor = e.ActiveEditor as RadMultiColumnComboBoxElement;

                //if (meditor != null)
                //{
                //    meditor.Enabled = false;
                //}

                // DDL 높이, 출력항목수 설정
                RadDropDownListEditor editor = this.gvMain.ActiveEditor as RadDropDownListEditor;
                if (editor != null)
                {
                    ((RadDropDownListEditorElement)((RadDropDownListEditor)this.gvMain.ActiveEditor).EditorElement).DefaultItemsCountInDropDown
                        = CommonValues.DDL_DefaultItemsCountInDropDown;
                    ((RadDropDownListEditorElement)((RadDropDownListEditor)this.gvMain.ActiveEditor).EditorElement).DropDownHeight
                        = CommonValues.DDL_DropDownHeight;
                }

                //// 날짜컬럼의 달력크기 설정
                //RadDateTimeEditor dtEditor = e.ActiveEditor as RadDateTimeEditor;
                //if (dtEditor != null)
                //{
                //    RadDateTimeEditorElement el = dtEditor.EditorElement as RadDateTimeEditorElement;
                //    //el.NullDate = new DateTime(2000, 1, 1);
                //    //el.NullText = "";
                //    el.CalendarSize = new Size(500, 400);

                //    if (el.Value.ToString().Length > 10)
                //    {
                //        Console.WriteLine(el.Value.ToString().Substring(0, 10));
                //        if (el.Value.ToString().Substring(0, 10) == "2000-01-01")
                //        {
                //            el.Value = Convert.ToDateTime(null);
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            RefleshWithCondition();
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

        private void btnSearch_Click_2(object sender, EventArgs e)
        {
            RefleshWithCondition();
        }
        
        /// <summary>
        /// 외주 요청 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = null;
                string NewCode = "";

                _gv1.EndEdit();
                GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);

                if (_gv1.Rows.Count <= 0)
                {
                    RadMessageBox.Show("Please select the Fabric", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }

                // 데이터 DB저장 
                //foreach (GridViewRowInfo row in gvMain.Rows)
                //{
                dr = Dev.Controller.OrderFabric.Insert(_orderIdx, Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value)); 
                //}

                // 입력완료 후 그리드뷰 갱신
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnUpdate_Click: " + ex.Message.ToString());
            }
        }

        public int OrderIdx
        {
            get { return _orderIdx; }
        }

        #endregion

        #region 메서드 

        /// <summary>
        /// 사이즈그룹 번호로 해당 사이즈 셋을 불러온다 
        /// </summary>
        /// <param name="SizeGroupIdx">사이즈 그룹번호</param>
        private void GetSizes(RadGridView gv)
        {
            _bRtn = false;
            try
            {
                //gvOrder.EndEdit();
                //GridViewRowInfo row = Int.Members.GetCurrentRow(gv);  // 현재 행번호 확인

                //DataRow dr = Codes.Controller.SizeGroup.Get(Convert.ToInt32(row.Cells["SizeGroupIdx"].Value));

                //if (dr != null)
                //{
                //    lstSize.Add(new Codes.Controller.Sizes(0, ""));
                //    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx1"]), dr["Size1"].ToString()));
                //    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx2"]), dr["Size2"].ToString()));
                //    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx3"]), dr["Size3"].ToString()));
                //    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx4"]), dr["Size4"].ToString()));
                //    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx5"]), dr["Size5"].ToString()));
                //    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx6"]), dr["Size6"].ToString()));
                //    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx7"]), dr["Size7"].ToString()));
                //    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx8"]), dr["Size8"].ToString()));
                //}
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }

        #endregion

    }
}
