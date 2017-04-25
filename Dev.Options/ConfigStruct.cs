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
        
    }

}
