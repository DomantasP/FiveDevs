var app;

$(document).ready(function () {
    
    app = Sammy('#main', function (context) {
<<<<<<< HEAD

        this.get('#/', function () {
            changeTabWithSpinner(changeTabToProduct);
        });
        this.get('#/Products', function () {
            changeTabWithSpinner(changeTabToProduct);
        });
        this.get('#/Naudotojai', function () {
            changeTabWithSpinner(changeTabToUser);    
        });
        this.get('#/Naudotojai/Blokuoti', function () {
            changeTabToUser();
        });
        this.get('#/Naudotojai/Atblokuoti', function () {
            changeTabToUser();
        });
        this.get('#Nepatvirtinti', function () {
            loadAllTabData(0, "#purchasesTableBody", '#purchasesTable', '#Nepatvirtinti');
        });
        this.get('#Neissiusti', function () {
            loadAllTabData(1, "#purchasesTableBody2", '#purchasesTable2', '#Neissiusti');
        });
        this.get('#Nepristatyti', function () {
            loadAllTabData(2, "#purchasesTableBody3", '#purchasesTable3', '#Nepristatyti');
        });
        this.get('#Pristatyti', function () {
            loadAllTabData(3, "#purchasesTableBody4", '#purchasesTable4', '#Pristatyti');
        });
        
        
        
    })
    app.run('#/')
})

function changeTabWithSpinner(functionn) {
    setTimeout(function () {
        $('.html-spinner').show();
    }, 0);

    setTimeout(function () {
        functionn();
        $('.html-spinner').hide();
    }, 100);
}

function loadAllTabData(orderStatus, tableBodyId, tableId, statusName) {
    setTimeout(function () {
        $('.html-spinner').show();
    }, 0);

    setTimeout(function () {
        changeTabToSales();
        loadHorizontalTabsStyle();
        appendPurchasesTableBody(loadOrders(orderStatus), tableBodyId, tableId, statusName);
        $('.html-spinner').hide();
    }, 100);

}


function appendPurchasesTableBody(data, tableBodyId, tableId, href) {
    for (var i = 0; i < data.length; i++) {
        record = data[i];
        $(tableBodyId).append("<tr id = " + record.id + ">\
                    <td>"+ record.date + "</td>\
                    <td>"+ record.id + "</td>\
                    <td>"+ record.user + "</td>\
                    <td>"+ record.cost + "</td>\
                    <td>"+ "<a href="+ href +" class='moreInformation' id=" + record.id + " onclick='showOrderData(this)'><img src='../lib/Table/expand_black.png' alt='Daugiau informacijos' height='18' width='25' /></a>" + "</td>\
                    </tr>");
    }
    $(tableId).DataTable();
}

function loadOrders(orderStatus) {
    var declaredData = { status: orderStatus}
    var returnValue;
    $.ajax({
        url: '/Admin/GetOrders',
        type: 'POST',
        data: declaredData,
        async: false,
        success: function (data) {
            returnValue = data;
        }
    })
    
    return returnValue;
    
}

function changeTab(value) {       
        $.ajax({
            url: '/Admin/' + value,
            type: 'POST',
            dataType: 'html',
            async: false,
            success: function (data) {

                $('.user-dashboard').empty();
                $('.user-dashboard').html(data);
            }
        });
        
=======
        this.get('#/', function () {
            changeTabToProduct();
        });
        this.get('#/Products', function () {
            changeTabToProduct();

        });
        this.get('#/Naudotojai', function () {
            changeTabToUser();
        });
        this.get('#/Products/EditProduct', function () {

        });
        
        
    });
    app.run('#/')
})

function changeTab(value) {
    $.ajax({
        url: '/Admin/' + value,
        type: 'POST',
        dataType: 'html',
        async: false,
        success: function (data) {
            $('.user-dashboard').empty();
            $('.user-dashboard').html(data);
        }
    });
>>>>>>> 08d56f08d5bd2ddc594122d1b251dd1800fd13f8
}

