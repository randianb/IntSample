using Dev.Options;
using Int.Code;
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
using Telerik.WinForms.Documents.Model;

namespace Dev.Sales
{
    public partial class NewWorkSheets : Telerik.WinControls.UI.RadForm
    {
        #region 변수 선언
        
        private string _fileNo, _styleNo = "";
        private int _orderIdx,  _orderStatus, _sizeGroup = 0;
        private bool _bRtn = false;
        private List<Codes.Controller.Sizes> lstSize = new List<Codes.Controller.Sizes>();
        private List<string> lstFiles = new List<string>();
        private List<string> lstFiles2 = new List<string>();
        private List<string> lstFileUrls = new List<string>();
        private List<string> lstFileUrls2 = new List<string>();

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
        public NewWorkSheets(int idx, string fileNo, string styleno, int sizeGroup, int orderStatus)
        {
            InitializeComponent();
            _orderIdx = idx; 
            _fileNo = fileNo;
            _styleNo = styleno;
            _sizeGroup = sizeGroup;
            _orderStatus = orderStatus; 

            lblFileno.Text = fileNo;
            lblStyle.Text = styleno;
            radLabel5.Text = DateTime.Today.ToString(); 

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
            //ddlSize.DataSource = lstSize;
            //ddlSize.DisplayMember = "sizeName";
            //ddlSize.ValueMember = "sizeIdx";

            beFiles.DialogType = BrowseEditorDialogType.OpenFileDialog;
            if (beFiles.DialogType == BrowseEditorDialogType.OpenFileDialog)
            {
                OpenFileDialog dialog = (OpenFileDialog)beFiles.Dialog;
                dialog.Multiselect = true;
            }
            SetDefaultFontPropertiesToEditor(txtComment);
        }

        public void SetDefaultFontPropertiesToEditor(RadRichTextEditor editor)
        {
            editor.Document.Selection.SelectAll();
            editor.RichTextBoxElement.ChangeFontFamily(new Telerik.WinControls.RichTextEditor.UI.FontFamily("Segoe UI"));
            editor.RichTextBoxElement.ChangeFontSize(Unit.PointToDip(9));
            editor.RichTextBoxElement.ChangeFontStyle(Telerik.WinControls.RichTextEditor.UI.FontStyles.Normal);
            editor.RichTextBoxElement.ChangeFontWeight(Telerik.WinControls.RichTextEditor.UI.FontWeights.Normal);

            editor.RichTextBoxElement.ChangeParagraphLineSpacingType(LineSpacingType.Auto);
            editor.RichTextBoxElement.ChangeParagraphLineSpacing(1);
            editor.RichTextBoxElement.ChangeParagraphSpacingAfter(12);

            editor.DocumentInheritsDefaultStyleSettings = true;

            Telerik.WinForms.Documents.DocumentPosition startPosition = editor.Document.CaretPosition;
            Telerik.WinForms.Documents.DocumentPosition endPosition = new Telerik.WinForms.Documents.DocumentPosition(startPosition);
            startPosition.MoveToCurrentWordEnd();
            endPosition.MoveToCurrentWordEnd();
            editor.Document.Selection.AddSelectionStart(startPosition);
            editor.Document.Selection.AddSelectionEnd(endPosition);
        }

        #endregion

