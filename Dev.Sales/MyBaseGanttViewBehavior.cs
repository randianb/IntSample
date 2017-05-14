using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Dev.Sales
{
    internal class MyBaseGanttViewBehavior : BaseGanttViewBehavior
    {
        protected override void ProcessMouseUpWhenResizingTask(GanttGraphicalViewBaseTaskElement element, MouseEventArgs e)
        {
            GanttViewTaskElement task = this.EditedTaskElement as GanttViewTaskElement;

            try
            {
                if (task != null)
                {
                    GanttViewDataItem data = ((GanttGraphicalViewBaseItemElement)element.Parent).Data;

                    string[] strTitle = data.Title.Split('/');
                    System.Console.WriteLine(strTitle[2] + ", " + data.Start + ", " + data.End);

                    bool result = Dev.Controller.WorkOrder.Update(strTitle[2].ToString().Trim(), data.Start, data.End, 0.4, Dev.Options.UserInfo.Idx);
                }
                base.ProcessMouseUpWhenResizingTask(element, e);

            }
            catch(Exception ex)
            {
                RadMessageBox.Show(ex.Message); 
            }

        }
        
    }
}
