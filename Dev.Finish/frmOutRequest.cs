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

namespace Dev.Out
{
    public partial class frmOutRequest : Telerik.WinControls.UI.RadForm
    {
        #region 변수 선언

        private DataTable _dt = null;                                           // 기본 데이터테이블
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        
        public Dictionary<CommonValues.KeyName, int> _searchKey;                // 쿼리값 value, 쿼리항목 key로 전달 
        private List<DepartmentName> deptName = new List<DepartmentName>();     // 부서
        private List<CodeContents> lstStatus = new List<CodeContents>();        // 오더상태

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
        public frmOutRequest(int idx, string worder)
        {
            InitializeComponent();
            _orderIdx = idx;
            radLabel9.Text = _orderIdx.ToString(); 
            _worderIdx = worder;
            radLabel11.Text = _worderIdx; 

            this.Text += " (ID#" + _orderIdx.ToString() + ")"; 
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // 선적완료, 오더마감 여부에 따라 편집 disable 
            if (_orderStatus==2 || _orderStatus==3)
            {
                tableLayoutPanel2.Enabled = false; 
            }
        }

        private void frmPatternRequest_Load(object sender, EventArgs e)
        {
            DataLoading_DDL();
            Config_DropDownList(); 
            GV1_CreateColumn(gvOrder);
            RefleshWithCondition();
            GV3_CreateColumn(gvOutSource); 
            
        }

