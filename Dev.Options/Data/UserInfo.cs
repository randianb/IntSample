﻿using System;
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
        private static int _idx;            // 사용자 고유번호
        private static string _username;    // 사용자 이름
        private static string _password;    // 비밀번호
        private static int _deptIdx;        // 부서번호
        private static int _reportNo;       // 리포트 번호 (영업부 구분용) 
        private static int _centerIdx;      // 비용센터번호 
        private static int _groupIdx;       // 그룹번호 (같은 영업부내 다른 그룹) 
        private static int _isLeader;       // 팀장권한 부여구분
        private static DataTable _dtAuthority;  // 사용자권한 테이블

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
    }

}
