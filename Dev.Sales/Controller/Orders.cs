using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;

namespace Dev.Sales.Controller
{
    public class Orders
    {
        #region Members

        private int _idx;
        private string _fileNo;
        private int _deptIdx;
        private int _reorder;
        private string _reorderReason;
        private DateTime _indate;
        private int _buyer;
        private int _vendor;
        private int _country;

        private string _pono;
        private string _styleNo;
        private string _sampleType;
        private string _inspType;

        private string _season;
        private string _description;
        private DateTime _deliveryDate;
        private int _isPrinting;
        private int _embelishId1;
        private int _embelishId2;
        private int _sizeGroupIdx;
        private int _sewThreadIdx;

        private int _orderQty;
        private double _orderPrice;
        private double _orderAmount;
        private string _remark;

        private DateTime _teamRequestedDate;
        private DateTime _splConfirmedDate;
        
        private int _handler;
        private int _status; 

        private DataRow _row;

        #endregion

        #region Property 

        //ID
        public int Idx
        {
            get { return _idx; }
            set { _idx = value; }
        }
        //영업파일번호 
        public string Fileno
        {
            get { return _fileNo; }
            set { _fileNo = value; }
        }
        //Division
        public int DeptIdx
        {
            get { return _deptIdx; }
            set { _deptIdx = value; }
        }
        
        // 재작업번호 
        public int Reorder
        {
            get { return _reorder; }
            set { _reorder = value; }
        }
        // 재작업 사유
        public string ReorderReason
        {
            get { return _reorderReason; }
            set { _reorderReason = value; }
        }
        
        //이슈날자
        public DateTime Indate
        {
            get { return _indate; }
            set { _indate = value; }
        }
        //바이어
        public int Buyer
        {
            get { return _buyer; }
            set { _buyer = value; }
        }
        
        //Vendor 
        public int Vendor
        {
            get { return _vendor; }
            set { _vendor = value; }
        }
        //제조국가
        public int Country
        {
            get { return _country; }
            set { _country = value; }
        }

        //PO#
        public string Pono
        {
            get { return _pono; }
            set { _pono = value; }
        }
        //Style#
        public string Styleno
        {
            get { return _styleNo; }
            set { _styleNo = value; }
        }
        //Style#
        public string SampleType
        {
            get { return _sampleType; }
            set { _sampleType = value; }
        }
        //Style#
        public string InspType
        {
            get { return _inspType; }
            set { _inspType = value; }
        }
        
        //Season
        public string Season
        {
            get { return _season; }
            set { _season = value; }
        }
        //Description
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
        //Delivery Date
        public DateTime DeliveryDate
        {
            get { return _deliveryDate; }
            set { _deliveryDate = value; }
        }
        //나염여부
        public int IsPrinting
        {
            get { return _isPrinting; }
            set { _isPrinting = value; }
        }
        //나염
        public int EmbelishId1
        {
            get { return _embelishId1; }
            set
            {
                _embelishId1 = value;
            }
        }
        //나염
        public int EmbelishId2
        {
            get { return _embelishId2; }
            set { _embelishId2 = value; }
        }
        
        //사이즈그룹
        public int SizeGroupIdx
        {
            get { return _sizeGroupIdx; }
            set { _sizeGroupIdx = value; }
        }
        //재봉사
        public int SewThreadIdx
        {
            get { return _sewThreadIdx; }
            set { _sewThreadIdx = value; }
        }
        //오더수량
        public int OrderQty
        {
            get { return _orderQty; }
            set { _orderQty = value; }
        }
        //단가
        public double OrderPrice
        {
            get { return _orderPrice; }
            set { _orderPrice = value; }
        }
        
        //금액
        public double OrderAmount
        {
            get { return _orderAmount; }
            set { _orderAmount = value; }
        }
       
        //비고
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        //요청일
        public DateTime TeamRequestedDate
        {
            get { return _teamRequestedDate; }
            set { _teamRequestedDate = value; }
        }
        //접수일
        public DateTime SplConfirmedDate
        {
            get { return _splConfirmedDate; }
            set { _splConfirmedDate = value; }
        }
        public int Handler
        {
            get { return _handler; }
            set { _handler = value; }
        }
        
