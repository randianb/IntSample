using System;
using System.Data;

namespace Dev.Codes.Controller
{
    public class Color
    {
        #region Members

        private int _colorIdx;
        private string _colorName;
        private int _classification;
        private int _isUse;
        
        private DataRow _row;

        #endregion

        #region Property 

        //ID
        public int ColorIdx
        {
            get { return _colorIdx; }
            set { _colorIdx = value; }
        }
        //컬러명 
        public string ColorName
        {
            get { return _colorName; }
            set { _colorName = value; }
        }
        //분류
        public int Classification
        {
            get { return _classification; }
            set { _classification = value; }
        }
        //사용여부
        public int IsUse
        {
            get { return _isUse; }
            set { _isUse = value; }
        }
        
        #endregion

        #region Constructor 

        public Color(int ColorIdx)
        {
            _row = Data.ColorData.Get(ColorIdx);

            if (_row != null)

            {
                _colorIdx = Convert.ToInt32(_row["ColorIdx"]);
                if (_row["ColorName"] != DBNull.Value) _colorName = Convert.ToString(_row["ColorName"]);
                if (_row["Classification"] != DBNull.Value) _classification = Convert.ToInt32(_row["Classification"]);
                if (_row["IsUse"] != DBNull.Value) _isUse = Convert.ToInt32(_row["IsUse"]);
            }
            else
            {
                Initializer();
            }
        }

        private void Initializer()
        {
            _colorIdx = 0;
            _colorName = "";
            _classification = 0;
            _isUse = 0;
        }
        #endregion

        #region Methods

        // Insert
        public static DataRow Insert(string ColorName, int Classification, int IsUse)
        {
            DataRow row = Data.ColorData.Insert(ColorName, Classification, IsUse);
            return row;
        }

        // Getlist with search parameter
        public static DataSet Getlist()
        {
            DataSet ds = new DataSet();
            ds = Data.ColorData.Getlist(); 
            return ds;
        }
        public static DataSet GetUselist()
        {
            DataSet ds = new DataSet();
            ds = Data.ColorData.GetUselist();
            return ds;
        }
        public static DataSet Getlist(string ColorName)
        {
            DataSet ds = new DataSet();
            ds = Data.ColorData.Getlist(ColorName);
            return ds;
        }
         
        public bool Update()
        {
            bool blRtn;
            blRtn = Data.ColorData.Update(_colorIdx, _colorName, _classification, _isUse);
            return blRtn;
        }
        
        // Multiple Delete
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.ColorData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
