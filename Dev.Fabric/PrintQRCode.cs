using Dev.Options;
using Dev.Fabric.Reports;
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
    public partial class PrintQRCode : Form
    {
        private DataRow row = null; 

        public PrintQRCode()
        {
            InitializeComponent();
        }

        private void PrintQRCode_Load(object sender, EventArgs e)
        {
            try
            {
                switch (CommonValues.WorkOperation.Trim())
                {
                    case "Fabric": LoadingFabric(); break;
                    default: break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
            
        }

        private void LoadingFabric()
        {
            try
            {
                List<FabricQRStruct> lstWorkOrder = new List<FabricQRStruct>();
                FabricQRStruct workOrder = null;

                foreach (string str in CommonValues.ListWorkID)
                {
                    row = Controller.Inbound.Getlist(str.Trim());
                    if (row != null)
                    {
                        workOrder = new FabricQRStruct();
                        workOrder.WorkOrderIdx = str.Trim();
                        workOrder.Qrcode = row["Qrcode"].ToString();
                        workOrder.ColorIdx = row["ColorIdx"].ToString();
                        workOrder.FabricIdx = row["FabricIdx"].ToString();
                        workOrder.FabricType = row["FabricType"].ToString();
                        lstWorkOrder.Add(workOrder);
                    }
                }

                rptPrintQRCode report = new rptPrintQRCode();
                reportViewer1.Report = report;
                reportViewer1.ZoomMode = Telerik.ReportViewer.WinForms.ZoomMode.FullPage;
                reportViewer1.ViewMode = Telerik.ReportViewer.WinForms.ViewMode.PrintPreview; 
                report.DataSource = lstWorkOrder;
                reportViewer1.RefreshReport(); 
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Fail to load data " + Environment.NewLine + ex.Message,
                    "Error", MessageBoxButtons.OK, RadMessageIcon.Error);

            }
        }
    }
}
