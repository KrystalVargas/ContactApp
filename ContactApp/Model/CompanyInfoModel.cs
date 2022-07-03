using System;

namespace ContactApp.Model
{
    class CompanyInfoModel : PropertyChangedHelper
    {
        public String Company
        {
            get { return company; }
            set
            {
                if (company != value)
                {
                    company = value;
                    OnPropertyChanged("Company");
                }
            }
        }

        public String VendorCode
        {
            get { return vendorCode; }
            set
            {
                if (vendorCode != value)
                {
                    vendorCode = value;
                    OnPropertyChanged("VendorCode");
                }
            }
        }

        private String company;
        private String vendorCode;
    }
}
