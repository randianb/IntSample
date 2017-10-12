using Int.Code;
using Int.Customer;
using Int.Department;
using Dev.Options;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using Telerik.WinControls;
using Telerik.WinControls.UI;
using System.Diagnostics;
using Microsoft.WindowsAzure.Storage;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Dev.Pattern.Properties;

namespace Dev.Pattern
{
    /// <summary>
    /// 작지리스트 보기화면 - 수정불가 > 추가등록
    /// </summary>
    public partial class TrimMain : InheritForm
    {
        #region 1. 변수 설정

        public InheritMDI __main__;                                             // 부모 MDI (하단 상태바 리턴용) 
        public Dictionary<CommonValues.KeyName, int> _searchKey;                // 쿼리값 value, 쿼리항목 key로 전달 
        public DataRow InsertedOrderRow = null;
        
        private bool _bRtn;                                                     // 쿼리결과 리턴
        private DataSet _ds1 = null;                                            // 기본 데이터셋
        private DataTable _dt = null;                                           // 기본 데이터테이블
        private Controller.Worksheet _obj1 = null;                                // 현재 생성된 객체 
        private Controller.OrderType _obj2 = null;                           // 현재 생성된 객체 
        private RadContextMenu contextMenu;                                     // 컨텍스트 메뉴
        private List<CodeContents> lstStatus = new List<CodeContents>();        // 오더상태
        private List<DepartmentName> deptName = new List<DepartmentName>();     // 부서
        private List<CustomerName> custName = new List<CustomerName>();         // 거래처
        private List<CustomerName> lstVendor = new List<CustomerName>();        // vendor
        private List<CustomerName> embName = new List<CustomerName>();          // 나염업체
        private List<CodeContents> codeName = new List<CodeContents>();         // 코드
        private List<CodeContents> lstOrigin = new List<CodeContents>();        // Origin country 
        private List<CodeContents> lstBrand = new List<CodeContents>();         // 브랜드
        private List<CodeContents> lstIsPrinting = new List<CodeContents>();    // 나염여부
        private List<CustomerName> lstUser = new List<CustomerName>();          // 유저명
        private List<CustomerName> lstSewthread = new List<CustomerName>();     // sewthread
        private List<CodeContents> lstColor = new List<CodeContents>();         // 컬러명
        private List<CodeContents> lstOperation = new List<CodeContents>();     // 공정명 
        private List<CodeContents> trimName = new List<CodeContents>();

        private string _layoutfile = "/GVLayoutOrderTrim.xml";
        private int _orderIdx=0;

        private List<string> lstFiles = new List<string>();
        private List<string> lstFileUrls = new List<string>();

        private string __AUTHCODE__ = CheckAuth.ValidCheck(CommonValues.packageNo, 106, 0);   // 패키지번호, 프로그램번호, 윈도우번호

        private ImageList imagesList = null;
        

        //RadMenuItem mnuNew, mnuDel, mnuHide, mnuShow, menuItem2, menuItem3, menuItem4, mnuWorksheet, mnuCutting, mnuOutsourcing, mnuSewing, mnuInspecting = null;

        #endregion

        #region 2. 초기로드 및 컨트롤 생성

        /// <summary>
        /// Initializer - InheritMDI 상속
        /// </summary>
        /// <param name="main"></param>
        public TrimMain(InheritMDI main, int OrderIdx)
        {
            base.InitializeComponent(); // parent 컴포넌트에 접근하기 위해 컨트롤을 public으로 지정 > 향후 수정 필요 (todo) 
            InitializeComponent();
            __main__ = main;            // MDI 연결 
            _gv1 = this.gvWorksheet;  // 그리드뷰 일반화를 위해 변수 별도 저장
            _orderIdx = OrderIdx;

            imagesList = new ImageList();
            imagesList.Images.Add(Resources.grey);
            imagesList.Images.Add(Resources.red);
            imagesList.Images.Add(Resources.blue);
        }

        /// <summary>
        /// Form Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmOrderActual_Load(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;  // 창 최대화
            DataLoading_DDL();          // DDL 데이터 로딩
            Config_DropDownList();      // 상단 DDL 생성 설정
            GV1_CreateColumn(_gv1);     // 그리드뷰 생성
            GV1_LayoutSetting(_gv1);    // 중앙 그리드뷰 설정 
            LoadGVLayout();             // 그리드뷰 레이아웃 복구 

            // 다른 폼으로부터 전달된 Work ID가 있을 경우, 해당 ID로 조회 
            if (_orderIdx>0)
            {
                DataBinding_GV1(0, 0, 0, 0, "", "", 0, _orderIdx);
            }
            
        }
        /// <summary>
        /// 상단 검색을 위한 Dropdownlist 생성
        /// </summary>
        private void Config_DropDownList()
        {
            // 부서 
            ddlDept.DataSource = deptName;
            ddlDept.DisplayMember = "DeptName";
            ddlDept.ValueMember = "DeptIdx";
            ddlDept.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlDept.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 바이어
            ddlCust.DataSource = custName;
            ddlCust.DisplayMember = "CustName";
            ddlCust.ValueMember = "CustIdx";
            ddlCust.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlCust.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 담당자
            ddlHandler.DataSource = lstUser;
            ddlHandler.DisplayMember = "CustName";
            ddlHandler.ValueMember = "CustIdx";
            ddlHandler.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlHandler.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 오더 상태 
            ddlStatus.DataSource = lstStatus;
            ddlStatus.DisplayMember = "Contents";
            ddlStatus.ValueMember = "CodeIdx";
            ddlStatus.DefaultItemsCountInDropDown = Options.CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlStatus.DropDownHeight = Options.CommonValues.DDL_DropDownHeight;

            // 정해진 바이어가 있는 경우, 해당 바이어로 dropdown 미리 선택
            if (CommonValues.NewOrderBuyerIdx > 0)
                ddlCust.SelectedValue = Convert.ToInt32(CommonValues.NewOrderBuyerIdx);
        }
        

