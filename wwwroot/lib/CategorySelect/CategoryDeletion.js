function deleteCategory() {
    setTimeout(function () {
        $('.html-spinner').show();
    }, 0);

    setTimeout(function () {
        doDeleteCategory();
        $('.html-spinner').hide();
    }, 100);
}

function doDeleteCategory() {
    aaa = $(function () {
        $.confirm.show({
            "message": "Ar tikrai norite ištrinti pasirinktą kategoriją? Kartu bus ištrintos ir jos subkategorijos",
            "type": "danger",
            "passFunction": deleteCategoryMethod,
            "passParam": $(".selected-label").text(),
            "yes": function () {
                $.confirm.show({
                    "message": "Welcome!",
                    "noText": "Cancel",
                    "type": "success",
                })
            }
        })

    })
}

function deleteCategoryMethod(categoryName) {
    setTimeout(function () {
        $('.html-spinner').show();
    }, 0);

    setTimeout(function () {
        doDeleteCategoryMethod(categoryName);
        $('.html-spinner').hide();
    }, 100);
}

function doDeleteCategoryMethod(categoryName) {
    var categoryData = { categoryName: categoryName };

    $.ajax({
        url: '/Category/DeleteCategory',
        type: 'POST',
        async: false,
        cache: false,
        timeout: 30000,
        data: categoryData,
        error: function () {
            notify({
                type: "error", //alert | success | error | warning | info
                title: "Nesėkmė",
                position: {
                    x: "right", //right | left | center
                    y: "top" //top | bottom | center
                },
                icon: '<img src="/./lib/Messages/images/paper_plane.png" alt = "nesėkmė"/>',
                message: "Kategorija nebuvo pašalinta. Patikrinkite ar tinkamai pasirinkote kategoriją"
            });
            return true;
        },
        success: function (product) {
            $("#CategoryDeletionBlock").hide();
            $(".deletionCategoriesList").empty();
            $(".selected-label").text("");
            notify({
                type: "success", //alert | success | error | warning | info
                title: "Sėkmė",
                position: {
                    x: "right", //right | left | center
                    y: "top" //top | bottom | center
                },
                icon: '<img src="/./lib/Messages/images/paper_plane.png" alt = "sėkmė"/>',
                message: "Kategorija ir jos subkategorijos buvo sėkmingai pašalintos"
            });
        }
    });
    $('.notify').fadeOut(4000, function () { $('.notify').remove(); });

}



function appendCategories(categoriesData) {
    count = 1;
    for (var i = 0; i < categoriesData.length; i++) {
        var categoryBatch = categoriesData[i];
        var parentId = categoryBatch.parentID;
        var parentTitle = categoryBatch.parentTitle;
        $('.deletionCategoriesList').append("\
                        <li class = 'categoryItem' data-value=" + count + " data-level='1' data-default-selected=''>\
                             <a href = #/ id = 'c"+ parentId + "'><b>" + parentTitle + "</b></a>\
                        </li >");
        count = count + 1;
        subCategoriesIds = categoryBatch.subCatIds;
        subCategoriesTitles = categoryBatch.subCatTitles;
        for (var j = 0; j < subCategoriesIds.length; j++) {
            subcategoryId = subCategoriesIds[j];
            subCategoriesTitle = subCategoriesTitles[j];
            $('.deletionCategoriesList').append("\
                        <li class = 'categoryItem' data-value=" + (count) + " data-level='2' data-default-selected=''>\
                             <a href = #/ id = 'c"+ subcategoryId + "'>" + subCategoriesTitle + "</a>\
                        </li>");
            count = count + 1;
        }
    }

}