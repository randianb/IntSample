using System;
using System.Data;

namespace Dev.Codes.Controller
{
    public class Sizes
    {
        #region Members

        private int _sizeIdx;
        private string _sizeName;
        private int _isUse;
        
        private DataRow _row;

        #endregion

        #region Property 

        //ID
        public int SizeIdx
        {
            get { return _sizeIdx; }
            set { _sizeIdx = value; }
        }
        //컬러명 
        public string SizeName
        {
            get { return _sizeName; }
            set { _sizeName = value; }
        }
        
        //사용여부
        public int IsUse
        {
            get { return _isUse; }
            set { _isUse = value; }
        }
        
        #endregion

        #region Constructor 

        public Sizes(int sizeIdx, string sizeName)
        {
            SizeIdx = sizeIdx;
            SizeName = sizeName; 
        }

        public Sizes(int SizeIdx)
        {
            _row = Data.SizeData.Get(SizeIdx);

            if (_row != null)
            {
                _sizeIdx = Convert.ToInt32(_row["SizeIdx"]);
                if (_row["SizeName"] != DBNull.Value) _sizeName = Convert.ToString(_row["SizeName"]);
                if (_row["IsUse"] != DBNull.Value) _isUse = Convert.ToInt32(_row["IsUse"]);
            }
            else
            {
                Initializer();
            }
        }

        private void Initializer()
        {
            _sizeIdx = 0;
            _sizeName = "";
            _isUse = 0;
        }
        #endregion

        #region Methods

        // Insert
        public static DataRow Insert(string SizeName, int IsUse)
        {
            DataRow row = Data.SizeData.Insert(SizeName, IsUse);
            return row;
        }

        // Getlist with search parameter
        public static DataSet Getlist()
        {
            DataSet ds = new DataSet();
            ds = Data.SizeData.Getlist();
            return ds;
        }

        public static DataSet Getlist(string SizeName)
        {
            DataSet ds = new DataSet();
            ds = Data.SizeData.Getlist(SizeName);
            return ds;
        }
        
            public static DataSet GetUselist()
        {
            DataSet ds = new DataSet();
            ds = Data.SizeData.GetUselist();
            return ds;
        }

        public bool Update()
        {
            bool blRtn;
            blRtn = Data.SizeData.Update(_sizeIdx, _sizeName, _isUse);
            return blRtn;
        }
        
        // Multiple Delete
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.SizeData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
