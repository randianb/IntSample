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
using Telerik.WinControls.UI;

namespace Dev.Sales
{
    public partial class frmCopyOrder : RadForm
    {
        private frmCopyOrder _obj1 = null;

        private string _fileNo = "";
        private int _orderIdx, _orderQty, _shipCompleted = 0, _copied=0;
        private double _orderAmount = 0f;
        private bool _bRtn = false;
        
        private void radButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewDataRowInfo item in gvCopy.Rows)
                {
                    //Console.WriteLine(item.Cells["fileno"].Value.ToString());
                    _bRtn = Data.OrdersData.CopyOrder(_orderIdx, item.Cells["fileno"].Value.ToString());
                    if (_bRtn) _copied++; 
                }
                DialogResult = System.Windows.Forms.DialogResult.OK;
                //this.Close(); 
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message.ToString()); 
            }
            
        }

        public int Copied
        {
            get { return _copied;  }
        }
        public frmCopyOrder(int idx, string fileNo, int qty, double amount, int shipCompleted)
        {
            InitializeComponent();
            _orderIdx = idx;
            _fileNo = fileNo;
            _orderQty = qty;
            _orderAmount = amount;
            _shipCompleted = shipCompleted;

            lblFileno.Text = fileNo;
            lblQty.Text = _orderQty.ToString("0,00#");
            lblAmount.Text = _orderAmount.ToString("0,000.00");
            if (shipCompleted > 0) lblStatus.Text = "Completed";
            else lblStatus.Text = "Progressing";

            this.StartPosition = FormStartPosition.CenterScreen;

            
        }

        private void frmCopyOrder_Load(object sender, EventArgs e)
        {

        }
    }
}
