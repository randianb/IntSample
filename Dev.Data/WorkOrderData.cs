using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Dev.Data
{
    public class WorkOrderData
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
        //public static DataRow Insert(int OrderIdx, string WorkOrderIdx, int OrdSizeIdx, DateTime TechpackDate, DateTime RequestedDate,
        //    int Requested, string Comments, string Attached1, string Attached2, string Attached3, string Attached4, string Attached5,
        //    string AttachedUrl1, string AttachedUrl2, string AttachedUrl3, string AttachedUrl4, string AttachedUrl5, int Handler
        //    )
        //{
        //    try
        //    {
        //        _cmd = new SqlCommand();
        //        _conn = new SqlConnection(_strConn);
        //        _conn.Open();
        //        _ds = new DataSet();
        //        _adapter = new SqlDataAdapter();

        //        _cmd.CommandText = "up_Pattern_Insert";
        //        _cmd.CommandType = CommandType.StoredProcedure;
        //        _cmd.Connection = _conn;

        //        _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
        //        _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

        //        _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
        //        _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

        //        _cmd.Parameters.Add("@OrdSizeIdx", SqlDbType.Int, 4);
        //        _cmd.Parameters["@OrdSizeIdx"].Value = OrdSizeIdx;

        //        _cmd.Parameters.Add("@TechpackDate", SqlDbType.DateTime, 8);
        //        _cmd.Parameters["@TechpackDate"].Value = TechpackDate;

        //        _cmd.Parameters.Add("@RequestedDate", SqlDbType.DateTime, 8);
        //        _cmd.Parameters["@RequestedDate"].Value = RequestedDate;

        //        _cmd.Parameters.Add("@Requested", SqlDbType.Int, 4);
        //        _cmd.Parameters["@Requested"].Value = Requested;

        //        _cmd.Parameters.Add("@Comments", SqlDbType.Text);
        //        _cmd.Parameters["@Comments"].Value = Comments;

        //        _cmd.Parameters.Add("@Attached1", SqlDbType.NVarChar, 50);
        //        _cmd.Parameters["@Attached1"].Value = Attached1;

        //        _cmd.Parameters.Add("@Attached2", SqlDbType.NVarChar, 50);
        //        _cmd.Parameters["@Attached2"].Value = Attached2;

        //        _cmd.Parameters.Add("@Attached3", SqlDbType.NVarChar, 50);
        //        _cmd.Parameters["@Attached3"].Value = Attached3;

        //        _cmd.Parameters.Add("@Attached4", SqlDbType.NVarChar, 50);
        //        _cmd.Parameters["@Attached4"].Value = Attached4;

        //        _cmd.Parameters.Add("@Attached5", SqlDbType.NVarChar, 50);
        //        _cmd.Parameters["@Attached5"].Value = Attached5;

        //        _cmd.Parameters.Add("@AttachedUrl1", SqlDbType.NVarChar, 255);
        //        _cmd.Parameters["@AttachedUrl1"].Value = AttachedUrl1;

        //        _cmd.Parameters.Add("@AttachedUrl2", SqlDbType.NVarChar, 255);
        //        _cmd.Parameters["@AttachedUrl2"].Value = AttachedUrl2;

        //        _cmd.Parameters.Add("@AttachedUrl3", SqlDbType.NVarChar, 255);
        //        _cmd.Parameters["@AttachedUrl3"].Value = AttachedUrl3;

        //        _cmd.Parameters.Add("@AttachedUrl4", SqlDbType.NVarChar, 255);
        //        _cmd.Parameters["@AttachedUrl4"].Value = AttachedUrl4;

        //        _cmd.Parameters.Add("@AttachedUrl5", SqlDbType.NVarChar, 255);
        //        _cmd.Parameters["@AttachedUrl5"].Value = AttachedUrl5;

        //        _cmd.Parameters.Add("@Handler", SqlDbType.Int, 4);
        //        _cmd.Parameters["@Handler"].Value = Handler;


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
        //        _dr = _ds.Tables[0].Rows[0];
        //        return _dr;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 기본 업데이트 코드 
        /// </summary>
        public static bool Update(string WorkOrderIdx, DateTime Start, DateTime End, double Progress, DateTime TicketDate, string Qrcode, 
            int Modified, int Status
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_WorkOrder_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;
                
                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

                _cmd.Parameters.Add("@Start", SqlDbType.DateTime, 8);
                _cmd.Parameters["@Start"].Value = Start;

                _cmd.Parameters.Add("@End", SqlDbType.DateTime, 8);
                _cmd.Parameters["@End"].Value = End;

                _cmd.Parameters.Add("@Progress", SqlDbType.Float, 8);
                _cmd.Parameters["@Progress"].Value = Progress;

                _cmd.Parameters.Add("@TicketDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@TicketDate"].Value = TicketDate;

                _cmd.Parameters.Add("@Qrcode", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@Qrcode"].Value = Qrcode;
                
                _cmd.Parameters.Add("@Modified", SqlDbType.Int, 4);
                _cmd.Parameters["@Modified"].Value = Modified;

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = Status;

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
        /// 작업관리 화면에서 QR코드 생성 업데이트할때 사용 
        /// </summary>
        /// <param name="WorkOrderIdx"></param>
        /// <param name="TicketDate"></param>
        /// <param name="Status"></param>
        /// <param name="Qrcode"></param>
        /// <param name="Modified"></param>
        /// <returns></returns>
        public static bool Update(string WorkOrderIdx, DateTime TicketDate, int Status, string Qrcode, int Modified)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_WorkOrder_Update3";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;
                
                _cmd.Parameters.Add("@TicketDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@TicketDate"].Value = TicketDate;

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = Status;

                _cmd.Parameters.Add("@Qrcode", SqlDbType.NVarChar, 255);
                _cmd.Parameters["@Qrcode"].Value = Qrcode;

                _cmd.Parameters.Add("@Modified", SqlDbType.Int, 4);
                _cmd.Parameters["@Modified"].Value = Modified;
                                
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
        /// 스케쥴관리 화면에서 작업일 변경시 사용 
        /// </summary>
        /// <param name="WorkOrderIdx"></param>
        /// <param name="Start"></param>
        /// <param name="End"></param>
        /// <param name="Progress"></param>
        /// <param name="Modified"></param>
        /// <returns></returns>
        public static bool Update(string WorkOrderIdx, DateTime Start, DateTime End, double Progress, int Modified)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_WorkOrder_Update2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 14);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

                _cmd.Parameters.Add("@Start", SqlDbType.DateTime, 8);
                _cmd.Parameters["@Start"].Value = Start;

                _cmd.Parameters.Add("@End", SqlDbType.DateTime, 8);
                _cmd.Parameters["@End"].Value = End;

                _cmd.Parameters.Add("@Progress", SqlDbType.Float, 8);
                _cmd.Parameters["@Progress"].Value = Progress;
                
                _cmd.Parameters.Add("@Modified", SqlDbType.Int, 4);
                _cmd.Parameters["@Modified"].Value = Modified;
                
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

        public static bool Update(string SQL)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_WorkOrder_Update4";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@SQL", SqlDbType.VarChar, 5000);
                _cmd.Parameters["@SQL"].Value = SQL;
                
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
        //public static DataRow Get(int Idx)
        //{
        //    try
        //    {
        //        _conn = new SqlConnection(_strConn);
        //        _cmd = new SqlCommand();
        //        _conn.Open();
        //        _ds = new DataSet();
        //        _adapter = new SqlDataAdapter();

        //        _cmd.CommandText = "up_Pattern_Get";
        //        _cmd.CommandType = CommandType.StoredProcedure;
        //        _cmd.Connection = _conn;

        //        _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
        //        _cmd.Parameters["@Idx"].Value = Idx;

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
        //        _dr = _ds.Tables[0].Rows[0];
        //        return _dr;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// 오더관리화면에서 작업지시 조회용 
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <param name="TicketDate"></param>
        /// <returns></returns>
        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey, string WorkOrderIdx, string TicketDate)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_WorkOrder_List2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = SearchKey[CommonValues.KeyName.OrderIdx];

                _cmd.Parameters.Add("@OperationIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OperationIdx"].Value = SearchKey[CommonValues.KeyName.OperationIdx];

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.Status];
                                
                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

                _cmd.Parameters.Add("@TicketDate", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@TicketDate"].Value = TicketDate;


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
        /// 스케쥴화면에서 기본 스케쥴 조회용
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <param name="WorkOrderIdx"></param>
        /// <param name="TicketDate"></param>
        /// <returns></returns>
        public static DataSet Getlist2(Dictionary<CommonValues.KeyName, int> SearchKey, string WorkOrderIdx, string TicketDate)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_WorkOrder_List5";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = SearchKey[CommonValues.KeyName.OrderIdx];

                _cmd.Parameters.Add("@OperationIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OperationIdx"].Value = SearchKey[CommonValues.KeyName.OperationIdx];

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.Status];

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@WorkOrderIdx"].Value = WorkOrderIdx;

                _cmd.Parameters.Add("@TicketDate", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@TicketDate"].Value = TicketDate;


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
        /// 작업관리화면 조회용
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <param name="SearchString"></param>
        /// <returns></returns>
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

                _cmd.CommandText = "up_WorkOrder_List3";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@CustIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@CustIdx"].Value = SearchKey[CommonValues.KeyName.CustIdx];

                _cmd.Parameters.Add("@OperationIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OperationIdx"].Value = SearchKey[CommonValues.KeyName.OperationIdx];

                _cmd.Parameters.Add("@Fileno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Fileno"].Value = SearchString[CommonValues.KeyName.Fileno];

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.Status];

                _cmd.Parameters.Add("@Styleno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Styleno"].Value = SearchString[CommonValues.KeyName.Styleno];

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@WorkOrderIdx"].Value = SearchString[CommonValues.KeyName.WorkOrderIdx];

                _cmd.Parameters.Add("@WorkStatus", SqlDbType.Int, 4);
                _cmd.Parameters["@WorkStatus"].Value = SearchKey[CommonValues.KeyName.WorkStatus];

                _cmd.Parameters.Add("@StartDate", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@StartDate"].Value = SearchString[CommonValues.KeyName.StartDate] == "2000-01-01" ? "" : SearchString[CommonValues.KeyName.StartDate];
                
                _cmd.Parameters.Add("@TicketDate", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@TicketDate"].Value = SearchString[CommonValues.KeyName.TicketDate] == "2000-01-01" ? "" : SearchString[CommonValues.KeyName.TicketDate];

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
        /// 작업티켓 발행용 반복 
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <param name="SearchString"></param>
        /// <returns></returns>
        public static DataRow Getlist(string Operation, string WorkOrderIdx)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_WorkOrder_List4";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Operation", SqlDbType.NVarChar, 15);
                _cmd.Parameters["@Operation"].Value = Operation;

                _cmd.Parameters.Add("@WorkOrderIdx", SqlDbType.NVarChar, 15);
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
                return _ds.Tables[0].Rows[0];
            }
            else
            {
                return null;
            }
        }

        //public static bool Delete(string condition)
        //{
        //    try
        //    {
        //        _cmd = new SqlCommand();
        //        _conn = new SqlConnection(_strConn);
        //        _conn.Open();

        //        _cmd.CommandText = "up_Pattern_Delete2";
        //        _cmd.CommandType = CommandType.StoredProcedure;
        //        _cmd.Connection = _conn;

        //        _cmd.Parameters.Add("@DeleteIdx", SqlDbType.NVarChar, 1000);
        //        _cmd.Parameters["@DeleteIdx"].Value = condition;

        //        _rtn = _cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message.ToString());
        //    }
        //    finally
        //    {
        //        _conn.Close();
        //    }

        //    if (_rtn > 0)
        //        return true;
        //    else
        //        return false;
        //}


        #endregion
    }
}
