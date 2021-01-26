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
            extend: 'pdf',
            text: 'Export as PDF'
        },
        {
            extend: 'excel',
            text: 'Export as Excel'
        }
    ],
    responsive: {
        details: {
            display: $.fn.dataTable.Responsive.display.childRow
        }
    }
});

$('[data-toggle="tooltip"]').tooltip()