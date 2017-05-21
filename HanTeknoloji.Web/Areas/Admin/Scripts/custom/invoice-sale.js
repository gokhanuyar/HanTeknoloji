/// <reference path="../../Content/vendor/jquery/jquery.min.js" />


$("#date").change(function () {
    $("#form_date").submit();
    //console.log($(this).val())
})
$("#month").change(function () {
    $("#form_month").submit();
})

function Customer(id) {
    $(".modal-body").empty();
    $.ajax({
        type: "get",
        url: "/Admin/AdminReport/GetCustomer/" + id,
        success: function (result) {
            $(".modal-body").append("<h4>Müşteri Adı/Ünvanı</h4>" +
                "<p>" + result.Name + "</p><hr />" +
                "<h4>T.C. Numarası</h4>" +
                "<p>" + result.TCNo + "</p><hr />" +
                "<h4>Telefon Numarası</h4>" +
                "<p>" + result.Phone + "</p><hr />" +
                "<h4>Adres</h4>" +
                "<p>" + result.Address + "</p>" +
                "<p>" + result.City + " / " + result.Region + "</p><hr />" +
                "<h4>Vergi Dairesi</h4>" +
                "<p>" + result.TaxOffice + "</p><hr />" +
                "<h4>Vergi Numarası</h4>" +
                "<p>" + result.TaxNumber + "</p>");
            $("#myModal").modal("show");
        }
    })
}