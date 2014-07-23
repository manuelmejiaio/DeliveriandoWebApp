using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
// NOTA: Si modificas algo aqui hacer Update-Database ,commit en Github y deploy en AppHabor
//Para borrar un atributo deben borrarse los valores en tabla (si tiene valores) y quitarlo del controller donde se use,luego  Update-Database -Force
namespace DeliveriandoWebApp.Models
{
    public class OrdenModels
    {
        [Key]
        public int id { get; set; }
        public virtual OrdenProduct product { get; set; } 
        public int quantity { get; set; }
        public double extPrice { get; set; }
        public double total { get; set; } // total es el sub-total.
        public double ITBIS { get; set; }
        public double finalTotal { get; set; } //Es la suma del total mas ITBIS.
        public Guid IdOrdenesUnicas { get; set; }
        public bool OrdenNotificadaPorCorreo { get; set; } //Para no repetir las ordenes por correo, ver OrderConfirmationMail()
        public string OrdenBorradaPor { get; set; }
        public string MotivoDeCancelacion { get; set; }
        public bool AlertaDeTiempo { get; set; }
        public string OrdenEstatus { get; set; }
    }


    public class OrdenProduct
    {
        [Key]
        public int id { get; set; }
        public string nombre { get; set; }
        public double precio { get; set; }
        public int IdRestaurante { get; set; }
        public int IdMotorista { get; set; }
        public string NombreRestaurante { get; set; }
        public bool isSelected { get; set; }
        public string shortDesc { get; set; }
        public bool stateHasChanged { get; set; }
        public int IdUsuario { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime Fecha { get; set; }
        public string DireccionUsuario { get; set; }
        public string NumeroUsuario { get; set; }
        //Checkout-------------------------
        public string FormaDeEntrega { get; set; } //Takeout o Delivery
        public string FormaDePago { get; set; }  // Efectivo o Tarjeta;
        public string NumeroDeTarjeta { get; set; }
        public string FechaDeExpiracionDeTarjeta { get; set; } 
    }
}
