function showOrderData(data) {
    setTimeout(function () {
        $('.html-spinner').show();
    }, 0);

    setTimeout(function () {
        calcOrderData(data);
    }, 100);
    
}

function calcOrderData(data) {

    $("#productsTableBody").empty();

    orderData = loadOrder(data.id);
    $('#orderMoreInformation').find("*").show();
    if (orderData.status == 3 && orderData.comment != "") {
        console.log(orderData);
        $('#orderCommentSection').find("*").show();
        $('#orderComment').empty();
        $('#orderComment').append("<span id = buyerComment>" + orderData.comment + "</span>");

    } else $('#orderCommentSection').find("*").hide();

    $('.orderDate').text(orderData.date);
    $('.orderId').text(orderData.id);
    $('.orderUsername').text(orderData.user);
    $('.orderAdress').text(orderData.address);
    $('.orderValue').text(orderData.cost + " €");
    if (orderData.stars >= 1 && orderData.stars <= 5) {
        $('.orderStars').text(orderData.stars + "/5");
    } else $('.orderStars').text("Nėra");

    $('.btn-primary').attr('id', data.id);
    switch (orderData.status) {
        case 0:
            $('.btn-primary').val('Patvirtinti, jog siunta ruošiama');
            break;
        case 1:
            $('.btn-primary').val('Patvirtinti, jog siunta išsiųsta');
            break;
        case 2:
            $('.btn-primary').val('Patvirtinti, jog siunta pristatyta');
            break;
        case 3:
            $('.btn-primary').remove();
            break;
    }
    productsData = loadProducts(data.id);
    appendOrderItemsBody(productsData);
    $('.orderTable').DataTable();
    $('.html-spinner').hide();

}


function appendOrderItemsBody(data) {
    for (var i = 0; i < data.length; i++) {
        record = data[i];
        $("#productsTableBody").append("<tr id = " + record.id + ">\
                    <td>"+ record.sku_code + "</td>\
                    <td>"+ record.category + "</td>\
                    <td>"+ record.title + "</td>\
                    <td>"+ record.price + "</td>\
                    <td>"+ record.quantity + "</td>\
                    </tr>");
    }
    $('#productsTable').DataTable();
}

function loadOrder(orderId) {

    var declaredData = { Id: orderId }
    var returnValue;
    $.ajax({
        url: '/Admin/GetOrder',
        type: 'POST',
        data: declaredData,
        async: false,
        success: function (data) {
            returnValue = data;
        }
    })
    return returnValue;
}

function loadProducts(orderId) {

    var declaredData = { Id: orderId }
    var returnValue;
    $.ajax({
        url: '/Admin/GetOrderItems',
        type: 'POST',
        data: declaredData,
        async: false,
        success: function (data) {
            returnValue = data;
        }
    })
    return returnValue;
}

// data.id - order id to be updated to 'confirmed'
function confirmOrder(data) {
    aaa = $(function () {
        $.confirm.show({
            "message": "Ar tikrai norite patvirtinti naują siuntos būseną?",
            "type": "danger",
            "passFunction": changeOrderStatus, //function
            "passParam": data.id, //function's parameter
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


//parameter - orderId
function changeOrderStatus(orderId) {

    var userData = { orderId: orderId }

    $.ajax({
        url: '/Admin/ConfirmOrder',
        type: 'POST',
        async: false,
        cache: false,
        timeout: 30000,
        data: userData,
        error: function (error) {

            notify({
                type: "error", //alert | success | error | warning | info
                title: "Nesėkmė",
                position: {
                    x: "right", //right | left | center
                    y: "top" //top | bottom | center
                },
                icon: '<img src="/./lib/Messages/images/paper_plane.png" alt = "nesėkmė"/>',
                message: "Siuntos būsena nebuvo pakeista"
            });
        },
        success: function (someData) {
            notify({
                type: "success", //alert | success | error | warning | info
                title: "Sėkmė",
                position: {
                    x: "right", //right | left | center
                    y: "top" //top | bottom | center
                },
                icon: '<img src="/./lib/Messages/images/paper_plane.png" alt = "sėkmė"/>',
                message: "Siuntos būsena buvo sėkmingai atnaujinta"
            });
            
            $('.orderTable').DataTable().row("#" + someData).remove().draw();
            $('#orderMoreInformation').hide();

        }
    });
    $('.notify').fadeOut(4000, function () { $('.notify').remove(); });
    return 1;
}