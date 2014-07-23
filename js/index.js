$(function () {

    // function helper
    OrdersApp.formatCurrency = function (value) {
        return "RD$" + value.toFixed(2);
    };

    OrdersApp.objectInArray = function (searchFor, property) {
        var retVal = false;
        $.each(this, function (index, item) {
            if (item.hasOwnProperty(property)) {
                if (item[property]() === searchFor) {
                    retVal = item[property];
                    return retVal;
                }
            }
        });
        return retVal;
    };
    Array.prototype.objectInArray = OrdersApp.objectInArray;

    // for creating Product Models
    OrdersApp.Product = function (selectedItem) {
        var self = this;
        self.id = ko.observable();
        self.nombre = ko.observable();
        self.categoria = ko.observable();
        self.descripcion = ko.observable();
        self.precio = ko.observable();
        self.IdRestaurante = ko.observable();
        self.isSelected = ko.computed(function () {
            return selectedItem() === self;
        });
        self.shortDesc = ko.computed(function () {
            return this.nombre() ? this.nombre() : "";
        }, self),
        self.stateHasChanged = ko.observable(false);
    };

    OrdersApp.CartItem = function () {
        var self = this;
        self.product = ko.observable();
        self.quantity = ko.observable();
        self.extPrice = ko.computed(function () {
            return this.product() ? this.product().precio() * this.quantity() : 0;
        }, self);
    };





    // The ViewModel
   
    OrdersApp.vm = function () {
        defaultAnimationSpeed = 500,
        products = ko.observableArray([]),
        selectedProduct = ko.observable(),
        sortFunction = function (a, b) {
            return a.shortDesc().toLowerCase() > b.shortDesc().toLowerCase() ? 1 : -1;
        },
        selectProduct = function (p) {
            selectedProduct(p);
        },
        hideItem = function (elem) {
            if (elem.nodeType === 1) {
                var effect = function () {
                    return $(elem).fadeOut(defaultAnimationSpeed);
                };
                effect();
            }
        },
        showItem = function (elem) {
            if (elem.nodeType === 1) {
                var effect = function () {
                    return $(elem).hide().fadeIn(defaultAnimationSpeed);
                };
                effect();
            }
        },
        shoppingCart = ko.observableArray([]),
        addToCart = function (product) {
            if (!shoppingCart().objectInArray(product, "product")) {
                var cartItem = new OrdersApp.CartItem()
                                            .product(product)
                                            .quantity(1);
                shoppingCart.push(cartItem);
                products.remove(product);
            }
        },
        removeFromCart = function (cartItem) {
            if (shoppingCart().indexOf(cartItem) > -1) {
                products.push(cartItem.product());
                shoppingCart.remove(cartItem);
            }
        },
        grandTotal = ko.computed(function () {
            var total = 0;
            $.each(shoppingCart(), function () {
                total += this.extPrice();
            });
            return total;
        }),
        ITBIS = ko.computed(function () {
            var total = 0;
            total = grandTotal() * 0.18;
            return total;
        }),
        finalTotal = ko.computed(function () {
            var total = 0;
            total = grandTotal() + ITBIS();
            return total;
        }),
        loadProductsCallback = function (json) {
            $.each(json, function (i, p) {
                products.push(new OrdersApp.Product(selectedProduct)
                        .id(p.Id)
                        .precio(p.Precio)
                        .nombre(p.Nombre)
                        .categoria(p.Categoria)
                        .descripcion(p.Descripcion)
                        .IdRestaurante(p.IdRestaurante)
                        .stateHasChanged(false)
                );
            });
            products.sort(sortFunction);
        },
        loadProducts = function () {
            OrdersApp.shoppingDataService.getProducts(OrdersApp.vm.loadProductsCallback);
        },
        placeOrderCallback = function (json) {
            dialogOptions.title("Place Order").text(json.message).open(true);
            alert("co;o");
        },
        placeOrder = function () {
            
            var ordenes = ko.toJSON(shoppingCart); // aqui esta el KO a Json de los productos seleccionados
            var subtotal = ko.toJSON(grandTotal); // aqui esta el KO a Json del total de los productos seleccionados.


            bootbox.confirm({
                title: "<b>¿Está seguro de su orden?</b>",
                message: "<b>Sub-Total:</b> RD$" + grandTotal().toFixed(2) + "<br><b>ITBIS:</b> RD$" + ITBIS().toFixed(2) + "<h3><b>Total:</b> RD$" + finalTotal().toFixed(2) + "</h3>",
                buttons: {
                    'cancel': {
                        label: 'Cancelar',
                        className: 'btn-default'
                    },
                    'confirm': {
                        label: 'Aceptar',
                        className: 'btn-danger'
                    }
                },
                callback: function (result) {
                    if (result)
                    {
                        $.ajax('/Orden/PreOrder', {
                            data: '{ "ordenes":' + ordenes + ', "subtotal":"' + subtotal + '"}',  //Los parametros deben tener el mismo nombre que en el Controlador
                            type: "POST",
                            contentType: 'application/json; charset=utf-8',
                            dataType: "json",
                            error: function (xhr, status, error) {
                                console.log(error);
                            },
                            success: function (data) { window.location.href = "/Orden/CheckOut"; }
                        });
                    }
                }
            });


        };



        showSplash = function() {

            bootbox.dialog({
                message: "<b>Descripción:</b>  " + this.descripcion() + "<br><b>Categoría:</b> " + this.categoria(),
                buttons: {
                    success: {
                        label: "OK",
                        className: "btn-danger",
                        callback: function () {

                        }
                    }
                }
            });
            
        };
        



        return {
            selectedProduct: selectedProduct,
            selectProduct: selectProduct,
            showSplash: showSplash,
            products: products,
            loadProductsCallback: loadProductsCallback,
            loadProducts: loadProducts,
            placeOrderCallback: placeOrderCallback,
            placeOrder: placeOrder, 
            hideItem: hideItem,
            showItem: showItem,
            shoppingCart: shoppingCart,
            addToCart: addToCart,
            removeFromCart: removeFromCart,
            grandTotal: grandTotal,
            ITBIS: ITBIS,
            finalTotal: finalTotal
        };
    }();

    OrdersApp.vm.loadProducts();
    ko.applyBindings(OrdersApp.vm);
});