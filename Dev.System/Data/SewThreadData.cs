using System;
using System.Data;
using System.Data.SqlClient;

namespace Dev.Codes.Data
{
    public class SewThreadData
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
        public static DataRow Insert(int SewThreadCustIdx, string SewThreadName, string ColorIdx, int IsUse)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_SewThread_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@SewThreadCustIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SewThreadCustIdx"].Value = SewThreadCustIdx;

                _cmd.Parameters.Add("@SewThreadName", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@SewThreadName"].Value = SewThreadName;

                _cmd.Parameters.Add("@ColorIdx", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@ColorIdx"].Value = ColorIdx;
                
                _cmd.Parameters.Add("@IsUse", SqlDbType.TinyInt, 1);
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
        public static bool Update(int SewThreadIdx, int SewThreadCustIdx, string SewThreadName, string ColorIdx, int IsUse, int IsAvailable, int IsProduction)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_SewThread_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@SewThreadIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SewThreadIdx"].Value = SewThreadIdx;

                _cmd.Parameters.Add("@SewThreadCustIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SewThreadCustIdx"].Value = SewThreadCustIdx;

                _cmd.Parameters.Add("@SewThreadName", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@SewThreadName"].Value = SewThreadName;

                _cmd.Parameters.Add("@ColorIdx", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@ColorIdx"].Value = ColorIdx;
                
                _cmd.Parameters.Add("@IsUse", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@IsUse"].Value = IsUse;

                _cmd.Parameters.Add("@IsAvailable", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@IsAvailable"].Value = IsAvailable;

                _cmd.Parameters.Add("@IsProduction", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@IsProduction"].Value = IsProduction;

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
        public static DataRow Get(int SewThreadIdx)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_SewThread_Get";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@SewThreadIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SewThreadIdx"].Value = SewThreadIdx;

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

                _cmd.CommandText = "up_SewThread_List";
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
        /// 사용가능한 목록조회
        /// </summary>
        public static DataSet GetUsablelist()
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_SewThread_List3";
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
        public static DataSet Getlist(int SewThreadCustIdx)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_SewThread_List2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@SewThreadCustIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SewThreadCustIdx"].Value = SewThreadCustIdx;

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

        //public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey, string fileno, string style)
        //{
        //    try
        //    {
        //        _conn = new SqlConnection(_strConn);
        //        _cmd = new SqlCommand();
        //        _conn.Open();
        //        _ds = new DataSet();
        //        _adapter = new SqlDataAdapter();

        //        _cmd.CommandText = "up_iOrderActual_List2";
        //        _cmd.CommandType = CommandType.StoredProcedure;
        //        _cmd.Connection = _conn;


        //        _cmd.Parameters.Add("@DeptIdx", SqlDbType.Int, 4);
        //        _cmd.Parameters["@DeptIdx"].Value = SearchKey[CommonValues.KeyName.DeptIdx];

        //        _cmd.Parameters.Add("@CustIdx", SqlDbType.Int, 4);
        //        _cmd.Parameters["@CustIdx"].Value = SearchKey[CommonValues.KeyName.CustIdx];

        //        _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
        //        _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.Status];

        //        _cmd.Parameters.Add("@EmbIdx", SqlDbType.Int, 4);
        //        _cmd.Parameters["@EmbIdx"].Value = SearchKey[CommonValues.KeyName.EmbelishId1];

        //        _cmd.Parameters.Add("@Fileno", SqlDbType.NVarChar, 40);
        //        _cmd.Parameters["@Fileno"].Value = fileno;

        //        _cmd.Parameters.Add("@Styleno", SqlDbType.NVarChar, 40);
        //        _cmd.Parameters["@Styleno"].Value = style;


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

        /// <summary>
        /// Delete
        /// </summary>
        public static bool Delete(int SewThreadIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_SewThread_Delete";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@SewThreadIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SewThreadIdx"].Value = SewThreadIdx;

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

        public static bool Delete(string condition)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_SewThread_Delete2";
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
