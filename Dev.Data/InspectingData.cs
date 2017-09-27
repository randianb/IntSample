using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Dev.Data
{
    public class InspectingData
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
        public static DataRow Insert(int OrderIdx, int SewIdx, string WorkOrderIdx, int TDName, 
            DateTime InspRequestedDate, string Result, string Action, int Reorder, int Handler 
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Inspecting_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@SewIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SewIdx"].Value = SewIdx;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 13);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

                _cmd.Parameters.Add("@TDName", SqlDbType.Int, 4);
                _cmd.Parameters["@TDName"].Value = TDName;
                
                _cmd.Parameters.Add("@InspRequestedDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@InspRequestedDate"].Value = InspRequestedDate;
                                
                _cmd.Parameters.Add("@Result", SqlDbType.NVarChar, 300);
                _cmd.Parameters["@Result"].Value = Result;

                _cmd.Parameters.Add("@Action", SqlDbType.NText, 2000);
                _cmd.Parameters["@Action"].Value = Action;

                _cmd.Parameters.Add("@Reorder", SqlDbType.Int, 4);
                _cmd.Parameters["@Reorder"].Value = Reorder;

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

        public static DataRow Insert(int OrderIdx, string WorkOrderIdx, int TDName,
            DateTime InspRequestedDate, string Result, string Action, int Reorder, int Handler
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Inspecting_Insert2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;
                
                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 13);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

                _cmd.Parameters.Add("@TDName", SqlDbType.Int, 4);
                _cmd.Parameters["@TDName"].Value = TDName;

                _cmd.Parameters.Add("@InspRequestedDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@InspRequestedDate"].Value = InspRequestedDate;

                _cmd.Parameters.Add("@Result", SqlDbType.NVarChar, 300);
                _cmd.Parameters["@Result"].Value = Result;

                _cmd.Parameters.Add("@Action", SqlDbType.NText, 2000);
                _cmd.Parameters["@Action"].Value = Action;

                _cmd.Parameters.Add("@Reorder", SqlDbType.Int, 4);
                _cmd.Parameters["@Reorder"].Value = Reorder;

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
        public static bool Update(int Idx, DateTime InspDate, DateTime InspCompletedDate
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Inspecting_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;
                
                _cmd.Parameters.Add("@InspDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@InspDate"].Value = InspDate;

                _cmd.Parameters.Add("@InspCompletedDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@InspCompletedDate"].Value = InspCompletedDate;
                
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

        public static bool Update(int Idx, string WorkOrderIdx, string Result, string Action, int Reorder, int Status
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Inspecting_Update2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

                _cmd.Parameters.Add("@Result", SqlDbType.NVarChar, 300);
                _cmd.Parameters["@Result"].Value = Result;

                _cmd.Parameters.Add("@Action", SqlDbType.NText, 2000);
                _cmd.Parameters["@Action"].Value = Action;

                _cmd.Parameters.Add("@Reorder", SqlDbType.Int, 4);
                _cmd.Parameters["@Reorder"].Value = Reorder;

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = Status;

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

                _cmd.CommandText = "up_Inspecting_Get";
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
                                      Dictionary<CommonValues.KeyName, string> SearchString)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Inspecting_List2";
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

                _cmd.Parameters.Add("@TDName", SqlDbType.Int, 4);
                _cmd.Parameters["@TDName"].Value = SearchKey[CommonValues.KeyName.User];
                
                _cmd.Parameters.Add("@InspRequestedDate", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@InspRequestedDate"].Value = SearchString[CommonValues.KeyName.RequestDate]=="2000-01-01" ? "" : SearchString[CommonValues.KeyName.RequestDate];

                _cmd.Parameters.Add("@InspCompletedDate", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@InspCompletedDate"].Value = SearchString[CommonValues.KeyName.CompleteDate] == "2000-01-01" ? "" : SearchString[CommonValues.KeyName.CompleteDate];

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

        public static DataSet Getlist(int Idx)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Inspecting_List3";
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
                return _ds;
            }
            else
            {
                return null;
            }
        }

        //public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey)
        //{
        //    try
        //    {
        //        _conn = new SqlConnection(_strConn);
        //        _cmd = new SqlCommand();
        //        _conn.Open();
        //        _ds = new DataSet();
        //        _adapter = new SqlDataAdapter();

        //        _cmd.CommandText = "up_Printing_List3";
        //        _cmd.CommandType = CommandType.StoredProcedure;
        //        _cmd.Connection = _conn;

        //        _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
        //        _cmd.Parameters["@OrderIdx"].Value = SearchKey[CommonValues.KeyName.OrderIdx];

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

        public static bool Delete(string condition)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Inspecting_Delete2";
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
