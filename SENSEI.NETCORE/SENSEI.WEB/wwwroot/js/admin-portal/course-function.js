var courseSaveLadda;

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
