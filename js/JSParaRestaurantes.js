function DetallesDeLaOrden(id) {
    $.ajax({
        type: "POST",
        url: '/Orden/filtroDeGuid',
        contentType: 'application/json; charset=utf-8',
        data: '{id:' + id + '}',
        dataType: "json",
        success: function (data) {
            DetallesEnBootbox(data) //Aqui enviamo el JSON listo para ser mostrados en la funcion DetallesEnBootbox()
        }
    });
}

// Aquie recibe el JSON de los productos que tienen el mismo GUID y se muestran los detalles.
function DetallesEnBootbox(listaDelJSON) {
    var espacioCliente = "<b>--INFORMACION DEL CLIENTE:</b><br>  ";
    var nombreCompleto;
    var numero;
    var direccion;
    var motor;
    var guid;
    var espacioPagoEntrega = "<br><b>--FORMA DE ENTREGA:</b><br>";
    var formaDeEntrega;
    var formaDePago;
    var numeroDeTarjeta;
    var fechaDeExpiracionDeTarjeta;
    var espacioPlatos = "<br><b>--PLATOS SELECIONADOS:</b>";
    var tablebegin = "<table> <tr> <th>&nbspPlato</th> <th>&nbspCantidad &nbsp</th> <th>&nbspPrecio c/u &nbsp</th>";
    var platosCantidadPrecio =  " ";
    var tableend = "</table><br>";
    var espacioCobro = "<b>--A COBRAR:</b><br>";
    var subtotal;
    var itbis;
    var finaltotal;
    

    listaDelJSON.forEach(function (entry) {
        nombreCompleto ="Nombre: <b>" + entry.product.NombreUsuario + "</b> <br>";
        numero = "Numero: <b>" + entry.product.NumeroUsuario + "</b> <br>";
        direccion = "Dirección: <b>" + entry.product.DireccionUsuario + "</b> <br>";
        formaDeEntrega = "Entrega: <b>" + entry.product.FormaDeEntrega + "</b> <br>";
        formaDePago = "Pago: <b>" + entry.product.FormaDePago + "</b> <br>";
        numeroDeTarjeta = "Numero de tarjeta: <b>" + entry.product.NumeroDeTarjeta + "</b> <br>";
        fechaDeExpiracionDeTarjeta = "Fecha de expiración: <b>" + entry.product.FechaDeExpiracionDeTarjeta + "</b> <br>";
        motor = "Motor #: <b>" + entry.product.IdMotorista + "</b> <br>";
        guid = "Orden #: <b>" + entry.IdOrdenesUnicas + "</b> <br>";
        platosCantidadPrecio += " <tr><td>&nbsp" + entry.product.nombre + "&nbsp </td><td>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp " + entry.quantity + "&nbsp </td><td>&nbspRD$ " + entry.product.precio.toFixed(2) + "&nbsp </td></tr> ";
        subtotal = "Sub-total: <b>RD$  " + entry.total.toFixed(2) + "</b><br>";
        itbis = "ITBIS: <b>RD$ " + entry.ITBIS.toFixed(2) + "</b> ";
        finaltotal =  "<h2> Total a pagar: RD$ " + entry.finalTotal.toFixed(2) + "</h2>";
    });

    bootbox.dialog({
        message: espacioCliente + nombreCompleto + numero + direccion + espacioPagoEntrega + formaDeEntrega + formaDePago + numeroDeTarjeta + fechaDeExpiracionDeTarjeta + motor + guid + espacioPlatos + tablebegin + platosCantidadPrecio + tableend + espacioCobro + subtotal + itbis + finaltotal,
        title: "Detalles de la orden.",
        buttons: {
            success: {
                label: "OK",
                className: "btn-danger",
                callback: function () {

                }
            }
        }
    });
}

function EnviarOrden(id) {

    $.ajax({
        type: "POST",
        url: '/Orden/filtroDeGuid',
        contentType: 'application/json; charset=utf-8',
        data: '{id:' + id + '}',
        dataType: "json",
        success: function (data) {
            envio(data) //Aqui enviamo el JSON listo para ser mostrados en la funcion DetallesEnBootbox()
        }
    });

    function envio(data) {

        var formaDeEntrega; // para saber si es delivery o takeout
        data.forEach(function (entry) {
            formaDeEntrega = entry.product.FormaDeEntrega;

        });

        if (formaDeEntrega == "Delivery") {
            bootbox.prompt("Ingrese el #ID del motor con la cual sera enviada la orden:", function (idMotor) {
                if (idMotor == null) { } //no hagas nada.
                else if (isNaN(idMotor) == true || idMotor < 1 || idMotor > 10) {
                    bootbox.alert("Error, ingrese un #ID del motor correcto.", function () { EnviarOrden(id); });
                }
                else {
                    $.ajax({
                        type: "POST",
                        url: '/Orden/SendOrder',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ idOrden: id, idMotor: idMotor }),
                        dataType: "json",
                        success: function (data) { window.location.href = "/Orden/RestauranteView"; }
                    });
                }
            });
        }
        else {
            var idMotor = 0; // necesario para el metodo /Orden/SendOrder
            $.ajax({
                type: "POST",
                url: '/Orden/SendOrder',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ idOrden: id, idMotor: idMotor }),
                dataType: "json",
                success: function (data) { window.location.href = "/Orden/RestauranteView"; }
            });

        }

        
    }
}

function CancelarOrden(idOrden) {

    bootbox.prompt("Introduzca su contraseña:", function (password) {
        if (password == null) { } //no hagas nada.

        else if (password === "12345") {

            bootbox.prompt("Motivo de cancelación:", function (motivo) {
                if (motivo != null) {
                    $.ajax({
                        type: "POST",
                        url: '/Orden/Cancel',
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify({ idOrden: idOrden, motivo: motivo }),
                        dataType: "json",
                        success: function (data) { window.location.href = "/Orden/AllCancelOrdersRestauranteView"; }
                    });
                }
                else {
                    bootbox.alert("Motivo incorrecto, intente de nuevo.", function () { CancelarOrden(idOrden); });
                }
            });
        }

        else {
            bootbox.alert("Contraseña incorrecta, intente de nuevo.", function () { CancelarOrden(idOrden); });
        }
    });


   
        
    

    

}