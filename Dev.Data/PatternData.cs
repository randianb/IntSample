using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Dev.Data
{
    public class PatternData
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
        /// Insert
        /// </summary>
        public static DataRow Insert(int OrderIdx, string WorkOrderIdx, int OrdSizeIdx, DateTime TechpackDate, DateTime RequestedDate,
            int Requested, string Comments, 
            string Attached1, string Attached2, string Attached3, string Attached4, string Attached5,
            string Attached6, string Attached7, string Attached8, string Attached9,
            string AttachedUrl1, string AttachedUrl2, string AttachedUrl3, string AttachedUrl4, string AttachedUrl5,
            string AttachedUrl6, string AttachedUrl7, string AttachedUrl8, string AttachedUrl9,
            int Handler,
            int IsPattern, int IsConsum,
            int OrdSizeIdx2, int OrdSizeIdx3, int OrdSizeIdx4, int OrdSizeIdx5, int OrdSizeIdx6, int OrdSizeIdx7, int OrdSizeIdx8, 
            int Confirmed, int Received
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Pattern_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

                _cmd.Parameters.Add("@OrdSizeIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx"].Value = OrdSizeIdx;
                
                _cmd.Parameters.Add("@TechpackDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@TechpackDate"].Value = TechpackDate;

                _cmd.Parameters.Add("@RequestedDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@RequestedDate"].Value = RequestedDate;

                _cmd.Parameters.Add("@Requested", SqlDbType.Int, 4);
                _cmd.Parameters["@Requested"].Value = Requested;
                
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

                _cmd.Parameters.Add("@IsPattern", SqlDbType.Int, 4);
                _cmd.Parameters["@IsPattern"].Value = IsPattern;

                _cmd.Parameters.Add("@IsConsum", SqlDbType.Int, 4);
                _cmd.Parameters["@IsConsum"].Value = IsConsum;

                _cmd.Parameters.Add("@OrdSizeIdx2", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx2"].Value = OrdSizeIdx2;

                _cmd.Parameters.Add("@OrdSizeIdx3", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx3"].Value = OrdSizeIdx3;

                _cmd.Parameters.Add("@OrdSizeIdx4", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx4"].Value = OrdSizeIdx4;

                _cmd.Parameters.Add("@OrdSizeIdx5", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx5"].Value = OrdSizeIdx5;

                _cmd.Parameters.Add("@OrdSizeIdx6", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx6"].Value = OrdSizeIdx6;

                _cmd.Parameters.Add("@OrdSizeIdx7", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx7"].Value = OrdSizeIdx7;

                _cmd.Parameters.Add("@OrdSizeIdx8", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx8"].Value = OrdSizeIdx8;

                _cmd.Parameters.Add("@Confirmed", SqlDbType.Int, 4);
                _cmd.Parameters["@Confirmed"].Value = Confirmed;

                _cmd.Parameters.Add("@Received", SqlDbType.Int, 4);
                _cmd.Parameters["@Received"].Value = Received;

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
        /// Update 헤더정보 
        /// </summary>
        public static bool Update(int Idx, DateTime TechpackDate, DateTime RequestedDate,
            int Requested, DateTime ConfirmedDate, int Confirmed,
            DateTime CompletedDate, DateTime SentDate, int Received, string Remarks 
            
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Pattern_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;
                
                _cmd.Parameters.Add("@TechpackDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@TechpackDate"].Value = TechpackDate;

                _cmd.Parameters.Add("@RequestedDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@RequestedDate"].Value = RequestedDate;

                _cmd.Parameters.Add("@Requested", SqlDbType.Int, 4);
                _cmd.Parameters["@Requested"].Value = Requested;

                _cmd.Parameters.Add("@ConfirmedDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@ConfirmedDate"].Value = ConfirmedDate;

                _cmd.Parameters.Add("@Confirmed", SqlDbType.Int, 4);
                _cmd.Parameters["@Confirmed"].Value = Confirmed;

                _cmd.Parameters.Add("@CompletedDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@CompletedDate"].Value = CompletedDate;

                _cmd.Parameters.Add("@SentDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@SentDate"].Value = SentDate;

                _cmd.Parameters.Add("@Received", SqlDbType.Int, 4);
                _cmd.Parameters["@Received"].Value = Received;

                _cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 300);
                _cmd.Parameters["@Remarks"].Value = Remarks;
                
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
        /// Update 첨부파일
        /// </summary>
        public static bool Update(int Idx, string Attached21, string Attached22, string Attached23, string Attached24,
            string AttachedUrl21, string AttachedUrl22, string AttachedUrl23, string AttachedUrl24
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Pattern_Update2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

                _cmd.Parameters.Add("@Attached21", SqlDbType.NVarChar, 150);
                _cmd.Parameters["@Attached21"].Value = Attached21;

                _cmd.Parameters.Add("@Attached22", SqlDbType.NVarChar, 150);
                _cmd.Parameters["@Attached22"].Value = Attached22;

                _cmd.Parameters.Add("@Attached23", SqlDbType.NVarChar, 150);
                _cmd.Parameters["@Attached23"].Value = Attached23;

                _cmd.Parameters.Add("@Attached24", SqlDbType.NVarChar, 150);
                _cmd.Parameters["@Attached24"].Value = Attached24;

                _cmd.Parameters.Add("@AttachedUrl21", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@AttachedUrl21"].Value = AttachedUrl21;

                _cmd.Parameters.Add("@AttachedUrl22", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@AttachedUrl22"].Value = AttachedUrl22;

                _cmd.Parameters.Add("@AttachedUrl23", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@AttachedUrl23"].Value = AttachedUrl23;

                _cmd.Parameters.Add("@AttachedUrl24", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@AttachedUrl24"].Value = AttachedUrl24;

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
        /// 영업부 패턴 요청 취소 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="Rejected"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool RejectTeam(int idx, int Rejected, int OrderIdx, string WorkOrderIdx, string Comment)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update pattern set RejectedDate=dbo.GetLocalDate(default), Rejected = @Rejected, Comments = @CommentCad where idx = @Idx 
                                     
                                     update workorder set status=4 where OrderIdx = @OrderIdx and WorkOrderIdx = @WorkOrderIdx
                                    ";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@Rejected", SqlDbType.Int, 4);
                _cmd.Parameters["@Rejected"].Value = Rejected;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@CommentCad", SqlDbType.NText);
                _cmd.Parameters["@CommentCad"].Value = Comment;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

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
        /// pattern confirm by CAD person 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="confirmed"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool ConfirmCAD(int idx, int confirmed, int OrderIdx, string WorkOrderIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update pattern set ConfirmedDate=dbo.GetLocalDate(default), Confirmed = @confirmed where idx = @Idx 
                                     
                                     update workorder set status=5 where OrderIdx = @OrderIdx and WorkOrderIdx = @WorkOrderIdx
                                    ";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@Confirmed", SqlDbType.Int, 4);
                _cmd.Parameters["@Confirmed"].Value = confirmed;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

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
        /// pattern Complete by CAD person 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="confirmed"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool CompleteCAD(int idx, int OrderIdx, string WorkOrderIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update pattern set CompletedDate=dbo.GetLocalDate(default) where idx = @Idx 
                                     
                                     update workorder set status=3 where OrderIdx = @OrderIdx and WorkOrderIdx = @WorkOrderIdx
                                    ";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;
                
                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

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
        /// pattern reject by CAD person 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="Rejected"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool RejectCAD(int idx, int Rejected, int OrderIdx, string WorkOrderIdx, string CommentCad)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update pattern set RejectedDate=dbo.GetLocalDate(default), Rejected = @Rejected, CommentCad = @CommentCad where idx = @Idx 
                                     
                                     update workorder set status=6 where OrderIdx = @OrderIdx and WorkOrderIdx = @WorkOrderIdx
                                    ";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@Rejected", SqlDbType.Int, 4);
                _cmd.Parameters["@Rejected"].Value = Rejected;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@CommentCad", SqlDbType.NText);
                _cmd.Parameters["@CommentCad"].Value = CommentCad;
                
                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

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
        /// pattern confirm by TD person 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="confirmed"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool ConfirmTD(int idx, int confirmed, int OrderIdx, string WorkOrderIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update pattern set SentDate=dbo.GetLocalDate(default), Received = @confirmed where idx = @Idx 
                                     
                                     update workorder set status=7 where OrderIdx = @OrderIdx and WorkOrderIdx = @WorkOrderIdx
                                    ";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@Confirmed", SqlDbType.Int, 4);
                _cmd.Parameters["@Confirmed"].Value = confirmed;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

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
        /// pattern reject by TD person 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="Rejected"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool RejectTD(int idx, int Rejected, int Status, int OrderIdx, string WorkOrderIdx, string CommentCad)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update pattern set RejectedDate=dbo.GetLocalDate(default), Rejected = @Rejected, CommentCad = @CommentCad where idx = @Idx 
                                     
                                     update workorder set status=@Status where OrderIdx = @OrderIdx and WorkOrderIdx = @WorkOrderIdx
                                    ";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@Rejected", SqlDbType.Int, 4);
                _cmd.Parameters["@Rejected"].Value = Rejected;

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = Status;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@CommentCad", SqlDbType.NText);
                _cmd.Parameters["@CommentCad"].Value = CommentCad;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

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
        /// pattern view by team 
        /// </summary>
        /// <param name="idx"></param>
        /// <param name="confirmed"></param>
        /// <param name="OrderIdx"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <returns></returns>
        public static bool ViewTeam(int idx, int Viewed)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update pattern set ViewedDate=dbo.GetLocalDate(default), Viewed = @Viewed where idx = @Idx";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@Viewed", SqlDbType.Int, 4);
                _cmd.Parameters["@Viewed"].Value = Viewed;
                
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

                _cmd.CommandText = "up_Pattern_Get";
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
        /// Getlist: 전체 목록조회
        /// </summary>
        //public static DataSet Getlist()
        //{
        //    try
        //    {
        //        _conn = new SqlConnection(_strConn);
        //        _cmd = new SqlCommand();
        //        _conn.Open();
        //        _ds = new DataSet();
        //        _adapter = new SqlDataAdapter();

        //        _cmd.CommandText = "up_Orders_List";
        //        _cmd.CommandType = CommandType.StoredProcedure;
        //        _cmd.Connection = _conn;

        //        _adapter.SelectCommand = _cmd;
        //        _adapter.Fill(_ds);

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message.ToString());
        //    }
        //    finally
        //    {
        //        _conn.Close();
        //    }

        //    // dataset 확인 및 결과 datarow 반환
        //    if ((_ds != null) && (_ds.Tables[0].Rows.Count > 0))
        //    {
        //        return _ds;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        
        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey, string fileno, string style)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Pattern_List2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@DeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@DeptIdx"].Value = SearchKey[CommonValues.KeyName.DeptIdx];
                
                _cmd.Parameters.Add("@CustIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@CustIdx"].Value = SearchKey[CommonValues.KeyName.CustIdx];

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.Status];

                _cmd.Parameters.Add("@SizeIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx"].Value = SearchKey[CommonValues.KeyName.Size];
                
                _cmd.Parameters.Add("@Fileno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Fileno"].Value = fileno;

                _cmd.Parameters.Add("@Styleno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Styleno"].Value = style;


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

        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey, string fileno, string style, int UserIdx)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Pattern_List4";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@CustIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@CustIdx"].Value = SearchKey[CommonValues.KeyName.CustIdx];

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.Status];

                _cmd.Parameters.Add("@SizeIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx"].Value = SearchKey[CommonValues.KeyName.Size];

                _cmd.Parameters.Add("@Fileno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Fileno"].Value = fileno;

                _cmd.Parameters.Add("@Styleno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Styleno"].Value = style;

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

        public static DataSet Print(Dictionary<CommonValues.KeyName, string> SearchString, Dictionary<CommonValues.KeyName, int> SearchKey, 
            DateTime dtFromDate, DateTime dtToDate)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Pattern_List3";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@CustIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@CustIdx"].Value = SearchKey[CommonValues.KeyName.CustIdx];

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.Status];

                _cmd.Parameters.Add("@SizeIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx"].Value = SearchKey[CommonValues.KeyName.Size];

                _cmd.Parameters.Add("@Fileno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Fileno"].Value = SearchString[CommonValues.KeyName.Fileno]; 

                _cmd.Parameters.Add("@Styleno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Styleno"].Value = SearchString[CommonValues.KeyName.Styleno];

                _cmd.Parameters.Add("@DateKind", SqlDbType.Int, 4);
                _cmd.Parameters["@DateKind"].Value = SearchKey[CommonValues.KeyName.Codes];

                _cmd.Parameters.Add("@FromDate", SqlDbType.DateTime, 4);
                _cmd.Parameters["@FromDate"].Value = dtFromDate;

                _cmd.Parameters.Add("@ToDate", SqlDbType.DateTime, 4);
                _cmd.Parameters["@ToDate"].Value = dtToDate;

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

        public static bool Delete(string condition)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Pattern_Delete2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@DeleteIdx", SqlDbType.NVarChar, 1000);
                _cmd.Parameters["@DeleteIdx"].Value = condition;

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
        
        
        #endregion
    }
}
