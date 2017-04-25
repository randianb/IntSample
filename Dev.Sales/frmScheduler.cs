using Dev.Sales.dsScheduleTableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls.UI;

namespace Dev.Sales
{
    public partial class frmScheduler : RadForm
    {
        DataSet ds = new DataSet();

        public frmScheduler()
        {
            InitializeComponent();
            
        }

        private void radScheduler1_CellFormatting(object sender, SchedulerCellEventArgs e)
        {

        }

        private void dsSchedule_Initialized(object sender, EventArgs e)
        {

        }

        private void frmScheduler_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;

            this.scNavigator.DayViewButtonVisible = false;
            this.scNavigator.WeekViewButtonVisible = false;
            this.scNavigator.TimelineViewButtonVisible = false;
            
            scNavigator.NavigationStepType = NavigationStepTypes.Month;

            SchedulerMonthView monthView = this.scDelivery.GetMonthView();
            monthView.ShowFullMonth = true; 

            DataLoadFromServer(); 
            DataBinding(); 
        }

        private void DataLoadFromServer()
        {
            //DataTable appointments = new DataTable("Appointments");
            //appointments.Columns.Add("Id", typeof(int));
            //appointments.Columns.Add("Start", typeof(DateTime));
            //appointments.Columns.Add("End", typeof(DateTime));
            //appointments.Columns.Add("Description", typeof(string));
            //appointments.Columns.Add("Summary", typeof(string));

            //DataSet dsTemp = Data.OrdersData.LoadAppointments(); 
            
            //foreach (DataRow row in dsTemp.Tables[0].Rows)
            //{
            //    appointments.Rows.Add(row["Id"], row["Start"], row["End"], row["Summary"], row["Summary"]); 
            //}

            //DataTable resources = new DataTable("Resources");
            //resources.Columns.Add("Id", typeof(int));
            //resources.Columns.Add("ResourceName", typeof(string));
            //for (int i = 1; i <= 10; i++)
            //{
            //    resources.Rows.Add(new object[] { i, "ResourceName" + i });
            //}

            //Random rand = new Random();
            //DataTable appointmentsResources = new DataTable("AppointmentsResources");
            //appointmentsResources.Columns.Add("AppointmentId", typeof(int));
            //appointmentsResources.Columns.Add("ResourceId", typeof(int));
            //for (int i = 0; i < appointments.Rows.Count; i++)
            //{
            //    appointmentsResources.Rows.Add(new object[] { appointments.Rows[i]["Id"], rand.Next(1, resources.Rows.Count + 1) });
            //}

            //this.ds.Tables.AddRange(new DataTable[] { appointments, resources, appointmentsResources });
            //this.ds.Relations.Add("Appointment_AppointmentsResources", 
            //        this.ds.Tables["Appointments"].Columns["Id"], 
            //        this.ds.Tables["AppointmentsResources"].Columns["AppointmentId"]);
        }
        private void DataBinding()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            AppointmentMappingInfo appointmentMappingInfo = new AppointmentMappingInfo();
            appointmentMappingInfo.Start = "Start";
            appointmentMappingInfo.End = "End";
            appointmentMappingInfo.Description = "Description";
            appointmentMappingInfo.ResourceId = "ResourceId";
            appointmentMappingInfo.Resources = "Appointment_AppointmentsResources";
            appointmentMappingInfo.Summary = "Summary";
            this.schedulerBindingDataSource1.EventProvider.Mapping = appointmentMappingInfo;

            ResourceMappingInfo resourceMappingInfo = new ResourceMappingInfo();
            resourceMappingInfo.Id = "Id";
            resourceMappingInfo.Name = "ResourceName";
            this.schedulerBindingDataSource1.ResourceProvider.Mapping = resourceMappingInfo;

            schedulerBindingDataSource1.ResourceProvider.DataMember = "Resources";
            schedulerBindingDataSource1.ResourceProvider.DataSource = this.ds;

            schedulerBindingDataSource1.EventProvider.DataMember = "Appointments";
            schedulerBindingDataSource1.EventProvider.DataSource = this.ds;

            this.scDelivery.DataSource = this.schedulerBindingDataSource1;
            //this.scDelivery.GroupType = GroupType.Resource;
            this.scNavigator.SchedulerNavigatorElement.TimeZonesDropDown.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.scNavigator.ShowWeekendCheckBox.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.scDelivery.AppointmentTitleFormat = "{2}"; 

            sw.Stop();
            Console.WriteLine(sw.Elapsed);
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void scDelivery_AppointmentEditDialogShowing(object sender, AppointmentEditDialogShowingEventArgs e)
        {
            RadForm f = e.AppointmentEditDialog as RadForm;
            f.Size = new Size(f.Size.Width, f.Size.Height + 100); 
        }
    }
}
