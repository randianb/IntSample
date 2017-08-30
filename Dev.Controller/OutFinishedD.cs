using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class OutFinishedD
    {
        #region Members

        private int _idx;
        private int _pidx;
        private int _orderIdx;
        private string _workOrderIdx;
        private int _inspectionIdx;
        private string _ordColorIdx;
        private int _ordSizeIdx;
        private int _outQty;
        
        private DataRow _row;

        #endregion

        #region Property 

        //ID
        public int Idx
        {
            get { return _idx; }
            set { _idx = value; }
        }
        public int PIdx
        {
            get { return _pidx; }
            set { _pidx = value; }
        }
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

        public int InspectionIdx
        {
            get { return _inspectionIdx; }
            set { _inspectionIdx = value; }
        }
        
        //
        public string OrdColorIdx
        {
            get { return _ordColorIdx; }
            set { _ordColorIdx = value; }
        }
        public int OrdSizeIdx
        {
            get { return _ordSizeIdx; }
            set { _ordSizeIdx = value; }
        }
        public int OutQty
        {
            get { return _outQty; }
            set { _outQty = value; }
        }
                
        #endregion

        #region Constructor 

        public OutFinishedD(int Idx)
        {
            _row = Dev.Data.OutFinishedDData.Get(Idx);
            
            if (_row != null)
            {
                _idx = Convert.ToInt32(_row["Idx"]);
                if (_row["PIdx"] != DBNull.Value) _pidx = Convert.ToInt32(_row["PIdx"]);
                if (_row["OrderIdx"] != DBNull.Value) _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
                if (_row["WorkOrderIdx"] != DBNull.Value) _workOrderIdx = Convert.ToString(_row["WorkOrderIdx"]);
                
                if (_row["InspectionIdx"] != DBNull.Value) _inspectionIdx = Convert.ToInt32(_row["InspectionIdx"]);
                if (_row["OrdColorIdx"] != DBNull.Value) _ordColorIdx = Convert.ToString(_row["OrdColorIdx"]);

                if (_row["OrdSizeIdx"] != DBNull.Value) _ordSizeIdx = Convert.ToInt32(_row["OrdSizeIdx"]); else _ordSizeIdx = 0;
                if (_row["OutQty"] != DBNull.Value) _outQty = Convert.ToInt32(_row["OutQty"]); else _outQty = 0;
                                
            }
            else
            {
                Initializer();
            }
        }

        private void Initializer()
        {
            _idx = 0;
            _pidx = 0;
            _orderIdx = 0;
            _workOrderIdx = "";
            _inspectionIdx = 0;
            _ordColorIdx = "";
            _ordSizeIdx = 0;
            _outQty = 0;
            
    }

        #endregion

        #region Methods
        
        public static DataRow Insert(int pIdx, int OrderIdx, string WorkOrderIdx, int InspectionIdx, string OrdColorIdx,
                                int OrdSizeIdx, int OutQty, int Handler)
        {
            DataRow row = Data.OutFinishedDData.Insert(pIdx, OrderIdx, WorkOrderIdx, InspectionIdx, OrdColorIdx,
                                OrdSizeIdx, OutQty, Handler);

            return row;
        }

        //public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey,
        //                            Dictionary<CommonValues.KeyName, string> SearchString,
        //                            string OutDateFrom, string OutDateTo,
        //                              string RcvdDateFrom, string RcvdDateTo)
        //{
        //    DataSet ds = new DataSet();

        //    ds = Data.OutFinishedDData.Getlist(SearchKey, SearchString,
        //                                       OutDateFrom,  OutDateTo,
        //                               RcvdDateFrom,  RcvdDateTo); 

        //    return ds;
        //}

        public static DataSet Getlist(int pIdx)
        {
            DataSet ds = new DataSet();

            ds = Data.OutFinishedDData.Getlist(pIdx);

            return ds;
        }

        public bool Update()
        {
            bool blRtn;
            blRtn = Data.OutFinishedDData.Update(_idx, _outQty);
            return blRtn;
        }


        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.OutFinishedDData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