        public int Status
        {
            get { return _status; }
            set { _status = value; }
        }

        #endregion

        #region Constructor 
        public Orders(int Idx)
        {
            _row = Data.OrdersData.Get(Idx);

            if (_row != null)
            {
                _idx = Convert.ToInt32(_row["Idx"]);
                if (_row["Fileno"] != DBNull.Value) _fileNo = Convert.ToString(_row["Fileno"]);
                if (_row["DeptIdx"] != DBNull.Value) _deptIdx = Convert.ToInt32(_row["DeptIdx"]);
                if (_row["Reorder"] != DBNull.Value) _reorder = Convert.ToInt32(_row["Reorder"]);
                if (_row["ReorderReason"] != DBNull.Value) _reorderReason = Convert.ToString(_row["ReorderReason"]);
               
                if (_row["Indate"] != DBNull.Value) _indate = Convert.ToDateTime(_row["Indate"]);
                if (_row["Buyer"] != DBNull.Value) _buyer = Convert.ToInt32(_row["Buyer"]);
                if (_row["Vendor"] != DBNull.Value) _vendor = Convert.ToInt32(_row["Vendor"]);
                if (_row["Country"] != DBNull.Value) _country = Convert.ToInt32(_row["Country"]);

                if (_row["Pono"] != DBNull.Value) _pono = Convert.ToString(_row["Pono"]);
                if (_row["Styleno"] != DBNull.Value) _styleNo = Convert.ToString(_row["Styleno"]);
                if (_row["SampleType"] != DBNull.Value) _sampleType = Convert.ToString(_row["SampleType"]);
                if (_row["InspType"] != DBNull.Value) _inspType = Convert.ToString(_row["InspType"]);

                if (_row["Season"] != DBNull.Value) _season = Convert.ToString(_row["Season"]);
                if (_row["Description"] != DBNull.Value) _description = Convert.ToString(_row["Description"]);
                if (_row["DeliveryDate"] != DBNull.Value) _deliveryDate = Convert.ToDateTime(_row["DeliveryDate"]);
                if (_row["IsPrinting"] != DBNull.Value) _isPrinting = Convert.ToInt32(_row["IsPrinting"]);
                if (_row["EmbelishId1"] != DBNull.Value) _embelishId1 = Convert.ToInt32(_row["EmbelishId1"]);
                if (_row["EmbelishId2"] != DBNull.Value) _embelishId2 = Convert.ToInt32(_row["EmbelishId2"]);

                if (_row["SizeGroupIdx"] != DBNull.Value) _sizeGroupIdx = Convert.ToInt32(_row["SizeGroupIdx"]);
                if (_row["SewThreadIdx"] != DBNull.Value) _sewThreadIdx = Convert.ToInt32(_row["SewThreadIdx"]);
                
                if (_row["OrderQty"] != DBNull.Value) _orderQty = Convert.ToInt32(_row["OrderQty"]);
                if (_row["OrderPrice"] != DBNull.Value) _orderPrice = Convert.ToInt32(_row["OrderPrice"]);
                if (_row["OrderAmount"] != DBNull.Value) _orderAmount = Convert.ToInt32(_row["OrderAmount"]);
                        
                if (_row["Remark"] != DBNull.Value) _remark = Convert.ToString(_row["Remark"]);
                if (_row["TeamRequestedDate"] != DBNull.Value) _teamRequestedDate = Convert.ToDateTime(_row["TeamRequestedDate"]);
                if (_row["SplConfirmedDate"] != DBNull.Value) _splConfirmedDate = Convert.ToDateTime(_row["SplConfirmedDate"]);
            
                if (_row["Handler"] != DBNull.Value) _handler = Convert.ToInt32(_row["Handler"]);
                if (_row["Status"] != DBNull.Value) _status = Convert.ToInt32(_row["Status"]);

            }
            else
            {
                Initializer();
            }
        }

