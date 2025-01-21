using CommunityToolkit.Mvvm.ComponentModel;
using ESalazarExamen.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ESalazarExamen.Models;
using CommunityToolkit.Mvvm.Input;
using System.Text.Json;

namespace ESalazarExamen.ViewModels
{
    public class PaisViewModel : ObservableObject
    {
        private Models.Pais _pais;
        private readonly PaisRepository _paisRepository;
        private string _statusMessage;
        public ObservableCollection<Pais> Paises { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand GetAllPaisesCommand { get; set; }
        public ICommand BuscarPaisCommand { get; set; }
        public ICommand LimpiarBusquedaCommand { get; set; }

        public Models.Pais Pais
        {
            get => _pais;
            set
            {
                if(SetProperty(ref _pais, value))
                {
                    OnPropertyChanged(nameof(_pais.Nombre));
                    OnPropertyChanged(nameof(_pais.region));
                    OnPropertyChanged(nameof(_pais.linkMaps));
                }
            }
        }
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        public string Nombre
        {
            get => _pais.Nombre;
            set
            {
                if(_pais.Nombre != value)
                {
                    _pais.Nombre = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Region
        {
            get => _pais.region;
            set
            {
                if(_pais.region != value)
                {
                    _pais.region = value;
                    OnPropertyChanged();
                }
            }

        }
        public string LinkMaps
        {
            get => _pais.linkMaps;
            set
            {
                if(_pais.linkMaps != value)
                {
                    _pais.linkMaps = value;
                    OnPropertyChanged();
                }
            }
        }
      
        public int Id => _pais.Id;
        
        public PaisViewModel()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EduardoSalazar1.db3");
            _paisRepository = new PaisRepository(dbPath);
            _pais = new Pais();
            Paises = new ObservableCollection<Pais>();

            SaveCommand = new AsyncRelayCommand(Guardar);
            BuscarPaisCommand = new AsyncRelayCommand(BuscarPais);
            GetAllPaisesCommand = new AsyncRelayCommand(GetAllPaises);
            LimpiarBusquedaCommand = new RelayCommand(LimpiarBusqueda);

        }

        private async Task Guardar()
        {
            try
            {
                if (string.IsNullOrEmpty(_pais.Nombre))
                {
                    throw new Exception("El nombre no puede estar vacío.");
                }
                if (string.IsNullOrEmpty(_pais.region))
                {
                    throw new Exception("El nombre no puede estar vacío.");
                }
                if (string.IsNullOrEmpty(_pais.linkMaps))
                {
                    throw new Exception("El nombre no puede estar vacío.");
                }

                _paisRepository.SavePais(_pais.Nombre, _pais.region, _pais.linkMaps);

                StatusMessage = $"Pais {_pais.Nombre} guardado exitosamente.";
                await Shell.Current.GoToAsync($"..?saved={_pais.Nombre}");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar la persona: {ex.Message}";
            }
        }

        private async Task GetAllPaises()
        {
            try
            {
                var paises = await _paisRepository.GetAllPaises();
                Paises.Clear();
                foreach (var pais in paises)
                {
                    Paises.Add(pais);
                }
                StatusMessage = $"Los paises se cargaron correctamente";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al cargar los paises. Detalles: {ex.Message}";
            }
        }
        private async Task BuscarPais()
        {
            try
            {
                // Verifica que el usuario haya ingresado un nombre para buscar
                if (string.IsNullOrWhiteSpace(Nombre))
                {
                    StatusMessage = "Por favor, ingrese un nombre de país para buscar.";
                    await Shell.Current.DisplayAlert("Error", StatusMessage, "OK");
                    return;
                }

                // Conexión a la API para obtener los datos del país
                var url = $"https://restcountries.com/v3.1/name/{Nombre}?fields=name,region,maps";
                using var client = new HttpClient();
                var response = await client.GetAsync(url);

                // Verifica si la respuesta fue exitosa
                if (response.IsSuccessStatusCode)
                {
                    // Deserializa la respuesta en un modelo
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var paises = JsonSerializer.Deserialize<List<Api>>(jsonResponse);

                    // Si se encuentra al menos un país, actualiza la lista y el país seleccionado
                    if (paises != null && paises.Count > 0)
                    {
                        var paisEncontrado = paises[0];
                        Pais.Nombre = paisEncontrado.name.common;
                        Pais.region = paisEncontrado.region;
                        Pais.linkMaps = paisEncontrado.maps.googleMaps;

                        // Agrega el país a la lista
                        Paises.Clear();
                        Paises.Add(Pais);

                        StatusMessage = $"País encontrado: {paisEncontrado.name.common}, Región: {paisEncontrado.region}.";
                        await Shell.Current.DisplayAlert("Éxito", StatusMessage, "OK");
                    }
                    else
                    {
                        StatusMessage = "No se encontró ningún país con ese nombre.";
                        await Shell.Current.DisplayAlert("Aviso", StatusMessage, "OK");
                    }
                }
                else
                {
                    StatusMessage = "Error al conectarse a la API.";
                    await Shell.Current.DisplayAlert("Error", StatusMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al buscar el país: {ex.Message}";
                await Shell.Current.DisplayAlert("Error", StatusMessage, "OK");
            }
        }
        private void LimpiarBusqueda()
        {
            Nombre = string.Empty;
        }

    }
}
