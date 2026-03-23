function loadSelectBox(options) {

    var $select = $('.' + options.className);
    if (!$select.length) return;

    if ($select.hasClass("select2-hidden-accessible")) {
        $select.select2('destroy');
    }

    var $dropdownParent = options.dropdownParentId
        ? $('#' + options.dropdownParentId)
        : $(document.body); // Default to body to avoid clipping

    $select.select2({
        theme: 'bootstrap5',
        allowClear: true,
        placeholder: options.title,
        dropdownParent: $dropdownParent,
        data: options.data,
        width: '100%'
    });

}

/**
 * Enhanced placeholder/reset for DataTables Search
 */
function initDataTableSearch(tableId) {
    const $searchWrapper = $(`#${tableId}_wrapper .dataTables_filter`);
    if ($searchWrapper.length) {
        $searchWrapper.find('input').attr('placeholder', 'Search anything...');
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

function updateUserNotification() {

    $(".usernotification-item").on("click", function () {

        var item = $(this);
        var q = item.data("action-parameter");

        $.ajax({
            url: "/Layout/UpdateUserNotificationReadability",
            type: "POST",
            data: { q: q },
            success: function (response) {

                if (response.success) {
                    item.remove();
                    popUpNotification('success', 'Notification marked as read');
                }
                else {
                    //console.log(response.message);
                    //popUpNotification('error', 'Failed to mark notification as read');
                    item.remove();
                }

                updateUserNotificationCount();
            },
            error: function () {
                //console.log("Something went wrong");
                //popUpNotification('error', 'Failed to mark notification as read');
                item.remove();
                updateUserNotificationCount();
            }
        });

    });

    $("#markAllAsRead").on("click", function (e) {
        e.preventDefault();

        var notificationKeys = [];

        $(".usernotification-item").each(function () {
            var actionParameter = $(this).data("action-parameter");

            if (actionParameter) {
                notificationKeys.push(actionParameter);
            }
        });

        if (notificationKeys.length === 0) {
            popUpNotification('error', 'No notifications to mark as read.');
            return;
        }

        $.ajax({
            url: '/Layout/UpdateUserNotificationReadabilityMultiple',
            type: 'POST',
            traditional: true,
            data: { q: notificationKeys },
            success: function (response) {
                if (response.success) {

                    $("#userNotificationlist").empty();
                    updateUserNotificationCount();
                    popUpNotification('success', "All Notifications marked as read");

                    feather.replace();
                } else {
                    popUpNotification('error', response.message || "Failed to update notifications.");
                }
            },
            error: function () {
                popUpNotification('error', "Something went wrong while updating notifications.");
            }
        });
    });

    feather.replace();

}

function updateUserNotificationCount() {

    var count = $(".usernotification-item").length;

    $("#notification-count").text(count);

    if (count > 0) {
        $("#notification-list").html(count + " <span>New Notifications</span>");
    } else {
        $("#notification-list").html("<span>No Notifications</span>");
    }
}