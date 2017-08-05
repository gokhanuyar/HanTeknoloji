/// <reference path="../../Content/vendor/jquery/jquery.min.js" />


$("#date").change(function () {
    $("#form_date").submit();
    //console.log($(this).val())
})
$("#month").change(function () {
    $("#form_month").submit();
})

function Customer(id) {
    $("#modal-customer-content").empty();
    $.ajax({
        type: "get",
        url: "/Admin/AdminReport/GetCustomer/" + id,
        success: function (result) {
            if (result.IsPerson) {
                $("#modal-customer-content").append("<h4>Müşteri Adı/Ünvanı</h4>" +
                    "<p>" + result.Name + "</p><hr />" +
                    "<h4>T.C. Numarası</h4>" +
                    "<p>" + result.TCNo + "</p><hr />" +
                    "<h4>Telefon Numarası</h4>" +
                    "<p>" + result.Phone + "</p><hr />" +
                    "<h4>Adres</h4>" +
                    "<p>" + result.Address + "</p>" +
                    "<p>" + result.City + " / " + result.Region + "</p>");
            }
            else {
                $("#modal-customer-content").append("<h4>Müşteri Adı/Ünvanı</h4>" +
                    "<p>" + result.Name + "</p><hr />" +
                    "<h4>Telefon Numarası</h4>" +
                    "<p>" + result.Phone + "</p><hr />" +
                    "<h4>Adres</h4>" +
                    "<p>" + result.Address + "</p>" +
                    "<p>" + result.City + " / " + result.Region + "</p><hr />" +
                    "<h4>Vergi Dairesi</h4>" +
                    "<p>" + result.TaxOffice + "</p><hr />" +
                    "<h4>Vergi Numarası</h4>" +
                    "<p>" + result.TaxNumber + "</p>");
            }
            $("#myModal").modal("show");
        }
    })
}

function GetDetail(id) {
    $("#modal-detail-content").empty();
    $.ajax({
        type: "get",
        url: "/admin/adminreport/GetSaleDetail/" + id,
        success: function (result) {
            $("#modal-detail-content").append("<h4>Ödeme Şekli :</h4>" +
                "<p>" + result.PaymentType + "</p><hr/>" +
                "<h4>Satış Tarihi :</h4>" +
                "<p>" + result.SaleDate + "</p><hr/>" +
                "<h4>Satış Saati :</h4>" +
                "<p>" + result.SaleTime + "</p><hr/>" +
                "<h4>Fatura Tarihi :</h4>" +
                "<p>" + result.InvoiceDate + "</p><hr/>" +
                "<h4>Personel :</h4>" +
                "<p>" + result.AdminUserName + "</p>");
            if (result.ImeiList.length > 0) {
                for (var i = 0; i < result.ImeiList.length; i++) {
                    var index = i + 1;
                    $("#modal-detail-content").append("<hr/><h4>" + index + ".IMEI Numarası</h4>" +
                        "<p>" + result.ImeiList[i] + "</p>");
                }
            }
            $("#myModal").modal("show");
        }
    })
}