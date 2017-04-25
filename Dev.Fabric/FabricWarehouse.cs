using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls; 

namespace Dev.Fabric
{
    public partial class FabricWarehouse : Telerik.WinControls.UI.RadForm
    {
        public FabricWarehouse()
        {
            InitializeComponent();
        }

        private void radMap1_SelectionChanged(object sender, Telerik.WinControls.UI.MapSelectionChangedEventArgs e)
        {

        }
    }
}
