using ESalazarExamen.Views;
using ESalazarExamen.ViewModels;

namespace ESalazarExamen
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
