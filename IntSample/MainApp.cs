using Dev.Codes;
using Dev.Fabric;
using Dev.Options;
using Dev.Pattern;
using Dev.Production;
using Dev.Sales;
using Dev.WorkOrder;
using Dev.Yarn;
using Int.Customer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
        private List<CustomerName> lstBuyer = new List<CustomerName>();
        private DataTable _dt = null;

        public MainApp()
        {
            InitializeComponent();
            this.Text = "INT Development Management"; 
            this.radRibbonBar1.QuickAccessToolBar.InnerItem.Fill.Visibility = Telerik.WinControls.ElementVisibility.Collapsed;
            this.WindowState = FormWindowState.Maximized;
        }

        private void bsExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void FillBuyerlist()
        {
            // 바이어
            _dt = CommonController.Getlist(CommonValues.KeyName.CustIdx).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                lstBuyer.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));

            }

            ddlNewOrderBuyer.DataSource = lstBuyer;
            ddlNewOrderBuyer.DisplayMember = "CustName";
            ddlNewOrderBuyer.ValueMember = "CustIdx";
            ddlNewOrderBuyer.DefaultItemsCountInDropDown = CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlNewOrderBuyer.DropDownHeight = CommonValues.DDL_DropDownHeight;



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
                    ddlNewOrderBuyer.SelectedValue = cs.NewOrderBuyerIdx;
                }
                // 없으면 기본값으로 새로 만듬 
                else
                {
                    ConfigStruct cs = new ConfigStruct
                    {
                        enablePaging = true,
                        enableSearchRow = false,
                        NewOrderBuyerIdx = 0,
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
                cs.NewOrderBuyerIdx = Convert.ToInt32(ddlNewOrderBuyer.SelectedValue);

                string confStr = JsonConvert.SerializeObject(cs, Formatting.Indented);
                System.IO.File.WriteAllText(path, confStr);

                CommonValues.enablePaging = enablePagingGV.Checked;
                CommonValues.enableSearchRow = enableSearchRow.Checked;
                CommonValues.NewOrderBuyerIdx = Convert.ToInt32(ddlNewOrderBuyer.SelectedValue);
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
                    loginWnd.FormSendEvent += new MainLogin.FormSendDataHandler(DieaseUpdateEventMethod);
                    loginWnd.ShowDialog();
                    lblServerInfo.Text = "Connected to " + Int.Members.IntSampleConnectionString.Substring(12, 20);
                }

                // 시작시 설정내용을 불러와 commonvalue에 저장               
                string url = Environment.CurrentDirectory + "/conf";
                string path = url + "/conf.json";

                try
                {
                    CheckFolder(url);
                    FillBuyerlist();

                    if (System.IO.File.Exists(path))
                    {
                        string confStr = System.IO.File.ReadAllText(path);

                        ConfigStruct cs = new ConfigStruct();
                        cs = JsonConvert.DeserializeObject<ConfigStruct>(confStr);
                        CommonValues.enablePaging = cs.enablePaging;
                        CommonValues.enableSearchRow = cs.enableSearchRow;
                        CommonValues.NewOrderBuyerIdx = cs.NewOrderBuyerIdx;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// 로그인시 메뉴 각 버튼 name, tag를 권한테이블과 비교하여 권한이 없는 경우 disable한다
        /// </summary>
        /// <param name="UserIdx"></param>
        private void DieaseUpdateEventMethod(int UserIdx)
        {
            Dictionary<int, RadItem> lstMenu = new Dictionary<int, RadItem>();
            
            // Order
            lstMenu.Add(18, btnOrderOrder); lstMenu.Add(19, btnOrderWorkSheet); lstMenu.Add(20, btnOrderWorkOrder); lstMenu.Add(21, btnOrderWorkSchedule);
            lstMenu.Add(22, btnOrderReportTicketPrint); lstMenu.Add(25, btnOrderReportWorkSchedule); lstMenu.Add(23, btnOrderReportOrderStatus);
            lstMenu.Add(24, btnOrderReportProductHistory);

            // Yarn
            lstMenu.Add(54, btnYarnCodeMain); lstMenu.Add(53, btnYarnPurchase); lstMenu.Add(55, btnYarnManager); lstMenu.Add(56, btnYarnInbound);
            lstMenu.Add(57, btnYarnOutbound); lstMenu.Add(58, btnYarnStock);

            // Fabric
            lstMenu.Add(26, btnFabricFabricCode); lstMenu.Add(27, btnFabricInbound); lstMenu.Add(28, btnFabricOutbound); lstMenu.Add(29, btnFabricStock);
            lstMenu.Add(30, btnFabricCodePrint); lstMenu.Add(31, btnFabricCodelist); lstMenu.Add(32, btnFabricReportInbound); lstMenu.Add(33, btnFabricReportOutbound);
            lstMenu.Add(34, btnFabricReportStock); lstMenu.Add(35, btnFabricReportWarehouse); 

            // Pattern
            lstMenu.Add(36, btnPatternRequest); lstMenu.Add(37, btnPatternMain); lstMenu.Add(38, btnPatternReportControl);

            // Production
            lstMenu.Add(39, btnProductCutting); lstMenu.Add(40, btnProductPrinting); lstMenu.Add(41, btnProductEmbroidery); lstMenu.Add(42, btnProductSewing);
            lstMenu.Add(43, btnProductInspection);

            // Outbound
            lstMenu.Add(44, btnOutboundFabric); lstMenu.Add(45, btnOutboundCutted); lstMenu.Add(46, btnOutboundFinished); 

            // System
            lstMenu.Add(47, btnSystemColor); lstMenu.Add(48, btnSizeGroup); lstMenu.Add(49, btnSize); lstMenu.Add(50, btnSewingThread);
            lstMenu.Add(51, btnSystemApproval); lstMenu.Add(59, btnLocation);

            foreach (int menuNo in lstMenu.Keys)
            {
                if (string.IsNullOrEmpty(CheckAuth.ValidCheck(CommonValues.packageNo, menuNo, 0)) ||
                    CheckAuth.ValidCheck(CommonValues.packageNo, menuNo, 0) == "00000")
                {
                    lstMenu[menuNo].Enabled = false;
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
            if (Close_All_Children("Outbound"))
            {
                Outbound frm = new Outbound(this, "", "");
                frm.Text = "Fabric Outbound";
                frm.MdiParent = this;
                frm.Show();
            }
            
        }

        private void btnFabricCodePrint_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("PrintQRCode"))
            {
                if (CommonValues.ListWorkID.Count > 0)
                {
                    PrintQRCode frm = new PrintQRCode();
                    frm.Text = "Print QR Code";
                    frm.MdiParent = this;
                    frm.Show();
                }
                else
                {
                    RadMessageBox.Show("There's no selected valid items. \nPlease select the Fabric or Rack.", "Notice",
                            MessageBoxButtons.OK, RadMessageIcon.Error);
                    return;
                }

            }
        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("CodeLocation"))
            {
                CodeLocation frm = new CodeLocation(this);
                frm.Text = "Location Code";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void MainApp_Activated(object sender, EventArgs e)
        {
            this.Text = "INT Development Management Ver"+CommonValues.verNo; 
        }

        private void btnFabricCodelist_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("rptFabricCode"))
            {
                rptFabricCode frm = new rptFabricCode();
                frm.Text = "Fabric Code";
                frm.MdiParent = this;
                frm.Show();
            }
            
        }

        private void btnFabricReportInbound_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("frmPrintInbound"))
            {
                frmPrintInbound frm = new frmPrintInbound();
                frm.Text = "Fabric Inbound";
                frm.MdiParent = this;
                frm.Show();
            }
            
        }

        private void btnFabricReportOutbound_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("frmPrintOutbound"))
            {
                frmPrintOutbound frm = new frmPrintOutbound();
                frm.Text = "Fabric Outbound";
                frm.MdiParent = this;
                frm.Show();
            }
            
        }

        private void btnFabricReportWarehouse_Click(object sender, EventArgs e)
        {
            
                Warehouse frm = new Warehouse(this);
                frm.Text = "Fabric Warehouse";
                //frm.MdiParent = this;
                frm.Show();
            
        }

        private void btnProductCutting_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("CuttingMain"))
            {
                CuttingMain frm = new CuttingMain(this, "");
                frm.Text = "Cutting Main";
                frm.MdiParent = this;
                frm.Show();
            } 
        }

        private void btnFabricReportStock_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("frmPrintStock"))
            {
                frmPrintStock frm = new frmPrintStock();
                frm.Text = "Fabric Stock";
                frm.MdiParent = this;
                frm.Show();
            }
            
        }
    }
}
