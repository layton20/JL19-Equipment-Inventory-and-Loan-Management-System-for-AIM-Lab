$(document).ready(function () {
    var recipientTable = $('.datatable-recipients').DataTable({
        searching: false,
        info: false,
        responsive: {
            details: {
                display: $.fn.dataTable.Responsive.display.childRow
            }
        }
    });

    if ($('.template-radio:checked').length > 0) {
        $('.instructionText').text('Select the recipient(s) you would like to schedule emails for');
        $('.recipient-container').show();
    }

    $('.template-radio').change(function () {
        $('.instructionText').text('Select the recipient(s) you would like to schedule emails for');
        $('.recipient-container').show();
    });

    $('.recipient-checkbox-selectAll').change(function () {
        $('.recipient-checkbox').prop('checked', this.checked);

        if (this.checked) {
            $('.recipient-selection').prop('disabled', false);
        } else {
            $('.recipient-selection').prop('disabled', true);
        }
    });

    $(document).on('change', '.recipient-checkbox', function () {
        if (this.checked) {
            $(this).siblings('.recipient-selection').first().prop('disabled', false);
        }
        else {
            $(this).siblings('.recipient-selection').first().prop('disabled', true);
        }
    });

    $('.btn-recipient').click(function () {
        //console.log($('.recipient-email').filter(x => x.innerText != $('.input-recipient').val().toUpperCase()))
        var inputEmail = $.trim($('.input-recipient').val());

        var existingEmailsMatchingInput = $('.recipient-email').filter(function () {
            return $.trim($(this).text().toUpperCase()) == $.trim(inputEmail.toUpperCase())
        });

        if ($('.input-recipient').val() && existingEmailsMatchingInput.length <= 0) {
            recipientTable.row.add([
                `<input type="checkbox" class="recipient-checkbox" checked/><input class="recipient-selection" type="hidden" name="SelectedRecipientEmailAddresses" value="${inputEmail}"/>`,
                `<span class="recipient-email">${inputEmail}</span>`
            ]).draw();
        }
    });
});