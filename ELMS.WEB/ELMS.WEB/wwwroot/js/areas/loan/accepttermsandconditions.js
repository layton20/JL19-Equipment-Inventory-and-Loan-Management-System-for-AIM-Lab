$(document).ready(function () {
    $('.submit').prop("disabled", true);

    $('.checkAcceptTAC').click(function () {
        console.log($(this).prop("checked"));
        if ($(this).prop("checked")) {
            $('.submit').prop("disabled", false);
        } else {
            $('.submit').prop("disabled", true);
        }
    });
});