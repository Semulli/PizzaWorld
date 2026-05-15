namespace PizzaWorld.Pages;

public partial class CartPage : ContentPage
{
	private readonly CartViewModel _cartViewModel;
	public CartPage(CartViewModel cartViewModel)
	{
		InitializeComponent();
		_cartViewModel = cartViewModel;
		BindingContext = _cartViewModel;
	}

	async void Button_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync(nameof(AllPizzasPage));
	}
}