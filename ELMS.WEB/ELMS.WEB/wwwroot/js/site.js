$('.datatable').DataTable({
    responsive: {
        details: {
            display: $.fn.dataTable.Responsive.display.childRow
        }
    }
});

$('.datatable-export').DataTable({
    dom: 'Bfrtip',
    buttons: [
        {
            extend: 'csv',
            text: 'Export as CSV'
        },
        {
            extend: 'pdfHtml5',
            text: 'Export as PDF',
            orientation: 'landscape',
            pageSize: 'LEGAL',
        },
        {
            extend: 'excel',
            text: 'Export as Excel'
        },
    ],
    responsive: {
        details: {
            display: $.fn.dataTable.Responsive.display.childRow
        }
    }
});

$('.datatable-no-options').DataTable({
    paging: false,
    searching: false,
    info: false
});

$('[data-toggle="tooltip"]').tooltip();

tinymce.init({
    selector: '.tinymce',
    menubar: false,
});

// FileInput
$('.form-file-simple .inputFileVisible').click(function () {
    $(this).siblings('.inputFileHidden').trigger('click');
});

$('.form-file-simple .inputFileHidden').change(function () {
    var filename = $(this).val().replace(/C:\\fakepath\\/i, '');
    $(this).siblings('.inputFileVisible').val(filename);
});

$('.form-file-multiple .inputFileVisible, .form-file-multiple .input-group-btn').click(function () {
    $(this).parent().parent().find('.inputFileHidden').trigger('click');
    $(this).parent().parent().addClass('is-focused');
});

$('.form-file-multiple .inputFileHidden').change(function () {
    var names = '';
    for (var i = 0; i < $(this).get(0).files.length; ++i) {
        if (i < $(this).get(0).files.length - 1) {
            names += $(this).get(0).files.item(i).name + ',';
        } else {
            names += $(this).get(0).files.item(i).name;
        }
    }
    $(this).siblings('.input-group').find('.inputFileVisible').val(names);
});

$('.form-file-multiple .btn').on('focus', function () {
    $(this).parent().siblings().trigger('focus');
});

$('.form-file-multiple .btn').on('focusout', function () {
    $(this).parent().siblings().trigger('focusout');
});