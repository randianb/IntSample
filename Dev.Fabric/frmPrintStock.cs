using Dev.Fabric.Reports;
using Dev.Options;
using Int.Code;
using Int.Costcenter;
using Int.Customer;
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
using Telerik.WinControls.UI;

namespace Dev.Fabric
{
    public partial class frmPrintStock : Form
    {
        private Dictionary<CommonValues.KeyName, string> _searchString;        // 쿼리값 value, 쿼리항목 key로 전달
        private Dictionary<CommonValues.KeyName, int> _searchKey;        // 쿼리값 value, 쿼리항목 key로 전달 
        private DataTable _dt = null;                                   // 기본 데이터테이블

        private List<CodeContents> lstStatus = new List<CodeContents>();        // 
        private List<CodeContents> lstOutStatus = new List<CodeContents>();        // 
        private List<CustomerName> lstBuyer = new List<CustomerName>();        // 
        //private List<CodeContents> lstColor = new List<CodeContents>();        //  
        private List<Controller.Fabric> lstFabric = new List<Controller.Fabric>();        // 
        private List<CodeContents> lstFabricType = new List<CodeContents>();        // 
        private List<CodeContents> lstRack1 = new List<CodeContents>();       // 
        private List<CodeContents> lstRack2 = new List<CodeContents>();        // 
        private List<CodeContents> lstRack3 = new List<CodeContents>();        // 
        private List<CodeContents> codeName = new List<CodeContents>();         // 코드
        private List<CostcenterName> lstCenter = new List<CostcenterName>();     // 
        private List<CustomerName> lstDept = new List<CustomerName>();     // 

        private DataSet dsData = null;
        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 34, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        public frmPrintStock()
        {
            InitializeComponent();
        }

        private void rptFabricCode_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            DataLoading_DDL(); 
            Config_DropDownList(); 
        }

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
            lstStatus.Add(new CodeContents(4, CommonValues.DicFabricInStatus[4], ""));
            lstStatus.Add(new CodeContents(9, CommonValues.DicFabricInStatus[9], ""));
            lstStatus.Add(new CodeContents(10, CommonValues.DicFabricInStatus[10], ""));
            
            lstOutStatus.Add(new CodeContents(0, CommonValues.DicFabricOutStatus[0], ""));
            lstOutStatus.Add(new CodeContents(3, CommonValues.DicFabricOutStatus[3], ""));
            lstOutStatus.Add(new CodeContents(5, CommonValues.DicFabricOutStatus[5], ""));
            lstOutStatus.Add(new CodeContents(6, CommonValues.DicFabricOutStatus[6], ""));
            lstOutStatus.Add(new CodeContents(7, CommonValues.DicFabricOutStatus[7], ""));
            lstOutStatus.Add(new CodeContents(8, CommonValues.DicFabricOutStatus[8], ""));
            lstOutStatus.Add(new CodeContents(11, CommonValues.DicFabricOutStatus[11], ""));
            
            // Buyer
            if (UserInfo.ExceptionGroup == 233)     // 팀장급 이상일 경우, 
            {
                _dt = CommonController.Getlist(CommonValues.KeyName.CustAll).Tables[0];
            }
            else
            {
                _dt = CommonController.Getlist(CommonValues.KeyName.CustIdx).Tables[0];
            }

            

            foreach (DataRow row in _dt.Rows)
            {
                lstBuyer.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
            }

            //// Color 
            //_dt = Codes.Controller.Color.GetUselist().Tables[0];
            //lstColor.Add(new CodeContents(0, "", ""));
            //foreach (DataRow row in _dt.Rows)
            //{
            //    lstColor.Add(new CodeContents(Convert.ToInt32(row["ColorIdx"]),
            //                                row["ColorName"].ToString(),
            //                                ""));
            //}

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
            _dt = Dev.Controller.Fabric.Getlist(_searchString).Tables[0];

