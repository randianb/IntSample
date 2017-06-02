using System;
using System.Data;

namespace Dev.Codes.Controller
{
    public class Location
    {
        #region Members

        private int _idx;
        private string _locationName;
        private int _costcenterIdx;
        private int _deptIdx;
        private int _warehouse;
        private int _posX;
        private int _posY;
        private int _rackNo;
        private int _floorno;
        private int _rackPos;
        private string _remark;
        private int _isUse;
        private string _qrcode;
        
        private DataRow _row;

        #endregion

        #region Property 
        
        public int      Idx             { get { return _idx; }     set { _idx = value; } }   
        public string   LocationName    { get { return _locationName; }     set { _locationName = value; } }   
        public int CostcenterIdx { get { return _costcenterIdx; } set { _costcenterIdx = value; } }   //
        public int DeptIdx { get { return _deptIdx; } set { _deptIdx = value; } }   //
        public int Warehouse { get { return _warehouse; } set { _warehouse = value; } }   // 비용센터내 창고구역 
        public int PosX { get { return _posX; } set { _posX = value; } }   // 가로 위치
        public int PosY { get { return _posY; } set { _posY = value; } }   // 세로 위치 
        public int RackNo { get { return _rackNo; } set { _rackNo = value; } }   // Rack이 있을경우 번호
        public int Floorno { get { return _floorno; } set { _floorno = value; } }   // 층번호
        public int RackPos { get { return _rackPos; } set { _rackPos = value; } }   // 열번호 
        public string Remark { get { return _remark; } set { _remark = value; } }   //
        public int IsUse { get { return _isUse; } set { _isUse = value; } }   //
        public string Qrcode { get { return _qrcode; } set { _qrcode = value; } }   //
        
        #endregion

        #region Constructor 

        public Location(int Idx)
        {
            _row = Data.LocationData.Get(Idx);

            if (_row != null)
            {
                _idx = Convert.ToInt32(_row["Idx"]);
                if (_row["LocationName"] != DBNull.Value) _locationName = Convert.ToString(_row["LocationName"]);
                if (_row["CostcenterIdx"] != DBNull.Value) _costcenterIdx = Convert.ToInt32(_row["CostcenterIdx"]);
                if (_row["DeptIdx"] != DBNull.Value) _deptIdx = Convert.ToInt32(_row["DeptIdx"]);
                if (_row["Warehouse"] != DBNull.Value) _warehouse = Convert.ToInt32(_row["Warehouse"]);
                if (_row["PosX"] != DBNull.Value) _posX = Convert.ToInt32(_row["PosX"]);
                if (_row["PosY"] != DBNull.Value) _posY = Convert.ToInt32(_row["PosY"]);
                if (_row["RackNo"] != DBNull.Value) _rackNo = Convert.ToInt32(_row["RackNo"]);
                if (_row["Floorno"] != DBNull.Value) _floorno = Convert.ToInt32(_row["Floorno"]);
                if (_row["RackPos"] != DBNull.Value) _rackPos = Convert.ToInt32(_row["RackPos"]);
                if (_row["Remark"] != DBNull.Value) _remark = Convert.ToString(_row["Remark"]);
                if (_row["IsUse"] != DBNull.Value) _isUse = Convert.ToInt32(_row["IsUse"]);
                if (_row["Qrcode"] != DBNull.Value) _qrcode = Convert.ToString(_row["Qrcode"]);
            }
            else
            {
                Initializer();
            }
        }

        private void Initializer()
        {
            _idx = 0;
            _locationName = "";
            _costcenterIdx = 0;
            _deptIdx = 0;
            _warehouse = 0;
            _posX = 0;
            _posY = 0;
            _rackNo = 0;
            _floorno = 0;
            _rackPos = 0;
            _remark = "";
            _isUse = 0;
            _qrcode = "";

        }
        #endregion

        #region Methods
        
        public static DataSet Getlist(int CostcenterIdx, int DeptIdx, int Warehouse, int RackNo,
            int Floorno, int RackPos, int PosX, int PosY, string Remark)
        {
            DataSet ds = new DataSet();
            ds = Data.LocationData.Getlist(CostcenterIdx, DeptIdx, Warehouse, PosX, PosY, RackNo,
                                        Floorno, RackPos, Remark);
            return ds;
        }
        
        // Insert
        public static DataRow Insert(int CostcenterIdx, int DeptIdx)
        {
            DataRow row = Data.LocationData.Insert(CostcenterIdx, DeptIdx);
            return row;
        }
         
        public static bool Update(int Idx, string LocationName, int CostcenterIdx, int DeptIdx, int Warehouse, int PosX, int PosY, int RackNo,
            int Floorno, int RackPos, string Remark, int IsUse, string Qrcode)
        {
            bool blRtn;
            blRtn = Data.LocationData.Update(Idx, LocationName, CostcenterIdx, DeptIdx, Warehouse, PosX, PosY, RackNo,
                                            Floorno, RackPos, Remark, IsUse, Qrcode);
            return blRtn;
        }
        
        // Multiple Delete
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.LocationData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
