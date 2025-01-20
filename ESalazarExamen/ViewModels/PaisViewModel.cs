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

namespace ESalazarExamen.Views
{
    public class PaisViewModel : ObservableObject, IQueryAttributable
    {
        private Models.Pais _pais;
        private readonly PaisRepository _paisRepository;
        public ObservableCollection<Pais> _paisesList { get; set; }
        public ICommand SavePaisCommand { get; set; }
        public ICommand GetAllPeopleCommand { get; set;  }
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
        public PaisViewModel()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "EduardoSalazar.db3");
            _paisRepository = new PaisRepository(dbPath);

            PaisList = new ObservableCollection<Pais>();
            SavePaisCommand = new Command(async () => await Save());
            GetAllPeopleCommand = new Command(async () => await GetAllPaises());


        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("pais") && query["pais"] is Models.Pais pais)
            {
                Pais = pais;
            }
            else if (query.ContainsKey("deleted"))
            {
                string nombre = query["save"].ToString();
                Models.Pais matchedPais = pais.

                if (matchedPerson != null)
                    Users.Remove(matchedPerson);
            }
        }
    }
}
