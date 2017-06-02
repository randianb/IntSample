using System;

namespace Dev.Fabric
{
    /// <summary>
    /// 원단 QR 스티커 발행용
    /// </summary>
    public class FabricQRStruct
    {
        public string WorkOrderIdx { get; set; }             // 입고번호
        public string Qrcode { get; set;  }          // QR코드
        public string ColorIdx { get; set;  }           // 컬러
        public string FabricIdx { get; set; }           // 원단
        public string FabricType { get; set; }           // Single, Rib
    }
    
    /// <summary>
    /// 원단창고내 랙 또는 적재위치 구분용 스티커 발행용
    /// </summary>
    public class LocationQRStruct
    {
        public string Qrcode { get; set; }          // QR코드
        public string Idx { get; set; }           // Location ID
        public string CenterIdx { get; set; }           // 비용센터
        public string DeptIdx { get; set; }           // 관리부서
        public string Warehouse { get; set; }           // 창고, 적재위치 
        public string PosX { get; set; }           // X좌표
        public string PosY { get; set; }           // Y좌표
        public string RackNo { get; set; }           // 랙번호
        public string Floorno { get; set; }           // 층번호
        public string RackPos { get; set; }           // 열번호

    }
}
