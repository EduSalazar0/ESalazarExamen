using ESalazarExamen.ViewModels;

namespace ESalazarExamen;

public partial class ApiPage : ContentPage
{
	public ApiPage()
	{
		InitializeComponent();
		BindingContext = new ApiViewModel();
	}
}