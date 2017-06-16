
function Delete(id) {
    swal({
        title: "Emin misiniz?",
        text: "Onayladığınız takdirde veri silinecektir !",
        type: "warning",
        showCancelButton: true,
        confirmButtonColor: "#DD6B55",
        confirmButtonText: "Evet, sil !",
        cancelButtonText: "İptal",
        closeOnConfirm: false
    },
        function () {
            window.location.href = "/Admin/AdminSupplier/Delete/" + id;
        });
}