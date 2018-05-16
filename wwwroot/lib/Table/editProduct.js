function editProdut(value) {
    $.ajax({
        url: '/Admin/AdminEditProductView',
        dataType: 'html',
        async: false,
        success: function (data) {
            $('.fillingForm').html(data);
        }
    });


    $("html, body").animate({ scrollTop: 500 }, 2000);

    var productId = { id: $(value).attr('id') };

    
        $.ajax({
            url: '/Admin/AdminGetProductById',
            type: 'POST',
            async: false,
            cache: false,
            timeout: 30000,
            data: productId,
            error: function () {
                return true;
            },
            success: function (product) {
                $('input#Id').attr('value', product.id);            
                $('input#Sku_code').attr('value', product.sku_code);
                $('input#Category').attr('value', product.category_id);
                $('input#Title').attr('value', product.title);
                $('input#Price').attr('value', product.price);
                $('input#Description').attr('value', product.description);
                $('input#Discount').attr('value', product.discount);
            }
        });
}

function submitForm() {

    id = $('input#Id').val();
    sku_code = $('input#Sku_code').val();
    category_id = $('input#Category').val();
    title = $('input#Title').val();
    price = $('input#Price').val();
    description = $('input#Description').val();
    discount = $('input#Discount').val();
    

    var product = {
        idS: id, sku_code: sku_code, category_idS: category_id, title: title,
        priceS: price, description: description, discountS: discount
    }

    $.ajax({
        url: '/Admin/AdminUpdateProduct',
        type: 'POST',
        async: false,
        cache: false,
        timeout: 30000,
        data: product,
        error: function (error) {
            notify({
                type: "error", //alert | success | error | warning | info
                title: "Nesėkmė",
                position: {
                    x: "right", //right | left | center
                    y: "top" //top | bottom | center
                },
                icon: '<img src="/./lib/Messages/images/paper_plane.png" alt = "sėkmė"/>',
                message: "Prekės duomenys nebuvo redaguoti. Patikrinkite duomenis"
            });
        },
        success: function (product) {
            $("html, body").animate({ scrollTop: -500 }, 2000);

            $('.editForm').remove();
            notify({
                type: "success", //alert | success | error | warning | info
                title: "Sėkmė",
                position: {
                    x: "right", //right | left | center
                    y: "top" //top | bottom | center
                },
                icon: '<img src="/./lib/Messages/images/paper_plane.png" alt = "sėkmė"/>',
                message: "Prekės duomenys sėkmingai redaguoti."
            });
        }
    });
    $('.notify').fadeOut(4000, function () { $('.notify').remove(); });

}