        /// <summary>
        /// 상단 검색을 위한 Dropdownlist 생성
        /// </summary>
        private void Config_DropDownList()
        {
            // 부서 
            ddlDept.DataSource = deptName;
            ddlDept.DisplayMember = "DeptName";
            ddlDept.ValueMember = "DeptIdx";
            ddlDept.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlDept.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

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

        private void GV3_CreateColumn(RadGridView gv)
        {
            GridViewTextBoxColumn pIdx = new GridViewTextBoxColumn();
            pIdx.Name = "pIdx";
            pIdx.FieldName = "pIdx";
            pIdx.Width = 60;
            pIdx.TextAlignment = ContentAlignment.MiddleLeft;
            pIdx.HeaderText = "ID";
            gv.Columns.Add(pIdx);

            GridViewTextBoxColumn OrderIdx = new GridViewTextBoxColumn();
            OrderIdx.Name = "OrderIdx";
            OrderIdx.FieldName = "OrderIdx";
            OrderIdx.Width = 60;
            OrderIdx.TextAlignment = ContentAlignment.MiddleLeft;
            OrderIdx.HeaderText = "Order";
            gv.Columns.Add(OrderIdx);

            GridViewTextBoxColumn OrdColorIdx = new GridViewTextBoxColumn();
            OrdColorIdx.Name = "OrdColorIdx";
            OrdColorIdx.FieldName = "OrdColorIdx";
            OrdColorIdx.Width = 200;
            OrdColorIdx.TextAlignment = ContentAlignment.MiddleLeft;
            OrdColorIdx.HeaderText = "Color";
            gv.Columns.Add(OrdColorIdx);

            GridViewTextBoxColumn OrdSizeIdx = new GridViewTextBoxColumn();
            OrdSizeIdx.Name = "OrdSizeIdx";
            OrdSizeIdx.FieldName = "OrdSizeIdx";
            OrdSizeIdx.Width = 70;
            OrdSizeIdx.TextAlignment = ContentAlignment.MiddleLeft;
            OrdSizeIdx.HeaderText = "SizeIdx";
            //OrdSizeIdx.IsVisible = false; 
            gv.Columns.Add(OrdSizeIdx);

            GridViewTextBoxColumn OrdSizeNm = new GridViewTextBoxColumn();
            OrdSizeNm.Name = "OrdSizeNm";
            OrdSizeNm.FieldName = "OrdSizeNm";
            OrdSizeNm.Width = 80;
            OrdSizeNm.TextAlignment = ContentAlignment.MiddleLeft;
            OrdSizeNm.HeaderText = "Size";
            gv.Columns.Add(OrdSizeNm);

            GridViewTextBoxColumn OutQty = new GridViewTextBoxColumn();
            OutQty.Name = "OutQty";
            OutQty.FieldName = "OutQty";
            OutQty.Width = 70;
            OutQty.TextAlignment = ContentAlignment.MiddleRight;
            OutQty.HeaderText = "Out Q'ty";
            gv.Columns.Add(OutQty);

        }
        /// <summary>
        /// 그리드뷰 컬럼 생성 (재단완료내역)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV1_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            GridViewTextBoxColumn Idx = new GridViewTextBoxColumn();
            Idx.Name = "Idx";
            Idx.FieldName = "Idx";
            Idx.Width = 30;
            Idx.TextAlignment = ContentAlignment.MiddleLeft;
            Idx.HeaderText = "ID";
            gv.Columns.Add(Idx); 

            GridViewComboBoxColumn cboDept = new GridViewComboBoxColumn();
            cboDept.Name = "DeptIdx";
            cboDept.DataSource = deptName;
            cboDept.ValueMember = "DeptIdx";
            cboDept.DisplayMember = "DeptName";
            cboDept.FieldName = "DeptIdx";
            cboDept.HeaderText = "Department";
            cboDept.Width = 50;
            gv.Columns.Add(cboDept);

            GridViewComboBoxColumn cboBuyer = new GridViewComboBoxColumn();
            cboBuyer.Name = "Buyer";
            cboBuyer.DataSource = custName;
            cboBuyer.ValueMember = "CustIdx";
            cboBuyer.DisplayMember = "CustName";
            cboBuyer.FieldName = "Buyer";
            cboBuyer.HeaderText = "Buyer";
            cboBuyer.Width = 100;
            gv.Columns.Add(cboBuyer);

            GridViewTextBoxColumn Styleno = new GridViewTextBoxColumn();
            Styleno.Name = "Styleno";
            Styleno.FieldName = "Styleno";
            Styleno.Width = 130;
            Styleno.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            Styleno.HeaderText = "Style#";
            gv.Columns.Add(Styleno);

            GridViewTextBoxColumn Fileno = new GridViewTextBoxColumn();
            Fileno.Name = "Fileno";
            Fileno.FieldName = "Fileno";
            Fileno.Width = 90;
            Fileno.HeaderText = "INT File #";
            Fileno.ReadOnly = true;
            gv.Columns.Add(Fileno);

            GridViewTextBoxColumn reorder = new GridViewTextBoxColumn();
            reorder.Name = "reorder";
            reorder.FieldName = "reorder";
            reorder.HeaderText = "Re#";
            reorder.ReadOnly = true;
            reorder.Width = 35;
            reorder.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(reorder);

            GridViewTextBoxColumn Description = new GridViewTextBoxColumn();
            Description.Name = "Description";
            Description.FieldName = "Description";
            Description.Width = 170;
            Description.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            Description.HeaderText = "Description";
            gv.Columns.Add(Description);

            GridViewTextBoxColumn SizeGroupIdx = new GridViewTextBoxColumn();
            SizeGroupIdx.Name = "SizeGroupIdx";
            SizeGroupIdx.FieldName = "SizeGroupIdx";
            SizeGroupIdx.IsVisible = false; 
            gv.Columns.Add(SizeGroupIdx);
            
            GridViewTextBoxColumn OrderQty = new GridViewTextBoxColumn();
            OrderQty.Name = "OrderQty";
            OrderQty.FieldName = "OrderQty";
            OrderQty.Width = 60;
            OrderQty.FormatString = "{0:N0}";
            OrderQty.HeaderText = "Q'ty(pcs)";
            OrderQty.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            gv.Columns.Add(OrderQty);

            //GridViewComboBoxColumn status = new GridViewComboBoxColumn();
            //status.Name = "Status";
            //status.DataSource = lstStatus;
            //status.DisplayMember = "Contents";
            //status.ValueMember = "CodeIdx";
            //status.FieldName = "Status";
            //status.HeaderText = "Status";
            //status.Width = 70;
            //status.ReadOnly = true;
            //gv.Columns.Insert(25, status);


            #endregion
        }

