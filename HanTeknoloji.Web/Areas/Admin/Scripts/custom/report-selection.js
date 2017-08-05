
$("#_dateSelect").change(function () {
    var value = this.value;
    $(".date-input").hide();
    if (value == "1") {
        $("#_date").show();
    }
    else if (value == "2") {
        $("#_month").show();
    }
})

$("#selection-post-btn").click(function () {
    var value = $("#_dateSelect").val();
    if (value == 1) {
        var date = $("#_date").val();
        if (date == "" || date == "," || date == null) {
            swal("Uyarı", "Gün seçimi için tarih girişi yapmalısınız", "warning")
        }
        else {
            $("#selection-form").submit();
        }
    }
    else if (value == 2) {
        var date = $("#_month").val();
        if (date == "" || date == "," || date == null) {
            swal("Uyarı", "Ay seçimi için tarih girişi yapmalısınız", "warning")
        }
        else {
            $("#selection-form").submit();
        }
    }
    else {
        $("#selection-form").submit();
    }
})

$("#selection-modal-btn").click(function () {
    $("#selectionModal").modal("show");
})