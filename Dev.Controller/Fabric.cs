using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class Fabric
    {
        #region Members

        private int _Idx;
        private string _longName; 
        private string _shortName;
        private int _yarn1;
        private int _yarn2; 
        private int _yarn3;
        private int _yarn4;
        private int _yarn5;

        private string _yarnnm1;
        private string _yarnnm2;
        private string _yarnnm3;
        private string _yarnnm4;
        private string _yarnnm5;

        private double _percent1;
        private double _percent2;
        private double _percent3;
        private double _percent4;
        private double _percent5;
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
        public string LongName
        {
            get { return _longName; }
            set { _longName = value; }
        }
        // 
        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }
        
        // 
        public int Yarn1
        {
            get { return _yarn1; }
            set { _yarn1 = value; }
        }
        // 
        public int Yarn2
        {
            get { return _yarn2; }
            set { _yarn2 = value; }
        }
        // 
        public int Yarn3
        {
            get { return _yarn3; }
            set { _yarn3 = value; }
        }
        // 
        public int Yarn4
        {
            get { return _yarn4; }
            set { _yarn4 = value; }
        }
        // 
        public int Yarn5
        {
            get { return _yarn5; }
            set { _yarn5 = value; }
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
        public double Percent5
        {
            get { return _percent5; }
            set { _percent5 = value; }
        }
        
        //사용여부
        public int IsUse
        {
            get { return _isUse; }
            set { _isUse = value; }
        }
        
        #endregion

        #region Constructor 
        
        public Fabric(int Idx)
        {
            _row = Data.FabricData.Get(Idx);

            if (_row != null)
            {
                _Idx = Convert.ToInt32(_row["Idx"]);
                if (_row["LongName"] != DBNull.Value) _longName = _row["LongName"].ToString();
                if (_row["ShortName"] != DBNull.Value) _shortName = _row["ShortName"].ToString();
                if (_row["Yarn1"] != DBNull.Value) _yarn1 = Convert.ToInt32(_row["Yarn1"]);
                if (_row["Yarn2"] != DBNull.Value) _yarn2 = Convert.ToInt32(_row["Yarn2"]);
                if (_row["Yarn3"] != DBNull.Value) _yarn3 = Convert.ToInt32(_row["Yarn3"]);
                if (_row["Yarn4"] != DBNull.Value) _yarn4 = Convert.ToInt32(_row["Yarn4"]);
                if (_row["Yarn5"] != DBNull.Value) _yarn5 = Convert.ToInt32(_row["Yarn5"]);

                if (_row["Percent1"] != DBNull.Value) _percent1 = Convert.ToDouble(_row["Percent1"]);
                if (_row["Percent2"] != DBNull.Value) _percent2 = Convert.ToDouble(_row["Percent2"]);
                if (_row["Percent3"] != DBNull.Value) _percent3 = Convert.ToDouble(_row["Percent3"]);
                if (_row["Percent4"] != DBNull.Value) _percent4 = Convert.ToDouble(_row["Percent4"]);
                if (_row["Percent5"] != DBNull.Value) _percent5 = Convert.ToDouble(_row["Percent5"]);
                
                if (_row["IsUse"] != DBNull.Value) _isUse = Convert.ToInt32(_row["IsUse"]);
            }
            else
            {
                Initializer();
            }
        }
        
        public Fabric(int Idx, string longName, string shortName)
        {
            _Idx = Idx;
            _longName = longName;
            _shortName = shortName; 
        }

        public Fabric(int Idx, string longName, string shortName, 
            string yarnnm1, double per1, string yarnnm2, double per2, string yarnnm3, double per3, string yarnnm4, double per4, string yarnnm5, double per5)
        {
            _Idx = Idx;
            _longName = longName;
            _shortName = shortName;

            _yarnnm1 = yarnnm1;
            _percent1 = per1;
            _yarnnm2 = yarnnm2;
            _percent2 = per2;
            _yarnnm3 = yarnnm3;
            _percent3 = per3;
            _yarnnm4 = yarnnm4;
            _percent4 = per4;
            _yarnnm5 = yarnnm5;
            _percent5 = per5;

        }

        private void Initializer()
        {
            _Idx = 0;
            _longName = "";
            _shortName = "";
            _yarn1 = 0;
            _yarn2 = 0;
            _yarn3 = 0;
            _yarn4 = 0;
            _yarn5 = 0;
            _percent1 = 0f;
            _percent2 = 0f;
            _percent3 = 0f;
            _percent4 = 0f;
            _percent5 = 0f;
            _isUse = 0;
        }
        #endregion

        #region Methods

        // Insert
        public static DataRow Insert()
        {
            DataRow row = Data.FabricData.Insert();
            return row;
        }

        

        public static DataSet Getlist(Dictionary<CommonValues.KeyName, string> SearchString)
        {
            DataSet ds = new DataSet();
            ds = Data.FabricData.Getlist(SearchString[CommonValues.KeyName.Remark], SearchString[CommonValues.KeyName.IsUse]);
            return ds;
        }
        
        public bool Update()
        {
            bool blRtn;
            blRtn = Data.FabricData.Update(_Idx, _longName, _shortName, _yarn1, _yarn2, _yarn3, _yarn4, _yarn5,
                                            _percent1, _percent2, _percent3, _percent4, _percent5, _isUse);
            return blRtn;
        }
        
        // Multiple Delete
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.FabricData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
