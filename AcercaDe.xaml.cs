namespace agaleasExamen;

public partial class AcercaDe : ContentPage
{
    public AcercaDe()
    {
        InitializeComponent();
    }

    private async void OnVolverClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}