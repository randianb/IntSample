using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Dev.Codes.Data
{
    public class LocationData
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
        public static DataRow Insert(int CostcenterIdx, int DeptIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Location_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@CostcenterIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@CostcenterIdx"].Value = CostcenterIdx;

                _cmd.Parameters.Add("@DeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@DeptIdx"].Value = DeptIdx;
                                
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
        public static bool Update(int Idx, string LocationName, int CostcenterIdx, int DeptIdx, int Warehouse, int PosX, int PosY, int RackNo,
            int Floorno, int RackPos, string Remark, int IsUse, string Qrcode)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Location_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

                _cmd.Parameters.Add("@LocationName", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@LocationName"].Value = LocationName;

                _cmd.Parameters.Add("@CostcenterIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@CostcenterIdx"].Value = CostcenterIdx;

                _cmd.Parameters.Add("@DeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@DeptIdx"].Value = DeptIdx;

                _cmd.Parameters.Add("@Warehouse", SqlDbType.Int, 4);
                _cmd.Parameters["@Warehouse"].Value = Warehouse;

                _cmd.Parameters.Add("@PosX", SqlDbType.SmallInt, 2);
                _cmd.Parameters["@PosX"].Value = PosX;

                _cmd.Parameters.Add("@PosY", SqlDbType.SmallInt, 2);
                _cmd.Parameters["@PosY"].Value = PosY;

                _cmd.Parameters.Add("@RackNo", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@RackNo"].Value = RackNo;

                _cmd.Parameters.Add("@Floorno", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@Floorno"].Value = Floorno;

                _cmd.Parameters.Add("@RackPos", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@RackPos"].Value = RackPos;

                _cmd.Parameters.Add("@Remark", SqlDbType.NVarChar, 100);
                _cmd.Parameters["@Remark"].Value = Remark;

                _cmd.Parameters.Add("@IsUse", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@IsUse"].Value = IsUse;

                _cmd.Parameters.Add("@Qrcode", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@Qrcode"].Value = Qrcode;
                
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

                _cmd.CommandText = "up_Location_Get";
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

        public static DataSet Getlist(int CostcenterIdx, int DeptIdx, int Warehouse, int PosX, int PosY, int RackNo,
            int Floorno, int RackPos, string Remark)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Location_List2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@CostcenterIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@CostcenterIdx"].Value = CostcenterIdx;

                _cmd.Parameters.Add("@DeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@DeptIdx"].Value = DeptIdx;

                _cmd.Parameters.Add("@Warehouse", SqlDbType.Int, 4);
                _cmd.Parameters["@Warehouse"].Value = Warehouse;
                
                _cmd.Parameters.Add("@RackNo", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@RackNo"].Value = RackNo;

                _cmd.Parameters.Add("@Floorno", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@Floorno"].Value = Floorno;

                _cmd.Parameters.Add("@RackPos", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@RackPos"].Value = RackPos;

                _cmd.Parameters.Add("@PosX", SqlDbType.SmallInt, 2);
                _cmd.Parameters["@PosX"].Value = PosX;

                _cmd.Parameters.Add("@PosY", SqlDbType.SmallInt, 2);
                _cmd.Parameters["@PosY"].Value = PosY;

                _cmd.Parameters.Add("@Remark", SqlDbType.NVarChar, 100);
                _cmd.Parameters["@Remark"].Value = Remark;

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
        /// Delete
        /// </summary>
        public static bool Delete(string condition)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Location_Delete2";
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
