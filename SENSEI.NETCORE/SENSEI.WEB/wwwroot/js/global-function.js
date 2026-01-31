function loadSelectBox(options) {

    var $select = $('.' + options.className);
    if (!$select.length) return;

    if ($select.hasClass("select2-hidden-accessible")) {
        $select.select2('destroy');
    }

    var $dropdownParent = options.dropdownParentId
        ? $('#' + options.dropdownParentId)
        : $select.parent();

    $select.select2({
        theme: 'bootstrap5',
        allowClear: true,
        placeholder: options.title,
        dropdownParent: $dropdownParent,
        data: options.data
    });
}



function popUpNotification(type, message) {

    if (!window.notyf) return;

    if (type === 'success') {
        notyf.success(message);
    } else if (type === 'error') {
        notyf.error(message);
    }
}

// if need multiple functions to execute.floow the code as below
// swalDelete(actionUrl, actionParameter, () => {
//     loadLessons();
//     updateLessonCount();
// });
function swalDelete(actionUrl, actionParameter, callback) {

    Swal.fire({
        title: "Are you sure?",
        text: `Do you really want to delete "${actionParameter}"?`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#dc3545",
        cancelButtonColor: "#6c757d",
        confirmButtonText: "Yes, delete it",
        cancelButtonText: "Cancel"
    }).then((result) => {

        if (result.value) {

            $.ajax({
                url: actionUrl,
                type: "POST",
                success: function (res) {

                    if (res.success) {
                        window.notyf.open({
                            type: "success",
                            message: res.message
                        });

                        if (typeof callback === "function") {
                            callback();
                        }

                    } else {
                        window.notyf.open({
                            type: "warning",
                            message: res.message
                        });
                    }
                },
                error: function () {
                    window.notyf.open({
                        type: "danger",
                        message: "Server error occurred"
                    });
                }
            });


        }
    });
}