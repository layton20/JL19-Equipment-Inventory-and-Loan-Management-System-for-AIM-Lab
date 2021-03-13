$(document).ready(function () {
    function loadModalAjax(url, queryString) {
        if (queryString) {
            url += `?${queryString}`;
        }

        $.get(url, function (data) {
            if (data.message) {
                window.location.href = encodeURI(`/Loan/Loan/DetailsView?ErrorMessage=${data.message}`);
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
            returnUrl = "/Loan/Loan?";
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

    $('.equipmentDetails').click(function () {
        loadModalAjax($(this).data('url'), `uid=${$(this).data('uid')}`);
    });

    $('.createLoanExtension').click(function () {
        loadModalAjax($(this).data('url'), `loanUID=${$(this).data('uid')}`);
    });

    $('#modalRoot').on("submit", "#formCreate", function (e) {
        e.preventDefault();
        var form = $(this);
        var equipmentUid = $(this).data('loanuid');
        postModalFormAjax(form, `/Loan/Loan/DetailsView?uid=${equipmentUid}`);
    });

    $('#modalRoot').on("submit", "#formCreateExtension", function (e) {
        e.preventDefault();
        var form = $(this);
        var loanUID = $("#LoanUID").val();
        postModalFormAjax(form, `/Loan/Loan/DetailsView?uid=${loanUID}`);
    });
});