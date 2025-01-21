using ESalazarExamen.ViewModels;

namespace ESalazarExamen.Views
{
    public partial class ApiPage : ContentPage
    {

        public ApiPage()
        {
            InitializeComponent();
            BindingContext = new ApiViewModel();
        }
    }

}