        #region 바인딩 & 이벤트

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 파일 선택창을 열고 선택된 파일을 리스트에 추가한다. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void beFiles_Click(object sender, EventArgs e)
        {
            //using (OpenFileDialog dlgOpen = new OpenFileDialog())
            //{
            //    dlgOpen.Title = "Select Pattern Files";
            //    dlgOpen.Multiselect = true;
            //    if (dlgOpen.ShowDialog() == DialogResult.OK)
            //    {
            //        listFiles.Items.Clear();
            //        for (int i = 0; i < dlgOpen.FileNames.Length; i++)
            //        {
            //            FileOpen_ListView(dlgOpen.FileNames[i], listFiles);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 리스트에 선택한 파일목록 추가 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="lstFiles"></param>
        private void FileOpen_ListView(string fileName, RadListView lstFiles)
        {
            if (File.Exists(fileName))
            {
                listFiles.Items.Add(fileName);
            }
        }

        /// <summary>
        /// 패턴 요청 버튼 클릭 (파일 및 데이터 저장) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRequest_Click(object sender, EventArgs e)
        {
            try
            {
                string NewCode = Code.GetPrimaryCode(Options.UserInfo.CenterIdx, Options.UserInfo.ReportNo, 3, _fileNo);

                //if (Convert.ToInt32(ddlSize.SelectedValue) <= 0)
                //{
                //    RadMessageBox.Show("Please input the Size", "Error", MessageBoxButtons.OK, RadMessageIcon.Error);
                //    return;
                //}

                if (!string.IsNullOrEmpty(NewCode))
                {
                    for (int i=0; i<=8; i++)
                    {
                        lstFiles2.Add(""); lstFileUrls2.Add(""); 
                    }
                    if (lstFiles.Count>0)
                    {
                        for (int i = 0; i < lstFiles.Count; i++)
                        {
                            if (!string.IsNullOrEmpty(lstFiles[i])) lstFiles2[i] = lstFiles[i]; lstFileUrls2[i] = lstFileUrls[i];
                        }
                    }
                    
                    DataRow dr = Dev.Controller.Worksheet.Insert(_orderIdx, NewCode, txtComment.Text,
                    lstFiles2[0].ToString(), lstFiles2[1].ToString(), lstFiles2[2].ToString(), lstFiles2[3].ToString(), lstFiles2[4].ToString(),
                    lstFiles2[5].ToString(), lstFiles2[6].ToString(), lstFiles2[7].ToString(), lstFiles2[8].ToString(),
                    lstFileUrls2[0].ToString(), lstFileUrls2[1].ToString(), lstFileUrls2[2].ToString(), lstFileUrls2[3].ToString(), lstFileUrls2[4].ToString(),
                    lstFileUrls2[5].ToString(), lstFileUrls2[6].ToString(), lstFileUrls2[7].ToString(), lstFileUrls2[8].ToString(),
                    Options.UserInfo.Idx);


                    // 데이터 DB저장 
                    

                    if (dr != null)
                    {
                        RadMessageBox.Show("Created WorkSheet", "Saved");
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

        private void beFiles_ValueChanged(object sender, EventArgs e)
        {

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

        #region 파일 다운로드 

        /// <summary>
        /// azure storage 파일 업로드
        /// </summary>
        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {    
                //bool result = Data.UpdateData.DeleteAll(_selectedNode);

                // 스토리지 설정 
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // 선택된 패키지의 폴더안의 blob 리스트 조회 (updateinterp, updateintsample, ...) 
                CloudBlobContainer container = blobClient.GetContainerReference(CommonValues.packageName + "worksheet");
                
                string[] fileNames = GetFiles();
                
                if (fileNames != null)
                {
                    foreach (string filename in fileNames)
                    {
                        // 업데이트 파일 storage저장 
                        using (var fileStream = System.IO.File.OpenRead(filename))
                        {
                            // blob명은 파일명과 같도록 생성
                            CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename.Substring(filename.LastIndexOf("\\") + 1));

                            blockBlob.UploadFromStream(fileStream);

                            lstFiles.Add(filename.Substring(filename.LastIndexOf("\\") + 1));
                            lstFileUrls.Add(blockBlob.StorageUri.PrimaryUri.ToString()); 
                            
                        }

                    }
                            
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        /// <summary>
        /// 다중 파일 선택
        /// </summary>
        /// <returns>string[] filenames</returns>
        private string[] GetFiles()
        {
            string[] fileNames;
            OpenFileDialog openDialog = new OpenFileDialog();

            openDialog.Filter = "All files|*.*"; 
            openDialog.Title = "Select files to upload";
            openDialog.RestoreDirectory = false;
            openDialog.Multiselect = true;
            openDialog.CheckFileExists = false;

            try
            {
                DialogResult result = openDialog.ShowDialog();

                if (result == DialogResult.OK && openDialog.FileNames.Length <= 9)
                {
                    // 리스트 초기화 
                    // listFiles.Items.Clear();
                    
                    for (int i = 0; i < openDialog.FileNames.Length; i++)
                    {
                        FileOpen_ListView(openDialog.FileNames[i], listFiles);
                    }
                   
                    return fileNames = openDialog.FileNames;
                    
                }
                else if (result == DialogResult.Cancel)
                {
                    return null;
                }
                else
                {
                    if (MessageBox.Show("Too many files were Selected. Please select files less than 9.",
                        "Too many files...", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        return null;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                return null;
            }

        }

        #endregion 
    }
}
