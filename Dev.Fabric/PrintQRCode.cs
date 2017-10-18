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
            //this.WindowState = System.Windows.Forms.FormWindowState.Maximized;  // 창 최대화
            try
            {
                // 원단코드, 랙코드 구분
                switch (CommonValues.WorkOperation.Trim())
                {
                    case "Fabric": LoadingFabric(); break;
                    case "Location": LoadingLocation(); break;
                    default: break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
            
        }
        
        /// <summary>
        /// 창고 랙에 부착할 QR코드 (랙이 적재할 장소구분 - 143:원단창고, 144:부자재창고, 145:완성창고 >> 코드관리에서 입력관리) 
        /// </summary>
        private void LoadingLocation()
        {
            try
            {
                List<LocationQRStruct> lstWorkOrder = new List<LocationQRStruct>();
                LocationQRStruct workOrder = null;

                foreach (string str in CommonValues.ListWorkID)
                {
                    //row = Controller.Inbound.Getlist(str.Trim());
                    string[] codeStr = str.Split(','); 
                    if (codeStr.Length>0)
                    {
                        workOrder = new LocationQRStruct();
                        workOrder.Qrcode = str.Trim();  
                        workOrder.Idx = codeStr[1].ToString();
                        if (codeStr[2].ToString()=="4") workOrder.CenterIdx = "Development"; else workOrder.CenterIdx = "None";
                        switch (codeStr[4].ToString().Trim())
                        {
                            case "143": workOrder.Warehouse = "Fabric"; break;
                            case "144": workOrder.Warehouse = "Accessories"; break;
                            case "145": workOrder.Warehouse = "Finish"; break;
                            default: workOrder.Warehouse = "None"; break; 
                        }  
                        
                        workOrder.PosX = codeStr[5].ToString();
                        workOrder.PosY = codeStr[6].ToString();
                        workOrder.RackNo = codeStr[7].ToString();
                        workOrder.Floorno = Convert.ToChar(Convert.ToInt32(codeStr[8].ToString().Trim()) + 64).ToString();
                        workOrder.RackPos = codeStr[9].ToString();
                        lstWorkOrder.Add(workOrder);
                    }
                }

                rptPrintLocationCode report = new rptPrintLocationCode();
                reportViewer1.Report = report;
                reportViewer1.ZoomPercent = 100; 
                reportViewer1.ZoomMode = Telerik.ReportViewer.WinForms.ZoomMode.Percent;
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

        /// <summary>
        /// 원단봉지에 부착할 QR코드
        /// </summary>
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
                        workOrder.Lotno = row["Lotno"].ToString();
                        workOrder.Yds = row["Yds"].ToString(); 
                        lstWorkOrder.Add(workOrder);
                    }
                }

                rptPrintQRCode report = new rptPrintQRCode();
                reportViewer1.Report = report;
                reportViewer1.ZoomPercent = 100;
                reportViewer1.ZoomMode = Telerik.ReportViewer.WinForms.ZoomMode.Percent;
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

        private void PrintQRCode_Deactivate(object sender, EventArgs e)
        {
            // 프린트 화면 종료 또는 다른 화면으로 이동시 코드리스트를 초기화 한다
            CommonValues.ListWorkID.Clear(); 
        }
    }
}
