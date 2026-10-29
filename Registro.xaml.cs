using System.Globalization;

namespace agaleasExamen;

public partial class Registro : ContentPage
{
    private readonly string _usuarioConectado;
    private double _montoInicialCalculado = 0;
    private double _pagoMensualCalculado = 0;
    private bool _calculoRealizado = false;

    public Registro(string usuario)
    {
        InitializeComponent();
        _usuarioConectado = usuario;
        lblUsuarioConectado.Text = $"Usuario conectado: {_usuarioConectado}";
        dpFechaInicio.MinimumDate = DateTime.Today;
        dpFechaInicio.Date = DateTime.Today;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        dpFechaInicio.MinimumDate = DateTime.Today;
    }

    private void ValidarSoloLetras(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            if (!e.NewTextValue.All(c => char.IsLetter(c) || char.IsWhiteSpace(c)))
            {
                entry!.Text = new string(e.NewTextValue.Where(c => char.IsLetter(c) || char.IsWhiteSpace(c)).ToArray());
            }
        }
    }

    private void ValidarSoloEnteros(object sender, TextChangedEventArgs e)
    {
        var entry = sender as Entry;
        if (!string.IsNullOrEmpty(e.NewTextValue))
        {
            if (!e.NewTextValue.All(char.IsDigit))
            {
                entry!.Text = new string(e.NewTextValue.Where(char.IsDigit).ToArray());
            }
        }
    }

    private async Task<bool> ValidarFormulario()
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtApellido.Text) ||
            string.IsNullOrWhiteSpace(txtEdad.Text) ||
            pkPlan.SelectedIndex == -1 ||
            string.IsNullOrWhiteSpace(txtMontoInicial.Text))
        {
            await DisplayAlertAsync("Validación", "Complete todos los campos del registro.", "OK");
            return false;
        }

        if (!int.TryParse(txtEdad.Text.Trim(), out int edad) || edad < 16)
        {
            await DisplayAlertAsync("Validación", "La edad debe ser numérica y mayor o igual a 16 años.", "OK");
            return false;
        }

        DateTime fechaSeleccionada = dpFechaInicio.Date ?? DateTime.Today;
        if (fechaSeleccionada.Date < DateTime.Today)
        {
            await DisplayAlertAsync("Validación", "No se permiten fechas pasadas.", "OK");
            return false;
        }

        string montoTexto = txtMontoInicial.Text.Trim().Replace(',', '.');
        if (!double.TryParse(montoTexto, NumberStyles.Any, CultureInfo.InvariantCulture, out double montoInicial) ||
            montoInicial < 120 || montoInicial > 600)
        {
            await DisplayAlertAsync("Validación", "El Monto Inicial debe estar entre 120 (10%) y 600 (50%).", "OK");
            return false;
        }

        double costoMembresia = 1200.0;
        double saldoRestante = costoMembresia - montoInicial;
        double cuotaBase = (saldoRestante / 5.0) + (costoMembresia * 0.02);

        if (swCasillero.IsToggled)
        {
            cuotaBase += 10.0;
        }

        _montoInicialCalculado = montoInicial;
        _pagoMensualCalculado = cuotaBase;
        txtPagoMensual.Text = _pagoMensualCalculado.ToString("F2");
        _calculoRealizado = true;

        return true;
    }

    private async void OnCalcularPagoClicked(object sender, EventArgs e)
    {
        await ValidarFormulario();
    }

    private async void OnVerResumenClicked(object sender, EventArgs e)
    {
        if (!await ValidarFormulario())
        {
            return;
        }

        if (!_calculoRealizado)
        {
            return;
        }

        string nombre = txtNombre.Text.Trim();
        string apellido = txtApellido.Text.Trim();
        string edad = txtEdad.Text.Trim();
        string fecha = (dpFechaInicio.Date ?? DateTime.Today).ToString("dd/MM/yyyy");
        string plan = pkPlan.SelectedItem?.ToString() ?? "";
        string casillero = swCasillero.IsToggled ? "Si" : "No";
        double pagoTotal = _montoInicialCalculado + (_pagoMensualCalculado * 5.0);

        await Navigation.PushAsync(new Resumen(
            _usuarioConectado,
            nombre,
            apellido,
            edad,
            fecha,
            plan,
            casillero,
            _montoInicialCalculado,
            _pagoMensualCalculado,
            pagoTotal));
    }

    public void LimpiarCampos()
    {
        txtNombre.Text = string.Empty;
        txtApellido.Text = string.Empty;
        txtEdad.Text = string.Empty;
        dpFechaInicio.Date = DateTime.Today;
        pkPlan.SelectedIndex = -1;
        swCasillero.IsToggled = false;
        txtMontoInicial.Text = string.Empty;
        txtPagoMensual.Text = string.Empty;
        _calculoRealizado = false;
    }
}