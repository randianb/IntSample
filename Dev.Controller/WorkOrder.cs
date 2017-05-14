using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;


namespace Dev.Controller
{
    public class WorkOrder
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

        private string _attachedUrl1;
        private string _attachedUrl2;
        private string _attachedUrl3;
        private string _attachedUrl4;
        private string _attachedUrl5; 

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

        #endregion

        #region Constructor 

        public WorkOrder(int Idx)
        {
            //_row = Dev.Data.PatternData.Get(Idx);
            
            //if (_row != null)
            //{
            //    _idx = Convert.ToInt32(_row["Idx"]);
            //    if (_row["OrderIdx"] != DBNull.Value) _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
            //    if (_row["WorkOrderIdx"] != DBNull.Value) _workOrderIdx = Convert.ToString(_row["WorkOrderIdx"]);

            //    if (_row["OrdSizeIdx"] != DBNull.Value) _ordSizeIdx = Convert.ToInt32(_row["OrdSizeIdx"]);
            //    if (_row["TechpackDate"] != DBNull.Value) _techpackDate = Convert.ToDateTime(_row["TechpackDate"]);
            //    if (_row["RequestedDate"] != DBNull.Value) _requestedDate = Convert.ToDateTime(_row["RequestedDate"]);
            //    if (_row["Requested"] != DBNull.Value) _requested = Convert.ToInt32(_row["Requested"]);
            //    if (_row["ConfirmedDate"] != DBNull.Value) _confirmedDate = Convert.ToDateTime(_row["ConfirmedDate"]);
            //    if (_row["Confirmed"] != DBNull.Value) _confirmed = Convert.ToInt32(_row["Confirmed"]);
                
            //    if (_row["CompletedDate"] != DBNull.Value) _completedDate = Convert.ToDateTime(_row["CompletedDate"]);
            //    if (_row["SentDate"] != DBNull.Value) _sentDate = Convert.ToDateTime(_row["SentDate"]);
            //    if (_row["Received"] != DBNull.Value) _received = Convert.ToInt32(_row["Received"]);
            //    if (_row["Remarks"] != DBNull.Value) _remarks = _row["Remarks"].ToString();
                
            //    if (_row["Attached1"] != DBNull.Value) _attached1 = _row["Attached1"].ToString();
            //    if (_row["Attached2"] != DBNull.Value) _attached2 = _row["Attached2"].ToString();
            //    if (_row["Attached3"] != DBNull.Value) _attached3 = _row["Attached3"].ToString();
            //    if (_row["Attached4"] != DBNull.Value) _attached4 = _row["Attached4"].ToString();
            //    if (_row["Attached5"] != DBNull.Value) _attached5 = _row["Attached5"].ToString();
                
            //    if (_row["AttachedUrl1"] != DBNull.Value) _attachedUrl1 = _row["AttachedUrl1"].ToString();
            //    if (_row["AttachedUrl2"] != DBNull.Value) _attachedUrl2 = _row["AttachedUrl2"].ToString();
            //    if (_row["AttachedUrl3"] != DBNull.Value) _attachedUrl3 = _row["AttachedUrl3"].ToString();
            //    if (_row["AttachedUrl4"] != DBNull.Value) _attachedUrl4 = _row["AttachedUrl4"].ToString();
            //    if (_row["AttachedUrl5"] != DBNull.Value) _attachedUrl5 = _row["AttachedUrl5"].ToString();

            //}
            //else
            //{
            //    Initializer();
            //}
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

            _attachedUrl1 = "";
            _attachedUrl2 = "";
            _attachedUrl3 = "";
            _attachedUrl4 = "";
            _attachedUrl5 = "";
    }

        #endregion

        #region Methods
        
        //public static DataRow Insert(int OrderIdx, string WorkOrderIdx, int OrdSizeIdx, DateTime TechpackDate, DateTime RequestedDate,
        //    int Requested, string Comments, string Attached1, string Attached2, string Attached3, string Attached4, string Attached5,
        //    string AttachedUrl1, string AttachedUrl2, string AttachedUrl3, string AttachedUrl4, string AttachedUrl5, int Handler)
        //{
        //    DataRow row = Data.PatternData.Insert(OrderIdx, WorkOrderIdx, OrdSizeIdx, TechpackDate, RequestedDate,
        //    Requested, Comments, Attached1, Attached2, Attached3, Attached4, Attached5,
        //    AttachedUrl1, AttachedUrl2, AttachedUrl3, AttachedUrl4, AttachedUrl5, Handler);

        //    return row;
        //}
        
        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey, string WorkOrderIdx, string TicketDate)
        {
            DataSet ds = new DataSet();
            
            ds = Data.WorkOrderData.Getlist(SearchKey, WorkOrderIdx, TicketDate); 
                        
            return ds;
        }
        
        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey, Dictionary<CommonValues.KeyName, string> SearchString)
        {
            DataSet ds = new DataSet();

            ds = Data.WorkOrderData.Getlist(SearchKey, SearchString);

            return ds;
        }
        public static DataRow Getlist(string Operation, string WorkOrderIdx) 
        {
            DataRow dr = Data.WorkOrderData.Getlist(Operation, WorkOrderIdx);

            return dr;
        }
        public static bool Update(string WorkOrderIdx, DateTime Start, DateTime End, double Progress, DateTime TicketDate, string Qrcode,
            int Modified, int Status) 
        {
            bool blRtn;
            blRtn = Data.WorkOrderData.Update(WorkOrderIdx, Start, End, Progress, TicketDate, Qrcode, Modified, Status);
            return blRtn;
        }
        
        public static bool Update(string WorkOrderIdx, DateTime Start, DateTime End, double Progress, int Modified)
        {
            bool blRtn;
            blRtn = Data.WorkOrderData.Update(WorkOrderIdx, Start, End, Progress, Modified);
            return blRtn;
        }
        public static bool Update(string WorkOrderIdx, DateTime TicketDate, int Status, string Qrcode, int Modified)
        {
            bool blRtn;
            blRtn = Data.WorkOrderData.Update(WorkOrderIdx, TicketDate, Status, Qrcode, Modified);
            return blRtn;
        }
        public static bool Update(string SQL)
        {
            bool blRtn;
            blRtn = Data.WorkOrderData.Update(SQL);
            return blRtn;
        }
        //public static bool Delete(string condition)
        //{
        //    bool blRtn;
        //    blRtn = Data.PatternData.Delete(condition);
        //    return blRtn;
        //}

        

        #endregion
    }
}
