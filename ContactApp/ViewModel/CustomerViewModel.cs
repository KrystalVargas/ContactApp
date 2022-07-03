using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


using Microsoft.Data.SqlClient;
using System.Data;
using ContactApp.Model;
using ContactApp.Database;
using ContactApp.Command;

namespace ContactApp.ViewModel
{
    class CustomerViewModel : PropertyChangedHelper, IPanelViewModel
    {
        public CustomerViewModel()
        {
            CustomerInfo = new CustomerInfoModel();
            CustomerDatabase = new CustomerDatabase();

            CustomerDatabase.CreateDatabase();

            // Read from database and fill out grid
            SavedCustomerInfo = CustomerDatabase.ReadFromDatabase() as DataTable;
        }

        public String Name
        {
            get { return "Customer"; }
        }

        private CustomerInfoModel customerInfo;
        public CustomerInfoModel CustomerInfo
        {
            get { return customerInfo; }
            set
            {
                if (customerInfo != value)
                {
                    customerInfo = value;
                    OnPropertyChanged("CustomerInfo");
                }
            }
        }

        public CustomerDatabase CustomerDatabase
        {
            get;
        }

        private DataTable savedCustomerInfo;
        public DataTable SavedCustomerInfo
        {
            get
            {
                return savedCustomerInfo;
            }
            set
            {
                if (savedCustomerInfo != value)
                {
                    savedCustomerInfo = value;
                    OnPropertyChanged("SavedCustomerInfo");
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
            //Save Info to Database
            CustomerDatabase.SaveToDatabase(CustomerInfo);

            // Read back from Database
            SavedCustomerInfo = CustomerDatabase.ReadFromDatabase() as DataTable;

            // Reset form
            ClearCustomerInfo();
        }

        private Boolean IsDataEmpty()
        {
            if (String.IsNullOrEmpty(CustomerInfo.Name)    ||
                String.IsNullOrEmpty(CustomerInfo.Company) ||
                String.IsNullOrEmpty(CustomerInfo.Address) ||
                String.IsNullOrEmpty(CustomerInfo.PhoneNumber))
                return true;
            else return false;
        }

        private void ClearCustomerInfo()
        {
            CustomerInfo.Name = null;
            CustomerInfo.Company = null;
            CustomerInfo.Address = null;
            CustomerInfo.PhoneNumber = null;
            CustomerInfo.Notes = null;
        }
    }
}