function changeTabToUser() {
    changeSelectedTabColor('#usersTab');
    changeTab('UserManagementView');
<<<<<<< HEAD
    getUserModel();  
}

function changeTabToSales() {
    changeSelectedTabColor('#salesTab');
    changeTab('SalesView');
}

function getUserModel() {

=======
    getUserModel();
    
  
}

function getUserModel() {
>>>>>>> 08d56f08d5bd2ddc594122d1b251dd1800fd13f8
    $.ajax({
        url: '/Admin/AdminGetUserModel',
        type: 'POST',
        async: false,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                record = data[i];
<<<<<<< HEAD

                if (record.ban_flag == 0) {
                    $("#usersTableBody").append("<tr id = " + record.username + ">\
                    <td>"+ record.username + "</td>\
                    <td>"+ record.email + "</td>\
                    <td>"+ record.ban_flag + "</td>\
                    <td><a href='#/Naudotojai/Blokuoti' class='ban' id=" + record.username + " onclick='blockUser(this)'><img src='../lib/Table/images/block_red.png' alt='blokuoti' height='20' width='20' /></a></td>\
                    </tr>");
                } else {
                    $("#usersTableBody").append("<tr id = " + record.username + ">\
                    <td>"+ record.username + "</td>\
                    <td>"+ record.email + "</td>\
                    <td>"+ record.ban_flag + "</td>\
                    <td><a href='#/Naudotojai/Atblokuoti' class='ban' id=" + record.username + " onclick='unblockUser(this)'><img src='../lib/Table/images/unblock_green.png' alt='atblokuoti' height='30' width='30' /></a></td>\
                    </tr>");
                }         
=======
                $("#usersTableBody").append("<tr>\
                    <td>"+ record.username + "</td>\
                    <td>"+ record.email + "</td>\
                    <td>"+ record.ban_flag + "</td>\
                    <td>"+ record.ban_flag + "</td>\
                    </tr>");
>>>>>>> 08d56f08d5bd2ddc594122d1b251dd1800fd13f8
            }

        }
    });

<<<<<<< HEAD
=======

>>>>>>> 08d56f08d5bd2ddc594122d1b251dd1800fd13f8
    $('#usersTable').DataTable();

}

function changeTabToProduct() {
    changeSelectedTabColor('#productsTab');
    changeTab('AdminProductView');
<<<<<<< HEAD

=======
>>>>>>> 08d56f08d5bd2ddc594122d1b251dd1800fd13f8
    getProductModel();
    
}



function getProductModel() {
    $.ajax({
        url: '/Admin/AdminGetModel',
        type: 'POST',
        async: false,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                record = data[i];
                $("#productsTable").append("<tr>\
                    <td>"+record.id+"</td>\
                    <td>"+ record.sku_code +"</td>\
                    <td>"+ record.category_id +"</td>\
                    <td>"+ record.title +"</td>\
                    <td>"+ record.price +"</td>\
                    <td>"+ record.discount +"</td>\
<<<<<<< HEAD
                    <td> <a href='#/Products' class='edit' id="+ record.id +" onclick='editProdut(this)'><img src='../lib/Table/pencil_green.png' alt='redaguoti' height='30' width='30' /></a>\
=======
                    <td> <a href='#/Products/EditProduct' class='edit' id="+ record.id +" onclick='editProdut(this)'><img src='../lib/Table/pencil_green.png' alt='redaguoti' height='30' width='30' /></a>\
>>>>>>> 08d56f08d5bd2ddc594122d1b251dd1800fd13f8
                         <a href='#' class='delete' id="+ record.id +"><img src='../lib/Table/cross_red.png' alt='ištrinti' height='30' width='30' /></a></td>\
                    </tr>");
            }
            
        }
    });

    
    $('#example').DataTable();
    
}
   

function changeSelectedTabColor(id) {
    $('li.active').attr('class', 'inactive');
    $(id).attr('class', 'active');
}

    


