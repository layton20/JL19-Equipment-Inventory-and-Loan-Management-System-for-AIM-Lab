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

    function postModalFormAjax(form) {
        $.ajax({
            method: form.attr('method'),
            url: form.attr('action'),
            data: form.serialize(),
            success: function (response) {
                if (response.success) {
                    window.location.href = encodeURI(`/Equipment/Equipment?SuccessMessage=${response.success}`);
                } else if (response.error) {
                    window.location.href = encodeURI(`/Equipment/Equipment?SuccessMessage=${response.error}`);
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

    $('#modalRoot').on("submit", "#formCreateEquipment", function (e) {
        e.preventDefault();
        var form = $(this);
        postModalFormAjax(form);
    });

    $('.deleteEquipment').click(function () {
        loadModalAjax($(this).data('url'), `EquipmentUID=${$(this).data('uid')}`);
    });
});