using System;

namespace PizzaWorld.MVVM.ViewModels;

public partial class CartViewModel : ObservableObject
{
    public event EventHandler<Pizza> CartItemRemoved;
    public event EventHandler<Pizza> CartItemUpdated;
    public event EventHandler CartCleared;

    public ObservableCollection<Pizza> Items { get; set; } = new();
    [ObservableProperty]
    private double _totalAmount;
    private void RecalculateTotalAmount() => TotalAmount = Items.Sum(i => i.Amount);

    [RelayCommand]
    private void UpdateCartItem(Pizza pizza)
    {
        var item = Items.FirstOrDefault(i => i.Name == pizza.Name);

        if (item is not null)
        {
            item.CartQuantity = pizza.CartQuantity;

            if (item.CartQuantity <= 0)
            {
                Items.Remove(item);
                CartItemRemoved?.Invoke(this, item);
            }
            else
            {
                CartItemUpdated?.Invoke(this, item);
            }
        }
        else
        {
            var clonedPizza = pizza.Clone();
            Items.Add(clonedPizza);
            CartItemUpdated?.Invoke(this, clonedPizza);
        }

        RecalculateTotalAmount();
    }
    [RelayCommand]
    private async void RemoveCartItem(string name)
    {
        var item = Items.FirstOrDefault(i => i.Name == name);

        if (item is not null)
        {
            Items.Remove(item);
            RecalculateTotalAmount();
            CartItemRemoved?.Invoke(this, item);

            var snackbarOptions = new SnackbarOptions
            {
                CornerRadius = 10,
                BackgroundColor = Colors.PaleGreen
            };

            var snackbar = Snackbar.Make(
                $"'{item.Name}' removed from cart",
                () =>
                {
                    Items.Add(item);
                    RecalculateTotalAmount();
                    CartItemUpdated?.Invoke(this, item);
                },
                "Undo",
                TimeSpan.FromSeconds(5),
                snackbarOptions);

            await snackbar.Show();
        }
    }
    [RelayCommand]
    private async void ClearCart()
    {


        if (await Shell.Current.DisplayAlert("Confirm Clear Cart?", "Do you really want to clear cart items?", "yes", "no"))
        {

            Items.Clear();
            RecalculateTotalAmount();
            CartCleared?.Invoke(this, EventArgs.Empty);

            await Toast.Make("Cart Cleared", ToastDuration.Short).Show();
        }

    }

    [RelayCommand]
    private async Task PlaceOrder()
    {
        Items.Clear();
        CartCleared?.Invoke(this, EventArgs.Empty);
        RecalculateTotalAmount();
        await Shell.Current.GoToAsync(nameof(CheckoutPage));
    }
}

