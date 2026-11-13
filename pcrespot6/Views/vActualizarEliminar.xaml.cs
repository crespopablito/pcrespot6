using pcrespot6.Models;
using System.Net.Http;

namespace pcrespot6.Views;

public partial class vActualizarEliminar : ContentPage
{
    private const string Url = "http://192.168.0.174/ws_estudiante/estudiante_ws.php";

    private static readonly HttpClient _cliente = new HttpClient();

    private readonly Estudiante _estudiante;

    public vActualizarEliminar(Estudiante datos)
    {
        InitializeComponent();

        _estudiante = datos;

        txtCodigo.Text = datos.codigo.ToString();
        txtNombre.Text = datos.nombre.ToString();
        txtApellido.Text = datos.apellido.ToString();
        txtEdad.Text = datos.edad.ToString();
    }

    private async void btnActualizar_Clicked(object? sender, EventArgs e)
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
            string parametros =
                $"?codigo={_estudiante.codigo}" +
                $"&nombre={Uri.EscapeDataString(txtNombre.Text.Trim())}" +
                $"&apellido={Uri.EscapeDataString(txtApellido.Text.Trim())}" +
                $"&edad={edad}";

            var response = await _cliente.PutAsync(
                Url + parametros,
                new StringContent(string.Empty));

            response.EnsureSuccessStatusCode();

            await DisplayAlertAsync(
                "Actualizar",
                "Estudiante actualizado correctamente",
                "Aceptar");

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Alerta", "Error: " + ex.Message, "OK");
        }
    }

    private async void btnEliminar_Clicked(object? sender, EventArgs e)
    {
        bool confirmar = await DisplayAlertAsync(
            "Eliminar",
            "¿Está seguro de eliminar este estudiante?",
            "Sí",
            "No");

        if (!confirmar)
            return;

        try
        {
            var response = await _cliente.DeleteAsync(
                $"{Url}?codigo={_estudiante.codigo}");

            response.EnsureSuccessStatusCode();

            await DisplayAlertAsync(
                "Eliminar",
                "Estudiante eliminado correctamente",
                "Aceptar");

            await Navigation.PopAsync();
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Alerta", "Error: " + ex.Message, "OK");
        }
    }
}
