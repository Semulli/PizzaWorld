using System;
using CommunityToolkit.Maui.Alerts;

namespace PizzaWorld.MVVM.ViewModels;

[QueryProperty(nameof(Pizza), nameof(Pizza))]
public partial class DetailsViewModel : ObservableObject, IDisposable
{
    private readonly CartViewModel _cartViewModel;
    public DetailsViewModel(CartViewModel cartViewModel)
    {
        _cartViewModel = cartViewModel;
        _cartViewModel.CartCleared += OnCartCleared;
        _cartViewModel.CartItemRemoved += OnCartItemRemoved;
        _cartViewModel.CartItemUpdated += OnCartItemUpdated;
    }
    private void OnCartCleared(object? _, EventArgs e) => pizza.CartQuantity = 0;
    private void OnCartItemRemoved(object? _, Pizza p) => onCartItemChanged(p, 0);
    private void OnCartItemUpdated(object? _, Pizza p) => onCartItemChanged(p, p.CartQuantity);
    private void onCartItemChanged(Pizza p, int quantity)
    {
        if (p.Name == Pizza.Name)
        {
            Pizza.CartQuantity = quantity;
        }
    }
    [ObservableProperty]
    private Pizza pizza;

    [RelayCommand]
    private void AddToCart()
    {
        Pizza.CartQuantity++;
        _cartViewModel.UpdateCartItemCommand.Execute(Pizza);
    }
    [RelayCommand]

    private void RemoveFromCart()
    {
        if (pizza.CartQuantity > 0)
        {
            Pizza.CartQuantity--;
            _cartViewModel.UpdateCartItemCommand.Execute(Pizza);

        }
    }

    [RelayCommand]
    private async Task ViewCart()
    {
        if (Pizza.CartQuantity > 0)
        {
            await Shell.Current.GoToAsync(nameof(CartPage));
        }
        else
        {
            Toast.Make("Please select the quantity using the plus button", ToastDuration.Short).Show();
        }
    }

    public void Dispose()
    {
        _cartViewModel.CartCleared -= OnCartCleared;
        _cartViewModel.CartItemRemoved -= OnCartItemRemoved;
        _cartViewModel.CartItemUpdated -= OnCartItemUpdated;
    }
}