            dataSetSizeGroup.DataTableFabric.Rows.Add(0, "", "", "", 0f, "", 0f, "", 0f, "", 0f, "", 0f);
            foreach (DataRow row in _dt.Rows)
            {
                dataSetSizeGroup.DataTableFabric.Rows.Add(Convert.ToInt32(row["Idx"]),
                                            row["LongName"].ToString(), row["ShortName"].ToString(),
                                            row["Yarnnm1"] == DBNull.Value ? "" : row["Yarnnm1"].ToString(), row["Percent1"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent1"]),
                                            row["Yarnnm2"] == DBNull.Value ? "" : row["Yarnnm2"].ToString(), row["Percent2"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent2"]),
                                            row["Yarnnm3"] == DBNull.Value ? "" : row["Yarnnm3"].ToString(), row["Percent3"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent3"]),
                                            row["Yarnnm4"] == DBNull.Value ? "" : row["Yarnnm4"].ToString(), row["Percent4"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent4"]),
                                            row["Yarnnm5"] == DBNull.Value ? "" : row["Yarnnm5"].ToString(), row["Percent5"] == DBNull.Value ? 0f : Convert.ToInt32(row["Percent5"])
                                            );
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
        }

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
            //ddlColor.DataSource = lstColor;
            //ddlColor.DisplayMember = "Contents";
            //ddlColor.ValueMember = "Contents";
            //ddlColor.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //ddlColor.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            //ddlColor.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 
            ddlFabric.DataSource = dataSetSizeGroup.DataTableFabric;
            ddlFabric.DisplayMember = "ShortName";
            ddlFabric.ValueMember = "Idx";
            ddlFabric.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            ddlFabric.AutoSizeDropDownColumnMode = BestFitColumnMode.AllCells;
            ddlFabric.AutoSizeDropDownHeight = true;
            ddlFabric.DropDownStyle = RadDropDownStyle.DropDown;
            ddlFabric.AutoSizeDropDownToBestFit = true;

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
        /// 검색조건을 이용한 자료 새로고침
        /// </summary>
        private void RefleshWithCondition()
        {
            try
            {
                if (ddlStatus.SelectedValue != null || ddlBuyer.SelectedValue != null || ddlColor.Text != null
                    || ddlFabric.SelectedValue != null || ddlFabricType.SelectedValue != null
                    || !string.IsNullOrEmpty(txtLotno.Text) || !string.IsNullOrEmpty(txtInboundno.Text)
                    || !string.IsNullOrEmpty(txtLocationX.Value.ToString()) || !string.IsNullOrEmpty(txtLocationY.Value.ToString())
                    || ddlRack1.SelectedValue != null || ddlRack2.SelectedValue != null || ddlRack3.SelectedValue != null
                    )
                {
                    _searchString = new Dictionary<CommonValues.KeyName, string>();
                    _searchString.Add(CommonValues.KeyName.Lotno, txtLotno.Text.Trim());
                    _searchString.Add(CommonValues.KeyName.WorkOrderIdx, txtInboundno.Text.Trim());
                    _searchString.Add(CommonValues.KeyName.ColorIdx, Convert.ToString(ddlColor.Text));

                    _searchKey = new Dictionary<CommonValues.KeyName, int>();
                    _searchKey.Add(CommonValues.KeyName.Status, Convert.ToInt32(ddlStatus.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.BuyerIdx, Convert.ToInt32(ddlBuyer.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.FabricIdx, Convert.ToInt32(ddlFabric.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.FabricType, Convert.ToInt32(ddlFabricType.SelectedValue));

                    _searchKey.Add(CommonValues.KeyName.RackNo, Convert.ToInt32(ddlRack1.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Floorno, Convert.ToInt32(ddlRack2.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.RackPos, Convert.ToInt32(ddlRack3.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.PosX, Convert.ToInt32(txtLocationX.Value));
                    _searchKey.Add(CommonValues.KeyName.PosY, Convert.ToInt32(txtLocationY.Value));
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

        }

        private void btnSearch_Click(object sender, EventArgs e)
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
                    RefleshWithCondition();

                    DataBinding_Order();

                    rptStock report = new rptStock();
                    reportViewer1.Report = report;
                    if (dsData != null)
                        report.DataSource = dsData.Tables[0].DefaultView;
                    else
                        report.DataSource = null;

                    reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
                    reportViewer1.ZoomMode = Telerik.ReportViewer.WinForms.ZoomMode.PageWidth;
                    reportViewer1.RefreshReport();
                }
            }
            catch(Exception ex)
            {

            }
            
        }

        private void DataBinding_Order()
        {
            try
            {
                dsData = null;
                CultureInfo ci = new CultureInfo("ko-KR");
                dsData = Data.InboundData.GetlistStock(_searchString, _searchKey, dtInboundDate.Text, dtInboundDateTo.Text);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Fail to load Data " + Environment.NewLine + ex.Message,
                    "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }
        }

        private void radLabel12_DoubleClick(object sender, EventArgs e)
        {
            dtInboundDate.Value = Convert.ToDateTime("2000-01-01");
        }
    }
}
