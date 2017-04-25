using System;
using System.Data;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using Dev.Options;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;

namespace SampleApp
{
    public partial class MainLogin : RadForm
    {
        const string RegRoot = @"Software\INT\SampleApp";
        private string __savedInfo__ = RegRoot + @"\savedLog";

        public MainLogin() 
        {
            InitializeComponent();
        }

        private void MainLogin_Load(object sender, EventArgs e)
        {
            //Office2013LightTheme theme = new Office2013LightTheme();
            //ThemeResolutionService.ApplicationThemeName = "Office2013Light";

            //TelerikMetroBlueTheme theme = new TelerikMetroBlueTheme();
            //ThemeResolutionService.ApplicationThemeName = "TelerikMetroBlue";

            // 업데이트 정보확인 
            string url = Environment.CurrentDirectory + "/conf";
            string path = url + "/VersionINTSample.json";

            try
            {
                CheckFolder(url);

                if (System.IO.File.Exists(path))
                {
                    bool IsDiffVersion = false; 

                    // 파일 읽고
                    string confStr = System.IO.File.ReadAllText(path);

                    List<VersionStruct> versionList = new List<VersionStruct>();
                    VersionStruct vs = new VersionStruct(); 

                    versionList = JsonConvert.DeserializeObject<List<VersionStruct>>(confStr);
                    
                    // 버전 확인한후
                    foreach(VersionStruct ver in versionList)
                    {
                        DataRow dr = Data.LoginData.FindUpdateInfo(
                                CommonValues.packageNo,
                                ver.FileName, 
                                Math.Round(ver.Version,2), 
                                ver.AppliedDate
                        );
                        if (dr == null)
                        {
                            IsDiffVersion = true; 
                        }
                    }
                    
                    // 버전이 다르다면 업데이트
                    if(IsDiffVersion)
                    {
                        if (RadMessageBox.Show("프로그램이 업데이트 되었습니다.\n지금 업데이트 하시겠습니까?", "Confirm",
                        MessageBoxButtons.YesNo, RadMessageIcon.Question) == DialogResult.Yes)
                        {
                            // 업데이트 
                            UpdateRun("INTUpdate.exe");
                            Application.Exit();
                        }
                        else
                        {
                            Application.Exit();
                        }
                    }
                }
                else
                {
                    //버전 파일이 없다면
                    if (RadMessageBox.Show("설치된 내역에 업데이트 정보가 없습니다.\n지금 업데이트 하시겠습니까?", "Confirm",
                        MessageBoxButtons.YesNo, RadMessageIcon.Question) == DialogResult.Yes)
                    {
                        // 업데이트 
                        UpdateRun("INTUpdate.exe");
                        Application.Exit();
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // 레지스트리 등록 로그인 정보확인 
            RegistryKey R = Registry.CurrentUser.OpenSubKey(__savedInfo__); 
            if (R!=null)
            {
                txtUsername.Text = (string)R.GetValue("intid");
                txtPassword.Text = (string)R.GetValue("intkey");
                R.Close();
                btnLogin.PerformClick(); 
            }
        }
        private void UpdateRun(string path)
        {
            if (File.Exists(path))
            {
                // 업데이터에 패키지 번호 전달 
                ProcessStartInfo startInfo = new ProcessStartInfo(path, CommonValues.packageNo.ToString());
                Process.Start(startInfo);
            }
            else
            {
                RadMessageBox.Show("업데이트 파일이 존재하지 않습니다.\n관리부에 문의해주시기 바랍니다.", "Warning", MessageBoxButtons.OK,
                    RadMessageIcon.Error);
                return; 
            }
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string strUserid = txtUsername.Text.ToString().Trim().ToLower();
            string strPasswd = txtPassword.Text.ToString().Trim().ToLower(); 

            if (string.IsNullOrEmpty(strUserid) || strUserid == "username")
            {
                RadMessageBox.Show("Please input the Username", "Error", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                return; 
            }
            if (string.IsNullOrEmpty(strPasswd) || strPasswd == "password")
            {
                RadMessageBox.Show("Please input the Password", "Error", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                return;
            }

            DataRow dr = Data.LoginData.TryUserLogin(strUserid, strPasswd); 
            
            if (dr!=null && Convert.ToInt32(dr["useridx"])>0)
            {
                if (chkSave.Checked)
                {
                    RegistryKey R = Registry.CurrentUser.OpenSubKey(__savedInfo__, true);
                    if (R == null)
                    {
                        R = Registry.CurrentUser.CreateSubKey(__savedInfo__);
                    }
                    R.SetValue("intid", strUserid);
                    R.SetValue("intkey", strPasswd);
                    R.Close();
                }

                UserInfo.Idx = Convert.ToInt32(dr["useridx"]);
                UserInfo.Username = strUserid;
                UserInfo.Password = strPasswd; 
                UserInfo.DeptIdx = Convert.ToInt32(dr["deptidx"]);
                UserInfo.ReportNo = Convert.ToInt32(dr["reportno"]);
                UserInfo.CenterIdx = Convert.ToInt32(dr["costcenteridx"]);
                
                //MainApp frm = new MainApp();
                //frm.Show();

                this.Close();
            }
            else
            {
                RadMessageBox.Show("Wrong password!\nPlease check the username and password", 
                        "Login error", MessageBoxButtons.OK, RadMessageIcon.Exclamation);
                return; 
            }
            
        }
        
        private void btnLostPassword_Click(object sender, EventArgs e)
        {
            RadMessageBox.Show("Please contact the IT team", "Information", MessageBoxButtons.OK, RadMessageIcon.Info);
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick(); 
            }
        }

        private void MainLogin_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (UserInfo.Idx <= 0) Application.Exit();
        }
    }
}
