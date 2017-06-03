using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace Dev.Controller
{
    public class MyCombBoxCellElement : GridComboBoxCellElement
    {
        public MyCombBoxCellElement(GridViewColumn col, GridRowElement row) : base(col, row)
        {
        }
        public override void SetContent()
        {
            // base.SetContent();
            this.SetContentCore(this.Value);
        }

        protected override Type ThemeEffectiveType
        {
            get
            {
                return typeof(GridComboBoxCellElement);
            }
        }
    }

}
