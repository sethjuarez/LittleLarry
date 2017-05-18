using System;

using GalaSoft.MvvmLight;
using LittleLarry.Model.Services;
using System.Collections.ObjectModel;
using LittleLarry.Model;
using GalaSoft.MvvmLight.Command;

namespace LittleLarry.ViewModels
{
    public class DataViewModel : ViewModelBase
    {
        private ObservableCollection<Data> _dataCollection;
        public ObservableCollection<Data> DataCollection => _dataCollection;

        private bool _isRefreshing;
        private RelayCommand _refreshCommand;
        public RelayCommand RefreshCommand
        {
            get
            {
                return _refreshCommand
                  ?? (_refreshCommand = new RelayCommand(
                    () =>
                    {
                        if (_isRefreshing)
                            return;

                        _isRefreshing = true;
                        RefreshCommand.RaiseCanExecuteChanged();

                        _dataCollection = new ObservableCollection<Data>(_dataService.GetData());
                        RaisePropertyChanged("DataCollection");

                        _isRefreshing = false;
                        RefreshCommand.RaiseCanExecuteChanged();
                    },
                    () => !_isRefreshing));
            }
        }

        IDataService _dataService;
        public DataViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataCollection = new ObservableCollection<Data>(_dataService.GetData());
        }
    }
}
