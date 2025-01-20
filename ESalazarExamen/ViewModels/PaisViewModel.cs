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
        public ObservableCollection<Pais> _paisesList { get; set; }
        
        
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
        public int Id => _pais.Id;
        
        public PaisViewModel()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EduardoSalazar.db3");
            _paisRepository = new PaisRepository(dbPath);
            _pais = new Pais();
            PaisList = new ObservableCollection<Pais>();

            GetAllPaisesCommand = new AsyncRelayCommand(GetAllPaises);


        }

        
        private async Task GetAllPaises()
        {
            try
            {
                var paises = await _paisRepository.GetAllPaises();
                PaisList.Clear();
                foreach (var pais in paises)
                {
                    PaisList.Add(pais);
                }
                StatusMessage = $"Los paises se cargaron correctamente";
            }
            catch(Exception ex)
            {
                StatusMessage = $"Error al cargar los paises. Detalles: {ex.Message}";
            }
        }

    }
}
