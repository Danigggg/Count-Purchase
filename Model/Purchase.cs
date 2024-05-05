namespace ContaSpese.Model;

public class Purchase
{
    public Purchase(string note, double purchaseCost, string dateOfCreation)
    {
        this.note = note;
        this.purchaseCost = purchaseCost;
        this.dateOfCreation = dateOfCreation;
    }
    public Purchase(string note, double purchaseCost)
    {
        this.note = note;
        this.purchaseCost=purchaseCost;
    }

    public Purchase(string note, double purchaseCost, bool isFavorite, string dateOfCreation)
    {
        this.note =note;
        this.purchaseCost=purchaseCost;
        this.isFavorite=isFavorite;
        this.dateOfCreation =dateOfCreation;
    }

    public Purchase() { }
    public string note { get; set; }
    public double purchaseCost { get; set; } 
    public bool isFavorite { get; set; } = false;
    public string dateOfCreation { get; set; }
}
