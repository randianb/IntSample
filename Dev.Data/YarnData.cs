using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Dev.Data
{
    public class YarnData
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

                _cmd.CommandText = "up_Yarn_Insert";
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
        public static bool Update(int Idx, string Composition, string BurnCount, string YarnType,
            string Contents1, string Contents2, string Contents3, string Contents4,
            double Percent1, double Percent2, double Percent3, double Percent4,
            string Remark, int IsUse)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Yarn_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

                _cmd.Parameters.Add("@Composition", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Composition"].Value = Composition;

                _cmd.Parameters.Add("@BurnCount", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@BurnCount"].Value = BurnCount;

                _cmd.Parameters.Add("@YarnType", SqlDbType.NVarChar, 20);
                _cmd.Parameters["@YarnType"].Value = YarnType;

                _cmd.Parameters.Add("@Contents1", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Contents1"].Value = Contents1;

                _cmd.Parameters.Add("@Contents2", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Contents2"].Value = Contents2;

                _cmd.Parameters.Add("@Contents3", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Contents3"].Value = Contents3;

                _cmd.Parameters.Add("@Contents4", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Contents4"].Value = Contents4;
                
                _cmd.Parameters.Add("@Percent1", SqlDbType.Float, 8);
                _cmd.Parameters["@Percent1"].Value = Percent1;

                _cmd.Parameters.Add("@Percent2", SqlDbType.Float, 8);
                _cmd.Parameters["@Percent2"].Value = Percent2;
                
                _cmd.Parameters.Add("@Percent3", SqlDbType.Float, 8);
                _cmd.Parameters["@Percent3"].Value = Percent3;

                _cmd.Parameters.Add("@Percent4", SqlDbType.Float, 8);
                _cmd.Parameters["@Percent4"].Value = Percent4;

                _cmd.Parameters.Add("@Remark", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Remark"].Value = Remark;
                
                _cmd.Parameters.Add("@IsUse", SqlDbType.TinyInt, 1);
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

                _cmd.CommandText = "up_Yarn_Get";
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
        public static DataSet Getlist()
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Yarn_List";
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
                return _ds;
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// YarnType
        /// </summary>
        public static DataTable Getlist_YarnType()
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _dt = new DataTable();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_YarnType_List";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _adapter.SelectCommand = _cmd;
                _adapter.Fill(_dt);

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
            if ((_dt != null) && (_dt.Rows.Count > 0))
            {
                return _dt;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Getlist_Composition
        /// </summary>
        public static DataTable Getlist_Composition()
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _dt = new DataTable();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_YarnComposition_List";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _adapter.SelectCommand = _cmd;
                _adapter.Fill(_dt);

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
            if ((_dt != null) && (_dt.Rows.Count > 0))
            {
                return _dt;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Getlist_Burncount
        /// </summary>
        public static DataTable Getlist_Burncount()
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _dt = new DataTable();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_YarnBurncount_List";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _adapter.SelectCommand = _cmd;
                _adapter.Fill(_dt);

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
            if ((_dt != null) && (_dt.Rows.Count > 0))
            {
                return _dt;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Getlist_YarnContents
        /// </summary>
        public static DataTable Getlist_YarnContents()
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _dt = new DataTable();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_YarnContents_List";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _adapter.SelectCommand = _cmd;
                _adapter.Fill(_dt);

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
            if ((_dt != null) && (_dt.Rows.Count > 0))
            {
                return _dt;
            }
            else
            {
                return null;
            }
        }

        public static DataSet Getlist(Dictionary<CommonValues.KeyName, string> SearchString)
        {
            try
            {
                
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Yarn_List2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@Composition", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Composition"].Value = SearchString[CommonValues.KeyName.Composition];

                _cmd.Parameters.Add("@BurnCount", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@BurnCount"].Value = SearchString[CommonValues.KeyName.BurnCount];

                _cmd.Parameters.Add("@YarnType", SqlDbType.NVarChar, 20);
                _cmd.Parameters["@YarnType"].Value = SearchString[CommonValues.KeyName.YarnType];

                _cmd.Parameters.Add("@Contents", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Contents"].Value = SearchString[CommonValues.KeyName.Contents];

                _cmd.Parameters.Add("@Remark", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Remark"].Value = SearchString[CommonValues.KeyName.Remark];
                
                _cmd.Parameters.Add("@IsUse", SqlDbType.NVarChar, 1);
                _cmd.Parameters["@IsUse"].Value = SearchString[CommonValues.KeyName.IsUse];

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

                _cmd.CommandText = "up_Yarn_Delete2";
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
