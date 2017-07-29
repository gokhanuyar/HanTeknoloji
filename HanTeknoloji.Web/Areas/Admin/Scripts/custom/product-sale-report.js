$("#date").change(function () {
            $("#form_date").submit();
        });
        $("#month").change(function () {
            $("#form_month").submit();
        });

        function GetDetail(id) {
            $(".modal-body").empty();
            $.ajax({
                type: "get",
                url: "/admin/adminreport/GetSaleDetail/" + id,
                success: function (result) {
                    $(".modal-body").append("<h4>Ödeme Şekli :</h4>" +
                        "<p>" + result.PaymentType + "</p><hr/>" +
                        "<h4>Satış Tarihi :</h4>" +
                        "<p>" + result.SaleDate + "</p><hr/>" +
                        "<h4>Satış Saati :</h4>" +
                        "<p>" + result.SaleTime + "</p><hr/>" +
                        "<h4>Personel :</h4>" +
                        "<p>" + result.AdminUserName + "</p>");
                    if (result.ImeiList.length > 0) {
                        for (var i = 0; i < result.ImeiList.length; i++) {
                            var index = i + 1;
                            $(".modal-body").append("<hr/><h4>" + index + ".IMEI Numarası</h4>" +
                                "<p>" + result.ImeiList[i] + "</p>");
                        }
                    }                    
                    $("#myModal").modal("show");
                }
            })
        }