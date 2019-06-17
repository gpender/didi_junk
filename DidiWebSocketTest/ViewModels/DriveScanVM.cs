using DidiWebSocketTest.Commands;
using Parker.DctEthernetComms;
using Parker.DctEthernetComms.Interfaces;
using Parker.DctEthernetComms.DCT.Messages;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace DidiWebSocketTest.ViewModels
{
    public class DriveScanVM : BaseVM
    {
        RelayCommand<DriveScanCommandName> driveScanCommand;
        IDriveBrowser3 driveBrowser;
        public ICollectionView DriveList
        {
            get
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(driveBrowser.DriveList);
                view.SortDescriptions.Add(new SortDescription("DriveName", ListSortDirection.Ascending));
                return view;
            }
        }
        public ISimpleDriveInfo SelectedDrive
        {
            get { return driveBrowser.SelectedDrive; }
            set
            {
                driveBrowser.SelectedDrive = value;
                OnPropertyChanged("SelectedDrive");
                OnPropertyChanged("DriveNotSelected");
            }
        }
        public DriveScanVM(IDriveBrowser3 driveBrowser)
        {
            this.driveBrowser = driveBrowser;
            this.driveBrowser.OnDriveSelected += DriveBrowser_OnDriveSelected;
        }

        private void DriveBrowser_OnDriveSelected(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
        }

        #region Commands
        public ICommand DriveScanCommand
        {
            get
            {
                if (driveScanCommand == null)
                {
                    driveScanCommand = new RelayCommand<DriveScanCommandName>(ExecuteDriveScanCommand, CanExecuteDriveScanCommand);
                }
                return driveScanCommand;
            }
        }
        bool CanExecuteDriveScanCommand(DriveScanCommandName messageType)
        {
            return true;
        }
        void ExecuteDriveScanCommand(DriveScanCommandName messageType)
        {
            switch (messageType)
            {
                case DriveScanCommandName.FIND:
                    driveBrowser.FindDrives();
                    //messages.Clear();
                    //parkerUdpClientManager.FindDrives(false);
                    break;
                //case ScopeCommandName.STOP:
                //    scope.Stop();
                //    break;
                //case ScopeCommandName.PAUSE:
                //    scope.Pause();
                //    break;
                //case ScopeCommandName.GETMENUS:
                //    parameterProtocol.GetMenus();
                //    break;
            }
        }
        #endregion
    }
}
