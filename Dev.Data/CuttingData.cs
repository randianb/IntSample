using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Dev.Data
{
    public class CuttingData
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
        public static DataRow Insert(int OrderIdx, string WorkOrderIdx, string OrdColorIdx, int OrdSizeIdx, double OrdYds, int OrdQty, 
            int FabricIdx, string Remarks, int Handler 
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Cutting_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;
                
                _cmd.Parameters.Add("@OrdColorIdx", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@OrdColorIdx"].Value = OrdColorIdx;
                
                _cmd.Parameters.Add("@OrdSizeIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx"].Value = OrdSizeIdx;

                _cmd.Parameters.Add("@OrdYds", SqlDbType.Float, 8);
                _cmd.Parameters["@OrdYds"].Value = OrdYds;

                _cmd.Parameters.Add("@OrdQty", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdQty"].Value = OrdQty;

                _cmd.Parameters.Add("@FabricIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@FabricIdx"].Value = FabricIdx;
                
                _cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 100);
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
        public static bool Update(int Idx, DateTime CuttedDate, string CuttedNo, int CuttedQty, int CuttedPQty, 
            int FabricIdx, string Remarks 
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Cutting_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;
                
                _cmd.Parameters.Add("@CuttedDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@CuttedDate"].Value = CuttedDate;
                
                _cmd.Parameters.Add("@CuttedNo", SqlDbType.NVarChar, 2);
                _cmd.Parameters["@CuttedNo"].Value = CuttedNo;

                _cmd.Parameters.Add("@CuttedQty", SqlDbType.Int, 4);
                _cmd.Parameters["@CuttedQty"].Value = CuttedQty;

                _cmd.Parameters.Add("@CuttedPQty", SqlDbType.Int, 4);
                _cmd.Parameters["@CuttedPQty"].Value = CuttedPQty;
                
                _cmd.Parameters.Add("@FabricIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@FabricIdx"].Value = FabricIdx;

                _cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 100);
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

        public static bool CompleteWork(string WorkOrderIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"update workorder set status=3 where workorderidx=@WorkOrderIdx";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;
                

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

                _cmd.CommandText = "up_Cutting_Get";
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

                _cmd.CommandText = "up_Cutting_List2";
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

                _cmd.Parameters.Add("@FabricIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@FabricIdx"].Value = SearchKey[CommonValues.KeyName.FabricIdx];

                _cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@Remarks"].Value = SearchString[CommonValues.KeyName.Remark];

                _cmd.Parameters.Add("@CuttedDate", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@CuttedDate"].Value = SearchString[CommonValues.KeyName.StartDate]=="2000-01-01" ? "" : SearchString[CommonValues.KeyName.StartDate];

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

        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Cutting_List3";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                                
                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = SearchKey[CommonValues.KeyName.OrderIdx];
                
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

                _cmd.CommandText = "up_Cutting_Delete2";
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
