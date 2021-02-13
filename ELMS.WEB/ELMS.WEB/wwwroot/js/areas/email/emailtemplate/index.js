$(document).ready(function () {
    function loadModalAjax(url, queryString) {
        if (queryString) {
            url += `?${queryString}`;
        }

        $.get(url, function (data) {
            if (data.message) {
                window.location.href = encodeURI(`/Email/EmailTemplate/Index?ErrorMessage=${data.message}`);
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
                    window.location.href = encodeURI(`/Email/EmailTemplate/Index?SuccessMessage=${response.success}`);
                } else if (response.error) {
                    window.location.href = encodeURI(`/Email/EmailTemplate/Index?SuccessMessage=${response.error}`);
                }
                else {
                    $('#modalDialog').html(response);
                }
            }
        })
    }

    $('.create').click(function () {
        console.log('hello')
        loadModalAjax($(this).data('url'), null);

        var observer = new MutationObserver(function (mutationRecords) {
            console.log("change detected");
            setTimeout(function () {
                tinymce.init({
                    selector: '.tinymce',
                    menubar: false,
                });
            }, 500);
        });

        observer.observe($('#modalRoot').get(0), { childList: true });
    });

    $('#modalRoot').on("submit", ".formCreate", function (e) {
        e.preventDefault();
        var form = $(this);
        postModalFormAjax(form);
    });

    $('.editEmailTemplate').click(function () {
        loadModalAjax($(this).data('url'), `uid=${$(this).data('uid')}`);
    });

    $('.deleteEmailTemplate').click(function () {
        loadModalAjax($(this).data('url'), `uid=${$(this).data('uid')}`);
    });

    $('#modalRoot').on("submit", "#formDeleteEmailTemplate", function (e) {
        e.preventDefault();
        var form = $(this);
        postModalFormAjax(form);
    });
});