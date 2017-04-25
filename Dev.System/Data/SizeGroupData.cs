using System;
using System.Data;
using System.Data.SqlClient;

namespace Dev.Codes.Data
{
    public class SizeGroupData
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
        public static DataRow Insert(int Client, string SizeGroupName, 
            int SizeIdx1, int SizeIdx2, int SizeIdx3, int SizeIdx4, int SizeIdx5, int SizeIdx6, int SizeIdx7, int SizeIdx8, 
            int IsUse)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_SizeGroup_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                                
                _cmd.Parameters.Add("@Client", SqlDbType.Int, 4);
                _cmd.Parameters["@Client"].Value = Client;

                _cmd.Parameters.Add("@SizeGroupName", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@SizeGroupName"].Value = SizeGroupName;

                _cmd.Parameters.Add("@SizeIdx1", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx1"].Value = SizeIdx1;

                _cmd.Parameters.Add("@SizeIdx2", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx2"].Value = SizeIdx2;

                _cmd.Parameters.Add("@SizeIdx3", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx3"].Value = SizeIdx3;

                _cmd.Parameters.Add("@SizeIdx4", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx4"].Value = SizeIdx4;

                _cmd.Parameters.Add("@SizeIdx5", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx5"].Value = SizeIdx5;

                _cmd.Parameters.Add("@SizeIdx6", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx6"].Value = SizeIdx6;

                _cmd.Parameters.Add("@SizeIdx7", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx7"].Value = SizeIdx7;

                _cmd.Parameters.Add("@SizeIdx8", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx8"].Value = SizeIdx8;
                
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
        public static bool Update(int SizeGroupIdx, int Client, string SizeGroupName,
            int SizeIdx1, int SizeIdx2, int SizeIdx3, int SizeIdx4, int SizeIdx5, int SizeIdx6, int SizeIdx7, int SizeIdx8,
            int IsUse)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_SizeGroup_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@SizeGroupIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeGroupIdx"].Value = SizeGroupIdx;

                _cmd.Parameters.Add("@Client", SqlDbType.Int, 4);
                _cmd.Parameters["@Client"].Value = Client;

                _cmd.Parameters.Add("@SizeGroupName", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@SizeGroupName"].Value = SizeGroupName;

                _cmd.Parameters.Add("@SizeIdx1", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx1"].Value = SizeIdx1;

                _cmd.Parameters.Add("@SizeIdx2", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx2"].Value = SizeIdx2;

                _cmd.Parameters.Add("@SizeIdx3", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx3"].Value = SizeIdx3;

                _cmd.Parameters.Add("@SizeIdx4", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx4"].Value = SizeIdx4;

                _cmd.Parameters.Add("@SizeIdx5", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx5"].Value = SizeIdx5;

                _cmd.Parameters.Add("@SizeIdx6", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx6"].Value = SizeIdx6;

                _cmd.Parameters.Add("@SizeIdx7", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx7"].Value = SizeIdx7;

                _cmd.Parameters.Add("@SizeIdx8", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeIdx8"].Value = SizeIdx8;

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
        public static DataRow Get(int SizeGroupIdx)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_SizeGroup_Get";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@SizeGroupIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeGroupIdx"].Value = SizeGroupIdx;

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

                _cmd.CommandText = "up_SizeGroup_List";
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
        public static DataSet Getlist(int Client)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_SizeGroup_List2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Client", SqlDbType.Int, 4);
                _cmd.Parameters["@Client"].Value = Client;

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
        public static bool Delete(int SizeGroupIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_SizeGroup_Delete";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@SizeGroupIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeGroupIdx"].Value = SizeGroupIdx;

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

                _cmd.CommandText = "up_SizeGroup_Delete2";
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
