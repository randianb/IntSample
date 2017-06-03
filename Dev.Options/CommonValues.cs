using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Options
{
    public static class CommonValues
    {
        #region 전역변수: 향후 json으로 변경필요 (todo) 

        public const int packageNo = 100;                        // 패키지번호 = intsample
        public const string packageName = "intsample";
        public static string verNo = "";                         // 프로그램 버전번호 
        public static int DDL_DefaultItemsCountInDropDown = 20; // DDL 갯수
        public static int DDL_DropDownHeight = 250;             // DDL 높이
        public static int DDL_DropDownWidth = 2;                // DDL 폭
        public static int PropertyValueColumnWidth = 230;       // 프로퍼티 값열 폭 
        public static bool enablePaging = true;                 // 그리드뷰 페이징 
        public static bool enableSearchRow = false;             // 그리드뷰 전체 컬럼 대상으로 검색여부
        public static int NewOrderBuyerIdx = 0;                 // 신규 오더입력시 바이어 프리셋 번호

        // 식별코드: 주로 조회를 위해 컨트롤러 호출시 key로 사용한다
        public enum KeyName {
            None, DeptIdx, CustIdx, CustAll, Brand, Pono, Styleno, Fileno, StatusShipment, FabricBody, VsslAir, Destination, SewThread, 
            EmbelishId1, EmbelishId2, ShipTerm, IsPrinting, Codes, Status, SizeGroup, User, Vendor, Size, 
            OrderIdx, OperationIdx, WorkOrderIdx, TicketDate, WorkStatus, StartDate, Composition, BurnCount, YarnType, Contents, IsUse,
            Remark, BuyerIdx, ColorIdx, FabricType, Lotno, FabricIdx, RackNo, Floorno, RackPos, PosX, PosY, Wash, InIdx, Handler 
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

        public static Dictionary<int, string> DicWorkOrderStatus = new Dictionary<int, string>()
        {
            {0, ""},
            {1, "New Work"},                        // 신규작업
            {2, "Printed Ticket"},                  // 작업지시를 위한 티켓발행
            {3, "Completed"},                       // 작업완료
            {4, "Canceled"}                         // 작업취소 
        };

        public static Dictionary<int, string> DicFabricInStatus = new Dictionary<int, string>()
        {
            {0, ""},
            {1, "1: Stand by"},                     // 대기상태
            {2, "2: Inbound(Customer)"},            // 입고(생산처)
            {3, "3: Inbound(Remain)"},              // 입고(사용가능 잔량회수)
            {4, "4: Buying"},                       // 입고(구매처)
            {9, "9: Inventory(Malicious)"},         // 악성재고
            {10, "10: Inventory(Factory)"}          // 공장용보관
        };

        public static Dictionary<int, string> DicFabricOutStatus = new Dictionary<int, string>()
        {
            {0, ""},
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
