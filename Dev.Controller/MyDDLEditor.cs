using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace Dev.Controller
{
    /// <summary>
    /// Gridview combobox용 custom editor 등록
    /// </summary>
    public class MyDDLEditor : RadDropDownListEditor
    {
        
        public override object Value
        {
            get
            {
                var result = base.Value;

                if (result == null || result.ToString() == string.Empty)
                {
                    var editor = this.EditorElement as RadDropDownListElement;

                    //if no item is selected return the text
                    return editor.Text;
                }
                return result;

            }

            set
            {
                base.Value = value;
            }
        }

       
    }
}
