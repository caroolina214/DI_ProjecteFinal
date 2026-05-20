using MilAventures.ViewModel;
using MilAventures.ViewModel.Categories;
using System.Windows;

namespace MilAventures.View.Views.Categories
{
    public partial class CategoryDialogView : Window
    {
        public CategoryDialogView(CategoryDialogViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;

            NavigationService.Instance.OnCloseDialog += Close;
            Closed += (s, e) => NavigationService.Instance.OnCloseDialog -= Close;
        }
    }
}