        /// <summary>
        /// 그리드뷰 컬럼 생성 (컬러사이즈)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV2_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            gv.MasterTemplate.Columns.Clear();
            gv.DataSource = null;

            GridViewTextBoxColumn cIdx = new GridViewTextBoxColumn();
            cIdx.Name = "Idx";
            cIdx.FieldName = "Idx";
            cIdx.Width = 50;
            cIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cIdx.HeaderText = "ID";
            cIdx.ReadOnly = true;
            gv.Columns.Add(cIdx);

            GridViewTextBoxColumn cOrderIdx = new GridViewTextBoxColumn();
            cOrderIdx.Name = "OrderIdx";
            cOrderIdx.FieldName = "OrderIdx";
            cOrderIdx.Width = 60;
            cOrderIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cOrderIdx.HeaderText = "Order ID";
            cOrderIdx.ReadOnly = true;
            cOrderIdx.IsVisible = false;
            gv.Columns.Add(cOrderIdx);

            GridViewTextBoxColumn cColorIdx = new GridViewTextBoxColumn();
            cColorIdx.Name = "ColorIdx";
            cColorIdx.FieldName = "ColorIdx";
            cColorIdx.Width = 120;
            cColorIdx.TextAlignment = ContentAlignment.MiddleLeft;
            cColorIdx.HeaderText = "Color";
            gv.Columns.Add(cColorIdx);
            
            #region Sizes 

            GridViewTextBoxColumn cSizeIdx1 = new GridViewTextBoxColumn();
            cSizeIdx1.Name = "SizeIdx1";
            cSizeIdx1.FieldName = "SizeIdx1";
            cSizeIdx1.IsVisible = false;
            gv.Columns.Add(cSizeIdx1);

            GridViewTextBoxColumn cSizeIdx2 = new GridViewTextBoxColumn();
            cSizeIdx2.Name = "SizeIdx2";
            cSizeIdx2.FieldName = "SizeIdx2";
            cSizeIdx2.IsVisible = false;
            gv.Columns.Add(cSizeIdx2);

            GridViewTextBoxColumn cSizeIdx3 = new GridViewTextBoxColumn();
            cSizeIdx3.Name = "SizeIdx3";
            cSizeIdx3.FieldName = "SizeIdx3";
            cSizeIdx3.IsVisible = false;
            gv.Columns.Add(cSizeIdx3);

            GridViewTextBoxColumn cSizeIdx4 = new GridViewTextBoxColumn();
            cSizeIdx4.Name = "SizeIdx4";
            cSizeIdx4.FieldName = "SizeIdx4";
            cSizeIdx4.IsVisible = false;
            gv.Columns.Add(cSizeIdx4);

            GridViewTextBoxColumn cSizeIdx5 = new GridViewTextBoxColumn();
            cSizeIdx5.Name = "SizeIdx5";
            cSizeIdx5.FieldName = "SizeIdx5";
            cSizeIdx5.IsVisible = false;
            gv.Columns.Add(cSizeIdx5);

            GridViewTextBoxColumn cSizeIdx6 = new GridViewTextBoxColumn();
            cSizeIdx6.Name = "SizeIdx6";
            cSizeIdx6.FieldName = "SizeIdx6";
            cSizeIdx6.IsVisible = false;
            gv.Columns.Add(cSizeIdx6);

            GridViewTextBoxColumn cSizeIdx7 = new GridViewTextBoxColumn();
            cSizeIdx7.Name = "SizeIdx7";
            cSizeIdx7.FieldName = "SizeIdx7";
            cSizeIdx7.IsVisible = false;
            gv.Columns.Add(cSizeIdx7);

