using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeliveriandoWebApp.Models
{
    public class CoordenadasModels
    {
        [Key]
        public int Id { get; set; }
        public string Latitud { get; set; }
        public string Longitud { get; set; }
        public DateTime Fecha { get; set; }
        public int IdMotorista { get; set; }
        public int IdRestaurante { get; set; }
    }
}