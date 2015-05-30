﻿$(document).ready(function () {
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
    //bug fix for tooltips when state of table changes
    $(".object-table").on("all.bs.table", function () {
        $('[data-toggle="tooltip"]').tooltip();
    });

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
    function addInput(id, value, label, type, className, wrapperClass, addition, attrs) {
        if (attrs == null || attrs == undefined) {
            attrs = "";
        }
        var cont = '<div class="' + wrapperClass + '">';
        cont += '<label for="' + id + '">' + label + ':</label>';
        cont += '<input id="' + id + '" type="' + type + '" class="form-control text-box ' + className + '" value="' + value + '" ' + attrs + '  />';
        if (className == 'quantity') {
            cont += '<span>' + addition + '</span>';
        }
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

        //FIX for chboxes
        var selectedItems = [];
        $table.on('check.bs.table', function (e, row) {
            if (selectedItems.indexOf($("#" + row._id).data("index")) === -1) {
                selectedItems.push($("#" + row._id).data("index"));
            }
        });
        $table.on('uncheck.bs.table', function (e, row) {
            selectedItems.splice(selectedItems.indexOf($("#" + row._id).data("index")), 1);
        });
        $table.on('page-change.bs.table', function (e, rows) {
            $.each(selectedItems, function (index, el) {
                if (el !== null) {
                    $table.bootstrapTable('check', el);
                }
            });
        });

        $("#add-new-items-btn").click(function () {
            selectedItems = [];
        });
        $button = $('#modal-add-items-confirm');

        $button.click(function () {
            var data = $table.bootstrapTable('getSelections');
            $.each(data, function (index, val) {
                var rand = Math.floor(Math.random() * 1000);
                var accItemsSectionId = "acc-items-section-" + val.id.trim() + "-" + rand;
                var accItemsSectionTitle = 'acc-items-section-title-' + val.id.trim() + rand;
                var accItemsId = "acc-items-" + val.id.trim() + "-" + rand;
                $("<div/>", { //accordion section
                    id: accItemsSectionId,
                    class: "acc-items-section"
                }).appendTo(".acc-items");

                //accordion title
                var html = '<a class="acc-items-section-title" id="' + accItemsSectionTitle + '" href="#' + accItemsId + '">' + val.name.trim();
                html += '<button type="button" class="close" aria-label="Close"><span aria-hidden="true">×</span></button>';
                html += '</a>';
                $("#" + accItemsSectionId).append(html);

                $("<div/>", {
                    id: accItemsId,
                    class: "acc-items-section-content"
                }).appendTo("#" + accItemsSectionId);

                html = '<div class="full-width">';
                html += addInput("item-quantity-" + val.id.trim(), val.quantity.trim(), "Quantity", "number", "quantity", "col-xs-12 col-sm-6 col-md-3", val.unitofmeasure.trim());

                html += addInput("item-price-" + val.id.trim(), "", "Price", "number", "price", "col-xs-12 col-sm-6 col-md-3");
                html += addInput("item-shipdate-" + val.id.trim(), "", "Shipping Date", "date", "shipdate", "col-xs-12 col-sm-6 col-md-3");
                html += '<input id="item-id-' + val.id.trim() + '" type="hidden" class="id" value="' + val.id.trim() +'">';
                html += '</div>';

                html += addTextBox("item-description-" + val.id.trim(), val.description.trim(), "Description", "description", "col-xs-12 col-sm-6 col-md-4");
                html += addTextBox("item-qualitiy-" + val.id.trim(), "", "Quality", "quality", "col-xs-12 col-sm-6 col-md-4");
                html += addTextBox("item-comment-" + val.id.trim(), "", "Comment", "comment", "col-xs-12 col-sm-6 col-md-4");

                $("#" + accItemsId).append(html);

                $('#' + accItemsSectionTitle).click(function (e) {
                    // Grab current anchor value
                    
                    var currentAttrValue = $(this).attr('href');
                    console.log(currentAttrValue);
                    if ($(e.target).is('.active')) {
                        close_accordion_section();
                    } else {
                        close_accordion_section();

                        // Add active class to section title
                        $(this).addClass('active');
                        // Open up the hidden content panel
                        $('.acc-items ' + currentAttrValue).slideDown(300).addClass('open').css("display", "inline-block");
                    }

                    e.preventDefault();
                });

                $('#modalUnselectedItems').modal('hide');
            });

            $(".acc-items-section-title .close").each(function (index, el) {
                $(el).click(function (e) {
                    e.stopPropagation();
                    $(this).parent().parent().remove();
                });
            });

            //cleanup
            $("#project-unselected-items-table").bootstrapTable('uncheckAll');

        });

        function close_accordion_section() {
            $('.acc-items .acc-items-section-title').removeClass('active');
            $('.acc-items .acc-items-section-content').slideUp(300).removeClass('open');
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
            $(".acc-items .acc-items-section").each(function (index, el) {
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

    /**
     *
     * ITEMS CART
     *
     **/
    var modalCart = $("#modalItemToCart");
    var form = {
        quantity: modalCart.find("#supply-item-quantity"),
        price: modalCart.find("#supply-item-price"),
        date: modalCart.find("#supply-item-date"),
        quality: modalCart.find("#supply-item-quality"),
        comment: modalCart.find("#supply-item-comment")
    }
    function cartSetup() {
        $(".shopping-cart-link").each(function (index, el) {
            $(el).click(function (e) {
                console.log("cart click");
                //MODAL setup
                e.preventDefault();
                var row = $(this).parent().parent();
                var product = {
                    id: row.children(".item-id").html().trim(),
                    name: row.children(".item-name").html().trim(),
                    quantityMax: row.children(".item-value").children(".item-quantity").html().trim(),
                    unitofmeasure: row.children(".item-value").children(".item-unitofmeasure").html().trim()
                }

                //show only defined inputs - CRITERIAS
                $(this).parent().parent().children(".item-criterias").children("span").each(function (index, el) {
                    if ($(el).html() == "kolicina") {
                        $(".form-quantity").removeClass("hidden");
                        $("#supply-item-quantity").prop("required", true);
                    } else if ($(el).html() == "napomena") {
                        $(".form-comment").removeClass("hidden");
                        $("#supply-item-comment").prop("required", true);
                    } else if ($(el).html() == "kvaliteta sukladna specifikaciji") {
                        $(".form-quality").removeClass("hidden");
                        $("#supply-item-quality").prop("required", true);
                    } else if ($(el).html() == "rok isporuke") {
                        $(".form-date").removeClass("hidden");
                        $("#supply-item-date").prop("required", true);
                    }
                });

                var modalElements = {
                    title: modalCart.find(".modal-title"),
                    quantity: modalCart.find(".item-quantity"),
                    unitOfMeasure: modalCart.find(".item-unitofmeasure"),
                    dismissBtn: modalCart.find("#dismiss-btn"),
                    addBtn: modalCart.find("#add-to-cart-btn")
                };
                function clean() {
                    modalElements.title.html("");
                    modalElements.quantity.html("");
                    modalElements.unitOfMeasure.html("");
                    
                    form.quality.val("");
                    form.quantity.val("");
                    form.price.val("");
                    form.date.val("");
                    form.comment.val("");

                    if (!$(".form-quantity").hasClass("hidden")) {
                        $(".form-quantity").addClass("hidden");
                        $("#supply-item-quantity").prop("required", false);
                    }
                    if (!$(".form-comment").hasClass("hidden")) {
                        $(".form-comment").addClass("hidden");
                        $("#supply-item-comment").prop("required", false);
                    }
                    if (!$(".form-quality").hasClass("hidden")) {
                        $(".form-quality").addClass("hidden");
                        $("#supply-item-quality").prop("required", false);
                    }
                    if (!$(".form-date").hasClass("hidden")) {
                        $(".form-date").addClass("hidden");
                        $("#supply-item-date").prop("required", false);
                    }

                    modalElements.dismissBtn.unbind("click");
                    modalElements.addBtn.unbind("click");
                }
                modalElements.title.html(product.name);
                modalElements.quantity.html(product.quantityMax);
                form.quantity.attr("max", product.quantityMax);
                modalElements.unitOfMeasure.html(product.unitofmeasure);

                //modalElements.dismissBtn.click(clean);
                //$(".modal-dialog .close").click(clean);
                $("#modalItemToCart").on("hidden.bs.modal", clean);


                //VALIDATION SETUP
                $("#add-to-cart-form").unbind("submit").bind("submit", function () {
                    
                    //1. get data (update product)
                    product.tempId = Math.random().toString(36).slice(2);
                    product.quality = form.quality.val();
                    product.quantity = form.quantity.val();
                    product.price = form.price.val();
                    product.date = form.date.val();
                    product.comment = form.comment.val();

                    //2. add to session
                    var cart = JSON.parse(sessionStorage.getItem("cart"));
                    if (cart == null) {
                        cart = [];
                    }
                    console.log("cart push");
                    cart.push(product);
                    //sessionStorage.removeItem("cart");
                    sessionStorage.setItem("cart", JSON.stringify(cart));
                    //3. update cart
                    cartUpdate();
                    modalCart.modal('hide');
                    clean();
                    return false;
                });
            });
        });
    }
    cartSetup();
    cartUpdate();
    $("#products-table").on("post-body.bs.table", function (e, rows) {
        cartSetup();
    });

    //session storage listener
    $(window).bind('storage', function (e) {
        cartUpdate();
        //console.log(e.originalEvent.key, e.originalEvent.newValue);
    });
    function cartUpdate() {
        if($(".moderator-sidebar #cart").length > 0) {
            //sessionStorage.clear();
            cart = sessionStorage.getItem("cart");
            if (cart != null) {
                cart = JSON.parse(cart);
                console.log(cart);
                var cartBody = $("#cart .body");
                $.each(cart, function (index, el) {
                    if (cartBody.find("#" + el.tempId).length == 0) {
                        $("<tr/>", {
                            id: el.tempId,
                            class: "cart-item",
                            role: "cart-item",
                            html: "<td class='name'>" + el.name + "</td><td class='price'>" + el.price + "</td>"
                        }).appendTo(cartBody);
                    }
                });
            }
        }
    }

    /**
     *
     * ITEM PURCHASE CONFIRMATION
     *
     **/
    if ($(".supply-header-create").length > 0) {
        if (sessionStorage.length > 0) {
            cart = JSON.parse(sessionStorage.getItem("cart"));
            $.each(cart, function (index, el) {

                var rand = Math.floor(Math.random() * 1000);
                var accItemsSectionId = "acc-items-section-" + el.tempId;
                var accItemsSectionTitle = 'acc-items-section-title-' + el.tempId;
                var accItemsId = "acc-items-" + el.tempId;
                $("<div/>", { //accordion section
                    id: accItemsSectionId,
                    class: "acc-items-section"
                }).appendTo(".acc-items");

                //accordion title
                var html = '<a class="acc-items-section-title" id="' + accItemsSectionTitle + '" href="#' + accItemsId + '">' + el.name;
                html += '<button type="button" class="close" aria-label="Close"><span aria-hidden="true">×</span></button>';
                html += '</a>';
                $("#" + accItemsSectionId).append(html);

                $("<div/>", {
                    id: accItemsId,
                    class: "acc-items-section-content"
                }).appendTo("#" + accItemsSectionId);
                
                html = '<div class="full-width">';
                if (el.quantity != "") {
                    html += addInput("item-quantity-" + el.id, el.quantity, "Quantity", "number", "quantity", "col-xs-12 col-sm-6 col-md-3", el.unitofmeasure, 'max="' + el.quantityMax + '"');
                }
                if (el.price != "") {
                    html += addInput("item-price-" + el.id, el.price, "Price", "number", "price", "col-xs-12 col-sm-6 col-md-3");
                }
                if (el.date != "") {
                    html += addInput("item-shipdate-" + el.id, el.date, "Shipping Date", "date", "shipdate", "col-xs-12 col-sm-6 col-md-3");
                }
                html += '<input id="item-id-' + el.id + '" type="hidden" class="id" value="' + el.id + '">';
                html += '</div>';

                if (el.quality != "") {
                    html += addTextBox("item-qualitiy-" + el.id, el.quality, "Quality", "quality", "col-xs-12 col-sm-6 col-md-4");
                }
                if (el.comment != "") {
                    html += addTextBox("item-comment-" + el.id, el.comment, "Comment", "comment", "col-xs-12 col-sm-6 col-md-4");
                }
                $("#" + accItemsId).append(html);
                
                $('#' + accItemsSectionTitle).click(function (e) {
                    // Grab current anchor value

                    var currentAttrValue = $(this).attr('href');
                    if ($(e.target).is('.active')) {
                        close_accordion_section();
                    } else {
                        close_accordion_section();

                        // Add active class to section title
                        $(this).addClass('active');
                        // Open up the hidden content panel
                        $('.acc-items ' + currentAttrValue).slideDown(300).addClass('open').css("display", "inline-block");
                    }

                    e.preventDefault();
                });

                $(".acc-items-section-title .close").each(function (index, el) {
                    $(el).click(function (e) {
                        e.stopPropagation();
                        $(this).parent().parent().remove();
                    });
                });
                
            });

            $("#save-btn").click(function () {
                saveData("save");
            });
            $("#send-btn").click(function () {
                saveData("send");
            });

            //paymentMethod,deliveryMethod,paymentDate,deliveryDate,supplier,date,project,status,currency
            function saveData(statusType) {
                var projectID;
                //1. set offer status
                var status = 1;
                if (statusType == "send") {
                    status = 2;
                }
                //2. set data
                var reqToken = $('input[name="__RequestVerificationToken"]').val();
                var supplyHeader = {
                    paymentMethod: $("#paymentMethod").val(),
                    deliveryMethod: $("#deliveryMethod").val(),
                    paymentDate: $("#paymentDate").val(),
                    deliveryDate: $("#deliveryDate").val(),
                    date: $("#date").val(),
                    status: status,
                    currency: $("#currency").val(),
                    __RequestVerificationToken: reqToken
                }
                console.log(supplyHeader);
                //save supplyHeader
                $.ajax({
                        url: "CreateViaAjax",
                        type: "POST",
                        data: supplyHeader
                    }).done(function (data) {
                        if (data == "ERROR") {
                            console.log("error");
                            errorRaised = true;
                            errorMsg = "Došlo je pogreške prilikom pohranjivanja ponude. Provjerite podatke i pokušajte ponovno!";
                        } else {
                            projectID = data;
                            console.log("projectID: " + projectID);
                            //Saving supply items

                            // supply, item, quantity, price, quality, comment, shipDate
                            $(".acc-items .acc-items-section").each(function (index, el) {
                                var supplyItem = {
                                    supply: projectID,
                                    __RequestVerificationToken: reqToken
                                };
                                $(el).find("input").each(function (index, subel) {
                                    if ($(subel).hasClass("quantity")) {
                                        supplyItem.quantity = $(subel).val();
                                    } else if ($(subel).hasClass("price")) {
                                        supplyItem.price = $(subel).val();
                                    } else if ($(subel).hasClass("shipdate")) {
                                        supplyItem.shipDate = $(subel).val();
                                    } else if ($(subel).hasClass("id")) {
                                        supplyItem.item = $(subel).val();
                                    }
                                });
                                $(el).find("textarea").each(function (index, subel) {
                                    if ($(subel).hasClass("quality")) {
                                        supplyItem.quality = $(subel).html();
                                    } else if ($(subel).hasClass("comment")) {
                                        supplyItem.comment = $(subel).html();
                                    }
                                });
                                
                                $.ajax({
                                    url: "/SupplyItem/CreateViaAjax",
                                    type: "POST",
                                    data: supplyItem
                                }).done(function (data) {
                                    console.log("SupplyItem: " + data);
                                });
                            });
                            sessionStorage.clear();
                            window.location.href = "/supplyHeader/Details/" + projectID;


                        }
                    }).error(function () {
                        console.log("nema veze");
                        errorRaised = true;
                        errorMsg = "Došlo je pogreške prilikom pohranjivanja projekta. Provjerite podatke i pokušajte ponovno!";
                    });
            } //function saveData
        }
    }
});