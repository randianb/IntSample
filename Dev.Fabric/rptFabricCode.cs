using Dev.Fabric.Reports;
using Dev.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace Dev.Fabric
{
    public partial class rptFabricCode : Form
    {
        private Dictionary<CommonValues.KeyName, string> _searchString;        // 쿼리값 value, 쿼리항목 key로 전달
        private DataSet dsData = null;
        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 31, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        public rptFabricCode()
        {
            InitializeComponent();
        }

        private void rptFabricCode_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
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
                    _searchString = new Dictionary<CommonValues.KeyName, string>();
                    _searchString.Add(CommonValues.KeyName.IsUse, "");
                    _searchString.Add(CommonValues.KeyName.Remark, "");

                    DataBinding_Order();
                    rptFabricCodeForm report = new rptFabricCodeForm();
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
                dsData = Dev.Controller.Fabric.Getlist(_searchString);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Fail to load Data " + Environment.NewLine + ex.Message,
                    "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }

        }
    }
}
