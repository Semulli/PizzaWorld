#if IOS
using UIKit;
#endif

namespace PizzaWorld.Pages;

public partial class DetailsPage : ContentPage
{
	private readonly DetailsViewModel _detailsViewModel;
	public DetailsPage(DetailsViewModel detailsViewModel)
	{
		InitializeComponent();
		_detailsViewModel = detailsViewModel;
		BindingContext = _detailsViewModel;
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
#if IOS
		var bottom = UIApplication.SharedApplication.Delegate.GetWindow().SafeAreaInsets.Bottom;

		bottomBox.Margin = new Thickness(-1, 0, -1, (bottom + 1) * -1);
#endif
	}

	async void ImageButton_Clicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..", animate: true);
	}
}