using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using Telerik.WinControls;

namespace Dev.Controller
{
    public class OrderType
    {
        #region Members

        private int _idx;
        private int _orderIdx;
        private int _ordSizeIdx;
        
        private int _type101;
        private int _type102;
        private int _type103;

        private int _type201;
        private int _type202;
        private int _type203;
        private int _type204;
        private int _type205;
        private int _type206;
        private int _type207;
        private int _type208;
        private int _type209;
        private int _type210;

        private int _type211;
        private int _type212;
        private int _type213;
        private int _type214;
        private int _type215;
        private int _type216;
        private int _type217;
        private int _type218;
        private int _type219;
        private int _type220;
        private int _type221;
        private int _type222;
        private int _type223;
        private int _type224;
        private int _type225;

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
        public int OrdSizeIdx
        {
            get { return _ordSizeIdx; }
            set { _ordSizeIdx = value; }
        }
        //
        public int Type101
        {
            get { return _type101; }
            set { _type101 = value; }
        }
        //
        public int Type102
        {
            get { return _type102; }
            set { _type102 = value; }
        }
        //
        public int Type103
        {
            get { return _type103; }
            set { _type103 = value; }
        }
        //
        public int Type201
        {
            get { return _type201; }
            set { _type201 = value; }
        }
        //
        public int Type202
        {
            get { return _type202; }
            set { _type202 = value; }
        }
        //
        public int Type203
        {
            get { return _type203; }
            set { _type203 = value; }
        }
        //
        public int Type204
        {
            get { return _type204; }
            set { _type204 = value; }
        }
        //
        public int Type205
        {
            get { return _type205; }
            set { _type205 = value; }
        }
        //
        public int Type206
        {
            get { return _type206; }
            set { _type206 = value; }
        }
        //
        public int Type207
        {
            get { return _type207; }
            set { _type207 = value; }
        }
        //
        public int Type208
        {
            get { return _type208; }
            set { _type208 = value; }
        }
        //
        public int Type209
        {
            get { return _type209; }
            set { _type209 = value; }
        }
        //
        public int Type210
        {
            get { return _type210; }
            set { _type210 = value; }
        }
        //
        public int Type211
        {
            get { return _type211; }
            set { _type211 = value; }
        }
        //
        public int Type212
        {
            get { return _type212; }
            set { _type212 = value; }
        }
        //
        public int Type213
        {
            get { return _type213; }
            set { _type213 = value; }
        }
        //
        public int Type214
        {
            get { return _type214; }
            set { _type214 = value; }
        }
        //
        public int Type215
        {
            get { return _type215; }
            set { _type215 = value; }
        }
        //
        public int Type216
        {
            get { return _type216; }
            set { _type216 = value; }
        }
        //
        public int Type217
        {
            get { return _type217; }
            set { _type217 = value; }
        }
        //
        public int Type218
        {
            get { return _type218; }
            set { _type218 = value; }
        }
        //
        public int Type219
        {
            get { return _type219; }
            set { _type219 = value; }
        }
        //
        public int Type220
        {
            get { return _type220; }
            set { _type220 = value; }
        }
        //
        public int Type221
        {
            get { return _type221; }
            set { _type221 = value; }
        }

        //
        public int Type222
        {
            get { return _type222; }
            set { _type222 = value; }
        }

        //
        public int Type223
        {
            get { return _type223; }
            set { _type223 = value; }
        }

        //
        public int Type224
        {
            get { return _type224; }
            set { _type224 = value; }
        }

        //
        public int Type225
        {
            get { return _type225; }
            set { _type225 = value; }
        }

        #endregion

        #region Constructor 

