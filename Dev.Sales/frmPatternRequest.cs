﻿using Dev.Options;
using Int.Code;
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
    public partial class frmPatternRequest : Telerik.WinControls.UI.RadForm
    {
        #region 변수 선언
        
        private string _fileNo, _styleNo = "";
        private int _orderIdx,  _orderStatus, _sizeGroup = 0;
        private bool _bRtn = false;
        private List<Codes.Controller.Sizes> lstSize = new List<Codes.Controller.Sizes>();

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
        public frmPatternRequest(int idx, string fileNo, string styleno, int sizeGroup, int orderStatus)
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
            GetSizes(_sizeGroup);
            ddlSize.DataSource = lstSize;
            ddlSize.DisplayMember = "sizeName";
            ddlSize.ValueMember = "sizeIdx";

            beFiles.DialogType = BrowseEditorDialogType.OpenFileDialog;
            if (beFiles.DialogType == BrowseEditorDialogType.OpenFileDialog)
            {
                OpenFileDialog dialog = (OpenFileDialog)beFiles.Dialog;
                dialog.Multiselect = true;
            }
            
        }

        private void beFiles_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlgOpen = new OpenFileDialog())
            {
                dlgOpen.Title = "Select Pattern Files";
                dlgOpen.Multiselect = true; 
                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    listFiles.Items.Clear();
                    for (int i=0; i<dlgOpen.FileNames.Length; i++)
                    {
                        FileOpen_ListView(dlgOpen.FileNames[i], listFiles); 
                    }
                }
            }
        }

        private void FileOpen_ListView(string fileName, RadListView lstFiles)
        {
            if (File.Exists(fileName))
            {
                listFiles.Items.Add(fileName); 
            }
        }
        #endregion

        #region 바인딩 & 이벤트

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                string NewCode = Code.GetPrimaryCode(UserInfo.CenterIdx, UserInfo.DeptIdx, 5, _fileNo);

                if (Convert.ToInt32(ddlSize.SelectedValue) <= 0)
                {
                    RadMessageBox.Show("Please input the Size", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }

                if (!string.IsNullOrEmpty(NewCode))
                {
                    DataRow dr = Dev.Controller.Pattern.Insert(_orderIdx, NewCode, Convert.ToInt32(ddlSize.SelectedValue),
                    dtTechPack.Value, DateTime.Today, UserInfo.Idx, txtComment.Text,
                    "", "", "", "", "",
                    "", "", "", "", "", UserInfo.Idx);

                    if (dr != null)
                    {
                        // 입력완료 후 그리드뷰 갱신
                        
                        DialogResult = System.Windows.Forms.DialogResult.OK;
                    }
                    else
                    {
                        //lblResult.Text = "Failed to input the data.";
                    }
                }
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
