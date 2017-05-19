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

}
