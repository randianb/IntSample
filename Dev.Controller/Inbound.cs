using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class Inbound
    {
        #region Members

        private int _Idx;
        private int _status; 
        private DateTime _iDate;
        private int _buyerIdx;
        private string _colorIdx;
        private int _fabricType;
        private string _artno;
        private string _lotno;
        private int _fabricIdx;
        private int _roll;
        private int _width;
        private double _kgs;
        private double _yds;

        private int _regCenterIdx;
        private int _regDeptIdx;
        private int _regUserIdx;
        private DateTime _regDate;
        private int _iOCenterIdx;
        private int _iODeptIdx;
        private string _comments;
        private string _workOrderIdx;
        private int _rackNo;
        private int _floorno;
        private int _rackPos;
        private int _posX;
        private int _posY;
        private string _qrcode;
        private string _filenm1;
        private string _filenm2;
        private string _fileurl1;
        private string _fileurl2;
        
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
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }
        // 
        public DateTime IDate
        {
            get { return _iDate; }
            set { _iDate = value; }
        }
        // 
        public int BuyerIdx
        {
            get { return _buyerIdx; }
            set { _buyerIdx = value; }
        }
        // 
        public string ColorIdx
        {
            get { return _colorIdx; }
            set { _colorIdx = value; }
        }
        // 
        public int FabricType
        {
            get { return _fabricType; }
            set { _fabricType = value; }
        }
        // 
        public string Artno
        {
            get { return _artno; }
            set { _artno = value; }
        }
        // 
        public string Lotno
        {
            get { return _lotno; }
            set { _lotno = value; }
        }
        // 
        public int FabricIdx
        {
            get { return _fabricIdx; }
            set { _fabricIdx = value; }
        }

        // 
        public int Roll
        {
            get { return _roll; }
            set { _roll = value; }
        }
        // 
        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }
        // 
        public double Kgs
        {
            get { return _kgs; }
            set { _kgs = value; }
        }
        // 
        public double Yds
        {
            get { return _yds; }
            set { _yds = value; }
        }
        // 
        public int RegCenterIdx
        {
            get { return _regCenterIdx; }
            set { _regCenterIdx = value; }
        }
        // 
        public int RegDeptIdx
        {
            get { return _regDeptIdx; }
            set { _regDeptIdx = value; }
        }
        //사용여부
        public int RegUserIdx
        {
            get { return _regUserIdx; }
            set { _regUserIdx = value; }
        }
        //
        public DateTime RegDate
        {
            get { return _regDate; }
            set { _regDate = value; }
        }
        //
        public int IOCenterIdx
        {
            get { return _iOCenterIdx; }
            set { _iOCenterIdx = value; }
        }
        //
        public int IODeptIdx
        {
            get { return _iODeptIdx; }
            set { _iODeptIdx = value; }
        }
        //
        public string Comments
        {
            get { return _comments; }
            set { _comments = value; }
        }
        //
        public string WorkOrderIdx
        {
            get { return _workOrderIdx; }
            set { _workOrderIdx = value; }
        }
        //
        public int RackNo
        {
            get { return _rackNo; }
            set { _rackNo = value; }
        }
        //
        public int Floorno
        {
            get { return _floorno; }
            set { _floorno = value; }
        }
        //
        public int RackPos
        {
            get { return _rackPos; }
            set { _rackPos = value; }
        }
        //
        public int PosX
        {
            get { return _posX; }
            set { _posX = value; }
        }
        //
        public int PosY
        {
            get { return _posY; }
            set { _posY = value; }
        }
        //
        public string Qrcode
        {
            get { return _qrcode; }
            set { _qrcode = value; }
        }
        //
        public string Filenm1
        {
            get { return _filenm1; }
            set { _filenm1 = value; }
        }
        //
        public string Filenm2
        {
            get { return _filenm2; }
            set { _filenm2 = value; }
        }
        //
        public string Fileurl1
        {
            get { return _fileurl1; }
            set { _fileurl1 = value; }
        }
        //
        public string Fileurl2
        {
            get { return _fileurl2; }
            set { _fileurl2 = value; }
        }
        
        #endregion

        #region Constructor 

        public Inbound(int Idx)
        {
            _row = Data.InboundData.Get(Idx);

            if (_row != null)
            {
                _Idx = Convert.ToInt32(_row["Idx"]);
                if (_row["Status"] != DBNull.Value) _status = Convert.ToInt32(_row["Status"]);
                if (_row["IDate"] != DBNull.Value) _iDate = Convert.ToDateTime(_row["IDate"]);
                if (_row["BuyerIdx"] != DBNull.Value) _buyerIdx = Convert.ToInt32(_row["BuyerIdx"]);
                if (_row["ColorIdx"] != DBNull.Value) _colorIdx = Convert.ToString(_row["ColorIdx"]);
                if (_row["FabricType"] != DBNull.Value) _fabricType = Convert.ToInt32(_row["FabricType"]);
                if (_row["Artno"] != DBNull.Value) _artno = _row["Artno"].ToString();
                if (_row["Lotno"] != DBNull.Value) _lotno = _row["Lotno"].ToString();
                if (_row["FabricIdx"] != DBNull.Value) _fabricIdx = Convert.ToInt32(_row["FabricIdx"]);
                if (_row["Roll"] != DBNull.Value) _roll = Convert.ToInt32(_row["Roll"]);
                if (_row["Width"] != DBNull.Value) _width = Convert.ToInt32(_row["Width"]);
                
                if (_row["Kgs"] != DBNull.Value) _kgs = Convert.ToDouble(_row["Kgs"]);
                if (_row["Yds"] != DBNull.Value) _yds = Convert.ToDouble(_row["Yds"]);
                if (_row["RegCenterIdx"] != DBNull.Value) _regCenterIdx = Convert.ToInt32(_row["RegCenterIdx"]);
                if (_row["RegDeptIdx"] != DBNull.Value) _regDeptIdx = Convert.ToInt32(_row["RegDeptIdx"]);
                if (_row["RegUserIdx"] != DBNull.Value) _regUserIdx = Convert.ToInt32(_row["RegUserIdx"]);
                if (_row["RegDate"] != DBNull.Value) _regDate = Convert.ToDateTime(_row["RegDate"]);
                if (_row["IOCenterIdx"] != DBNull.Value) _iOCenterIdx = Convert.ToInt32(_row["IOCenterIdx"]);
                if (_row["IODeptIdx"] != DBNull.Value) _iODeptIdx = Convert.ToInt32(_row["IODeptIdx"]);
                if (_row["Comments"] != DBNull.Value) _comments = _row["Comments"].ToString();

                if (_row["WorkOrderIdx"] != DBNull.Value) _workOrderIdx = _row["WorkOrderIdx"].ToString();
                if (_row["RackNo"] != DBNull.Value) _rackNo = Convert.ToInt32(_row["RackNo"]);
                if (_row["Floorno"] != DBNull.Value) _floorno = Convert.ToInt32(_row["Floorno"]);
                if (_row["RackPos"] != DBNull.Value) _rackPos = Convert.ToInt32(_row["RackPos"]);
                if (_row["PosX"] != DBNull.Value) _posX = Convert.ToInt32(_row["PosX"]);
                if (_row["PosY"] != DBNull.Value) _posY = Convert.ToInt32(_row["PosY"]);
                if (_row["Qrcode"] != DBNull.Value) _qrcode = _row["Qrcode"].ToString();
                if (_row["filenm1"] != DBNull.Value) _filenm1 = _row["filenm1"].ToString();
                if (_row["filenm2"] != DBNull.Value) _filenm2 = _row["filenm2"].ToString();
                if (_row["fileurl1"] != DBNull.Value) _fileurl1 = _row["fileurl1"].ToString();
                if (_row["fileurl2"] != DBNull.Value) _fileurl2 = _row["fileurl2"].ToString();

            }
            else
            {
                Initializer();
            }
        }
        
        private void Initializer()
        {
            _Idx = 0; 
            _status = 0;
            _iDate = DateTime.Now; 
            _buyerIdx = 0;
            _colorIdx = "";
            _fabricType = 0;
            _artno = ""; 
            _lotno = "";
            _fabricIdx = 0;
            _roll = 0;
            _width = 0;
            _kgs = 0f; 
            _yds = 0f;
            _regCenterIdx = 0;
            _regDeptIdx = 0;
            _regUserIdx = 0;
            _regDate = DateTime.Now;
            _iOCenterIdx = 0;
            _iODeptIdx = 0;
            _comments = "";
            _workOrderIdx = "";
            _rackNo = 0;
            _floorno = 0;
            _rackPos = 0;
            _posX = 0;
            _posY = 0;
            _qrcode = "";
            _filenm1 = "";
            _filenm2 = "";
            _fileurl1 = "";
            _fileurl2 = "";
    }
        #endregion

        #region Methods

        // Insert
        public static DataRow Insert(string WorkOrderIdx, string Qrcode, int RegCenterIdx, int RegDeptIdx, int RegUserIdx)
        {
            DataRow row = Data.InboundData.Insert(WorkOrderIdx, Qrcode, RegCenterIdx, RegDeptIdx, RegUserIdx);
            return row;
        }

        // Getlist with search parameter
        public static DataSet Getlist()
        {
            DataSet ds = new DataSet();
            ds = Data.InboundData.Getlist();
            return ds;
        }

        public static DataRow Getlist(string WorkOrderIdx)
        {
            DataRow dr = Data.InboundData.Getlist(WorkOrderIdx);
            return dr;
        }

        public static DataSet Getlist(Dictionary<CommonValues.KeyName, string> SearchString, Dictionary<CommonValues.KeyName, int> SearchKey, string indate)
        {
            DataSet ds = new DataSet();
            ds = Data.InboundData.Getlist(SearchString, SearchKey, indate);
            return ds;
        }

        public static DataSet GetlistWareHouse(Dictionary<CommonValues.KeyName, string> SearchString, Dictionary<CommonValues.KeyName, int> SearchKey, string indate)
        {
            DataSet ds = new DataSet();
            ds = Data.InboundData.GetlistWareHouse(SearchString, SearchKey, indate);
            return ds;
        }

        public bool Update()
        {
            bool blRtn;
            blRtn = Data.InboundData.Update(_Idx, _status, _iDate, _buyerIdx, _colorIdx, _fabricType,
                                        _artno, _lotno, _fabricIdx, _roll, _width, _kgs, _yds,
                                        _iOCenterIdx, _iODeptIdx, _comments, _rackNo, _floorno, _rackPos,
                                        _posX, _posY, _qrcode, _filenm1, _filenm2, _fileurl1, _fileurl2);
            return blRtn;
        }

        /// <summary>
        /// 코드스캔시 적재위치 업데이트 
        /// </summary>
        public static bool UpdateIn(string WorkOrderIdx, DateTime Indate, int RackNo, int Floorno, int RackPos, int PosX, int PosY)
        {
            bool blRtn;
            blRtn = Data.InboundData.UpdateIn(WorkOrderIdx, Indate, RackNo, Floorno, RackPos, PosX, PosY);
            return blRtn;
        }

        // Multiple Delete
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.InboundData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
