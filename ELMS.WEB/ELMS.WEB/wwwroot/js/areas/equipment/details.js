$(document).ready(function () {
    $('.btn-edit-submit').hide();
    $('.formDetails :input').prop("disabled", true);

    $('.btn-edit-lock').click(function () {
        if ($('.formDetails :input').prop("disabled")) {
            $('.formDetails :input').prop("disabled", false);
            $(this).html('<i class="fas fa-lock mr-2"></i> Lock Edit Mode');
            $('.btn-edit-submit').show();
        }
        else {
            $('.formDetails :input').prop("disabled", true);
            $(this).html('<i class="fas fa-lock-open mr-2"></i> Unlock Edit Mode');
            $('.btn-edit-submit').hide();
        }
    });

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
        loadModalAjax($(this).data('url'), `uid=${$(this).data('uid')}`);
    });

    $('.editNote').click(function () {
        loadModalAjax($(this).data('url'), `NoteUID=${$(this).data('uid')}`);
    });

    $('#modalRoot').on("submit", ".formCreate", function (e) {
        e.preventDefault();
        var form = $(this);
        var equipmentUid = $(this).data('uid');
        postModalFormAjax(form, `/Equipment/Equipment/DetailsView?uid=${equipmentUid}`);
    });

    $('#modalRoot').on("submit", ".formNote", function (e) {
        e.preventDefault();
        var form = $(this);
        var equipmentUid = $(this).data('uid');
        postModalFormAjax(form, `/Equipment/Equipment/DetailsView?uid=${equipmentUid}`);
    });

    $('.deleteEquipment').click(function () {
        loadModalAjax($(this).data('url'), `uid=${$(this).data('uid')}`);
    });

    $('.deleteNote').click(function () {
        loadModalAjax($(this).data('url'), `NoteUID=${$(this).data('uid')}`);
    });

    $('.deleteImage').click(function () {
        loadModalAjax($(this).data('url'), `uid=${$(this).data('uid')}`);
    });
});