using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Sales
{
    public class Shipment
    {
        #region Members

        private int _idx;
        private int _orderIdx;
        private DateTime _exdate;
        private int _qty;
        private double _amount;
        
        private DataRow _row;

        #endregion

        #region Property 

        // 고유번호
        public int Idx {
            get { return _idx; }
            set { _idx = value; }
        }
        // 오더고유번호
        public int OrderIdx
        {
            get { return _orderIdx; }
            set { _orderIdx = value; }
        }
        // 선적일 exfactory
        public DateTime Exdate
        {
            get { return _exdate; }
            set { _exdate = value; }
        }
        // 수량
        public int Qty
        {
            get { return _qty; }
            set { _qty = value; }
        }
        // 금액
        public double Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        #endregion

        #region Constructor 
        public Shipment(int Idx)
        {
            _row = Dev.Sales.Data.ShipmentData.Get(Idx);

            if (_row != null)
            {

                _idx = Convert.ToInt32(_row["Idx"]);
                _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
                _exdate = Convert.ToDateTime(_row["ExfactoryDate"]);
                _qty = Convert.ToInt32(_row["Qty"]);
                _amount = Convert.ToInt32(_row["Amount"]);
            }
            else
            {
                Initializer();
            }
        }

        private void Initializer()
        {
            _idx = 0; 
            _orderIdx = 0; 
            _exdate = DateTime.Now;
            _qty = 0;
            _amount = 0f; 
        }
        #endregion

        #region Methods

        // ID없이 즉시 입력가능하도록 정적 메소드로 정의후 파라미터를 전달
        public static bool Insert(int OrderIdx, DateTime Exdate, int Qty, double Amount)
        {
            bool blRtn;
            blRtn = Data.ShipmentData.Insert(OrderIdx, Exdate, Qty, Amount);
            return blRtn;
        }

        // 어디서든 즉시 조회 가능하도록 정적 메소드로 정의
        public static DataSet Getlist(int OrderIdx)
        {
            DataSet ds = new DataSet();
            ds = Data.ShipmentData.Getlist(OrderIdx); 
            return ds;
        }


        public bool Update()
        {
            bool blRtn;
            blRtn = Data.ShipmentData.Update(_idx, _orderIdx, _exdate, _qty, _amount);
            return blRtn;
        }

        public bool Delete()
        {
            bool blRtn;
            blRtn = Data.ShipmentData.Delete(_idx);
            return blRtn;
        }

        #endregion
    }
}