            GridViewTextBoxColumn cSizeIdx8 = new GridViewTextBoxColumn();
            cSizeIdx8.Name = "SizeIdx8";
            cSizeIdx8.FieldName = "SizeIdx8";
            cSizeIdx8.IsVisible = false;
            gv.Columns.Add(cSizeIdx8);

            #endregion 

            #region Pcs 

            GridViewDecimalColumn cPcs1 = new GridViewDecimalColumn();
            cPcs1.Name = "pcs1";
            cPcs1.FieldName = "pcs1";
            cPcs1.Width = 70;
            cPcs1.FormatString = "{0:N0}";
            cPcs1.HeaderText = lstSize[1].SizeName;
            gv.Columns.Add(cPcs1);

            GridViewDecimalColumn cPcs2 = new GridViewDecimalColumn();
            cPcs2.Name = "pcs2";
            cPcs2.FieldName = "pcs2";
            cPcs2.Width = 70;
            cPcs2.FormatString = "{0:N0}";
            cPcs2.HeaderText = lstSize[2].SizeName;
            gv.Columns.Add(cPcs2);

            GridViewDecimalColumn cPcs3 = new GridViewDecimalColumn();
            cPcs3.Name = "pcs3";
            cPcs3.FieldName = "pcs3";
            cPcs3.Width = 70;
            cPcs3.FormatString = "{0:N0}";
            cPcs3.HeaderText = lstSize[3].SizeName;
            gv.Columns.Add(cPcs3);

            GridViewDecimalColumn cPcs4 = new GridViewDecimalColumn();
            cPcs4.Name = "pcs4";
            cPcs4.FieldName = "pcs4";
            cPcs4.Width = 70;
            cPcs4.FormatString = "{0:N0}";
            cPcs4.HeaderText = lstSize[4].SizeName;
            gv.Columns.Add(cPcs4);

            GridViewDecimalColumn cPcs5 = new GridViewDecimalColumn();
            cPcs5.Name = "pcs5";
            cPcs5.FieldName = "pcs5";
            cPcs5.Width = 70;
            cPcs5.FormatString = "{0:N0}";
            cPcs5.HeaderText = lstSize[5].SizeName;
            gv.Columns.Add(cPcs5);

            GridViewDecimalColumn cPcs6 = new GridViewDecimalColumn();
            cPcs6.Name = "pcs6";
            cPcs6.FieldName = "pcs6";
            cPcs6.Width = 70;
            cPcs6.FormatString = "{0:N0}";
            cPcs6.HeaderText = lstSize[6].SizeName;
            gv.Columns.Add(cPcs6);

            GridViewDecimalColumn cPcs7 = new GridViewDecimalColumn();
            cPcs7.Name = "pcs7";
            cPcs7.FieldName = "pcs7";
            cPcs7.Width = 70;
            cPcs7.FormatString = "{0:N0}";
            cPcs7.HeaderText = lstSize[7].SizeName;
            gv.Columns.Add(cPcs7);

            GridViewDecimalColumn cPcs8 = new GridViewDecimalColumn();
            cPcs8.Name = "pcs8";
            cPcs8.FieldName = "pcs8";
            cPcs8.Width = 70;
            cPcs8.FormatString = "{0:N0}";
            cPcs8.HeaderText = lstSize[8].SizeName;
            gv.Columns.Add(cPcs8);

            #endregion

            #endregion
        }

