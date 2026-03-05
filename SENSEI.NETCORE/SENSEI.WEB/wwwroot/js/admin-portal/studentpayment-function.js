var paymentSaveLadda;


function approveButton() {
    //Edit Batch
    $(".approve-payment").on("click", function () {

        var actionUrl = $(this).data("action-url");

        $("#studentPayment-offcanvas-content").load(actionUrl, function () {

            // Reset unobtrusive validation (important)
            var form = $("#studentPayment-Approve-create-form");
            if (form.length) {
                form.removeData("validator");
                form.removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse(form);
            }

            // Show offcanvas
            var offcanvasEl = document.getElementById('studentPaymentOffcanvas');
            var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
            offcanvas.show();


        });

    });

}

function onApproveCreateBegin() {
    paymentSaveLadda = Ladda.create($('#btnApprove')[0]);
    paymentSaveLadda.start();
}

function onApproveCreateSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('studentPaymentOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadStudentPayments();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onApproveCreateFailure() {
    popUpNotification('error', 'Failed to register student');
}

function onApproveCreateComplete() {
    if (paymentSaveLadda) {
        paymentSaveLadda.stop();
    }
}

function rejectButton() {
    //Edit Batch
    $(".reject-payment").on("click", function () {

        var actionUrl = $(this).data("action-url");

        $("#studentPayment-offcanvas-content").load(actionUrl, function () {

            // Reset unobtrusive validation (important)
            var form = $("#studentPayment-Reject-create-form");
            if (form.length) {
                form.removeData("validator");
                form.removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse(form);
            }

            // Show offcanvas
            var offcanvasEl = document.getElementById('studentPaymentOffcanvas');
            var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
            offcanvas.show();

        });

    });

}

function onRejectCreateBegin() {
    paymentSaveLadda = Ladda.create($('#btnReject')[0]);
    paymentSaveLadda.start();
}

function onRejectCreateSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('studentPaymentOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadStudentPayments();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onRejectCreateFailure() {
    popUpNotification('error', 'Failed to register student');
}

function onRejectCreateComplete() {
    if (paymentSaveLadda) {
        paymentSaveLadda.stop();
    }
}

