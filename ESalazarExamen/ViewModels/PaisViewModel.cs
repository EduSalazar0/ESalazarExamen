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

namespace ESalazarExamen.Views
{
    public class PaisViewModel : ObservableObject
    {
        private Models.Pais _pais;
        private readonly PaisRepository _paisRepository;
        private string _statusMessage;
        private readonly HttpClient _httpClient = new HttpClient();
        public ObservableCollection<Pais> _paisesList { get; set; }
        
        public ICommand SavePaisCommand { get; set; }
        public ICommand GetAllPaisesCommand { get; set;  }
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
            set => SetProperty(ref _statusMessage, value);
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
        public string NombreBusqueda { get; set; }
        public ObservableCollection<Models.Pais> PaisList
        {
            get => _paisesList;
            set
            {
                if (_paisesList != value)
                {
                    _paisesList = value;
                    OnPropertyChanged();
                }
            }
        }
        public PaisViewModel()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EduardoSalazar.db3");
            _paisRepository = new PaisRepository(dbPath);
            _httpClient = new HttpClient();

            PaisList = new ObservableCollection<Pais>();

            SavePaisCommand = new AsyncRelayCommand(SavePais);
            GetAllPaisesCommand = new AsyncRelayCommand(GetAllPaises);
            BuscarPaisCommand = new AsyncRelayCommand(BuscarPais);
            //LimpiarBusquedaCommand = new RelayCommand(LimpiarBusqueda);

        }

        public async Task SavePais()
        {
            try
            {
                if (Pais == null)
                {
                    StatusMessage = "No hay información de pais para guardar";
                    return;
                }
            } catch (Exception ex)
            {
                StatusMessage = $"Error al guardar la informacion. Detalle: {ex.Message}";
            }

            await _paisRepository.SavePais(Pais);
            StatusMessage = $"Pais {Pais.Nombre} guardado con exito";
        }

        private async Task GetAllPaises()
        {
            var paises = await _paisRepository.GetAllPaises();
            PaisList.Clear();
            foreach(var pais in paises)
            {
                PaisList.Add(pais);
            }
        }

        private async Task BuscarPais()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NombreBusqueda))
                {
                    StatusMessage = "Por favor, ingrese un nombre de país para buscar.";
                    return;
                }

                string url = $"https://restcountries.com/v3.1/name/{NombreBusqueda}?fields=name,region,maps";
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var paises = JsonSerializer.Deserialize<List<Pais>>(jsonResponse);

                    if (paises != null && paises.Count > 0)
                    {
                        Pais = paises[0];
                        StatusMessage = $"País encontrado: {Pais.Nombre}. Región: {Pais.region}.";
                    }
                    else
                    {
                        StatusMessage = "No se encontró ningún país con ese nombre.";
                    }
                }
                else
                {
                    StatusMessage = "Error al conectarse al servicio.";
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al buscar el país: {ex.Message}";
            }


        }

        private void LimpiarBusqueda()
        {
            NombreBusqueda = string.Empty;
            StatusMessage = string.Empty;
        }





    }
}
