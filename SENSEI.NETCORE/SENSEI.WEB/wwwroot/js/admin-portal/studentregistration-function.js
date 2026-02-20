var registrationSaveLadda;


function loadBatchDropdown() {
    $.getJSON('/AdminPortal/StudentRegistration/GetBatchListJsonResult',
        {
            courseId: $('#CourseId').val() || 0
        },
        function (data) {

        $('#BatchId').empty();
        $('#BatchId').append('<option value="0">Select Batch</option>')

        loadSelectBox({
            data: data,
            className: 'batch-select',
            title: 'Select Batch',
            dropdownParentId: 'studentRegistrationOffcanvas'
        });

        var batchId = $('#hdnBatchId').val();

        if (batchId) {
            $('#BatchId').val(batchId).trigger('change');
        }
    });
}

function approveButton() {
    //Edit Batch
    $(".approve-registration").on("click", function () {

        var actionUrl = $(this).data("action-url");

        $("#studentRegistration-offcanvas-content").load(actionUrl, function () {

            // Reset unobtrusive validation (important)
            var form = $("#studentRegistration-Approve-create-form");
            if (form.length) {
                form.removeData("validator");
                form.removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse(form);
            }

            // Show offcanvas
            var offcanvasEl = document.getElementById('studentRegistrationOffcanvas');
            var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
            offcanvas.show();


            loadBatchDropdown();

        });

    });

}

function onApproveCreateBegin() {
    batchSaveLadda = Ladda.create($('#btnApprove')[0]);
    batchSaveLadda.start();
}

function onApproveCreateSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('studentRegistrationOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadStudentRegistrations();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onApproveCreateFailure() {
    popUpNotification('error', 'Failed to register student');
}

function onApproveCreateComplete() {
    if (batchSaveLadda) {
        batchSaveLadda.stop();
    }
}

