using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class Inspecting
    {
        #region Members

        private int _idx;
        private int _orderIdx;
        private int _sewIdx;
        private string _workOrderIdx;
        private int _tDName;
        
        private DateTime _inspRequestedDate;
        private DateTime _inspDate;
        private DateTime _inspCompletedDate;
        
        private string _result;
        private string _action;
        private int _reorder;

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
        public int SewIdx
        {
            get { return _sewIdx; }
            set { _sewIdx = value; }
        }
        //
        public string WorkOrderIdx
        {
            get { return _workOrderIdx; }
            set { _workOrderIdx = value; }
        }
        //
        public int TDName
        {
            get { return _tDName; }
            set { _tDName = value; }
        }
        
        public DateTime InspRequestedDate
        {
            get { return _inspRequestedDate; }
            set { _inspRequestedDate = value; }
        }
        public DateTime InspDate
        {
            get { return _inspDate; }
            set { _inspDate = value; }
        }
        public DateTime InspCompletedDate
        {
            get { return _inspCompletedDate; }
            set { _inspCompletedDate = value; }
        }
        
        public string Result
        {
            get { return _result; }
            set { _result = value; }
        }
        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }
        public int Reorder
        {
            get { return _reorder; }
            set { _reorder = value; }
        }

        #endregion

        #region Constructor 

        public Inspecting(int Idx)
        {
            _row = Dev.Data.InspectingData.Get(Idx);
            
            if (_row != null)
            {
                _idx = Convert.ToInt32(_row["Idx"]);
                if (_row["OrderIdx"] != DBNull.Value) _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
                if (_row["SewIdx"] != DBNull.Value) _sewIdx = Convert.ToInt32(_row["SewIdx"]);
                if (_row["WorkOrderIdx"] != DBNull.Value) _workOrderIdx = Convert.ToString(_row["WorkOrderIdx"]);
                if (_row["TDName"] != DBNull.Value) _tDName = Convert.ToInt32(_row["TDName"]);
                
                if (_row["InspRequestedDate"] != DBNull.Value) _inspRequestedDate = Convert.ToDateTime(_row["InspRequestedDate"]);
                if (_row["InspDate"] != DBNull.Value) _inspDate = Convert.ToDateTime(_row["InspDate"]);
                if (_row["InspCompletedDate"] != DBNull.Value) _inspCompletedDate = Convert.ToDateTime(_row["InspCompletedDate"]);
                
                if (_row["Result"] != DBNull.Value) _result = _row["Result"].ToString(); else _result = "";
                if (_row["Action"] != DBNull.Value) _action = _row["Action"].ToString(); else _action = "";
                if (_row["Reorder"] != DBNull.Value) _reorder = Convert.ToInt32(_row["Reorder"]); else _reorder = 0;
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
            _sewIdx = 0;
            _workOrderIdx = "";
            _tDName = 0;
            _inspRequestedDate = DateTime.Now;
            _inspDate = DateTime.Now;
            _inspCompletedDate = DateTime.Now;
            _result = "";
            _action = "";
            _reorder = 0;
    }

        #endregion

        #region Methods
        
        public static DataRow Insert(int OrderIdx, int SewIdx, string WorkOrderIdx, int TDName,
            DateTime InspRequestedDate, string Result, string Action, int Reorder, int Handler)
        {
            DataRow row = Data.InspectingData.Insert(OrderIdx, SewIdx, WorkOrderIdx, TDName, InspRequestedDate, 
                                                Result, Action, Reorder, Handler);

            return row;
        }
         
        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey,
                                    Dictionary<CommonValues.KeyName, string> SearchString)
        {
            DataSet ds = new DataSet();
            
            ds = Data.InspectingData.Getlist(SearchKey, SearchString); 
                        
            return ds;
        }
        public static DataSet Getlist(int Idx)
        {
            DataSet ds = new DataSet();

            ds = Data.InspectingData.Getlist(Idx);

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
            blRtn = Data.InspectingData.Update(_idx, _inspDate, _inspCompletedDate);
            return blRtn;
        }

        public static bool Update2(int Idx, string WorkOrderIdx, string Result, string Action, int Reorder, int Status)
        {
            bool blRtn;
            blRtn = Data.InspectingData.Update(Idx, WorkOrderIdx, Result, Action, Reorder, Status);
            return blRtn;
        }

        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.InspectingData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
