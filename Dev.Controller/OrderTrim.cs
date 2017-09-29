using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class OrderTrim
    {
        #region Members

        private int _Idx;
        private int _orderIdx;
        private int _colorIdx;
        private int _status01;
        private int _status02;
        private int _status03;
        private int _status04;
        private int _status05;
        private int _status06;
        private int _status07;
        private int _status08;
        private int _status09;
        private int _status10;
        private int _status11;
        private int _status12;
        private int _status13;
        private int _status14;
        private int _status15;
        private int _status16;
        private int _status17;
        private int _status18;
        private int _status19;
        private int _status20;
        
        private DateTime _modified01;
        private DateTime _modified02;
        private DateTime _modified03;
        private DateTime _modified04;
        private DateTime _modified05;
        private DateTime _modified06;
        private DateTime _modified07;
        private DateTime _modified08;
        private DateTime _modified09;
        private DateTime _modified10;
        private DateTime _modified11;
        private DateTime _modified12;
        private DateTime _modified13;
        private DateTime _modified14;
        private DateTime _modified15;
        private DateTime _modified16;
        private DateTime _modified17;
        private DateTime _modified18;
        private DateTime _modified19;
        private DateTime _modified20;

        private string _memo01;
        private string _memo02;
        private string _memo03;
        private string _memo04;
        private string _memo05;
        private string _memo06;
        private string _memo07;
        private string _memo08;
        private string _memo09;
        private string _memo10;
        private string _memo11;
        private string _memo12;
        private string _memo13;
        private string _memo14;
        private string _memo15;
        private string _memo16;
        private string _memo17;
        private string _memo18;
        private string _memo19;
        private string _memo20;

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
        public int ColorIdx
        {
            get { return _colorIdx; }
            set { _colorIdx = value; }
        }
        // 
        public int Status01 { get { return _status01; } set { _status01 = value; } }
        public int Status02 { get { return _status02; } set { _status02 = value; } }
        public int Status03 { get { return _status03; } set { _status03 = value; } }
        public int Status04 { get { return _status04; } set { _status04 = value; } }
        public int Status05 { get { return _status05; } set { _status05 = value; } }
        public int Status06 { get { return _status06; } set { _status06 = value; } }
        public int Status07 { get { return _status07; } set { _status07 = value; } }
        public int Status08 { get { return _status08; } set { _status08 = value; } }
        public int Status09 { get { return _status09; } set { _status09 = value; } }
        public int Status10 { get { return _status10; } set { _status10 = value; } }
        public int Status11 { get { return _status11; } set { _status11 = value; } }
        public int Status12 { get { return _status12; } set { _status12 = value; } }
        public int Status13 { get { return _status13; } set { _status13 = value; } }
        public int Status14 { get { return _status14; } set { _status14 = value; } }
        public int Status15 { get { return _status15; } set { _status15 = value; } }
        public int Status16 { get { return _status16; } set { _status16 = value; } }
        public int Status17 { get { return _status17; } set { _status17 = value; } }
        public int Status18 { get { return _status18; } set { _status18 = value; } }
        public int Status19 { get { return _status19; } set { _status19 = value; } }
        public int Status20 { get { return _status20; } set { _status20 = value; } }

        // 
        public DateTime Modified01 { get { return _modified01; } set { _modified01 = value; } }
        public DateTime Modified02 { get { return _modified02; } set { _modified02 = value; } }
        public DateTime Modified03 { get { return _modified03; } set { _modified03 = value; } }
        public DateTime Modified04 { get { return _modified04; } set { _modified04 = value; } }
        public DateTime Modified05 { get { return _modified05; } set { _modified05 = value; } }
        public DateTime Modified06 { get { return _modified06; } set { _modified06 = value; } }
        public DateTime Modified07 { get { return _modified07; } set { _modified07 = value; } }
        public DateTime Modified08 { get { return _modified08; } set { _modified08 = value; } }
        public DateTime Modified09 { get { return _modified09; } set { _modified09 = value; } }
        public DateTime Modified10 { get { return _modified10; } set { _modified10 = value; } }
        public DateTime Modified11 { get { return _modified11; } set { _modified11 = value; } }
        public DateTime Modified12 { get { return _modified12; } set { _modified12 = value; } }
        public DateTime Modified13 { get { return _modified13; } set { _modified13 = value; } }
        public DateTime Modified14 { get { return _modified14; } set { _modified14 = value; } }
        public DateTime Modified15 { get { return _modified15; } set { _modified15 = value; } }
        public DateTime Modified16 { get { return _modified16; } set { _modified16 = value; } }
        public DateTime Modified17 { get { return _modified17; } set { _modified17 = value; } }
        public DateTime Modified18 { get { return _modified18; } set { _modified18 = value; } }
        public DateTime Modified19 { get { return _modified19; } set { _modified19 = value; } }
        public DateTime Modified20 { get { return _modified20; } set { _modified20 = value; } }

        public string Memo01 { get { return _memo01; } set { _memo01 = value; } }
        public string Memo02 { get { return _memo02; } set { _memo02 = value; } }
        public string Memo03 { get { return _memo03; } set { _memo03 = value; } }
        public string Memo04 { get { return _memo04; } set { _memo04 = value; } }
        public string Memo05 { get { return _memo05; } set { _memo05 = value; } }
        public string Memo06 { get { return _memo06; } set { _memo06 = value; } }
        public string Memo07 { get { return _memo07; } set { _memo07 = value; } }
        public string Memo08 { get { return _memo08; } set { _memo08 = value; } }
        public string Memo09 { get { return _memo09; } set { _memo09 = value; } }
        public string Memo10 { get { return _memo10; } set { _memo10 = value; } }
        public string Memo11 { get { return _memo11; } set { _memo11 = value; } }
        public string Memo12 { get { return _memo12; } set { _memo12 = value; } }
        public string Memo13 { get { return _memo13; } set { _memo13 = value; } }
        public string Memo14 { get { return _memo14; } set { _memo14 = value; } }
        public string Memo15 { get { return _memo15; } set { _memo15 = value; } }
        public string Memo16 { get { return _memo16; } set { _memo16 = value; } }
        public string Memo17 { get { return _memo17; } set { _memo17 = value; } }
        public string Memo18 { get { return _memo18; } set { _memo18 = value; } }
        public string Memo19 { get { return _memo19; } set { _memo19 = value; } }
        public string Memo20 { get { return _memo20; } set { _memo20 = value; } }

        #endregion

        #region Constructor 

        public OrderTrim(int Idx)
        {
            _row = Data.WorksheetData.Get(Idx);

            if (_row != null)
            {
                _Idx = Convert.ToInt32(_row["Idx"]);
                if (_row["OrderIdx"] != DBNull.Value) _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
                if (_row["ColorIdx"] != DBNull.Value) _colorIdx = Convert.ToInt32(_row["ColorIdx"]);

                if (_row["Status01"] != DBNull.Value) _status01 = Convert.ToInt32(_row["Status01"]);
                if (_row["Status02"] != DBNull.Value) _status02 = Convert.ToInt32(_row["Status02"]);
                if (_row["Status03"] != DBNull.Value) _status03 = Convert.ToInt32(_row["Status03"]);
                if (_row["Status04"] != DBNull.Value) _status04 = Convert.ToInt32(_row["Status04"]);
                if (_row["Status05"] != DBNull.Value) _status05 = Convert.ToInt32(_row["Status05"]);
                if (_row["Status06"] != DBNull.Value) _status06 = Convert.ToInt32(_row["Status06"]);
                if (_row["Status07"] != DBNull.Value) _status07 = Convert.ToInt32(_row["Status07"]);
                if (_row["Status08"] != DBNull.Value) _status08 = Convert.ToInt32(_row["Status08"]);
                if (_row["Status09"] != DBNull.Value) _status09 = Convert.ToInt32(_row["Status09"]);
                if (_row["Status10"] != DBNull.Value) _status10 = Convert.ToInt32(_row["Status10"]);
                if (_row["Status11"] != DBNull.Value) _status11 = Convert.ToInt32(_row["Status11"]);
                if (_row["Status12"] != DBNull.Value) _status12 = Convert.ToInt32(_row["Status12"]);
                if (_row["Status13"] != DBNull.Value) _status13 = Convert.ToInt32(_row["Status13"]);
                if (_row["Status14"] != DBNull.Value) _status14 = Convert.ToInt32(_row["Status14"]);
                if (_row["Status15"] != DBNull.Value) _status15 = Convert.ToInt32(_row["Status15"]);
                if (_row["Status16"] != DBNull.Value) _status16 = Convert.ToInt32(_row["Status16"]);
                if (_row["Status17"] != DBNull.Value) _status17 = Convert.ToInt32(_row["Status17"]);
                if (_row["Status18"] != DBNull.Value) _status18 = Convert.ToInt32(_row["Status18"]);
                if (_row["Status19"] != DBNull.Value) _status19 = Convert.ToInt32(_row["Status19"]);
                if (_row["Status20"] != DBNull.Value) _status20 = Convert.ToInt32(_row["Status20"]);

                if (_row["Modified01"] != DBNull.Value) _modified01 = Convert.ToDateTime(_row["Modified01"]);
                if (_row["Modified02"] != DBNull.Value) _modified02 = Convert.ToDateTime(_row["Modified02"]);
                if (_row["Modified03"] != DBNull.Value) _modified03 = Convert.ToDateTime(_row["Modified03"]);
                if (_row["Modified04"] != DBNull.Value) _modified04 = Convert.ToDateTime(_row["Modified04"]);
                if (_row["Modified05"] != DBNull.Value) _modified05 = Convert.ToDateTime(_row["Modified05"]);
                if (_row["Modified06"] != DBNull.Value) _modified06 = Convert.ToDateTime(_row["Modified06"]);
                if (_row["Modified07"] != DBNull.Value) _modified07 = Convert.ToDateTime(_row["Modified07"]);
                if (_row["Modified08"] != DBNull.Value) _modified08 = Convert.ToDateTime(_row["Modified08"]);
                if (_row["Modified09"] != DBNull.Value) _modified09 = Convert.ToDateTime(_row["Modified09"]);
                if (_row["Modified10"] != DBNull.Value) _modified10 = Convert.ToDateTime(_row["Modified10"]);
                if (_row["Modified11"] != DBNull.Value) _modified11 = Convert.ToDateTime(_row["Modified11"]);
                if (_row["Modified12"] != DBNull.Value) _modified12 = Convert.ToDateTime(_row["Modified12"]);
                if (_row["Modified13"] != DBNull.Value) _modified13 = Convert.ToDateTime(_row["Modified13"]);
                if (_row["Modified14"] != DBNull.Value) _modified14 = Convert.ToDateTime(_row["Modified14"]);
                if (_row["Modified15"] != DBNull.Value) _modified15 = Convert.ToDateTime(_row["Modified15"]);
                if (_row["Modified16"] != DBNull.Value) _modified16 = Convert.ToDateTime(_row["Modified16"]);
                if (_row["Modified17"] != DBNull.Value) _modified17 = Convert.ToDateTime(_row["Modified17"]);
                if (_row["Modified18"] != DBNull.Value) _modified18 = Convert.ToDateTime(_row["Modified18"]);
                if (_row["Modified19"] != DBNull.Value) _modified19 = Convert.ToDateTime(_row["Modified19"]);
                if (_row["Modified20"] != DBNull.Value) _modified20 = Convert.ToDateTime(_row["Modified20"]);

                if (_row["Memo01"] != DBNull.Value) _memo01 = Convert.ToString(_row["Memo01"]);
                if (_row["Memo02"] != DBNull.Value) _memo02 = Convert.ToString(_row["Memo02"]);
                if (_row["Memo03"] != DBNull.Value) _memo03 = Convert.ToString(_row["Memo03"]);
                if (_row["Memo04"] != DBNull.Value) _memo04 = Convert.ToString(_row["Memo04"]);
                if (_row["Memo05"] != DBNull.Value) _memo05 = Convert.ToString(_row["Memo05"]);
                if (_row["Memo06"] != DBNull.Value) _memo06 = Convert.ToString(_row["Memo06"]);
                if (_row["Memo07"] != DBNull.Value) _memo07 = Convert.ToString(_row["Memo07"]);
                if (_row["Memo08"] != DBNull.Value) _memo08 = Convert.ToString(_row["Memo08"]);
                if (_row["Memo09"] != DBNull.Value) _memo09 = Convert.ToString(_row["Memo09"]);
                if (_row["Memo10"] != DBNull.Value) _memo10 = Convert.ToString(_row["Memo10"]);
                if (_row["Memo11"] != DBNull.Value) _memo11 = Convert.ToString(_row["Memo11"]);
                if (_row["Memo12"] != DBNull.Value) _memo12 = Convert.ToString(_row["Memo12"]);
                if (_row["Memo13"] != DBNull.Value) _memo13 = Convert.ToString(_row["Memo13"]);
                if (_row["Memo14"] != DBNull.Value) _memo14 = Convert.ToString(_row["Memo14"]);
                if (_row["Memo15"] != DBNull.Value) _memo15 = Convert.ToString(_row["Memo15"]);
                if (_row["Memo16"] != DBNull.Value) _memo16 = Convert.ToString(_row["Memo16"]);
                if (_row["Memo17"] != DBNull.Value) _memo17 = Convert.ToString(_row["Memo17"]);
                if (_row["Memo18"] != DBNull.Value) _memo18 = Convert.ToString(_row["Memo18"]);
                if (_row["Memo19"] != DBNull.Value) _memo19 = Convert.ToString(_row["Memo19"]);
                if (_row["Memo20"] != DBNull.Value) _memo20 = Convert.ToString(_row["Memo20"]);

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
            _colorIdx = 0;

            _status01 = 0;
            _status02 = 0;
            _status03 = 0;
            _status04 = 0;
            _status05 = 0;
            _status06 = 0;
            _status07 = 0;
            _status08 = 0;
            _status09 = 0;
            _status10 = 0;
            _status11 = 0;
            _status12 = 0;
            _status13 = 0;
            _status14 = 0;
            _status15 = 0;
            _status16 = 0;
            _status17 = 0;
            _status18 = 0;
            _status19 = 0;
            _status20 = 0;

            _modified01 = DateTime.Now;
            _modified02 = DateTime.Now;
            _modified03 = DateTime.Now;
            _modified04 = DateTime.Now;
            _modified05 = DateTime.Now;
            _modified06 = DateTime.Now;
            _modified07 = DateTime.Now;
            _modified08 = DateTime.Now;
            _modified09 = DateTime.Now;
            _modified10 = DateTime.Now;
            _modified11 = DateTime.Now;
            _modified12 = DateTime.Now;
            _modified13 = DateTime.Now;
            _modified14 = DateTime.Now;
            _modified15 = DateTime.Now;
            _modified16 = DateTime.Now;
            _modified17 = DateTime.Now;
            _modified18 = DateTime.Now;
            _modified19 = DateTime.Now;
            _modified20 = DateTime.Now;

            _memo01 = "";
            _memo02 = "";
            _memo03 = "";
            _memo04 = "";
            _memo05 = "";
            _memo06 = "";
            _memo07 = "";
            _memo08 = "";
            _memo09 = "";
            _memo10 = "";
            _memo11 = "";
            _memo12 = "";
            _memo13 = "";
            _memo14 = "";
            _memo15 = "";
            _memo16 = "";
            _memo17 = "";
            _memo18 = "";
            _memo19 = "";
            _memo20 = "";
        }
        #endregion

        #region Methods

        // Insert
        public static DataRow Insert(int OrderIdx, int ColorIdx)
        {
            DataRow row = Data.OrderTrimData.Insert(OrderIdx, ColorIdx);
            return row;
        }
        
        public static DataSet Getlist(int DeptIdx, int CustIdx, int Handler, int Status, string Fileno, string Styleno, int WorkStatus, int OrderIdx)
        {
            DataSet ds = new DataSet();
            ds = Data.OrderTrimData.Getlist(DeptIdx, CustIdx, Handler, Status, Fileno, Styleno, WorkStatus, OrderIdx);
            return ds;
        }
        
        public static bool Update(int Idx, int Trimno, int Status)
        {
            bool blRtn;
            blRtn = Data.OrderTrimData.Update(Idx, Trimno, Status); 
            return blRtn;
        }
        

        // Multiple Delete
        //public static bool Delete(string condition)
        //{
        //    bool blRtn;
        //    blRtn = Data.OrderTrimData.Delete(condition);
        //    return blRtn;
        //}

        #endregion
    }
}
