
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
