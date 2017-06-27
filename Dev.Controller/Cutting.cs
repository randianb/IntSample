using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class Cutting
    {
        #region Members

        private int _idx;
        private int _orderIdx;
        private string _workOrderIdx;
        private string _ordColorIdx;
        private int _ordSizeIdx;
        private double _ordYds;
        private int _ordQty;
        private DateTime _ordDate;
        private DateTime _cuttedDate;

        private string _cuttedNo;
        private int _cuttedQty;
        private int _cuttedPQty;
        private int _fabricIdx;
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
        //
        public string WorkOrderIdx
        {
            get { return _workOrderIdx; }
            set { _workOrderIdx = value; }
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
        public double OrdYds
        {
            get { return _ordYds; }
            set { _ordYds = value; }
        }
        //
        public int OrdQty
        {
            get { return _ordQty; }
            set { _ordQty = value; }
        }
        public DateTime OrdDate
        {
            get { return _ordDate; }
            set { _ordDate = value; }
        }
       
        public DateTime CuttedDate
        {
            get { return _cuttedDate; }
            set { _cuttedDate = value; }
        }
       
        public string CuttedNo
        {
            get { return _cuttedNo; }
            set { _cuttedNo = value; }
        }
        public int CuttedQty
        {
            get { return _cuttedQty; }
            set { _cuttedQty = value; }
        }
        public int CuttedPQty
        {
            get { return _cuttedPQty; }
            set { _cuttedPQty = value; }
        }
        public int FabricIdx
        {
            get { return _fabricIdx; }
            set { _fabricIdx = value; }
        }

        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }
        
        #endregion

        #region Constructor 

        public Cutting(int Idx)
        {
            _row = Dev.Data.CuttingData.Get(Idx);
            
            if (_row != null)
            {
                _idx = Convert.ToInt32(_row["Idx"]);
                if (_row["OrderIdx"] != DBNull.Value) _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
                if (_row["WorkOrderIdx"] != DBNull.Value) _workOrderIdx = Convert.ToString(_row["WorkOrderIdx"]);
                if (_row["OrdColorIdx"] != DBNull.Value) _ordColorIdx = Convert.ToString(_row["OrdColorIdx"]);
                if (_row["OrdSizeIdx"] != DBNull.Value) _ordSizeIdx = Convert.ToInt32(_row["OrdSizeIdx"]);
                if (_row["OrdYds"] != DBNull.Value) _ordYds = Convert.ToInt32(_row["OrdYds"]); else _ordYds = 0f;
                if (_row["OrdQty"] != DBNull.Value) _ordQty = Convert.ToInt32(_row["OrdQty"]); else _ordQty = 0;

                if (_row["OrdDate"] != DBNull.Value) _ordDate = Convert.ToDateTime(_row["OrdDate"]);
                if (_row["CuttedDate"] != DBNull.Value) _cuttedDate = Convert.ToDateTime(_row["CuttedDate"]);

                if (_row["CuttedNo"] != DBNull.Value) _cuttedNo = _row["CuttedNo"].ToString();
                if (_row["CuttedQty"] != DBNull.Value) _cuttedQty = Convert.ToInt32(_row["CuttedQty"]); else _cuttedQty = 0;
                if (_row["CuttedPQty"] != DBNull.Value) _cuttedPQty = Convert.ToInt32(_row["CuttedPQty"]); else _cuttedPQty = 0;
                if (_row["FabricIdx"] != DBNull.Value) _fabricIdx = Convert.ToInt32(_row["FabricIdx"]); else _fabricIdx = 0;
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
            _workOrderIdx = "";
            _ordColorIdx = "";
            _ordSizeIdx = 0;
            _ordYds = 0f;
            _ordQty = 0;
            _ordDate = DateTime.Now;
            _cuttedDate = DateTime.Now;
            _cuttedNo = "";
            _cuttedQty = 0;
            _cuttedPQty = 0;
            _fabricIdx = 0;
            _remarks = "";
    }

        #endregion

        #region Methods
        
        public static DataRow Insert(int OrderIdx, string WorkOrderIdx, string OrdColorIdx, int OrdSizeIdx, double OrdYds, int OrdQty,
            int FabricIdx, string Remarks, int Handler)
        {
            DataRow row = Data.CuttingData.Insert(OrderIdx, WorkOrderIdx, OrdColorIdx, OrdSizeIdx, OrdYds, OrdQty,
                                                FabricIdx, Remarks, Handler);

            return row;
        }
        
        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey,
                                    Dictionary<CommonValues.KeyName, string> SearchString)
        {
            DataSet ds = new DataSet();
            
            ds = Data.CuttingData.Getlist(SearchKey, SearchString); 
                        
            return ds;
        }

        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey)
        {
            DataSet ds = new DataSet();

            ds = Data.CuttingData.Getlist(SearchKey);

            return ds;
        }

        public bool Update()
        {
            bool blRtn;
            blRtn = Data.CuttingData.Update(_idx, _cuttedDate, _cuttedNo, _cuttedQty, _cuttedPQty, 
                                            _fabricIdx,  _remarks);
            return blRtn;
        }


        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.CuttingData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
