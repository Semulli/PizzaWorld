namespace PizzaWorld.MVVM.ViewModels;

[QueryProperty(nameof(FromSearch), nameof(FromSearch))]
public partial class AllPizzaViewModel : ObservableObject
{
    private readonly PizzaService _pizzaService;

    public AllPizzaViewModel(PizzaService pizzaService)
    {
        _pizzaService = pizzaService;
        Pizzas = new(_pizzaService.GetAllPizzas());
    }

    public ObservableCollection<Pizza> Pizzas { get; set; }

    [ObservableProperty]
    private bool _fromSearch;

    [ObservableProperty]
    private bool _searching;

    [RelayCommand]
    private async Task SearchPizzas(string searchTerm)
    {
        Pizzas.Clear();
        Searching = true;
        await Task.Delay(2000);
        foreach (var pizza in _pizzaService.SearchPizzas(searchTerm))
        {
            Pizzas.Add(pizza);
        }

        Searching = false;
    }
    [RelayCommand]
    private async Task GoToDetailsPage(Pizza pizza)
    {
        var parameters = new Dictionary<string, object>
        {
            [nameof(DetailsViewModel.Pizza)] = pizza
        };
        await Shell.Current.GoToAsync(nameof(DetailsPage), animate: true, parameters);
    }
}