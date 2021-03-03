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