using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContaSpese.Model;
using System.Collections.ObjectModel;

namespace ContaSpese.ViewModel;

[QueryProperty("Purchase", "Purchase")]
[QueryProperty("MainView", "MainView")]
[QueryProperty("ModifiableNote", "ModifiableNote")]
[QueryProperty("ModifiableCost", "ModifiableCost")]
public partial class DetailViewModel : ObservableObject
{
    [ObservableProperty]
    Purchase? purchase;

    [ObservableProperty]
    public MainViewModel mainView;

    [ObservableProperty]
    string? modifiableNote;


    [ObservableProperty]
    string? modifiableCost;


    [RelayCommand]
    void Modify()
    {
        try
        {
            double modifiedPurchaseCost = double.Parse(ModifiableCost);

            Purchase modifiedCopy = new(ModifiableNote, modifiedPurchaseCost, Purchase.isFavorite, Purchase.dateOfCreation);

            if (CanEnterObservableCollection(ref modifiedCopy) == false) throw new Exception();


            //checks made, we can modify

            MainView.SumOfAll -= Purchase.purchaseCost;
            MainView.Items[MainView.Items.IndexOf(Purchase)] = modifiedCopy;
            MainView.SumOfAll += modifiedCopy.purchaseCost;

            MainView.Save();
            Shell.Current.GoToAsync("..");

        }
        catch (Exception)
        {
            Shell.Current.DisplayAlert("Errore", "Non hai inserito un costo valido o questa spesa è già inserita", "OK");
        }
    }

    [RelayCommand]
    void PinUpInDetail()
    {
        MainView.PinUpCommand.Execute(Purchase);
    }

    [RelayCommand]
    void DeleteInDetail()
    {
        MainView.DeleteCommand.Execute(Purchase);
        Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    void UnPinUpInDetail()
    {
        MainView.UnPinUpCommand.Execute(Purchase);
    }

    private bool CanEnterObservableCollection(ref Purchase pur)
    {
        //if there's a purchase with the same note and same import triggers error
        foreach (Purchase purchase in MainView.Items)
            if (pur.purchaseCost == purchase.purchaseCost && pur.note == purchase.note)
                return false;

        return true;
    }

}
