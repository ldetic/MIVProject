$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip();

    /**
     *
     * FUNCTIONS
     *
     **/
    function queryParams() {
        console.log("query");
        return {
            type: 'owner',
            sort: 'updated',
            direction: 'desc',
            per_page: 1,
            page: 1
        };
    }
    function createAlert(type, text) {
        var idAlert = "alert-" + Math.floor(Math.random() * 100000);

        $("<div/>", {
            id: idAlert,
            class: "alert alert-" + type + " alert-dismissible",
            role: "alert",
        }).appendTo(".alerts");

        $("#" + idAlert).append('<button type="button" class="close" data-dismiss="alert" aria-label="Close"><span aria-hidden="true">&times;</span></button>');
        $("#" + idAlert).append(text);
    }

    /**
     * 
     * PROJECT TABLE 
     *
     **/
    $('#projects-table').on('click', 'tr', function (event) {
        console.log('test');

    });

    /**
     * 
     * PROJECT MODAL 
     *
     **/
    $('#modalDelete').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var objectName = button.data('object-name');
        var objectId = button.data('object-id');
        var objectType = button.data('object-type');
        var objectToken = $('input[name="__RequestVerificationToken"]').val();
        var modal = $(this);
        modal.find('.modal-object-name').text(objectName);
        modal.find('.modal-title').text(objectName);

        //DEBUG
        $("#projectTable").bootstrapTable("hideRow", 1);

        modal.find('#modal-delete-confirm').click(function (e) {
            e.preventDefault();
            $('#modalDelete').modal('hide');
            var url = "";
            if (objectType === "project") {
                url = "/Project/DeleteViaAjax/";
            } else if (objectType === "item") {
                url = "/Item/DeleteViaAjax/";
            }
            url += objectId;

            $.ajax({
                url: url,
                type: "POST",
                data: { __RequestVerificationToken: objectToken }
            }).done(function (data) {
                if (data === "OK") {
                    $("#object-row-" + objectId).toggle(400, function () {
                        createAlert("success", "<strong>" + objectName + "</strong> uspješno obrisan!");
                    });
                } else {
                    createAlert("danger", "Došlo je do pogreške, <strong>" + objectName + "</strong> nije obrisan!");
                }
            }).error(function () {
                createAlert("danger", "Došlo je do pogreške, <strong>" + objectName + "</strong> nije obrisan!");
            });
        });
    });

    $('#modalDelete').on('hide.bs.modal', function (event) {
        $(this).find('#modal-delete-confirm').unbind("click");
    });

});
