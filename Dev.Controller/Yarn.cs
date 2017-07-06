using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class Yarn
    {
        #region Members

        private int _Idx;
        private string _yarnName; 
        private string _composition;
        private string _burnCount;
        private string _yarnType;
        private string _contents1;
        private string _contents2;
        private string _contents3;
        private string _contents4;
        private double _percent1;
        private double _percent2;
        private double _percent3;
        private double _percent4;
        private string _remark;
        private int _isUse;
        
        private DataRow _row;

        #endregion

        #region Property 

        //ID
        public int Idx
        {
            get { return _Idx; }
            set { _Idx = value; }
        }
        // 
        public string YarnName
        {
            get { return _yarnName; }
            set { _yarnName = value; }
        }
        // 
        public string Composition
        {
            get { return _composition; }
            set { _composition = value; }
        }
        // 
        public string BurnCount
        {
            get { return _burnCount; }
            set { _burnCount = value; }
        }
        // 
        public string YarnType
        {
            get { return _yarnType; }
            set { _yarnType = value; }
        }
        // 
        public string Contents1
        {
            get { return _contents1; }
            set { _contents1 = value; }
        }
        // 
        public string Contents2
        {
            get { return _contents2; }
            set { _contents2 = value; }
        }
        // 
        public string Contents3
        {
            get { return _contents3; }
            set { _contents3 = value; }
        }
        // 
        public string Contents4
        {
            get { return _contents4; }
            set { _contents4 = value; }
        }
        // 
        public double Percent1
        {
            get { return _percent1; }
            set { _percent1 = value; }
        }
        // 
        public double Percent2
        {
            get { return _percent2; }
            set { _percent2 = value; }
        }
        // 
        public double Percent3
        {
            get { return _percent3; }
            set { _percent3 = value; }
        }
        // 
        public double Percent4
        {
            get { return _percent4; }
            set { _percent4 = value; }
        }
        // 
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        //사용여부
        public int IsUse
        {
            get { return _isUse; }
            set { _isUse = value; }
        }
        
        #endregion

        #region Constructor 
        
        public Yarn(int Idx)
        {
            _row = Data.YarnData.Get(Idx);

            if (_row != null)
            {
                _Idx = Convert.ToInt32(_row["Idx"]);
                if (_row["Composition"] != DBNull.Value) _composition = _row["Composition"].ToString();
                if (_row["BurnCount"] != DBNull.Value) _burnCount = _row["BurnCount"].ToString();
                if (_row["YarnType"] != DBNull.Value) _yarnType = _row["YarnType"].ToString();
                if (_row["Contents1"] != DBNull.Value) _contents1 = _row["Contents1"].ToString();
                if (_row["Contents2"] != DBNull.Value) _contents2 = _row["Contents2"].ToString();
                if (_row["Contents3"] != DBNull.Value) _contents3 = _row["Contents3"].ToString();
                if (_row["Contents4"] != DBNull.Value) _contents4 = _row["Contents4"].ToString();

                if (_row["Percent1"] != DBNull.Value) _percent1 = Convert.ToDouble(_row["Percent1"]);
                if (_row["Percent2"] != DBNull.Value) _percent2 = Convert.ToDouble(_row["Percent2"]);
                if (_row["Percent3"] != DBNull.Value) _percent3 = Convert.ToDouble(_row["Percent3"]);
                if (_row["Percent4"] != DBNull.Value) _percent4 = Convert.ToDouble(_row["Percent4"]);

                if (_row["Remark"] != DBNull.Value) _remark = _row["Remark"].ToString();
                if (_row["IsUse"] != DBNull.Value) _isUse = Convert.ToInt32(_row["IsUse"]);
            }
            else
            {
                Initializer();
            }
        }

        public Yarn(int Idx, string YarnName, string Composition, string BurnCount, string YarnType,
            string Contents1, double Percent1, string Contents2, string Contents3, string Contents4,
             double Percent2, double Percent3, double Percent4,
            string Remark)
        {
            _Idx = Idx;
            _yarnName = YarnName; 
            _composition = Composition;
            _burnCount = BurnCount;
            _yarnType = YarnType;
            _contents1 = Contents1;
            _percent1 = Percent1;
            _contents2 = Contents2;
            _contents3 = Contents3;
            _contents4 = Contents4;
            _percent2 = Percent2;
            _percent3 = Percent3;
            _percent4 = Percent4;
            _remark = Remark;
        }
        private void Initializer()
        {
            _Idx = 0;
            _composition = "";
            _burnCount = "";
            _yarnType = "";
            _contents1 = "";
            _percent1 = 0f;
            _contents2 = "";
            _contents3 = "";
            _contents4 = "";
            _percent2 = 0f;
            _percent3 = 0f;
            _percent4 = 0f;
            _remark = "";
            _isUse = 0;
        }
        #endregion

        #region Methods

        // Insert
        public static DataRow Insert()
        {
            DataRow row = Data.YarnData.Insert();
            return row;
        }

        // Getlist with search parameter
        public static DataSet Getlist()
        {
            DataSet ds = new DataSet();
            ds = Data.YarnData.Getlist();
            return ds;
        }

        public static DataSet Getlist(Dictionary<CommonValues.KeyName, string> SearchString)
        {
            DataSet ds = new DataSet();
            ds = Data.YarnData.Getlist(SearchString);
            return ds;
        }
        

        public static bool Update(int Idx, string Composition, string BurnCount, string YarnType,
            string Contents1, string Contents2, string Contents3, string Contents4,
            double Percent1, double Percent2, double Percent3, double Percent4,
            string Remark, int IsUse)
        {
            bool blRtn;
            blRtn = Data.YarnData.Update(Idx, Composition, BurnCount, YarnType,
                                        Contents1, Contents2, Contents3, Contents4,
                                        Percent1, Percent2, Percent3, Percent4,
                                        Remark, IsUse);
            return blRtn;
        }
        
        // Multiple Delete
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.YarnData.Delete(condition);
            return blRtn;
        }

        #endregion


        
    }
}
