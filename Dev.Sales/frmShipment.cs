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
    public partial class frmShipment : Telerik.WinControls.UI.RadForm
    {
        #region 변수 선언

        private RadGridView _gv1 = null;
        private DataSet _ds1 = null;
        private Shipment _obj1 = null; 
        private string _fileNo = "";
        private int _orderIdx, _orderQty, _shipCompleted, _orderStatus = 0;
        private double _orderAmount = 0f;
        private bool _bRtn = false;
        private bool _rowTotal = false; 
        private bool _overShip = false;

        #endregion

        /// <summary>
        /// parent로부터 order 정보 받아오기 
        /// </summary>
        /// <param name="idx">고유번호</param>
        /// <param name="fileNo">파일번호</param>
        /// <param name="qty">수량</param>
        /// <param name="amount">금액</param>
        /// <param name="shipCompleted">선적완료 여부</param>
        public frmShipment(int idx, string fileNo, int qty, double amount, int shipCompleted, int orderStatus)
        {
            InitializeComponent();
            _orderIdx = idx; 
            _fileNo = fileNo;
            _orderQty = qty;
            _orderAmount = amount;
            _shipCompleted = shipCompleted;
            _orderStatus = orderStatus; 

            lblFileno.Text = fileNo;
            lblQty.Text = _orderQty.ToString("0,00#"); 
            lblAmount.Text = _orderAmount.ToString("0,000.00");
            if (shipCompleted > 0) lblStatus.Text = "Completed"; 
            else lblStatus.Text = "Progressing";

            this.StartPosition = FormStartPosition.CenterScreen;

            _gv1 = this.gvShipment;
            GV1_LayoutSetting(_gv1);
            
            // 선적완료, 오더마감 여부에 따라 편집 disable 
            if (_shipCompleted==1 || _orderStatus==2 || _orderStatus==3)
            {
                radCommandBar1.Enabled = false; 
                _gv1.Enabled = false; 
            }
        }

        private void frmShipment_Load(object sender, EventArgs e)
        {
            DataBinding_GV1(_orderIdx);
            AddSummaryRows();
        }

        /// <summary>
        /// 데이터 그리드 레이아웃 설정
        /// </summary>
        /// <param name="gv"></param>
        private void GV1_LayoutSetting(RadGridView gv)
        {
            gv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;

            GridViewTextBoxColumn cIdx = new GridViewTextBoxColumn();
            cIdx.Name = "Idx";
            cIdx.HeaderText = "ID";
            cIdx.FieldName = "Idx";
            cIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cIdx.Width = 50;
            cIdx.ReadOnly = true; 
            gv.Columns.Insert(0, cIdx);

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ko-KR");
            GridViewDateTimeColumn cExdate = new GridViewDateTimeColumn();
            cExdate.Name = "ExfactoryDate";
            cExdate.HeaderText = "Date Exfactoried";
            cExdate.TextAlignment = ContentAlignment.MiddleCenter;
            cExdate.Width = 120; 
            cExdate.FieldName = "ExfactoryDate";
            cExdate.FormatString = "{0:d}";
            gv.Columns.Insert(1, cExdate);

            GridViewDecimalColumn cOrderQty = new GridViewDecimalColumn();
            cOrderQty.Name = "Qty";
            cOrderQty.HeaderText = "Q'ty Shipped";
            cOrderQty.FieldName = "Qty";
            cOrderQty.DecimalPlaces = 0;
            cOrderQty.Width = 100;
            cOrderQty.FormatString = "{0:N0}";
            gv.Columns.Insert(2, cOrderQty);

            GridViewDecimalColumn cOrderAmount = new GridViewDecimalColumn();
            cOrderAmount.Name = "Amount";
            cOrderAmount.HeaderText = "Amount";
            cOrderAmount.FieldName = "Amount";
            cOrderAmount.DecimalPlaces = 3;
            cOrderAmount.Width = 120;
            cOrderAmount.FormatString = "{0:N2}"; 
            gv.Columns.Insert(3, cOrderAmount);
                        
            gv.Dock = DockStyle.Fill;
            gv.EnablePaging = false;
            gv.AllowAddNewRow = false;
            gv.AllowCellContextMenu = false;
            gv.AllowColumnHeaderContextMenu = false;
            gv.EnableGrouping = false;

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataBinding_GV1(_orderIdx);
        }

        /// <summary>
        /// 신규 입력
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnInsert_Click(object sender, EventArgs e)
        {
            try
            {
                _bRtn = Shipment.Insert(_orderIdx, DateTime.Today, 0, 0f);

                if (_bRtn)
                {
                    // 입력완료 후 그리드뷰 갱신
                    DataBinding_GV1(_orderIdx);
                    _gv1.CurrentRow = _gv1.Rows[_gv1.RowCount-1];
                    if (_gv1.CurrentRow != null) _gv1.CurrentRow.IsSelected = true;
                    
                }
                else
                {
                    //lblResult.Text = "Failed to input the data.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnUpdate_Click: " + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 삭제 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (RadMessageBox.Show("Do you want to delete this data?\n(ID: " +
                    _gv1.Rows[_gv1.CurrentRow.Index].Cells[0].Value.ToString() + ")", "Confirm",
                    System.Windows.Forms.MessageBoxButtons.YesNo, RadMessageIcon.Question) ==
                    System.Windows.Forms.DialogResult.Yes)
                {
                    _gv1.EndEdit();
                    _obj1 = new Shipment(Convert.ToInt32(_gv1.Rows[_gv1.CurrentRow.Index].Cells["Idx"].Value));
                    _obj1.Idx = Convert.ToInt32(_gv1.Rows[_gv1.CurrentRow.Index].Cells["Idx"].Value);
                    _bRtn = _obj1.Delete();
                }

                if (_bRtn)
                {
                    // 삭제 후 그리드뷰 갱신
                    btnRefresh_Click(sender, e);
                }
                else
                {
                    //lblResult.Text = "Failed to input the data.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnInsert_Click: " + ex.Message.ToString());
            }
        }

        /// <summary>
        /// 데이터 업데이트 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GV1_Update(object sender, GridViewCellEventArgs e)
        {
            _bRtn = false;

            try
            {
                Console.WriteLine("GV1_Update"); 
                _gv1.EndEdit();
                
                // 객체생성 
                _obj1 = new Shipment(Convert.ToInt32(_gv1.Rows[_gv1.CurrentRow.Index].Cells["Idx"].Value));
                // 선택된 그리드행의 데이터를 객체 프로퍼티에 전달
                _obj1.Idx = Convert.ToInt32(_gv1.CurrentRow.Cells["Idx"].Value.ToString());
                _obj1.OrderIdx = _orderIdx; 
                _obj1.Exdate = Convert.ToDateTime(_gv1.CurrentRow.Cells["ExfactoryDate"].Value);
                _obj1.Qty = Convert.ToInt32(_gv1.CurrentRow.Cells["Qty"].Value.ToString());
                _obj1.Amount = Convert.ToDouble(_gv1.CurrentRow.Cells["Amount"].Value.ToString());

                int gvQty = 0;
                double gvAmount = 0; 
                foreach(GridViewRowInfo row in _gv1.Rows)
                {
                    gvQty += Convert.ToInt32(row.Cells["Qty"].Value);
                    gvAmount += Convert.ToDouble(row.Cells["Amount"].Value); 
                }
                // Overship 여부확인 
                if (_orderQty < gvQty || _orderAmount < gvAmount)
                {
                    // Allow Overship 체크버튼으로 반드시 담당자가 확인토록 
                    if (AllowOvership.Checked)
                    {
                        // Update
                        _bRtn = _obj1.Update();
                    }
                    else
                    {
                        btnRefresh_Click(sender, e);
                        return;
                    }

                    //if (RadMessageBox.Show("Are you sure to over shipment?", "Confirm", MessageBoxButtons.YesNo, RadMessageIcon.Question)
                    //    == DialogResult.Yes)
                    //{
                    //    // Update
                    //    _bRtn = _obj1.Update();
                    //}
                    //else
                    //{
                    //    btnRefresh_Click(sender, e);
                    //    return; 
                    //}
                }
                else
                {
                    _bRtn = _obj1.Update();
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine("GV1_Update: " + ex.Message.ToString());
            }

            if (_bRtn)
            {

            }
            else
            {

            }
        }

        #region 바인딩

        private void DataBinding_GV1(int orderIdx)
        {
            try
            {
                _gv1.DataSource = null;
                _ds1 = Shipment.Getlist(orderIdx);
                if (_ds1 != null) _gv1.DataSource = _ds1.Tables[0].DefaultView;
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBindingGv1: " + ex.Message.ToString());
            }
        }

        private void GV1_AddedRow(object sender, GridViewRowEventArgs e)
        {
            //_gv1.CurrentRow = _gv1.Rows[e.Row.Index];
            //if (_gv1.CurrentRow != null) _gv1.CurrentRow.IsSelected = true;
        }

        private void radButton1_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void GV1_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            if (e.CellElement is GridSummaryCellElement)
            {
                e.CellElement.TextAlignment = ContentAlignment.MiddleRight;
                
                // 0미만일 경우 컬러 변경
                GridSummaryCellElement summaryCell = e.CellElement as GridSummaryCellElement;
                decimal summaryValue = 0m; 
                if (summaryCell != null && decimal.TryParse(e.CellElement.Value + "", out summaryValue) &&
                    summaryValue < 0)
                {
                    summaryCell.ForeColor = Color.Red;
                }
                else
                {
                    summaryCell.ForeColor = Color.Black; 
                }

                // Summary 합계행열을 식별하거나 OverShip또는 Shortage판별하여 제목변경 
                if (e.Column.Name == "ExfactoryDate")
                {
                    if (e.CellElement.Text == "Total Shipment")
                    {
                        _rowTotal = true;
                    }
                    else
                    {
                        if (_overShip) e.CellElement.Text = "Over Ship";
                        else e.CellElement.Text = "Shortage";
                    }
                } 
                
                // Summary 합계행의 Amount열에서 Order금액비교하여 Over/Shortage 여부저장 (저장후, 합계행 여부 리셋) 
                if (e.Column.Name == "Amount" && _rowTotal)
                {
                    if ((Convert.ToDouble(e.CellElement.Value) - _orderAmount) < 0)
                    {
                        _overShip = false;
                    }
                    else
                    {
                        _overShip = true;
                    }
                    _rowTotal = false; 
                }
            }
        }

        #endregion

        #region 메서드 

        private void AddSummaryRows()
        {
            // 선적합계 
            GridViewSummaryRowItem summaryRowItem = new GridViewSummaryRowItem();
            GridViewSummaryItem summaryItem = null;

            summaryItem = new GridViewSummaryItem("ExfactoryDate", "Total Shipment", GridAggregateFunction.Max);
            summaryRowItem.Add(summaryItem); 

            summaryItem = new GridViewSummaryItem(); 
            summaryItem.Name = "Qty";
            summaryItem.Aggregate = GridAggregateFunction.Sum;
            summaryRowItem.Add(summaryItem);

            summaryItem = new GridViewSummaryItem();
            summaryItem.Name = "Amount";
            summaryItem.Aggregate = GridAggregateFunction.Sum;
            summaryRowItem.Add(summaryItem);
            
            _gv1.SummaryRowsBottom.Add(summaryRowItem);

            string summaryTitle = "";

            if (_shipCompleted == 0) summaryTitle = "미 선적 ";
            else summaryTitle = "Over ship "; 
            
            // 미선적/Over/Shortage 
            summaryRowItem = new GridViewSummaryRowItem();
            summaryItem = null;

            summaryItem = new GridViewSummaryItem("ExfactoryDate", summaryTitle, GridAggregateFunction.Max);
            summaryRowItem.Add(summaryItem);

            summaryItem = new GridViewSummaryItem();
            summaryItem.Name = "Qty";
            summaryItem.AggregateExpression = "Sum(Qty)-" + _orderQty;
            summaryItem.FormatString = "{0:N0}";
            summaryRowItem.Add(summaryItem);

            summaryItem = new GridViewSummaryItem();
            summaryItem.Name = "Amount";
            summaryItem.AggregateExpression = "Sum(Amount)-" + _orderAmount;
            summaryItem.FormatString = "{0:N2}";
            summaryRowItem.Add(summaryItem);

            _gv1.SummaryRowsBottom.Add(summaryRowItem);
        }

        #endregion  
    }
}
