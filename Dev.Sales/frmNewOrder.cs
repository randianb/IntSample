using Int.Code;
using Int.Customer;
using Int.Department;
using Dev.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telerik.WinControls;
using Telerik.WinControls.UI;

namespace Dev.Sales
{
    public partial class frmNewOrder : RadForm
    {
        #region member variables 

        RadForm _parent = null;
        private List<DepartmentName> deptName = new List<DepartmentName>();     // 부서
        private List<CustomerName> custName = new List<CustomerName>();         // 거래처
        private List<CustomerName> custName2 = new List<CustomerName>();         // 거래처
        private List<CodeContents> codeName = new List<CodeContents>();         // 코드
        private List<CodeContents> lstIsPrinting = new List<CodeContents>();    // 나염여부
        private List<CustomerName> embName1 = new List<CustomerName>();          // 나염업체
        private List<CustomerName> embName2 = new List<CustomerName>();          // 나염업체
        private List<Codes.Controller.SizeGroup> lstSizeGroup = new List<Codes.Controller.SizeGroup>();      // 사이즈그룹
        private List<CustomerName> lstSewthread = new List<CustomerName>();         // sewthread
        private List<CodeContents> lstColor = new List<CodeContents>();         // 컬러

        private Dictionary<int, List<Codes.Controller.Sizes>> lstSizes =
            new Dictionary<int, List<Codes.Controller.Sizes>>();
        private List<CustomerName> lstUser = new List<CustomerName>();         // 유저명

        #endregion   

        public frmNewOrder(RadForm frm)
        {
            InitializeComponent();
            _parent = frm as OrderMain;
            dtIssueDate.Value = DateTime.Now;
            dtDelivery.Value = DateTime.Now;
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(ddlDept.SelectedValue)<=0 || Convert.ToInt32(ddlCust.SelectedValue) <= 0 ||
                    string.IsNullOrWhiteSpace(txtStyleno.Text))
                {
                    RadMessageBox.Show("Please input the Department / Date / Buyer / Brand / Style#", "Validation Warning", MessageBoxButtons.OK,
                        RadMessageIcon.Error);
                    return;     
                }

                DataRow row = Controller.Orders.Insert(txtFileno.Text, 
                    Convert.ToInt32(ddlDept.SelectedValue), 0, "", 
                    dtIssueDate.Value, 
                    Convert.ToInt32(ddlCust.SelectedValue),  
                    txtPono.Text, txtStyleno.Text, 
                    txtSeason.Text, txtDesc.Text, dtDelivery.Value,
                    Convert.ToInt32(ddlPrinting.SelectedValue), 
                    Convert.ToInt32(ddlEmb1.SelectedValue), 
                    Convert.ToInt32(ddlEmb2.SelectedValue),
                    Convert.ToInt32(ddlSizeGroup.SelectedValue),
                    Convert.ToInt32(ddlSewThread.SelectedValue),
                    Convert.ToInt32(txtQty.Value), Convert.ToDouble(txtUprice.Value),
                    Convert.ToDouble(txtAmount.Value), txtRemark.Text, 
                    DateTime.Now, DateTime.Now, 
                    Convert.ToInt32(ddlHandler.SelectedValue));
                    
                ((OrderMain)_parent).InsertedOrderRow = row;
                this.Close();
            }
            catch(Exception ex)
            {
                RadMessageBox.Show(ex.Message); 
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        /// <summary>
        /// Dropdownlist 데이터 로딩
        /// </summary>
        private void DataLoading_DDL()
        {
            DataTable _dt = null; 

            // 부서    
            _dt = CommonController.Getlist(CommonValues.KeyName.DeptIdx).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                // 관리부와 임원은 모든 부서에 접근가능
                if (UserInfo.ReportNo >= 9)
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
            _dt = CommonController.Getlist(CommonValues.KeyName.CustIdx).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                custName.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
                custName2.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
            }

            // 나염업체
            embName1.Add(new CustomerName(0, "", 0));
            embName2.Add(new CustomerName(0, "", 0));
            _dt = CommonController.Getlist(CommonValues.KeyName.EmbelishId1).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                embName1.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
                embName2.Add(new CustomerName(Convert.ToInt32(row["CustIdx"]),
                                            row["CustName"].ToString(),
                                            Convert.ToInt32(row["Classification"])));
            }


            // 사이즈 그룹 데이터로딩 후, 데이터테이블에 추가하고 바인딩해준다 bindingSourceSizeGroup 
            // MultipleColumnComboBox의 DataMember 설정하기 위해 필요 
            _dt = Codes.Controller.SizeGroup.GetlistName().Tables[0]; //Getlist(0).Tables[0];
            foreach (DataRow row in _dt.Rows)
            {
                dataSetSizeGroup.DataTableSizeGroup.Rows.Add(Convert.ToInt32(row["SizeGroupIdx"]),
                                            row["Client"].ToString(),
                                            row["SizeGroupName"].ToString(),
                                            row["SizeIdx1"].ToString(),
                                            row["SizeIdx2"].ToString(),
                                            row["SizeIdx3"].ToString(),
                                            row["SizeIdx4"].ToString(),
                                            row["SizeIdx5"].ToString(),
                                            row["SizeIdx6"].ToString(),
                                            row["SizeIdx7"].ToString(),
                                            row["SizeIdx8"].ToString()); 
            }
            
