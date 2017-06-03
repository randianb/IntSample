using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Controller
{
    public class Outbound
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
        private int _orderIdx;
        private string _workOrderIdx;
        private int _handler;
        private int _inIdx;
        private int _isOut;
        
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
        public int OrderIdx
        {
            get { return _orderIdx; }
            set { _orderIdx = value; }
        }
        //
        public string WorkOrderIdx
        {
            get { return _workOrderIdx; }
            set { _workOrderIdx = value; }
        }
        //
        public int Handler
        {
            get { return _handler; }
            set { _handler = value; }
        }
        //
        public int InIdx
        {
            get { return _inIdx; }
            set { _inIdx = value; }
        }
        //
        public int IsOut
        {
            get { return _isOut; }
            set { _isOut = value; }
        }
                
        #endregion

        #region Constructor 

        public Outbound(int Idx)
        {
            _row = Data.OutboundData.Get(Idx);

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
                
                if (_row["Kgs"] != DBNull.Value) _kgs = Convert.ToInt32(_row["Kgs"]);
                if (_row["Yds"] != DBNull.Value) _yds = Convert.ToInt32(_row["Yds"]);
                if (_row["RegCenterIdx"] != DBNull.Value) _regCenterIdx = Convert.ToInt32(_row["RegCenterIdx"]);
                if (_row["RegDeptIdx"] != DBNull.Value) _regDeptIdx = Convert.ToInt32(_row["RegDeptIdx"]);
                if (_row["RegUserIdx"] != DBNull.Value) _regUserIdx = Convert.ToInt32(_row["RegUserIdx"]);
                if (_row["RegDate"] != DBNull.Value) _regDate = Convert.ToDateTime(_row["RegDate"]);
                if (_row["IOCenterIdx"] != DBNull.Value) _iOCenterIdx = Convert.ToInt32(_row["IOCenterIdx"]);
                if (_row["IODeptIdx"] != DBNull.Value) _iODeptIdx = Convert.ToInt32(_row["IODeptIdx"]);
                if (_row["Comments"] != DBNull.Value) _comments = _row["Comments"].ToString();
                if (_row["OrderIdx"] != DBNull.Value) _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
                if (_row["WorkOrderIdx"] != DBNull.Value) _workOrderIdx = _row["WorkOrderIdx"].ToString();
                
                if (_row["Handler"] != DBNull.Value) _handler = Convert.ToInt32(_row["Handler"]);
                if (_row["InIdx"] != DBNull.Value) _inIdx = Convert.ToInt32(_row["InIdx"]);
                if (_row["IsOut"] != DBNull.Value) _isOut = Convert.ToInt32(_row["IsOut"]);
                
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
            _orderIdx = 0;
            _workOrderIdx = "";
            _handler = 0;
            _inIdx = 0;
            _isOut = 0;
            
    }
        #endregion

        #region Methods

        /// <summary>
        /// 스캔 출고
        /// </summary>
        public static DataRow Insert(string InWorkOrderIdx, int Status, int RegCenterIdx, int RegDeptIdx, int RegUserIdx, int IOCenterIdx, int IODeptIdx,
                int OrderIdx, string WorkOrderIdx)
        {
            DataRow row = Data.OutboundData.Insert(InWorkOrderIdx, Status, RegCenterIdx, RegDeptIdx, RegUserIdx, IOCenterIdx, IODeptIdx,
                OrderIdx, WorkOrderIdx);
            return row;
        }
        /// <summary>
        /// 수동 출고
        /// </summary>
        public static DataRow Insert2(int Status, int RegCenterIdx, int RegDeptIdx, int RegUserIdx, string WorkOrderIdx, int InIdx)
        {
            DataRow row = Data.OutboundData.Insert2(Status, RegCenterIdx, RegDeptIdx, RegUserIdx, WorkOrderIdx, InIdx);
            return row;
        }
        
        public static DataSet Getlist(Dictionary<CommonValues.KeyName, string> SearchString, Dictionary<CommonValues.KeyName, int> SearchKey, string indate)
        {
            DataSet ds = new DataSet();
            ds = Data.OutboundData.Getlist(SearchString, SearchKey, indate);
            return ds;
        }
        
        public bool Update()
        {
            bool blRtn;
            blRtn = Data.OutboundData.Update(_Idx, _iDate, _buyerIdx, _colorIdx, _fabricType,
                                        _artno, _lotno, _fabricIdx, _roll, _width, _kgs, _yds,
                                        _orderIdx, _comments, _handler, _inIdx, _isOut);
            return blRtn;
        }
        
        // Multiple Delete
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.OutboundData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
