using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ESalazarExamen.Models;

namespace ESalazarExamen.Repositories
{
    public class PaisRepository
    {
        string _dbPath;

        public string StatusMessage { get; set; }

        private SQLiteConnection conn;

        private void Init()
        {
            if (conn == null)
                return;
            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Pais>();
        }

        public PaisRepository(string dbPath)
        {
            _dbPath = dbPath;
        }

        public async Task SavePais(String nombre, string region, string linkmaps)
        {
            int result = 0;
            try
            {
                Init();

                if (string.IsNullOrEmpty(nombre))
                    throw new Exception("Se requiere un nombre valido");

                if (string.IsNullOrEmpty(region))
                    throw new Exception("Se requiere una region valida");

                
                result = conn.Insert(new Pais { Nombre = nombre, region = region, linkMaps = linkmaps });
              
                StatusMessage = string.Format("Pais {0} guardado correctamente",nombre);
            }catch (Exception ex)
            {
                StatusMessage =string.Format("Error al guardar el pais. Detalles: {0}",ex.Message) ;
            }
            
            
        }
            

        public async Task<List<Pais>> GetAllPaises()
        {
            try
            {
                Init();
                return conn.Table<Pais>().ToList();
            } catch (Exception ex)
            {
                StatusMessage = $"Error al listar la informacion. DEtalle: {ex.Message}";
            }
            return new List<Pais>();
        }
    }

}