        private void Initializer()
        {
            _idx = 0;
            _fileNo = "";
            _deptIdx = 0;
            _reorder = 0;
            _reorderReason = ""; 
            _indate = DateTime.Now;
            _buyer = 0;
            _vendor = 0;
            _country = 0;
            _pono = "";
            _styleNo = "";
            _sampleType = "000";
            _inspType = "00000000000000000000";
            _season = "";
            _description = "";
            _deliveryDate = DateTime.Now;
            _isPrinting = 0;
            _embelishId1 = 0;
            _embelishId2 = 0;
            _sizeGroupIdx = 0;
            _sewThreadIdx = 0; 
            _orderQty = 0;
            _orderPrice = 0.0f;
            _orderAmount = 0.0f;
            _remark = "";
            _teamRequestedDate = DateTime.Now;
            _splConfirmedDate = DateTime.Now;
            _handler = 0;
            _status = 0;
        }
        #endregion

        #region Methods

        // ID없이 즉시 입력가능하도록 정적 메소드로 정의후 파라미터를 전달
        public static DataRow Insert(string Fileno, int DeptIdx, int Reorder, string ReorderReason,
            DateTime Indate, int Buyer, string Pono, string Styleno, 
            string Season, string Description, DateTime DeliveryDate, int IsPrinting,
            int EmbelishId1, int EmbelishId2, int SizeGroupIdx, int SewThreadIdx,
            int OrderQty, double OrderPrice, double OrderAmount, string Remark,
            DateTime TeamRequestedDate, DateTime SplConfirmedDate, int Handler)
        {
            DataRow row = Data.OrdersData.Insert(Fileno, DeptIdx, Reorder, ReorderReason, Indate, Buyer, 0, 0, 
                                    Pono, Styleno, "000", "00000000000000000000", Season, Description, DeliveryDate, IsPrinting,
                                    EmbelishId1, EmbelishId2, SizeGroupIdx, SewThreadIdx, OrderQty, OrderPrice, OrderAmount, Remark,
                                    TeamRequestedDate, SplConfirmedDate, 
                                    UserInfo.Idx);
            return row;
        }

        // 어디서든 즉시 조회 가능하도록 정적 메소드로 정의
        public static DataSet Getlist(int KeyCount, Dictionary<CommonValues.KeyName, int> SearchKey, string fileno, string style)
        {
            DataSet ds = new DataSet();

            switch (KeyCount)
            {
                case 0: // 전체조회
                    ds = Data.OrdersData.Getlist(); break;
                case 2: // 부서+바이어+상태+나염
                    ds = Data.OrdersData.Getlist(SearchKey, fileno, style);
                    break;
                default:
                    break;
            }
            
            return ds;
        }
        ///// <summary>
        ///// Report
        ///// </summary>
        ///// <param name="DeptIdx"></param>
        ///// <param name="fromDate"></param>
        ///// <param name="toDate"></param>
        ///// <param name="bIncShip"></param>
        ///// <returns></returns>
        //public static DataSet OrderDetail(int DeptIdx, DateTime fromDate, DateTime toDate, bool bIncShip)
        //{
        //    DataSet ds = new DataSet();

        //    if (bIncShip)
        //        ds = Dev.Sales.Data.OrderActualData.OrderDetail(DeptIdx, fromDate, toDate); 
        //    else
        //        ds = Dev.Sales.Data.OrderActualData.OrderDetailNoShip(DeptIdx, fromDate, toDate);

        //    return ds;
        //}

        //public static DataSet OrderSummary(DateTime indate)
        //{
        //    DataSet ds = new DataSet();
            
        //    ds = Dev.Sales.Data.OrderActualData.OrderSummary(indate);
            
        //    return ds;
        //}

        public bool Update()
        {
            bool blRtn;
            blRtn = Data.OrdersData.Update(_idx, _fileNo, _deptIdx, _reorder, _reorderReason, _indate, _buyer,  
                                                         _vendor, _country, _pono, _styleNo, _sampleType, _inspType, 
                                                         _season, _description, _deliveryDate, _isPrinting,
                                                         _embelishId1, _embelishId2, _sizeGroupIdx, _sewThreadIdx, 
                                                         _orderQty, _orderPrice, _orderAmount, _remark, 
                                                         _teamRequestedDate, _splConfirmedDate, UserInfo.Idx);
            return blRtn;
        }

        //public static DataSet OrderMonthly(DateTime indate)
        //{
        //    DataSet ds = new DataSet();

        //    ds = Dev.Sales.Data.OrderActualData.OrderMonthly(indate);

        //    return ds;
        //}
        
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.OrdersData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
