using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class Printing
    {
        #region Members

        private int _idx;
        private int _orderIdx;
        private int _cuttedIdx;
        private string _workOrderIdx;
        private string _outOrderIdx;
        private string _ordColorIdx;
        private int _ordSizeIdx;
        private int _ordQty;
        private DateTime _rcvdDate;
        
        private int _rcvdQty;
        private int _rcvdFrom;
        private string _remarks;
        
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
        public int CuttedIdx
        {
            get { return _cuttedIdx; }
            set { _cuttedIdx = value; }
        }
        //
        public string WorkOrderIdx
        {
            get { return _workOrderIdx; }
            set { _workOrderIdx = value; }
        }
        //
        public string OutOrderIdx
        {
            get { return _outOrderIdx; }
            set { _outOrderIdx = value; }
        }
        //
        public string OrdColorIdx
        {
            get { return _ordColorIdx; }
            set { _ordColorIdx = value; }
        }
        //
        public int OrdSizeIdx
        {
            get { return _ordSizeIdx; }
            set { _ordSizeIdx = value; }
        }
        //
        public int OrdQty
        {
            get { return _ordQty; }
            set { _ordQty = value; }
        }
        public DateTime RcvdDate
        {
            get { return _rcvdDate; }
            set { _rcvdDate = value; }
        }
        
        public int RcvdQty
        {
            get { return _rcvdQty; }
            set { _rcvdQty = value; }
        }
        public int RcvdFrom
        {
            get { return _rcvdFrom; }
            set { _rcvdFrom = value; }
        }
        
        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }
        
        #endregion

        #region Constructor 

        public Printing(int Idx)
        {
            _row = Dev.Data.PrintingData.Get(Idx);
            
            if (_row != null)
            {
                _idx = Convert.ToInt32(_row["Idx"]);
                if (_row["OrderIdx"] != DBNull.Value) _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
                if (_row["CuttedIdx"] != DBNull.Value) _cuttedIdx = Convert.ToInt32(_row["CuttedIdx"]);
                if (_row["WorkOrderIdx"] != DBNull.Value) _workOrderIdx = Convert.ToString(_row["WorkOrderIdx"]);
                if (_row["OutOrderIdx"] != DBNull.Value) _outOrderIdx = Convert.ToString(_row["OutOrderIdx"]);
                if (_row["OrdColorIdx"] != DBNull.Value) _ordColorIdx = Convert.ToString(_row["OrdColorIdx"]);
                if (_row["OrdSizeIdx"] != DBNull.Value) _ordSizeIdx = Convert.ToInt32(_row["OrdSizeIdx"]);
                if (_row["OrdQty"] != DBNull.Value) _ordQty = Convert.ToInt32(_row["OrdQty"]); else _ordQty = 0;

                if (_row["RcvdDate"] != DBNull.Value) _rcvdDate = Convert.ToDateTime(_row["RcvdDate"]);
                
                if (_row["RcvdQty"] != DBNull.Value) _rcvdQty = Convert.ToInt32(_row["RcvdQty"]); else _rcvdQty = 0;
                if (_row["RcvdFrom"] != DBNull.Value) _rcvdFrom = Convert.ToInt32(_row["RcvdFrom"]); else _rcvdFrom = 0;
                if (_row["Remarks"] != DBNull.Value) _remarks = _row["Remarks"].ToString(); else _remarks = ""; 
                
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
            _cuttedIdx = 0;
            _workOrderIdx = "";
            _outOrderIdx = ""; 
            _ordColorIdx = "";
            _ordSizeIdx = 0;
            _ordQty = 0;
            _rcvdDate = DateTime.Now;
            _rcvdQty = 0;
            _rcvdFrom = 0;
            _remarks = "";
    }

        #endregion

        #region Methods
        
        public static DataRow Insert(int OrderIdx, int CuttedIdx, string WorkOrderIdx, string OutOrderIdx, string OrdColorIdx, int OrdSizeIdx, int OrdQty,
            DateTime RcvdDate, int RcvdQty, int RcvdFrom, string Remarks, int Handler)
        {
            DataRow row = Data.PrintingData.Insert(OrderIdx, CuttedIdx, WorkOrderIdx, OutOrderIdx, OrdColorIdx, OrdSizeIdx, OrdQty,
                                                RcvdDate, RcvdQty, RcvdFrom, Remarks, Handler);

            return row;
        }
         
        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey,
                                    Dictionary<CommonValues.KeyName, string> SearchString)
        {
            DataSet ds = new DataSet();
            
            ds = Data.PrintingData.Getlist(SearchKey, SearchString); 
                        
            return ds;
        }

        //public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey)
        //{
        //    DataSet ds = new DataSet();

        //    ds = Data.PrintingData.Getlist(SearchKey);

        //    return ds;
        //}

        public bool Update()
        {
            bool blRtn;
            blRtn = Data.PrintingData.Update(_idx, _outOrderIdx, _rcvdDate, _rcvdQty, _rcvdFrom, 
                                            _remarks);
            return blRtn;
        }


        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.PrintingData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
