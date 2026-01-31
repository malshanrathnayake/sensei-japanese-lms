var courseSaveLadda;

//Create Course
$("#btnNewCourse").on("click", function () {

    var actionUrl = $(this).data("action-url");

    $("#course-offcanvas-content").load(actionUrl, function () {

        // Reset unobtrusive validation (important)
        var form = $("#course-create-form");
        if (form.length) {
            form.removeData("validator");
            form.removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse(form);
        }

        // Show offcanvas
        var offcanvasEl = document.getElementById('courseOffcanvas');
        var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
        offcanvas.show();

    });

});

function onCourseCreateBegin() {
    courseSaveLadda = Ladda.create($('#btnSaveCourse')[0]);
    courseSaveLadda.start();
}

function onCourseCreateSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('courseOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadCourses();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onCourseCreateFailure() {
    popUpNotification('error', 'Failed to save course');
}

function onCourseCreateComplete() {
    if (courseSaveLadda) {
        courseSaveLadda.stop();
    }
}


function editCourseButton() {
    //Edit Course
    $(".edit-course").on("click", function () {

        var actionUrl = $(this).data("action-url");

        $("#course-offcanvas-content").load(actionUrl, function () {

            // Reset unobtrusive validation (important)
            var form = $("#course-edit-form");
            if (form.length) {
                form.removeData("validator");
                form.removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse(form);
            }

            // Show offcanvas
            var offcanvasEl = document.getElementById('courseOffcanvas');
            var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
            offcanvas.show();

        });

    });

}

function onCourseEditBegin() {
    courseSaveLadda = Ladda.create($('#btnUpdateCourse')[0]);
    courseSaveLadda.start();
}

function onCourseEditSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('courseOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadCourses();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onCourseEditFailure() {
    popUpNotification('error', 'Failed to save course');
}

function onCourseEditComplete() {
    if (courseSaveLadda) {
        courseSaveLadda.stop();
    }
}

function deleteCourseButton() {

    $(".delete-course").on("click", function () {

        var actionUrl = $(this).data("action-url");
        var actionParameter = $(this).data("action-parameter");

        swalDelete(actionUrl, actionParameter, loadCourses);

        //Swal.fire({
        //    title: "Are you sure?",
        //    text: `Do you really want to delete "${actionParameter}"?`,
        //    icon: "warning",
        //    showCancelButton: true,
        //    confirmButtonColor: "#dc3545",
        //    cancelButtonColor: "#6c757d",
        //    confirmButtonText: "Yes, delete it",
        //    cancelButtonText: "Cancel"
        //}).then((result) => {

        //    if (result.value) {

        //        $.ajax({
        //            url: actionUrl,
        //            type: "POST",
        //            success: function (res) {

        //                if (res.success) {
        //                    window.notyf.open({
        //                        type: "success",
        //                        message: res.message
        //                    });

        //                    loadCourses();

        //                } else {
        //                    window.notyf.open({
        //                        type: "warning",
        //                        message: res.message
        //                    });
        //                }
        //            },
        //            error: function () {
        //                window.notyf.open({
        //                    type: "danger",
        //                    message: "Server error occurred"
        //                });
        //            }
        //        });


        //    }
        //});

    });
}



