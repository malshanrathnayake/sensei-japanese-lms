var batchSaveLadda;

function loadCourseDropdown() {
    $.getJSON('/AdminPortal/Batch/GetCourseListJsonResult', function (data) {

        $('#CourseId').empty();
        $('#CourseId').append('<option value="0">Select Course</option>')

        loadSelectBox({
            data: data,
            className: 'course-select',
            title: 'Select Course',
            dropdownParentId: 'batchOffcanvas'
        });

        var courseId = $('#hdnCourseId').val();

        if (courseId) {
            $('#CourseId').val(courseId).trigger('change');
        }
    });
}


//Create Batch
$("#btnNewBatch").on("click", function () {

    var actionUrl = $(this).data("action-url");

    $("#batch-offcanvas-content").load(actionUrl, function () {

        // Reset unobtrusive validation (important)
        var form = $("#batch-create-form");
        if (form.length) {
            form.removeData("validator");
            form.removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse(form);
        }

        // Show offcanvas
        var offcanvasEl = document.getElementById('batchOffcanvas');
        var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
        offcanvas.show();

        loadCourseDropdown();

    });

});

function onBatchCreateBegin() {
    batchSaveLadda = Ladda.create($('#btnSaveBatch')[0]);
    batchSaveLadda.start();
}

function onBatchCreateSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('batchOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadBatches();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onBatchCreateFailure() {
    popUpNotification('error', 'Failed to save batch');
}

function onBatchCreateComplete() {
    if (batchSaveLadda) {
        batchSaveLadda.stop();
    }
}


function editBatchButton() {
    //Edit Batch
    $(".edit-batch").on("click", function () {

        var actionUrl = $(this).data("action-url");

        $("#batch-offcanvas-content").load(actionUrl, function () {

            // Reset unobtrusive validation (important)
            var form = $("#batch-edit-form");
            if (form.length) {
                form.removeData("validator");
                form.removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse(form);
            }

            // Show offcanvas
            var offcanvasEl = document.getElementById('batchOffcanvas');
            var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
            offcanvas.show();

            loadCourseDropdown();

        });

    });

}

function onBatchEditBegin() {
    batchSaveLadda = Ladda.create($('#btnUpdateBatch')[0]);
    batchSaveLadda.start();
}

function onBatchEditSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('batchOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadBatches();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onBatchEditFailure() {
    popUpNotification('error', 'Failed to save batch');
}

function onBatchEditComplete() {
    if (batchSaveLadda) {
        batchSaveLadda.stop();
    }
}

function deleteBatchButton() {

    $(".delete-batch").on("click", function () {

        var actionUrl = $(this).data("action-url");
        var actionParameter = $(this).data("action-parameter");

        swalDelete(actionUrl, actionParameter, loadBatches);

    });
}



