using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Dev.Data
{
    public class OrderColorData
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
        public static DataRow Insert(int OrderIdx, int ColorIdx, 
            int SizeIdx1, int SizeIdx2, int SizeIdx3, int SizeIdx4, int SizeIdx5, int SizeIdx6, int SizeIdx7, int SizeIdx8
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_OrderColor_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@ColorIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@ColorIdx"].Value = ColorIdx;
                
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
        public static bool Update(int Idx, int ColorIdx, int Classification, 
            int Pcs1, int Pcs2, int Pcs3, int Pcs4, int Pcs5, int Pcs6, int Pcs7, int Pcs8
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_OrderColor_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;
                
                _cmd.Parameters.Add("@ColorIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@ColorIdx"].Value = ColorIdx;

                _cmd.Parameters.Add("@Classification", SqlDbType.Int, 4);
                _cmd.Parameters["@Classification"].Value = Classification;

                _cmd.Parameters.Add("@Pcs1", SqlDbType.Int, 4);
                _cmd.Parameters["@Pcs1"].Value = Pcs1;

                _cmd.Parameters.Add("@Pcs2", SqlDbType.Int, 4);
                _cmd.Parameters["@Pcs2"].Value = Pcs2;

                _cmd.Parameters.Add("@Pcs3", SqlDbType.Int, 4);
                _cmd.Parameters["@Pcs3"].Value = Pcs3;

                _cmd.Parameters.Add("@Pcs4", SqlDbType.Int, 4);
                _cmd.Parameters["@Pcs4"].Value = Pcs4;

                _cmd.Parameters.Add("@Pcs5", SqlDbType.Int, 4);
                _cmd.Parameters["@Pcs5"].Value = Pcs5;

                _cmd.Parameters.Add("@Pcs6", SqlDbType.Int, 4);
                _cmd.Parameters["@Pcs6"].Value = Pcs6;

                _cmd.Parameters.Add("@Pcs7", SqlDbType.Int, 4);
                _cmd.Parameters["@Pcs7"].Value = Pcs7;

                _cmd.Parameters.Add("@Pcs8", SqlDbType.Int, 4);
                _cmd.Parameters["@Pcs8"].Value = Pcs8;

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

                _cmd.CommandText = "up_OrderColor_Get";
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
        /// 오더 건별 선택시 해당 오더의 컬러 사이즈 조회 
        /// </summary>
        /// <param name="OrderIdx">오더고유번호</param>
        /// <returns></returns>
        public static DataSet Getlist(int OrderIdx)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_OrderColor_List";
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

        /// <summary>
        /// 오더 건별 출고대비 밸런스 조회 (출고등록시 조회용) 
        /// </summary>
        /// <param name="OrderIdx">오더고유번호</param>
        /// <returns></returns>
        public static DataSet GetlistOutBalance(int OrderIdx)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_OrderColor_List2";
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


        public static bool Delete(string DeleteIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_OrderColor_Delete2";
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
