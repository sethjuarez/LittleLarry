using GalaSoft.MvvmLight.Ioc;
using LittleLarry.Model;
using LittleLarry.Model.Hardware;
using LittleLarry.Model.Services;
using LittleLarry.Models;
using LittleLarry.Services;
using LittleLarry.Views;

using Microsoft.Practices.ServiceLocation;
using Windows.System.Profile;

namespace LittleLarry.ViewModels
{
    public class ViewModelLocator
    {
        NavigationServiceEx _navigationService = new NavigationServiceEx();

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            if (!SimpleIoc.Default.IsRegistered<NavigationServiceEx>())
                SimpleIoc.Default.Register(() => _navigationService);
            SimpleIoc.Default.Register<ShellViewModel>();

            // device (should have fez hat)
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.IoT")
            {
                // add fez hat data
            }
            // use fakes for everything else
            else
            {
                SimpleIoc.Default.Register<IDataService, FakeDataService>();
                SimpleIoc.Default.Register<IFezHat, FakeFezHat>();
            }

            SimpleIoc.Default.Register<Device>();

            Register<MainViewModel, MainPage>();
            Register<SettingsViewModel, SettingsPage>();
            Register<DataViewModel, DataPage>();
            Register<ModelViewModel, ModelPage>();
        }

        public ModelViewModel ModelViewModel => ServiceLocator.Current.GetInstance<ModelViewModel>();

        public DataViewModel DataViewModel => ServiceLocator.Current.GetInstance<DataViewModel>();

        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public void Register<VM, V>() where VM : class
        {
            SimpleIoc.Default.Register<VM>();
            _navigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
