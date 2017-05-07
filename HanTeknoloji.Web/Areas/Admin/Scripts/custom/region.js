
$("#CityID").change(function () {
    $("#RegionID").empty();
    var id = $(this).val();
    $.ajax({
        type: "get",
        url: "/Admin/AdminSupplier/GetRegionsByID/" + id,
        success: function (data) {
            if (data.length != 0) {
                $.each(data, function (i, item) {
                    $("#RegionID").append('<option value="' + item.ID + '">' + item.Name + '</option>');
                });
            }
        }
    })
});