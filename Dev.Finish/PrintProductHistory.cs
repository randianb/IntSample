using Dev.Out.Reports;
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

namespace Dev.Out
{
    public partial class PrintProductHistory : Form
    {
        private Dictionary<CommonValues.KeyName, string> _searchString;        // 쿼리값 value, 쿼리항목 key로 전달
        private Dictionary<CommonValues.KeyName, int> _searchKey;        // 쿼리값 value, 쿼리항목 key로 전달 
        private DataTable _dt = null;                                           // 기본 데이터테이블
        private List<CustomerName> custName = new List<CustomerName>();         // 거래처
        private List<CodeContents> sizeName = new List<CodeContents>();
        private List<CustomerName> lstOut = new List<CustomerName>();        // 
        private List<CustomerName> lstReceived = new List<CustomerName>();        // 
        private List<CodeContents> lstWorkStatus = new List<CodeContents>();        // 오더상태

        private DataSet dsData = null;
        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 12, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        public PrintProductHistory()
        {
            InitializeComponent();
        }

        private void rptFabricCode_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            // 바이어
            _dt = CommonController.Getlist(CommonValues.KeyName.CustAll).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                custName.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
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


            // Sewing Vendor 
            _dt = CommonController.Getlist(CommonValues.KeyName.Vendor).Tables[0];

            lstOut.Add(new CustomerName(0, "", 0));
           
            foreach (DataRow row in _dt.Rows)
            {
                lstOut.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
               
            }

            // Username
            lstReceived.Add(new CustomerName(0, "", 0));
            
            _dt = CommonController.Getlist(CommonValues.KeyName.AllUser).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                lstReceived.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]),
                                            row["UserName"].ToString(),
                                            Convert.ToInt32(row["DeptIdx"])));
               
            }

            // 작업진행상태
            lstWorkStatus.Add(new CodeContents(0, "", ""));
            lstWorkStatus.Add(new CodeContents(1, "New Work", ""));
            lstWorkStatus.Add(new CodeContents(2, "Printed Ticket", ""));
            lstWorkStatus.Add(new CodeContents(3, "Completed", ""));
            lstWorkStatus.Add(new CodeContents(4, "Canceled", ""));

            txtFileno.Select();
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
                    _searchKey = new Dictionary<CommonValues.KeyName, int>();
                    _searchString = new Dictionary<CommonValues.KeyName, string>();

                    _searchKey.Add(CommonValues.KeyName.BuyerIdx, Convert.ToInt32(ddlCust.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.Size, Convert.ToInt32(ddlSize.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.CustIdx, Convert.ToInt32(ddlOut.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.User, Convert.ToInt32(ddlReceived.SelectedValue));
                    _searchKey.Add(CommonValues.KeyName.WorkStatus, Convert.ToInt32(ddlWorkStatus.SelectedValue));

                    _searchString.Add(CommonValues.KeyName.OrderIdx, txtFileno.Text.ToString().Trim());
                    _searchString.Add(CommonValues.KeyName.Styleno, txtStyle.Text.ToString().Trim());
                    _searchString.Add(CommonValues.KeyName.WorkOrderIdx, txtWorkorder.Text.ToString().Trim());
                    _searchString.Add(CommonValues.KeyName.ColorIdx, txtColor.Text.ToString().Trim());
                    _searchString.Add(CommonValues.KeyName.Remark, txtDelivered.Text.ToString().Trim());

                    DataBinding_Order();


                    rptPrintProductHistory report = new rptPrintProductHistory();
                    reportViewer1.Report = report;
                    if (dsData != null)
                        report.DataSource = dsData.Tables[0].DefaultView;
                    else
                        report.DataSource = null;

                    reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview;
                    reportViewer1.ZoomMode = Telerik.ReportViewer.WinForms.ZoomMode.FullPage;
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
                dsData = Dev.Controller.OutFinished.GetlistD(_searchKey, _searchString,
                                dtOutFrom.Value.ToString("d", ci).Substring(0, 10),
                                dtOutTo.Value.ToString("d", ci).Substring(0, 10),
                                dtRcvdFrom.Value.ToString("d", ci).Substring(0, 10),
                                dtRcvdTo.Value.ToString("d", ci).Substring(0, 10));
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Fail to load Data " + Environment.NewLine + ex.Message,
                    "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }

        }

        private void radLabel8_DoubleClick(object sender, EventArgs e)
        {
            dtOutFrom.Value = Convert.ToDateTime("2000-01-01");
        }

        private void radLabel12_DoubleClick(object sender, EventArgs e)
        {
            dtOutTo.Value = Convert.ToDateTime("2900-01-01");
        }

        private void radLabel7_DoubleClick(object sender, EventArgs e)
        {
            dtRcvdFrom.Value = Convert.ToDateTime("2000-01-01");
        }

        private void radLabel13_DoubleClick(object sender, EventArgs e)
        {
            dtRcvdTo.Value = Convert.ToDateTime("2900-01-01");
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

        private void txtColor_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        private void txtWorkorder_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }
    }
}
