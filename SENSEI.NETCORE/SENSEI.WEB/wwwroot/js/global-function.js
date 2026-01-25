function loadSelectBox(data, className, title, modalId) {

    var selectBox;


    if (data !== '') {

        if (modalId == '') {
            modalId = this.$('.' + className).parent();
        }

        selectBox = $('.' + className).select2({

            theme: 'bootstrap4',
            allowClear: true,
            dropdownParent: $('#' + modalId),
            data: data,
            placeholder: title,
        });

        selectBox.on("results:message", function () {

        });

    } else {

        if (modalId == '') {
            modalId = this.$('.' + className).parent();
        }

        selectBox = $('.' + className).select2({
            theme: 'bootstrap4',
            allowClear: true,
            dropdownParent: $('#' + modalId),
            data: data,
            placeholder: title,
        });

        selectBox.on("results:message", function () {
        });
    }
}

function popUpNotification(type, message) {

    if (!window.notyf) return;

    if (type === 'success') {
        notyf.success(message);
    } else if (type === 'error') {
        notyf.error(message);
    }
}
