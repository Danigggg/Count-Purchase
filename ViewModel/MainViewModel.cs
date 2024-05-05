using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ContaSpese.Model;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace ContaSpese.ViewModel;

[QueryProperty("ModifiedNote","ModifiedNote")]
[QueryProperty("ModifiedCost","ModifiedCost")]
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<Purchase> items;
    
    [ObservableProperty]
    string notePurchase;

    [ObservableProperty]
    string purchaseCost;

    [ObservableProperty]
    double sumOfAll = 0;



    [ObservableProperty]
    string modifiedNote;

    [ObservableProperty]
    string modifiedCost;

    public MainViewModel()
    {
        Items = [];
    }

    [RelayCommand]
    void Add()
    {
        if (string.IsNullOrEmpty(PurchaseCost))
            return;

        try
        {
            if (PurchaseCost.Contains(",")) PurchaseCost = PurchaseCost.Replace(",", ".");

            double purchaseCostDouble = double.Parse(PurchaseCost); //this throws if string is not parsable
            
            if (purchaseCostDouble == 0) throw new Exception();

            Purchase copy = new Purchase(NotePurchase, purchaseCostDouble, DateTime.Now.ToString("dd/MM/yyyy"));

            if (CanEnterObservableCollection(ref copy) == false) 
                throw new Exception(); 



            //every exception has been checked, we can add the purchase and save it
            Items.Add(copy);

            SumOfAll += purchaseCostDouble;

            Save();

            return;
        }
        catch (Exception)
        {
            Shell.Current.DisplayAlert("Errore", "Non hai inserito un importo valido o hai già inserito questa spesa con stesso importo e stessa nota", "OK");
            return;
        }
    }

    public void Save()
    {

        string json = JsonSerializer.Serialize(Items, new JsonSerializerOptions
        {
            WriteIndented = true,
        });

        // Save JSON to file
        File.WriteAllText(MainPage.savingPath, json);
    }


    [RelayCommand]
    void Delete(Purchase pur)
    {
        if (Items.Contains(pur))
        {
            SumOfAll -= pur.purchaseCost;
            Items.Remove(pur);
        }

        this.Save();
    }


    [RelayCommand]
    void PinUp(Purchase pur)
    {
        this.DeleteWithoutBounds(pur);
        Items.Insert(0, pur);
        Items[0].isFavorite = true;
        this.Save();
    }


    [RelayCommand]
    void UnPinUp(Purchase pur)
    {
        pur.isFavorite = false;
        this.DeleteWithoutBounds(pur);
        Items.Add(pur);
        this.Save();
    }


    private bool CanEnterObservableCollection(ref Purchase pur)
    {
        //if there's a purchase with the same note and same import triggers error
        foreach(Purchase purchase in Items)
            if (pur.purchaseCost == purchase.purchaseCost && pur.note == purchase.note)
                return false;

        return true;
    }

    private void DeleteWithoutBounds(Purchase purchase)
    {
        if(Items.Contains(purchase))
            Items.Remove(purchase);
    }


    [RelayCommand]
    async Task Tap(Purchase pur)
    {
        await Shell.Current.GoToAsync($"{nameof(DetailPage)}", true,
            new Dictionary<string, object>
            {
                {"Purchase", pur},
                {"MainView", this},
                {"ModifiableNote", pur.note},
                {"ModifiableCost", pur.purchaseCost.ToString() }
            });

    }

    private void LinearSum()
    {
        SumOfAll = 0;
        foreach (Purchase pur in Items)
            SumOfAll += pur.purchaseCost;
    }
}
