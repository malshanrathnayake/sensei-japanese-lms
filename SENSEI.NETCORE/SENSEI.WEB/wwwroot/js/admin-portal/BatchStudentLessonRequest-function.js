var BatchStudentLessonRequestSaveLaddaBatchStudentLessonRequestSaveLadda;


function approveButton() {
    //Edit Batch
    $(".approve-request").on("click", function () {

        var actionUrl = $(this).data("action-url");

        $("#lessonRequest-offcanvas-content").load(actionUrl, function () {

            // Reset unobtrusive validation (important)
            var form = $("#BatchStudentLessonRequest-Approve-create-form");
            if (form.length) {
                form.removeData("validator");
                form.removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse(form);
            }

            // Show offcanvas
            var offcanvasEl = document.getElementById('lessonRequestOffcanvas');
            var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
            offcanvas.show();


        });

    });

}

function onApproveCreateBegin() {
    BatchStudentLessonRequestSaveLadda = Ladda.create($('#btnApprove')[0]);
    BatchStudentLessonRequestSaveLadda.start();
}

function onApproveCreateSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('lessonRequestOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadBatchStudentLessonRequests();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onApproveCreateFailure() {
    popUpNotification('error', 'Failed to register student');
}

function onApproveCreateComplete() {
    if (BatchStudentLessonRequestSaveLadda) {
        BatchStudentLessonRequestSaveLadda.stop();
    }
}
