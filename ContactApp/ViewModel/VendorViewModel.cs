using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ContactApp.Model;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using System.Data;
using ContactApp.Database;
using ContactApp.Command;

namespace ContactApp.ViewModel
{
    class VendorViewModel : PropertyChangedHelper, IPanelViewModel
    {
        public VendorViewModel()
        {
            VendorInfo = new VendorInfoModel();
            NewCompanyInfo = new CompanyInfoModel();
            VendorDatabase = new VendorDatabase();
            CompanyDatabase = new CompanyDatabase();

            // Create Databases
            VendorDatabase.CreateDatabase();
            CompanyDatabase.CreateDatabase();

            // Read from database and fill out grid
            SavedVendorInfo = VendorDatabase.ReadFromDatabase() as DataTable;

            // Read Company List from database
            Companies = CompanyDatabase.ReadFromDatabase() as List<String>;

            // Default AddCompany to false
            AddCompany = false;
        }

        public String Name
        {
            get { return "Vendor"; }
        }

        private VendorInfoModel vendorInfo;
        public VendorInfoModel VendorInfo
        {
            get { return vendorInfo; }
            set
            {
                if (vendorInfo != value)
                {
                    vendorInfo = value;
                    OnPropertyChanged("VendorInfo");
                }
            }
        }

        private CompanyInfoModel newCompanyInfo;
        public CompanyInfoModel NewCompanyInfo
        {
            get { return newCompanyInfo; }
            set
            {
                if (newCompanyInfo != value)
                {
                    newCompanyInfo = value;
                    OnPropertyChanged("NewCompanyInfo");
                }
            }
        }

        public VendorDatabase VendorDatabase
        {
            get;
        }

        public CompanyDatabase CompanyDatabase
        {
            get;
        }

        private DataTable savedVendorInfo;
        public DataTable SavedVendorInfo
        {
            get
            {
                return savedVendorInfo;
            }
            set
            {
                if (savedVendorInfo != value)
                {
                    savedVendorInfo = value;
                    OnPropertyChanged("SavedVendorInfo");
                }
            }
        }


        private String selectedCompany;
        public String SelectedCompany
        {
            get
            {
                return selectedCompany;
            }
            set
            {
                if (selectedCompany != value)
                {
                    selectedCompany = value;
                    OnPropertyChanged("SelectedCompany");
                    OnSelectedCompanyChanged();
                }
            }
        }

        private void OnSelectedCompanyChanged()
        {
            if (SelectedCompany != CompanyDatabase.AddNewCompanyText)
            {
                VendorInfo.Company = SelectedCompany;
                AddCompany = false;
            }
            else
            {
                AddCompany = true;
            }
        }

        private List<String> companies;
        public List<String> Companies
        {
            get
            {
                if (companies == null)
                    companies = new List<String>();
                return companies;
            }
            set
            {
                if (companies != value)
                {
                    companies = value;
                    OnPropertyChanged("Companies");
                }
            }
        }
 
        private Boolean addCompany;
        public Boolean AddCompany
        {
            get { return addCompany; }
            set
            {
                if (addCompany != value)
                {
                    addCompany = value;
                    OnPropertyChanged("AddCompany");
                }    
            }
        }

        // Save Command
        private ICommand saveCommand;
        public ICommand SaveCommand
        {
            get
            {
                if (saveCommand == null)
                {
                    // Save to Database
                    saveCommand = new RelayCommand(o => OnSave(),
                        o => !IsDataEmpty());
                }
                return saveCommand;
            }
        }

        private void OnSave()
        {      
            if (AddCompany)
            {
                // Check if company is already in database
                if (!CompanyDatabase.CompanyExist(NewCompanyInfo))
                {
                    // Set Vendor Info to New Company
                    VendorInfo.Company = NewCompanyInfo.Company;

                    // Add New Company to Company List
                    CompanyDatabase.SaveToDatabase(NewCompanyInfo);

                    // Read Company List from database
                    Companies = CompanyDatabase.ReadFromDatabase() as List<String>;
                }
                else
                {
                    // Let user know company is already in database and do not finish save command
                    MessageBox.Show("Company or Vendor Code is already in the database!", "Canceling Save");
                    return;
                }
            }

            // Save Vendor Contact Info to Database
            VendorDatabase.SaveToDatabase(VendorInfo);

            // Read Vendor Info from Database
            SavedVendorInfo = VendorDatabase.ReadFromDatabase() as DataTable;

            // Reset Add Company
            AddCompany = false;

            // Reset Form
            ClearVendorInfo();
            ClearNewCompanyInfo();
            SelectedCompany = null;
        }

        private Boolean IsDataEmpty()
        {
            if (String.IsNullOrEmpty(VendorInfo.Name)    ||
                (String.IsNullOrEmpty(VendorInfo.Company) && !AddCompany)  ||
                (AddCompany && (String.IsNullOrEmpty(NewCompanyInfo.Company) || String.IsNullOrEmpty(NewCompanyInfo.VendorCode))) ||
                String.IsNullOrEmpty(VendorInfo.Address) ||
                String.IsNullOrEmpty(VendorInfo.PhoneNumber))
                return true;
            else 
                return false;
        }

        private void ClearVendorInfo()
        {
            VendorInfo.Name = null;
            VendorInfo.Company = null;
            VendorInfo.Address = null;
            VendorInfo.PhoneNumber = null;         
        }

        private void ClearNewCompanyInfo()
        {
            NewCompanyInfo.Company = null;
            NewCompanyInfo.VendorCode = null;
        }
    }
}
