using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using LittleLarry.Model;
using System;

namespace LittleLarry.ViewModels
{
    public class ModelViewModel : ViewModelBase
    {
        private string _model;
        public string Model
        {
            get { return _model; }
            set
            {
                if (_model != value)
                {
                    _model = value;
                    RaisePropertyChanged();
                }
            }
        }

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

                        Model = _device.Model.ToString();

                        _isRefreshing = false;
                        RefreshCommand.RaiseCanExecuteChanged();
                    },
                    () => !_isRefreshing));
            }
        }

        Device _device;
        public ModelViewModel(Device device)
        {
            _device = device;
            Model = _device.Model.ToString();
        }
    }
}
