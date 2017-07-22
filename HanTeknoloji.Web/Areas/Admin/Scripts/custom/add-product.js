
$("#TradeMarkID").change(function () {
    $("#ProductModelID").empty();
    var id = $(this).val();
    $.ajax({
        type: "get",
        url: "/Admin/AdminProduct/GetProductModelsByID/" + id,
        success: function (data) {
            if (data.length != 0) {
                $.each(data, function (i, item) {
                    $("#ProductModelID").append('<option value="' + item.ID + '">' + item.Name + '</option>');
                });
            }
        }
    })
});

//$("#CategoryID").change(function () {
//    if (this.value == 6 || this.value == 7) {
//        $(".imei-box").fadeIn();
//    }
//    else {
//        $(".imei-box").fadeOut();
//    }
//});

$("#Payment").change(function () {
    $(".display-row").hide();
    var value = this.value;
    switch (value) {
        case "Kredi Kartı":
            $(".credit-card").show();
            break;
        case "Havale":
            $(".transfer").show();
            break;
        case "Vadeli":
            $(".expiry").show();
            break;
        case "Çek":
            $(".expiry-date").show();
            $(".check").show();
            break;
    }
})

$(function () {
    var value = $("#hidden-payment").val();
    switch (value) {
        case "Kredi Kartı":
            $(".credit-card").show();
            $("#Payment").val("Kredi Kartı");
            break;
        case "Havale":
            $(".transfer").show();
            $("#Payment").val("Havale");
            break;
        case "Vadeli":
            $(".expiry").show();
            $("#Payment").val("Vadeli");
            break;
        case "Çek":
            $(".expiry-date").show();
            $(".check").show();
            $("#Payment").val("Çek");
            break;
        default:
            $("#Payment").val("Nakit");
            break;
    }
});

$(".btn-primary").click(function () {
    var payment = $("#Payment").val();
    var date = $("#ExpiryDate").val();
    var categoryId = $("#CategoryID").val();
    var datetimeNow = $.format.date(new Date($.now()), "yyyy-MM-dd");
    if (payment == "Vadeli" && date == "" || payment == "Çek" && date == "") {
        swal("Uyarı", "Vadeli alımlarda vade tarihini girmelisiniz.", "warning");
    }
    else if ((payment == "Vadeli" || payment == "Çek") && new Date(datetimeNow) > new Date(date)) {
        sweetAlert("Uyarı", "Vade tarihi için bugünden sonraki bir tarih seçiniz !", "warning");
    }
    else if (categoryId == 6 || categoryId == 8) {
        swal("Uyarı", "Yeni telefon veya bilgisayar ürünlerinde barkod numarası girmelisiniz.", "warning");
    }
    else {
        $("#form").submit();
    }
});

$("#barcode-read-input").change(function () {
    $("#barcode-form").submit();
});
