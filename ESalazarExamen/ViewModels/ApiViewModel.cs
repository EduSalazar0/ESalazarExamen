using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ESalazarExamen.Models;
using ESalazarExamen.Models;
using ESalazarExamen.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;


namespace ESalazarExamen.ViewModels
{
    public class ApiViewModel : ObservableObject
    {
        private Api _paisSeleccionado;
        private string _nombreSeleccionado;
        private string _statusMessage;

        private readonly PaisRepository _paisRepository;

        public ObservableCollection<Api> Paises { get; set; }
        public ICommand BuscarPaisCommand { get; set; }
        public ICommand SavePaisCommand { get; set; }

        public ApiViewModel()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GabrielCalderon.db3");
            _paisRepository = new PaisRepository(dbPath);

            Paises = new ObservableCollection<Api>();

            BuscarPaisCommand = new AsyncRelayCommand(BuscarPais);
            SavePaisCommand = new AsyncRelayCommand(SavePais);

        }

        public string NombreSeleccionado
        {
            get => _nombreSeleccionado;
            set => SetProperty(ref _nombreSeleccionado, value);
        }

        public Api PaisSeleccionado
        {
            get => _paisSeleccionado;
            set => SetProperty(ref _paisSeleccionado, value);
        }
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        private async Task BuscarPais()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NombreSeleccionado))
                {
                    await Shell.Current.DisplayAlert("Error", "Ingrese un nombre de país para buscar.", "OK");
                    return;
                }

                var paisObtenido = await Api.ObtenerPaisPorNombre(NombreSeleccionado);

                if (paisObtenido != null)
                {
                    PaisSeleccionado = paisObtenido;

                    // Mostrar datos del país encontrado
                    await Shell.Current.DisplayAlert(
                        "País Encontrado",
                        $"Nombre: {PaisSeleccionado.Nombre}\n" +
                        $"Región: {PaisSeleccionado.Region}\n" +
                        $"Mapa: {PaisSeleccionado.LinkMaps}",
                        "OK"
                    );

                    // Actualizar la lista para mostrar el país en el ListView
                    Paises.Clear();
                    Paises.Add(paisObtenido);

                    StatusMessage = "País encontrado correctamente.";
                }
                else
                {
                    await Shell.Current.DisplayAlert("Aviso", "No se encontró un país con ese nombre.", "OK");
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al buscar el país: {ex.Message}";
                await Shell.Current.DisplayAlert("Error", StatusMessage, "OK");
            }
        }
        public async Task SavePais()
        {
            try
            {
                if (PaisSeleccionado == null)
                {
                    throw new Exception("No hay un pais seleccionado para guardar.");
                    return;
                }

                _paisRepository.SavePais(PaisSeleccionado.Nombre, PaisSeleccionado.Region, PaisSeleccionado.LinkMaps);
                StatusMessage = $"Pais {PaisSeleccionado.Nombre} guardado exitosamente.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardarel pais: {ex.Message}";
            }
        }

    }
}
