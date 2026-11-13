using System.Net.Http;

namespace pcrespot6.Views;

public partial class vAgregarEstudiante : ContentPage
{
    private const string Url = "http://192.168.0.174/ws_estudiante/estudiante_ws.php";

    private static readonly HttpClient _cliente = new HttpClient();

    public vAgregarEstudiante()
    {
        InitializeComponent();
    }

    private async void btnCancelar_Clicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }

    private async void btnAgregar_Clicked(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
            string.IsNullOrWhiteSpace(txtApellido.Text) ||
            string.IsNullOrWhiteSpace(txtEdad.Text))
        {
            await DisplayAlertAsync("Alerta", "Complete todos los campos", "OK");
            return;
        }

        if (!int.TryParse(txtEdad.Text, out int edad))
        {
            await DisplayAlertAsync("Alerta", "La edad debe ser un número", "OK");
            return;
        }

        try
        {
            var parametros = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["nombre"] = txtNombre.Text.Trim(),
                ["apellido"] = txtApellido.Text.Trim(),
                ["edad"] = edad.ToString()
            });

            var response = await _cliente.PostAsync(Url, parametros);
            response.EnsureSuccessStatusCode();

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Alerta", "Error: " + ex.Message, "OK");
        }
    }
}
