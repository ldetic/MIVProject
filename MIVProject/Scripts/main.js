$(document).ready(function () {
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
     * PROJECT MODAL DELETE
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
                var html = '<a class="accordion-section-title" id="accordion-section-title-' + val.id.trim() + '" href="#accordion-' + val.id.trim() + '">' + val.name.trim() + '</a>';
                $("#accordion-section-" + val.id.trim()).append(html);

                $("<div/>", {
                    id: "accordion-" + val.id.trim(),
                    class: "accordion-section-content"
                }).appendTo("#accordion-section-" + val.id.trim());

                html = '<div class="full-width">';

                html += addInput("item-quantity-" + val.id.trim(), val.quantity.trim(), "Quantity", "number", "quantity", "col-xs-12 col-sm-6 col-md-3");
                html += addInput("item-unitofmeasure-" + val.id.trim(), val.unitofmeasure.trim(), "Unit of measure", "text", "unitofmeasure", "col-xs-12 col-sm-6 col-md-3");
                html += addInput("item-price-" + val.id.trim(), "", "Price", "number", "price", "col-xs-12 col-sm-6 col-md-3");
                html += addInput("item-shipdate-" + val.id.trim(), "", "Shipping Date", "date", "shipdate", "col-xs-12 col-sm-6 col-md-3");
                html += '<input id="item-id-' + val.id.trim() + '" type="hidden" class="id" value="' + val.id.trim() +'">';
                html += '</div>';

                html += addTextBox("item-description-" + val.id.trim(), val.description.trim(), "Description", "description", "col-xs-12 col-sm-6 col-md-4");
                html += addTextBox("item-qualitiy-" + val.id.trim(), "", "Quality", "quality", "col-xs-12 col-sm-6 col-md-4");
                html += addTextBox("item-comment-" + val.id.trim(), "", "Comment", "comment", "col-xs-12 col-sm-6 col-md-4");

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

        function addInput(id, value, label, type, className, wrapperClass) {
            var cont = '<div class="' + wrapperClass + '">';
            cont += '<label for="' + id + '">' + label + ':</label>';
            cont += '<input id="' + id + '" type="' + type + '" class="form-control text-box ' + className +'" value="' + value + '"  />';
            cont += '</div>';
            return cont;
        }
        function addTextBox(id, value, label, className, wrapperClass) {
            var cont = '<div class="' + wrapperClass + '">';
            cont += '<label for="' + id + '">' + label + '</label>';
            cont += '<br/>';
            cont += '<textarea id="' + id + '" class="col-xs-12 ' + className + '">' + value + '</textarea>';
            cont += '</div>';
            return cont;
        }
        function close_accordion_section() {
            $('.accordion .accordion-section-title').removeClass('active');
            $('.accordion .accordion-section-content').slideUp(300).removeClass('open');
        }

        $("#submit-project").click(function () {
            //1. Getting project basic info
            var reqToken = $("input[name='__RequestVerificationToken']").val();
            var project = {
                __RequestVerificationToken: reqToken,
                name: $("#name").val(),
                paymentMethod: $("#paymentMethod").val(),
                paymentDate: $("#paymentDate").val(),
                deliveryMethod: $("#deliveryMethod").val(),
                deliveryDate: $("#deliveryDate").val(),
                description: $("#description").val()
            };
            console.log(project);
            //2. Getting project items
            var items = [];
            $(".accordion .accordion-section").each(function (index, el) {
                items.push({
                    item: $(el).find(".id").val(),
                    quantity: $(el).find(".quantity").val(),
                    price: $(el).find(".price").val(),
                    shipDate: $(el).find(".shipdate").val(),
                    description: $(el).find(".description").val(),
                    quality: $(el).find(".quality").val(),
                    comment: $(el).find(".comment").val()
                });
            });

            //3. Saving project and retrieving projectID
            var projectID;
            var errorRaised = false;
            var errorMsg = "";
            $.ajax({
                url: "CreateViaAjax",
                type: "POST",
                data: project
            }).done(function (data) {
                console.log(data);
                if (data == "ERROR") {
                    console.log("error");
                    errorRaised = true;
                    errorMsg = "Došlo je pogreške prilikom pohranjivanja projekta. Provjerite podatke i pokušajte ponovno!";
                } else {
                    projectID = data;
                    //4. Saving project items
                    for (var i = 0; i < items.length; i++) {
                        items[i].project = projectID;
                        items[i].__RequestVerificationToken = reqToken;
                        console.log(items[i]);
                        $.ajax({
                            url: "/ProjectItem/CreateViaAjax",
                            type: "POST",
                            data: items[i]
                        }).done(function (data) {
                            console.log(data);
                        });
                    }
                }
                
            }).error(function () {
                console.log("nema veze");
                errorRaised = true;
                errorMsg = "Došlo je pogreške prilikom pohranjivanja projekta. Provjerite podatke i pokušajte ponovno!";
            });

        });
    }
});

