using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class OrderFabric
    {
        #region Members

        private int _idx;
        private int _orderIdx;
        private int _fabricIdx;

        private double _yds;
        private string _remark; 
                
        private DataRow _row;

        #endregion

        #region Property 

        //ID
        public int Idx
        {
            get { return _idx; }
            set { _idx = value; }
        }
        
        //
        public int OrderIdx
        {
            get { return _orderIdx; }
            set { _orderIdx = value; }
        }
        //
        public int FabricIdx
        {
            get { return _fabricIdx; }
            set { _fabricIdx = value; }
        }

        //
        public double Yds
        {
            get { return _yds; }
            set { _yds = value; }
        }
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        
        #endregion

        #region Constructor 

        public OrderFabric(int Idx)
        {
            _row = Dev.Data.OrderFabricData.Get(Idx);
            
            if (_row != null)
            {
                _idx = Convert.ToInt32(_row["Idx"]);
                if (_row["OrderIdx"] != DBNull.Value) _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
                if (_row["FabricIdx"] != DBNull.Value) _fabricIdx = Convert.ToInt32(_row["FabricIdx"]);
                
                if (_row["Yds"] != DBNull.Value) _yds = Convert.ToDouble(_row["Yds"]);
                if (_row["Remark"] != DBNull.Value) _remark = Convert.ToString(_row["Remark"]);
                
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
            _fabricIdx = 0;

            _yds = 0f;
            _remark = "";
            

        }

        #endregion

        #region Methods
        
        public static DataRow Insert(int OrderIdx)
        {
            DataRow row = Data.OrderFabricData.Insert(OrderIdx);

            return row;
        }
        
        public static DataSet Getlist(int OrderIdx)
        {
            DataSet ds = new DataSet();
            
            ds = Data.OrderFabricData.Getlist(OrderIdx); 
                        
            return ds;
        }
       
        public bool Update()
        {
            bool blRtn;
            blRtn = Data.OrderFabricData.Update(_idx, _fabricIdx,
                _yds, _remark
                );
            return blRtn;
        }
        
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.OrderFabricData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
