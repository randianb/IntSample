using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Dev.Data
{
    public class WorksheetData
    {
        #region Members 

        private static string _strConn = Int.Members.IntSampleConnectionString;
        private static SqlDataAdapter _adapter = null;
        private static SqlConnection _conn = null;
        private static SqlCommand _cmd = null;
        private static DataSet _ds = null;
        private static DataTable _dt = null;
        private static DataRow _dr = null;
        private static int _rtn = 0;

        #endregion

        #region Queries

        /// <summary>
        /// Insert
        /// </summary>
        public static DataRow Insert(int OrderIdx, string WorksheetIdx, string Comments, 
            string Attached1, string Attached2, string Attached3, string Attached4, string Attached5, 
            string Attached6, string Attached7, string Attached8, string Attached9,
            string AttachedUrl1, string AttachedUrl2, string AttachedUrl3, string AttachedUrl4, 
            string AttachedUrl5, string AttachedUrl6, string AttachedUrl7, 
            string AttachedUrl8, string AttachedUrl9, int Handler)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Worksheet_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@WorksheetIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorksheetIdx"].Value = WorksheetIdx;

                _cmd.Parameters.Add("@Comments", SqlDbType.NText);
                _cmd.Parameters["@Comments"].Value = Comments;

                _cmd.Parameters.Add("@Attached1", SqlDbType.NVarChar, 150);
                _cmd.Parameters["@Attached1"].Value = Attached1;

                _cmd.Parameters.Add("@Attached2", SqlDbType.NVarChar, 150);
                _cmd.Parameters["@Attached2"].Value = Attached2;

                _cmd.Parameters.Add("@Attached3", SqlDbType.NVarChar, 150);
                _cmd.Parameters["@Attached3"].Value = Attached3;

                _cmd.Parameters.Add("@Attached4", SqlDbType.NVarChar, 150);
                _cmd.Parameters["@Attached4"].Value = Attached4;

                _cmd.Parameters.Add("@Attached5", SqlDbType.NVarChar, 150);
                _cmd.Parameters["@Attached5"].Value = Attached5;

                _cmd.Parameters.Add("@Attached6", SqlDbType.NVarChar, 150);
                _cmd.Parameters["@Attached6"].Value = Attached6;

                _cmd.Parameters.Add("@Attached7", SqlDbType.NVarChar, 150);
                _cmd.Parameters["@Attached7"].Value = Attached7;

                _cmd.Parameters.Add("@Attached8", SqlDbType.NVarChar, 150);
                _cmd.Parameters["@Attached8"].Value = Attached8;

                _cmd.Parameters.Add("@Attached9", SqlDbType.NVarChar, 150);
                _cmd.Parameters["@Attached9"].Value = Attached9;

                _cmd.Parameters.Add("@AttachedUrl1", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@AttachedUrl1"].Value = AttachedUrl1;

                _cmd.Parameters.Add("@AttachedUrl2", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@AttachedUrl2"].Value = AttachedUrl2;

                _cmd.Parameters.Add("@AttachedUrl3", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@AttachedUrl3"].Value = AttachedUrl3;

                _cmd.Parameters.Add("@AttachedUrl4", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@AttachedUrl4"].Value = AttachedUrl4;

                _cmd.Parameters.Add("@AttachedUrl5", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@AttachedUrl5"].Value = AttachedUrl5;

                _cmd.Parameters.Add("@AttachedUrl6", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@AttachedUrl6"].Value = AttachedUrl6;

                _cmd.Parameters.Add("@AttachedUrl7", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@AttachedUrl7"].Value = AttachedUrl7;

                _cmd.Parameters.Add("@AttachedUrl8", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@AttachedUrl8"].Value = AttachedUrl8;

                _cmd.Parameters.Add("@AttachedUrl9", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@AttachedUrl9"].Value = AttachedUrl9;

                _cmd.Parameters.Add("@Handler", SqlDbType.Int, 4);
                _cmd.Parameters["@Handler"].Value = Handler;
                
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
        /// Update
        /// </summary>
        public static bool Update(int Idx, int ConfirmUser
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                if (UserInfo.DeptIdx == 12)
                {
                    _cmd.CommandText = "up_Worksheet_Update2";
                }
                else
                {
                    _cmd.CommandText = "up_Worksheet_Update";
                }
                
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;
                
                _cmd.Parameters.Add("@ConfirmUser", SqlDbType.Int, 4);
                _cmd.Parameters["@ConfirmUser"].Value = ConfirmUser;
                
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

            if (_rtn > 0) return true;
            else return false;
        }

        
        /// <summary>
        /// Get: 해당 ID조회 
        /// </summary>
        public static DataRow Get(int Idx)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Worksheet_Get";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

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
        /// Getlist: 목록조회 (오더메인용) 
        /// </summary>
        public static DataSet Getlist(int OrderIdx, string WorksheetIdx, int Handler, int ConfirmUser, int WorkStatus)
        {
            try
            {
                
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Worksheet_List2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;
                
                _cmd.Parameters.Add("@WorksheetIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorksheetIdx"].Value = WorksheetIdx;

                _cmd.Parameters.Add("@Handler", SqlDbType.Int, 4);
                _cmd.Parameters["@Handler"].Value = Handler;

                _cmd.Parameters.Add("@ConfirmUser", SqlDbType.Int, 4);
                _cmd.Parameters["@ConfirmUser"].Value = ConfirmUser;

                _cmd.Parameters.Add("@WorkStatus", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@WorkStatus"].Value = WorkStatus;

                //_cmd.Parameters.Add("@ConfirmDate", SqlDbType.NVarChar, 10);
                //_cmd.Parameters["@ConfirmDate"].Value = (ConfirmDate == "2000-01-01" ? "" : ConfirmDate);
                
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
                return _ds;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Getlist: 목록조회 (작업지시 메인용) 
        /// TD일땐 useridx로 해당 바이어만 조회하고 나머진 useridx=0로 모두 조회 
        /// </summary>
        public static DataSet Getlist(int DeptIdx, int CustIdx, int Handler, int WorkStatus, string Fileno, string Styleno, string WorksheetIdx, int UserIdx)
        {
            try
            {

                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Worksheet_List3";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@DeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@DeptIdx"].Value = DeptIdx;

                _cmd.Parameters.Add("@CustIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@CustIdx"].Value = CustIdx;

                _cmd.Parameters.Add("@Handler", SqlDbType.Int, 4);
                _cmd.Parameters["@Handler"].Value = Handler;

                _cmd.Parameters.Add("@WorkStatus", SqlDbType.Int, 4);
                _cmd.Parameters["@WorkStatus"].Value = WorkStatus;

                _cmd.Parameters.Add("@Fileno", SqlDbType.NVarChar, 11);
                _cmd.Parameters["@Fileno"].Value = Fileno;

                _cmd.Parameters.Add("@Styleno", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Styleno"].Value = Styleno;

                _cmd.Parameters.Add("@WorksheetIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorksheetIdx"].Value = WorksheetIdx;

                _cmd.Parameters.Add("@UserIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@UserIdx"].Value = UserIdx;

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
                return _ds;
            }
            else
            {
                return null;
            }
        }

        public static bool Delete(string DeleteIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Worksheet_Delete2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@DeleteIdx", SqlDbType.NVarChar, 1000);
                _cmd.Parameters["@DeleteIdx"].Value = DeleteIdx;

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
                return true;
            else
                return false;
        }

        /// <summary>
        /// 영업부 작업지시서 취소 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="Rejected"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool CancelWorksheetTeam(int idx, int Rejected, string Comment)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update Worksheet set RejectDate=dbo.GetLocalDate(default), Rejected=@Rejected, Comments=@CommentCad, 
                                     status=4 where idx=@Idx";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@Rejected", SqlDbType.Int, 4);
                _cmd.Parameters["@Rejected"].Value = Rejected;
                
                _cmd.Parameters.Add("@CommentCad", SqlDbType.NText);
                _cmd.Parameters["@CommentCad"].Value = Comment;

                _rtn = _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _conn.Close();
            }
            if (_rtn > 0) return true;
            else return false;
        }

        /// <summary>
        /// 사무실 작업지시서 승인
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="confirmed"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool ConfirmOffice(int idx, int confirmed, string CommentTD)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update Worksheet set ConfirmDate=dbo.GetLocalDate(default), ConfirmUser=@confirmed, CommentTD=@CommentTD, 
                                     status=10 where idx=@Idx
                                    ";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@Confirmed", SqlDbType.Int, 4);
                _cmd.Parameters["@Confirmed"].Value = confirmed;

                _cmd.Parameters.Add("@CommentTD", SqlDbType.NText);
                _cmd.Parameters["@CommentTD"].Value = CommentTD;

                _rtn = _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _conn.Close();
            }
            if (_rtn > 0) return true;
            else return false;
        }

        /// <summary>
        /// 사무실 작업지시서 취소 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="Rejected"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool RejectOffice(int idx, int Rejected, string CommentTD)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update Worksheet set RejectDate=dbo.GetLocalDate(default), Rejected=@Rejected, CommentTD=@CommentTD,
                                     status=11 where idx=@Idx";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@Rejected", SqlDbType.Int, 4);
                _cmd.Parameters["@Rejected"].Value = Rejected;

                _cmd.Parameters.Add("@CommentTD", SqlDbType.NText);
                _cmd.Parameters["@CommentTD"].Value = CommentTD;

                _rtn = _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _conn.Close();
            }
            if (_rtn > 0) return true;
            else return false;
        }

        /// <summary>
        /// TD 작업지시서 승인
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="confirmed"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool ConfirmTD(int idx, int confirmed, string CommentTD)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update Worksheet set ConfirmDateTD=dbo.GetLocalDate(default), ConfirmUserTD=@confirmed, CommentTD=@CommentTD, status=7 where idx=@Idx";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@Confirmed", SqlDbType.Int, 4);
                _cmd.Parameters["@Confirmed"].Value = confirmed;

                _cmd.Parameters.Add("@CommentTD", SqlDbType.NText);
                _cmd.Parameters["@CommentTD"].Value = CommentTD;

                _rtn = _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _conn.Close();
            }
            if (_rtn > 0) return true;
            else return false;
        }

        /// <summary>
        /// TD 작업지시서 취소 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="Rejected"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool RejectTD(int idx, int Rejected, string CommentTD)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update Worksheet set RejectDate=dbo.GetLocalDate(default), Rejected=@Rejected, CommentTD=@CommentTD,
                                     status=8 where idx=@Idx";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@Rejected", SqlDbType.Int, 4);
                _cmd.Parameters["@Rejected"].Value = Rejected;

                _cmd.Parameters.Add("@CommentTD", SqlDbType.NText);
                _cmd.Parameters["@CommentTD"].Value = CommentTD;

                _rtn = _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _conn.Close();
            }
            if (_rtn > 0) return true;
            else return false;
        }

        /// <summary>
        /// 개발실 총괄 작업지시서 승인
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="confirmed"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool ConfirmAdmin(int idx, int confirmed, string CommentTD)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update Worksheet set ConfirmDateLast=dbo.GetLocalDate(default), ConfirmUserLast=@confirmed, CommentTD=@CommentTD, 
                                     status=12 where idx=@Idx
                                    ";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@Confirmed", SqlDbType.Int, 4);
                _cmd.Parameters["@Confirmed"].Value = confirmed;

                _cmd.Parameters.Add("@CommentTD", SqlDbType.NText);
                _cmd.Parameters["@CommentTD"].Value = CommentTD;

                _rtn = _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _conn.Close();
            }
            if (_rtn > 0) return true;
            else return false;
        }

        /// <summary>
        /// 개발실 총괄 작업지시서 취소 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="Rejected"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool RejectAdmin(int idx, int Rejected, string CommentTD)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update Worksheet set RejectDate=dbo.GetLocalDate(default), Rejected=@Rejected, CommentTD=@CommentTD,
                                     status=13 where idx=@Idx";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@Rejected", SqlDbType.Int, 4);
                _cmd.Parameters["@Rejected"].Value = Rejected;

                _cmd.Parameters.Add("@CommentTD", SqlDbType.NText);
                _cmd.Parameters["@CommentTD"].Value = CommentTD;

                _rtn = _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _conn.Close();
            }
            if (_rtn > 0) return true;
            else return false;
        }

        #endregion
    }
}
