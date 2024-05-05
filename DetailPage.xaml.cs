using ContaSpese.ViewModel;
using ContaSpese.Model;
namespace ContaSpese;

public partial class DetailPage : ContentPage
{
	public DetailPage(DetailViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
	

}