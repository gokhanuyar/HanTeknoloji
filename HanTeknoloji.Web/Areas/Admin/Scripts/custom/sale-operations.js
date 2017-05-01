﻿/// <reference path="../../Content/vendor/jquery/jquery.min.js" />

//$("#bardoce-btn").click(function () {
//    var barcode = $("#BarcodeNumber").val().trim();
//    if (barcode != "") {
//        $.ajax({
//            type: "get",
//            url: "/Admin/AdminSale/GetProductBySerialNumber/" + barcode,
//            success: function (result) {
//                if (result != null) {
//                    $(".table-body").append("<tr id='tr_" + result.ID + "'></tr>");
//                    $("#tr_" + result.ID).append("<td>" + result.SerialNumber + "</td>");
//                    $("#tr_" + result.ID).append("<td>" + result.TradeMark + result.ProductModel + "</td>");
//                    $("#tr_" + result.ID).append("<td>" + result.Color + "</td>");
//                    $("#tr_" + result.ID).append("<td>" + result.Count + "</td>");
//                    $("#tr_" + result.ID).append("<td>" + result.UnitPrice + "</td>");
//                }
//            }
//        })
//    }
//});

$("#BarcodeNumber").focus();
$("#BarcodeNumber").change(function () {
    $("#barcode-form").submit();
})

$(".quantity-input").change(function () {
    var quantity = this.value;
    var id = parseInt(this.id.split('_')[1]);
    var stockQuantity = parseInt($(this).attr("max"));
    if (quantity > stockQuantity) {
        sweetAlert("Uyarı", "Girilen adet sayısı stokta mevcut değil !", "warning");
        $(this).val($("#hidden_" + id).val());
    }
    else {
        location.href = "/Admin/AdminSale/ChangeQuantity/" + id + "/" + quantity;
    }
});

$(".total-input").change(function () {
    var data = new Object();
    data.Price = this.value;
    data.ID = parseInt(this.id.split('_')[1]);
    $.ajax({
        type: "post",
        url: "/Admin/AdminSale/ChangeProductSalePrice",
        data: data,
        success: function (result) {
            if(result=="succ"){
                window.location.href = "/Admin/AdminSale/Index"
            }
            else {
                sweetAlert("Hatalı Giriş", "Lütfen yalnız rakam girildiğinden emin olun ve nokta yerine virgül kullanın !", "warning");
            }
        }
    });
});

function QuantityPlus(id) {
    var firstQuantity = $("#quantity_" + id).val();
    var firstPrice = $("#hidden_" + id).val().replace(",", ".");
    var totalPrice = $(".total-price").text().replace(",", ".");

    var lastQuantity = parseFloat(firstQuantity) + 1;
    var lastPrice = (parseFloat(firstPrice) * lastQuantity).toFixed(1);
    totalPrice = (parseFloat(totalPrice) + parseFloat(firstPrice)).toFixed(1);

    $(".total-price").text(totalPrice);
    $("#quantity_" + id).val(lastQuantity);
    $("#total_" + id).text(lastPrice);
    $.ajax({
        type: "get",
        url: "/AlphaCart/ChangeQuantity/" + id + "/" + lastQuantity,
    });
}