using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Options
{
    /// <summary>
    /// 시작메뉴 옵션 설정변수 (/conf/conf.json에 저장) 
    /// </summary>
    public class ConfigStruct
    {
        public bool enablePaging { get; set; }      // 그리드뷰 페이징 여부
        public bool enableSearchRow { get; set; }   // 그리드뷰 상단에 검색옵션 여부
        public int NewOrderBuyerIdx { get; set; }   // 오더 신규 입력시 자동템플릿 입력을 위한 기본 바이어 설정 
        public bool SetHandler { get; set; }   // 오더화면 > 기본으로 유저명을 Handler로 설정  
        public bool OrderOpCheck1 { get; set; }
        public bool OrderOpCheck2 { get; set; }
        public int OrderOpCheck3 { get; set; }
        public bool WorksheetOpCheck1 { get; set; }
        public bool WorksheetOpCheck2 { get; set; }
        public bool WorksheetOpCheck3 { get; set; }
        public bool WorksheetOpCheck4 { get; set; }
        public bool WorksheetOpCheck5 { get; set; }
        public bool PatternOpCheck1 { get; set; }
        public bool PatternOpCheck2 { get; set; }
        public bool PatternOpCheck3 { get; set; }
        public bool PatternOpCheck4 { get; set; }
        public bool PatternOpCheck5 { get; set; }
        public bool PatternOpCheck6 { get; set; }
    }

}
