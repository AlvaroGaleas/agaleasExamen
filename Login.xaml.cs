using Microsoft.Win32;

namespace agaleasExamen;

public partial class Login : ContentPage
{
    private readonly string[,] _usuarios = new string[4, 2]
    {
        { "recepcion", "gym2026" },
        { "entrenador", "fit_01" },
        { "uisrael", "2026" },
        { "gerente", "admin123" }
    };

    public Login()
    {
        InitializeComponent();
    }

    private async void OnIniciarSesionClicked(object sender, EventArgs e)
    {
        string user = txtUsuario.Text?.Trim() ?? "";
        string pass = txtPassword.Text?.Trim() ?? "";

        bool accesoValido = false;

        for (int i = 0; i < 4; i++)
        {
            if (_usuarios[i, 0] == user && _usuarios[i, 1] == pass)
            {
                accesoValido = true;
                break;
            }
        }

        if (accesoValido)
        {
            await Navigation.PushAsync(new Registro(user));
        }
        else
        {
            await DisplayAlertAsync("Error", "Dato incorrecto: Usuario o contraseña inválidos.", "OK");
        }
    }

    private async void OnAcercaDeClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new AcercaDe());
    }
}