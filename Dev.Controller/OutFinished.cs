using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class OutFinished
    {
        #region Members

        private int _idx;
        private string _workOrderIdx;
        private int _out1;
        private int _out2;
        private string _delivered;
        private int _received1;
        private int _received2;

        private DateTime _outDate;
        private DateTime _rcvdDate;
        
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
        public string WorkOrderIdx
        {
            get { return _workOrderIdx; }
            set { _workOrderIdx = value; }
        }
        
        //
        public int Out1
        {
            get { return _out1; }
            set { _out1 = value; }
        }
        //
        public int Out2
        {
            get { return _out2; }
            set { _out2 = value; }
        }
        //
        public string Delivered
        {
            get { return _delivered; }
            set { _delivered = value; }
        }
        public int Received1
        {
            get { return _received1; }
            set { _received1 = value; }
        }
        public int Received2
        {
            get { return _received2; }
            set { _received2 = value; }
        }

        public DateTime OutDate
        {
            get { return _outDate; }
            set { _outDate = value; }
        }
        public DateTime RcvdDate
        {
            get { return _rcvdDate; }
            set { _rcvdDate = value; }
        }

        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }
        
        #endregion

        #region Constructor 

        public OutFinished(int Idx)
        {
            _row = Dev.Data.OutFinishedData.Get(Idx);
            
            if (_row != null)
            {
                _idx = Convert.ToInt32(_row["Idx"]);
                if (_row["WorkOrderIdx"] != DBNull.Value) _workOrderIdx = Convert.ToString(_row["WorkOrderIdx"]);
                if (_row["Out1"] != DBNull.Value) _out1 = Convert.ToInt32(_row["Out1"]);
                if (_row["Out2"] != DBNull.Value) _out2 = Convert.ToInt32(_row["Out2"]);
                if (_row["Delivered"] != DBNull.Value) _delivered = Convert.ToString(_row["Delivered"]);

                if (_row["Received1"] != DBNull.Value) _received1 = Convert.ToInt32(_row["Received1"]); else _received1 = 0;
                if (_row["Received2"] != DBNull.Value) _received2 = Convert.ToInt32(_row["Received2"]); else _received2 = 0;

                if (_row["OutDate"] != DBNull.Value) _outDate = Convert.ToDateTime(_row["OutDate"]);
                if (_row["RcvdDate"] != DBNull.Value) _rcvdDate = Convert.ToDateTime(_row["RcvdDate"]);
                
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
            _workOrderIdx = "";
            _out1 = 0;
            _out2 = 0;
            _delivered = "";
            _received1 = 0;
            _received2 = 0;
            _outDate = DateTime.Now;
            _rcvdDate = DateTime.Now;
            _remarks = "";
    }

        #endregion

        #region Methods
        
        public static DataRow Insert(string WorkOrderIdx)
        {
            DataRow row = Data.OutFinishedData.Insert(WorkOrderIdx);

            return row;
        }
         
        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey,
                                    Dictionary<CommonValues.KeyName, string> SearchString,
                                    string OutDateFrom, string OutDateTo,
                                      string RcvdDateFrom, string RcvdDateTo)
        {
            DataSet ds = new DataSet();
            
            ds = Data.OutFinishedData.Getlist(SearchKey, SearchString,
                                               OutDateFrom,  OutDateTo,
                                       RcvdDateFrom,  RcvdDateTo); 
                        
            return ds;
        }

        public static DataSet GetlistD(Dictionary<CommonValues.KeyName, int> SearchKey,
                                    Dictionary<CommonValues.KeyName, string> SearchString,
                                    string OutDateFrom, string OutDateTo,
                                      string RcvdDateFrom, string RcvdDateTo)
        {
            DataSet ds = new DataSet();

            ds = Data.OutFinishedData.GetlistD(SearchKey, SearchString,
                                               OutDateFrom, OutDateTo,
                                       RcvdDateFrom, RcvdDateTo);

            return ds;
        }

        //public static DataSet Getlist(int OrderIdx)
        //{
        //    DataSet ds = new DataSet();

        //    ds = Data.SewingData.Getlist(OrderIdx);

        //    return ds;
        //}

        public bool Update()
        {
            bool blRtn;
            blRtn = Data.OutFinishedData.Update(_idx, _out1, _out2, _delivered, _received1, _received2, _outDate, _rcvdDate, 
                                            _remarks);
            return blRtn;
        }


        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.OutFinishedData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
