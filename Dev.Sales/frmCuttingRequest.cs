using Dev.Options;
using Int.Code;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing; 
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Dev.Sales
{
    /// <summary>
    /// 재단 작업 지시
    /// </summary>
    public partial class frmCuttingRequest : RadForm
    {
        #region 1. 변수 선언

        private DataTable _dt = null;                                           // 기본 데이터테이블
        private DataSet _ds1 = null;                                            // 기본 데이터셋

        public Dictionary<CommonValues.KeyName, string> _searchString;          // 쿼리값 value, 쿼리항목 key로 전달 
        public Dictionary<CommonValues.KeyName, int> _searchKey;                // 쿼리값 value, 쿼리항목 key로 전달 

        private List<Codes.Controller.Sizes> lstSize = new List<Codes.Controller.Sizes>();
        private List<Int.Code.CodeContents> lstFabric = new List<Int.Code.CodeContents>();

        private string _fileNo, _styleNo = "";
        private int _orderIdx,  _orderStatus, _sizeGroup = 0;
        private bool _bRtn = false;

        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 39, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        #endregion

        #region 2. 폼 초기화

        /// <summary>
        /// parent로부터 order 정보 받아오기 
        /// </summary>
        /// <param name="idx">고유번호</param>
        /// <param name="fileNo">파일번호</param>
        /// <param name="qty">수량</param>
        /// <param name="amount">금액</param>
        /// <param name="shipCompleted">선적완료 여부</param>
        public frmCuttingRequest(int idx, string fileNo, string styleno, int sizeGroup, int orderStatus)
        {
            InitializeComponent();
            _orderIdx = idx; 
            _fileNo = fileNo;
            _styleNo = styleno;
            _sizeGroup = sizeGroup;
            _orderStatus = orderStatus; 

            lblFileno.Text = fileNo;
            lblStyle.Text = styleno;
            lblStatus.Text = orderStatus.ToString();
            lblWork.Text = DateTime.Today.ToString(); 

            this.StartPosition = FormStartPosition.CenterScreen;
            
            // 선적완료, 오더마감 여부에 따라 편집 disable 
            if (_orderStatus==2 || _orderStatus==3)
            {
                tableLayoutPanel2.Enabled = false; 
            }
        }

        /// <summary>
        /// 폼 로딩 (초기 메서드 호출) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmCuttingRequest_Load(object sender, EventArgs e)
        {
            DataLoading_DDL();
            GetSizes(_sizeGroup);

            GV1_CreateColumn(gvCutting);
            GV2_CreateColumn(gvColorSize);
            GV5_CreateColumn(gvFabric);
            
            DataBinding_GV2(gvColorSize, _orderIdx);
            DataBinding_GV5(gvFabric, _orderIdx);
            
        }

        #endregion

        #region 3. 컨트롤 생성 및 기본설정 

        /// <summary>
        /// 그리드뷰 컬럼 생성 (재단오더내역)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV1_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            gv.MasterTemplate.Columns.Clear();
            gv.DataSource = null;

            GridViewTextBoxColumn cColorIdx = new GridViewTextBoxColumn();
            cColorIdx.Name = "cColorIdx";
            cColorIdx.FieldName = "cColorIdx";
            cColorIdx.Width = 120;
            cColorIdx.TextAlignment = ContentAlignment.MiddleLeft;
            cColorIdx.HeaderText = "Color";
            gv.Columns.Add(cColorIdx);

            GridViewTextBoxColumn CutSizeIdx = new GridViewTextBoxColumn();
            CutSizeIdx.Name = "CutSizeIdx";
            CutSizeIdx.FieldName = "CutSizeIdx";
            CutSizeIdx.Width = 60;
            CutSizeIdx.TextAlignment = ContentAlignment.MiddleCenter;
            CutSizeIdx.HeaderText = "Size#";
            CutSizeIdx.IsVisible = false;
            gv.Columns.Add(CutSizeIdx);

            GridViewTextBoxColumn CutSize = new GridViewTextBoxColumn();
            CutSize.Name = "CutSize";
            CutSize.FieldName = "CutSize";
            CutSize.Width = 80;
            CutSize.TextAlignment = ContentAlignment.MiddleCenter;
            CutSize.HeaderText = "Size";
            CutSize.ReadOnly = true;
            gv.Columns.Add(CutSize);
            
            GridViewTextBoxColumn OrdQty = new GridViewTextBoxColumn();
            OrdQty.Name = "OrdQty";
            OrdQty.FieldName = "OrdQty";
            OrdQty.Width = 80;
            OrdQty.TextAlignment = ContentAlignment.MiddleRight;
            OrdQty.HeaderText = "Order Q'ty";
            gv.Columns.Add(OrdQty);
                        
            GridViewTextBoxColumn FabricIdx = new GridViewTextBoxColumn();
            FabricIdx.Name = "FabricIdx";
            FabricIdx.FieldName = "FabricIdx";
            CutSizeIdx.IsVisible = false;
            gv.Columns.Add(FabricIdx);

            GridViewTextBoxColumn FabricNm = new GridViewTextBoxColumn();
            FabricNm.Name = "FabricNm";
            FabricNm.FieldName = "FabricNm";
            FabricNm.Width = 150;
            FabricNm.TextAlignment = ContentAlignment.MiddleLeft;
            FabricNm.HeaderText = "Fabric";
            gv.Columns.Add(FabricNm);
            
            #endregion

        }
        /// <summary>
        /// 그리드뷰 컬럼 생성 (컬러사이즈)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV2_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            gv.MasterTemplate.Columns.Clear();
            gv.DataSource = null;

            GridViewTextBoxColumn cIdx = new GridViewTextBoxColumn();
            cIdx.Name = "Idx";
            cIdx.FieldName = "Idx";
            cIdx.Width = 50;
            cIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cIdx.HeaderText = "ID";
            cIdx.ReadOnly = true;
            cIdx.IsVisible = false; 
            gv.Columns.Add(cIdx);

            GridViewTextBoxColumn cOrderIdx = new GridViewTextBoxColumn();
            cOrderIdx.Name = "OrderIdx";
            cOrderIdx.FieldName = "OrderIdx";
            cOrderIdx.Width = 60;
            cOrderIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cOrderIdx.HeaderText = "Order ID";
            cOrderIdx.ReadOnly = true;
            cOrderIdx.IsVisible = false;
            gv.Columns.Add(cOrderIdx);

            GridViewTextBoxColumn cColorIdx = new GridViewTextBoxColumn();
            cColorIdx.Name = "ColorIdx";
            cColorIdx.FieldName = "ColorIdx";
            cColorIdx.Width = 120;
            cColorIdx.TextAlignment = ContentAlignment.MiddleLeft;
            cColorIdx.HeaderText = "Color";
            gv.Columns.Add(cColorIdx);


            #region Sizes 

            GridViewTextBoxColumn cSizeIdx1 = new GridViewTextBoxColumn();
            cSizeIdx1.Name = "SizeIdx1";
            cSizeIdx1.FieldName = "SizeIdx1";
            cSizeIdx1.IsVisible = false;
            gv.Columns.Add(cSizeIdx1);

            GridViewTextBoxColumn cSizeIdx2 = new GridViewTextBoxColumn();
            cSizeIdx2.Name = "SizeIdx2";
            cSizeIdx2.FieldName = "SizeIdx2";
            cSizeIdx2.IsVisible = false;
            gv.Columns.Add(cSizeIdx2);

            GridViewTextBoxColumn cSizeIdx3 = new GridViewTextBoxColumn();
            cSizeIdx3.Name = "SizeIdx3";
            cSizeIdx3.FieldName = "SizeIdx3";
            cSizeIdx3.IsVisible = false;
            gv.Columns.Add(cSizeIdx3);

            GridViewTextBoxColumn cSizeIdx4 = new GridViewTextBoxColumn();
            cSizeIdx4.Name = "SizeIdx4";
            cSizeIdx4.FieldName = "SizeIdx4";
            cSizeIdx4.IsVisible = false;
            gv.Columns.Add(cSizeIdx4);

            GridViewTextBoxColumn cSizeIdx5 = new GridViewTextBoxColumn();
            cSizeIdx5.Name = "SizeIdx5";
            cSizeIdx5.FieldName = "SizeIdx5";
            cSizeIdx5.IsVisible = false;
            gv.Columns.Add(cSizeIdx5);

            GridViewTextBoxColumn cSizeIdx6 = new GridViewTextBoxColumn();
            cSizeIdx6.Name = "SizeIdx6";
            cSizeIdx6.FieldName = "SizeIdx6";
            cSizeIdx6.IsVisible = false;
            gv.Columns.Add(cSizeIdx6);

            GridViewTextBoxColumn cSizeIdx7 = new GridViewTextBoxColumn();
            cSizeIdx7.Name = "SizeIdx7";
            cSizeIdx7.FieldName = "SizeIdx7";
            cSizeIdx7.IsVisible = false;
            gv.Columns.Add(cSizeIdx7);

            GridViewTextBoxColumn cSizeIdx8 = new GridViewTextBoxColumn();
            cSizeIdx8.Name = "SizeIdx8";
            cSizeIdx8.FieldName = "SizeIdx8";
            cSizeIdx8.IsVisible = false;
            gv.Columns.Add(cSizeIdx8);

            #endregion 

            #region Pcs 

            GridViewDecimalColumn cPcs1 = new GridViewDecimalColumn();
            cPcs1.Name = "pcs1";
            cPcs1.FieldName = "pcs1";
            cPcs1.Width = 60;
            cPcs1.FormatString = "{0:N0}";
            cPcs1.HeaderText = lstSize[1].SizeName;
            gv.Columns.Add(cPcs1);

            GridViewDecimalColumn cPcs2 = new GridViewDecimalColumn();
            cPcs2.Name = "pcs2";
            cPcs2.FieldName = "pcs2";
            cPcs2.Width = 60;
            cPcs2.FormatString = "{0:N0}";
            cPcs2.HeaderText = lstSize[2].SizeName;
            gv.Columns.Add(cPcs2);

            GridViewDecimalColumn cPcs3 = new GridViewDecimalColumn();
            cPcs3.Name = "pcs3";
            cPcs3.FieldName = "pcs3";
            cPcs3.Width = 60;
            cPcs3.FormatString = "{0:N0}";
            cPcs3.HeaderText = lstSize[3].SizeName;
            gv.Columns.Add(cPcs3);

            GridViewDecimalColumn cPcs4 = new GridViewDecimalColumn();
            cPcs4.Name = "pcs4";
            cPcs4.FieldName = "pcs4";
            cPcs4.Width = 60;
            cPcs4.FormatString = "{0:N0}";
            cPcs4.HeaderText = lstSize[4].SizeName;
            gv.Columns.Add(cPcs4);

            GridViewDecimalColumn cPcs5 = new GridViewDecimalColumn();
            cPcs5.Name = "pcs5";
            cPcs5.FieldName = "pcs5";
            cPcs5.Width = 60;
            cPcs5.FormatString = "{0:N0}";
            cPcs5.HeaderText = lstSize[5].SizeName;
            gv.Columns.Add(cPcs5);

            GridViewDecimalColumn cPcs6 = new GridViewDecimalColumn();
            cPcs6.Name = "pcs6";
            cPcs6.FieldName = "pcs6";
            cPcs6.Width = 60;
            cPcs6.FormatString = "{0:N0}";
            cPcs6.HeaderText = lstSize[6].SizeName;
            gv.Columns.Add(cPcs6);

            GridViewDecimalColumn cPcs7 = new GridViewDecimalColumn();
            cPcs7.Name = "pcs7";
            cPcs7.FieldName = "pcs7";
            cPcs7.Width = 60;
            cPcs7.FormatString = "{0:N0}";
            cPcs7.HeaderText = lstSize[7].SizeName;
            gv.Columns.Add(cPcs7);

            GridViewDecimalColumn cPcs8 = new GridViewDecimalColumn();
            cPcs8.Name = "pcs8";
            cPcs8.FieldName = "pcs8";
            cPcs8.Width = 60;
            cPcs8.FormatString = "{0:N0}";
            cPcs8.HeaderText = lstSize[8].SizeName;
            gv.Columns.Add(cPcs8);

            #endregion

            #endregion
        }

        /// <summary>
        /// 그리드뷰 컬럼 생성 (원단수량)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV5_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            gv.MasterTemplate.Columns.Clear();
            gv.DataSource = null;

            GridViewTextBoxColumn cIdx = new GridViewTextBoxColumn();
            cIdx.Name = "Idx";
            cIdx.FieldName = "Idx";
            cIdx.Width = 50;
            cIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cIdx.HeaderText = "ID";
            cIdx.ReadOnly = true;
            cIdx.IsVisible = false;
            gv.Columns.Add(cIdx);

            GridViewTextBoxColumn cOrderIdx = new GridViewTextBoxColumn();
            cOrderIdx.Name = "OrderIdx";
            cOrderIdx.FieldName = "OrderIdx";
            cOrderIdx.Width = 60;
            cOrderIdx.TextAlignment = ContentAlignment.MiddleCenter;
            cOrderIdx.HeaderText = "Order ID";
            cOrderIdx.ReadOnly = true;
            cOrderIdx.IsVisible = false;
            gv.Columns.Add(cOrderIdx);

            GridViewTextBoxColumn FabricIdx = new GridViewTextBoxColumn();
            FabricIdx.Name = "FabricIdx";
            FabricIdx.FieldName = "FabricIdx";
            FabricIdx.HeaderText = "Fabric";
            FabricIdx.IsVisible = false; 
            gv.Columns.Add(FabricIdx);

            GridViewTextBoxColumn FabricNm = new GridViewTextBoxColumn();
            FabricNm.Name = "FabricNm";
            FabricNm.FieldName = "FabricNm";
            FabricNm.HeaderText = "Fabric";
            FabricNm.Width = 200;
            gv.Columns.Add(FabricNm);

            GridViewTextBoxColumn Yds = new GridViewTextBoxColumn();
            Yds.DataType = typeof(double);
            Yds.Name = "Yds";
            Yds.FieldName = "Yds";
            Yds.Width = 70;
            Yds.FormatString = "{0:N2}";
            Yds.TextAlignment = ContentAlignment.MiddleRight;
            Yds.HeaderText = "Yds";
            gv.Columns.Add(Yds);

            GridViewTextBoxColumn Remark = new GridViewTextBoxColumn();
            Remark.Name = "Remark";
            Remark.FieldName = "Remark";
            Remark.Width = 140;
            Remark.TextAlignment = ContentAlignment.MiddleLeft;
            Remark.HeaderText = "Remark";
            gv.Columns.Add(Remark);


            #endregion
        }

        #endregion
        
        #region 4. 메서드 (데이터로딩) 

        /// <summary>
        /// 사이즈그룹 번호로 해당 사이즈 셋을 불러온다 
        /// </summary>
        /// <param name="SizeGroupIdx">사이즈 그룹번호</param>
        private void GetSizes(int SizeGroupIdx)
        {
            _bRtn = false;
            try
            {
                DataRow dr = Codes.Controller.SizeGroup.Get(SizeGroupIdx);

                if (dr != null)
                {
                    lstSize.Add(new Codes.Controller.Sizes(0, ""));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx1"]), dr["Size1"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx2"]), dr["Size2"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx3"]), dr["Size3"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx4"]), dr["Size4"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx5"]), dr["Size5"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx6"]), dr["Size6"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx7"]), dr["Size7"].ToString()));
                    lstSize.Add(new Codes.Controller.Sizes(Convert.ToInt32(dr["SizeIdx8"]), dr["Size8"].ToString()));
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Dropdownlist 데이터 로딩
        /// </summary>
        private void DataLoading_DDL()
        {
            try
            {
                // Fabric 
                _dt = Dev.Controller.OrderFabric.Getlist(_orderIdx).Tables[0];

                foreach (DataRow row in _dt.Rows)
                {
                    lstFabric.Add(new CodeContents(Convert.ToInt32(row["Idx"]), row["FabricNm"].ToString(), ""));
                }

                //// Username
                //lstUser.Add(new CustomerName(0, "", 0));
                //_dt = CommonController.Getlist(CommonValues.KeyName.User).Tables[0];

                //foreach (DataRow row in _dt.Rows)
                //{
                //    lstUser.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]),
                //                                row["UserName"].ToString(),
                //                                Convert.ToInt32(row["DeptIdx"])));
                //}

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }

        }

        /// <summary>
        /// 데이터 로딩 (컬러사이즈) 
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="OrderIdx"></param>
        private void DataBinding_GV2(RadGridView gv, int OrderIdx)
        {
            try
            {
                gv.DataSource = null;

                _ds1 = Controller.OrderColor.Getlist(OrderIdx);
                if (_ds1 != null)
                {
                    gv.DataSource = _ds1.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBinding " + gv.Name + ": " + ex.Message.ToString());
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvColorSize.SelectedCells.Count<=0)
                {
                    RadMessageBox.Show("Please select the color or size q'ty.");
                    return; 
                }
                if (gvFabric.SelectedRows.Count <= 0)
                {
                    RadMessageBox.Show("Please select the fabric.");
                    return;
                }
                
                foreach (GridViewRowInfo rowFabric in gvFabric.SelectedRows)
                {
                    foreach (GridViewCellInfo cellColor in gvColorSize.SelectedCells)
                    {
                        GridViewRowInfo rowColor = cellColor.RowInfo;
                        
                        // 사이즈수량 컬럼이면서 값이 존재하는 경우만
                        if (cellColor.ColumnInfo.Index > 2 && Convert.ToInt32(cellColor.Value) > 0)
                        {
                            gvCutting.Rows.Add(rowColor.Cells["ColorIdx"].Value, lstSize[cellColor.ColumnInfo.Index - 10].SizeIdx,
                                            lstSize[cellColor.ColumnInfo.Index - 10].SizeName,
                                            Convert.ToInt32(cellColor.Value), Convert.ToInt32(rowFabric.Cells["FabricIdx"].Value), rowFabric.Cells["FabricNm"].Value);
                        }
                    }
                }
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
        }

        /// <summary>
        /// 데이터 로딩 (원단) 
        /// </summary>
        /// <param name="gv"></param>
        /// <param name="OrderIdx"></param>
        private void DataBinding_GV5(RadGridView gv, int OrderIdx)
        {
            try
            {
                gv.DataSource = null;

                _ds1 = Dev.Controller.OrderFabric.Getlist(OrderIdx);
                if (_ds1 != null)
                {
                    gv.DataSource = _ds1.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBinding " + gv.Name + ": " + ex.Message.ToString());
            }
        }
        
        #endregion
        
        #region 5. 메서드 (데이터 신규/업데이트) 
        
        /// <summary>
        /// 재단 요청 버튼 클릭 (업데이트) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = null;
                string NewCode = ""; 

                if (gvCutting.Rows.Count <= 0)
                {
                    RadMessageBox.Show("Please select the Color/Size Q'ty and the Fabric", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }
                                
                // 데이터 DB저장 
                foreach (GridViewRowInfo row in gvCutting.Rows)
                {
                    NewCode = Code.GetPrimaryCode(UserInfo.CenterIdx, UserInfo.DeptIdx, 6, "");

                    if (!string.IsNullOrEmpty(NewCode))
                    {
                        dr = Dev.Controller.Cutting.Insert(_orderIdx, NewCode, row.Cells["cColorIdx"].Value.ToString().Trim(),
                            Convert.ToInt32(row.Cells["CutSizeIdx"].Value), 0f,
                            Convert.ToInt32(row.Cells["OrdQty"].Value),
                            Convert.ToInt32(row.Cells["FabricIdx"].Value), txtComment.Text, UserInfo.Idx);
                    }
                }

                // 입력완료 후 그리드뷰 갱신
                DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                Console.WriteLine("btnUpdate_Click: " + ex.Message.ToString());
            }
        }

        #endregion
        
        #region 6. 이벤트

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
