$(document).ready(function(){
    $('[data-toggle="tooltip"]').tooltip();

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

    //project modal settings
    $('#modalDelete').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var projectName = button.data('project-name');
        var projectId = button.data('project-id');
        var modal = $(this);
        modal.find('.modal-project-name').text(projectName);
        modal.find('.modal-title').text(projectName);
        var deleteLink = modal.find('.modal-project-delete-link').attr('href', '/Project/Delete/' + projectId);

        modal.find('#modal-delete-confirm').click(function (e) {
            e.preventDefault();
            $('#modalDelete').modal('hide');
            
            $.ajax({
                url: "/Project/DeleteAjax/" + projectId,
                type: "POST",
            }).done(function (data) {
                if (data === "OK") {
                    $("#project-row-" + projectId).toggle(400, function () {
                        createAlert("success", "Projekt <strong>" + projectName + "</strong> uspješno obrisan!");
                    });
                } else {
                    createAlert("danger", "Došlo je do pogreške, Projekt <strong>" + projectName + "</strong> nije obrisan!");
                }
            }).error(function () {
                createAlert("danger", "Došlo je do pogreške, Projekt <strong>" + projectName + "</strong> nije obrisan!");
            });
        });
    });

    $('#modalDelete').on('hide.bs.modal', function (event) {
        $(this).find('#modal-delete-confirm').unbind("click");
    });

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

});
