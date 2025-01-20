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
             Init();
        }

        public async Task SavePais(Pais pais)
        {
            try
            {
                if (string.IsNullOrEmpty(pais.Nombre))
                    throw new Exception("Se requiere un nombre valido");

                if (string.IsNullOrEmpty(pais.region))
                    throw new Exception("Se requiere una region valida");

                if (string.IsNullOrEmpty(pais.linkMaps))
                    throw new Exception("Se requiere un link valido");

                Init();
                var existingPais = conn.Table<Pais>().FirstOrDefault(p => p.Nombre == pais.Nombre);
                if (existingPais != null)
                {
                    StatusMessage = $"El pais {pais.Nombre} ya existe existe en la base de datos";
                    return;
                }

                conn.Insert(pais);
                StatusMessage = $"Pais {pais.Nombre} guardado correctamente";
            }catch (Exception ex)
            {
                StatusMessage = $"Error al guardar el pais. Detalles: {ex.Message}";
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
