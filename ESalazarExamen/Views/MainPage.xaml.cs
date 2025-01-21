using ESalazarExamen.Views;
using ESalazarExamen.ViewModels;

namespace ESalazarExamen.Views
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new  PaisViewModel();
        }

        
    }

}
