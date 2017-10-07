using Dev.Codes;
using Dev.Fabric;
using Dev.Options;
using Dev.Out;
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
            txtEmail.Text = UserInfo.Email;
            txtPhone.Text = UserInfo.Phone; 

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
                    SetHandler.Checked = cs.SetHandler;

                    OrderOpCheck1.Checked = cs.OrderOpCheck1;
                    OrderOpCheck2.Checked = cs.OrderOpCheck2;

                    switch (cs.OrderOpCheck3)
                    {
                        case 0: OrderOpCheck3.CheckState = CheckState.Unchecked; break;
                        case 1: OrderOpCheck3.CheckState = CheckState.Checked; break;
                        case 2: OrderOpCheck3.CheckState = CheckState.Indeterminate; break;
                    }

                    WorksheetOpCheck1.Checked = cs.WorksheetOpCheck1;
                    WorksheetOpCheck2.Checked = cs.WorksheetOpCheck2;
                    WorksheetOpCheck3.Checked = cs.WorksheetOpCheck3;
                    WorksheetOpCheck4.Checked = cs.WorksheetOpCheck4;
                    WorksheetOpCheck5.Checked = cs.WorksheetOpCheck5;

                    PatternOpCheck1.Checked = cs.PatternOpCheck1;
                    PatternOpCheck2.Checked = cs.PatternOpCheck2;
                    PatternOpCheck3.Checked = cs.PatternOpCheck3;
                    PatternOpCheck4.Checked = cs.PatternOpCheck4;
                    PatternOpCheck5.Checked = cs.PatternOpCheck5;
                    PatternOpCheck6.Checked = cs.PatternOpCheck6;

                }
                // 없으면 기본값으로 새로 만듬 
                else
                {
                    ConfigStruct cs = new ConfigStruct
                    {
                        enablePaging = true,
                        enableSearchRow = false,
                        NewOrderBuyerIdx = 0,

                        OrderOpCheck1 = true,
                        OrderOpCheck2 = true,
                        OrderOpCheck3 = 2,

                        WorksheetOpCheck1 = true,
                        WorksheetOpCheck2 = true,
                        WorksheetOpCheck3 = true,
                        WorksheetOpCheck4 = true,
                        WorksheetOpCheck5 = true,

                        PatternOpCheck1 = true,
                        PatternOpCheck2 = true,
                        PatternOpCheck3 = true,
                        PatternOpCheck4 = true,
                        PatternOpCheck5 = true,
                        PatternOpCheck6 = true,
                    };

                    string confStr = JsonConvert.SerializeObject(cs, Formatting.Indented);
                    System.IO.File.WriteAllText(path, confStr);

                    OrderOpCheck1.Checked = true;
                    OrderOpCheck2.Checked = true;
                    OrderOpCheck3.CheckState = CheckState.Indeterminate;

                    WorksheetOpCheck1.Checked = true;
                    WorksheetOpCheck2.Checked = true;
                    WorksheetOpCheck3.Checked = true;
                    WorksheetOpCheck4.Checked = true;
                    WorksheetOpCheck5.Checked = true;

                    PatternOpCheck1.Checked = true;
                    PatternOpCheck2.Checked = true;
                    PatternOpCheck3.Checked = true;
                    PatternOpCheck4.Checked = true;
                    PatternOpCheck5.Checked = true;
                    PatternOpCheck6.Checked = true;
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
                cs.SetHandler = SetHandler.Checked; 
                cs.NewOrderBuyerIdx = Convert.ToInt32(ddlNewOrderBuyer.SelectedValue);

                cs.OrderOpCheck1 = OrderOpCheck1.Checked;
                cs.OrderOpCheck2 = OrderOpCheck2.Checked;
                cs.OrderOpCheck3 = Convert.ToInt32(OrderOpCheck3.CheckState);

                cs.WorksheetOpCheck1 = WorksheetOpCheck1.Checked;
                cs.WorksheetOpCheck2 = WorksheetOpCheck2.Checked;
                cs.WorksheetOpCheck3 = WorksheetOpCheck3.Checked;
                cs.WorksheetOpCheck4 = WorksheetOpCheck4.Checked;
                cs.WorksheetOpCheck5 = WorksheetOpCheck5.Checked;
                
                cs.PatternOpCheck1 = PatternOpCheck1.Checked;
                cs.PatternOpCheck2 = PatternOpCheck2.Checked;
                cs.PatternOpCheck3 = PatternOpCheck3.Checked;
                cs.PatternOpCheck4 = PatternOpCheck4.Checked;
                cs.PatternOpCheck5 = PatternOpCheck5.Checked;
                cs.PatternOpCheck6 = PatternOpCheck6.Checked;

                string confStr = JsonConvert.SerializeObject(cs, Formatting.Indented);
                System.IO.File.WriteAllText(path, confStr);

                CommonValues.enablePaging = enablePagingGV.Checked;
                CommonValues.enableSearchRow = enableSearchRow.Checked;
                CommonValues.SetHandler = SetHandler.Checked; 
                CommonValues.NewOrderBuyerIdx = Convert.ToInt32(ddlNewOrderBuyer.SelectedValue);

                CommonValues.OrderOpCheck1 = OrderOpCheck1.Checked;
                CommonValues.OrderOpCheck2 = OrderOpCheck2.Checked;
                CommonValues.OrderOpCheck3 = Convert.ToInt32(OrderOpCheck3.CheckState);

                CommonValues.WorksheetOpCheck1 = WorksheetOpCheck1.Checked;
                CommonValues.WorksheetOpCheck2 = WorksheetOpCheck2.Checked;
                CommonValues.WorksheetOpCheck3 = WorksheetOpCheck3.Checked;
                CommonValues.WorksheetOpCheck4 = WorksheetOpCheck4.Checked;
                CommonValues.WorksheetOpCheck5 = WorksheetOpCheck5.Checked;

                CommonValues.PatternOpCheck1 = PatternOpCheck1.Checked;
                CommonValues.PatternOpCheck2 = PatternOpCheck2.Checked;
                CommonValues.PatternOpCheck3 = PatternOpCheck3.Checked;
                CommonValues.PatternOpCheck4 = PatternOpCheck4.Checked;
                CommonValues.PatternOpCheck5 = PatternOpCheck5.Checked;
                CommonValues.PatternOpCheck6 = PatternOpCheck6.Checked;
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

                        CommonValues.OrderOpCheck1 = cs.OrderOpCheck1;
                        CommonValues.OrderOpCheck2 = cs.OrderOpCheck2;
                        CommonValues.OrderOpCheck3 = cs.OrderOpCheck3;

                        CommonValues.WorksheetOpCheck1 = cs.WorksheetOpCheck1; 
                        CommonValues.WorksheetOpCheck2 = cs.WorksheetOpCheck2;
                        CommonValues.WorksheetOpCheck3 = cs.WorksheetOpCheck3;
                        CommonValues.WorksheetOpCheck4 = cs.WorksheetOpCheck4;
                        CommonValues.WorksheetOpCheck5 = cs.WorksheetOpCheck5;

                        CommonValues.PatternOpCheck1 = cs.PatternOpCheck1; 
                        CommonValues.PatternOpCheck2 = cs.PatternOpCheck2;
                        CommonValues.PatternOpCheck3 = cs.PatternOpCheck3;
                        CommonValues.PatternOpCheck4 = cs.PatternOpCheck4;
                        CommonValues.PatternOpCheck5 = cs.PatternOpCheck5;
                        CommonValues.PatternOpCheck6 = cs.PatternOpCheck6;
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
            lstMenu.Add(24, btnOrderReportProductHistory); lstMenu.Add(106, btnOrderTrim);
            
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
            lstMenu.Add(43, btnProductInspection); lstMenu.Add(82, prodCutting); lstMenu.Add(89, prodPrinting); lstMenu.Add(90, prodEmbroidery);
            lstMenu.Add(91, prodSewing); lstMenu.Add(92, prodInspection);

            // Outbound
            lstMenu.Add(44, btnOutboundFabric); lstMenu.Add(45, btnOutboundCutted); lstMenu.Add(46, btnOutboundFinished);
            lstMenu.Add(12, btnOutPrintInvoice); lstMenu.Add(13, btnOutPrintProductHistory);
            

            // System
            lstMenu.Add(47, btnSystemColor); lstMenu.Add(48, btnSizeGroup); lstMenu.Add(49, btnSize); lstMenu.Add(50, btnSewingThread);
            lstMenu.Add(51, btnSystemApproval); lstMenu.Add(59, btnLocation);
            
            bool bOrder = false;
            bool bYarn = false;
            bool bFabric = false;
            bool bPattern = false;
            bool bProduction = false;
            bool bOut = false;
            bool bSystem = false;

            // 각 메뉴별 사용자 권한을 리스트에 저장 
            foreach (int menuNo in lstMenu.Keys)
            {
                if (string.IsNullOrEmpty(CheckAuth.ValidCheck(CommonValues.packageNo, menuNo, 0)) ||
                    CheckAuth.ValidCheck(CommonValues.packageNo, menuNo, 0) == "00000")
                {
                    lstMenu[menuNo].Visibility = ElementVisibility.Collapsed;
                    lstMenu[menuNo].Enabled = false;
                }
                
                // 권한 테이블을 탐색하면서 
                foreach (DataRow row in UserInfo.DtAuthority.Rows)
                {
                    if (Convert.ToInt32(row["ProgramIdx"]) == menuNo)
                    {
                        if (row["ClassName"].ToString().Trim() == "Yarn")
                        {
                            bYarn = true;
                        }
                        else if (row["ClassName"].ToString().Trim() == "Fabric")
                        {
                            bFabric = true;
                        }
                        else if (row["ClassName"].ToString().Trim() == "Pattern")
                        {
                            bPattern = true;
                        }
                        else if (row["ClassName"].ToString().Trim() == "Production")
                        {
                            bProduction = true;
                        }
                        else if (row["ClassName"].ToString().Trim() == "Outbound")
                        {
                            bOut = true;
                        }
                        else if (row["ClassName"].ToString().Trim() == "Codes")
                        {
                            bSystem = true;
                        }
                        else if (row["ClassName"].ToString().Trim() == "Sales")
                        {
                            bOrder = true;
                        }
                    }
                }
            }
            // 각 탭별로 권한이 전혀 없는 탭은 숨기기 
            if (!bYarn)
            {
                ribbonTab1.Visibility = ElementVisibility.Collapsed;
            }
            if (!bFabric)
            {
                ribbonTab4.Visibility = ElementVisibility.Collapsed;
            }
            if (!bPattern)
            {
                ribbonTab3.Visibility = ElementVisibility.Collapsed;
            }
            if (!bProduction)
            {
                ribbonTab5.Visibility = ElementVisibility.Collapsed;
            }
            if (!bOut)
            {
                ribbonTab6.Visibility = ElementVisibility.Collapsed;
            }
            if (!bSystem)
            {
                ribbonTab7.Visibility = ElementVisibility.Collapsed;
            }
            if (!bOrder)
            {
                ribbonTab2.Visibility = ElementVisibility.Collapsed;
                //ribGroupAccounting.Visibility = ElementVisibility.Collapsed;
            }

            // 기타 
            if (!lstMenu[51].Enabled) radRibbonBarGroup13.Visibility = ElementVisibility.Collapsed;     // System > Other
            if (!lstMenu[26].Enabled) radRibbonBarGroup15.Visibility = ElementVisibility.Collapsed;     // Fabric > Code

            if (!lstMenu[27].Enabled && !lstMenu[28].Enabled && !lstMenu[29].Enabled)
                radRibbonBarGroup8.Visibility = ElementVisibility.Collapsed;     // Fabric > Fabric

            if (!lstMenu[22].Enabled && !lstMenu[25].Enabled && !lstMenu[23].Enabled && !lstMenu[24].Enabled)
                radRibbonBarGroup5.Visibility = ElementVisibility.Collapsed;    // order > report 
            
            ribbonTab2.Focus();
            ribbonTab2.Select();
            //radRibbonBarGroup4.AutoSize = true;

            //실행시 order tab 선택 
            foreach (RadPageViewItem ri in radRibbonBar1.RibbonBarElement.TabStripElement.Items)
            {
                if (ri.Text == "Order")
                {
                    radRibbonBar1.RibbonBarElement.TabStripElement.SelectedItem = ri;
                    break;
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
                    //iRtn = false;
                }
                f.Close();
            }
            iRtn = true;
            return iRtn;
        }
        private bool Close_Wnd_Children(string frm)
        {
            bool iRtn = true;

            foreach (Form f in this.MdiChildren)
            {
                if (f.Name == frm.ToString())
                {
                    iRtn = true;
                    f.Close();
                }
                
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
            try
            {
                if (Close_All_Children("OrderMain"))
                {
                    OrderMain ordfrm = null; 
                    ordfrm = new OrderMain(this, "");
                    ordfrm.Text = "OrderMain";
                    ordfrm.MdiParent = this;
                    ordfrm.Show();
                }
            }
            catch(Exception ex) {  }
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
                PatternMain frm = null;
                frm = new PatternMain(this, "");
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
            if (Close_Wnd_Children("WorkOrderTicketPrint"))
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
            if (Close_All_Children("frmFabricCode"))
            {
                frmFabricCode frm = new frmFabricCode();
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

        private void btnProductPrinting_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("PrintingMain"))
            {
                PrintingMain frm = new PrintingMain(this, "");
                frm.Text = "Printing Main";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnProductEmbroidery_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("EmbroideryMain"))
            {
                EmbroideryMain frm = new EmbroideryMain(this, "");
                frm.Text = "Embroidery Main";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnProductSewing_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("SewingMain"))
            {
                SewingMain frm = new SewingMain(this, "");
                frm.Text = "Sewing Main";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnProductInspection_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("InspectionMain"))
            {
                InspectionMain frm = new InspectionMain(this, "");
                frm.Text = "Inspection Main";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnPatternReportControl_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("PrintPattern"))
            {
                PrintPattern frm = new PrintPattern();
                frm.Text = "PrintPattern";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnOutboundFinished_Click(object sender, EventArgs e)
        {
            if (Close_All_Children("FinishedMain"))
            {
                FinishedMain frm = new FinishedMain(this, "");
                frm.Text = "FinishedMain";
                frm.MdiParent = this; 
                frm.Show();
            }
        }

        private void ProductionReport_Click(object sender, EventArgs e)
        {
            if (Close_Wnd_Children("PrintCutting"))
            {
                PrintCutting frm = new PrintCutting();
                frm.Text = "PrintCutting";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void prodPrinting_Click(object sender, EventArgs e)
        {
            if (Close_Wnd_Children("PrintPrinting"))
            {
                PrintPrinting frm = new PrintPrinting();
                frm.Text = "PrintPrinting";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void prodEmbroidery_Click(object sender, EventArgs e)
        {
            if (Close_Wnd_Children("PrintEmbroidery"))
            {
                PrintEmbroidery frm = new PrintEmbroidery();
                frm.Text = "PrintEmbroidery";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void prodSewing_Click(object sender, EventArgs e)
        {
            if (Close_Wnd_Children("PrintSewing"))
            {
                PrintSewing frm = new PrintSewing();
                frm.Text = "PrintSewing";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void prodInspection_Click(object sender, EventArgs e)
        {
            if (Close_Wnd_Children("PrintInspection"))
            {
                PrintInspection frm = new PrintInspection();
                frm.Text = "PrintInspection";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnOrderWorkSheet_Click(object sender, EventArgs e)
        {
            try
            {
                if (Close_All_Children("WorksheetMain"))
                {
                    WorksheetMain frm = null;
                    frm = new WorksheetMain(this, "");
                    frm.Text = "Worksheet Main";
                    frm.MdiParent = this;
                    frm.Show();
                }
            }
            catch(Exception ex) {  }

        }

        private void btnOutPrintInvoice_Click(object sender, EventArgs e)
        {
            if (Close_Wnd_Children("PrintInvoice"))
            {
                PrintInvoice frm = new PrintInvoice("");
                frm.Text = "Print Invoice";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnOutPrintProductHistory_Click(object sender, EventArgs e)
        {
            if (Close_Wnd_Children("PrintProductHistory"))
            {
                PrintProductHistory frm = new PrintProductHistory();
                frm.Text = "PrintProductHistory";
                frm.MdiParent = this;
                frm.Show();
            }
        }

        private void btnOrderReportOrderStatus_Click(object sender, EventArgs e)
        {
            if (Close_Wnd_Children("PrintOrderReview"))
            {
                PrintOrderReview frm = new PrintOrderReview(0);
                frm.Text = "PrintOrderReview";
                frm.MdiParent = this;
                frm.Show();
            }
            
        }

        private void btnOrderTrim_Click(object sender, EventArgs e)
        {
            try
            {
                if (Close_All_Children("TrimMain"))
                {
                    TrimMain frm = new TrimMain(this, 0);
                    frm.Text = "Trim Main";
                    frm.MdiParent = this;
                    frm.Show();
                }

            }
            catch (Exception ex) { }
        }

        private void btnAccountSave_Click(object sender, EventArgs e)
        {
            try
            {
                string Email = txtEmail.Text.Trim(); 
                string Phone = txtPhone.Text.Trim();

                bool bResult = Data.LoginData.ModifyUserInfo(UserInfo.Idx, Email, Phone);

                if (bResult)
                {
                    RadMessageBox.Show("Updated User Information", "Info", MessageBoxButtons.OK, RadMessageIcon.Info);
                    UserInfo.Email = Email;
                    UserInfo.Phone = Phone;
                }
                else
                {
                    RadMessageBox.Show("Failed to update", "Error", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message.ToString());
            }
        }
    }
}
