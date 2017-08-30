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
            int Requested, string Comments, string Attached1, string Attached2, string Attached3, string Attached4, string Attached5,
            string AttachedUrl1, string AttachedUrl2, string AttachedUrl3, string AttachedUrl4, string AttachedUrl5, int Handler
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
                
                _cmd.Parameters.Add("@Comments", SqlDbType.Text);
                _cmd.Parameters["@Comments"].Value = Comments;

                _cmd.Parameters.Add("@Attached1", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Attached1"].Value = Attached1;

                _cmd.Parameters.Add("@Attached2", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Attached2"].Value = Attached2;

                _cmd.Parameters.Add("@Attached3", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Attached3"].Value = Attached3;

                _cmd.Parameters.Add("@Attached4", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Attached4"].Value = Attached4;

                _cmd.Parameters.Add("@Attached5", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Attached5"].Value = Attached5;

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

                _cmd.Parameters.Add("@Attached21", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Attached21"].Value = Attached21;

                _cmd.Parameters.Add("@Attached22", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Attached22"].Value = Attached22;

                _cmd.Parameters.Add("@Attached23", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Attached23"].Value = Attached23;

                _cmd.Parameters.Add("@Attached24", SqlDbType.NVarChar, 50);
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
