using System;

namespace Dev.WorkOrder
{
    /// <summary>
    /// 프로그램 버전정보 저장
    /// </summary>
    public class WorkOrderTicketStruct
    {
        public string WorkOrderIdx { get; set; }             // 작업번호
        public string Operation { get; set;  }             // 공정명
        public string Qrcode { get; set;  }          // QR코드
        public string Fileno { get; set;  }           // 파일번호
        public string Styleno { get; set; }           // 스타일
        public string Buyer { get; set; }           // 바이어
        public string Handler { get; set; }           // 담당자
        public string Fabric { get; set; }           // 원단
        public string Size { get; set; }           // 사이즈
        public string SampleType { get; set; }           // 샘플타입
        public string Color { get; set; }           // 컬러
        public double Yds { get; set; }           // 컬러
        public int Qty { get; set; }           // 컬러
        public DateTime OrderDate { get; set; }      // 발행일 
        public DateTime TicketDate { get; set; }      // 발행일 
    }

}
