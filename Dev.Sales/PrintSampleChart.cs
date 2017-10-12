using Dev.Production.Reports;
using Dev.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using Int.Customer;
using Int.Code;
using Dev.Sales.Reports;
using Int.Department;

namespace Dev.Sales
{
    public partial class PrintSampleChart : Form
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                             // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, int> _searchKey;                // 쿼리값 value, 쿼리항목 key로 전달 
        public DataRow InsertedOrderRow = null;

        private bool _bRtn;                                                     // 쿼리결과 리턴
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        private DataTable _dt = null;                                           // 기본 데이터테이블
                
        private List<CodeContents> lstStatus = new List<CodeContents>();        // 오더상태
        private List<DepartmentName> deptName = new List<DepartmentName>();     // 부서
        private List<CustomerName> custName = new List<CustomerName>();         // 거래처
        private List<CustomerName> lstVendor = new List<CustomerName>();        // vendor
        private List<CustomerName> embName = new List<CustomerName>();          // 나염업체
        private List<CodeContents> codeName = new List<CodeContents>();         // 코드
        private List<CodeContents> lstOrigin = new List<CodeContents>();        // Origin country 
        private List<CodeContents> lstBrand = new List<CodeContents>();         // 브랜드
        private List<CodeContents> lstIsPrinting = new List<CodeContents>();    // 나염여부
        private List<CustomerName> lstUser = new List<CustomerName>();          // 유저명
        private List<CustomerName> lstSewthread = new List<CustomerName>();     // sewthread
        private List<CodeContents> lstColor = new List<CodeContents>();         // 컬러명
        private List<CodeContents> lstOperation = new List<CodeContents>();     // 공정명
        private List<CodeContents> sizeName = new List<CodeContents>();
        
