using System;

using GalaSoft.MvvmLight;
using LittleLarry.Model.Services;

namespace LittleLarry.ViewModels
{
    public class DataViewModel : ViewModelBase
    {
        IDataService _dataService;
        public DataViewModel(IDataService dataService)
        {
            _dataService = dataService;
        }
    }
}
