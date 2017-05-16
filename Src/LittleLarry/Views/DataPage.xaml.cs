using LittleLarry.ViewModels;

using Windows.UI.Xaml.Controls;

namespace LittleLarry.Views
{
    public sealed partial class DataPage : Page
    {
        private DataViewModel ViewModel
        {
            get { return DataContext as DataViewModel; }
        }

        public DataPage()
        {
            InitializeComponent();
        }
    }
}
