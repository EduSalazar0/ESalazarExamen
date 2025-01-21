using ESalazarExamen.ViewModels;

namespace ESalazarExamen.Views
{
    public partial class ListadoPage : ContentPage
    {

        public ListadoPage()
        {
            InitializeComponent();
            BindingContext = new PaisViewModel();
        }
    }

}
