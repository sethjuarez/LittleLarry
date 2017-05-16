using LittleLarry.ViewModels;

using Windows.UI.Xaml.Controls;

namespace LittleLarry.Views
{
    public sealed partial class ModelPage : Page
    {
        private ModelViewModel ViewModel
        {
            get { return DataContext as ModelViewModel; }
        }

        public ModelPage()
        {
            InitializeComponent();
        }
    }
}
