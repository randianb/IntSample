using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Dev.Data
{
    public class FabricData
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
        public static DataRow Insert()
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Fabrics_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
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
        public static bool Update(int Idx, string LongName, string ShortName, int Yarn1, int Yarn2, int Yarn3, int Yarn4, int Yarn5,
            double Percent1, double Percent2, double Percent3, double Percent4, double Percent5,
            int IsUse)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Fabrics_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

                _cmd.Parameters.Add("@LongName", SqlDbType.NVarChar, 80);
                _cmd.Parameters["@LongName"].Value = LongName;

                _cmd.Parameters.Add("@ShortName", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@ShortName"].Value = ShortName;

                _cmd.Parameters.Add("@Yarn1", SqlDbType.Int, 4);
                _cmd.Parameters["@Yarn1"].Value = Yarn1;

                _cmd.Parameters.Add("@Yarn2", SqlDbType.Int, 4);
                _cmd.Parameters["@Yarn2"].Value = Yarn2;

                _cmd.Parameters.Add("@Yarn3", SqlDbType.Int, 4);
                _cmd.Parameters["@Yarn3"].Value = Yarn3;

                _cmd.Parameters.Add("@Yarn4", SqlDbType.Int, 4);
                _cmd.Parameters["@Yarn4"].Value = Yarn4;

                _cmd.Parameters.Add("@Yarn5", SqlDbType.Int, 4);
                _cmd.Parameters["@Yarn5"].Value = Yarn5;
                
                _cmd.Parameters.Add("@Percent1", SqlDbType.Float, 8);
                _cmd.Parameters["@Percent1"].Value = Percent1;

                _cmd.Parameters.Add("@Percent2", SqlDbType.Float, 8);
                _cmd.Parameters["@Percent2"].Value = Percent2;
                
                _cmd.Parameters.Add("@Percent3", SqlDbType.Float, 8);
                _cmd.Parameters["@Percent3"].Value = Percent3;

                _cmd.Parameters.Add("@Percent4", SqlDbType.Float, 8);
                _cmd.Parameters["@Percent4"].Value = Percent4;

                _cmd.Parameters.Add("@Percent5", SqlDbType.Float, 8);
                _cmd.Parameters["@Percent5"].Value = Percent5;

                _cmd.Parameters.Add("@IsUse", SqlDbType.SmallInt, 2);
                _cmd.Parameters["@IsUse"].Value = IsUse;

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

                _cmd.CommandText = "up_Fabrics_Get";
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

        
        public static DataSet Getlist(string FabricName, string IsUse)
        {
            try
            {
                
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Fabrics_List2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@FabricName", SqlDbType.NVarChar, 80);
                _cmd.Parameters["@FabricName"].Value = FabricName;

                _cmd.Parameters.Add("@IsUse", SqlDbType.NVarChar, 1);
                _cmd.Parameters["@IsUse"].Value = IsUse;
                
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

                _cmd.CommandText = "up_Fabrics_Delete2";
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
        
        #endregion
    }
}
