$(document).ready(function () {
    /**
     *
     * VARS & SETUP
     *
     **/
    var tableView = "table";
    var tableToggleThreshold = 974;
    if ($(window).outerWidth() <= tableToggleThreshold) {
        tableView = "card";
        $(".object-table").bootstrapTable("toggleView");
    }
    winResize();
    $(window).on('resize', winResize);

    $('[data-toggle="tooltip"]').tooltip();
    //bug fix for tooltips when state of table changes
    $(".object-table").on("all.bs.table", function () {
        $('[data-toggle="tooltip"]').tooltip();
    });

    /**
     *
     * GLOBAL FUNCTIONS
     *
     **/
    //4 using
    function randomString() {
        return Math.random().toString(36).slice(2);
    }
    function createAlert(type, text) {
        var idAlert = "alert-" + randomString();

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
    var accordionItem = {};
    accordionItem.createSection = function () {
        $("<div/>", {
            id: this.sectionId,
            class: "acc-items-section"
        }).appendTo("." + this.className);
    }
    accordionItem.createTitle = function () {
        //accordion title
        var html = '<a class="acc-items-section-title" id="' + this.sectionTitle + '" href="#' + this.id + '">' + this.val.name.trim();
        html += '<button type="button" class="close" aria-label="Close"><span aria-hidden="true">×</span></button>';
        html += '</a>';
        $("#" + this.sectionId).append(html);
    }
    accordionItem.createSectionContent = function (className) {
        $("<div/>", {
            id: accordionItem.id,
            class: "acc-items-section-content"
        }).appendTo("#" + accordionItem.sectionId);
    }
    accordionItem.createInputs = function () {
        var html = '<div class="full-width">';
        html += addInput("item-quantity-" + this.val.id.trim(), this.val.quantity.trim(), "Quantity", "number", "quantity", "col-xs-12 col-sm-6 col-md-3", this.val.unitofmeasure.trim());

        html += addInput("item-price-" + this.val.id.trim(), "", "Price", "number", "price", "col-xs-12 col-sm-6 col-md-3");
        html += addInput("item-shipdate-" + this.val.id.trim(), "", "Shipping Date", "date", "shipdate", "col-xs-12 col-sm-6 col-md-3");
        html += '<input id="item-id-' + this.val.id.trim() + '" type="hidden" class="id" value="' + this.val.id.trim() + '">';
        html += '</div>';

        html += addTextBox("item-description-" + this.val.id.trim(), this.val.description.trim(), "Description", "description", "col-xs-12 col-sm-6 col-md-4");
        html += addTextBox("item-qualitiy-" + this.val.id.trim(), "", "Quality", "quality", "col-xs-12 col-sm-6 col-md-4");
        html += addTextBox("item-comment-" + this.val.id.trim(), "", "Comment", "comment", "col-xs-12 col-sm-6 col-md-4");

        $("#" + accordionItem.id).append(html);
    }
    accordionItem.setup = function () {
        this.createSection();
        this.createTitle();
        this.createSectionContent();
        this.createInputs();
        this.eventsSetup();
    }
    accordionItem.closeSection = function () {
        $('.' + this.className + ' .acc-items-section-title').removeClass('active');
        $('.' + this.className + ' .acc-items-section-content').slideUp(300).removeClass('open');

    }
    accordionItem.eventsSetup = function () {
        var parent = this;
        var sectionTitleClick = function (e) {
            // Grab current anchor value
            var currentAttrValue = $(this).attr('href');
            if ($(e.target).is('.active')) {
                parent.closeSection();
            } else {
                parent.closeSection();
                // Add active class to section title
                $(this).addClass('active');
                // Open up the hidden content panel
                $('.' + parent.className + " " + currentAttrValue).slideDown(300).addClass('open').css("display", "inline-block");
            }

            e.preventDefault();
        }


        $('#' + parent.sectionTitle).click(sectionTitleClick);

        $(parent.modalId).modal('hide');

        $(".acc-items-section-title .close").each(function (index, el) {
            $(el).click(function (e) {
                e.stopPropagation();
                $(this).parent().parent().remove();
            });
        });

        //cleanup
        $("#project-unselected-items-table").bootstrapTable('uncheckAll');
    }

    var table = {
        $: $("#project-unselected-items-table"),
        addNewItemsBtn: $("#add-new-items-btn"),
        confirmItemsAddBtn: $('#modal-add-items-confirm'),
        submitBtn: $("#submit-project")
    };
    table.setup = function () {
        if (table.$.length > 0) {
            this.eventSetup();
            return true;
        }
        return false;
    }
    table.eventSetup = function () {
        console.log("event setup");
        var parent = this;
        
        var addItems = function (button) {
            var data = parent.$.bootstrapTable('getSelections');
            $.each(data, function (index, val) {
                accordionItem.sectionId = "acc-items-section-" + val.id.trim() + "-" + randomString();
                accordionItem.sectionTitle = 'acc-items-section-title-' + val.id.trim() + randomString();
                accordionItem.id = "acc-items-" + val.id.trim() + "-" + randomString();
                accordionItem.className = "acc-items";
                accordionItem.modalId = '#modalUnselectedItems';
                accordionItem.val = val;
                accordionItem.setup();
            });

        }
        var submitAllData = function () {
            //1. Get project basic info
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
                        $.ajax(
                            {
                                url: "/ProjectItem/CreateViaAjax",
                                type: "POST",
                                data: items[i]
                            }).done(function (data) {
                                console.log("item:" + data);
                            });
                    }
                }

            }).error(function () {
                console.log("nema veze");
                errorRaised = true;
                errorMsg = "Došlo je pogreške prilikom pohranjivanja projekta. Provjerite podatke i pokušajte ponovno!";
            })
        }


        //FIX for chboxes
        var selectedItems = [];
        this.$.on('check.bs.table', function (e, row) {
            if (selectedItems.indexOf($("#" + row._id).data("index")) === -1) {
                selectedItems.push($("#" + row._id).data("index"));
            }
        });
        this.$.on('uncheck.bs.table', function (e, row) {
            selectedItems.splice(selectedItems.indexOf($("#" + row._id).data("index")), 1);
        });
        this.$.on('page-change.bs.table', function (e, rows) {
            $.each(selectedItems, function (index, el) {
                if (el !== null) {
                    table.$.bootstrapTable('check', el);
                }
            });
        });

        this.addNewItemsBtn.click(function () {
            selectedItems = [];
        });
        this.confirmItemsAddBtn.click(addItems);
        this.submitBtn.click(submitAllData);
    }
    //MAIN START
    table.setup();
    // MAIN END
    
    //to be deleted
    function close_accordion_section() {
        $('.acc-items .acc-items-section-title').removeClass('active');
        $('.acc-items .acc-items-section-content').slideUp(300).removeClass('open');
    }


    /**
     *
     * ITEMS CART
     *
     **/
   
    var cart = {
        $: $(".moderator-sidebar #cart"),
        body: $("#cart .body")
    }
    cart.setup = function (modalObj) {
        if (modalObj.length == 0 || this.$.length == 0) {
            return false;
        }
        this.modal = {
            $: modalObj
        }
        this.modal.title = this.modal.$.find(".modal-title");
        this.modal.quantity = this.modal.$.find(".item-quantity");
        this.modal.unitOfMeasure = this.modal.$.find(".item-unitofmeasure");
        this.modal.dismissBtn = this.modal.$.find("#dismiss-btn");
        this.modal.addBtn = this.modal.$.find("#add-to-cart-btn");
        this.modal.clean = function () {
            this.title.html("");
            this.quantity.html("");
            this.unitOfMeasure.html("");

            this.dismissBtn.unbind("click");
            this.addBtn.unbind("click");
        }

        this.form = {
            quantity: this.modal.$.find("#supply-item-quantity"),
            price: this.modal.$.find("#supply-item-price"),
            date: this.modal.$.find("#supply-item-date"),
            quality: this.modal.$.find("#supply-item-quality"),
            comment: this.modal.$.find("#supply-item-comment")
        }
        this.form._setProps = function (formClass, input) {
            if (!$(formClass).hasClass("hidden")) {
                $(formClass).addClass("hidden");
                $(input).prop("required", false);
            }
        }
        this.form.clean = function () {
            this.quality.val("");
            this.quantity.val("");
            this.price.val("");
            this.date.val("");
            this.comment.val("");

            this._setProps(".form-quantity", "#supply-item-quantity");
            this._setProps(".form-comment", "#supply-item-comment");
            this._setProps(".form-quality", "#supply-item-quality");
            this._setProps(".form-date", "#supply-item-date");
        }
        var parent = this;
        this.clean = function () {
            parent.modal.clean();
            parent.form.clean();
        }
        this.modal.$.on("hidden.bs.modal", this.clean);
        return true;
    } 
    cart.linkSetup = function () {
        var parent = this;
        var setupByCriterias = function (that) {
            //show only defined inputs - CRITERIAS
            $(that).parent().parent().children(".item-criterias").children("span").each(function (index, el) {
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
        }
        $(".shopping-cart-link").each(function (index, el) {
            $(el).click(function (e) {
                //MODAL setup
                e.preventDefault();
                var row = $(this).parent().parent(); //table row
                var product = {
                    id: row.children(".item-id").html().trim(),
                    name: row.children(".item-name").html().trim(),
                    quantityMax: row.children(".item-value").children(".item-quantity").html().trim(),
                    unitofmeasure: row.children(".item-value").children(".item-unitofmeasure").html().trim()
                }

                parent.modal.title.html(product.name);
                parent.modal.quantity.html(product.quantityMax);
                parent.form.quantity.attr("max", product.quantityMax);
               
                parent.modal.unitOfMeasure.html(product.unitofmeasure);

                setupByCriterias(this);

                //VALIDATION SETUP
                $("#add-to-cart-form").unbind("submit").bind("submit", function () {
                    //1. get data (update product)
                    product.tempId = randomString();
                    product.quality = parent.form.quality.val();
                    product.quantity = parent.form.quantity.val();
                    product.price = parent.form.price.val();
                    product.date = parent.form.date.val();
                    product.comment = parent.form.comment.val();

                    //2. add to session
                    var sessionCart = JSON.parse(sessionStorage.getItem("cart"));
                    if (sessionCart == null) {
                        sessionCart = [];
                    }
                    sessionCart.push(product);
                    sessionStorage.setItem("cart", JSON.stringify(sessionCart));

                    //3. update cart
                    parent.update();
                    parent.modal.$.modal('hide');
                    parent.clean();
                    return false;
                });
            });
        });
    }
    cart.update = function() {
        if (this.$.length > 0) {
            //sessionStorage.clear();
            sessionCart = sessionStorage.getItem("cart");
            if (sessionCart != null) {
                sessionCart = JSON.parse(sessionCart);
                var parent = this;
                $.each(sessionCart, function (index, el) {
                    if (parent.body.find("#" + el.tempId).length == 0) {
                        $("<tr/>", {
                            id: el.tempId,
                            class: "cart-item",
                            role: "cart-item",
                            html: "<td class='name'>" + el.name + "</td><td class='price'>" + el.price + "</td>"
                        }).appendTo(parent.body);
                    }
                });
            }
        }
    }

    cart.setup($("#modalItemToCart"));
    cart.linkSetup();
    cart.update();
    $("#products-table").on("post-body.bs.table", function () {
        cart.setup($("#modalItemToCart"));
        cart.linkSetup();
    });
    //session storage listener
    $(window).bind('storage', cart.update);


    /**
     *
     * ITEM OFFER CONFIRMATION
     *
     **/
    if ($(".supply-header-items-create").length > 0) {
        if (sessionStorage.length > 0) {
            cart = JSON.parse(sessionStorage.getItem("cart"));
            $.each(cart, function (index, el) {

                var rand = randomString();
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

                            //used for redirect
                            var itemFlags = [];
                            $(".acc-items .acc-items-section").each(function (i, el) {
                                itemFlags.push(i);
                            });
                            
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
                                    itemFlags.pop();
                                    if (itemFlags.length == 0) {
                                        console.log("done");
                                        sessionStorage.clear();
                                        window.location.href = "/supplyHeader/Details/" + projectID;
                                    }
                                });
                            });

                        }
                    }).error(function () {
                        console.log("nema veze");
                        errorRaised = true;
                        errorMsg = "Došlo je pogreške prilikom pohranjivanja projekta. Provjerite podatke i pokušajte ponovno!";
                    });
            } //function saveData
        }
    }

    /**
     *
     * PROJECT OFFER CONFIRMATION
     *
     **/
    if ($(".supply-header-project-create").length > 0) {
        $(".acc-items-section-title").each(function (index, el) {
            $(el).click(function (e) {
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
        });
    }



    /**
     *
     * SUPPLY HEADER EDIT
     *
     **/
    accSupplyEditSetup = function() {
        var sectionTitleClick = function (e) {
            var currentAttrValue = $(this).attr('href');
            if ($(e.target).is('.active')) {
                accCloseSection();
            } else {
                accCloseSection();
                $(this).addClass('active');
                $('.acc-items' + " " + currentAttrValue).slideDown(300).addClass('open').css("display", "inline-block");
            }
            e.preventDefault();
        }

        $(".acc-items-section-title").each(function (index, el) {
            $(el).click(sectionTitleClick);
        });


        //cleanup
        //$("#project-unselected-items-table").bootstrapTable('uncheckAll');
    }
    accCloseSection = function () {
        $('.acc-items .acc-items-section-title').removeClass('active');
        $('.acc-items .acc-items-section-content').slideUp(300).removeClass('open');

    }

    //paymentMethod,deliveryMethod,paymentDate,deliveryDate,supplier,date,project,status,currency
    function editSupplyData(statusType) {
        var projectID = $("#supplyID").val();
        //1. set offer status
        var status = 1;
        if (statusType == "send") {
            status = 2;
        }
        //2. set data
        var reqToken = $('input[name="__RequestVerificationToken"]').val();
        var supplyHeader = {
            supplyID: projectID,
            paymentMethod: $("#paymentMethod").val(),
            deliveryMethod: $("#deliveryMethod").val(),
            paymentDate: $("#paymentDate").val(),
            deliveryDate: $("#deliveryDate").val(),
            date: $("#date").val(),
            status: status,
            currency: $("#currency").val(),
            __RequestVerificationToken: reqToken
        }
        //save supplyHeader
        //potential break: ignores ajax url name and calls default edit method
        $.ajax({
            url: "UpdateViaAjax",
            type: "POST",
            data: supplyHeader
        }).done(function (data) {
            if (data == "ERROR") {
                console.log("error");
                errorRaised = true;
                errorMsg = "Došlo je pogreške prilikom pohranjivanja ponude. Provjerite podatke i pokušajte ponovno!";
            } else {
                console.log("projectID: " + data);
                //Saving supply items

                //delete removed items
                $.each(deletedItems, function (i, el) {
                    $.ajax({
                        url: "/SupplyItem/DeleteViaAjax",
                        type: "POST",
                        data: { id: deletedItems[i], __RequestVerificationToken: reqToken }
                    }).done(function (data) {
                        console.log("deleted: " + deletedItems[i]);
                    });
                });

                //used for redirect
                var itemFlags = [];
                $(".acc-items .acc-items-section").each(function (i, el) {
                    itemFlags.push(i);
                });

                // supply, item, quantity, price, quality, comment, shipDate
                $(".acc-items .acc-items-section").each(function (index, el) {
                    var supplyItem = {
                        supply: projectID,
                        __RequestVerificationToken: reqToken
                    };
                    $(el).find("input").each(function (index, subel) {
                        if ($(subel).hasClass("supplyItemId")) {
                            supplyItem.supplyItemID = $(subel).val();
                        } else if ($(subel).hasClass("quantity")) {
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
                        console.log($(subel));
                        if ($(subel).hasClass("quality")) {
                            supplyItem.quality = $(subel).val();
                        } else if ($(subel).hasClass("comment")) {
                            supplyItem.comment = $(subel).val();
                        }
                    });
                    console.log(supplyItem);
                    $.ajax({
                        url: "/SupplyItem/EditViaAjax",
                        type: "POST",
                        data: supplyItem
                    }).done(function (data) {
                        console.log(data);
                        itemFlags.pop();
                        if (itemFlags.length == 0) {
                            console.log("done");
                            window.location.href = "/supplyHeader/Details/" + projectID;
                        }
                    });
                });

            }
        }).error(function () {
            console.log("nema veze");
            errorRaised = true;
            errorMsg = "Došlo je pogreške prilikom pohranjivanja projekta. Provjerite podatke i pokušajte ponovno!";
        });
    } //function saveData

    if ($(".supply-header-edit").length > 0) {
        var deletedItems = [];
        accSupplyEditSetup();
        $("#save-btn").click(function () {
            editSupplyData("save");
        });
        $("#send-btn").click(function () {
            editSupplyData("send");
        });

        $(".acc-items-section-title .close").each(function (index, el) {
            $(el).click(function (e) {
                e.stopPropagation();
                var id = $(this).parent().parent().find(".supplyItemId").val();
                if (id != undefined) {
                    deletedItems.push(id);
                }
                $(this).parent().parent().remove();
            });
        });
    }

});