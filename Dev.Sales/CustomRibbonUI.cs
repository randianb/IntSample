using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI;

namespace Dev.Sales
{
    public class CustomRibbonUI : RichTextEditorRibbonBar
    {
        protected override void Initialize()
        {
            base.Initialize();

            // 필요없는 버튼 그룹 비활성화 
            this.groupLinks.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.groupPages.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.groupHeaderAndFooter.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.groupStyles.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.groupText.Visibility = Telerik.WinControls.ElementVisibility.Collapsed; 
        }
    }
}
