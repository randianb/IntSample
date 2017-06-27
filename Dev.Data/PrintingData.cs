using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Dev.Data
{
    public class PrintingData
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
        public static DataRow Insert(int OrderIdx, int CuttedIdx, string WorkOrderIdx, string OutOrderIdx, string OrdColorIdx, int OrdSizeIdx, 
            int OrdQty, DateTime RcvdDate, int RcvdQty, int RcvdFrom, string Remarks, int Handler 
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Printing_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@CuttedIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@CuttedIdx"].Value = CuttedIdx;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 13);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

                _cmd.Parameters.Add("@OutOrderIdx", SqlDbType.NVarChar, 13);
                _cmd.Parameters["@OutOrderIdx"].Value = OutOrderIdx;

                _cmd.Parameters.Add("@OrdColorIdx", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@OrdColorIdx"].Value = OrdColorIdx;
                
                _cmd.Parameters.Add("@OrdSizeIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx"].Value = OrdSizeIdx;
                
                _cmd.Parameters.Add("@OrdQty", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdQty"].Value = OrdQty;

                _cmd.Parameters.Add("@RcvdDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@RcvdDate"].Value = RcvdDate;

                _cmd.Parameters.Add("@RcvdQty", SqlDbType.Int, 4);
                _cmd.Parameters["@RcvdQty"].Value = RcvdQty;

                _cmd.Parameters.Add("@RcvdFrom", SqlDbType.Int, 4);
                _cmd.Parameters["@RcvdFrom"].Value = RcvdFrom;

                _cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Remarks"].Value = Remarks;
                
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
        public static bool Update(int Idx, string OutOrderIdx, DateTime RcvdDate, int RcvdQty, int RcvdFrom, string Remarks 
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Printing_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

                _cmd.Parameters.Add("@OutOrderIdx", SqlDbType.NVarChar, 13);
                _cmd.Parameters["@OutOrderIdx"].Value = OutOrderIdx;
                
                _cmd.Parameters.Add("@RcvdDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@RcvdDate"].Value = RcvdDate;
                
                _cmd.Parameters.Add("@RcvdQty", SqlDbType.Int, 4);
                _cmd.Parameters["@RcvdQty"].Value = RcvdQty;

                _cmd.Parameters.Add("@RcvdFrom", SqlDbType.Int, 4);
                _cmd.Parameters["@RcvdFrom"].Value = RcvdFrom;

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

                _cmd.CommandText = "up_Printing_Get";
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

                _cmd.CommandText = "up_Printing_List2";
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

                _cmd.Parameters.Add("@RcvdFrom", SqlDbType.Int, 4);
                _cmd.Parameters["@RcvdFrom"].Value = SearchKey[CommonValues.KeyName.CustIdx];

                _cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@Remarks"].Value = SearchString[CommonValues.KeyName.Remark];

                _cmd.Parameters.Add("@RcvdDate", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@RcvdDate"].Value = SearchString[CommonValues.KeyName.StartDate]=="2000-01-01" ? "" : SearchString[CommonValues.KeyName.StartDate];

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

                _cmd.CommandText = "up_Printing_Delete2";
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
