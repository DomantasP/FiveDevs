function blockUser(tag) {
    aaa = $(function () {
         $.confirm.show({
            "message": "Ar tikrai norite užblokuoti naudotoją?",
             "type": "danger",
             "passFunction": banUser,
             "passParam": tag.id,
            "yes": function () {
                //H-confirm-alert.js
                $.confirm.show({
                    "message": "Welcome!",
                    "noText": "Cancel",
                    "type": "success",
                })
            }
        })
        
    })
}

function banUser(usernamee) {

    var userData = { username: usernamee }

    $.ajax({
        url: '/Admin/AdminBanUser',
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
                message: "Naudotojas nebuvo užblokuotas"
            });
        },
        success: function (user) {
            notify({
                type: "success", //alert | success | error | warning | info
                title: "Sėkmė",
                position: {
                    x: "right", //right | left | center
                    y: "top" //top | bottom | center
                },
                icon: '<img src="/./lib/Messages/images/paper_plane.png" alt = "sėkmė"/>',
                message: "Naudotojas buvo sėkmingai užblokuotas."
            });

        }
    });
    $('.notify').fadeOut(4000, function () { $('.notify').remove(); });
    return 1;
}

function unblockUser(tag) {
    aaa = $(function () {
        $.confirm.show({
            "message": "Ar tikrai norite atblokuoti naudotoją?",
            "type": "danger",
            "passFunction": unbanFunction,
            "passParam": tag.id,
            "yes": function () {
                //H-confirm-alert.js
                $.confirm.show({
                    "message": "Welcome!",
                    "noText": "Cancel",
                    "type": "success",
                })
            }
        })

    })
}


function unbanFunction(usernamee) {

    var userData = { username: usernamee }

    $.ajax({
        url: '/Admin/AdminUnbanUser',
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
                message: "Naudotojas nebuvo atblokuotas"
            });
        },
        success: function (user) {
            notify({
                type: "success", //alert | success | error | warning | info
                title: "Sėkmė",
                position: {
                    x: "right", //right | left | center
                    y: "top" //top | bottom | center
                },
                icon: '<img src="/./lib/Messages/images/paper_plane.png" alt = "sėkmė"/>',
                message: "Naudotojas buvo sėkmingai atblokuotas."
            });

        }
    });
    $('.notify').fadeOut(4000, function () { $('.notify').remove(); });
    return 1;
}