            // 코드
            _dt = CommonController.Getlist(CommonValues.KeyName.Codes).Tables[0];
            codeName.Add(new CodeContents(0, "", ""));
            foreach (DataRow row in _dt.Rows)
            {

                codeName.Add(new CodeContents(Convert.ToInt32(row["Idx"]),
                                            row["Contents"].ToString(),
                                            row["Classification"].ToString()));
            }

            // lstIsPrinting 
            lstIsPrinting.Clear();
            lstIsPrinting = codeName.FindAll(
                delegate (CodeContents code)
                {
                    return code.CodeIdx == 0 || code.Classification == "IsPrinting";
                });
            
            // Sewthread
            //lstSewthread.Add(new CustomerName(0, "", 0));
            _dt = Codes.Controller.SewThread.GetUsablelist(); // CommonController.Getlist(CommonValues.KeyName.SewThread).Tables[0];
            
            foreach (DataRow row in _dt.Rows)
            {
                dataSetSizeGroup.DataTableSewThread.Rows.Add(Convert.ToInt32(row["SewThreadIdx"]),
                                            row["SewThreadCustIdx"].ToString(),
                                            row["SewThreadName"].ToString(),
                                            row["ColorIdx"].ToString());
            }

            // Username
            lstUser.Add(new CustomerName(0, "", 0));
            _dt = CommonController.Getlist(CommonValues.KeyName.User).Tables[0];

            foreach (DataRow row in _dt.Rows)
            {
                lstUser.Add(new CustomerName(Convert.ToInt32(row["UserIdx"]),
                                            row["UserName"].ToString(),
                                            Convert.ToInt32(row["DeptIdx"])));
            }

            // color
            _dt = Dev.Codes.Controller.Color.GetUselist().Tables[0]; 
            foreach (DataRow row in _dt.Rows)
            {
                lstColor.Add(new CodeContents(Convert.ToInt32(row["ColorIdx"]),
                                            row["ColorName"].ToString(),
                                            row["Classification"].ToString()));
            }
        }

        private void Config_DropDownList()
        {
            // 부서 
            ddlDept.DataSource = deptName;
            ddlDept.DisplayMember = "DeptName";
            ddlDept.ValueMember = "DeptIdx";
            ddlDept.DefaultItemsCountInDropDown = CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlDept.DropDownHeight = CommonValues.DDL_DropDownHeight;

            // 바이어
            ddlCust.DataSource = custName;
            ddlCust.DisplayMember = "CustName";
            ddlCust.ValueMember = "CustIdx";
            ddlCust.DefaultItemsCountInDropDown = CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlCust.DropDownHeight = CommonValues.DDL_DropDownHeight;

            
            // 나염
            ddlEmb1.DataSource = embName1;
            ddlEmb1.DisplayMember = "CustName";
            ddlEmb1.ValueMember = "CustIdx";
            ddlEmb1.DefaultItemsCountInDropDown = CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlEmb1.DropDownHeight = CommonValues.DDL_DropDownHeight;

            // 나염
            ddlEmb2.DataSource = embName2;
            ddlEmb2.DisplayMember = "CustName";
            ddlEmb2.ValueMember = "CustIdx";
            ddlEmb2.DefaultItemsCountInDropDown = CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlEmb2.DropDownHeight = CommonValues.DDL_DropDownHeight;
                                    
            this.ddlSizeGroup.AutoSizeDropDownToBestFit = true;
            this.ddlSizeGroup.AutoSizeDropDownHeight = true;
            this.ddlSizeGroup.SelectedIndex = 0; 

            // lstIsPrinting
            ddlPrinting.DataSource = lstIsPrinting;
            ddlPrinting.DisplayMember = "Contents";
            ddlPrinting.ValueMember = "CodeIdx";
            ddlPrinting.DefaultItemsCountInDropDown = CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlPrinting.DropDownHeight = CommonValues.DDL_DropDownHeight;
            
            // ddlSewThread
            ddlSewThread.AutoSizeDropDownToBestFit = true;
            ddlSewThread.AutoSizeDropDownHeight = true;
            ddlSewThread.SelectedIndex = 0;

            // Handler
            ddlHandler.DataSource = lstUser;
            ddlHandler.DisplayMember = "CustName";
            ddlHandler.ValueMember = "CustIdx";
            ddlHandler.DefaultItemsCountInDropDown = CommonValues.DDL_DefaultItemsCountInDropDown;
            ddlHandler.DropDownHeight = CommonValues.DDL_DropDownHeight;

        }


        private void frmNewOrder_Load(object sender, EventArgs e)
        {
            DataLoading_DDL(); 
            Config_DropDownList(); 
        }
        
        private void txtQty_ValueChanged(object sender, EventArgs e)
        {
            txtAmount.Value = (txtQty.Value * txtUprice.Value);
        }

        private void txtUprice_ValueChanged(object sender, EventArgs e)
        {
            txtAmount.Value = (txtQty.Value * txtUprice.Value);
        }

        private void btnCreateFileno_Click(object sender, EventArgs e)
        {
            string NewCode = Code.GetPrimaryCode(UserInfo.CenterIdx, UserInfo.DeptIdx, 2, "");
            txtFileno.Text = NewCode; 
        }

        
        
    }
}