        private string _workOrderIdx;
        //private GridViewRowInfo _currentRow = null;
        private List<string> lstFiles = new List<string>();
        private List<string> lstFileUrls = new List<string>();

        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 110, 0);   // 패키지번호, 프로그램번호, 윈도우번호
        #endregion
        

        public PrintSampleChart()
        {
            InitializeComponent();
        }

        private void rptFabricCode_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            DataLoading_DDL();
            Config_DropDownList(); 

            //TD확인 
            if (Options.UserInfo.DeptIdx == 12 || Options.UserInfo.DeptIdx == 11)
            {
                toggTD.Value = true;
            }
            else
            {
                toggTD.Value = false;
            }

            WorksheetOpCheck1.Checked = CommonValues.WorksheetOpCheck1;
            WorksheetOpCheck2.Checked = CommonValues.WorksheetOpCheck2;
            WorksheetOpCheck3.Checked = CommonValues.WorksheetOpCheck3;
            WorksheetOpCheck4.Checked = CommonValues.WorksheetOpCheck4;
            WorksheetOpCheck5.Checked = CommonValues.WorksheetOpCheck5;
            
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

            // 담당자
            ddlHandler.DataSource = lstUser;
            ddlHandler.DisplayMember = "CustName";
            ddlHandler.ValueMember = "CustIdx";
            ddlHandler.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlHandler.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 오더 상태 
            ddlStatus.DataSource = lstStatus;
            ddlStatus.DisplayMember = "Contents";
            ddlStatus.ValueMember = "CodeIdx";
            ddlStatus.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlStatus.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 정해진 바이어가 있는 경우, 해당 바이어로 dropdown 미리 선택
            if (CommonValues.NewOrderBuyerIdx > 0)
                ddlCust.SelectedValue = Convert.ToInt32(CommonValues.NewOrderBuyerIdx);
        }

        /// <summary>
        /// Dropdownlist 데이터 로딩
        /// </summary>
        private void DataLoading_DDL()
        {
            // 부서    
            _dt = CommonController.Getlist(CommonValues.KeyName.DeptIdx).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                // 관리부와 임원은 모든 부서에 접근가능
                if (Options.UserInfo.CenterIdx != 1 || Options.UserInfo.DeptIdx == 5 || Options.UserInfo.DeptIdx == 6 ||
                    Options.UserInfo.ExceptionGroup == 233)
                {
                    deptName.Add(new DepartmentName(Convert.ToInt32(row["DeptIdx"]),
                                                row["DeptName"].ToString(),
                                                Convert.ToInt32(row["CostcenterIdx"])));
                }
                // 영업부는 해당 부서 데이터만 접근가능
                else
                {
                    if (Convert.ToInt32(row["DeptIdx"]) <= 0 || Convert.ToInt32(row["DeptIdx"]) == Options.UserInfo.DeptIdx)
                    {
                        deptName.Add(new DepartmentName(Convert.ToInt32(row["DeptIdx"]),
                                                row["DeptName"].ToString(),
                                                Convert.ToInt32(row["CostcenterIdx"])));
                    }
                }

                //if (UserInfo.ExceptionGroup == 233)     // 팀장급 이상일 경우, 
            }

            // 바이어
            _dt = CommonController.Getlist(CommonValues.KeyName.CustAll).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                custName.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
            }

            //// 코드
            //_dt = CommonController.Getlist(CommonValues.KeyName.Codes).Tables[0];

            //foreach (DataRow row in _dt.Rows)
            //{
            //    codeName.Add(new CodeContents(Convert.ToInt32(row["Idx"]),
            //                                row["Contents"].ToString(),
            //                                row["Classification"].ToString()));
            //}

            //// 사이즈명 
            //_dt = Dev.Codes.Controller.Sizes.GetUselist().Tables[0];

            //sizeName = new List<CodeContents>();

            //sizeName.Add(new CodeContents(0, "", "")); 
            //foreach (DataRow row in _dt.Rows)
            //{

            //    sizeName.Add(new CodeContents(Convert.ToInt32(row["SizeIdx"]),
            //                            row["SizeName"].ToString(),
            //                            ""));
            //}

            // Username
            lstUser.Add(new CustomerName(0, "", 0));

            if (Options.UserInfo.CenterIdx != 1 || Options.UserInfo.DeptIdx == 5 || Options.UserInfo.DeptIdx == 6)
            {
                _dt = CommonController.Getlist(CommonValues.KeyName.AllUser).Tables[0];
            }
            else
            {
                _dt = CommonController.Getlist(CommonValues.KeyName.User).Tables[0];
            }

            foreach (DataRow row in _dt.Rows)
            {
                lstUser.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]),
                                            row["UserName"].ToString(),
                                            Convert.ToInt32(row["DeptIdx"])));
            }

            // 오더상태 (CommonValues정의)
            lstStatus.Add(new CodeContents(0, CommonValues.DicWorkOrderStatus[0], ""));
            lstStatus.Add(new CodeContents(1, CommonValues.DicWorkOrderStatus[1], ""));
            lstStatus.Add(new CodeContents(2, CommonValues.DicWorkOrderStatus[2], ""));
            lstStatus.Add(new CodeContents(3, CommonValues.DicWorkOrderStatus[3], ""));
            lstStatus.Add(new CodeContents(4, CommonValues.DicWorkOrderStatus[4], ""));
            lstStatus.Add(new CodeContents(5, CommonValues.DicWorkOrderStatus[5], ""));
            lstStatus.Add(new CodeContents(6, CommonValues.DicWorkOrderStatus[6], ""));
            lstStatus.Add(new CodeContents(7, CommonValues.DicWorkOrderStatus[7], ""));
            lstStatus.Add(new CodeContents(8, CommonValues.DicWorkOrderStatus[8], ""));
            lstStatus.Add(new CodeContents(10, CommonValues.DicWorkOrderStatus[10], ""));
            lstStatus.Add(new CodeContents(11, CommonValues.DicWorkOrderStatus[11], ""));
            lstStatus.Add(new CodeContents(12, CommonValues.DicWorkOrderStatus[12], ""));
            lstStatus.Add(new CodeContents(13, CommonValues.DicWorkOrderStatus[13], ""));
            lstStatus.Add(new CodeContents(14, CommonValues.DicWorkOrderStatus[14], ""));
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
                //if (ddlCust.SelectedValue != null || ddlSize.SelectedValue != null
                //    || ddlStatus.SelectedValue != null || ddlSize.SelectedValue != null
                //    || !string.IsNullOrEmpty(txtFileno.Text) || !string.IsNullOrEmpty(txtStyle.Text))
                //{
                _searchKey = new Dictionary<CommonValues.KeyName, int>();

                // 영업부인경우, 해당 부서만 조회할수 있도록 제한 
                if (Options.UserInfo.ReportNo < 9 && Options.UserInfo.ExceptionGroup != 233)
                    _searchKey.Add(CommonValues.KeyName.DeptIdx, Options.UserInfo.DeptIdx);
                else
                    _searchKey.Add(CommonValues.KeyName.DeptIdx, Convert.ToInt32(ddlDept.SelectedValue));

                //_searchKey.Add(CommonValues.KeyName.CustIdx, Convert.ToInt32(ddlCust.SelectedValue));
                //_searchKey.Add(CommonValues.KeyName.Status, Convert.ToInt32(ddlStatus.SelectedValue));
                //_searchKey.Add(CommonValues.KeyName.Size, Convert.ToInt32(ddlSize.SelectedValue));

                //int OrderIdx, string WorksheetIdx, int Handler, int ConfirmUser, int WorkStatus

                DataBinding_GV1(_searchKey[CommonValues.KeyName.DeptIdx], Convert.ToInt32(ddlCust.SelectedValue),
                                Convert.ToInt32(ddlHandler.SelectedValue), Convert.ToInt32(ddlStatus.SelectedValue),
                                txtFileno.Text.Trim(), txtStyle.Text.Trim(), txtWorksheet.Text.Trim(),
                                WorksheetOpCheck1.Checked ? 1 : 0,
                                WorksheetOpCheck2.Checked ? 1 : 0,
                                WorksheetOpCheck3.Checked ? 1 : 0,
                                WorksheetOpCheck4.Checked ? 1 : 0,
                                WorksheetOpCheck5.Checked ? 1 : 0
                                );
                //}
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
        private void DataBinding_GV1(int DeptIdx, int CustIdx, int Handler, int WorkStatus, string Fileno, string Styleno, string WorksheetIdx,
                                    int OptionCheck1, int OptionCheck2, int OptionCheck3, int OptionCheck4, int OptionCheck5)
        {
            try
            {
                
                if (toggTD.Value)
                {
                    // TD일땐 해당 바이어만 
                    _ds1 = Dev.Controller.Worksheet.GetlistReport(DeptIdx, CustIdx, Handler, WorkStatus, Fileno, Styleno, WorksheetIdx, Options.UserInfo.Idx,
                                    OptionCheck1, OptionCheck2, OptionCheck3, OptionCheck4, OptionCheck5);
                }
                else
                {
                    // TD아니면 모든 바이어 
                    _ds1 = Dev.Controller.Worksheet.GetlistReport(DeptIdx, CustIdx, Handler, WorkStatus, Fileno, Styleno, WorksheetIdx,
                                    OptionCheck1, OptionCheck2, OptionCheck3, OptionCheck4, OptionCheck5);
                }

                if (_ds1 != null)
                {
                    rpPrintSampleChart report = new rpPrintSampleChart();
                    reportViewer1.Report = report;
                    if (_ds1 != null)
                        report.DataSource = _ds1.Tables[0].DefaultView;
                    else
                        report.DataSource = null;

                    reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
                    reportViewer1.ZoomMode = Telerik.ReportViewer.WinForms.ZoomMode.PageWidth;
                    reportViewer1.RefreshReport();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBindingGv1: " + ex.Message.ToString());
            }
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

        private void txtWorksheet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }
    }
}
