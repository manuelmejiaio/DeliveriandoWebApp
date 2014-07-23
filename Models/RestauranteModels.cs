using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeliveriandoWebApp.Models
{
    public class RestauranteModels
    {

        [Key]
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Rating { get; set; }
        public string Ubicacion { get; set; }
        public string LogoLink { get; set; }
    }
}