$(document).ready(function () {

    $(".checkbox-equipment").click(function () {
        var selectedEquipmentElem = $(this).siblings('.equipment-selection');

        if ($(this).prop('checked')) {
            selectedEquipmentElem.prop('disabled', false);
        } else {
            selectedEquipmentElem.prop('disabled', true);
        }
    });

    $("#collapseLinkManualUser").click(function () {
        window.setTimeout(function () {
            if ($("#collapseManualUser").hasClass("show")) {
                $('.user-detail').each(function () {
                    if ($(this).siblings('.user-radio').attr("checked")) {
                        $(this).prop('disabled', false);
                    }
                });
            }
            else {
                $('.user-detail').prop('disabled', true);
            }
        }, 1000);
    });

    $("#collapseLinkExistingUser").click(function () {
        window.setTimeout(function () {
            if ($("#collapseExistingUser").hasClass("show")) {
                $('.user-detail').each(function () {
                    if ($(this).siblings('.user-radio').attr("checked")) {
                        $(this).prop('disabled', false);
                    } else {
                        $(this).prop('disabled', true);
                    }
                });
            }
            else {
                $('.user-detail').prop('disabled', true);
            }
        }, 500);
    });

    $('.user-detail').each(function () {
        if ($(this).siblings('.user-radio').attr("checked")) {
            $(this).prop('disabled', false);
        } else {
            $(this).prop('disabled', true);
        }
    });

    $('.user-radio').click(function () {
        $('.user-radio').removeAttr("checked");
        $(this).attr("checked", true);

        window.setTimeout(function () {
            $('.user-detail').each(function () {
                if ($(this).siblings('.user-radio').attr("checked")) {
                    $(this).prop('disabled', false);
                } else {
                    $(this).prop('disabled', true);
                }
            });
        }, 500);
    });
});