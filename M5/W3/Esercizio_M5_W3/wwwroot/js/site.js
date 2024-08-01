$(document).ready(function () {
    $(".add-to-cart").click(function () {
        var productId = $(this).data("id");
        $.ajax({
            type: "POST",
            url: addToOrderUrl,
            data: { productId: productId },
            success: function (response) {
                if (response.success) {
                    alert("Prodotto aggiunto al carrello!");
                } else {
                    alert("Errore durante l'aggiunta del prodotto al carrello: " + response.message);
                }
            },
            error: function () {
                alert("Errore durante l'aggiunta del prodotto al carrello.");
            }
        });
    });
});
