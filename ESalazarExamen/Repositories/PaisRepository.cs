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

        public List<Pais> GetAllPaises()
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
