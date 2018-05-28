function editProdut(value) {
    setTimeout(function () {
        $('.html-spinner').show();
    }, 0);

    setTimeout(function () {
        doEditProduct(value);
        $('.html-spinner').hide();
    }, 100);
}

function loadActionData(actionMethod) {
    var returnValue;
    $.ajax({
        url: '/Admin/' + actionMethod,
        type: 'POST',
        async: false,
        success: function (data) {
            returnValue = data;
        }
    })
    return returnValue;
}


function doEditProduct(value) {
    $.ajax({
        url: '/Admin/AdminEditProductView',
        dataType: 'html',
        async: false,
        success: function (data) {
            $('.fillingForm').html(data);
            categoriesData = loadActionData("GetSubcategoriesInBatches");
            appendCategoriesData(categoriesData)
        }
    });


    function appendCategoriesData(categoriesData) {
        var count = 1;
        //console.log(categoriesData);
        for (var i = 0; i < categoriesData.length; i++) {
            var categoryBatch = categoriesData[i];
            var parentId = categoryBatch.parentID;
            var parentTitle = categoryBatch.parentTitle;
            $('.categoriesList').append("\
                        <li class = 'categoryItem' data-value=" + count + " data-level='1' data-default-selected='active'>\
                             <a href = '#' id = 'c"+ parentId + "'><b>" + parentTitle + "</b></a>\
                        </li >");
            count = count + 1;
            subCategoriesIds = categoryBatch.subCatIds;
            subCategoriesTitles = categoryBatch.subCatTitles;
            for (var j = 0; j < subCategoriesIds.length; j++) {
                subcategoryId = subCategoriesIds[j];
                subCategoriesTitle = subCategoriesTitles[j];
                $('.categoriesList').append("\
                        <li class = 'categoryItem' data-value=" + (count) + " data-level='2' data-default-selected=''>\
                             <a href = '#' id = 'c"+ subcategoryId + "'>" + subCategoriesTitle + "</a>\
                        </li>");
                count = count + 1;
            }
        }
                $('.categoriesList').append("\
                        <li class = 'categoryItem' id='addNewCategory' data-value=" + (count) + " data-level='1' data-default-selected=''>\
                             <a href = '#' id = 'addNewCategory'><b><i>Pridėti naują kategoriją</i></b></a>\
                        </li>");
                $('.categoriesList').append("\
                        <li class = 'categoryItem' id='addNewSubCategory' data-value=" + (count+1) + " data-level='1' data-default-selected=''>\
                             <a href = '#' id = 'addNewSubCategory'><b><i>Pridėti naują subkategoriją</i></b></a>\
                        </li>");
    }

    $(document).ready(function () {

        $("#addNewCategory").click(function () {
            $(".categoryHidden").show();
        });
        $(".categoryItem:not(#addNewCategory)").click(function () {
            $(".categoryHidden").hide();
        });
        $("#addNewSubCategory").click(function () {
            count = 1;
            var categoriesData = loadActionData("GetRootCategories");
            console.log(categoriesData);
            for (var i = 0; i < categoriesData.length; i++) {
                rootCategory = categoriesData[i];
                $('.rootCategoriesList').append("\
                        <li class = 'rootCategoryItem' data-value=" + count + " data-level='1' data-default-selected=''>\
                             <a href = '#' id = '"+ rootCategory.id + "'><b>" + rootCategory.title + "</b></a>\
                        </li>");
                count = count + 1;
            }
            $('.rootCategoriesList').append("\
                        <li class = 'rootCategoryItem' id='addSubcategorysCategory' data-value=" + count + " data-level='1' data-default-selected=''>\
                             <a href = '#' id = 'addSubcategorysCategory'><b><i>Pridėti naują kategoriją.</i></b></a>\
                        </li>");

            $(".subCategories_categories").show();
            $(".subcategoryHidden").show();
        });
        $(".categoryItem:not(#addNewSubCategory)").click(function () {
            $(".subCategories_categories").hide();
            $(".subcategoryHidden").hide();
            $(".subcategoryCategoryHidden").hide();
            $(".rootCategoryItem").remove();
        });
    });

    $(document).ready(function () {
        $('body').on('click', '#addSubcategorysCategory', function () {
            $(".subcategoryCategoryHidden").show();
        });
        $('body').on('click', '.rootCategoryItem:not(#addSubcategorysCategory)', function () {
            $(".subcategoryCategoryHidden").hide();
        });
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
            //$('input#Category').attr('value', product.category);
            $('#c' + product.categoryid).click();
            //console.log($('#c' + product.categoryid).parent());
            $('input#Title').attr('value', product.title);
            $('input#Price').attr('value', product.price);
            $('textarea#Description').text(product.description);
            //$('textarea#Description').attr('value', product.description); //  'input#Description'
            $('input#Discount').attr('value', product.discount);
        }
    });
    $('.html-spinner').hide();
}




function submitForm() {

    id = $('input#Id').val();
    sku_code = $('input#Sku_code').val();
    title = $('input#Title').val();
    price = $('input#Price').val();
    description = $('textarea#Description').val(); // input#Description .. val()
    discount = $('input#Discount').val();

    var newCategory;
    var newSubcategory;
    if ($("#subCategory_new").is(":visible") && $("#subCategory_newCategory").is(":visible")) {
        newCategory = $("#subCategory_newCategory").val();
        newSubcategory = $("#subCategory_new").val();
    } else if ($("#subCategory_new").is(":visible") && !$("#subCategory_newCategory").is(":visible")) {
        newCategory = $(".subCategories_categories span.selected-label").text();
        newSubcategory = $("#subCategory_new").val();
    } else if ($("#Category_new").is(":visible")) {
        newCategory = $("#Category_new").val();
    } else if (!$("#Category_new").is(":visible")) {
        newCategory = $(".categoriesAll span.selected-label").text();
    }

    var formData = new FormData();

    var fileSelect = document.getElementById('imageUpload');
    if (fileSelect != null) {
        var files = fileSelect.files;

        for (var i = 0; i < files.length; i++) {
            formData.append('files', files[i]);
        }
    }
  

    //data.append('file0', file);
    formData.append('idS', id);
    formData.append('sku_code', sku_code);
    formData.append('categoryS', newCategory);
    formData.append('title', title);
    formData.append('priceS', price);
    formData.append('description', description); 
    formData.append('discountS', discount);
    formData.append('subCategoryS', newSubcategory);


    product = formData;
    /*var product = {
        idS: id, sku_code: sku_code, categoryS: newCategory, title: title,
        priceS: price, description: description, discountS: discount, subCategoryS: newSubcategory
    }*/

    $.ajax({
        url: '/Admin/AdminUpdateProduct',
        type: 'POST',
        async: false,
        cache: false,
        timeout: 30000,
        data: product,
        contentType: false,
        processData: false,
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
