var paymentSaveLadda;

//Create Payment
$("#new-payment").on("click", function () {

    var actionUrl = $(this).data("action-url");

    $("#studentbatchpayment-offcanvas-content").load(actionUrl, function () {

        // Reset unobtrusive validation (important)
        var form = $("#studentbatchpayment-create-form");
        if (form.length) {
            form.removeData("validator");
            form.removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse(form);
        }

        // Show offcanvas
        var offcanvasEl = document.getElementById('studentbatchpaymentOffcanvas');
        var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
        offcanvas.show();

        $('#PaymentMonth').datepicker({
            format: "yyyy-mm",
            startView: "months",
            minViewMode: "months",
            autoclose: true
        });


        applyFormValidations();

    });

});

function onPaymentCreateBegin() {
    paymentSaveLadda = Ladda.create($('#btnSavePayment')[0]);
    paymentSaveLadda.start();
}

function onPaymentCreateSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('studentbatchpaymentOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        loadPaymentTable();
        loadSummary();

        popUpNotification('success', response.message);

    } else {

        popUpNotification('error', response.message);

    }
}

function onPaymentCreateFailure() {
    popUpNotification('error', 'Failed to save payment');
}

function onPaymentCreateComplete() {
    if (paymentSaveLadda) {
        paymentSaveLadda.stop();
    }
}

$("#SlipImage").on("change", function () {
    $(this).valid();
});

function applyFormValidations() {

    $("#studentbatchpayment-create-form").validate({

        ignore: [],

        rules: {
            StudentBatchId: "required",
            Amount: { required: true, min: 1 },
            PaymentMonth: "required",
            SlipImage: {
                required: true,
                imageValidation: true
            }
        },

        messages: {
            StudentBatchId: "Please select a batch",
            Amount: "Enter a valid amount",
            PaymentMonth: "Select month",
            SlipImage: "Upload a valid image (JPG/PNG/WebP max 5MB)"
        },

        errorClass: "text-danger small mt-1",
        errorElement: "div"
    });
}