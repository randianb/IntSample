using Dev.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Dev.Sales.Data
{
    public class OrdersData
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
        public static DataRow Insert(string Fileno, int DeptIdx, int Reorder, string ReorderReason, 
            DateTime Indate, int Buyer, int Vendor, int Country, string Pono, string Styleno, string SampleType, string InspType,
            string Season, string Description, DateTime DeliveryDate, int IsPrinting,
            int EmbelishId1, int EmbelishId2, int SizeGroupIdx, int SewThreadIdx, 
            int OrderQty, double OrderPrice, double OrderAmount, string Remark,
            DateTime TeamRequestedDate, DateTime SplConfirmedDate, int Handler, int IsSample
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Orders_Insert";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Fileno", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@Fileno"].Value = Fileno;

                _cmd.Parameters.Add("@DeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@DeptIdx"].Value = DeptIdx;

                _cmd.Parameters.Add("@Reorder", SqlDbType.Int, 4);
                _cmd.Parameters["@Reorder"].Value = Reorder;

                _cmd.Parameters.Add("@ReorderReason", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@ReorderReason"].Value = ReorderReason;

                _cmd.Parameters.Add("@Indate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@Indate"].Value = Indate;

                _cmd.Parameters.Add("@Buyer", SqlDbType.Int, 4);
                _cmd.Parameters["@Buyer"].Value = Buyer;

                _cmd.Parameters.Add("@Vendor", SqlDbType.Int, 4);
                _cmd.Parameters["@Vendor"].Value = Vendor;
                
                _cmd.Parameters.Add("@Country", SqlDbType.Int, 4);
                _cmd.Parameters["@Country"].Value = Country;

                _cmd.Parameters.Add("@Pono", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Pono"].Value = Pono;

                _cmd.Parameters.Add("@Styleno", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Styleno"].Value = Styleno;

                _cmd.Parameters.Add("@SampleType", SqlDbType.NVarChar, 3);
                _cmd.Parameters["@SampleType"].Value = SampleType;

                _cmd.Parameters.Add("@InspType", SqlDbType.NVarChar, 20);
                _cmd.Parameters["@InspType"].Value = InspType;

                _cmd.Parameters.Add("@Season", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Season"].Value = Season;

                _cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 300);
                _cmd.Parameters["@Description"].Value = Description;

                _cmd.Parameters.Add("@DeliveryDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@DeliveryDate"].Value = DeliveryDate;

                _cmd.Parameters.Add("@IsPrinting", SqlDbType.Int, 4);
                _cmd.Parameters["@IsPrinting"].Value = IsPrinting;

                _cmd.Parameters.Add("@EmbelishId1", SqlDbType.Int, 4);
                _cmd.Parameters["@EmbelishId1"].Value = EmbelishId1;

                _cmd.Parameters.Add("@EmbelishId2", SqlDbType.Int, 4);
                _cmd.Parameters["@EmbelishId2"].Value = EmbelishId2;
                
                _cmd.Parameters.Add("@SizeGroupIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeGroupIdx"].Value = SizeGroupIdx;

                _cmd.Parameters.Add("@SewThreadIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SewThreadIdx"].Value = SewThreadIdx;

                _cmd.Parameters.Add("@OrderQty", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderQty"].Value = OrderQty;

                _cmd.Parameters.Add("@OrderPrice", SqlDbType.Float, 8);
                _cmd.Parameters["@OrderPrice"].Value = OrderPrice;

                _cmd.Parameters.Add("@OrderAmount", SqlDbType.Float, 8);
                _cmd.Parameters["@OrderAmount"].Value = OrderAmount;

                _cmd.Parameters.Add("@Remark", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Remark"].Value = Remark;

                _cmd.Parameters.Add("@TeamRequestedDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@TeamRequestedDate"].Value = TeamRequestedDate;

                _cmd.Parameters.Add("@SplConfirmedDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@SplConfirmedDate"].Value = SplConfirmedDate;
                
                _cmd.Parameters.Add("@Handler", SqlDbType.Int, 4);
                _cmd.Parameters["@Handler"].Value = Handler;

                _cmd.Parameters.Add("@IsSample", SqlDbType.Int, 4);
                _cmd.Parameters["@IsSample"].Value = IsSample;

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
            if (_ds != null)
            {
                _dr = _ds.Tables[0].Rows[0];
                return _dr;
            }
            else
            {
                return null;
            }
        }

        public static bool CloseOrder(int idx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "update Orders set status=@status where idx=@idx";
                _cmd.CommandType = CommandType.Text; 
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@status", SqlDbType.TinyInt, 2);
                _cmd.Parameters["@status"].Value = 3;

                _rtn = _cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                _conn.Close(); 
            }
            if (_rtn>0) return true;
            else return false;
        }
        public static bool CancelOrder(int idx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "update Orders set status=@status where idx=@idx";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = idx;

                _cmd.Parameters.Add("@status", SqlDbType.TinyInt, 2);
                _cmd.Parameters["@status"].Value = 2;

                _rtn = _cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _conn.Close();
            }
            if (_rtn > 0) return true;
            else return false;
        }

        /// <summary>
        /// Update
        /// </summary>
        public static bool Update(int Idx, string Fileno, int DeptIdx, int Reorder, string ReorderReason,
            DateTime Indate, int Buyer, int Vendor, int Country, string Pono, string Styleno, string SampleType, string InspType,
            string Season, string Description, DateTime DeliveryDate, int IsPrinting,
            int EmbelishId1, int EmbelishId2, int SizeGroupIdx, int SewThreadIdx,
            int OrderQty, double OrderPrice, double OrderAmount, string Remark,
            DateTime TeamRequestedDate, DateTime SplConfirmedDate, int Handler, int IsSample
            )
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Orders_Update";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

                _cmd.Parameters.Add("@Fileno", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@Fileno"].Value = Fileno;

                _cmd.Parameters.Add("@DeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@DeptIdx"].Value = DeptIdx;

                _cmd.Parameters.Add("@Reorder", SqlDbType.Int, 4);
                _cmd.Parameters["@Reorder"].Value = Reorder;

                _cmd.Parameters.Add("@ReorderReason", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@ReorderReason"].Value = ReorderReason;

                _cmd.Parameters.Add("@Indate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@Indate"].Value = Indate;

                _cmd.Parameters.Add("@Buyer", SqlDbType.Int, 4);
                _cmd.Parameters["@Buyer"].Value = Buyer;

                _cmd.Parameters.Add("@Vendor", SqlDbType.Int, 4);
                _cmd.Parameters["@Vendor"].Value = Vendor;

                _cmd.Parameters.Add("@Country", SqlDbType.Int, 4);
                _cmd.Parameters["@Country"].Value = Country;

                _cmd.Parameters.Add("@Pono", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Pono"].Value = Pono;

                _cmd.Parameters.Add("@Styleno", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Styleno"].Value = Styleno;

                _cmd.Parameters.Add("@SampleType", SqlDbType.NVarChar, 3);
                _cmd.Parameters["@SampleType"].Value = SampleType;

                _cmd.Parameters.Add("@InspType", SqlDbType.NVarChar, 20);
                _cmd.Parameters["@InspType"].Value = InspType;

                _cmd.Parameters.Add("@Season", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Season"].Value = Season;

                _cmd.Parameters.Add("@Description", SqlDbType.NVarChar, 300);
                _cmd.Parameters["@Description"].Value = Description;

                _cmd.Parameters.Add("@DeliveryDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@DeliveryDate"].Value = DeliveryDate;

                _cmd.Parameters.Add("@IsPrinting", SqlDbType.Int, 4);
                _cmd.Parameters["@IsPrinting"].Value = IsPrinting;

                _cmd.Parameters.Add("@EmbelishId1", SqlDbType.Int, 4);
                _cmd.Parameters["@EmbelishId1"].Value = EmbelishId1;

                _cmd.Parameters.Add("@EmbelishId2", SqlDbType.Int, 4);
                _cmd.Parameters["@EmbelishId2"].Value = EmbelishId2;

                _cmd.Parameters.Add("@SizeGroupIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SizeGroupIdx"].Value = SizeGroupIdx;

                _cmd.Parameters.Add("@SewThreadIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@SewThreadIdx"].Value = SewThreadIdx;

                _cmd.Parameters.Add("@OrderQty", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderQty"].Value = OrderQty;

                _cmd.Parameters.Add("@OrderPrice", SqlDbType.Float, 8);
                _cmd.Parameters["@OrderPrice"].Value = OrderPrice;

                _cmd.Parameters.Add("@OrderAmount", SqlDbType.Float, 8);
                _cmd.Parameters["@OrderAmount"].Value = OrderAmount;

                _cmd.Parameters.Add("@Remark", SqlDbType.NVarChar, 50);
                _cmd.Parameters["@Remark"].Value = Remark;

                _cmd.Parameters.Add("@TeamRequestedDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@TeamRequestedDate"].Value = TeamRequestedDate;

                _cmd.Parameters.Add("@SplConfirmedDate", SqlDbType.DateTime, 8);
                _cmd.Parameters["@SplConfirmedDate"].Value = SplConfirmedDate;

                _cmd.Parameters.Add("@Handler", SqlDbType.Int, 4);
                _cmd.Parameters["@Handler"].Value = Handler;

                _cmd.Parameters.Add("@IsSample", SqlDbType.Int, 4);
                _cmd.Parameters["@IsSample"].Value = IsSample;
                
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

                _cmd.CommandText = "up_Orders_Get";
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

                _cmd.CommandText = "up_Orders_List";
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
        
        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey, string fileno, string style)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Orders_List2";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                
                _cmd.Parameters.Add("@DeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@DeptIdx"].Value = SearchKey[CommonValues.KeyName.DeptIdx];

                _cmd.Parameters.Add("@CustIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@CustIdx"].Value = SearchKey[CommonValues.KeyName.CustIdx];

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.Status];

                _cmd.Parameters.Add("@Handler", SqlDbType.Int, 4);
                _cmd.Parameters["@Handler"].Value = SearchKey[CommonValues.KeyName.Handler];

                _cmd.Parameters.Add("@EmbIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@EmbIdx"].Value = SearchKey[CommonValues.KeyName.EmbelishId1];

                // 1711001-01S
                _cmd.Parameters.Add("@Fileno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Fileno"].Value = fileno;

                _cmd.Parameters.Add("@Styleno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Styleno"].Value = style;


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

        public static DataSet Getlist(Dictionary<CommonValues.KeyName, int> SearchKey, string fileno, string style,
                                     int orderOpCheck1, int orderOpCheck2, int orderOpCheck3)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Orders_List4";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;


                _cmd.Parameters.Add("@DeptIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@DeptIdx"].Value = SearchKey[CommonValues.KeyName.DeptIdx];

                _cmd.Parameters.Add("@CustIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@CustIdx"].Value = SearchKey[CommonValues.KeyName.CustIdx];

                _cmd.Parameters.Add("@Status", SqlDbType.Int, 4);
                _cmd.Parameters["@Status"].Value = SearchKey[CommonValues.KeyName.Status];

                _cmd.Parameters.Add("@Handler", SqlDbType.Int, 4);
                _cmd.Parameters["@Handler"].Value = SearchKey[CommonValues.KeyName.Handler];

                _cmd.Parameters.Add("@EmbIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@EmbIdx"].Value = SearchKey[CommonValues.KeyName.EmbelishId1];

                // 1711001-01S
                _cmd.Parameters.Add("@Fileno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Fileno"].Value = fileno;

                _cmd.Parameters.Add("@Styleno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Styleno"].Value = style;

                _cmd.Parameters.Add("@orderOpCheck1", SqlDbType.Int, 4);
                _cmd.Parameters["@orderOpCheck1"].Value = orderOpCheck1;

                _cmd.Parameters.Add("@orderOpCheck2", SqlDbType.Int, 4);
                _cmd.Parameters["@orderOpCheck2"].Value = orderOpCheck2;

                _cmd.Parameters.Add("@orderOpCheck3", SqlDbType.Int, 4);
                _cmd.Parameters["@orderOpCheck3"].Value = orderOpCheck3;


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

        public static DataSet Getlist(int Idx, int CustIdx, string Fileno, string Styleno)
        {
            try
            {
                _conn = new SqlConnection(_strConn);
                _cmd = new SqlCommand();
                _conn.Open();
                _ds = new DataSet();
                _adapter = new SqlDataAdapter();

                _cmd.CommandText = "up_Orders_List3";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

                _cmd.Parameters.Add("@CustIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@CustIdx"].Value = CustIdx;

                _cmd.Parameters.Add("@Fileno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Fileno"].Value = Fileno;

                _cmd.Parameters.Add("@Styleno", SqlDbType.NVarChar, 40);
                _cmd.Parameters["@Styleno"].Value = Styleno;

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

        //public static DataSet OrderDetail(int DeptIdx, DateTime fromDate, DateTime toDate)
        //{
        //    try
        //    {
        //        _conn = new SqlConnection(_strConn);
        //        _cmd = new SqlCommand();
        //        _conn.Open();
        //        _ds = new DataSet();
        //        _adapter = new SqlDataAdapter();

        //        _cmd.CommandText = "up_iOrderStatus_Detail";
        //        _cmd.CommandType = CommandType.StoredProcedure;
        //        _cmd.Connection = _conn;

        //        _cmd.Parameters.Add("@DeptIdx", SqlDbType.Int, 4);
        //        _cmd.Parameters["@DeptIdx"].Value = DeptIdx;

        //        _cmd.Parameters.Add("@fromDate", SqlDbType.DateTime, 8);
        //        _cmd.Parameters["@fromDate"].Value = fromDate;

        //        _cmd.Parameters.Add("@toDate", SqlDbType.DateTime, 8);
        //        _cmd.Parameters["@toDate"].Value = toDate;

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

        //public static DataSet OrderDetailNoShip(int DeptIdx, DateTime fromDate, DateTime toDate)
        //{
        //    try
        //    {
        //        _conn = new SqlConnection(_strConn);
        //        _cmd = new SqlCommand();
        //        _conn.Open();
        //        _ds = new DataSet();
        //        _adapter = new SqlDataAdapter();

        //        _cmd.CommandText = "up_iOrderStatus_DetailNoShip";
        //        _cmd.CommandType = CommandType.StoredProcedure;
        //        _cmd.Connection = _conn;

        //        _cmd.Parameters.Add("@DeptIdx", SqlDbType.Int, 4);
        //        _cmd.Parameters["@DeptIdx"].Value = DeptIdx;

        //        _cmd.Parameters.Add("@fromDate", SqlDbType.DateTime, 8);
        //        _cmd.Parameters["@fromDate"].Value = fromDate;

        //        _cmd.Parameters.Add("@toDate", SqlDbType.DateTime, 8);
        //        _cmd.Parameters["@toDate"].Value = toDate;

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

        //public static DataSet OrderSummary(DateTime indate)
        //{
        //    try
        //    {
        //        _conn = new SqlConnection(_strConn);
        //        _cmd = new SqlCommand();
        //        _conn.Open();
        //        _ds = new DataSet();
        //        _adapter = new SqlDataAdapter();

        //        _cmd.CommandText = "up_iOrderStatus_List";
        //        _cmd.CommandType = CommandType.StoredProcedure;
        //        _cmd.Connection = _conn;

        //        _cmd.Parameters.Add("@indate", SqlDbType.DateTime, 8);
        //        _cmd.Parameters["@indate"].Value = indate;

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

        //public static DataSet OrderMonthly(DateTime indate)
        //{
        //    try
        //    {
        //        _conn = new SqlConnection(_strConn);
        //        _cmd = new SqlCommand();
        //        _conn.Open();
        //        _ds = new DataSet();
        //        _adapter = new SqlDataAdapter();

        //        _cmd.CommandText = "up_iOrderStatus_Monthly";
        //        _cmd.CommandType = CommandType.StoredProcedure;
        //        _cmd.Connection = _conn;

        //        _cmd.Parameters.Add("@indate", SqlDbType.DateTime, 8);
        //        _cmd.Parameters["@indate"].Value = indate;

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
        public static bool Delete(int Idx)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = "up_Orders_Delete";
                _cmd.CommandType = CommandType.StoredProcedure;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

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

                _cmd.CommandText = "up_Orders_Delete2";
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

        public static bool CopyOrder(int Idx, string fileno)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"INSERT INTO Orders (Fileno, DeptIdx, Reorder, ReorderReason, 
		                            Indate, Buyer, Vendor, Country, Pono, Styleno, SampleType, InspType, 
                                    Season, [Description], DeliveryDate, IsPrinting, 
		                            EmbelishId1, EmbelishId2, SizeGroupIdx, SewThreadIdx, 
		                            OrderQty, OrderPrice, OrderAmount, Remark, 
		                            TeamRequestedDate,  
		                            Handler, Status)
                         
                        select @fileno, DeptIdx, 0, '', Indate, Buyer, Vendor, Country, '' Pono, Styleno, SampleType, InspType, 
                        Season, Description, DeliveryDate, 
		                IsPrinting, EmbelishId1, EmbelishId2, SizeGroupIdx, SewThreadIdx, 
                        OrderQty, OrderPrice, OrderAmount, '', 
                        getdate(),  
		                Handler, 0 Status 
                   from Orders where idx = @idx";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@Idx", SqlDbType.Int, 4);
                _cmd.Parameters["@Idx"].Value = Idx;

                _cmd.Parameters.Add("@fileno", SqlDbType.NVarChar, 10);
                _cmd.Parameters["@fileno"].Value = fileno;
                
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

        //public static DataSet GetlistOrderSumDate()
        //{
        //    try
        //    {
        //        _conn = new SqlConnection(_strConn);
        //        _cmd = new SqlCommand();
        //        _conn.Open();
        //        _ds = new DataSet();
        //        _adapter = new SqlDataAdapter();

        //        _cmd.CommandText = "up_iOrderStatus_List_OrderSumDate";
        //        _cmd.CommandType = CommandType.StoredProcedure;
        //        _cmd.Connection = _conn;

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

        //      public static DataSet LoadAppointments()
        //      {
        //          try
        //          {
        //              _conn = new SqlConnection(_strConn);
        //              _cmd = new SqlCommand();
        //              _conn.Open();
        //              _ds = new DataSet();
        //              _adapter = new SqlDataAdapter();

        //              _cmd.CommandText = @"SELECT [ID]
        //    ,[Summary]
        //    ,[Start]
        //    ,[End]
        //    ,[RecurrenceRule]
        //    ,[MasterEventId]
        //    ,[Location]
        //    ,[Description]
        //    ,[BackgroundId]
        //    ,[DeptIdx]
        //FROM [intsample].[dbo].[Appointments] 
        //  where year(start)=year(getdate())";
        //              _cmd.CommandType = CommandType.Text;
        //              _cmd.Connection = _conn;

        //              _adapter.SelectCommand = _cmd;
        //              _adapter.Fill(_ds);

        //          }
        //          catch (Exception ex)
        //          {
        //              Console.WriteLine(ex.Message.ToString());
        //          }
        //          finally
        //          {
        //              _conn.Close();
        //          }

        //          // dataset 확인 및 결과 datarow 반환
        //          if ((_ds != null) && (_ds.Tables[0].Rows.Count > 0))
        //          {
        //              return _ds;
        //          }
        //          else
        //          {
        //              return null;
        //          }
        //      }

        public static bool ModifyOrderQty(int OrderIdx, int tqty)
        {
            try
            {
                _cmd = new SqlCommand();
                _conn = new SqlConnection(_strConn);
                _conn.Open();

                _cmd.CommandText = @"Update Orders set OrderQty=@tqty where idx = @OrderIdx";
                _cmd.CommandType = CommandType.Text;
                _cmd.Connection = _conn;

                _cmd.Parameters.Add("@OrderIdx", SqlDbType.Int, 4);
                _cmd.Parameters["@OrderIdx"].Value = OrderIdx;

                _cmd.Parameters.Add("@tqty", SqlDbType.Int, 4);
                _cmd.Parameters["@tqty"].Value = tqty;
                
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
