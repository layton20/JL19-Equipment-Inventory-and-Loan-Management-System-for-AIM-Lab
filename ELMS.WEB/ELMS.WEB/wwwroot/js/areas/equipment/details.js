$(document).ready(function () {
    function loadModalAjax(url, queryString) {
        if (queryString) {
            url += `?${queryString}`;
        }

        $.get(url, function (data) {
            if (data.message) {
                window.location.href = encodeURI(`/Equipment/Equipment?ErrorMessage=${data.message}`);
            } else {
                $('#modalDialog').html(data);
                $('#modalRoot').modal('show');

                if ($('#modalRoot').is(':visible')) {
                    var form = jQuery('form', $modal).first();
                    jQuery.validator.unobtrusive.parse(form);
                }
            }
        });
    };

    function postModalFormAjax(form, returnUrl = "") {
        if (returnUrl == "") {
            returnUrl = "/Equipment/Equipment?";
        }

        $.ajax({
            method: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize(),
            success: function (response) {
                if (response.success) {
                    window.location.href = encodeURI(`${returnUrl}&successMessage=${response.success}`);
                } else if (response.error) {
                    window.location.href = encodeURI(`${returnUrl}&errorMessage=${response.error}`);
                }
                else {
                    $('#modalDialog').html(response);
                }
            }
        })
    }

    $('.create').click(function () {
        loadModalAjax($(this).data('url'), null);
    });

    $('.createNote').click(function () {
        loadModalAjax($(this).data('url'), `EquipmentUID=${$(this).data('equipmentuid')}`);
    });

    $('.editNote').click(function () {
        loadModalAjax($(this).data('url'), `NoteUID=${$(this).data('uid')}`);
    });

    $('#modalRoot').on("submit", ".formCreate", function (e) {
        e.preventDefault();
        var form = $(this);
        var equipmentUid = $(this).data('equipmentuid');
        postModalFormAjax(form, `/Equipment/Equipment/DetailsView?EquipmentUID=${equipmentUid}`);
    });

    $('#modalRoot').on("submit", ".formNote", function (e) {
        e.preventDefault();
        var form = $(this);
        var equipmentUid = $(this).data('equipmentuid');
        postModalFormAjax(form, `/Equipment/Equipment/DetailsView?EquipmentUID=${equipmentUid}`);
    });

    $('.deleteEquipment').click(function () {
        loadModalAjax($(this).data('url'), `EquipmentUID=${$(this).data('uid')}`);
    });

    $('.deleteNote').click(function () {
        loadModalAjax($(this).data('url'), `NoteUID=${$(this).data('uid')}`);
    });
});