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
                    case "Cut": LoadingCutting(); break;
                    case "Print": LoadingPrint(); break;
                    case "Embroidery": LoadingEmbroidery(); break;
                    case "Sew": LoadingSew(); break;
                    case "Inspection": LoadingInspection(); break;

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

                rptWorkOrderPatternTicket report = new rptWorkOrderPatternTicket();
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
                        workOrder.Color = row["Color"].ToString();
                        workOrder.Fabric = row["Fabric"].ToString();
                        workOrder.Yds = Convert.ToDouble(row["Yds"].ToString());
                        workOrder.Qty = Convert.ToInt32(row["Qty"].ToString());
                        workOrder.SampleType = row["SampleType"].ToString();
                        if (row["OrderDate"] != DBNull.Value) workOrder.OrderDate = Convert.ToDateTime(row["OrderDate"]);
                        if (row["TicketDate"] != DBNull.Value) workOrder.TicketDate = Convert.ToDateTime(row["TicketDate"]);
                        lstWorkOrder.Add(workOrder);
                    }
                }

                rptWorkOrderCuttingTicket report = new rptWorkOrderCuttingTicket();
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

        private void LoadingPrint()
        {
            try
            {
                List<WorkOrderTicketStruct> lstWorkOrder = new List<WorkOrderTicketStruct>();
                WorkOrderTicketStruct workOrder = null;

                // 작업오더지시 화면에서 선택된 작업정보를 가져와 재단티켓에 필요한 내용구성 
                foreach (string str in CommonValues.ListWorkID)
                {
                    row = Controller.WorkOrder.Getlist("Print", str.Trim());
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
                        workOrder.Color = row["Color"].ToString();
                        workOrder.Fabric = ""; //  row["Fabric"].ToString();  
                        workOrder.Yds = Convert.ToDouble(row["Yds"].ToString());
                        workOrder.Qty = Convert.ToInt32(row["Qty"].ToString());
                        workOrder.SampleType = row["SampleType"].ToString();
                        if (row["OrderDate"] != DBNull.Value) workOrder.OrderDate = Convert.ToDateTime(row["OrderDate"]);
                        if (row["TicketDate"] != DBNull.Value) workOrder.TicketDate = Convert.ToDateTime(row["TicketDate"]);
                        lstWorkOrder.Add(workOrder);
                    }
                }

                rptWorkOrderPrintTicket report = new rptWorkOrderPrintTicket();
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

        private void LoadingEmbroidery()
        {
            try
            {
                List<WorkOrderTicketStruct> lstWorkOrder = new List<WorkOrderTicketStruct>();
                WorkOrderTicketStruct workOrder = null;

                // 작업오더지시 화면에서 선택된 작업정보를 가져와 재단티켓에 필요한 내용구성 
                foreach (string str in CommonValues.ListWorkID)
                {
                    row = Controller.WorkOrder.Getlist("Embroidery", str.Trim());
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
                        workOrder.Color = row["Color"].ToString();
                        workOrder.Fabric = ""; //  row["Fabric"].ToString();  
                        workOrder.Yds = Convert.ToDouble(row["Yds"].ToString());
                        workOrder.Qty = Convert.ToInt32(row["Qty"].ToString());
                        workOrder.SampleType = row["SampleType"].ToString();
                        if (row["OrderDate"] != DBNull.Value) workOrder.OrderDate = Convert.ToDateTime(row["OrderDate"]);
                        if (row["TicketDate"] != DBNull.Value) workOrder.TicketDate = Convert.ToDateTime(row["TicketDate"]);
                        lstWorkOrder.Add(workOrder);
                    }
                }

                rptWorkOrderEmbroideryTicket report = new rptWorkOrderEmbroideryTicket();
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

        private void LoadingSew()
        {
            try
            {
                List<WorkOrderTicketStruct> lstWorkOrder = new List<WorkOrderTicketStruct>();
                WorkOrderTicketStruct workOrder = null;

                // 작업오더지시 화면에서 선택된 작업정보를 가져와 재단티켓에 필요한 내용구성 
                foreach (string str in CommonValues.ListWorkID)
                {
                    row = Controller.WorkOrder.Getlist("Sew", str.Trim());
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
                        workOrder.Color = row["Color"].ToString();
                        workOrder.Fabric = ""; //  row["Fabric"].ToString();  
                        workOrder.Yds = Convert.ToDouble(row["Yds"].ToString());
                        workOrder.Qty = Convert.ToInt32(row["Qty"].ToString());
                        workOrder.SampleType = row["SampleType"].ToString();
                        if (row["OrderDate"] != DBNull.Value) workOrder.OrderDate = Convert.ToDateTime(row["OrderDate"]);
                        if (row["TicketDate"] != DBNull.Value) workOrder.TicketDate = Convert.ToDateTime(row["TicketDate"]);
                        lstWorkOrder.Add(workOrder);
                    }
                }

                rptWorkOrderSewTicket report = new rptWorkOrderSewTicket();
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

        private void LoadingInspection()
        {
            try
            {
                List<WorkOrderTicketStruct> lstWorkOrder = new List<WorkOrderTicketStruct>();
                WorkOrderTicketStruct workOrder = null;

                // 작업오더지시 화면에서 선택된 작업정보를 가져와 재단티켓에 필요한 내용구성 
                foreach (string str in CommonValues.ListWorkID)
                {
                    row = Controller.WorkOrder.Getlist("Inspection", str.Trim());
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
                        workOrder.Color = row["Color"].ToString();
                        workOrder.Fabric = row["Fabric"].ToString();  
                        workOrder.Yds = Convert.ToDouble(row["Yds"].ToString());
                        workOrder.Qty = Convert.ToInt32(row["Qty"].ToString());
                        workOrder.SampleType = row["SampleType"].ToString();
                        if (row["OrderDate"] != DBNull.Value) workOrder.OrderDate = Convert.ToDateTime(row["OrderDate"]);
                        if (row["TicketDate"] != DBNull.Value) workOrder.TicketDate = Convert.ToDateTime(row["TicketDate"]);
                        lstWorkOrder.Add(workOrder);
                    }
                }

                rptWorkOrderInspectionTicket report = new rptWorkOrderInspectionTicket();
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
