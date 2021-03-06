﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Options
{
    public static class CommonValues
    {
        #region 전역변수: 향후 json으로 변경필요 (todo) 

        public const int packageNo = 100;                           // 패키지번호 = intsample
        public const string packageName = "intsample";
        public static string verNo = "";                            // 프로그램 버전번호 
        public static int DDL_DefaultItemsCountInDropDown = 30;     // DDL 갯수
        public static int DDL_DropDownHeight = 300;                 // DDL 높이
        public static int DDL_DropDownWidth = 300;                  // DDL 폭
        public static int PropertyValueColumnWidth = 230;           // 프로퍼티 값열 폭 
        public static bool enablePaging = false;                    // 그리드뷰 페이징 
        public static bool enableSearchRow = false;                 // 그리드뷰 전체 컬럼 대상으로 검색여부
        public static bool SetHandler = false;                      // 기본 핸들러 설정 

        public static bool OrderOpCheck1 = true;                    // 보기옵션 설정 Cancel
        public static bool OrderOpCheck2 = true;                    // 보기옵션 설정 Closed
        public static int  OrderOpCheck3 = 2;                       // 보기옵션 설정 Sample 또는 Only Consumption 또는 All

        public static bool WorksheetOpCheck1 = true;                // 보기옵션 설정 Cancel 
        public static bool WorksheetOpCheck2 = true;                // 보기옵션 설정 Reject
        public static bool WorksheetOpCheck3 = true;                // 보기옵션 설정 Confirm Office
        public static bool WorksheetOpCheck4 = true;                // 보기옵션 설정 Confirm TD
        public static bool WorksheetOpCheck5 = true;                // 보기옵션 설정 Confirm Admin

        public static bool PatternOpCheck1 = true;                  // 보기옵션 설정 Cancel
        public static bool PatternOpCheck2 = true;                  // 보기옵션 설정 Reject
        public static bool PatternOpCheck3 = true;                  // 보기옵션 설정 Confirm Cad
        public static bool PatternOpCheck4 = true;                  // 보기옵션 설정 Complete
        public static bool PatternOpCheck5 = true;                  // 보기옵션 설정 Confirm TD
        public static int PatternOpCheck6 = 2;                      // 보기옵션 설정 Viewed Team 

        public static int NewOrderBuyerIdx = 0;                     // 신규 오더입력시 바이어 프리셋 번호
        public static int PeriodTime = 0;                           // 작업지서서, 패턴요척 화면 자동갱신 주기 (0: 갱신안함)

        // 식별코드: 주로 조회를 위해 컨트롤러 호출시 key로 사용한다
        public enum KeyName {
            None, DeptIdx, CustIdx, CustAll, Brand, Pono, Styleno, Fileno, StatusShipment, FabricBody, VsslAir, Destination, SewThread, 
            EmbelishId1, EmbelishId2, ShipTerm, IsPrinting, Codes, Status, SizeGroup, User, Vendor, Size, CustAllExceptBuyer, 
            OrderIdx, OperationIdx, WorkOrderIdx, TicketDate, WorkStatus, StartDate, Composition, BurnCount, YarnType, Contents, IsUse,
            Remark, BuyerIdx, ColorIdx, FabricType, Lotno, FabricIdx, RackNo, Floorno, RackPos, PosX, PosY, Wash, InIdx, Handler, 
            RequestDate, CompleteDate, AllUser, TDUser, CADUser, TrimCode, Period, 
            OptionCheck1, OptionCheck2, OptionCheck3, OptionCheck4, OptionCheck5, OptionCheck6
        };      

        // 클래스명에 매칭된 ID로 해당 분류의 거래처를 검색하기 위한 용도 (code테이블값 변경시 반드시 일치시켜줘야함) 
        public static Dictionary<string, int> DictionaryCodeClass = new Dictionary<string, int>()
        {
            {"Buyer", 24},
            {"Embllishment", 25}, 
            {"Knit", 26}, 
            {"Yarn", 27}, 
            {"Bank", 28}, 
            {"Trim", 29}, 
            {"Sew", 30}, 
            {"Print", 31}, 
            {"Dye", 32}, 
            {"Wash", 33}
        };

        public static Dictionary<int, string> DicOrderStatus = new Dictionary<int, string>()
        {
            {0, ""},
            {1, "Progress"},
            {2, "Canceled"},
            {3, "Shipped"}
        };

        public static Dictionary<int, string> DicTrimStatus = new Dictionary<int, string>()
        {
            {0, ""},
            {1, "Pending"},
            {2, "Ready"}
        };

        public static Dictionary<int, string> DicWorksheetStatus = new Dictionary<int, string>()
        {
            {0, ""},
            {1, "Office Sent"},
            {2, "Dev. Confirmed"},
        };

        public static Dictionary<int, string> DicWorkOrderStatus = new Dictionary<int, string>()
        {
            {0, ""},
            {1, "New Work"},                        // 신규작업
            {2, "Printed Ticket"},                  // 작업지시를 위한 티켓발행
            {3, "Completed"},                       // 작업완료
            {4, "Canceled"},                        // 작업취소 
            {5, "Confirmed(CAD)"},                  // 작업확인 (CAD)
            {6, "Rejected(CAD)"},                   // 작업반려 (CAD)
            {7, "Confirmed(TD)"},                   // 작업확인 (TD)
            {8, "Rejected(TD)"},                    // 작업반려 (TD)
            {10, "Confirmed(Office)"},              // 작업확인 (개발총괄)
            {11, "Rejected(Office)"},               // 작업반려 (개발총괄)
            {12, "Confirmed(Admin)"},               // 작업확인 (사무실)
            {13, "Rejected(Admin)"},                // 작업반려 (사무실)
            {14, "Wrong Input"},                    // 입력실수
            {15, "Rejected(Modifiable)"},           // 검사반려 (수정가능)
            {16, "Rejected(Re-Order)"}              // 검사반려 (재작업오더)
        };

        public static Dictionary<int, string> DicFabricInStatus = new Dictionary<int, string>()
        {
            {0, ""},
            {1, "1: Stand by"},                     // 대기상태
            {2, "2: Inbound(Customer)"},            // 입고(생산처)
            {3, "3: Inbound(Remain)"},              // 입고(사용가능 잔량회수)
            {4, "4: Buying"},                       // 입고(구매처)
            {9, "9: Malicious Inventory"},         // 악성재고
            {10, "10: Factory Inventory"}          // 공장용보관
        };

        public static Dictionary<int, string> DicFabricOutStatus = new Dictionary<int, string>()
        {
            {0, ""},
            {3, "3: Inbound(Remain)"},              // 입고(사용가능 잔량회수)
            {5, "5: Outbound(Normal)"},             // 정상출고
            {6, "6: Outbound(Bad)"},                // 출고(원단불량)
            {7, "7: Sales"},                        // 판매
            {8, "8: Disposal"},                     // 폐기
            {11, "11: Inventory adjustment"},       // 재고조정
        };

        public static List<string> ListWorkID = new List<string>();     // 티켓발행용 WorkID 리스트 임시보관
        public static string WorkOperation = "";                        // 공정구분

        #endregion
    }
}
