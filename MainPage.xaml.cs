using ContaSpese.Model;
using ContaSpese.ViewModel;
using System.Text.Json;

namespace ContaSpese;
public partial class MainPage : ContentPage
{
    private readonly MainViewModel viewModel;
    public static readonly string savingPath = Path.Combine(FileSystem.Current.AppDataDirectory,"data.json");
    public MainPage(MainViewModel vm)
    {
        InitializeComponent();

        viewModel = vm;
        BindingContext = vm;

        FillItems();
    }

    private void FillItems()
    {

        if (!File.Exists(savingPath))
            File.Create(savingPath).Close();

        string jsonString = File.ReadAllText(savingPath);

        if (new FileInfo(savingPath).Length != 0)
        {
            var list = JsonSerializer.Deserialize<List<Purchase>>(jsonString);

            foreach (Purchase p in list)
            {
                viewModel.Items.Add(p);
                viewModel.SumOfAll += p.purchaseCost;
            }
        }

        return;
    }
}
