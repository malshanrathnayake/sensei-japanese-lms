var batchLessonReferenceSaveLadda;

function addBatchLessonReferenceButton() {

    $(".add-batch-lesson-reference").on("click", function () {

        var actionUrl = $(this).data("action-url");

        $("#batch-lesson-Reference-offcanvas-content").load(actionUrl, function () {

            // Reset unobtrusive validation
            var form = $("#batch-lesson-reference-create-form");
            if (form.length) {
                form.removeData("validator");
                form.removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse(form);
            }

            // Show offcanvas
            var offcanvasEl = document.getElementById('batchLessonReferenceOffcanvas');
            var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
            offcanvas.show();

        });

    });
}

function onBatchLessonReferenceCreateBegin() {
    batchLessonReferenceSaveLadda = Ladda.create($('#btnSaveBatchLessonReference')[0]);
    batchLessonReferenceSaveLadda.start();
}

function onBatchLessonReferenceCreateSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('batchLessonReferenceOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadBatchLessons();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onBatchLessonReferenceCreateFailure() {
    popUpNotification('error', 'Failed to save batch lesson reference');
}

function onBatchLessonReferenceCreateComplete() {
    if (batchLessonReferenceSaveLadda) {
        batchLessonReferenceSaveLadda.stop();
    }
}