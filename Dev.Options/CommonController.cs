﻿using Int.Department;
using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Telerik.WinControls.UI;
using System.Globalization;
using System.Threading;
using System.Drawing;
using Telerik.WinControls;

namespace Dev.Options
{
    public class CommonController
    {
        #region Methods

        /// <summary>
        /// key를 받아 해당 자료를 DB로부터 불러와서 데이터셋에서 저장후 리턴한다 
        /// </summary>
        /// <param name="keyName">CommonValues에 정의된 구분키</param>
        /// <returns>데이터셋 컬렉션</returns>
        public static DataSet Getlist(CommonValues.KeyName keyName)
        {
            DataSet ds = new DataSet();
            DataTable dt = new DataTable(); 

            switch (keyName)
            {
                case CommonValues.KeyName.Brand: 
                    ds = Data.CommonData.GetlistCode("Brand"); break;

                case CommonValues.KeyName.Wash:
                    ds = Int.Customer.Customer.Getlist(33); break;

                case CommonValues.KeyName.IsPrinting:
                    ds = Data.CommonData.GetlistCode("IsPrinting"); break; 

                case CommonValues.KeyName.ShipTerm:
                    ds = Data.CommonData.GetlistCode("ShipTerm"); break;

                case CommonValues.KeyName.VsslAir:
                    ds = Data.CommonData.GetlistCode("VsslAir"); break;

                case CommonValues.KeyName.Destination:
                    ds = Data.CommonData.GetlistCode("Destination"); break;

                case CommonValues.KeyName.Period:
                    ds = Data.CommonData.GetlistCode("Period Time"); break;

                case CommonValues.KeyName.Codes:                // 전체 코드 
                    ds = Data.CommonData.GetlistCode(""); break;

                case CommonValues.KeyName.TrimCode:                // 전체 부자재 코드 
                    ds = Data.CommonData.GetlistCode("Trim2"); break;

                case CommonValues.KeyName.DeptIdx:              // 부서
                    ds = Dept.GetNamelist(); break;

                case CommonValues.KeyName.CustIdx:              // 바이어
                    if (UserInfo.ReportNo < 9 && Options.UserInfo.ExceptionGroup != 233 &&
                        UserInfo.CenterIdx != 1 && UserInfo.DeptIdx == 5 && UserInfo.DeptIdx == 6)
                        ds = Int.Customer.Customer.GetNamelist(UserInfo.DeptIdx);
                    else
                        ds = Int.Customer.Customer.GetNamelist();
                    break;

                case CommonValues.KeyName.CustAll:              // 모든 거래처 
                    ds = Int.Customer.Customer.GetNamelist();
                    break;

                case CommonValues.KeyName.CustAllExceptBuyer:   // 바이어, 은행등을 제외한 모든 거래처
                    ds = Int.Customer.Customer.Getlist(0);
                    break;

                case CommonValues.KeyName.SewThread:
                    ds = Int.Customer.Customer.Getlist(102); break;     // Trim(Sewing Thread) Maker

                case CommonValues.KeyName.Vendor:
                    ds = Int.Customer.Customer.Getlist(30); break;     // Sew Vendor
                    
                // 나염여부 
                case CommonValues.KeyName.EmbelishId1:
                    ds = Int.Customer.Customer.Getlist(CommonValues.DictionaryCodeClass["Embllishment"]); break;
                case CommonValues.KeyName.EmbelishId2:
                    ds = Int.Customer.Customer.Getlist(CommonValues.DictionaryCodeClass["Embllishment"]); break;

                // 유저명
                case CommonValues.KeyName.User:                         // 해당 부서의 모든 유저
                    ds = Int.Users.Users.Getlist(UserInfo.DeptIdx); break;

                case CommonValues.KeyName.AllUser:                      // 모든 유저
                    ds = Int.Users.Users.Getlist(0); break;

                case CommonValues.KeyName.TDUser:                       // 개발실 TD 유저
                    ds = Int.Users.Users.Getlist(12); break;

                case CommonValues.KeyName.CADUser:                       // 개발실 CAD 유저
                    ds = Int.Users.Users.Getlist(11); break;
                
                default:
                    break;
            }

            return ds;
        }

        /// <summary>
        /// 로그기록을 위한 현재 날짜, 시간 포맷
        /// </summary>
        /// <returns>yyyy-MM-dd HH:mm:ss:000</returns>
        private static string GetDateTime()
        {
            DateTime NowDate = DateTime.Now;
            return NowDate.ToString("yyyy-MM-dd HH:mm:ss") + ":" + NowDate.Millisecond.ToString("000");
        }

        /// <summary>
        /// 각종 로그를 기록 (/logs/Log-yyyyMM 에 저장)
        /// </summary>
        /// <param name="str">기록하고자 하는 로그내역 (내역앞부분에 윈도우명 명시필요)</param>
        public static void Log(string str)
        {
            // 저장위치
            string FilePath = Environment.CurrentDirectory + @"\logs\Log-" + DateTime.Today.ToString("yyyyMM") + ".log";
            string DirPath = Environment.CurrentDirectory + @"\logs";
            string temp;

            DirectoryInfo di = new DirectoryInfo(DirPath);
            FileInfo fi = new FileInfo(FilePath);

            try
            {
                if (di.Exists != true) Directory.CreateDirectory(DirPath);

                // 로그파일이 없으면 쓰기모드
                if (fi.Exists != true)
                {
                    using (StreamWriter sw = new StreamWriter(FilePath))
                    {
                        temp = string.Format("[{0}] : {1}", GetDateTime(), str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
                // 이미 있으면 추가모드
                else
                {
                    using (StreamWriter sw = File.AppendText(FilePath))
                    {
                        temp = string.Format("[{0}] : {1}", GetDateTime(), str);
                        sw.WriteLine(temp);
                        sw.Close();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// 현재 열려진 창을 닫고 새로 오픈
        /// </summary>
        /// <param name="frm"></param>
        /// <returns></returns>
        public static void Close_All_Children(RadForm thisform, string frm)
        {
            foreach (Form f in thisform.MdiParent.MdiChildren)
            {
                if (f.Name == frm.ToString())
                {
                    f.Close();
                }
            }
        }

        /// <summary>
        /// 날짜변환 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DateTime ConvertDate(DateTime dt)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ko-KR");
            return Convert.ToDateTime(dt.ToString("d"));
        }

        /// <summary>
        /// 알림메시지 띄우기
        /// </summary>
        /// <param name="caption"></param>
        /// <param name="content"></param>
        public static void Notify_WorkStatus(string caption, string content)
        {
            RadDesktopAlert ra = new RadDesktopAlert();
            //ra.ContentImage = Properties.Resources.envelope;

            ra.Popup.AlertElement.CaptionElement.TextAndButtonsElement.TextElement.ForeColor = Color.Red;
            ra.Popup.AlertElement.CaptionElement.CaptionGrip.BackColor = Color.Red;
            ra.Popup.AlertElement.CaptionElement.CaptionGrip.GradientStyle = GradientStyles.Solid;
            ra.Popup.AlertElement.ContentElement.Font = new Font("Arial", 9f, FontStyle.Regular);
            ra.Popup.AlertElement.ContentElement.TextImageRelation = TextImageRelation.TextBeforeImage;
            ra.Popup.AlertElement.BackColor = Color.Yellow;
            ra.Popup.AlertElement.GradientStyle = GradientStyles.Solid;
            ra.Popup.AlertElement.BorderColor = Color.Red;

            ra.CaptionText = caption;
            ra.ContentText = content;
            ra.AutoCloseDelay = 20;
            ra.Show();
        }
        #endregion

    }
}
