using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class Worksheet
    {
        #region Members

        private int _Idx;
        private int _orderIdx;
        private string _worksheetIdx;
        private string _comments;
        private int _handler;
        
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

        private DateTime _regDate;
        private int _confirmUser;
        private DateTime _confirmDate;
        
        private DataRow _row;

        #endregion

        #region Property 
        
        //ID
        public int Idx
        {
            get { return _Idx; }
            set { _Idx = value; }
        }
        public int OrderIdx
        {
            get { return _orderIdx; }
            set { _orderIdx = value; }
        }

        // 
        public string WorksheetIdx { get { return _worksheetIdx; } set { _worksheetIdx = value; } }

        public string Comments { get { return _comments; } set { _comments = value; } }
        // 
        public int Handler
        {
            get { return _handler; }
            set { _handler = value; }
        }

        // 
        public string Attached1 { get { return _attached1; } set { _attached1 = value; } }
        public string Attached2 { get { return _attached2; } set { _attached2 = value; } }
        public string Attached3 { get { return _attached3; } set { _attached3 = value; } }
        public string Attached4 { get { return _attached4; } set { _attached4 = value; } }
        public string Attached5 { get { return _attached5; } set { _attached5 = value; } }
        public string Attached6 { get { return _attached6; } set { _attached6 = value; } }
        public string Attached7 { get { return _attached7; } set { _attached7 = value; } }
        public string Attached8 { get { return _attached8; } set { _attached8 = value; } }
        public string Attached9 { get { return _attached9; } set { _attached9 = value; } }

        public string AttachedUrl1 { get { return _attachedUrl1; } set { _attachedUrl1 = value; } }
        public string AttachedUrl2 { get { return _attachedUrl2; } set { _attachedUrl2 = value; } }
        public string AttachedUrl3 { get { return _attachedUrl3; } set { _attachedUrl3 = value; } }
        public string AttachedUrl4 { get { return _attachedUrl4; } set { _attachedUrl4 = value; } }
        public string AttachedUrl5 { get { return _attachedUrl5; } set { _attachedUrl5 = value; } }
        public string AttachedUrl6 { get { return _attachedUrl6; } set { _attachedUrl6 = value; } }
        public string AttachedUrl7 { get { return _attachedUrl7; } set { _attachedUrl7 = value; } }
        public string AttachedUrl8 { get { return _attachedUrl8; } set { _attachedUrl8 = value; } }
        public string AttachedUrl9 { get { return _attachedUrl9; } set { _attachedUrl9 = value; } }

        // 
        public DateTime RegDate
        {
            get { return _regDate; }
            set { _regDate = value; }
        }
        // 
        public int ConfirmUser
        {
            get { return _confirmUser; }
            set { _confirmUser = value; }
        }
        // 
        public DateTime ConfirmDate
        {
            get { return _confirmDate; }
            set { _confirmDate = value; }
        }
        
        
        #endregion

        #region Constructor 

        public Worksheet(int Idx)
        {
            _row = Data.WorksheetData.Get(Idx);

            if (_row != null)
            {
                _Idx = Convert.ToInt32(_row["Idx"]);
                if (_row["OrderIdx"] != DBNull.Value) _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
                if (_row["WorksheetIdx"] != DBNull.Value) _worksheetIdx = Convert.ToString(_row["WorksheetIdx"]);
                if (_row["Comments"] != DBNull.Value) _comments = Convert.ToString(_row["Comments"]);
                
                if (_row["Handler"] != DBNull.Value) _handler = Convert.ToInt32(_row["Handler"]);

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
                
                if (_row["RegDate"] != DBNull.Value) _regDate = Convert.ToDateTime(_row["RegDate"]);
                if (_row["ConfirmUser"] != DBNull.Value) _confirmUser = Convert.ToInt32(_row["ConfirmUser"]);
                if (_row["ConfirmDate"] != DBNull.Value) _confirmDate = Convert.ToDateTime(_row["ConfirmDate"]);
            }
            else
            {
                Initializer();
            }
        }
        
        private void Initializer()
        {
            _Idx = 0;           
            _orderIdx = 0;
            _worksheetIdx = "";
            _comments = "";
            _handler = 0;

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

            _regDate = DateTime.Now;
            _confirmUser = 0;
            _confirmDate = DateTime.Now;
        }
        #endregion

        #region Methods

        // Insert
        public static DataRow Insert(int OrderIdx, string WorksheetIdx, string Comments,
            string Attached1, string Attached2, string Attached3, string Attached4, string Attached5, string Attached6, string Attached7, string Attached8, string Attached9,
            string AttachedUrl1, string AttachedUrl2, string AttachedUrl3, string AttachedUrl4, string AttachedUrl5, string AttachedUrl6, string AttachedUrl7,
            string AttachedUrl8, string AttachedUrl9, int Handler)
        {
            DataRow row = Data.WorksheetData.Insert(OrderIdx, WorksheetIdx, Comments,
            Attached1,  Attached2,  Attached3,  Attached4,  Attached5,  Attached6,  Attached7,  Attached8,  Attached9,
             AttachedUrl1,  AttachedUrl2,  AttachedUrl3,  AttachedUrl4,  AttachedUrl5,  AttachedUrl6,  AttachedUrl7,
             AttachedUrl8,  AttachedUrl9, Handler);
            return row;
        }
        

        public static DataSet Getlist(int OrderIdx, string WorksheetIdx, int Handler, int ConfirmUser, int WorkStatus)
        {
            DataSet ds = new DataSet();
            ds = Data.WorksheetData.Getlist(OrderIdx, WorksheetIdx, Handler, ConfirmUser, WorkStatus);
            return ds;
        }

        public static DataSet Getlist(int DeptIdx, int CustIdx, int Handler, int WorkStatus, string Fileno, string Styleno, string WorksheetIdx,
                                        int OptionCheck1, int OptionCheck2, int OptionCheck3, int OptionCheck4, int OptionCheck5)
        {
            DataSet ds = new DataSet();
            ds = Data.WorksheetData.Getlist(DeptIdx, CustIdx, Handler, WorkStatus, Fileno, Styleno, WorksheetIdx, 0,
                                        OptionCheck1, OptionCheck2, OptionCheck3, OptionCheck4, OptionCheck5);
            return ds;
        }

        public static DataSet Getlist(int DeptIdx, int CustIdx, int Handler, int WorkStatus, string Fileno, string Styleno, string WorksheetIdx, int UserIdx,
                int OptionCheck1, int OptionCheck2, int OptionCheck3, int OptionCheck4, int OptionCheck5)
        {
            DataSet ds = new DataSet();
            ds = Data.WorksheetData.Getlist(DeptIdx, CustIdx, Handler, WorkStatus, Fileno, Styleno, WorksheetIdx, UserIdx,
                                        OptionCheck1, OptionCheck2, OptionCheck3, OptionCheck4, OptionCheck5);
            return ds;
        }

        public static DataSet GetlistReport(int DeptIdx, int CustIdx, int Handler, int WorkStatus, string Fileno, string Styleno, string WorksheetIdx,
                                        int OptionCheck1, int OptionCheck2, int OptionCheck3, int OptionCheck4, int OptionCheck5)
        {
            DataSet ds = new DataSet();
            ds = Data.WorksheetData.GetlistReport(DeptIdx, CustIdx, Handler, WorkStatus, Fileno, Styleno, WorksheetIdx, 0,
                                        OptionCheck1, OptionCheck2, OptionCheck3, OptionCheck4, OptionCheck5);
            return ds;
        }

        public static DataSet GetlistReport(int DeptIdx, int CustIdx, int Handler, int WorkStatus, string Fileno, string Styleno, string WorksheetIdx, int UserIdx,
                int OptionCheck1, int OptionCheck2, int OptionCheck3, int OptionCheck4, int OptionCheck5)
        {
            DataSet ds = new DataSet();
            ds = Data.WorksheetData.GetlistReport(DeptIdx, CustIdx, Handler, WorkStatus, Fileno, Styleno, WorksheetIdx, UserIdx,
                                        OptionCheck1, OptionCheck2, OptionCheck3, OptionCheck4, OptionCheck5);
            return ds;
        }

        public bool Update()
        {
            bool blRtn;
            blRtn = Data.WorksheetData.Update(_Idx, _confirmUser); 
            return blRtn;
        }
        

        // Multiple Delete
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.WorksheetData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
