using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DeliveriandoWebApp.Models
{
    public class DBModels
    {
        public class DefaultConnection : DbContext
        {
            public DefaultConnection()
                : base("DefaultConnection")
            {
            }

            public DbSet<RestauranteModels> Restaurantes { get; set; }
            public DbSet<Product> Products { get; set; } //Los Platos
            public DbSet<OrdenModels> Ordenes { get; set; } // Las Ordenes
            public DbSet<OrdenProduct> OrdenProducts { get; set; } //Esta es una subclase de OrdenModels
            public IDbSet<CoordenadasModels> Coordenadas { get; set; }//Las coordenadas

          
        }
    }
}