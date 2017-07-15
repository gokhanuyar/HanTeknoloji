$(function () {
    $.ajax({
        type: "get",
        url: "/Admin/AdminHome/BarChart",
        success: function (result) {
            Morris.Bar({
                element: 'morris-bar-chart',
                data: result,
                xkey: 'month',
                ykeys: ['kar', 'ciro'],
                labels: ['Kar', 'Ciro'],
                hideHover: 'auto',
                resize: true
            });
        }
    });
});

$("#product-sale").click(function () {
    $("#content").empty();
    $.ajax({
        type: "get",
        url: "/Admin/AdminHome/GetSales",
        success: function (result) {
            $.each(result, function (i, item) {
                $("#content").append("<tr>" +
                    "<td>" + item.Product.TradeMark + " " + item.Product.ProductModel + "</td>" +
                    "<td>" + item.Quantity + "</td>" +
                    "<td>" + item.KdvPrice + " " + "TL</td>" +
                    "<td>" + item.Price + " " + "TL</td>" +
                    "<td>" + item.SaleTime + "</td>" +
                    "<td>" + item.PaymentType + "</td></tr>")
            });
            $("#sale-modal").modal("show");
        }
    });
});

$("#service-sale").click(function () {
    $("#service-sale-content").empty();
    $.ajax({
        type: "get",
        url: "/Admin/AdminHome/GetServiceSales",
        success: function (result) {
            $.each(result, function (i, item) {
                $("#service-sale-content").append("<tr>" +
                    "<td>" + item.Note + "</td>" +
                    "<td>" + item.Price + " " + "TL</td>" +
                    "<td>" + item.SaleTime + " " + "TL</td>" +
                    "<td>" + item.PaymentType + "</td></tr>")
            });
            $("#service-sale-modal").modal("show");
        }
    });
});

$("#few-counted-products").click(function () {
    $("#few-counted-content").empty();
    $.ajax({
        type: "get",
        url: "/Admin/AdminHome/FewCountedProducts",
        success: function (result) {
            $.each(result, function (i, item) {
                $("#few-counted-content").append("<tr>" +
                    "<td>" + item.SerialNumber + "</td>" +
                    "<td>" + item.TradeMark + " " + item.ProductModel + "</td>" +
                    "<td>" + item.Count + "</td></tr>")
            });
            $("#few-counted-modal").modal("show");
        }
    });
});

$("#payments").click(function () {
    $("#payment-content").empty();
    $.ajax({
        type: "get",
        url: "/Admin/AdminHome/WeeklyPayments",
        success: function (result) {
            $.each(result, function (i, item) {
                $("#payment-content").append("<tr>" +
                    "<td>" + item.Name + "</td>" +
                    "<td>" + item.SalePrice + "</td>" +
                    "<td>" + item.PaidPrice + "</td>" +
                    "<td>" + item.ExpiryValue + "</td></tr>")
            });
            $("#payment-modal").modal("show");
        }
    });
});

$("#collects").click(function () {
    $("#collect-content").empty();
    $.ajax({
        type: "get",
        url: "/Admin/AdminHome/WeeklyCollects",
        success: function (result) {
            $.each(result, function (i, item) {
                $("#collect-content").append("<tr>" +
                    "<td>" + item.Name + "</td>" +
                    "<td>" + item.SalePrice + "</td>" +
                    "<td>" + item.PaidPrice + "</td>" +
                    "<td>" + item.ExpiryValue + "</td></tr>")
            });
            $("#collect-modal").modal("show");
        }
    });
});

$("#pasted-payments").click(function () {
    $("#pasted-payment-content").empty();
    $.ajax({
        type: "get",
        url: "/Admin/AdminHome/PastedPayments",
        success: function (result) {
            $.each(result, function (i, item) {
                $("#pasted-payment-content").append("<tr>" +
                    "<td>" + item.Name + "</td>" +
                    "<td>" + item.SalePrice + "</td>" +
                    "<td>" + item.PaidPrice + "</td>" +
                    "<td>" + item.ExpiryValue + "</td></tr>")
            });
            $("#pasted-payment-modal").modal("show");
        }
    });
});

$("#pasted-collects").click(function () {
    $("#pasted-collect-content").empty();
    $.ajax({
        type: "get",
        url: "/Admin/AdminHome/PastedCollects",
        success: function (result) {
            $.each(result, function (i, item) {
                $("#pasted-collect-content").append("<tr>" +
                    "<td>" + item.Name + "</td>" +
                    "<td>" + item.SalePrice + "</td>" +
                    "<td>" + item.PaidPrice + "</td>" +
                    "<td>" + item.ExpiryValue + "</td></tr>")
            });
            $("#pasted-collect-modal").modal("show");
        }
    });
});