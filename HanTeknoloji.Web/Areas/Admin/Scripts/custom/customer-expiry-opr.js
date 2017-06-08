
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
                url: "/Admin/AdminExpiry/Pay",
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