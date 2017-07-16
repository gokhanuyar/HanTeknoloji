/// <reference path="../../Content/vendor/jquery/jquery.min.js" />

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
            if (result == "succ") {
                window.location.href = "/Admin/AdminSale/Index";
            }
            else {
                sweetAlert("Hatalı Giriş", "Lütfen yalnız rakam girildiğinden emin olun ve nokta yerine virgül kullanın !", "warning");
            }
        }
    });
});

//function QuantityPlus(id) {
//    var firstQuantity = $("#quantity_" + id).val();
//    var firstPrice = $("#hidden_" + id).val().replace(",", ".");
//    var totalPrice = $(".total-price").text().replace(",", ".");

//    var lastQuantity = parseFloat(firstQuantity) + 1;
//    var lastPrice = (parseFloat(firstPrice) * lastQuantity).toFixed(1);
//    totalPrice = (parseFloat(totalPrice) + parseFloat(firstPrice)).toFixed(1);

//    $(".total-price").text(totalPrice);
//    $("#quantity_" + id).val(lastQuantity);
//    $("#total_" + id).text(lastPrice);
//    $.ajax({
//        type: "get",
//        url: "/AlphaCart/ChangeQuantity/" + id + "/" + lastQuantity,
//    });
//}

$("#sale-button").click(function () {
    var payment = $("#payment-type").find(":selected").text();
    var invoice = $("select[name='Invoice']").val();
    var customer = $("#CustomerID").val();
    var priceString = $.trim($("input[name='PriceString']").val());
    var expiryDate = $("input[name='ExpiryDate']").val();
    var datetimeNow = $.format.date(new Date($.now()), "yyyy-MM-dd");
    var expiryValue = $("input[name='PaidExpiryValue']").val();
    var price = $("#price-control").text().replace(",", ".");

    if (customer == "" && invoice == 1) {
        sweetAlert("Uyarı", "Faturalı satışlar için müşteri seçmek zorundasınız !", "warning");
    }
    else if (priceString == "" && invoice == 1) {
        sweetAlert("Uyarı", "Satış fiyatını yazılı olarak giriniz !", "warning");
    }
    else if (payment == "Vadeli" && expiryDate == "") {
        sweetAlert("Uyarı", "Vade tarihini giriniz !", "warning");
    }
    else if (!$.isNumeric(expiryValue) && payment == "Vadeli") {
        sweetAlert("Uyarı", "Teslim alınan bakiye sayı değeri olmalıdır ! ", "warning");
    }
    else if (new Date(datetimeNow) > new Date(expiryDate)) {
        sweetAlert("Uyarı", "Vade tarihi için bugünden sonraki bir tarih seçiniz !", "warning");
    }
    else if (payment == "Vadeli" && customer == "") {
        sweetAlert("Uyarı", "Vadeli satışlar için müşteri seçiniz !", "warning");
    }
    else {
        if (price > 0) {
            var newVal = expiryValue.replace(".", ",");
            $("input[name='ExpiryValue']").val(newVal);
            $("#sale-form").submit();
        }
    }
});

$("select[name='Invoice']").change(function () {
    var payment = $("#payment-type").find(":selected").text();
    value = this.value;
    if (value == 1) {
        $(".display-input").fadeIn();
    }
    else {
        if (payment == "Vadeli") {
            $(".optional").fadeOut();
        }
        else {
            $(".display-input").fadeOut();
        }
    }
})

$("#service-sale-button").click(function () {
    var price = $.trim($("#service-sale-price").val());

    if (price == "") {
        sweetAlert("Uyarı", "Lütfen tutar giriniz!", "warning");
    }
    else {
        var newPrice = price.replace(".", ",");
        $("#service-sale-price").val(newPrice);
        $("#service-sale-form").submit();
    }
});

$("select[name='PaymentType']").change(function () {
    var invoiceValue = $("select[name='Invoice']").val();
    var value = this.value;
    if (value == "Vadeli") {
        $(".expiry-date").fadeIn();
        $(".display-input-customer").fadeIn();
    }
    else {
        $(".expiry-date").fadeOut();
        if (invoiceValue != 1) {
            $(".display-input-customer").fadeOut();
        }
    }
});

function SuccessFunction(result) {
    $("#response").html(result.Message);
    $("#response").attr("class", result.CssClass);
}

function ImeiFunction(result) {
    $("#imei-response").html(result.Message);
    $("#imei-response").attr("class", result.CssClass);
}


$(function () {
    $.ajax({
        type: "get",
        datatype: "json",
        url: "/Admin/AdminSale/GetPhoneProducts",
        success: function (result) {
            $("#imei-pro-select").append("<option value='0'>Ürün Seçiniz</option>");
            $.each(result, function (i, item) {
                $("#imei-pro-select").append("<option value='" + item.ID + "'>" + item.TradeMark + " " + item.ProductModel + "</option>")
            })
        }
    })
})

$("#imei-pro-select").change(function () {
    var id = this.value;
    if (id != 0) {
        $.ajax({
            type: "get",
            url: "/Admin/AdminSale/GetPhoneProductCount/" + id,
            success: function (result) {
                $("#imei-count-hidden").val(result.Count);
                for (var i = 0; i < result.Count; i++) {
                    $("#imei-form").append("<div class='imei-wrap'><div class='form-group' style='display:grid;'>" +
                        "<label>IMEI</label>" +
                        "<select class='form-control' id='imei-number-select_" + i + "' name='imei-number-select_" + i + "'></select></div></div>");

                    $.each(result.ImeiList, function (key, item) {
                        $("#imei-number-select_" + i).append("<option value='" + item.ID + "'>" + item.IMEI + "</option>");
                    });
                }
                $("#imei-form").append("<button class='btn btn-success' type='submit'>Gönder</button>");
            }
        })
    }
    else {
        $(".imei-wrap").remove();
    }
})