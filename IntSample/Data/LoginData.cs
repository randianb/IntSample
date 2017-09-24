using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SampleApp.Data
{
    public class LoginData
    {
        #region Members 

        private static string _strConn = Int.Members.IntSampleConnectionString;
        private static SqlDataAdapter _adapter = null;
        private static SqlConnection _conn = null;
        private static SqlCommand _cmd = null;
        private static DataSet _ds = null;
        private static DataRow _dr = null;
        private static int _rtn = 0;

        #endregion

        #region Queries 

        /// <summary>
        /// 권한 사용자 검색
        /// </summary>
        /// <param name="strUserid">검색 아이디</param>
        /// <param name="strPasswd">비밀번호</param>
        /// <returns></returns>
        public static DataRow TryUserLogin(string strUserid, string strPasswd)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = @"select useridx, deptidx, costcenteridx, Email, Phone, 
                               (select reportno from dept where deptidx=users.deptidx)reportno, 
                                (select isnull(IsUse,0) from dept where deptidx=users.deptidx) useDept, 
                                (select isnull(IsUse,0) from Costcenter where CostcenterIdx=users.CostcenterIdx) useCenter, 
                                isnull(GroupIdx,'')GroupIdx, isnull(IsLeader,0)IsLeader, 
                                isnull(Position,0)Position,   isnull(ExceptionGroup,0)ExceptionGroup, isnull(Nationality,0)Nationality, 
                                (select contents from Codes where Idx=users.Position and Classification='App Position') PositionNm, 
                                (select contents from Codes where Idx=users.Nationality and Classification='Nationality') NationalityNm

                                from users 
                                where userid = @userid and passwd=@passwd and isnull(IsUse,0)=1";
                _cmd.CommandTimeout = 5;
                _cmd.CommandType = CommandType.Text; 
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@userid", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@userid"].Value = strUserid;

                _cmd.Parameters.Add("@passwd", SqlDbType.NVarChar, 15);
                _cmd.Parameters["@passwd"].Value = strPasswd; 

                _adapter.SelectCommand = _cmd;
                _adapter.Fill(_ds);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                _conn.Close();
            }

            // dataset 확인 및 결과 datarow 반환
            if ((_ds != null) && (_ds.Tables[0].Rows.Count > 0))
            {
                _dr = _ds.Tables[0].Rows[0];
                return _dr;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 사용자 비밀번호 변경
        /// </summary>
        /// <param name="useridx">사용자 고유번호</param>
        /// <param name="strPasswd">변경 비밀번호</param>
        /// <returns></returns>
        public static bool ModifyPassword(int useridx, string strPasswd)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                
                _cmd.CommandText = "update users set passwd=@passwd where useridx = @useridx";
                _cmd.CommandTimeout = 15;
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@useridx", SqlDbType.Int, 8);
                _cmd.Parameters["@useridx"].Value = useridx;

                _cmd.Parameters.Add("@passwd", SqlDbType.NVarChar, 15);
                _cmd.Parameters["@passwd"].Value = strPasswd;

                _rtn = _cmd.ExecuteNonQuery(); 

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                _conn.Close();
            }
            
            if (_rtn > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 업데이트 정보 검색
        /// </summary>
        /// <param name="FileName">파일명</param>
        /// <param name="Version">버전번호</param>
        /// <param name="AppliedDate">적용일</param>
        /// <returns></returns>
        public static DataRow FindUpdateInfo(int Package, string FileName, double Version, DateTime AppliedDate)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_FindUpdateInfo";
                _cmd.CommandTimeout = 40;
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Package", SqlDbType.Int, 4);
                _cmd.Parameters["@Package"].Value = Package;

                _cmd.Parameters.Add("@FileName", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@FileName"].Value = FileName;

                _cmd.Parameters.Add("@Version", SqlDbType.Float, 8);
                _cmd.Parameters["@Version"].Value = Version;

                _cmd.Parameters.Add("@AppliedDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@AppliedDate"].Value = AppliedDate;

                _adapter.SelectCommand = _cmd;
                _adapter.Fill(_ds);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                _conn.Close();
            }

            // dataset 확인 및 결과 datarow 반환
            if ((_ds != null) && (_ds.Tables[0].Rows.Count > 0))
            {
                _dr = _ds.Tables[0].Rows[0];
                return _dr;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 사용자 정보 변경 (메일, 전화번호) 
        /// </summary>
        /// <param name="useridx">사용자 고유번호</param>
        /// <param name="strPasswd">변경 비밀번호</param>
        /// <returns></returns>
        public static bool ModifyUserInfo(int useridx, string Email, string Phone)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();

                _cmd.CommandText = "update users set Email=@Email, Phone=@Phone where useridx = @useridx";
                _cmd.CommandTimeout = 15;
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@useridx", SqlDbType.Int, 8);
                _cmd.Parameters["@useridx"].Value = useridx;

                _cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Email"].Value = Email;

                _cmd.Parameters.Add("@Phone", SqlDbType.NVarChar, 20);
                _cmd.Parameters["@Phone"].Value = Phone;

                _rtn = _cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
            finally
            {
                _conn.Close();
            }

            if (_rtn > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion
    }
}
