var batchLessonSaveLadda;

function loadBatchDropdown(batchSelectClass, wrapperId, hdnBatchIdSelector) {
    $.getJSON('/AdminPortal/BatchLesson/GetBatchListJsonResult', function (data) {

        var $select = $('.' + batchSelectClass).first();
        $select.empty();

        loadSelectBox({
            data: data,
            className: batchSelectClass,
            title: 'Select Batch',
            dropdownParentId: wrapperId,
            allowClear: false
        });

        var batchId = $(hdnBatchIdSelector).val();
        if (batchId && batchId !== "0") {
            $select.val(batchId).trigger('change');
            loadLessonDropdown('edit-lesson-select', 'edit-lesson-wrapper', '#hdnLessonId');
        } else {
            loadLessonDropdown('lesson-select', 'create-lesson-wrapper', '#hdnLessonId');
        }

    });
}

function loadLessonDropdown(lessonSelectClass, wrapperId, hdnLessonIdSelector) {
    var currentBatchId = $('#BatchId').val() || $('#EditBatchId').val() || "0";
    
    $.getJSON('/AdminPortal/BatchLesson/GetLessonListJsonResult', {
        batchId: currentBatchId
    }, function (data) {

        var $select = $('.' + lessonSelectClass).first();
        $select.empty();

        loadSelectBox({
            data: data,
            className: lessonSelectClass,
            title: 'Select Lesson',
            dropdownParentId: wrapperId,
            allowClear: false
        });

        var lessonId = $(hdnLessonIdSelector).val();
        if (lessonId && lessonId !== "0") {
            $select.val(lessonId).trigger('change');
        }

        setTimeout(function () {
            $(".select2-container").css("width", "100%");
        }, 100);

    });
}


// ─── Create Batch Lesson ─────────────────────────────────────────────────────

$("#btnNewBatchLesson").on("click", function () {

    var actionUrl = $(this).data("action-url");

    $("#batch-lesson-offcanvas-content").load(actionUrl, function () {

        // Reset unobtrusive validation
        var form = $("#batch-lesson-create-form");
        if (form.length) {
            form.removeData("validator");
            form.removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse(form);
        }

        // Show offcanvas
        var offcanvasEl = document.getElementById('batchLessonOffcanvas');
        var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
        offcanvas.show();

        // Init datepicker for recording expire date
        $('#RecordingExpireDate').datepicker({
            format: 'yyyy-mm-dd',
            autoclose: true,
            todayHighlight: true,
        });

        // Init datepicker for recording expire date
        $('#LessonDateTime').datepicker({
            format: 'yyyy-mm-dd',
            autoclose: true,
            todayHighlight: true,
        });

        $("#BatchId").change(function () {

            loadLessonDropdown('lesson-select', 'create-lesson-wrapper', '#hdnLessonId');

        });

        loadBatchDropdown('batch-select', 'create-batch-wrapper', '#hdnBatchId');

    });

});

function onBatchLessonCreateBegin() {
    batchLessonSaveLadda = Ladda.create($('#btnSaveBatchLesson')[0]);
    batchLessonSaveLadda.start();
}

function onBatchLessonCreateSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('batchLessonOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadBatchLessons();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onBatchLessonCreateFailure() {
    popUpNotification('error', 'Failed to save batch lesson');
}

function onBatchLessonCreateComplete() {
    if (batchLessonSaveLadda) {
        batchLessonSaveLadda.stop();
    }
}


// ─── Edit Batch Lesson ───────────────────────────────────────────────────────

function editBatchLessonButton() {

    $(".edit-batch-lesson").on("click", function () {

        var actionUrl = $(this).data("action-url");

        $("#batch-lesson-offcanvas-content").load(actionUrl, function () {

            // Reset unobtrusive validation
            var form = $("#batch-lesson-edit-form");
            if (form.length) {
                form.removeData("validator");
                form.removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse(form);
            }

            // Show offcanvas
            var offcanvasEl = document.getElementById('batchLessonOffcanvas');
            var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
            offcanvas.show();

            // Init datepicker for recording expire date
            $('#RecordingExpireDate').datepicker({
                format: 'yyyy-mm-dd',
                autoclose: true,
                todayHighlight: true,
            });

            // Init datepicker for recording expire date
            $('#LessonDateTime').datepicker({
                format: 'yyyy-mm-dd',
                autoclose: true,
                todayHighlight: true,
            });

            $("#EditBatchId").change(function () {

                loadLessonDropdown('edit-lesson-select', 'edit-lesson-wrapper', '#hdnLessonId');

            });

            loadBatchDropdown('edit-batch-select', 'edit-batch-wrapper', '#hdnBatchId');
            

        });

    });

}

function onBatchLessonEditBegin() {
    batchLessonSaveLadda = Ladda.create($('#btnUpdateBatchLesson')[0]);
    batchLessonSaveLadda.start();
}

function onBatchLessonEditSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('batchLessonOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadBatchLessons();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onBatchLessonEditFailure() {
    popUpNotification('error', 'Failed to save batch lesson');
}

function onBatchLessonEditComplete() {
    if (batchLessonSaveLadda) {
        batchLessonSaveLadda.stop();
    }
}


// ─── Delete Batch Lesson ─────────────────────────────────────────────────────

function deleteBatchLessonButton() {

    $(".delete-batch-lesson").on("click", function () {

        var actionUrl = $(this).data("action-url");
        var actionParameter = $(this).data("action-parameter");

        swalDelete(actionUrl, actionParameter, loadBatchLessons);

    });
}
