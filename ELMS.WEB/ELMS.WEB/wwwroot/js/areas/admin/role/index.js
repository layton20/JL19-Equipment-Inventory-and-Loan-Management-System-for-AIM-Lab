$(document).ready(function () {
    function loadModalAjax(url, queryString) {
        if (queryString) {
            url += `?${queryString}`;
        }

        $.get(url, function (data) {
            if (data.message) {
                window.location.href = encodeURI(`/Admin/Role?ErrorMessage=${data.message}`);
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
                    window.location.href = encodeURI(`/Admin/Role?successMessage=${response.success}`);
                } else if (response.error) {
                    window.location.href = encodeURI(`/Admin/Role?errorMessage=${response.error}`);
                }
                else {
                    $('#modalDialog').html(response);
                }
            }
        })
    }

    $('.createRole').click(function () {
        loadModalAjax($(this).data('url'), null);
    });

    $('.editRole').click(function () {
        loadModalAjax($(this).data('url'), `uid=${$(this).data('uid')}`);
    });

    $('#modalRoot').on("submit", ".form", function (e) {
        e.preventDefault();
        var form = $(this);
        postModalFormAjax(form, `/Admin/Role/Index`);
    });
});