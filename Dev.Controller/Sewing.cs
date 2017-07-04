using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class Sewing
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
        private DateTime _workDate;
        
        private int _workQty;
        private int _worked;
        private int _workType;
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
        public DateTime WorkDate
        {
            get { return _workDate; }
            set { _workDate = value; }
        }
        
        public int WorkQty
        {
            get { return _workQty; }
            set { _workQty = value; }
        }
        public int Worked
        {
            get { return _worked; }
            set { _worked = value; }
        }
        public int WorkType
        {
            get { return _workType; }
            set { _workType = value; }
        }

        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }
        
        #endregion

        #region Constructor 

        public Sewing(int Idx)
        {
            _row = Dev.Data.SewingData.Get(Idx);
            
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

                if (_row["WorkDate"] != DBNull.Value) _workDate = Convert.ToDateTime(_row["WorkDate"]);
                
                if (_row["WorkQty"] != DBNull.Value) _workQty = Convert.ToInt32(_row["WorkQty"]); else _workQty = 0;
                if (_row["Worked"] != DBNull.Value) _worked = Convert.ToInt32(_row["Worked"]); else _worked = 0;
                if (_row["WorkType"] != DBNull.Value) _workType = Convert.ToInt32(_row["WorkType"]); else _workType = 0;
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
            _workDate = DateTime.Now;
            _workQty = 0;
            _worked = 0;
            _workType = 0; 
            _remarks = "";
    }

        #endregion

        #region Methods
        
        public static DataRow Insert(int OrderIdx, int CuttedIdx, string WorkOrderIdx, string OutOrderIdx, string OrdColorIdx, int OrdSizeIdx, int OrdQty,
            DateTime WorkDate, int WorkQty, int Worked, int WorkType, string Remarks, int Handler)
        {
            DataRow row = Data.SewingData.Insert(OrderIdx, CuttedIdx, WorkOrderIdx, OutOrderIdx, OrdColorIdx, OrdSizeIdx, OrdQty,
                                                WorkDate, WorkQty, Worked, WorkType, Remarks, Handler);

            return row;
        }
         
        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey,
                                    Dictionary<CommonValues.KeyName, string> SearchString)
        {
            DataSet ds = new DataSet();
            
            ds = Data.SewingData.Getlist(SearchKey, SearchString); 
                        
            return ds;
        }

        public static DataSet Getlist(int OrderIdx)
        {
            DataSet ds = new DataSet();

            ds = Data.SewingData.Getlist(OrderIdx);

            return ds;
        }

        public bool Update()
        {
            bool blRtn;
            blRtn = Data.SewingData.Update(_idx, _outOrderIdx, _workDate, _workQty, _worked, _workType, 
                                            _remarks);
            return blRtn;
        }


        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.SewingData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
