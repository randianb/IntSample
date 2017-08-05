using Dev.Options;
using Int.Code;
using Int.Customer;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Dev.Sales
{
    public partial class frmInspectingRequest : Telerik.WinControls.UI.RadForm
    {
        #region 변수 선언

        private DataTable _dt = null;                                           // 기본 데이터테이블
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        
        public Dictionary<CommonValues.KeyName, int> _searchKey;                // 쿼리값 value, 쿼리항목 key로 전달 

        private string _fileNo, _styleNo = "";
        private int _orderIdx,  _orderStatus, _sizeGroup = 0;
        private bool _bRtn = false;
        private List<Codes.Controller.Sizes> lstSize = new List<Codes.Controller.Sizes>();
        private List<CustomerName> lstUser = new List<CustomerName>();         // 거래처

        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 43, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        #endregion

        #region 초기화

        /// <summary>
        /// parent로부터 order 정보 받아오기 
        /// </summary>
        /// <param name="idx">고유번호</param>
        /// <param name="fileNo">파일번호</param>
        /// <param name="qty">수량</param>
        /// <param name="amount">금액</param>
        /// <param name="shipCompleted">선적완료 여부</param>
        public frmInspectingRequest(int idx, string fileNo, string styleno, int sizeGroup, int orderStatus)
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

            this.StartPosition = FormStartPosition.CenterScreen;
            
            // 선적완료, 오더마감 여부에 따라 편집 disable 
            if (_orderStatus==2 || _orderStatus==3)
            {
                tableLayoutPanel2.Enabled = false; 
            }
        }

        private void frmPatternRequest_Load(object sender, EventArgs e)
        {
            DataLoading_DDL(); 
            GV1_CreateColumn(gvCutted);
            RefleshWithCondition();
            GV2_CreateColumn(gvOutSource);
            //GetSizes(_sizeGroup);
            //ddlSize.DataSource = lstSize;
            //ddlSize.DisplayMember = "sizeName";
            //ddlSize.ValueMember = "sizeIdx";

            //beFiles.DialogType = BrowseEditorDialogType.OpenFileDialog;
            //if (beFiles.DialogType == BrowseEditorDialogType.OpenFileDialog)
            //{
            //    OpenFileDialog dialog = (OpenFileDialog)beFiles.Dialog;
            //    dialog.Multiselect = true;
            //}

        }

        /// <summary>
        /// 그리드뷰 컬럼 생성 (봉제완료내역)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV1_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            GridViewTextBoxColumn Idx = new GridViewTextBoxColumn();
            Idx.DataType = typeof(int);
            Idx.Name = "Idx";
            Idx.FieldName = "Idx";
            Idx.HeaderText = "ID";
            Idx.Width = 40;
            Idx.ReadOnly = true;
            Idx.TextAlignment = ContentAlignment.MiddleLeft;
            gv.Columns.Add(Idx);
            
            GridViewTextBoxColumn WorkOrderIdx = new GridViewTextBoxColumn();
            WorkOrderIdx.Name = "WorkOrderIdx";
            WorkOrderIdx.FieldName = "WorkOrderIdx";
            WorkOrderIdx.HeaderText = "Work ID";
            WorkOrderIdx.Width = 100;
            WorkOrderIdx.ReadOnly = true;
            WorkOrderIdx.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(WorkOrderIdx);

            GridViewTextBoxColumn OrdColorIdx = new GridViewTextBoxColumn();
            OrdColorIdx.Name = "OrdColorIdx";
            OrdColorIdx.FieldName = "OrdColorIdx";
            OrdColorIdx.HeaderText = "Color";
            OrdColorIdx.Width = 150;
            OrdColorIdx.ReadOnly = true;
            OrdColorIdx.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(OrdColorIdx);

            GridViewComboBoxColumn cboSize = new GridViewComboBoxColumn();
            cboSize.Name = "OrdSizeIdx";
            cboSize.FieldName = "OrdSizeIdx";
            cboSize.HeaderText = "Size";
            cboSize.ReadOnly = true;
            cboSize.Width = 70;
            gv.Columns.Add(cboSize);

            GridViewTextBoxColumn SizeIdx = new GridViewTextBoxColumn();
            SizeIdx.Name = "SizeIdx";
            SizeIdx.FieldName = "SizeIdx";
            SizeIdx.IsVisible = false;
            gv.Columns.Add(SizeIdx);
            
            GridViewDateTimeColumn WorkDate = new GridViewDateTimeColumn();
            WorkDate.Name = "WorkDate";
            WorkDate.FieldName = "WorkDate";
            WorkDate.Width = 100;
            WorkDate.TextAlignment = ContentAlignment.MiddleCenter;
            WorkDate.CustomFormat = "{d}";
            WorkDate.FormatString = "{0:d}";
            WorkDate.HeaderText = "Worked Date";
            gv.Columns.Add(WorkDate);
            
            GridViewTextBoxColumn WorkQty = new GridViewTextBoxColumn();
            WorkQty.Name = "WorkQty";
            WorkQty.FieldName = "WorkQty";
            WorkQty.HeaderText = "Sew Q'ty";
            WorkQty.Width = 70;
            WorkQty.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            gv.Columns.Add(WorkQty);
            
            GridViewTextBoxColumn Remarks = new GridViewTextBoxColumn();
            Remarks.Name = "Remarks";
            Remarks.FieldName = "Remarks";
            Remarks.HeaderText = "Remarks";
            Remarks.Width = 150;
            gv.Columns.Add(Remarks);
            
            #endregion
        }

        /// <summary>
        /// 그리드뷰 컬럼 생성 (검사요청내역)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV2_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            GridViewTextBoxColumn SewIdx = new GridViewTextBoxColumn();
            SewIdx.Name = "SewIdx";
            SewIdx.FieldName = "SewIdx";
            SewIdx.HeaderText = "Sew ID";
            SewIdx.Width = 40;
            SewIdx.ReadOnly = true;
            SewIdx.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(SewIdx);

            GridViewTextBoxColumn OrdColorIdx = new GridViewTextBoxColumn();
            OrdColorIdx.Name = "OrdColorIdx";
            OrdColorIdx.FieldName = "OrdColorIdx";
            OrdColorIdx.HeaderText = "Color";
            OrdColorIdx.Width = 150;
            OrdColorIdx.ReadOnly = true;
            OrdColorIdx.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            gv.Columns.Add(OrdColorIdx);

            GridViewComboBoxColumn cboSize = new GridViewComboBoxColumn();
            cboSize.Name = "OrdSizeIdx";
            cboSize.FieldName = "OrdSizeIdx";
            cboSize.HeaderText = "Size";
            cboSize.ReadOnly = true;
            cboSize.Width = 70;
            gv.Columns.Add(cboSize);

            GridViewTextBoxColumn SizeIdx = new GridViewTextBoxColumn();
            SizeIdx.Name = "SizeIdx";
            SizeIdx.FieldName = "SizeIdx";
            SizeIdx.IsVisible = false;
            gv.Columns.Add(SizeIdx);

            GridViewTextBoxColumn OrdQty = new GridViewTextBoxColumn();
            OrdQty.Name = "OrdQty";
            OrdQty.FieldName = "OrdQty";
            OrdQty.Width = 80;
            OrdQty.TextAlignment = ContentAlignment.MiddleRight;
            OrdQty.HeaderText = "Insp. Q'ty";
            gv.Columns.Add(OrdQty);

            GridViewComboBoxColumn TDName = new GridViewComboBoxColumn();
            TDName.Name = "TDName";
            TDName.DataSource = lstUser;
            TDName.DisplayMember = "CustName";
            TDName.ValueMember = "CustIdx";
            TDName.FieldName = "TDName";
            TDName.HeaderText = "TD Name";
            TDName.TextAlignment = ContentAlignment.MiddleCenter;
            TDName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            TDName.DropDownStyle = RadDropDownStyle.DropDown;
            TDName.Width = 200;
            gv.Columns.Add(TDName);
            
            #endregion
        }

        /// <summary>
        /// Dropdownlist 데이터 로딩
        /// </summary>
        private void DataLoading_DDL()
        {
            try
            {
                // Cutted 
                //_dt = Dev.Controller.OrderFabric.Getlist(_orderIdx).Tables[0];

                //foreach (DataRow row in _dt.Rows)
                //{
                //    lstFabric.Add(new CodeContents(Convert.ToInt32(row["Idx"]), row["FabricNm"].ToString(), ""));
                //}

                // Username
                lstUser.Add(new CustomerName(0, "", 0));
                _dt = CommonController.Getlist(CommonValues.KeyName.TDUser).Tables[0];

                foreach (DataRow row in _dt.Rows)
                {
                    lstUser.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]),
                                                row["UserName"].ToString(),
                                                Convert.ToInt32(row["DeptIdx"])));
                }

                //// Embelishment
                //_dt = CommonController.Getlist(CommonValues.KeyName.Vendor).Tables[0];

                //foreach (DataRow row in _dt.Rows)
                //{
                //    custName.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                //                                row["CustName"].ToString(),
                //                                Convert.ToInt32(row["Classification"])));
                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        /// <summary>
        /// 메인 검색버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            RefleshWithCondition();
        }

        /// <summary>
        /// 검색조건을 이용한 자료 새로고침
        /// </summary>
        private void RefleshWithCondition()
        {
            try
            {
                if (_orderIdx>0)
                {
                    DataBinding_GV1();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

        }

        /// <summary>
        /// 데이터 로딩 (메인)
        /// </summary>
        private void DataBinding_GV1()
        {
            try
            {
                gvCutted.DataSource = null;

                _ds1 = Dev.Controller.Sewing.Getlist(_orderIdx);
                if (_ds1 != null)
                {
                    gvCutted.DataSource = _ds1.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBindingGv1: " + ex.Message.ToString());
            }
        }

        #endregion

        #region 바인딩 & 이벤트

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            try
            {
                if (gvCutted.SelectedRows.Count <= 0)
                {
                    RadMessageBox.Show("Please select the cutted q'ty.");
                    return;
                }                
                
                foreach (GridViewRowInfo rowColor in gvCutted.SelectedRows)
                {
                    //
                    gvOutSource.Rows.Add(rowColor.Cells["Idx"].Value, rowColor.Cells["OrdColorIdx"].Value, rowColor.Cells["OrdSizeIdx"].Value, 
                                    rowColor.Cells["SizeIdx"].Value, Convert.ToInt32(rowColor.Cells["WorkQty"].Value));
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 그리드뷰 셀 생성후, DDL설정 (메인)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MasterTemplate_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            try
            {
                //RadMultiColumnComboBoxElement meditor = e.ActiveEditor as RadMultiColumnComboBoxElement;

                //if (meditor != null)
                //{
                //    meditor.Enabled = false;
                //}

                // DDL 높이, 출력항목수 설정
                RadDropDownListEditor editor = this.gvOutSource.ActiveEditor as RadDropDownListEditor;
                if (editor != null)
                {
                    ((RadDropDownListEditorElement)((RadDropDownListEditor)this.gvOutSource.ActiveEditor).EditorElement).DefaultItemsCountInDropDown
                        = CommonValues.DDL_DefaultItemsCountInDropDown;
                    ((RadDropDownListEditorElement)((RadDropDownListEditor)this.gvOutSource.ActiveEditor).EditorElement).DropDownHeight
                        = CommonValues.DDL_DropDownHeight;
                }

                //// 날짜컬럼의 달력크기 설정
                //RadDateTimeEditor dtEditor = e.ActiveEditor as RadDateTimeEditor;
                //if (dtEditor != null)
                //{
                //    RadDateTimeEditorElement el = dtEditor.EditorElement as RadDateTimeEditorElement;
                //    //el.NullDate = new DateTime(2000, 1, 1);
                //    //el.NullText = "";
                //    el.CalendarSize = new Size(500, 400);

                //    if (el.Value.ToString().Length > 10)
                //    {
                //        Console.WriteLine(el.Value.ToString().Substring(0, 10));
                //        if (el.Value.ToString().Substring(0, 10) == "2000-01-01")
                //        {
                //            el.Value = Convert.ToDateTime(null);
                //        }
                //    }

                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// 외주 요청 버튼 클릭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = null;
                string NewCode = "";

                if (gvOutSource.Rows.Count <= 0)
                {
                    RadMessageBox.Show("Please select the sew Q'ty", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }

                // 데이터 DB저장 
                foreach (GridViewRowInfo row in gvOutSource.Rows)
                {
                    NewCode = Code.GetPrimaryCode(UserInfo.CenterIdx, UserInfo.DeptIdx, 10, "");

                    if (!string.IsNullOrEmpty(NewCode))
                    {
                        dr = Dev.Controller.Inspecting.Insert(_orderIdx, Convert.ToInt32(row.Cells["SewIdx"].Value), NewCode, Convert.ToInt32(row.Cells["TDName"].Value), 
                            DateTime.Now, "", "", 0, 
                            UserInfo.Idx);
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

        public int OrderIdx
        {
            get { return _orderIdx; }
        }

        #endregion

        #region 메서드 

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

        #endregion
        
    }
}
