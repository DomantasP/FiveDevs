function uploadExcel() {
    var formData = new FormData();

    var fileSelect = document.getElementById('excelUpload');
    if (fileSelect != null) {
        var files = fileSelect.files;

        formData.append('file', files[0]);
        console.log(files[0]);
    }


    $.ajax({
        url: '/Product/UploadProductByExcel',
        type: 'POST',
        async: false,
        cache: false,
        timeout: 30000,
        data: formData,
        processData: false,  // tell jQuery not to process the data
        contentType: false,
        error: function () {
            return true;
        },
        success: function (product) {
            console.log(product);
        }
    });

}

