$("#date").change(function () {
    $("#form_date").submit();
})
$("#month").change(function () {
    $("#form_month").submit();
})
function GetDetail(id) {
    $(".modal-body").empty();
    $.ajax({
        type: "get",
        url: "/admin/adminreport/GetServiceSaleDetail/" + id,
        success: function (result) {
            $(".modal-body").append("<h4>Müşteri :</h4>" +
                "<p>" + result.CustomerName + "</p><hr/>" +
                "<h4>Kayıt Yapan Personel :</h4>" +
                "<p>" + result.User + "</p><hr/>" +
                "<h4>İşlem Yapan Personel :</h4>" +
                "<p>" + result.Employee + "</p><hr/>" +
                "<h4>Tarih :</h4>" +
                "<p>" + result.SaleDate + "</p> <hr />" +
                "<h4>Ürün :</h4>" +
                "<p>" + result.ProductModel + "</p><hr />" +
                "<h4>IMEI :</h4>" +
                "<p>" + result.IMEINumber + "</p>");
            $("#myModal").modal("show");
        }
    })
}