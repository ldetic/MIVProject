$(document).ready(function(){
    /**
     *
     * VARS & SETUP
     *
     **/
    var tableView = "table";
    var tableToggleThreshold = 974;
    if ($(window).outerWidth() <= tableToggleThreshold) {
        console.log("below threshold");
        tableView = "card";
        $(".object-table").bootstrapTable("toggleView");
    }

    $('[data-toggle="tooltip"]').tooltip();

    /**
     *
     * FUNCTIONS
     *
     **/
    
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
    function winResize() {
        var win = $(window);
        if (win.outerWidth() <= tableToggleThreshold && tableView === "table") {
            tableView = "card";
            $(".object-table").bootstrapTable("toggleView");
        }
        if (win.outerWidth() > tableToggleThreshold && tableView === "card") {
            tableView = "table";
            $(".object-table").bootstrapTable("toggleView");
        }
    }

    /**
     * 
     * PROJECT TABLE 
     *
     **/
    //$('#projects-table').on('click', 'tr', function (event) {});
    
    winResize();
    $(window).on('resize', winResize);
    

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
        var ids = $.map($(".object-table").bootstrapTable('getSelections'), function (row) {           
            return row.id;
        });
        
        var rowId = button.parent().parent().children().first().html();
        //bug: doesn't work! find out why!
        $(".object-table").bootstrapTable("remove", { field: 'id', value: [rowId] });

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

    /**
     *
     * PROJECT CREATE
     *
     **/
    
    var $table = $("#project-unselected-items-table");
    if ($table.length > 0) {      
        $button = $('#modal-add-items-confirm');
        
            $button.click(function () {
                var data = $table.bootstrapTable('getSelections');
                $.each(data, function (index, val) {

                    $("<div/>", { //accordion section
                        id: "accordion-section-" + val.id.trim(),
                        class: "accordion-section"
                    }).appendTo(".accordion");

                    //accordion title
                    var html = '<a class="accordion-section-title" id="accordion-section-title-' + val.id.trim() +'" href="#accordion-' + val.id.trim() + '">' + val.name.trim() + '</a>';
                    $("#accordion-section-" + val.id.trim()).append(html);

                    $("<div/>", {
                        id: "accordion-" + val.id.trim(),
                        class: "accordion-section-content"
                    }).appendTo("#accordion-section-" + val.id.trim());

                    html = '<div class="full-width">';

                    html += addInput("item-quantity-" + val.id.trim(), val.quantity.trim(), "Quantity", "number", "col-xs-12 col-sm-6 col-md-3");
                    html += addInput("item-unitofmeasure-" + val.id.trim(), val.unitofmeasure.trim(), "Unit of measure", "text", "col-xs-12 col-sm-6 col-md-3");
                    html += addInput("item-price-" + val.id.trim(), "", "Price", "number", "col-xs-12 col-sm-6 col-md-3");
                    html += addInput("item-shipdate-" + val.id.trim(), "", "Shipping Date", "date", "col-xs-12 col-sm-6 col-md-3");

                    html += '</div>';
                    
                    html += addTextBox("item-description-" + val.id.trim(), val.description.trim(), "Description", "col-xs-12 col-sm-6 col-md-4");
                    html += addTextBox("item-qualitiy-" + val.id.trim(), "", "Quality", "col-xs-12 col-sm-6 col-md-4");
                    html += addTextBox("item-comment-" + val.id.trim(), "", "Comment", "col-xs-12 col-sm-6 col-md-4");

                    $("#accordion-" + val.id.trim() + "").append(html);


                    $('#accordion-section-title-' + val.id.trim()).click(function (e) {
                        // Grab current anchor value
                        var currentAttrValue = $(this).attr('href');

                        if ($(e.target).is('.active')) {
                            close_accordion_section();
                        } else {
                            close_accordion_section();

                            // Add active class to section title
                            $(this).addClass('active');
                            // Open up the hidden content panel
                            $('.accordion ' + currentAttrValue).slideDown(300).addClass('open').css("display", "inline-block");
                        }

                        e.preventDefault();
                    });

                    $('#modalUnselectedItems').modal('hide');
                });
                
            });
        
            function addInput(id, value, label, type, wrapperClass) {
                var cont = '<div class="' + wrapperClass + '">';
                cont += '<label for="' + id + '">' + label + ':</label>';
                cont += '<input id="' + id + '" type="' + type + '" class="form-control text-box" value="' + value + '"  />';
                cont += '</div>';
                return cont;
            }
            function addTextBox(id, value, label, wrapperClass) {
                var cont = '<div class="' + wrapperClass + '">';
                cont += '<label for="' + id + '">' + label + '</label>';
                cont += '<br/>';
                cont += '<textarea id="' + id + '" class="col-xs-12">' + value + '</textarea>';
                cont += '</div>';
                return cont;
            }
        function close_accordion_section() {
            $('.accordion .accordion-section-title').removeClass('active');
            $('.accordion .accordion-section-content').slideUp(300).removeClass('open');
        }
    }
});

