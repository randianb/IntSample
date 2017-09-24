using System;
using System.Data;

namespace Dev.Sales.Controller
{
    public class Operation
    {
        #region Members

        private int _idx;
        private int _orderIdx;
        private int _operationIdx;
        private int _priority;
        private int _work1;
        private int _work2;

        private DataRow _row;

        #endregion

        #region Property 

        //ID
        public int Idx
        {
            get { return _idx; }
            set { _idx = value; }
        }
        
        //
        public int OrderIdx
        {
            get { return _orderIdx; }
            set { _orderIdx = value; }
        }
        //
        public int OperationIdx
        {
            get { return _operationIdx; }
            set { _operationIdx = value; }
        }

        //
        public int Priority
        {
            get { return _priority; }
            set { _priority = value; }
        }
        //
        public int Work1
        {
            get { return _work1; }
            set { _work1 = value; }
        }
        //
        public int Work2
        {
            get { return _work2; }
            set { _work2 = value; }
        }
        #endregion

        #region Constructor 

        public Operation(int Idx)
        {
            _row = Data.OperationData.Get(Idx);
            
            if (_row != null)
            {
                _idx = Convert.ToInt32(_row["Idx"]);
                if (_row["OrderIdx"] != DBNull.Value) _orderIdx = Convert.ToInt32(_row["OrderIdx"]);
                if (_row["OperationIdx"] != DBNull.Value) _operationIdx = Convert.ToInt32(_row["OperationIdx"]);
                if (_row["Priority"] != DBNull.Value) _priority = Convert.ToInt32(_row["Priority"]);
                if (_row["Work1"] != DBNull.Value) _work1 = Convert.ToInt32(_row["Work1"]);
                if (_row["Work2"] != DBNull.Value) _work2 = Convert.ToInt32(_row["Work2"]);
            }
            else
            {
                Initializer();
            }
        }

        private void Initializer()
        {
            _idx = 0;
            _orderIdx = 0;
            _operationIdx = 0;
            _priority = 0;
            _work1 = 0;
            _work2 = 0;
            
        }

        #endregion

        #region Methods
        
        public static bool Insert(int OrderIdx)
        {
            bool rtn = Data.OperationData.Insert(OrderIdx);

            return rtn;
        }
        public static DataRow Insert2(int OrderIdx)
        {
            DataRow row = Data.OperationData.Insert2(OrderIdx);

            return row;
        }
        public static DataSet Getlist(int OrderIdx)
        {
            DataSet ds = new DataSet();
            
            ds = Data.OperationData.Getlist(OrderIdx); 
                        
            return ds;
        }
       
        public bool Update()
        {
            bool blRtn;
            blRtn = Data.OperationData.Update(_idx, _operationIdx, _priority, _work1, _work2); 
            return blRtn;
        }

        public static bool SwapPriority(int indexFrom, int indexTo, int priorityFrom, int priorityTo)
        {
            bool rtn = Data.OperationData.SwapPriority(indexFrom, indexTo, priorityFrom, priorityTo);

            return rtn;
        }
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.OperationData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
