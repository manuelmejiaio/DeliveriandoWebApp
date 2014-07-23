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
    var nombreCompleto;
    var numero;
    var direccion;
    var fecha;
    var numeroOrden;
    var tablebegin = "<table> <tr> <th>&nbspPlato</th> <th>&nbspCantidad &nbsp</th> <th>&nbspPrecio c/u &nbsp</th>";
    var platosCantidadPrecio = " ";
    var tableend = "</table><br><br>";
    var subtotal;
    var itbis;
    var finaltotal;
    
    listaDelJSON.forEach(function (entry) {
        platosCantidadPrecio += " <tr><td>&nbsp" + entry.product.nombre + "&nbsp </td><td>&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp " + entry.quantity + "&nbsp </td><td>&nbspRD$ " + entry.product.precio.toFixed(2) + "&nbsp </td></tr> ";
        numero = "Numero: <b>" + entry.product.NumeroUsuario + "</b> <br>";
        direccion = "Dirección: <b>" + entry.product.DireccionUsuario + "</b> <br>";
        nombreCompleto = "Nombre: <b>" + entry.product.NombreUsuario + "</b> <br>";
        fecha = entry.product.Fecha ;
        //fecha = "Fecha: <b>" + entry.product.Fecha + "</b> <br>";
        numeroOrden = "Orden: <b>" + entry.IdOrdenesUnicas + "</b> <br><br>";
        subtotal = "Sub-total: <b>RD$  " + entry.total.toFixed(2) + "</b><br>";
        itbis = "ITBIS: <b>RD$ " + entry.ITBIS.toFixed(2) + "</b> <br>";
        finaltotal = "<h2> Total a pagar: RD$ " + entry.finalTotal.toFixed(2) + "</h2>";

    });

    //Force Para mostrar la fecha en un formato correcto
    var milli = fecha.replace(/\/Date\((-?\d+)\)\//, '$1');
    var d = new Date(parseInt(milli));

    var month = d.getMonth() + 1; // porque enero es 0
    var day = d.getDate();
    var year = d.getFullYear();
    var hour = d.getHours() + 4; //le agregamos 4 hora por problema con le servidor
    var minute = d.getMinutes();
    var seconds = d.getSeconds();
    var ap = "AM";

    if (hour > 11) { ap = "PM"; }
    if (hour > 12) { hour = hour - 12; }
    if (hour == 0) { hour = 12; }

    var fechafinal = "Fecha: <b>" + month + "/" + day + "/" + year + "&nbsp&nbsp " + hour + ":" + minute + ":" + seconds + "</b> <br>";
 

    bootbox.dialog({
        message: nombreCompleto + numero + direccion + fechafinal + numeroOrden + tablebegin + platosCantidadPrecio + tableend + subtotal + itbis + finaltotal,
        title: "Detalles de tu orden.",
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

function ComentarLaOrden(id)
{
    bootbox.prompt("Dejenos saber su opinión acerca de su orden:", function (opinion) {
        if (opinion == null) {  } //no hagas nada.
        else if (opinion < 5) {
            bootbox.alert("Escriba un comentario.", function () { ComentarLaOrden(id) });
        }
        else {
            $.ajax({
                type: "POST",
                url: '/Orden/ComentarOrden',
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify({ idOrden: id, comentario: opinion }),
                dataType: "json",
                success: function (data) {
                    bootbox.alert("Gracias por dejarnos saber su opinión !", function () { });
                }
            });
        }
    });

}