        /// <summary>
        /// Dropdownlist 데이터 로딩
        /// </summary>
        private void DataLoading_DDL()
        {
            try
            {
                // 부서    
                _dt = CommonController.Getlist(CommonValues.KeyName.DeptIdx).Tables[0];

                foreach (DataRow row in _dt.Rows)
                {
                    // 관리부와 임원은 모든 부서에 접근가능
                    if (UserInfo.CenterIdx != 1 || UserInfo.DeptIdx == 5 || UserInfo.DeptIdx == 6)
                    {
                        deptName.Add(new DepartmentName(Convert.ToInt32(row["DeptIdx"]),
                                                    row["DeptName"].ToString(),
                                                    Convert.ToInt32(row["CostcenterIdx"])));
                    }
                    // 영업부는 해당 부서 데이터만 접근가능
                    else
                    {
                        if (Convert.ToInt32(row["DeptIdx"]) <= 0 || Convert.ToInt32(row["DeptIdx"]) == UserInfo.DeptIdx)
                        {
                            deptName.Add(new DepartmentName(Convert.ToInt32(row["DeptIdx"]),
                                                    row["DeptName"].ToString(),
                                                    Convert.ToInt32(row["CostcenterIdx"])));
                        }
                    }
                }

                // 바이어
                _dt = CommonController.Getlist(CommonValues.KeyName.CustIdx).Tables[0];

                foreach (DataRow row in _dt.Rows)
                {
                    custName.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                                row["CustName"].ToString(),
                                                Convert.ToInt32(row["Classification"])));
                }
                
