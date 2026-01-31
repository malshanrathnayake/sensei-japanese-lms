var lessonSaveLadda;

function loadCourseDropdown() {
    $.getJSON('/AdminPortal/Lesson/GetCourseListJsonResult', function (data) {

        $('#CourseId').empty();
        $('#CourseId').append('<option value="0">Select Course</option>')

        loadSelectBox({
            data: data,
            className: 'course-select',
            title: 'Select Course',
            dropdownParentId: 'lessonOffcanvas'
        });

        var courseId = $('#CourseId').val();

        if (courseId) {
            $('#CourseId').val(courseId).trigger('change');
        }
    });
}


//Create Lesson
$("#btnNewLesson").on("click", function () {

    var actionUrl = $(this).data("action-url");

    $("#lesson-offcanvas-content").load(actionUrl, function () {

        // Reset unobtrusive validation (important)
        var form = $("#lesson-create-form");
        if (form.length) {
            form.removeData("validator");
            form.removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse(form);
        }

        // Show offcanvas
        var offcanvasEl = document.getElementById('lessonOffcanvas');
        var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
        offcanvas.show();

        loadCourseDropdown();

    });

});

function onLessonCreateBegin() {
    lessonSaveLadda = Ladda.create($('#btnSaveLesson')[0]);
    lessonSaveLadda.start();
}

function onLessonCreateSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('lessonOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadLessons();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onLessonCreateFailure() {
    popUpNotification('error', 'Failed to save lesson');
}

function onLessonCreateComplete() {
    if (lessonSaveLadda) {
        lessonSaveLadda.stop();
    }
}


function editLessonButton() {
    //Edit Lesson
    $(".edit-lesson").on("click", function () {

        var actionUrl = $(this).data("action-url");

        $("#lesson-offcanvas-content").load(actionUrl, function () {

            // Reset unobtrusive validation (important)
            var form = $("#lesson-edit-form");
            if (form.length) {
                form.removeData("validator");
                form.removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse(form);
            }

            // Show offcanvas
            var offcanvasEl = document.getElementById('lessonOffcanvas');
            var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
            offcanvas.show();

            loadCourseDropdown();

        });

    });

}

function onLessonEditBegin() {
    lessonSaveLadda = Ladda.create($('#btnUpdateLesson')[0]);
    lessonSaveLadda.start();
}

function onLessonEditSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('lessonOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadLessons();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onLessonEditFailure() {
    popUpNotification('error', 'Failed to save lesson');
}

function onLessonEditComplete() {
    if (lessonSaveLadda) {
        lessonSaveLadda.stop();
    }
}

function deleteLessonButton() {

    $(".delete-lesson").on("click", function () {

        var actionUrl = $(this).data("action-url");
        var actionParameter = $(this).data("action-parameter");

        swalDelete(actionUrl, actionParameter, loadLessons);

    });
}



