
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
            $("#ProductModelID").append('<option value="0">-</option>');
        }
    })
});

$("#CategoryID").change(function () {
    if (this.value == 3 || this.value == 4) {
        $(".imei-box").fadeIn();
    }
    else {
        $(".imei-box").fadeOut();
    }
});

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
            $(".expiry-date").show();
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
            $(".expiry-date").show();
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
})