        /// <summary>
        /// 그리드뷰 컬럼 생성 (메인)
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV1_CreateColumn(RadGridView gv)
        {
            #region Columns 생성

            GridViewTextBoxColumn Idx = new GridViewTextBoxColumn();
            Idx.Name = "Idx";
            Idx.FieldName = "Idx";
            Idx.Width = 50;
            Idx.TextAlignment = ContentAlignment.MiddleCenter;
            Idx.HeaderText = "ID";
            Idx.ReadOnly = true;
            gv.Columns.Add(Idx);
                        
            GridViewTextBoxColumn DeptIdx = new GridViewTextBoxColumn();
            DeptIdx.Name = "DeptIdx";
            DeptIdx.FieldName = "DeptIdx";
            DeptIdx.ReadOnly = true;
            DeptIdx.Width = 55;
            DeptIdx.TextAlignment = ContentAlignment.MiddleLeft;
            DeptIdx.HeaderText = "Dept.";
            gv.Columns.Add(DeptIdx);

            GridViewTextBoxColumn Buyer = new GridViewTextBoxColumn();
            Buyer.Name = "Buyer";
            Buyer.FieldName = "Buyer";
            Buyer.ReadOnly = true;
            Buyer.Width = 110;
            Buyer.TextAlignment = ContentAlignment.MiddleLeft;
            Buyer.HeaderText = "Buyer";
            gv.Columns.Add(Buyer);

            GridViewTextBoxColumn Fileno = new GridViewTextBoxColumn();
            Fileno.Name = "Fileno";
            Fileno.FieldName = "Fileno";
            Fileno.ReadOnly = true;
            Fileno.Width = 80;
            Fileno.TextAlignment = ContentAlignment.MiddleLeft;
            Fileno.HeaderText = "File#";
            gv.Columns.Add(Fileno);

            GridViewTextBoxColumn Styleno = new GridViewTextBoxColumn();
            Styleno.Name = "Styleno";
            Styleno.FieldName = "Styleno";
            Styleno.ReadOnly = true;
            Styleno.Width = 130;
            Styleno.TextAlignment = ContentAlignment.MiddleLeft;
            Styleno.HeaderText = "Style#";
            gv.Columns.Add(Styleno);

            GridViewTextBoxColumn ColorNm = new GridViewTextBoxColumn();
            ColorNm.Name = "ColorNm";
            ColorNm.FieldName = "ColorNm";
            ColorNm.ReadOnly = true;
            ColorNm.Width = 120;
            ColorNm.TextAlignment = ContentAlignment.MiddleLeft;
            ColorNm.HeaderText = "Color";
            gv.Columns.Add(ColorNm);
                        
            GridViewTextBoxColumn Handler = new GridViewTextBoxColumn();
            Handler.Name = "Handler";
            Handler.FieldName = "Handler";
            Handler.ReadOnly = true;
            Handler.Width = 90;
            Handler.TextAlignment = ContentAlignment.MiddleLeft;
            Handler.HeaderText = "Order\nHandler";
            gv.Columns.Add(Handler);
            
            GridViewComboBoxColumn Status = new GridViewComboBoxColumn();
            Status.Name = "Status";
            Status.DataSource = lstStatus;
            Status.DisplayMember = "Contents";
            Status.ValueMember = "CodeIdx";
            Status.FieldName = "Status";
            Status.IsVisible = false;
            Status.ReadOnly = true;
            gv.Columns.Add(Status);

            #region Status columns 

            GridViewImageColumn Status01 = new GridViewImageColumn();
            Status01.Name = "S01";
            Status01.FieldName = "S01";
            Status01.HeaderText = trimName[0].Contents.ToString(); // "Trim1";
            Status01.ImageLayout = ImageLayout.Zoom;
            Status01.WrapText = true; 
            Status01.Width = 60;
            gv.Columns.Add(Status01);

            GridViewImageColumn Status02 = new GridViewImageColumn();
            Status02.Name = "S02";
            Status02.FieldName = "S02";
            Status02.HeaderText = trimName[1].Contents.ToString();
            Status02.ImageLayout = ImageLayout.Zoom;
            Status02.Width = 60;
            Status02.WrapText = true;
            gv.Columns.Add(Status02);

            GridViewImageColumn Status03 = new GridViewImageColumn();
            Status03.Name = "S03";
            Status03.FieldName = "S03";
            Status03.HeaderText = trimName[2].Contents.ToString();
            Status03.ImageLayout = ImageLayout.Zoom;
            Status03.Width = 60;
            Status03.WrapText = true;
            gv.Columns.Add(Status03);

            GridViewImageColumn Status04 = new GridViewImageColumn();
            Status04.Name = "S04";
            Status04.FieldName = "S04";
            Status04.HeaderText = trimName[3].Contents.ToString();
            Status04.ImageLayout = ImageLayout.Zoom;
            Status04.Width = 60;
            Status04.WrapText = true;
            gv.Columns.Add(Status04);

            GridViewImageColumn Status05 = new GridViewImageColumn();
            Status05.Name = "S05";
            Status05.FieldName = "S05";
            Status05.HeaderText = trimName[4].Contents.ToString();
            Status05.ImageLayout = ImageLayout.Zoom;
            Status05.Width = 60;
            Status05.WrapText = true;
            gv.Columns.Add(Status05);

            GridViewImageColumn Status06 = new GridViewImageColumn();
            Status06.Name = "S06";
            Status06.FieldName = "S06";
            Status06.HeaderText = trimName[5].Contents.ToString();
            Status06.ImageLayout = ImageLayout.Zoom;
            Status06.Width = 60;
            Status06.WrapText = true;
            gv.Columns.Add(Status06);

            GridViewImageColumn Status07 = new GridViewImageColumn();
            Status07.Name = "S07";
            Status07.FieldName = "S07";
            Status07.HeaderText = trimName[6].Contents.ToString();
            Status07.ImageLayout = ImageLayout.Zoom;
            Status07.Width = 60;
            Status07.WrapText = true;
            gv.Columns.Add(Status07);

            GridViewImageColumn Status08 = new GridViewImageColumn();
            Status08.Name = "S08";
            Status08.FieldName = "S08";
            Status08.HeaderText = trimName[7].Contents.ToString();
            Status08.ImageLayout = ImageLayout.Zoom;
            Status08.Width = 60;
            Status08.WrapText = true;
            gv.Columns.Add(Status08);

            GridViewImageColumn Status09 = new GridViewImageColumn();
            Status09.Name = "S09";
            Status09.FieldName = "S09";
            Status09.HeaderText = trimName[8].Contents.ToString();
            Status09.ImageLayout = ImageLayout.Zoom;
            Status09.Width = 60;
            Status09.WrapText = true;
            gv.Columns.Add(Status09);

            GridViewImageColumn Status10 = new GridViewImageColumn();
            Status10.Name = "S10";
            Status10.FieldName = "S10";
            Status10.HeaderText = trimName[9].Contents.ToString();
            Status10.ImageLayout = ImageLayout.Zoom;
            Status10.Width = 60;
            Status10.WrapText = true;
            gv.Columns.Add(Status10);

            GridViewImageColumn Status11 = new GridViewImageColumn();
            Status11.Name = "S11";
            Status11.FieldName = "S11";
            Status11.HeaderText = trimName[10].Contents.ToString();
            Status11.ImageLayout = ImageLayout.Zoom;
            Status11.Width = 60;
            Status11.WrapText = true;
            gv.Columns.Add(Status11);

            GridViewImageColumn Status12 = new GridViewImageColumn();
            Status12.Name = "S12";
            Status12.FieldName = "S12";
            Status12.HeaderText = trimName[11].Contents.ToString();
            Status12.ImageLayout = ImageLayout.Zoom;
            Status12.Width = 60;
            Status12.WrapText = true;
            gv.Columns.Add(Status12);

            GridViewImageColumn Status13 = new GridViewImageColumn();
            Status13.Name = "S13";
            Status13.FieldName = "S13";
            Status13.HeaderText = trimName[12].Contents.ToString();
            Status13.ImageLayout = ImageLayout.Zoom;
            Status13.Width = 60;
            Status13.WrapText = true;
            gv.Columns.Add(Status13);

            GridViewImageColumn Status14 = new GridViewImageColumn();
            Status14.Name = "S14";
            Status14.FieldName = "S14";
            Status14.HeaderText = trimName[13].Contents.ToString();
            Status14.ImageLayout = ImageLayout.Zoom;
            Status14.Width = 60;
            Status14.WrapText = true;
            gv.Columns.Add(Status14);

            GridViewImageColumn Status15 = new GridViewImageColumn();
            Status15.Name = "S15";
            Status15.FieldName = "S15";
            Status15.HeaderText = trimName[14].Contents.ToString();
            Status15.ImageLayout = ImageLayout.Zoom;
            Status15.Width = 60;
            Status15.WrapText = true;
            gv.Columns.Add(Status15);

            GridViewImageColumn Status16 = new GridViewImageColumn();
            Status16.Name = "S16";
            Status16.FieldName = "S16";
            Status16.HeaderText = trimName[15].Contents.ToString();
            Status16.ImageLayout = ImageLayout.Zoom;
            Status16.Width = 60;
            Status16.WrapText = true;
            gv.Columns.Add(Status16);

            GridViewImageColumn Status17 = new GridViewImageColumn();
            Status17.Name = "S17";
            Status17.FieldName = "S17";
            Status17.HeaderText = trimName[16].Contents.ToString();
            Status17.ImageLayout = ImageLayout.Zoom;
            Status17.Width = 60;
            Status17.WrapText = true;
            gv.Columns.Add(Status17);

            GridViewImageColumn Status18 = new GridViewImageColumn();
            Status18.Name = "S18";
            Status18.FieldName = "S18";
            Status18.HeaderText = trimName[17].Contents.ToString();
            Status18.ImageLayout = ImageLayout.Zoom;
            Status18.Width = 60;
            Status18.WrapText = true;
            gv.Columns.Add(Status18);

            GridViewImageColumn Status19 = new GridViewImageColumn();
            Status19.Name = "S19";
            Status19.FieldName = "S19";
            Status19.HeaderText = trimName[18].Contents.ToString();
            Status19.ImageLayout = ImageLayout.Zoom;
            Status19.Width = 60;
            Status19.WrapText = true;
            gv.Columns.Add(Status19);

            GridViewImageColumn Status20 = new GridViewImageColumn();
            Status20.Name = "S20";
            Status20.FieldName = "S20";
            Status20.HeaderText = trimName[19].Contents.ToString();
            Status20.ImageLayout = ImageLayout.Zoom;
            Status20.Width = 60;
            Status20.WrapText = true;
            gv.Columns.Add(Status20);

            #endregion

            #region Hidden value columns 

            GridViewTextBoxColumn Hidden01 = new GridViewTextBoxColumn(); Hidden01.Name = "Status01"; 
            Hidden01.FieldName = "Status01"; Hidden01.IsVisible = false; gv.Columns.Add(Hidden01);
            GridViewTextBoxColumn Hidden02 = new GridViewTextBoxColumn(); Hidden02.Name = "Status02";
            Hidden02.FieldName = "Status02"; Hidden02.IsVisible = false; gv.Columns.Add(Hidden02);
            GridViewTextBoxColumn Hidden03 = new GridViewTextBoxColumn(); Hidden03.Name = "Status03";
            Hidden03.FieldName = "Status03"; Hidden03.IsVisible = false; gv.Columns.Add(Hidden03);
            GridViewTextBoxColumn Hidden04 = new GridViewTextBoxColumn(); Hidden04.Name = "Status04";
            Hidden04.FieldName = "Status04"; Hidden04.IsVisible = false; gv.Columns.Add(Hidden04);
            GridViewTextBoxColumn Hidden05 = new GridViewTextBoxColumn(); Hidden05.Name = "Status05";
            Hidden05.FieldName = "Status05"; Hidden05.IsVisible = false; gv.Columns.Add(Hidden05);
            GridViewTextBoxColumn Hidden06 = new GridViewTextBoxColumn(); Hidden06.Name = "Status06";
            Hidden06.FieldName = "Status06"; Hidden06.IsVisible = false; gv.Columns.Add(Hidden06);
            GridViewTextBoxColumn Hidden07 = new GridViewTextBoxColumn(); Hidden07.Name = "Status07";
            Hidden07.FieldName = "Status07"; Hidden07.IsVisible = false; gv.Columns.Add(Hidden07);
            GridViewTextBoxColumn Hidden08 = new GridViewTextBoxColumn(); Hidden08.Name = "Status08";
            Hidden08.FieldName = "Status08"; Hidden08.IsVisible = false; gv.Columns.Add(Hidden08);
            GridViewTextBoxColumn Hidden09 = new GridViewTextBoxColumn(); Hidden09.Name = "Status09";
            Hidden09.FieldName = "Status09"; Hidden09.IsVisible = false; gv.Columns.Add(Hidden09);
            GridViewTextBoxColumn Hidden10 = new GridViewTextBoxColumn(); Hidden10.Name = "Status10";
            Hidden10.FieldName = "Status10"; Hidden10.IsVisible = false; gv.Columns.Add(Hidden10);

            GridViewTextBoxColumn Hidden11 = new GridViewTextBoxColumn(); Hidden11.Name = "Status11";
            Hidden11.FieldName = "Status11"; Hidden11.IsVisible = false; gv.Columns.Add(Hidden11);
            GridViewTextBoxColumn Hidden12 = new GridViewTextBoxColumn(); Hidden12.Name = "Status12";
            Hidden12.FieldName = "Status12"; Hidden12.IsVisible = false; gv.Columns.Add(Hidden12);
            GridViewTextBoxColumn Hidden13 = new GridViewTextBoxColumn(); Hidden13.Name = "Status13";
            Hidden13.FieldName = "Status13"; Hidden13.IsVisible = false; gv.Columns.Add(Hidden13);
            GridViewTextBoxColumn Hidden14 = new GridViewTextBoxColumn(); Hidden14.Name = "Status14";
            Hidden14.FieldName = "Status14"; Hidden14.IsVisible = false; gv.Columns.Add(Hidden14);
            GridViewTextBoxColumn Hidden15 = new GridViewTextBoxColumn(); Hidden15.Name = "Status15";
            Hidden15.FieldName = "Status15"; Hidden15.IsVisible = false; gv.Columns.Add(Hidden15);
            GridViewTextBoxColumn Hidden16 = new GridViewTextBoxColumn(); Hidden16.Name = "Status16";
            Hidden16.FieldName = "Status16"; Hidden16.IsVisible = false; gv.Columns.Add(Hidden16);
            GridViewTextBoxColumn Hidden17 = new GridViewTextBoxColumn(); Hidden17.Name = "Status17";
            Hidden17.FieldName = "Status17"; Hidden17.IsVisible = false; gv.Columns.Add(Hidden17);
            GridViewTextBoxColumn Hidden18 = new GridViewTextBoxColumn(); Hidden18.Name = "Status18";
            Hidden18.FieldName = "Status18"; Hidden18.IsVisible = false; gv.Columns.Add(Hidden18);
            GridViewTextBoxColumn Hidden19 = new GridViewTextBoxColumn(); Hidden19.Name = "Status19";
            Hidden19.FieldName = "Status19"; Hidden19.IsVisible = false; gv.Columns.Add(Hidden19);
            GridViewTextBoxColumn Hidden20 = new GridViewTextBoxColumn(); Hidden20.Name = "Status20";
            Hidden20.FieldName = "Status20"; Hidden20.IsVisible = false; gv.Columns.Add(Hidden20);

            #endregion

            #endregion
        }


