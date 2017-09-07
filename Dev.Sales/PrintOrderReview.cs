﻿using Dev.Production.Reports;
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

namespace Dev.Sales
{
    public partial class PrintOrderReview : Form
    {
        private Dictionary<CommonValues.KeyName, string> _searchString;        // 쿼리값 value, 쿼리항목 key로 전달
        private Dictionary<CommonValues.KeyName, int> _searchKey;        // 쿼리값 value, 쿼리항목 key로 전달 
        private DataTable _dt = null;                                           // 기본 데이터테이블
        private List<CustomerName> custName = new List<CustomerName>();         // 거래처
        private List<CodeContents> sizeName = new List<CodeContents>();

        private DataSet dsData = null;
        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 23, 0);   // 패키지번호, 프로그램번호, 윈도우번호
        private int _orderIdx=0; 

        public PrintOrderReview(int OrderIdx)
        {
            InitializeComponent();
            _orderIdx = OrderIdx; 
        }

        private void rptFabricCode_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            //DateKind.SelectedIndex = 0;
            //dtFromDate.Value = Convert.ToDateTime(DateTime.Today.Year + "-01-01"); 
            //dtToDate.Value = DateTime.Today;

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

            // 사이즈 
            //ddlSize.DataSource = sizeName;
            //ddlSize.DisplayMember = "Contents";
            //ddlSize.ValueMember = "CodeIdx";
            //ddlSize.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            //ddlSize.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 바이어
            ddlCust.DataSource = custName;
            ddlCust.DisplayMember = "CustName";
            ddlCust.ValueMember = "CustIdx";
            ddlCust.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlCust.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            try
            {
                /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                /// 읽기: 0, 쓰기: 1, 삭제: 2
                int _mode_ = 0;
                if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                    CheckAuth.ShowMessage(_mode_);
                else
                {
                    if (_orderIdx > 0)
                    {
                        dsData = null;
                        dsData = Dev.Sales.Controller.Orders.Getlist(_orderIdx, Convert.ToInt32(ddlCust.SelectedValue),
                            txtFileno.Text.Trim(), txtStyle.Text.Trim());

                        rpPrintOrderReview report = new rpPrintOrderReview();
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
            }
            catch (Exception ex)
            {

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
                    //_searchString = new Dictionary<CommonValues.KeyName, string>();
                    //_searchString.Add(CommonValues.KeyName.OrderIdx, "");
                    //_searchString.Add(CommonValues.KeyName.WorkOrderIdx, "");
                    //_searchString.Add(CommonValues.KeyName.ColorIdx, "");
                    //_searchString.Add(CommonValues.KeyName.Fileno, "");
                    //_searchString.Add(CommonValues.KeyName.Styleno, txtStyle.Text.Trim());
                    //_searchString.Add(CommonValues.KeyName.Remark, "");
                    //_searchString.Add(CommonValues.KeyName.StartDate, "2000-01-01");

                    //_searchKey = new Dictionary<CommonValues.KeyName, int>();
                    //_searchKey.Add(CommonValues.KeyName.BuyerIdx, Convert.ToInt32(ddlCust.SelectedValue));
                    //_searchKey.Add(CommonValues.KeyName.FabricIdx, 0);
                    ////_searchKey.Add(CommonValues.KeyName.Size, Convert.ToInt32(ddlSize.SelectedValue));
                    
                    DataBinding_Order();

                    rpPrintOrderReview report = new rpPrintOrderReview();
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
                //CultureInfo ci = new CultureInfo("ko-KR");
                dsData = Dev.Sales.Controller.Orders.Getlist(0, Convert.ToInt32(ddlCust.SelectedValue), 
                    txtFileno.Text.Trim(), txtStyle.Text.Trim()); 
                //, dtFromDate.Value, dtToDate.Value);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Fail to load Data " + Environment.NewLine + ex.Message,
                    "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }

        }
    }
}