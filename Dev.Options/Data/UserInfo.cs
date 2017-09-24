using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dev.Options
{
    /// <summary>
    /// 모든 윈도우에서 로그인 사용자 정보에 접근하기 위한 용도 
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 변수설정
        /// </summary>
        private static int _idx;                // 사용자 고유번호
        private static string _username;        // 사용자 이름
        private static string _password;        // 비밀번호
        private static int _deptIdx;            // 부서번호
        private static int _reportNo;           // 리포트 번호 (영업부 구분용) 
        private static int _centerIdx;          // 비용센터번호 
        private static int _groupIdx;           // 그룹번호 (같은 영업부내 다른 그룹) 

        private static int _isLeader;           // 팀장권한 부여구분
        private static int _position;           // 직급코드
        private static string _positionNm;         // 직급명
        private static int _exceptionGroup;     // 예외그룹 (운용시 직급/리더구분도 아닌 섞여있거나 하위직급자에 대한 상위권한을 부여해야하는 경우)
        private static int _nationality;        // 국적코드
        private static string _nationalityNm;      // 국적명
        private static string _email;      // 이메일
        private static string _phone;      // 전화번호

        private static DataTable _dtAuthority;  // 사용자권한 테이블
        
        /// <summary>
        /// 프로퍼티 
        /// </summary>
        public static int Idx
        {
            get { return _idx; }
            set { _idx = value; }
        }
        public static string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public static string Password
        {
            get { return _password; }
            set { _password = value; }
        }
        public static int DeptIdx
        {
            get { return _deptIdx; }
            set { _deptIdx = value; }
        }
        public static int ReportNo
        {
            get { return _reportNo; }
            set { _reportNo = value; }
        }
        public static int CenterIdx
        {
            get { return _centerIdx; }
            set { _centerIdx = value; }
        }

        public static int GroupIdx
        {
            get { return _groupIdx; }
            set { _groupIdx = value; }
        }
        public static int IsLeader
        {
            get { return _isLeader; }
            set { _isLeader = value; }
        }
        public static DataTable DtAuthority
        {
            get { return _dtAuthority; }
            set { _dtAuthority = value; }
        }
        public static int Position
        {
            get { return _position; }
            set { _position = value; }
        }
        public static string PositionNm
        {
            get { return _positionNm; }
            set { _positionNm = value; }
        }
        public static int ExceptionGroup
        {
            get { return _exceptionGroup; }
            set { _exceptionGroup = value; }
        }
        public static int Nationality
        {
            get { return _nationality; }
            set { _nationality = value; }
        }
        public static string NationalityNm
        {
            get { return _nationalityNm; }
            set { _nationalityNm = value; }
        }
        public static string Email
        {
            get { return _email; }
            set { _email = value; }
        }
        public static string Phone
        {
            get { return _phone; }
            set { _phone = value; }
        }
    }

}
