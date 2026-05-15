using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PizzaWorld.MVVM.ViewModels;
using PizzaWorld.Pages;
using PizzaWorld.Services;

namespace PizzaWorld;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit();

		builder.ConfigureFonts(fonts =>
		{
			fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
		});

		AddPizzaServices(builder.Services);

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
	private static IServiceCollection AddPizzaServices(IServiceCollection services)
	{
		services.AddSingleton<PizzaService>();
		services.AddSingleton<HomeViewModel>();
		services.AddSingleton<HomePage>();
		services.AddTransientWithShellRoute<AllPizzasPage, AllPizzaViewModel>(nameof(AllPizzasPage));
		services.AddTransientWithShellRoute<DetailsPage, DetailsViewModel>(nameof(DetailsPage));

		services.AddSingleton<CartViewModel>();
		services.AddTransient<CartPage>();
		services.AddTransient<CheckoutPage>();

		return services;
	}
}
