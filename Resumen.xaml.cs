namespace agaleasExamen;

public partial class Resumen : ContentPage
{
    private readonly string _contenidoTxt;
    private readonly string _nombreSocio;
    private readonly string _apellidoSocio;

    public Resumen(
        string usuario,
        string nombre,
        string apellido,
        string edad,
        string fecha,
        string plan,
        string casillero,
        double montoInicial,
        double pagoMensual,
        double pagoTotal)
    {
        InitializeComponent();

        _nombreSocio = nombre;
        _apellidoSocio = apellido;

        lblUsuarioConectado.Text = $"Usuario conectado: {usuario}";
        lblNombre.Text = nombre;
        lblApellido.Text = apellido;
        lblEdad.Text = edad;
        lblFecha.Text = fecha;
        lblPlan.Text = plan;
        lblCasillero.Text = casillero;
        lblMontoInicial.Text = $"${montoInicial:F2}";
        lblPagoMensual.Text = $"${pagoMensual:F2}";
        lblPagoTotal.Text = $"${pagoTotal:F2}";

        _contenidoTxt = "=== REPORTE DE SOCIO - GIMNASIO ===\n" +
                        $"Usuario que registró: {usuario}\n" +
                        $"Nombre: {nombre}\n" +
                        $"Apellido: {apellido}\n" +
                        $"Edad: {edad}\n" +
                        $"Fecha de inicio: {fecha}\n" +
                        $"Plan: {plan}\n" +
                        $"Casillero: {casillero}\n" +
                        $"Monto Inicial: ${montoInicial:F2}\n" +
                        $"Pago mensual: ${pagoMensual:F2}\n" +
                        $"Pago Total: ${pagoTotal:F2}\n" +
                        "===================================\n";
    }

    private async void OnExportarTxtClicked(object sender, EventArgs e)
    {
        try
        {
            string nombreArchivo = $"Reporte_{_nombreSocio}_{_apellidoSocio}.txt";
            string rutaArchivo = Path.Combine(FileSystem.AppDataDirectory, nombreArchivo);

            File.WriteAllText(rutaArchivo, _contenidoTxt);

            await DisplayAlertAsync(
                "Archivo Exportado",
                $"Nombre del archivo: {nombreArchivo}\n\nRuta de almacenamiento:\n{rutaArchivo}",
                "Aceptar");
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }

    private async void OnNuevoRegistroClicked(object sender, EventArgs e)
    {
        var stack = Navigation.NavigationStack;
        if (stack.Count >= 2 && stack[stack.Count - 2] is Registro paginaRegistro)
        {
            paginaRegistro.LimpiarCampos();
        }
        await Navigation.PopAsync();
    }

    private void OnCerrarSesionClicked(object sender, EventArgs e)
    {
        Application.Current!.Windows[0].Page = new NavigationPage(new Login());
    }
}