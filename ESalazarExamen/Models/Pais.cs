using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESalazarExamen.Models
{
    [SQLite.Table("Pais")]
    public class Pais
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [SQLite.Column("Nombre")]
        public string Nombre { get; set; }
        [SQLite.Column("Region")]
        public string region { get; set; }
        [SQLite.Column("Link")]
        public string linkMaps { get; set; }

    }
}
