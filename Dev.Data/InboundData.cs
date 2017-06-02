using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Dev.Data
{
    public class InboundData
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
        public static DataRow Insert(string WorkOrderIdx, string Qrcode, int RegCenterIdx, int RegDeptIdx, int RegUserIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_FabricIn_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

                _cmd.Parameters.Add("@Qrcode", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@Qrcode"].Value = Qrcode;

                _cmd.Parameters.Add("@RegCenterIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@RegCenterIdx"].Value = RegCenterIdx;

                _cmd.Parameters.Add("@RegDeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@RegDeptIdx"].Value = RegDeptIdx;

                _cmd.Parameters.Add("@RegUserIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@RegUserIdx"].Value = RegUserIdx;

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
        public static bool Update(int Idx, int Status, DateTime IDate, int BuyerIdx, int ColorIdx, int FabricType, 
            string Artno, string Lotno, int FabricIdx, int Roll, int Width, double Kgs, double Yds, 
            int IOCenterIdx, int IODeptIdx, string Comments, int RackNo, int Floorno, int RackPos, 
            int PosX, int PosY, string Qrcode, string filenm1, string filenm2, string fileurl1, string fileurl2
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_FabricIn_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

                _cmd.Parameters.Add("@Status", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@Status"].Value = Status;

                _cmd.Parameters.Add("@IDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@IDate"].Value = IDate;

                _cmd.Parameters.Add("@BuyerIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@BuyerIdx"].Value = BuyerIdx;

                _cmd.Parameters.Add("@ColorIdx", SqlDbType.Int, 4);
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
                
                _cmd.Parameters.Add("@IOCenterIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@IOCenterIdx"].Value = IOCenterIdx;

                _cmd.Parameters.Add("@IODeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@IODeptIdx"].Value = IODeptIdx;

                _cmd.Parameters.Add("@Comments", SqlDbType.NVarChar, 80);
                _cmd.Parameters["@Comments"].Value = Comments;

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

                _cmd.Parameters.Add("@Qrcode", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@Qrcode"].Value = Qrcode;

                _cmd.Parameters.Add("@filenm1", SqlDbType.VarChar, 50);
                _cmd.Parameters["@filenm1"].Value = filenm1;

                _cmd.Parameters.Add("@filenm2", SqlDbType.VarChar, 50);
                _cmd.Parameters["@filenm2"].Value = filenm2;

                _cmd.Parameters.Add("@fileurl1", SqlDbType.VarChar, 255);
                _cmd.Parameters["@fileurl1"].Value = fileurl1;

                _cmd.Parameters.Add("@fileurl2", SqlDbType.VarChar, 255);
                _cmd.Parameters["@fileurl2"].Value = fileurl2;
                
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
        /// 코드스캔시 적재위치 업데이트 
        /// </summary>
        public static bool UpdateIn(string WorkOrderIdx, DateTime Indate, int RackNo, int Floorno, int RackPos, int PosX, int PosY
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_FabricIn_Inbound";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 12);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

                _cmd.Parameters.Add("@Indate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@Indate"].Value = Indate;

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

                _cmd.CommandText = "up_FabricIn_Get";
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

                _cmd.CommandText = "up_FabricIn_List";
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

                _cmd.CommandText = "up_FabricIn_List2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.Status];

                _cmd.Parameters.Add("@BuyerIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@BuyerIdx"].Value = SearchKey[CommonValues.KeyName.BuyerIdx];

                _cmd.Parameters.Add("@ColorIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@ColorIdx"].Value = SearchKey[CommonValues.KeyName.ColorIdx];

                _cmd.Parameters.Add("@FabricType", SqlDbType.Int, 4);
                _cmd.Parameters["@FabricType"].Value = SearchKey[CommonValues.KeyName.FabricType];

                _cmd.Parameters.Add("@Lotno", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Lotno"].Value = SearchString[CommonValues.KeyName.Lotno];

                _cmd.Parameters.Add("@FabricIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@FabricIdx"].Value = SearchKey[CommonValues.KeyName.FabricIdx];

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@WorkOrderIdx"].Value = SearchString[CommonValues.KeyName.WorkOrderIdx];

                _cmd.Parameters.Add("@RackNo", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@RackNo"].Value = SearchKey[CommonValues.KeyName.RackNo];

                _cmd.Parameters.Add("@Floorno", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@Floorno"].Value = SearchKey[CommonValues.KeyName.Floorno];

                _cmd.Parameters.Add("@RackPos", SqlDbType.TinyInt, 1);
                _cmd.Parameters["@RackPos"].Value = SearchKey[CommonValues.KeyName.RackPos];

                _cmd.Parameters.Add("@PosX", SqlDbType.SmallInt, 2);
                _cmd.Parameters["@PosX"].Value = SearchKey[CommonValues.KeyName.PosX];

                _cmd.Parameters.Add("@PosY", SqlDbType.SmallInt, 2);
                _cmd.Parameters["@PosY"].Value = SearchKey[CommonValues.KeyName.PosY];

                _cmd.Parameters.Add("@indate", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@indate"].Value = (indate=="2000-01-01" ? "" : indate);


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

        public static DataRow Getlist(string WorkOrderIdx)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_FabricIn_List3";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
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

        public static bool Delete(string DeleteIdx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_FabricIn_Delete2";
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
