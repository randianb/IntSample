using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Telerik.WinControls;

namespace Dev.Data
{
    public class OrderTypeData
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

        ///// <summary>
        ///// Insert
        ///// </summary>
        public static DataRow Insert(int OrderIdx, int OrdSizeIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_OrderType_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@OrdSizeIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx"].Value = OrdSizeIdx;


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
            if (_ds!=null && _ds.Tables[0].Rows.Count > 0)
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
        public static bool Update(int Idx, int type101, int type102, int type103,
            int type201, int type202, int type203, int type204, int type205, int type206, int type207, int type208, int type209, int type210,
            int type211, int type212, int type213, int type214, int type215, int type216, int type217, int type218, int type219, int type220, 
            int type221, int type222, int type223, int type224, int type225 
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_OrderType_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

                _cmd.Parameters.Add("@type101", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type101"].Value = type101;

                _cmd.Parameters.Add("@type102", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type102"].Value = type102;

                _cmd.Parameters.Add("@type103", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type103"].Value = type103;

                #region Type 201 ~ 

                _cmd.Parameters.Add("@type201", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type201"].Value = type201;

                _cmd.Parameters.Add("@type202", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type202"].Value = type202;

                _cmd.Parameters.Add("@type203", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type203"].Value = type203;

                _cmd.Parameters.Add("@type204", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type204"].Value = type204;

                _cmd.Parameters.Add("@type205", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type205"].Value = type205;

                _cmd.Parameters.Add("@type206", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type206"].Value = type206;

                _cmd.Parameters.Add("@type207", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type207"].Value = type207;

                _cmd.Parameters.Add("@type208", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type208"].Value = type208;

                _cmd.Parameters.Add("@type209", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type209"].Value = type209;

                _cmd.Parameters.Add("@type210", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type210"].Value = type210;

                #endregion
                
                #region Type 211 ~

                _cmd.Parameters.Add("@type211", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type211"].Value = type211;

                _cmd.Parameters.Add("@type212", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type212"].Value = type212;

                _cmd.Parameters.Add("@type213", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type213"].Value = type213;

                _cmd.Parameters.Add("@type214", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type214"].Value = type214;

                _cmd.Parameters.Add("@type215", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type215"].Value = type215;

                _cmd.Parameters.Add("@type216", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type216"].Value = type216;

                _cmd.Parameters.Add("@type217", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type217"].Value = type217;

                _cmd.Parameters.Add("@type218", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type218"].Value = type218;

                _cmd.Parameters.Add("@type219", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type219"].Value = type219;

                _cmd.Parameters.Add("@type220", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type220"].Value = type220;

                _cmd.Parameters.Add("@type221", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type221"].Value = type221;

                _cmd.Parameters.Add("@type222", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type222"].Value = type222;

                _cmd.Parameters.Add("@type223", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type223"].Value = type223;

                _cmd.Parameters.Add("@type224", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type224"].Value = type224;

                _cmd.Parameters.Add("@type225", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@type225"].Value = type225;

                #endregion

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

                _cmd.CommandText = "up_OrderType_Get";
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

        public static DataSet Getlist(int OrderIdx)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_OrderType_List2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;
                
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

        public static DataSet Getlist(int OrderIdx, int OrdSizeIdx)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_OrderType_List";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@OrdSizeIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrdSizeIdx"].Value = OrdSizeIdx;

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

                _cmd.CommandText = "up_OrderType_Delete2";
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
