using Dev.Options;
using Dev.WorkOrder.Reports;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace Dev.WorkOrder
{
    public partial class WorkOrderTicketPrint : Form
    {
        private DataRow row = null; 

        public WorkOrderTicketPrint()
        {
            InitializeComponent();
        }

        private void WorkOrderTicketPrint_Load(object sender, EventArgs e)
        {
            //this.reportViewer1.RefreshReport();

            //Console.WriteLine(CommonValues.WorkOperation.Trim()); 
            try
            {
                switch (CommonValues.WorkOperation.Trim())
                {
                    case "Pattern": LoadingPattern(); break;
                    case "Cutting": LoadingPattern(); break;

                    default: break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
            
        }

        private void LoadingPattern()
        {
            try
            {
                List<WorkOrderTicketStruct> lstWorkOrder = new List<WorkOrderTicketStruct>();
                WorkOrderTicketStruct workOrder = null;

                // 작업오더지시 화면에서 선택된 작업정보를 가져와 패턴티켓에 필요한 내용구성 
                foreach (string str in CommonValues.ListWorkID)
                {
                    row = Controller.WorkOrder.Getlist("Pattern", str.Trim());
                    if (row!=null)
                    {
                        workOrder = new WorkOrderTicketStruct();
                        workOrder.WorkOrderIdx = str.Trim();
                        workOrder.Operation = row["Operation"].ToString();
                        workOrder.Qrcode = row["Qrcode"].ToString();
                        workOrder.Fileno = row["Fileno"].ToString();
                        workOrder.Styleno = row["Styleno"].ToString();
                        workOrder.Buyer = row["Buyer"].ToString();
                        workOrder.Handler = row["Handler"].ToString();
                        workOrder.Size = row["Size"].ToString();
                        workOrder.SampleType = row["SampleType"].ToString();
                        if(row["TicketDate"]!=DBNull.Value) workOrder.TicketDate = Convert.ToDateTime(row["TicketDate"]);
                        lstWorkOrder.Add(workOrder); 
                    }
                }

                rptWorkOrderTicket report = new rptWorkOrderTicket();
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

        private void LoadingCutting()
        {
            try
            {
                List<WorkOrderTicketStruct> lstWorkOrder = new List<WorkOrderTicketStruct>();
                WorkOrderTicketStruct workOrder = null;

                // 작업오더지시 화면에서 선택된 작업정보를 가져와 재단티켓에 필요한 내용구성 
                foreach (string str in CommonValues.ListWorkID)
                {
                    row = Controller.WorkOrder.Getlist("Cut", str.Trim());
                    if (row != null)
                    {
                        workOrder = new WorkOrderTicketStruct();
                        workOrder.WorkOrderIdx = str.Trim();
                        workOrder.Operation = row["Operation"].ToString();
                        workOrder.Qrcode = row["Qrcode"].ToString();
                        workOrder.Fileno = row["Fileno"].ToString();
                        workOrder.Styleno = row["Styleno"].ToString();
                        workOrder.Buyer = row["Buyer"].ToString();
                        workOrder.Handler = row["Handler"].ToString();
                        workOrder.Size = row["Size"].ToString();
                        workOrder.SampleType = row["SampleType"].ToString();
                        if (row["TicketDate"] != DBNull.Value) workOrder.TicketDate = Convert.ToDateTime(row["TicketDate"]);
                        lstWorkOrder.Add(workOrder);
                    }
                }

                rptWorkOrderTicket report = new rptWorkOrderTicket();
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
