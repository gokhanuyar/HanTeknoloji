﻿
function SetButton(id) {
    $("#post-btn").attr("onclick", "Pay(" + id + ")");
}

function Pay(id) {
    var value = $("#pay-value").val().replace(",", ".");
    if ($.isNumeric(value)) {
        var debt = $("#expiry-value").text().replace(",", ".");
        var sonuc = parseFloat(debt) - parseFloat(value);
        if (sonuc < 0) {
            sweetAlert("Uyarı", "Girilen miktar borç değerinin üzerindedir ! ", "warning");
        }
        else {
            var data = new Object();
            data.ID = id;
            data.Price = value.replace(".", ",");
            $.ajax({
                type: "post",
                url: "/Admin/AdminExpiry/PaySupplier",
                data: data,
                success: function (result) {
                    location.reload();
                }
            });
        }
    }
    else {
        sweetAlert("Uyarı", "Teslim alınan bakiye sayı değeri olmalıdır ! ", "warning");
    }
}

function Details(id) {
    $("#sale-datails-content").empty();
    $.ajax({
        type: "get",
        url: "/Admin/AdminExpiry/GetProductDetails/" + id,
        success: function (result) {
                $("#sale-datails-content").append("<tr>" +
                    "<td>" + result.Note + "</td>" +
                    "<td>" + result.Quantity + "</td>" +
                    "<td>" + result.Price.toFixed(2) + " " + "TL</td ></tr > ");
            $("#myModal2").modal("show");
        }
    })
}

function Payments(id) {
    $("#expiry-content").empty();
    $.ajax({
        type: "get",
        url: "/Admin/AdminExpiry/GetPayments/" + id,
        success: function (result) {
            if (result.length > 0) {
                $.each(result, function (i, item) {
                    $("#expiry-content").append("<tr>" +
                        "<td>" + item.AdminUserName + "</td>" +
                        "<td>" + item.Date + "</td>" +
                        "<td>" + item.Hour + "</td>" +
                        "<td>" + item.Price + " " + "TL</td ></tr > ");
                });
                $("#myModal3").modal("show");
            }
            else {
                swal("Uyarı", "Ödeme geçmişi bulunmamaktadır.", "warning");
            }
        }
    })
}