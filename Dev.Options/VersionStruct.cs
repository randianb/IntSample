using System;

namespace Dev.Options
{
    /// <summary>
    /// 프로그램 버전정보 저장
    /// </summary>
    public class VersionStruct
    {
        public int ProgramIdx { get; set; }             // 프로그램 번호
        public float Version { get; set;  }             // 버전
        public DateTime AppliedDate { get; set;  }      // 적용일
        public string UploadUrl { get; set;  }          // 업로드 URL
        public string FileName { get; set;  }           // 파일명
        public int IsUse { get; set;  }                 // 파일사용여부
    }

}
