using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using DeliveriandoWebApp.Models;

namespace DeliveriandoWebApp.Models
{
    public class Product
    {

        [Key]
        public long Id { get; set; }
        public int IdRestaurante { get; set; }
        public string Nombre { get; set; }
        public string Categoria { get; set; }
        public double Precio { get; set; }
        public string Descripcion { get; set; }

        
    }
}