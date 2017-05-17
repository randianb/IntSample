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
        public static int DDL_DefaultItemsCountInDropDown = 20; // DDL 갯수
        public static int DDL_DropDownHeight = 250;             // DDL 높이
        public static int DDL_DropDownWidth = 2;             // DDL 폭
        public static int PropertyValueColumnWidth = 230;       // 프로퍼티 값열 폭 
        public static bool enablePaging = true;                 // 그리드뷰 페이징 
        public static bool enableSearchRow = false;             // 그리드뷰 전체 컬럼 대상으로 검색여부

        // 식별코드: 주로 조회를 위해 컨트롤러 호출시 key로 사용한다
        public enum KeyName {
            None, DeptIdx, CustIdx, CustAll, Brand, Pono, Styleno, Fileno, StatusShipment, FabricBody, VsslAir, Destination, SewThread, 
            EmbelishId1, EmbelishId2, ShipTerm, IsPrinting, Codes, Status, SizeGroup, User, Vendor, Size, 
            OrderIdx, OperationIdx, WorkOrderIdx, TicketDate, WorkStatus, StartDate, Composition, BurnCount, YarnType, Contents, IsUse,
            Remark, BuyerIdx, ColorIdx, FabricType, Lotno, FabricIdx, RackNo, Floorno, RackPos, PosX, PosY, Wash
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
            {1, "New Work"},
            {2, "Printed Ticket"},
            {3, "Completed"},
            {4, "Canceled"}
        };

        public static Dictionary<int, string> DicFabricInStatus = new Dictionary<int, string>()
        {
            {0, ""},
            {1, "1: Stand by"},
            {2, "2: Inbound(Customer)"},
            {3, "3: Inbound(Remain)"},
            {9, "9: Inventory(Malicious)"},
            {10, "10: Inventory(Factory)"}
        };
        
        public static List<string> ListWorkID = new List<string>();
        public static string WorkOperation = ""; 

        #endregion
    }
}