        #endregion

        #region 3. 컨트롤 초기 설정


        /// <summary>
        /// 그리드뷰 설정
        /// </summary>
        /// <param name="gv">그리드뷰</param>
        private void GV1_LayoutSetting(RadGridView gv)
        {
            #region Config Gridview 

            gv.Dock = DockStyle.Fill;
            gv.AllowAddNewRow = false;
            gv.AllowCellContextMenu = true;
            gv.AutoSizeColumnsMode = GridViewAutoSizeColumnsMode.None;
            gv.AllowColumnHeaderContextMenu = false;
            gv.EnableGrouping = false;
            gv.MasterView.TableHeaderRow.MinHeight = 50;
            
            gv.GridViewElement.PagingPanelElement.NumericButtonsCount = 15;
            gv.GridViewElement.PagingPanelElement.ShowFastBackButton = false;
            gv.GridViewElement.PagingPanelElement.ShowFastForwardButton = false;

            gv.MultiSelect = true;

            #endregion

            #region Config Cell Conditions: 오더상태에 따라 행스타일 변경

            // 캔슬오더 색상변경
            Font f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Strikeout);
            ConditionalFormattingObject obj = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "2", "", true);
            obj.RowForeColor = Color.Black;
            obj.RowBackColor = Color.FromArgb(255, 255, 230, 230);
            obj.RowFont = f;
            gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj);

            // 마감오더 색상변경
            f = new Font(new FontFamily("Segoe UI"), 8.25f);
            ConditionalFormattingObject obj2 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "3", "", true);
            obj2.RowForeColor = Color.Black;
            obj2.RowBackColor = Color.FromArgb(255, 220, 255, 240);
            obj2.RowFont = f;
            gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj2);
            
            f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            ConditionalFormattingObject obj4 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "0", "", true);
            obj4.RowForeColor = Color.Black;
            obj4.RowFont = f;
            gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj4);

            f = new Font(new FontFamily("Segoe UI"), 8.25f, FontStyle.Regular);
            ConditionalFormattingObject obj5 = new ConditionalFormattingObject("MyCondition", ConditionTypes.Equal, "1", "", true);
            obj5.RowForeColor = Color.Black;
            obj5.RowFont = f;
            gv.Columns["Status"].ConditionalFormattingObjectList.Add(obj5);

            #endregion

        }

        #endregion
        
        /// <summary>
        /// 메인에서 신규자료가 입력된후, 입력된 행을 선택 포커싱한다 
        /// </summary>
        /// <param name="gv">선택하고자하는 그리드뷰</param>
        /// <param name="row">선택 행(Insert시 신규Idx를 리턴받는다)</param>
        private void SetCurrentRow(RadGridView gv, int row)
        {
            try
            {
                row = 0;
                // 페이징 되어 있는 경우, 제일 첫페이지로 이동한후, 
                if (CommonValues.enablePaging == true)
                {
                    gv.MasterTemplate.MoveToFirstPage();
                }
                gv.CurrentRow = gv.Rows[row];
                if (gv.CurrentRow != null)
                {
                    gv.CurrentRow.IsSelected = true;
                }

            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);

            }
        }
        
        #region 5. 데이터 조회 (바인딩 테스트후, 무겁고 느려서 직접 쿼리로 제어)

        /// <summary>
        /// Dropdownlist 데이터 로딩
        /// </summary>
        private void DataLoading_DDL()
        {
            // 부서    
            _dt = CommonController.Getlist(CommonValues.KeyName.DeptIdx).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                // 관리부와 임원은 모든 부서에 접근가능
                if (UserInfo.CenterIdx != 1 || UserInfo.DeptIdx == 5 || UserInfo.DeptIdx == 6)
                {
                    deptName.Add(new DepartmentName(Convert.ToInt32(row["DeptIdx"]),
                                                row["DeptName"].ToString(),
                                                Convert.ToInt32(row["CostcenterIdx"])));
                }
                // 영업부는 해당 부서 데이터만 접근가능
                else
                {
                    if (Convert.ToInt32(row["DeptIdx"]) <= 0 || Convert.ToInt32(row["DeptIdx"]) == UserInfo.DeptIdx)
                    {
                        deptName.Add(new DepartmentName(Convert.ToInt32(row["DeptIdx"]),
                                                row["DeptName"].ToString(),
                                                Convert.ToInt32(row["CostcenterIdx"])));
                    }
                }
            }

            // 바이어
            _dt = CommonController.Getlist(CommonValues.KeyName.CustAll).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                custName.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
            }
                        
            // Username
            lstUser.Add(new CustomerName(0, "", 0));

            if (UserInfo.CenterIdx != 1 || UserInfo.DeptIdx == 5 || UserInfo.DeptIdx == 6)
            {
                _dt = CommonController.Getlist(CommonValues.KeyName.AllUser).Tables[0];
            }
            else
            {
                _dt = CommonController.Getlist(CommonValues.KeyName.User).Tables[0];
            }

            foreach (DataRow row in _dt.Rows)
            {
                lstUser.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]),
                                            row["UserName"].ToString(),
                                            Convert.ToInt32(row["DeptIdx"])));
            }

            // 부자재 코드명
            _dt = CommonController.Getlist(CommonValues.KeyName.TrimCode).Tables[0];

            // trimName.Add(new CodeContents(0, "", ""));  
            foreach (DataRow row in _dt.Rows)
            {
                trimName.Add(new CodeContents(Convert.ToInt32(row["Idx"]), row["Contents"].ToString(), row["Classification"].ToString()));
            }
            
            // 오더상태 (CommonValues정의)
            lstStatus.Add(new CodeContents(0, CommonValues.DicTrimStatus[0], ""));
            lstStatus.Add(new CodeContents(1, CommonValues.DicTrimStatus[1], ""));
            lstStatus.Add(new CodeContents(2, CommonValues.DicTrimStatus[2], ""));
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
                if (Convert.ToInt32(ddlCust.SelectedValue) == 0 && Convert.ToInt32(ddlDept.SelectedValue) == 0 &&
                    Convert.ToInt32(ddlStatus.SelectedValue) == 0 && Convert.ToInt32(ddlHandler.SelectedValue) == 0 &&
                    string.IsNullOrEmpty(txtFileno.Text.Trim()) && string.IsNullOrEmpty(txtStyle.Text.Trim()))
                {
                    RadMessageBox.Show("Por favor ingresar condicion (numero de File o Estilo) para buscar.", "Noticiar", MessageBoxButtons.OK, 
                                        RadMessageIcon.Exclamation);
                    txtFileno.Select(); 
                    return; 
                }
                _searchKey = new Dictionary<CommonValues.KeyName, int>();

                // 영업부인경우, 해당 부서만 조회할수 있도록 제한 
                if (UserInfo.ReportNo < 9)
                    _searchKey.Add(CommonValues.KeyName.DeptIdx, UserInfo.DeptIdx);
                else
                    _searchKey.Add(CommonValues.KeyName.DeptIdx, Convert.ToInt32(ddlDept.SelectedValue));

                //_searchKey.Add(CommonValues.KeyName.CustIdx, Convert.ToInt32(ddlCust.SelectedValue));
                //_searchKey.Add(CommonValues.KeyName.Status, Convert.ToInt32(ddlStatus.SelectedValue));
                //_searchKey.Add(CommonValues.KeyName.Size, Convert.ToInt32(ddlSize.SelectedValue));

                //int OrderIdx, string WorksheetIdx, int Handler, int ConfirmUser, int WorkStatus

                DataBinding_GV1(_searchKey[CommonValues.KeyName.DeptIdx], Convert.ToInt32(ddlCust.SelectedValue), 
                                    Convert.ToInt32(ddlHandler.SelectedValue), 0, 
                                    txtFileno.Text.Trim(), txtStyle.Text.Trim(), Convert.ToInt32(ddlStatus.SelectedValue), 0);
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

        }

        /// <summary>
        /// 데이터 로딩 (메인)
        /// </summary>
        /// <param name="KeyCount">0:전체, 2:조건검색</param>
        /// <param name="SearchKey">RefleshWithCondition()에서 검색조건(key, value) 확인</param>
        /// <param name="fileno">검색조건: 파일번호</param>
        /// <param name="styleno">검색조건: 스타일번호</param>
        private void DataBinding_GV1(int DeptIdx, int CustIdx, int Handler, int Status, string Fileno, string Styleno, int WorkStatus, int OrderIdx)
        {
            try
            {
                _gv1.DataSource = null;
                                
                _ds1 = Controller.OrderTrim.Getlist(DeptIdx, CustIdx, Handler, Status, Fileno, Styleno, WorkStatus, OrderIdx);
                
                if (_ds1 != null)
                {
                    _gv1.DataSource = _ds1.Tables[0].DefaultView;
                    __main__.lblRows.Text = _gv1.RowCount.ToString() + " Rows"; 
                    _gv1.EnablePaging = CommonValues.enablePaging;
                    _gv1.AllowSearchRow = CommonValues.enableSearchRow; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("DataBindingGv1: " + ex.Message.ToString());
            }
        }
        
        
        #endregion

        #region 6. 컨트롤 이벤트 및 기타 설정
        
        /// <summary>
        /// 데이터 업데이트 (메인)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GV1_Update(object sender, GridViewCellEventArgs e)
        {
            _bRtn = false;
            try
            {
                ///// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                ///// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
                //int _mode_ = 1;
                //if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                //    CheckAuth.ShowMessage(_mode_);
                //else
                //{

                //    _gv1.EndEdit();
                //    GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);  // 현재 행번호 확인

                //    // 객체생성 및 값 할당
                //    _obj1 = new Controller.Worksheet(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value));
                //    _obj1.Idx = Convert.ToInt32(row.Cells["Idx"].Value.ToString());

                //    if (row.Cells["OrdSizeIdx"].Value != DBNull.Value) _obj1.OrdSizeIdx = Convert.ToInt32(row.Cells["OrdSizeIdx"].Value.ToString());

                //    if (row.Cells["TechpackDate"].Value != DBNull.Value && row.Cells["TechpackDate"] != null)
                //    {
                //        _obj1.TechpackDate = Convert.ToDateTime(row.Cells["TechpackDate"].Value);
                //    }
                //    else
                //    {
                //        _obj1.TechpackDate = new DateTime(2000, 1, 1);
                //    }

                //    if (row.Cells["RequestedDate"].Value != DBNull.Value) _obj1.RequestedDate = Convert.ToDateTime(row.Cells["RequestedDate"].Value);
                //    if (row.Cells["Requested"].Value != DBNull.Value) _obj1.Requested = Convert.ToInt32(row.Cells["Requested"].Value.ToString());

                //    if (row.Cells["ConfirmedDate"].Value != DBNull.Value && row.Cells["ConfirmedDate"] != null)
                //    {
                //        _obj1.ConfirmedDate = Convert.ToDateTime(row.Cells["ConfirmedDate"].Value);
                //    }
                //    else
                //    {
                //        _obj1.ConfirmedDate = new DateTime(2000, 1, 1);
                //    }

                //    if (row.Cells["Confirmed"].Value != DBNull.Value) _obj1.Confirmed = Convert.ToInt32(row.Cells["Confirmed"].Value.ToString());

                //    if (row.Cells["CompletedDate"].Value != DBNull.Value && row.Cells["CompletedDate"] != null)
                //    {
                //        _obj1.CompletedDate = Convert.ToDateTime(row.Cells["CompletedDate"].Value);
                //    }
                //    else
                //    {
                //        _obj1.CompletedDate = new DateTime(2000, 1, 1);
                //    }

                //    if (row.Cells["SentDate"].Value != DBNull.Value && row.Cells["SentDate"] != null)
                //    {
                //        _obj1.SentDate = Convert.ToDateTime(row.Cells["SentDate"].Value);
                //    }
                //    else
                //    {
                //        _obj1.SentDate = new DateTime(2000, 1, 1);
                //    }

                //    if (row.Cells["Received"].Value != DBNull.Value) _obj1.Received = Convert.ToInt32(row.Cells["Received"].Value.ToString());
                //    if (row.Cells["Remarks"].Value != DBNull.Value) _obj1.Remarks = row.Cells["Remarks"].Value.ToString();

                //    //if (row.Cells["Attached1"].Value != DBNull.Value) _obj1.Attached1 = row.Cells["Attached1"].Value.ToString();
                //    //if (row.Cells["Attached2"].Value != DBNull.Value) _obj1.Attached2 = row.Cells["Attached2"].Value.ToString();
                //    //if (row.Cells["Attached3"].Value != DBNull.Value) _obj1.Attached3 = row.Cells["Attached3"].Value.ToString();
                //    //if (row.Cells["Attached4"].Value != DBNull.Value) _obj1.Attached4 = row.Cells["Attached4"].Value.ToString();
                //    //if (row.Cells["Attached5"].Value != DBNull.Value) _obj1.Attached5 = row.Cells["Attached5"].Value.ToString();

                //    //if (row.Cells["AttachedUrl1"].Value != DBNull.Value) _obj1.AttachedUrl1 = row.Cells["AttachedUrl1"].Value.ToString();
                //    //if (row.Cells["AttachedUrl2"].Value != DBNull.Value) _obj1.AttachedUrl2 = row.Cells["AttachedUrl2"].Value.ToString();
                //    //if (row.Cells["AttachedUrl3"].Value != DBNull.Value) _obj1.AttachedUrl3 = row.Cells["AttachedUrl3"].Value.ToString();
                //    //if (row.Cells["AttachedUrl4"].Value != DBNull.Value) _obj1.AttachedUrl4 = row.Cells["AttachedUrl4"].Value.ToString();
                //    //if (row.Cells["AttachedUrl5"].Value != DBNull.Value) _obj1.AttachedUrl5 = row.Cells["AttachedUrl5"].Value.ToString();

                //    // 업데이트 (오더캔슬, 선적완료 상태가 아닐경우)
                //    _bRtn = _obj1.Update();
                //    __main__.lblRows.Text = "Updated Pattern Info";

                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine("GV1_Update: " + ex.Message.ToString());
            }
            
        }
         
        /// <summary>
        /// 그리드뷰 셀포맷팅 (메인) 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font f = new Font(new FontFamily("Segoe UI"), 8.25f);

            // 중앙 그리드뷰 헤더 폰트 변경
            GridHeaderCellElement element = sender as GridHeaderCellElement;
            if (element != null)
            {
                element.Font = f;
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
                RadMultiColumnComboBoxElement meditor = e.ActiveEditor as RadMultiColumnComboBoxElement;

                if (meditor != null)
                {
                    meditor.Enabled = false;
                }

                // DDL 높이, 출력항목수 설정
                RadDropDownListEditor editor = this._gv1.ActiveEditor as RadDropDownListEditor;
                if (editor != null)
                {
                    ((RadDropDownListEditorElement)((RadDropDownListEditor)this._gv1.ActiveEditor).EditorElement).DefaultItemsCountInDropDown
                        = CommonValues.DDL_DefaultItemsCountInDropDown;
                    ((RadDropDownListEditorElement)((RadDropDownListEditor)this._gv1.ActiveEditor).EditorElement).DropDownHeight
                        = CommonValues.DDL_DropDownHeight;
                }

                // 날짜컬럼의 달력크기 설정
                RadDateTimeEditor dtEditor = e.ActiveEditor as RadDateTimeEditor;
                if (dtEditor != null)
                {
                    RadDateTimeEditorElement el = dtEditor.EditorElement as RadDateTimeEditorElement;
                    //el.NullDate = new DateTime(2000, 1, 1);
                    //el.NullText = "";
                    el.CalendarSize = new Size(500, 400);

                    if (el.Value.ToString().Substring(0, 10) == "2000-01-01")
                    {
                        el.Value = Convert.ToDateTime(null);
                    }
                }
            }
            catch(Exception ex)
            {
                
            }

        }
        
        /// <summary>
        /// 현재 윈도우 종료시 그리드뷰 레이아웃 저장
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmOrderActual_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveGVLayout(); 
        }
        
        /// <summary>
        /// 행 선택시 - 오더캔슬, 마감일때 편집 불가능하도록
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvOrderActual_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (Int.Members.GetCurrentRow(_gv1, "Status") == 2
                || Int.Members.GetCurrentRow(_gv1, "Status") == 3)
                {
                    Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = true;
                }
                else
                {
                    Int.Members.GetCurrentRow(_gv1).ViewTemplate.ReadOnly = false;
                }
                
                GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);
                
                //if (row.Cells["Attached1"].Value != DBNull.Value) linkLabel1.Text = row.Cells["Attached1"].Value.ToString();
                //if (row.Cells["Attached2"].Value != DBNull.Value) linkLabel2.Text = row.Cells["Attached2"].Value.ToString();
                //if (row.Cells["Attached3"].Value != DBNull.Value) linkLabel3.Text = row.Cells["Attached3"].Value.ToString();
                //if (row.Cells["Attached4"].Value != DBNull.Value) linkLabel4.Text = row.Cells["Attached4"].Value.ToString();
                //if (row.Cells["Attached5"].Value != DBNull.Value) linkLabel5.Text = row.Cells["Attached5"].Value.ToString();
                //if (row.Cells["Attached6"].Value != DBNull.Value) linkLabel6.Text = row.Cells["Attached6"].Value.ToString();
                //if (row.Cells["Attached7"].Value != DBNull.Value) linkLabel7.Text = row.Cells["Attached7"].Value.ToString();
                //if (row.Cells["Attached8"].Value != DBNull.Value) linkLabel8.Text = row.Cells["Attached8"].Value.ToString();
                //if (row.Cells["Attached9"].Value != DBNull.Value) linkLabel9.Text = row.Cells["Attached9"].Value.ToString();
                
            }
            catch(Exception ex)
            {

            }

        }
        
        /// <summary>
        /// 업데이트 버튼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSaveData_Click(object sender, EventArgs e)
        {
            try
            {
                ///// 작업 수행하기 전에 해당 유저가 작업 권한 검사
                ///// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
                //int _mode_ = 1;
                //if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                //    CheckAuth.ShowMessage(_mode_);
                //else
                //{
                //    _gv1.EndEdit();
                //    GridViewRowInfo row = Int.Members.GetCurrentRow(_gv1);

                //    if (UserInfo.DeptIdx == 12)     // TD
                //    {
                //        _bRtn = Data.WorksheetData.ConfirmTD(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value),
                //        UserInfo.Idx, txtCommentTD.Text.Trim());

                //        if (_bRtn)
                //        {
                //            __main__.lblRows.Text = "Confirmed Worksheet by TD";
                //            RadMessageBox.Show("Confirmed Worksheet by TD.", "Confirmed TD");
                //        }
                //    } 
                //    else if (UserInfo.DeptIdx == 7 && UserInfo.IsLeader==1)     // 개발 총괄
                //    {
                //        _bRtn = Data.WorksheetData.ConfirmAdmin(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value),
                //        UserInfo.Idx, txtCommentTD.Text.Trim());

                //        if (_bRtn)
                //        {
                //            __main__.lblRows.Text = "Confirmed Worksheet by Admin";
                //            RadMessageBox.Show("Confirmed Worksheet by Admin.", "Confirmed");
                //        }
                //    }
                //    else if (UserInfo.DeptIdx == 7 && UserInfo.IsLeader != 1)     // 사무실
                //    {
                //        _bRtn = Data.WorksheetData.ConfirmOffice(Convert.ToInt32(_gv1.Rows[row.Index].Cells["Idx"].Value),
                //        UserInfo.Idx, txtCommentTD.Text.Trim());

                //        if (_bRtn)
                //        {
                //            __main__.lblRows.Text = "Confirmed Worksheet by Office";
                //            RadMessageBox.Show("Confirmed Worksheet by Office.", "Confirmed");
                //        }
                //    }

                //}

            }
            catch (Exception ex)
            {
                Console.WriteLine("btnUpdate_Click: " + ex.Message.ToString());
            }
        }
        
        #endregion

        #region 7. 기능 멤버 (그리드뷰 레이아웃 저장)

        /// <summary>
        /// 그리드뷰 레이아웃 복구 (/conf에 그리드별로 저장함) 
        /// </summary>
        private void LoadGVLayout()
        {
            // 레이아웃 xml 파일 위치 설정 
            string url = Environment.CurrentDirectory + "/conf";
            string path = url + _layoutfile;

            try
            {
                if (System.IO.File.Exists(path))
                {
                    _gv1.LoadLayout(path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// 현재 폼의 그리드뷰 레이아웃 저장 (form closed) 
        /// </summary>
        private void SaveGVLayout()
        {
            string url = Environment.CurrentDirectory + "/conf";
            string path = url + _layoutfile;

            try
            {
                _gv1.SaveLayout(path);
            }
            catch (Exception ex)
            {
                RadMessageBox.Show(ex.Message);
            }
        }



        #endregion

        private void LinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Tackpack 첨부파일 링크 
            try
            {
                CheckFolder(@"C:\INT\Data\intsampleworksheet");
                Download_File("intsampleworksheet", ((System.Windows.Forms.LinkLabel)sender).Text);
                Process process = new Process();
                process.StartInfo.FileName = @"C:\INT\Data\intsampleworksheet\" + ((System.Windows.Forms.LinkLabel)sender).Text.Trim();
                process.Start();
            }
            catch(Exception ex) {  }
        }
        
        private void Download_File(string containerurl, string filename)
        {
            try
            {
                // Retrieve storage account from connection string.
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));

                // Create the blob client.
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Retrieve reference to a previously created container.
                CloudBlobContainer container = blobClient.GetContainerReference(containerurl);

                // Retrieve reference to a blob named "photo1.jpg".
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(filename);

                // Save blob contents to a file.
                using (var fileStream = System.IO.File.OpenWrite(@"C:\INT\Data\" + containerurl + @"\" + filename))
                {
                    blockBlob.DownloadToStream(fileStream);
                }
            }
            catch(Exception ex)
            {

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

        private void gvOrderActual_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            // Overdue 
            if (e.CellElement.ColumnInfo.Name == "RequestedDate" || e.CellElement.ColumnInfo.Name == "ConfirmedDate" || 
                e.CellElement.ColumnInfo.Name == "CompletedDate")
            {
                e.CellElement.BackColor = Color.LightYellow;
                e.CellElement.ForeColor = Color.Black;
                e.CellElement.GradientStyle = GradientStyles.Solid;
                e.CellElement.DrawFill = true;
                e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
            }
            else
            {
                e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);


            }
            
        }

        private void LinkLabel_LinkClicked2(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // 요척 첨부파일 링크 
            //try
            //{
            //    CheckFolder(@"C:\INT\Data\intsampleconsum");
            //    Download_File("intsampleconsum", ((System.Windows.Forms.LinkLabel)sender).Text);
            //    Process process = new Process();
            //    process.StartInfo.FileName = @"C:\INT\Data\intsampleconsum\" + ((System.Windows.Forms.LinkLabel)sender).Text.Trim();
            //    process.Start();
            //}
            //catch (Exception ex) { }
        }

        /// <summary>
        /// azure storage 파일 업로드
        /// </summary>
        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            try
            {
                int tagNo = Convert.ToInt16(((RadBrowseEditor)sender).Tag); 
                //bool result = Data.UpdateData.DeleteAll(_selectedNode);

                // 스토리지 설정 
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                    CloudConfigurationManager.GetSetting("StorageConnectionString"));

                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // 선택된 패키지의 폴더안의 blob 리스트 조회 (updateinterp, updateintsample, ...) 
                CloudBlobContainer container = blobClient.GetContainerReference(CommonValues.packageName + "consum");

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

                            lstFiles[tagNo] = filename.Substring(filename.LastIndexOf("\\") + 1);
                            lstFileUrls[tagNo] =blockBlob.StorageUri.PrimaryUri.ToString();

                        }

                    }

                }
                RadMessageBox.Show("Uploaded completed.", "Saved"); 
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
            openDialog.RestoreDirectory = true;
            openDialog.Multiselect = false;
            openDialog.CheckFileExists = true;

            try
            {
                DialogResult result = openDialog.ShowDialog();

                if (result == DialogResult.OK && openDialog.FileNames.Length <= 1)
                {
                    //listFiles.Items.Clear();

                    //for (int i = 0; i < openDialog.FileNames.Length; i++)
                    //{
                    //    FileOpen_ListView(openDialog.FileNames[i], listFiles);
                    //}

                    return fileNames = openDialog.FileNames;

                }
                else if (result == DialogResult.Cancel)
                {
                    return null;
                }
                else
                {
                    if (MessageBox.Show("Too many files were Selected. Please select files less than 5.",
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

        private void gvWorksheet_CellEditorInitialized(object sender, GridViewCellEventArgs e)
        {
            try
            {
                RadMultiColumnComboBoxElement meditor = e.ActiveEditor as RadMultiColumnComboBoxElement;

                if (meditor != null)
                {
                    meditor.Enabled = false;
                }

                // DDL 높이, 출력항목수 설정
                RadDropDownListEditor editor = this._gv1.ActiveEditor as RadDropDownListEditor;
                if (editor != null)
                {
                    ((RadDropDownListEditorElement)((RadDropDownListEditor)this._gv1.ActiveEditor).EditorElement).DefaultItemsCountInDropDown
                        = CommonValues.DDL_DefaultItemsCountInDropDown;
                    ((RadDropDownListEditorElement)((RadDropDownListEditor)this._gv1.ActiveEditor).EditorElement).DropDownHeight
                        = CommonValues.DDL_DropDownHeight;
                }

                // 날짜컬럼의 달력크기 설정
                RadDateTimeEditor dtEditor = e.ActiveEditor as RadDateTimeEditor;
                if (dtEditor != null)
                {
                    RadDateTimeEditorElement el = dtEditor.EditorElement as RadDateTimeEditorElement;
                    //el.NullDate = new DateTime(2000, 1, 1);
                    //el.NullText = "";
                    el.CalendarSize = new Size(500, 400);

                    if (el.Value.ToString().Substring(0, 10) == "2000-01-01")
                    {
                        el.Value = Convert.ToDateTime(null);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void gvWorksheet_CellFormatting(object sender, CellFormattingEventArgs e)
        {
            //  
            if (e.CellElement.ColumnInfo.Name == "ConfirmUser" || e.CellElement.ColumnInfo.Name == "ConfirmDate" )
            {
                e.CellElement.BackColor = Color.LightYellow;
                e.CellElement.ForeColor = Color.Black;
                e.CellElement.GradientStyle = GradientStyles.Solid;
                e.CellElement.DrawFill = true;
                e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
            }
            else if (e.CellElement.ColumnInfo.Name == "ConfirmUserTD" || e.CellElement.ColumnInfo.Name == "ConfirmDateTD")
            {
                e.CellElement.BackColor = Color.PaleGreen;
                e.CellElement.ForeColor = Color.Black;
                e.CellElement.GradientStyle = GradientStyles.Solid;
                e.CellElement.DrawFill = true;
                e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
            }
            else if (e.CellElement.ColumnInfo.Name == "RejectDate" || e.CellElement.ColumnInfo.Name == "Rejected")
            {
                e.CellElement.BackColor = Color.White;
                e.CellElement.ForeColor = Color.Red;
                e.CellElement.GradientStyle = GradientStyles.Solid;
                e.CellElement.DrawFill = true;
                e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
            }
            else if (e.CellElement.ColumnInfo.Name == "RegDate" || e.CellElement.ColumnInfo.Name == "Handler")
            {
                e.CellElement.BackColor = Color.SeaShell;
                e.CellElement.ForeColor = Color.Black;
                e.CellElement.GradientStyle = GradientStyles.Solid;
                e.CellElement.DrawFill = true;
                e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
            }
            else if (e.CellElement.ColumnInfo.Name == "ConfirmUserLast" || e.CellElement.ColumnInfo.Name == "ConfirmDateLast")
            {
                e.CellElement.BackColor = Color.AliceBlue;
                e.CellElement.ForeColor = Color.Black;
                e.CellElement.GradientStyle = GradientStyles.Solid;
                e.CellElement.DrawFill = true;
                e.CellElement.TextAlignment = ContentAlignment.MiddleCenter;
            }
            else
            {
                e.CellElement.ResetValue(LightVisualElement.BackColorProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.DrawFillProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.GradientStyleProperty, ValueResetFlags.Local);
                e.CellElement.ResetValue(LightVisualElement.ForeColorProperty, ValueResetFlags.Local);
            }

            
            if (e.CellElement.ColumnInfo.Name == "S01")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status01"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status01"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S02")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status02"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status02"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S03")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status03"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status03"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S04")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status04"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status04"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S05")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status05"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status05"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S06")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status06"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status06"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S07")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status07"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status07"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S08")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status08"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status08"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S09")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status09"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status09"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S10")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status10"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status10"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }

            else if (e.CellElement.ColumnInfo.Name == "S11")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status11"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status11"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S12")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status12"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status12"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S13")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status13"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status13"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S14")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status14"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status14"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S15")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status15"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status15"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S16")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status16"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status16"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S17")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status17"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status17"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S18")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status18"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status18"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S19")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status19"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status19"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
            else if (e.CellElement.ColumnInfo.Name == "S20")
            {
                if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status20"].Value) == 0) e.CellElement.Image = imagesList.Images[0];
                else if (Convert.ToInt32(e.CellElement.RowInfo.Cells["Status20"].Value) == 1) e.CellElement.Image = imagesList.Images[1];
                else e.CellElement.Image = imagesList.Images[2];
            }
        }

        private void gvWorksheet_CellValueChanged(object sender, GridViewCellEventArgs e)
        {
            
        }

        private void gvWorksheet_ViewCellFormatting(object sender, CellFormattingEventArgs e)
        {
            Font f = new Font(new FontFamily("Segoe UI"), 8.25f);

            // 중앙 그리드뷰 헤더 폰트 변경
            GridHeaderCellElement element = sender as GridHeaderCellElement;
            if (element != null)
            {
                element.Font = f;
            }
        }

        /// <summary>
        /// 각 상태 셀 더블클릭시 상태값/코멘트/날짜등을 수정하기 위한 새 창을 띄워서 수정
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gvWorksheet_CellDoubleClick(object sender, GridViewCellEventArgs e)
        {
            /// 작업 수행하기 전에 해당 유저가 작업 권한 검사
            /// 읽기: 0, 쓰기: 1, 삭제: 2, 센터: 3, 부서: 4
            int _mode_ = 1;
            if (Convert.ToInt16(__AUTHCODE__.Substring(_mode_, 1).Trim()) <= 0)
                CheckAuth.ShowMessage(_mode_);
            else
            {
                _bRtn = false;
                
                if (e.ColumnIndex >= 8 && e.ColumnIndex <= 27)
                {
                    //Console.WriteLine(e.Row.Cells["Idx"].Value.ToString());
                    //Console.WriteLine(e.Row.Cells["Status" + string.Format("{0:d2}", e.ColumnIndex-6)].Value.ToString());

                    try
                    {
                        _bRtn = Dev.Controller.OrderTrim.Update(Convert.ToInt32(e.Row.Cells["Idx"].Value.ToString()),
                                                                e.ColumnIndex - 7,
                                                                Convert.ToInt32(e.Row.Cells["Status" + string.Format("{0:d2}", e.ColumnIndex - 7)].Value.ToString()));

                        __main__.lblRows.Text = "Updated Trim Status Info";

                        if (_bRtn) RefleshWithCondition(); 
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("GV1_Update: " + ex.Message.ToString());
                    }
                }
            }
        }

        private void txtFileno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }

        private void txtStyle_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.PerformClick();
            }
        }
    }
}
