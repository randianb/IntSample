using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Dev.Data
{
    public class OutboundData
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
        /// 스캔 출고
        /// </summary>
        public static DataRow Insert(string InWorkOrderIdx, int Status, int RegCenterIdx, int RegDeptIdx, int RegUserIdx, int IOCenterIdx, int IODeptIdx, 
                int OrderIdx, string WorkOrderIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_FabricOut_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@InWorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@InWorkOrderIdx"].Value = InWorkOrderIdx;

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = Status;

                _cmd.Parameters.Add("@RegCenterIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@RegCenterIdx"].Value = RegCenterIdx;

                _cmd.Parameters.Add("@RegDeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@RegDeptIdx"].Value = RegDeptIdx;

                _cmd.Parameters.Add("@RegUserIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@RegUserIdx"].Value = RegUserIdx;

                _cmd.Parameters.Add("@IOCenterIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@IOCenterIdx"].Value = IOCenterIdx;

                _cmd.Parameters.Add("@IODeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@IODeptIdx"].Value = IODeptIdx;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;
                
                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;
                
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
        /// 수동 출고
        /// </summary>
        public static DataRow Insert2(int Status, int RegCenterIdx, int RegDeptIdx, int RegUserIdx, string WorkOrderIdx, int InIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_FabricOut_Insert2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = Status;

                _cmd.Parameters.Add("@RegCenterIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@RegCenterIdx"].Value = RegCenterIdx;

                _cmd.Parameters.Add("@RegDeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@RegDeptIdx"].Value = RegDeptIdx;

                _cmd.Parameters.Add("@RegUserIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@RegUserIdx"].Value = RegUserIdx;
                
                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

                _cmd.Parameters.Add("@InIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@InIdx"].Value = InIdx;

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
        public static bool Update(int Idx, DateTime IDate, int BuyerIdx, string ColorIdx, int FabricType, 
            string Artno, string Lotno, int FabricIdx, int Roll, int Width, double Kgs, double Yds, 
            int OrderIdx, string Comments, int Handler, int InIdx, int IsOut )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_FabricOut_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;
                
                _cmd.Parameters.Add("@IDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@IDate"].Value = IDate;

                _cmd.Parameters.Add("@BuyerIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@BuyerIdx"].Value = BuyerIdx;

                _cmd.Parameters.Add("@ColorIdx", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@ColorIdx"].Value = ColorIdx;

                _cmd.Parameters.Add("@FabricType", SqlDbType.Int, 4);
                _cmd.Parameters["@FabricType"].Value = FabricType;

                _cmd.Parameters.Add("@Artno", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@Artno"].Value = Artno;

                _cmd.Parameters.Add("@Lotno", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@Lotno"].Value = Lotno;

                _cmd.Parameters.Add("@FabricIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@FabricIdx"].Value = FabricIdx;

                _cmd.Parameters.Add("@Roll", SqlDbType.SmallInt, 2);
                _cmd.Parameters["@Roll"].Value = Roll;

                _cmd.Parameters.Add("@Width", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@Width"].Value = Width;

                _cmd.Parameters.Add("@Kgs", SqlDbType.Float, 4);
                _cmd.Parameters["@Kgs"].Value = Kgs;

                _cmd.Parameters.Add("@Yds", SqlDbType.Float, 4);
                _cmd.Parameters["@Yds"].Value = Yds;
                
                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;
                
                _cmd.Parameters.Add("@Comments", SqlDbType.NVarChar, 80);
                _cmd.Parameters["@Comments"].Value = Comments;

                _cmd.Parameters.Add("@Handler", SqlDbType.Int, 4);
                _cmd.Parameters["@Handler"].Value = Handler;

                _cmd.Parameters.Add("@InIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@InIdx"].Value = InIdx;

                _cmd.Parameters.Add("@IsOut", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@IsOut"].Value = IsOut;
                                
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

                _cmd.CommandText = "up_FabricOut_Get";
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
                
        
        public static DataSet Getlist(Dictionary<CommonValues.KeyName, string> SearchString, 
                                        Dictionary<CommonValues.KeyName, int> SearchKey, string indate)
        {
            try
            {
                
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_FabricOut_List2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.Status];

                _cmd.Parameters.Add("@BuyerIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@BuyerIdx"].Value = SearchKey[CommonValues.KeyName.BuyerIdx];

                _cmd.Parameters.Add("@ColorIdx", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@ColorIdx"].Value = SearchString[CommonValues.KeyName.ColorIdx];

                _cmd.Parameters.Add("@FabricType", SqlDbType.Int, 4);
                _cmd.Parameters["@FabricType"].Value = SearchKey[CommonValues.KeyName.FabricType];

                _cmd.Parameters.Add("@Lotno", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Lotno"].Value = SearchString[CommonValues.KeyName.Lotno];

                _cmd.Parameters.Add("@FabricIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@FabricIdx"].Value = SearchKey[CommonValues.KeyName.FabricIdx];

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = SearchKey[CommonValues.KeyName.OrderIdx];
                
                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@WorkOrderIdx"].Value = SearchString[CommonValues.KeyName.WorkOrderIdx];

                _cmd.Parameters.Add("@Handler", SqlDbType.Int, 4);
                _cmd.Parameters["@Handler"].Value = SearchKey[CommonValues.KeyName.Handler];

                _cmd.Parameters.Add("@InIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@InIdx"].Value = SearchKey[CommonValues.KeyName.InIdx];

                _cmd.Parameters.Add("@indate", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@indate"].Value = (indate == "2000-01-01" || string.IsNullOrEmpty(indate) ? "" : indate);
                
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

        public static DataSet GetlistReport(Dictionary<CommonValues.KeyName, string> SearchString,
                                        Dictionary<CommonValues.KeyName, int> SearchKey, string indate)
        {
            try
            {

                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_FabricOut_List4";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.Status];

                _cmd.Parameters.Add("@BuyerIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@BuyerIdx"].Value = SearchKey[CommonValues.KeyName.BuyerIdx];

                _cmd.Parameters.Add("@ColorIdx", SqlDbType.NVarChar, 30);
                _cmd.Parameters["@ColorIdx"].Value = SearchString[CommonValues.KeyName.ColorIdx];

                _cmd.Parameters.Add("@FabricType", SqlDbType.Int, 4);
                _cmd.Parameters["@FabricType"].Value = SearchKey[CommonValues.KeyName.FabricType];

                _cmd.Parameters.Add("@Lotno", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Lotno"].Value = SearchString[CommonValues.KeyName.Lotno];

                _cmd.Parameters.Add("@FabricIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@FabricIdx"].Value = SearchKey[CommonValues.KeyName.FabricIdx];

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = SearchKey[CommonValues.KeyName.OrderIdx];

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@WorkOrderIdx"].Value = SearchString[CommonValues.KeyName.WorkOrderIdx];

                _cmd.Parameters.Add("@Handler", SqlDbType.Int, 4);
                _cmd.Parameters["@Handler"].Value = SearchKey[CommonValues.KeyName.Handler];

                _cmd.Parameters.Add("@InIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@InIdx"].Value = SearchKey[CommonValues.KeyName.InIdx];

                _cmd.Parameters.Add("@indate", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@indate"].Value = (indate == "2000-01-01" || string.IsNullOrEmpty(indate) ? "" : indate);


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

                _cmd.CommandText = "up_FabricOut_Delete2";
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
