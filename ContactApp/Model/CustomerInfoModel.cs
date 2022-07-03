using System;

namespace ContactApp.Model
{
    class CustomerInfoModel : ContactInfoBaseModel
    {
        public String Notes
        {
            get { return notes; }
            set
            {
                if (notes != value)
                {
                    notes = value;
                    OnPropertyChanged("Notes");
                }
            }
        }
        private String notes;
    }
}
