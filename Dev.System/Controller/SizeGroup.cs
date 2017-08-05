using System;
using System.Data;

namespace Dev.Codes.Controller
{
    public class SizeGroup
    {
        #region Members
                
        private int _sizeGroupIdx;
        private int _client;
        private string _sizeGroupName;
        private int _sizeIdx1;
        private int _sizeIdx2;
        private int _sizeIdx3;
        private int _sizeIdx4;
        private int _sizeIdx5;
        private int _sizeIdx6;
        private int _sizeIdx7;
        private int _sizeIdx8;
        private int _sizeIdx9;
        private int _sizeIdx10;
        private int _isUse;
        
        private DataRow _row;

        #endregion

        #region Property 
        
        //ID
        public int SizeGroupIdx
        {
            get { return _sizeGroupIdx; }
            set { _sizeGroupIdx = value; }
        }
        //바이어
        public int Client
        {
            get { return _client; }
            set { _client = value; }
        }
        //사이즈그룹명 
        public string SizeGroupName
        {
            get { return _sizeGroupName; }
            set { _sizeGroupName = value; }
        }
        //사이즈
        public int SizeIdx1
        {
            get { return _sizeIdx1; }
            set { _sizeIdx1 = value; }
        }
        //사이즈
        public int SizeIdx2
        {
            get { return _sizeIdx2; }
            set { _sizeIdx2 = value; }
        }
        //사이즈
        public int SizeIdx3
        {
            get { return _sizeIdx3; }
            set { _sizeIdx3 = value; }
        }
        //사이즈
        public int SizeIdx4
        {
            get { return _sizeIdx4; }
            set { _sizeIdx4 = value; }
        }
        //사이즈
        public int SizeIdx5
        {
            get { return _sizeIdx5; }
            set { _sizeIdx5 = value; }
        }
        //사이즈
        public int SizeIdx6
        {
            get { return _sizeIdx6; }
            set { _sizeIdx6 = value; }
        }
        //사이즈
        public int SizeIdx7
        {
            get { return _sizeIdx7; }
            set { _sizeIdx7 = value; }
        }
        //사이즈
        public int SizeIdx8
        {
            get { return _sizeIdx8; }
            set { _sizeIdx8 = value; }
        }
        public int SizeIdx9
        {
            get { return _sizeIdx9; }
            set { _sizeIdx9 = value; }
        }
        public int SizeIdx10
        {
            get { return _sizeIdx10; }
            set { _sizeIdx10 = value; }
        }
        //사용여부
        public int IsUse
        {
            get { return _isUse; }
            set { _isUse = value; }
        }

        #endregion

        #region Constructor 

        public SizeGroup(int sizeGroupIdx, int client, string sizeGroupName, 
            int sizeIdx1, int sizeIdx2, int sizeIdx3, int sizeIdx4,
            int sizeIdx5, int sizeIdx6, int sizeIdx7, int sizeIdx8, int sizeIdx9, int sizeIdx10,
            int isUse)
        {
            SizeGroupIdx = sizeGroupIdx;
            Client = client;
            SizeGroupName = sizeGroupName;
            SizeIdx1 = sizeIdx1;
            SizeIdx2 = sizeIdx2;
            SizeIdx3 = sizeIdx3;
            SizeIdx4 = sizeIdx4;
            SizeIdx5 = sizeIdx5;
            SizeIdx6 = sizeIdx6;
            SizeIdx7 = sizeIdx7;
            SizeIdx8 = sizeIdx8;
            SizeIdx9 = sizeIdx9;
            SizeIdx10 = sizeIdx10;
            IsUse = isUse; 
        }