                // 오더상태
                lstStatus.Add(new CodeContents(0, CommonValues.DicOrderStatus[0], ""));
                lstStatus.Add(new CodeContents(1, CommonValues.DicOrderStatus[1], ""));
                lstStatus.Add(new CodeContents(2, CommonValues.DicOrderStatus[2], ""));
                lstStatus.Add(new CodeContents(3, CommonValues.DicOrderStatus[3], ""));
                
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
                if (ddlCust.SelectedValue != null || ddlDept.SelectedValue != null
                    || ddlStatus.SelectedValue != null || !string.IsNullOrEmpty(txtFileno.Text) || !string.IsNullOrEmpty(txtStyle.Text))
                {
                    _searchKey = new Dictionary<CommonValues.KeyName, int>();

                    // 영업부인경우, 해당 부서만 조회할수 있도록 제한 
                    if (UserInfo.ReportNo < 9 && UserInfo.CenterIdx != 4)
                        _searchKey.Add(CommonValues.KeyName.DeptIdx, UserInfo.DeptIdx);
                    else
                        _searchKey.Add(CommonValues.KeyName.DeptIdx, Convert.ToInt32(ddlDept.SelectedValue));

                    _searchKey.Add(CommonValues.KeyName.CustIdx, Convert.ToInt32(ddlCust.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Status, Convert.ToInt32(ddlStatus.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.EmbelishId1, 0);

                    DataBinding_GV1(2, _searchKey, txtFileno.Text.Trim(), txtStyle.Text.Trim());
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
        private void DataBinding_GV1(int KeyCount, Dictionary<CommonValues.KeyName, int> SearchKey, string fileno, string styleno)
        {
            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                int _mode_ = 0;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    //조회시간
                    //Stopwatch sw = new Stopwatch();
                    //sw.Start(); 

                    gvOrder.DataSource = null;

                    _ds1 = Controller.Orders.Getlist(KeyCount, SearchKey, fileno, styleno);
                    if (_ds1 != null)
                    {
                        gvOrder.DataSource = _ds1.Tables[0].DefaultView;
                        // 조회 후, 상태알림 및 설정적용
                        gvOrder.EnablePaging = false;
                        gvOrder.AllowSearchRow = false;
                    }
                    //sw.Stop();
                    //lblTime.Text = sw.Elapsed.ToString();
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBindingGv1: " + ex.Message.ToString());
            }
        }

        private void DataBinding_GV2(RadGridView gv, int OrderIdx)
        {
            try
            {
                gv.DataSource = null;

                _ds1 = Controller.OrderColor.Getlist(OrderIdx);
                if (_ds1 != null)
                {
                    gv.DataSource = _ds1.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBinding " + gv.Name + ": " + ex.Message.ToString());
            }
        }

        #endregion

        #region 바인딩 & 이벤트

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvColor.SelectedCells.Count <= 0)
                {
                    RadMessageBox.Show("Please select the ordered q'ty.");
                    return;
                }

                GridViewRowInfo row = Int.Members.GetCurrentRow(gvOrder);

                foreach (GridViewCellInfo cellColor in gvColor.SelectedCells)
                {
                    if (Convert.ToInt32(cellColor.Value) > 0)
                    {
                        // Worked 기본값 > 개발실로 설정 
                        gvOutSource.Rows.Add(_orderIdx, Convert.ToInt32(row.Cells["Idx"].Value),
                                    cellColor.RowInfo.Cells["ColorIdx"].Value.ToString(),
                                    lstSize[cellColor.ColumnInfo.Index - 10].SizeIdx,
                                    lstSize[cellColor.ColumnInfo.Index - 10].SizeName, 
                                    Convert.ToInt32(cellColor.Value));
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
                //RadMultiColumnComboBoxElement meditor = e.ActiveEditor as RadMultiColumnComboBoxElement;

                //if (meditor != null)
                //{
                //    meditor.Enabled = false;
                //}

                // DDL 높이, 출력항목수 설정
                RadDropDownListEditor editor = this.gvOutSource.ActiveEditor as RadDropDownListEditor;
                if (editor != null)
                {
                    ((RadDropDownListEditorElement)((RadDropDownListEditor)this.gvOutSource.ActiveEditor).EditorElement).DefaultItemsCountInDropDown
                        = CommonValues.DDL_DefaultItemsCountInDropDown;
                    ((RadDropDownListEditorElement)((RadDropDownListEditor)this.gvOutSource.ActiveEditor).EditorElement).DropDownHeight
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

        private void gvOrder_SelectionChanged(object sender, EventArgs e)
        {
            lstSize.Clear(); // 기존 저장된 사이즈 초기화 
            GetSizes(gvOrder); // 하단 Color Size 데이터용 Size 정보 업데이트 
            
            // 오더별 사이즈 그룹이 다르므로 컬러사이즈 제목 갱신후 자료갱신
            GV2_CreateColumn(gvColor);
            DataBinding_GV2(gvColor, Int.Members.GetCurrentRow(gvOrder, "Idx"));

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

                if (gvOutSource.Rows.Count <= 0)
                {
                    RadMessageBox.Show("Please select the cutted Q'ty", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }

                // 데이터 DB저장 
                foreach (GridViewRowInfo row in gvOutSource.Rows)
                {
                    NewCode = Code.GetPrimaryCode(UserInfo.CenterIdx, UserInfo.DeptIdx, 15, _worderIdx);  

                    if (!string.IsNullOrEmpty(NewCode))
                    {
                        dr = Dev.Controller.OutFinishedD.Insert(Convert.ToInt32(row.Cells["pIdx"].Value),
                            Convert.ToInt32(row.Cells["OrderIdx"].Value), NewCode, 0, 
                            row.Cells["OrdColorIdx"].Value.ToString().Trim(),
                            Convert.ToInt32(row.Cells["OrdSizeIdx"].Value), Convert.ToInt32(row.Cells["OutQty"].Value),  
                            UserInfo.Idx);
                    }
                }

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
                gvOrder.EndEdit();
                GridViewRowInfo row = Int.Members.GetCurrentRow(gv);  // 현재 행번호 확인

                DataRow dr = Codes.Controller.SizeGroup.Get(Convert.ToInt32(row.Cells["SizeGroupIdx"].Value));

                if (dr != null)
                {
                    lstSize.Add(new Codes.Controller.Sizes(0, ""));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx1"]), dr["Size1"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx2"]), dr["Size2"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx3"]), dr["Size3"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx4"]), dr["Size4"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx5"]), dr["Size5"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx6"]), dr["Size6"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx7"]), dr["Size7"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx8"]), dr["Size8"].ToString()));
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }

        #endregion

    }
}
