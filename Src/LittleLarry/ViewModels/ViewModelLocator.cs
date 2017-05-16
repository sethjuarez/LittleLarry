using GalaSoft.MvvmLight.Ioc;

using LittleLarry.Services;
using LittleLarry.Views;

using Microsoft.Practices.ServiceLocation;

namespace LittleLarry.ViewModels
{
    public class ViewModelLocator
    {
        NavigationServiceEx _navigationService = new NavigationServiceEx();

        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => _navigationService);
            SimpleIoc.Default.Register<ShellViewModel>();
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
