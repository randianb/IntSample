using System;
using System.Data;

namespace Dev.Codes.Controller
{
    public class SewThread
    {
        #region Members

        private int _sewThreadIdx;
        private int _sewThreadCustIdx;
        private string _sewThreadName;
        private int _colorIdx;
        private int _isUse;
        
        private DataRow _row;

        #endregion

        #region Property 

        //ID
        public int SewThreadIdx
        {
            get { return _sewThreadIdx; }
            set { _sewThreadIdx = value; }
        }
        //제조사
        public int SewThreadCustIdx
        {
            get { return _sewThreadCustIdx; }
            set { _sewThreadCustIdx = value; }
        }
        //재봉사명 
        public string SewThreadName
        {
            get { return _sewThreadName; }
            set { _sewThreadName = value; }
        }
        //컬러번호
        public int ColorIdx
        {
            get { return _colorIdx; }
            set { _colorIdx = value; }
        }
        
        //사용여부
        public int IsUse
        {
            get { return _isUse; }
            set { _isUse = value; }
        }
        
        #endregion

        #region Constructor 

        public SewThread(int SewThreadIdx)
        {
            _row = Data.SewThreadData.Get(SewThreadIdx);

            if (_row != null)
            {
                _sewThreadIdx = Convert.ToInt32(_row["SewThreadIdx"]);
                if (_row["SewThreadCustIdx"] != DBNull.Value) _sewThreadCustIdx = Convert.ToInt32(_row["SewThreadCustIdx"]);
                if (_row["SewThreadName"] != DBNull.Value) _sewThreadName = Convert.ToString(_row["SewThreadName"]);
                if (_row["ColorIdx"] != DBNull.Value) _colorIdx = Convert.ToInt32(_row["ColorIdx"]);
                if (_row["IsUse"] != DBNull.Value) _isUse = Convert.ToInt32(_row["IsUse"]);
            }
            else
            {
                Initializer();
            }
        }

        private void Initializer()
        {
            _sewThreadIdx = 0;
            _sewThreadCustIdx = 0;
            _sewThreadName = "";
            _colorIdx = 0;
            _isUse = 0;
        }
        #endregion

        #region Methods

        // Insert
        public static DataRow Insert(int SewThreadCustIdx, string SewThreadName, int ColorIdx, int IsUse)
        {
            DataRow row = Data.SewThreadData.Insert(SewThreadCustIdx, SewThreadName, ColorIdx, IsUse);
            return row;
        }

        // Getlist with search parameter
        public static DataSet Getlist()
        {
            DataSet ds = new DataSet();
            ds = Data.SewThreadData.Getlist();
            return ds;
        }
        public static DataSet Getlist(int SewThreadCustIdx)
        {
            DataSet ds = new DataSet();
            ds = Data.SewThreadData.Getlist(SewThreadCustIdx);
            return ds;
        }
        public static DataTable GetUsablelist()
        {
            DataTable dt = new DataTable();
            dt = Data.SewThreadData.GetUsablelist().Tables[0];
            return dt;
           
        }

        public bool Update()
        {
            bool blRtn;
            blRtn = Data.SewThreadData.Update(_sewThreadIdx, _sewThreadCustIdx, _sewThreadName, _colorIdx, _isUse);
            return blRtn;
        }
        
        // Multiple Delete
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.SewThreadData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
