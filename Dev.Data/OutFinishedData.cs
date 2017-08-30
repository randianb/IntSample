using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Dev.Data
{
    public class OutFinishedData
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
        public static DataRow Insert(string WorkOrderIdx
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_OutFinished_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 13);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;
                
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
        public static bool Update(int Idx, int Out1, int Out2, string Delivered, int Received1, int Received2, DateTime OutDate, DateTime RcvdDate
                , string Remarks
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_OutFinished_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

                _cmd.Parameters.Add("@Out1", SqlDbType.Int, 4);
                _cmd.Parameters["@Out1"].Value = Out1;

                _cmd.Parameters.Add("@Out2", SqlDbType.Int, 4);
                _cmd.Parameters["@Out2"].Value = Out2;

                _cmd.Parameters.Add("@Delivered", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@Delivered"].Value = Delivered;

                _cmd.Parameters.Add("@Received1", SqlDbType.Int, 4);
                _cmd.Parameters["@Received1"].Value = Received1;

                _cmd.Parameters.Add("@Received2", SqlDbType.Int, 4);
                _cmd.Parameters["@Received2"].Value = Received2;

                _cmd.Parameters.Add("@OutDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@OutDate"].Value = OutDate;

                _cmd.Parameters.Add("@RcvdDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@RcvdDate"].Value = RcvdDate;
                
                _cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 50);
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

                _cmd.CommandText = "up_OutFinished_Get";
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
        
        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey, 
                                      Dictionary<CommonValues.KeyName, string> SearchString, 
                                      string OutDateFrom, string OutDateTo,
                                      string RcvdDateFrom, string RcvdDateTo)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_OutFinished_List";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Buyer", SqlDbType.Int, 4);
                _cmd.Parameters["@Buyer"].Value = SearchKey[CommonValues.KeyName.BuyerIdx];

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@OrderIdx"].Value = SearchString[CommonValues.KeyName.OrderIdx];

                _cmd.Parameters.Add("@Styleno", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@Styleno"].Value = SearchString[CommonValues.KeyName.Styleno];

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = SearchString[CommonValues.KeyName.WorkOrderIdx];

                _cmd.Parameters.Add("@OrdColorIdx", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@OrdColorIdx"].Value = SearchString[CommonValues.KeyName.ColorIdx];

                _cmd.Parameters.Add("@OrdSizeIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx"].Value = SearchKey[CommonValues.KeyName.Size];

                _cmd.Parameters.Add("@Out", SqlDbType.Int, 4);
                _cmd.Parameters["@Out"].Value = SearchKey[CommonValues.KeyName.CustIdx];

                _cmd.Parameters.Add("@Received", SqlDbType.Int, 4);
                _cmd.Parameters["@Received"].Value = SearchKey[CommonValues.KeyName.User];
                
                _cmd.Parameters.Add("@Delivered", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@Delivered"].Value = SearchString[CommonValues.KeyName.Remark];

                _cmd.Parameters.Add("@OutDateFrom", SqlDbType.DateTime, 8);
                _cmd.Parameters["@OutDateFrom"].Value = OutDateFrom;

                _cmd.Parameters.Add("@OutDateTo", SqlDbType.DateTime, 8);
                _cmd.Parameters["@OutDateTo"].Value = OutDateTo;

                _cmd.Parameters.Add("@RcvdDateFrom", SqlDbType.DateTime, 8);
                _cmd.Parameters["@RcvdDateFrom"].Value = RcvdDateFrom;

                _cmd.Parameters.Add("@RcvdDateTo", SqlDbType.DateTime, 8);
                _cmd.Parameters["@RcvdDateTo"].Value = RcvdDateTo;

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.WorkStatus];

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
        /// Getlist: 전체 목록조회
        /// </summary>

        public static DataSet GetlistD(Dictionary<CommonValues.KeyName, int> SearchKey,
                                      Dictionary<CommonValues.KeyName, string> SearchString,
                                      string OutDateFrom, string OutDateTo,
                                      string RcvdDateFrom, string RcvdDateTo)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_OutFinished_List2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Buyer", SqlDbType.Int, 4);
                _cmd.Parameters["@Buyer"].Value = SearchKey[CommonValues.KeyName.BuyerIdx];

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@OrderIdx"].Value = SearchString[CommonValues.KeyName.OrderIdx];

                _cmd.Parameters.Add("@Styleno", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@Styleno"].Value = SearchString[CommonValues.KeyName.Styleno];

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = SearchString[CommonValues.KeyName.WorkOrderIdx];

                _cmd.Parameters.Add("@OrdColorIdx", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@OrdColorIdx"].Value = SearchString[CommonValues.KeyName.ColorIdx];

                _cmd.Parameters.Add("@OrdSizeIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx"].Value = SearchKey[CommonValues.KeyName.Size];

                _cmd.Parameters.Add("@Out", SqlDbType.Int, 4);
                _cmd.Parameters["@Out"].Value = SearchKey[CommonValues.KeyName.CustIdx];

                _cmd.Parameters.Add("@Received", SqlDbType.Int, 4);
                _cmd.Parameters["@Received"].Value = SearchKey[CommonValues.KeyName.User];

                _cmd.Parameters.Add("@Delivered", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@Delivered"].Value = SearchString[CommonValues.KeyName.Remark];

                _cmd.Parameters.Add("@OutDateFrom", SqlDbType.DateTime, 8);
                _cmd.Parameters["@OutDateFrom"].Value = OutDateFrom;

                _cmd.Parameters.Add("@OutDateTo", SqlDbType.DateTime, 8);
                _cmd.Parameters["@OutDateTo"].Value = OutDateTo;

                _cmd.Parameters.Add("@RcvdDateFrom", SqlDbType.DateTime, 8);
                _cmd.Parameters["@RcvdDateFrom"].Value = RcvdDateFrom;

                _cmd.Parameters.Add("@RcvdDateTo", SqlDbType.DateTime, 8);
                _cmd.Parameters["@RcvdDateTo"].Value = RcvdDateTo;

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.WorkStatus];

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

                _cmd.CommandText = "up_OutFinished_Delete2";
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
