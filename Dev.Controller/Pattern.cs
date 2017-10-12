using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class Pattern
    {
        #region Members

        private int _idx;
        private int _orderIdx;
        private string _workOrderIdx;
        private int _ordSizeIdx;
        private DateTime _techpackDate;
        private DateTime _requestedDate;
        private int _requested;
        private DateTime _confirmedDate;
        private int _confirmed;

        private DateTime _completedDate;
        private DateTime _sentDate;
        private int _received;
        private string _remarks;
        private string _attached1;
        private string _attached2;
        private string _attached3;
        private string _attached4;
        private string _attached5;
        private string _attached6;
        private string _attached7;
        private string _attached8;
        private string _attached9;

        private string _attachedUrl1;
        private string _attachedUrl2;
        private string _attachedUrl3;
        private string _attachedUrl4;
        private string _attachedUrl5;
        private string _attachedUrl6;
        private string _attachedUrl7;
        private string _attachedUrl8;
        private string _attachedUrl9;

        private string _attached21;
        private string _attached22;
        private string _attached23;
        private string _attached24;

        private string _attachedUrl21;
        private string _attachedUrl22;
        private string _attachedUrl23;
        private string _attachedUrl24;

        private DataRow _row;

        #endregion

        #region Property Header 

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
        public int OrdSizeIdx
        {
            get { return _ordSizeIdx; }
            set { _ordSizeIdx = value; }
        }
        public DateTime TechpackDate
        {
            get { return _techpackDate; }
            set { _techpackDate = value; }
        }
       
        public DateTime RequestedDate
        {
            get { return _requestedDate; }
            set { _requestedDate = value; }
        }
        public int Requested
        {
            get { return _requested; }
            set { _requested = value; }
        }
        public DateTime ConfirmedDate
        {
            get { return _confirmedDate; }
            set { _confirmedDate = value; }
        }
        public int Confirmed
        {
            get { return _confirmed; }
            set { _confirmed = value; }
        }
        
        public DateTime CompletedDate
        {
            get { return _completedDate; }
            set { _completedDate = value; }
        }
        public DateTime SentDate
        {
            get { return _sentDate; }
            set { _sentDate = value; }
        }
        public int Received
        {
            get { return _received; }
            set { _received = value; }
        }
        
        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; }
        }

        #endregion

        #region Property Attachment 

        public string Attached1
        {
            get { return _attached1; }
            set { _attached1 = value; }
        }
        public string Attached2
        {
            get { return _attached2; }
            set { _attached2 = value; }
        }
        public string Attached3
        {
            get { return _attached3; }
            set { _attached3 = value; }
        }
        public string Attached4
        {
            get { return _attached4; }
            set { _attached4 = value; }
        }
        public string Attached5
        {
            get { return _attached5; }
            set { _attached5 = value; }
        }
        public string Attached6
        {
            get { return _attached6; }
            set { _attached6 = value; }
        }
        public string Attached7
        {
            get { return _attached7; }
            set { _attached7 = value; }
        }
        public string Attached8
        {
            get { return _attached8; }
            set { _attached8 = value; }
        }
        public string Attached9
        {
            get { return _attached9; }
            set { _attached9 = value; }
        }

        public string AttachedUrl1
        {
            get { return _attachedUrl1; }
            set { _attachedUrl1 = value; }
        }
        public string AttachedUrl2
        {
            get { return _attachedUrl2; }
            set { _attachedUrl2 = value; }
        }
        public string AttachedUrl3
        {
            get { return _attachedUrl3; }
            set { _attachedUrl3 = value; }
        }
        public string AttachedUrl4
        {
            get { return _attachedUrl4; }
            set { _attachedUrl4 = value; }
        }
        public string AttachedUrl5
        {
            get { return _attachedUrl5; }
            set { _attachedUrl5 = value; }
        }
        public string AttachedUrl6
        {
            get { return _attachedUrl6; }
            set { _attachedUrl6 = value; }
        }
        public string AttachedUrl7
        {
            get { return _attachedUrl7; }
            set { _attachedUrl7 = value; }
        }
        public string AttachedUrl8
        {
            get { return _attachedUrl8; }
            set { _attachedUrl8 = value; }
        }
        public string AttachedUrl9
        {
            get { return _attachedUrl9; }
            set { _attachedUrl9 = value; }
        }

        public string Attached21
        {
            get { return _attached21; }
            set { _attached21 = value; }
        }
        public string Attached22
        {
            get { return _attached22; }
            set { _attached22 = value; }
        }
        public string Attached23
        {
            get { return _attached23; }
            set { _attached23 = value; }
        }
        public string Attached24
        {
            get { return _attached24; }
            set { _attached24 = value; }
        }
        
        public string AttachedUrl21
        {
            get { return _attachedUrl21; }
            set { _attachedUrl21 = value; }
        }
        public string AttachedUrl22
        {
            get { return _attachedUrl22; }
            set { _attachedUrl22 = value; }
        }
        public string AttachedUrl23
        {
            get { return _attachedUrl23; }
            set { _attachedUrl23 = value; }
        }
        public string AttachedUrl24
        {
            get { return _attachedUrl24; }
            set { _attachedUrl24 = value; }
        }
        

        #endregion

        #region Constructor 

        public Pattern(int Idx)
        {
            _row = Dev.Data.PatternData.Get(Idx);
            
            if (_row != null)
            {
                _idx = Convert.ToInt32(_row["Idx"]);
                if (_row["OrderIdx"] != DBNull.Value) _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
                if (_row["WorkOrderIdx"] != DBNull.Value) _workOrderIdx = Convert.ToString(_row["WorkOrderIdx"]);

                if (_row["OrdSizeIdx"] != DBNull.Value) _ordSizeIdx = Convert.ToInt32(_row["OrdSizeIdx"]);
                if (_row["TechpackDate"] != DBNull.Value) _techpackDate = Convert.ToDateTime(_row["TechpackDate"]);
                if (_row["RequestedDate"] != DBNull.Value) _requestedDate = Convert.ToDateTime(_row["RequestedDate"]);
                if (_row["Requested"] != DBNull.Value) _requested = Convert.ToInt32(_row["Requested"]); else _requested = 0;
                if (_row["ConfirmedDate"] != DBNull.Value) _confirmedDate = Convert.ToDateTime(_row["ConfirmedDate"]); else _confirmedDate = new DateTime(2000, 1, 1);
                if (_row["Confirmed"] != DBNull.Value) _confirmed = Convert.ToInt32(_row["Confirmed"]); else _confirmed = 0;

                if (_row["CompletedDate"] != DBNull.Value) _completedDate = Convert.ToDateTime(_row["CompletedDate"]); else _completedDate = new DateTime(2000, 1, 1);
                if (_row["SentDate"] != DBNull.Value) _sentDate = Convert.ToDateTime(_row["SentDate"]); else _sentDate = new DateTime(2000, 1, 1);
                if (_row["Received"] != DBNull.Value) _received = Convert.ToInt32(_row["Received"]); else _received = 0;
                if (_row["Remarks"] != DBNull.Value) _remarks = _row["Remarks"].ToString(); else _remarks = ""; 
                
                if (_row["Attached1"] != DBNull.Value) _attached1 = _row["Attached1"].ToString();
                if (_row["Attached2"] != DBNull.Value) _attached2 = _row["Attached2"].ToString();
                if (_row["Attached3"] != DBNull.Value) _attached3 = _row["Attached3"].ToString();
                if (_row["Attached4"] != DBNull.Value) _attached4 = _row["Attached4"].ToString();
                if (_row["Attached5"] != DBNull.Value) _attached5 = _row["Attached5"].ToString();
                if (_row["Attached6"] != DBNull.Value) _attached6 = _row["Attached6"].ToString();
                if (_row["Attached7"] != DBNull.Value) _attached7 = _row["Attached7"].ToString();
                if (_row["Attached8"] != DBNull.Value) _attached8 = _row["Attached8"].ToString();
                if (_row["Attached9"] != DBNull.Value) _attached9 = _row["Attached9"].ToString();

                if (_row["AttachedUrl1"] != DBNull.Value) _attachedUrl1 = _row["AttachedUrl1"].ToString();
                if (_row["AttachedUrl2"] != DBNull.Value) _attachedUrl2 = _row["AttachedUrl2"].ToString();
                if (_row["AttachedUrl3"] != DBNull.Value) _attachedUrl3 = _row["AttachedUrl3"].ToString();
                if (_row["AttachedUrl4"] != DBNull.Value) _attachedUrl4 = _row["AttachedUrl4"].ToString();
                if (_row["AttachedUrl5"] != DBNull.Value) _attachedUrl5 = _row["AttachedUrl5"].ToString();
                if (_row["AttachedUrl6"] != DBNull.Value) _attachedUrl6 = _row["AttachedUrl6"].ToString();
                if (_row["AttachedUrl7"] != DBNull.Value) _attachedUrl7 = _row["AttachedUrl7"].ToString();
                if (_row["AttachedUrl8"] != DBNull.Value) _attachedUrl8 = _row["AttachedUrl8"].ToString();
                if (_row["AttachedUrl9"] != DBNull.Value) _attachedUrl9 = _row["AttachedUrl9"].ToString();

                if (_row["Attached21"] != DBNull.Value) _attached1 = _row["Attached21"].ToString();
                if (_row["Attached22"] != DBNull.Value) _attached2 = _row["Attached22"].ToString();
                if (_row["Attached23"] != DBNull.Value) _attached3 = _row["Attached23"].ToString();
                if (_row["Attached24"] != DBNull.Value) _attached4 = _row["Attached24"].ToString();
                
                if (_row["AttachedUrl21"] != DBNull.Value) _attachedUrl21 = _row["AttachedUrl21"].ToString();
                if (_row["AttachedUrl22"] != DBNull.Value) _attachedUrl22 = _row["AttachedUrl22"].ToString();
                if (_row["AttachedUrl23"] != DBNull.Value) _attachedUrl23 = _row["AttachedUrl23"].ToString();
                if (_row["AttachedUrl24"] != DBNull.Value) _attachedUrl24 = _row["AttachedUrl24"].ToString();
                
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
            _ordSizeIdx = 0;
            _techpackDate = DateTime.Now;
            _requestedDate = DateTime.Now;
            _requested = 0;

            _confirmedDate = DateTime.Now; 
            _confirmed = 0;
            _confirmedDate = DateTime.Now;
            _confirmed = 0;

            _completedDate = DateTime.Now;
            _sentDate = DateTime.Now;
            _received = 0;
            _remarks = "";

            _attached1 = "";
            _attached2 = "";
            _attached3 = "";
            _attached4 = "";
            _attached5 = "";
            _attached6 = "";
            _attached7 = "";
            _attached8 = "";
            _attached9 = "";

            _attachedUrl1 = "";
            _attachedUrl2 = "";
            _attachedUrl3 = "";
            _attachedUrl4 = "";
            _attachedUrl5 = "";
            _attachedUrl6 = "";
            _attachedUrl7 = "";
            _attachedUrl8 = "";
            _attachedUrl9 = "";

            _attached21 = "";
            _attached22 = "";
            _attached23 = "";
            _attached24 = "";
            
            _attachedUrl21 = "";
            _attachedUrl22 = "";
            _attachedUrl23 = "";
            _attachedUrl24 = "";
        }

        #endregion

        #region Methods
        
        public static DataRow Insert(int OrderIdx, string WorkOrderIdx, int OrdSizeIdx, DateTime TechpackDate, DateTime RequestedDate,
            int Requested, string Comments, 
            string Attached1, string Attached2, string Attached3, string Attached4, string Attached5,
            string Attached6, string Attached7, string Attached8, string Attached9,
            string AttachedUrl1, string AttachedUrl2, string AttachedUrl3, string AttachedUrl4, string AttachedUrl5,
            string AttachedUrl6, string AttachedUrl7, string AttachedUrl8, string AttachedUrl9,
            int Handler,
            int IsPattern, int IsConsum,
            int OrdSizeIdx2, int OrdSizeIdx3, int OrdSizeIdx4, int OrdSizeIdx5, int OrdSizeIdx6, int OrdSizeIdx7, int OrdSizeIdx8,
            int Confirmed, int Received)
        {
            DataRow row = Data.PatternData.Insert(OrderIdx, WorkOrderIdx, OrdSizeIdx, TechpackDate, RequestedDate,
            Requested, Comments, Attached1, Attached2, Attached3, Attached4, Attached5, Attached6, Attached7, Attached8, Attached9,
            AttachedUrl1, AttachedUrl2, AttachedUrl3, AttachedUrl4, AttachedUrl5, AttachedUrl6, AttachedUrl7, AttachedUrl8, AttachedUrl9, Handler,
            IsPattern, IsConsum,
            OrdSizeIdx2, OrdSizeIdx3, OrdSizeIdx4, OrdSizeIdx5, OrdSizeIdx6, OrdSizeIdx7, OrdSizeIdx8,
            Confirmed, Received);

            return row;
        }
        
        public static DataSet Getlist(int KeyCount, Dictionary<CommonValues.KeyName, int> SearchKey, string fileno, string style,
                            int OptionCheck1, int OptionCheck2, int OptionCheck3, int OptionCheck4, int OptionCheck5, int OptionCheck6)
        {
            DataSet ds = new DataSet();
            
            ds = Data.PatternData.Getlist(SearchKey, fileno, style,
                                    OptionCheck1, OptionCheck2, OptionCheck3, OptionCheck4, OptionCheck5, OptionCheck6); 
                        
            return ds;
        }

        public static DataSet Getlist(int KeyCount, Dictionary<CommonValues.KeyName, int> SearchKey, string fileno, string style, int UserIdx,
                            int OptionCheck1, int OptionCheck2, int OptionCheck3, int OptionCheck4, int OptionCheck5, int OptionCheck6)
        {
            DataSet ds = new DataSet();

            ds = Data.PatternData.Getlist(SearchKey, fileno, style, UserIdx,
                                    OptionCheck1, OptionCheck2, OptionCheck3, OptionCheck4, OptionCheck5, OptionCheck6);

            return ds;
        }

        public static DataSet Print(Dictionary<CommonValues.KeyName, string> SearchString, Dictionary<CommonValues.KeyName, int> SearchKey,
            DateTime dtFromDate, DateTime dtToDate)
        {
            DataSet ds = new DataSet();

            ds = Data.PatternData.Print(SearchString, SearchKey, dtFromDate, dtToDate);

            return ds;
        }

        public bool Update()
        {
            bool blRtn;
            blRtn = Data.PatternData.Update(_idx, _techpackDate, _requestedDate, _requested, _confirmedDate, _confirmed, 
                _completedDate, _sentDate, _received, _remarks 
                );
            return blRtn;
        }

        public bool Updatefiles()
        {
            bool blRtn;
            blRtn = Data.PatternData.Update(_idx, _attached21, _attached22, _attached23, _attached24,
                _attachedUrl21, _attachedUrl22, _attachedUrl23, _attachedUrl24
                );
            return blRtn;
        }

        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.PatternData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
