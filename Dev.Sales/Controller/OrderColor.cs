using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Sales.Controller
{
    public class OrderColor
    {
        #region Members

        private int _idx;
        private int _orderIdx;
        private int _colorIdx;

        private int _sizeIdx1;
        private int _sizeIdx2;
        private int _sizeIdx3;
        private int _sizeIdx4;
        private int _sizeIdx5;
        private int _sizeIdx6;
        private int _sizeIdx7;
        private int _sizeIdx8;

        private int _pcs1;
        private int _pcs2;
        private int _pcs3;
        private int _pcs4;
        private int _pcs5;
        private int _pcs6;
        private int _pcs7;
        private int _pcs8;
        
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
        public int ColorIdx
        {
            get { return _colorIdx; }
            set { _colorIdx = value; }
        }

        //
        public int SizeIdx1
        {
            get { return _sizeIdx1; }
            set { _sizeIdx1 = value; }
        }
        public int SizeIdx2
        {
            get { return _sizeIdx2; }
            set { _sizeIdx2 = value; }
        }
        public int SizeIdx3
        {
            get { return _sizeIdx3; }
            set { _sizeIdx3 = value; }
        }
        public int SizeIdx4
        {
            get { return _sizeIdx4; }
            set { _sizeIdx4 = value; }
        }
        public int SizeIdx5
        {
            get { return _sizeIdx5; }
            set { _sizeIdx5 = value; }
        }
        public int SizeIdx6
        {
            get { return _sizeIdx6; }
            set { _sizeIdx6 = value; }
        }
        public int SizeIdx7
        {
            get { return _sizeIdx7; }
            set { _sizeIdx7 = value; }
        }
        public int SizeIdx8
        {
            get { return _sizeIdx8; }
            set { _sizeIdx8 = value; }
        }

        //
        public int Pcs1
        {
            get { return _pcs1; }
            set { _pcs1 = value; }
        }
        public int Pcs2
        {
            get { return _pcs2; }
            set { _pcs2 = value; }
        }

        public int Pcs3
        {
            get { return _pcs3; }
            set { _pcs3 = value; }
        }

        public int Pcs4
        {
            get { return _pcs4; }
            set { _pcs4 = value; }
        }
        public int Pcs5
        {
            get { return _pcs5; }
            set { _pcs5 = value; }
        }
        public int Pcs6
        {
            get { return _pcs6; }
            set { _pcs6 = value; }
        }
        public int Pcs7
        {
            get { return _pcs7; }
            set { _pcs7 = value; }
        }
        public int Pcs8
        {
            get { return _pcs8; }
            set { _pcs8 = value; }
        }
        #endregion

        #region Constructor 

        public OrderColor(int Idx)
        {
            _row = Data.OrderColorData.Get(Idx);
            
            if (_row != null)
            {
                _idx = Convert.ToInt32(_row["Idx"]);
                if (_row["OrderIdx"] != DBNull.Value) _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
                if (_row["ColorIdx"] != DBNull.Value) _colorIdx = Convert.ToInt32(_row["ColorIdx"]);
                
                if (_row["SizeIdx1"] != DBNull.Value) _sizeIdx1 = Convert.ToInt32(_row["SizeIdx1"]);
                if (_row["SizeIdx2"] != DBNull.Value) _sizeIdx2 = Convert.ToInt32(_row["SizeIdx2"]);
                if (_row["SizeIdx3"] != DBNull.Value) _sizeIdx3 = Convert.ToInt32(_row["SizeIdx3"]);
                if (_row["SizeIdx4"] != DBNull.Value) _sizeIdx4 = Convert.ToInt32(_row["SizeIdx4"]);
                if (_row["SizeIdx5"] != DBNull.Value) _sizeIdx5 = Convert.ToInt32(_row["SizeIdx5"]);
                if (_row["SizeIdx6"] != DBNull.Value) _sizeIdx6 = Convert.ToInt32(_row["SizeIdx6"]);
                if (_row["SizeIdx7"] != DBNull.Value) _sizeIdx7 = Convert.ToInt32(_row["SizeIdx7"]);
                if (_row["SizeIdx8"] != DBNull.Value) _sizeIdx8 = Convert.ToInt32(_row["SizeIdx8"]);

                if (_row["Pcs1"] != DBNull.Value) _pcs1 = Convert.ToInt32(_row["Pcs1"]);
                if (_row["Pcs2"] != DBNull.Value) _pcs2 = Convert.ToInt32(_row["Pcs2"]);
                if (_row["Pcs3"] != DBNull.Value) _pcs3 = Convert.ToInt32(_row["Pcs3"]);
                if (_row["Pcs4"] != DBNull.Value) _pcs4 = Convert.ToInt32(_row["Pcs4"]);
                if (_row["Pcs5"] != DBNull.Value) _pcs5 = Convert.ToInt32(_row["Pcs5"]);
                if (_row["Pcs6"] != DBNull.Value) _pcs6 = Convert.ToInt32(_row["Pcs6"]);
                if (_row["Pcs7"] != DBNull.Value) _pcs7 = Convert.ToInt32(_row["Pcs7"]);
                if (_row["Pcs8"] != DBNull.Value) _pcs8 = Convert.ToInt32(_row["Pcs8"]);
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
            _colorIdx = 0;
            
            _sizeIdx1 = 0;
            _sizeIdx2 = 0;
            _sizeIdx3 = 0;
            _sizeIdx4 = 0;
            _sizeIdx5 = 0;
            _sizeIdx6 = 0;
            _sizeIdx7 = 0;
            _sizeIdx8 = 0;

            _pcs1 = 0;
            _pcs2 = 0;
            _pcs3 = 0;
            _pcs4 = 0;
            _pcs5 = 0;
            _pcs6 = 0;
            _pcs7 = 0;
            _pcs8 = 0;

        }

        #endregion

        #region Methods
        
        public static DataRow Insert(int OrderIdx, int ColorIdx,
            int SizeIdx1, int SizeIdx2, int SizeIdx3, int SizeIdx4, int SizeIdx5, int SizeIdx6, int SizeIdx7, int SizeIdx8)
        {
            DataRow row = Data.OrderColorData.Insert(OrderIdx, ColorIdx,
            SizeIdx1, SizeIdx2, SizeIdx3, SizeIdx4, SizeIdx5, SizeIdx6, SizeIdx7, SizeIdx8);

            return row;
        }
        
        public static DataSet Getlist(int OrderIdx)
        {
            DataSet ds = new DataSet();
            
            ds = Data.OrderColorData.Getlist(OrderIdx); 
                        
            return ds;
        }
       
        public bool Update()
        {
            bool blRtn;
            blRtn = Data.OrderColorData.Update(_idx, _colorIdx, 
                _pcs1, _pcs2, _pcs3, _pcs4, _pcs5, _pcs6, _pcs7, _pcs8
                );
            return blRtn;
        }
        
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.OrderColorData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
