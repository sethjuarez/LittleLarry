using LittleLarry.Services;
using LittleLarry.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace LittleLarry.Views
{
    public sealed partial class ShellPage : Page
    {
        private ShellViewModel ViewModel { get { return DataContext as ShellViewModel; } }

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame);
        }
    }
}