        public OrderType(int Idx)
        {
            _row = Dev.Data.OrderTypeData.Get(Idx);
            
            if (_row != null)
            {
                _idx = Convert.ToInt32(_row["Idx"]);
                if (_row["OrderIdx"] != DBNull.Value) _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
                if (_row["OrdSizeIdx"] != DBNull.Value) _ordSizeIdx = Convert.ToInt32(_row["OrdSizeIdx"]);

                if (_row["Type101"] != DBNull.Value) _type101 = Convert.ToInt32(_row["Type101"]);
                if (_row["Type102"] != DBNull.Value) _type102 = Convert.ToInt32(_row["Type102"]);
                if (_row["Type103"] != DBNull.Value) _type103 = Convert.ToInt32(_row["Type103"]);

                if (_row["Type201"] != DBNull.Value) _type201 = Convert.ToInt32(_row["Type201"]);
                if (_row["Type202"] != DBNull.Value) _type202 = Convert.ToInt32(_row["Type202"]);
                if (_row["Type203"] != DBNull.Value) _type203 = Convert.ToInt32(_row["Type203"]);
                if (_row["Type204"] != DBNull.Value) _type204 = Convert.ToInt32(_row["Type204"]);
                if (_row["Type205"] != DBNull.Value) _type205 = Convert.ToInt32(_row["Type205"]);
                if (_row["Type206"] != DBNull.Value) _type206 = Convert.ToInt32(_row["Type206"]);
                if (_row["Type207"] != DBNull.Value) _type207 = Convert.ToInt32(_row["Type207"]);
                if (_row["Type208"] != DBNull.Value) _type208 = Convert.ToInt32(_row["Type208"]);
                if (_row["Type209"] != DBNull.Value) _type209 = Convert.ToInt32(_row["Type209"]);
                if (_row["Type210"] != DBNull.Value) _type210 = Convert.ToInt32(_row["Type210"]);

                if (_row["Type211"] != DBNull.Value) _type211 = Convert.ToInt32(_row["Type211"]);
                if (_row["Type212"] != DBNull.Value) _type212 = Convert.ToInt32(_row["Type212"]);
                if (_row["Type213"] != DBNull.Value) _type213 = Convert.ToInt32(_row["Type213"]);
                if (_row["Type214"] != DBNull.Value) _type214 = Convert.ToInt32(_row["Type214"]);
                if (_row["Type215"] != DBNull.Value) _type215 = Convert.ToInt32(_row["Type215"]);
                if (_row["Type216"] != DBNull.Value) _type216 = Convert.ToInt32(_row["Type216"]);
                if (_row["Type217"] != DBNull.Value) _type217 = Convert.ToInt32(_row["Type217"]);
                if (_row["Type218"] != DBNull.Value) _type218 = Convert.ToInt32(_row["Type218"]);
                if (_row["Type219"] != DBNull.Value) _type219 = Convert.ToInt32(_row["Type219"]);
                if (_row["Type220"] != DBNull.Value) _type220 = Convert.ToInt32(_row["Type220"]);
                if (_row["Type221"] != DBNull.Value) _type221 = Convert.ToInt32(_row["Type221"]);

                if (_row["Type222"] != DBNull.Value) _type222 = Convert.ToInt32(_row["Type222"]);
                if (_row["Type223"] != DBNull.Value) _type223 = Convert.ToInt32(_row["Type223"]);
                if (_row["Type224"] != DBNull.Value) _type224 = Convert.ToInt32(_row["Type224"]);
                if (_row["Type225"] != DBNull.Value) _type225 = Convert.ToInt32(_row["Type225"]);

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
            _ordSizeIdx = 0;

            _type101 = 0;
            _type102 = 0;
            _type103 = 0;

            _type201 = 0;
            _type202 = 0;
            _type203 = 0;
            _type204 = 0;
            _type205 = 0;
            _type206 = 0;
            _type207 = 0;
            _type208 = 0;
            _type209 = 0;
            _type210 = 0;

            _type211 = 0;
            _type212 = 0;
            _type213 = 0;
            _type214 = 0;
            _type215 = 0;
            _type216 = 0;
            _type217 = 0;
            _type218 = 0;
            _type219 = 0;
            _type220 = 0;
            _type221 = 0;
            _type222 = 0;
            _type223 = 0;
            _type224 = 0;
            _type225 = 0;
        }

        #endregion

        #region Methods

        public static DataRow Insert(int OrderIdx, int OrdSizeIdx)
        {
            DataRow row = Data.OrderTypeData.Insert(OrderIdx, OrdSizeIdx);

            if (row == null)
            {
                RadMessageBox.Show("Failed to insert. \nPlease ensure if there's inserted same size.");
                return null; 
            }
            else
            {
                return row;
            }
            
        }

        public static DataSet Getlist(int OrderIdx)
        {
            DataSet ds = new DataSet();

            ds = Data.OrderTypeData.Getlist(OrderIdx);

            return ds;
        }

        public static DataRow Getlist(int OrderIdx, int OrdSizeIdx)
        {
            DataRow ds = null;

            if (OrdSizeIdx>0)
                ds = Data.OrderTypeData.Getlist(OrderIdx, OrdSizeIdx).Tables[0].Rows[0];

            if (ds != null)
                return ds;
            else
                return null; 
        }
       
        public bool Update()
        {
            bool blRtn;
            blRtn = Data.OrderTypeData.Update(_idx, _type101, _type102, _type103, 
                _type201, _type202, _type203, _type204, _type205, _type206, _type207, _type208, _type209, _type210, 
                _type211, _type212, _type213, _type214, _type215, _type216, _type217, _type218, _type219, _type220, 
                _type221, _type222, _type223, _type224, _type225 
                );
            return blRtn;
        }
        
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.OrderTypeData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
