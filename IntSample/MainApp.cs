using Dev.Codes;
using Dev.Fabric;
using Dev.Options;
using Dev.Pattern;
using Dev.Sales;
using Dev.WorkOrder;
using Dev.Yarn;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace SampleApp
{
    public partial class MainApp : InheritMDI 
    {
        public MainApp()
        {
            InitializeComponent();
            this.Text = "INT Sample Management"; 
            this.radRibbonBar1.QuickAccessToolBar.InnerItem.Fill.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.WindowState = FormWindowState.Maximized;
        }

        private void bsExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bsOptions_Click(object sender, EventArgs e)
        {
            // 설정 버튼 클릭시 파일 없으면 새로 만들고 있으면 내용 불러옴 
            txtUsername.Text = UserInfo.Username;

            string url = Environment.CurrentDirectory + "/conf";
            string path = url + "/conf.json";

            try
            {
                CheckFolder(url);

                // 설정파일이 있으면 내용을 불러옴
                if (System.IO.File.Exists(path))
                {
                    string confStr = System.IO.File.ReadAllText(path);

                    ConfigStruct cs = new ConfigStruct();
                    cs = JsonConvert.DeserializeObject<ConfigStruct>(confStr);

                    enablePagingGV.Checked = cs.enablePaging;
                    enableSearchRow.Checked = cs.enableSearchRow;
                }
                // 없으면 기본값으로 새로 만듬 
                else
                {
                    ConfigStruct cs = new ConfigStruct
                    {
                        enablePaging = true,
                        enableSearchRow = false,
                    };

                    string confStr = JsonConvert.SerializeObject(cs, Formatting.Indented);
                    System.IO.File.WriteAllText(path, confStr);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void bsExport_Click(object sender, EventArgs e)
        {

        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            // 그리드뷰 파일로 내보내기 저장 
            try
            {
                InheritForm activeChild = this.ActiveMdiChild as InheritForm;

                if (activeChild != null)
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "Excel file|*.xlsx";
                    saveFileDialog.Title = "Export to Excel File";
                    saveFileDialog.ShowDialog();

                    if (!string.IsNullOrEmpty(saveFileDialog.FileName))
                    {
                        //Console.WriteLine(saveFileDialog.FileName);
                        Telerik.WinControls.Export.GridViewSpreadExport spreadExporter = new Telerik.WinControls.Export.GridViewSpreadExport(activeChild._gv1);
                        Telerik.WinControls.Export.SpreadExportRenderer exportRenderer = new Telerik.WinControls.Export.SpreadExportRenderer();
                        spreadExporter.RunExport(saveFileDialog.FileName, exportRenderer);

                        RadMessageBox.Show("Exported file");
                    }
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            try
            {
                string origin = originalPass.Text.Trim().ToLower();
                string newpass = newPass.Text.Trim().ToLower();
                string rePass = retypePass.Text.Trim().ToLower();

                if (string.IsNullOrEmpty(origin))
                {
                    RadMessageBox.Show("Please input the Original Password", "Error", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    return;
                }
                if (origin != UserInfo.Password)
                {
                    RadMessageBox.Show("Invalid original password", "Error", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    return;
                }
                if (string.IsNullOrEmpty(newpass))
                {
                    RadMessageBox.Show("Please input new password", "Error", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    return;
                }
                if (string.IsNullOrEmpty(rePass))
                {
                    RadMessageBox.Show("Please input re-type password", "Error", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    return;
                }
                if (newpass != rePass)
                {
                    RadMessageBox.Show("Invalid new password", "Error", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                    return;
                }

                bool bResult = Data.LoginData.ModifyPassword(UserInfo.Idx, newpass);

                if (bResult)
                {
                    RadMessageBox.Show("Password modified", "Info", MessageBoxButtons.OK, RadMessageIcon.Info);
                    UserInfo.Password = newpass;
                }
                else
                {
                    RadMessageBox.Show("Failed to modify", "Error", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message.ToString());
            }
        }


        private void bsViewMain_Leave(object sender, EventArgs e)
        {
            try
            {
                // 백스테이지 나갈때 설정내용 저장 
                string url = Environment.CurrentDirectory + "/conf";
                string path = url + "/conf.json";

                SetDirectorySecurity(url);
                ConfigStruct cs = new ConfigStruct();

                cs.enablePaging = enablePagingGV.Checked;
                cs.enableSearchRow = enableSearchRow.Checked;

                string confStr = JsonConvert.SerializeObject(cs, Formatting.Indented);
                System.IO.File.WriteAllText(path, confStr);

                CommonValues.enablePaging = enablePagingGV.Checked;
                CommonValues.enableSearchRow = enableSearchRow.Checked;

            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }

        private void MainApp_Load(object sender, EventArgs e)
        {
            bool __TESTMODE__ = false;

            if (!__TESTMODE__)
            {
                if (UserInfo.Idx <= 0)
                {
                    MainLogin loginWnd = new MainLogin();
                    loginWnd.ShowDialog();
                    lblServerInfo.Text = "Connected to " + Int.Members.IntSampleConnectionString.Substring(12, 20);
                }

                // 시작시 설정내용을 불러와 commonvalue에 저장               
                string url = Environment.CurrentDirectory + "/conf";
                string path = url + "/conf.json";

                try
                {
                    CheckFolder(url);

                    if (System.IO.File.Exists(path))
                    {
                        string confStr = System.IO.File.ReadAllText(path);

                        ConfigStruct cs = new ConfigStruct();
                        cs = JsonConvert.DeserializeObject<ConfigStruct>(confStr);
                        CommonValues.enablePaging = cs.enablePaging;
                        CommonValues.enableSearchRow = cs.enableSearchRow;

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        #region Methods 

        private bool Close_All_Children(string frm)
        {
            bool iRtn = true;

            foreach (Form f in this.MdiChildren)
            {
                if (f.Name == frm.ToString())
                {

                    f.Activate();
                    iRtn = false;
                }
                // f.Close();
            }

            return iRtn;
        }

        private RadForm Find_Children(string frm)
        {
            foreach (RadForm f in this.MdiChildren)
            {
                if (f.Name == frm.ToString())
                {
                    return f; 
                }
            }
            return null; 
        }

        private void CheckFolder(string sPath)
        {
            // 폴더 유무확인 및 생성 
            DirectoryInfo di = new DirectoryInfo(sPath);
            if (di.Exists == false)
            {
                di.Create();
            }
        }

        private void SetDirectorySecurity(string linePath)
        {
            DirectorySecurity dSecurity = Directory.GetAccessControl(linePath);
            var sid = new SecurityIdentifier(WellKnownSidType.AuthenticatedUserSid, null);

            dSecurity.AddAccessRule(new FileSystemAccessRule(sid,
                                FileSystemRights.FullControl,
                                InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit,
                                PropagationFlags.None,
                                AccessControlType.Allow));

            Directory.SetAccessControl(linePath, dSecurity);
        }

        #endregion

        private void btnOrderOrder_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("OrderMain"))
            {
                OrderMain frm = new OrderMain(this);
                frm.Text = "Orders";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnSystemColor_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("CodeColor"))
            {
                CodeColor frm = new CodeColor(this);
                frm.Text = "Colors";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnSize_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("CodeSize"))
            {
                CodeSize frm = new CodeSize(this);
                frm.Text = "Sizes";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnSewingThread_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("CodeSewThread"))
            {
                CodeSewThread frm = new CodeSewThread(this);
                frm.Text = "Sewing Thread";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnSizeGroup_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("CodeSizeGroup"))
            {
                CodeSizeGroup frm = new CodeSizeGroup(this);
                frm.Text = "Size Group";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnPatternMain_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("PatternMain"))
            {
                PatternMain frm = new PatternMain(this, "");
                frm.Text = "Pattern Main";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnOrderWorkSchedule_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("SampleScheduler"))
            {
                SampleScheduler frm = new SampleScheduler(this, 0);
                frm.Text = "Developement Schedule";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnOrderWorkOrder_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("WorkOrderMain"))
            {
                WorkOrderMain frm = new WorkOrderMain(this, "");
                frm.Text = "Work Order";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnOrderReportTicketPrint_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("WorkOrderTicketPrint"))
            {
                if (CommonValues.ListWorkID.Count>0)
                {
                    WorkOrderTicketPrint frm = new WorkOrderTicketPrint();
                    frm.Text = "Print Work Ticket";
                    frm.MdiParent = this;
                    frm.Show();
                }
                else
                {
                    RadMessageBox.Show("There's no selected Work Orders. \nPlease select Work Orders.", "Notice", 
                            MessageBoxButtons.OK, RadMessageIcon.Error);
                    return; 
                }
                
            }
        }

        private void btnYarnCodeMain_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("YarnCode"))
            {
                YarnCode frm = new YarnCode(this);
                frm.Text = "Yarn Code";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnFabricFabricCode_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("FabricCode"))
            {
                FabricCode frm = new FabricCode(this);
                frm.Text = "Fabric Code";
                frm.MdiParent = this;
                frm.Show();
            }
            
        }

        private void btnFabricInbound_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("Inbound"))
            {
                Inbound frm = new Inbound(this);
                frm.Text = "Fabric Inbound";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnFabricOutbound_Click(object sender, EventArgs e)
        {

        }
    }
}
