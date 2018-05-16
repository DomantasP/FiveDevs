var app;

$(document).ready(function () {
    
    app = Sammy('#main', function (context) {
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
}

function changeTabToUser() {
    changeSelectedTabColor('#usersTab');
    changeTab('UserManagementView');
    getUserModel();
    
  
}

function getUserModel() {
    $.ajax({
        url: '/Admin/AdminGetUserModel',
        type: 'POST',
        async: false,
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                record = data[i];
                $("#usersTableBody").append("<tr>\
                    <td>"+ record.username + "</td>\
                    <td>"+ record.email + "</td>\
                    <td>"+ record.ban_flag + "</td>\
                    <td>"+ record.ban_flag + "</td>\
                    </tr>");
            }

        }
    });


    $('#usersTable').DataTable();

}

function changeTabToProduct() {
    changeSelectedTabColor('#productsTab');
    changeTab('AdminProductView');
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
                    <td> <a href='#/Products/EditProduct' class='edit' id="+ record.id +" onclick='editProdut(this)'><img src='../lib/Table/pencil_green.png' alt='redaguoti' height='30' width='30' /></a>\
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

    


