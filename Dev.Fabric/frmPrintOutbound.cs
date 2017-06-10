using Dev.Fabric.Reports;
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

namespace Dev.Fabric
{
    public partial class frmPrintOutbound : Form
    {
        private Dictionary<CommonValues.KeyName, string> _searchString;        // 쿼리값 value, 쿼리항목 key로 전달
        private Dictionary<CommonValues.KeyName, int> _searchKey;        // 쿼리값 value, 쿼리항목 key로 전달 
        private DataSet dsData = null;
        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 33, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        public frmPrintOutbound()
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
                    _searchString.Add(CommonValues.KeyName.Lotno, "");
                    _searchString.Add(CommonValues.KeyName.WorkOrderIdx, "");
                    _searchString.Add(CommonValues.KeyName.ColorIdx, "");

                    _searchKey = new Dictionary<CommonValues.KeyName, int>();
                    _searchKey.Add(CommonValues.KeyName.Status, 0);
                    _searchKey.Add(CommonValues.KeyName.BuyerIdx, 0);

                    _searchKey.Add(CommonValues.KeyName.FabricIdx, 0);
                    _searchKey.Add(CommonValues.KeyName.FabricType, 0);

                    _searchKey.Add(CommonValues.KeyName.InIdx, 0);
                    _searchKey.Add(CommonValues.KeyName.Handler, 0);
                    _searchKey.Add(CommonValues.KeyName.OrderIdx, 0);

                    DataBinding_Order();
                    rptOutbound report = new rptOutbound();
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
                dsData = Data.OutboundData.GetlistReport(_searchString, _searchKey, "2000-01-01");
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Fail to load Data " + Environment.NewLine + ex.Message,
                    "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
            }

        }
    }
}
