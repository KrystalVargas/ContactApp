using System;
using System.Collections.Generic;
using System.Windows.Threading;
using ContactApp.ViewModel;

namespace ContactApp
{
    class MainWindowViewModel : PropertyChangedHelper
    {
        public MainWindowViewModel()
        {
            // Create List of View Models
            ViewModels.Add(new CustomerViewModel());
            ViewModels.Add(new VendorViewModel());

            // Set selected view model to first
            SelectedViewModel = ViewModels[0];

            // Create & Start timer that updates time every second
            timer = new DispatcherTimer(DispatcherPriority.Normal);
            timer.Tick += new EventHandler(OnTimerTick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        // Update current time string every second
        private void OnTimerTick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss");
        }

        // View Model List
        private List<IPanelViewModel> viewModels;
        public List<IPanelViewModel> ViewModels
        {
            get
            {
                if (viewModels == null)
                    viewModels = new List<IPanelViewModel>();
                return viewModels;
            }
        }

        // Selected View Model
        private IPanelViewModel selectedViewModel;
        public IPanelViewModel SelectedViewModel
        {
            get { return selectedViewModel; }
            set
            {
                if (selectedViewModel != value)
                {
                    selectedViewModel = value;
                    OnPropertyChanged("SelectedViewModel");
                }
            }
        }

        // Hold current time in string
        private String currentTime;
        public String CurrentTime
        {
            get { return currentTime; }
            set
            {
                if (currentTime != value)
                {
                    currentTime = value;
                    OnPropertyChanged("CurrentTime");
                }
            }
        }

        // DispatcherTimer
        private DispatcherTimer timer;
    }
}