        public SizeGroup(int SizeGroupIdx)
        {
            _row = Data.SizeGroupData.Get(SizeGroupIdx);

            if (_row != null)
            {
                _sizeGroupIdx = Convert.ToInt32(_row["SizeGroupIdx"]);
                if (_row["Client"] != DBNull.Value) _client = Convert.ToInt32(_row["Client"]);
                if (_row["SizeGroupName"] != DBNull.Value) _sizeGroupName = Convert.ToString(_row["SizeGroupName"]);
                if (_row["SizeIdx1"] != DBNull.Value) _sizeIdx1 = Convert.ToInt32(_row["SizeIdx1"]);
                if (_row["SizeIdx2"] != DBNull.Value) _sizeIdx2 = Convert.ToInt32(_row["SizeIdx2"]);
                if (_row["SizeIdx3"] != DBNull.Value) _sizeIdx3 = Convert.ToInt32(_row["SizeIdx3"]);
                if (_row["SizeIdx4"] != DBNull.Value) _sizeIdx4 = Convert.ToInt32(_row["SizeIdx4"]);
                if (_row["SizeIdx5"] != DBNull.Value) _sizeIdx5 = Convert.ToInt32(_row["SizeIdx5"]);
                if (_row["SizeIdx6"] != DBNull.Value) _sizeIdx6 = Convert.ToInt32(_row["SizeIdx6"]);
                if (_row["SizeIdx7"] != DBNull.Value) _sizeIdx7 = Convert.ToInt32(_row["SizeIdx7"]);
                if (_row["SizeIdx8"] != DBNull.Value) _sizeIdx8 = Convert.ToInt32(_row["SizeIdx8"]);
                if (_row["SizeIdx9"] != DBNull.Value) _sizeIdx9 = Convert.ToInt32(_row["SizeIdx9"]);
                if (_row["SizeIdx10"] != DBNull.Value) _sizeIdx10 = Convert.ToInt32(_row["SizeIdx10"]);


                if (_row["IsUse"] != DBNull.Value) _isUse = Convert.ToInt32(_row["IsUse"]);
            }
            else
            {
                Initializer();
            }
        }

        private void Initializer()
        {
            _sizeGroupIdx = 0;
            _client = 0;
            _sizeGroupName = "";
            _sizeIdx1 = 0;
            _sizeIdx2 = 0;
            _sizeIdx3 = 0;
            _sizeIdx4 = 0;
            _sizeIdx5 = 0;
            _sizeIdx6 = 0;
            _sizeIdx7 = 0;
            _sizeIdx8 = 0;
            _sizeIdx9 = 0;
            _sizeIdx10 = 0;
            _isUse = 0;
        }
        #endregion

        #region Methods

        // Insert
        public static DataRow Insert(int Client, string SizeGroupName,
            int SizeIdx1, int SizeIdx2, int SizeIdx3, int SizeIdx4, int SizeIdx5, int SizeIdx6, int SizeIdx7, int SizeIdx8, int SizeIdx9, int SizeIdx10,
            int IsUse)
        {
            DataRow row = Data.SizeGroupData.Insert(Client, SizeGroupName,
            SizeIdx1, SizeIdx2, SizeIdx3, SizeIdx4, SizeIdx5, SizeIdx6, SizeIdx7, SizeIdx8, SizeIdx9, SizeIdx10, 
            IsUse);
            return row;
        }

        // Getlist with search parameter
        public static DataSet Getlist()
        {
            DataSet ds = new DataSet();
            ds = Data.SizeGroupData.Getlist();
            return ds;
        }
        /// <summary>
        /// 신규 샘플오더 생성시 사이즈 그룹 선택창을 위한 조회 
        /// </summary>
        /// <returns></returns>
        public static DataSet GetlistName()
        {
            DataSet ds = new DataSet();
            ds = Data.SizeGroupData.GetlistName();
            return ds;
        }
        

        public static DataSet Getlist(int Client)
        {
            DataSet ds = new DataSet();
            ds = Data.SizeGroupData.Getlist(Client);
            return ds;
        }

        public static DataSet Getlist(int Client, int DeptIdx)
        {
            DataSet ds = new DataSet();
            ds = Data.SizeGroupData.Getlist(Client, DeptIdx);
            return ds;
        }

        public static DataRow Get(int SizeGroupIdx)
        {
            DataRow dr = Data.SizeGroupData.Get(SizeGroupIdx);
            return dr;
        }

        public bool Update()
        {
            bool blRtn;
            blRtn = Data.SizeGroupData.Update(_sizeGroupIdx, _client, _sizeGroupName,
            _sizeIdx1, _sizeIdx2, _sizeIdx3, _sizeIdx4, _sizeIdx5, _sizeIdx6, _sizeIdx7, _sizeIdx8, _sizeIdx9, _sizeIdx10,
            _isUse);
            return blRtn;
        }
        
        // Multiple Delete
        public static bool Delete(string condition)
        {
            bool blRtn;
            blRtn = Data.SizeGroupData.Delete(condition);
            return blRtn;
        }

        #endregion
    }
}
