var completeSaveLadda;

function initializeRatingControl() {
    var $ratingInput = $("#Rating");
    var currentRating = parseInt($ratingInput.val()) || 0;

    paintStars(currentRating);

    $("#rating-stars .star").off("mouseenter").on("mouseenter", function () {
        var value = parseInt($(this).data("value"));
        paintStars(value);
    });

    $("#rating-stars").off("mouseleave").on("mouseleave", function () {
        var selectedValue = parseInt($ratingInput.val()) || 0;
        paintStars(selectedValue);
    });

    $("#rating-stars .star").off("click").on("click", function () {
        var value = parseInt($(this).data("value"));
        $ratingInput.val(value).trigger("change");
        paintStars(value);
    });
}

function paintStars(rating) {
    $("#rating-stars .star").each(function () {
        var starValue = parseInt($(this).data("value"));
        $(this).toggleClass("active", starValue <= rating);
    });
}

//Create Payment
$("#btnComplete").on("click", function () {

    var actionUrl = $(this).data("action-url");

    $("#myLearning-offcanvas-content").load(actionUrl, function () {

        // Reset unobtrusive validation (important)
        var form = $("#batchlessoncomplete-create-form");
        if (form.length) {
            form.removeData("validator");
            form.removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse(form);
        }

        initializeRatingControl();

        // Show offcanvas
        var offcanvasEl = document.getElementById('myLearningOffcanvas');
        var offcanvas = new bootstrap.Offcanvas(offcanvasEl);
        offcanvas.show();

    });

});

function onBatchlessoncompleteCreateBegin() {
    completeSaveLadda = Ladda.create($('#btnCompleteBatchLesson')[0]);
    completeSaveLadda.start();
}

function onBatchlessoncompleteCreateSuccess(response) {

    if (response.success) {

        // Close offcanvas
        var offcanvasEl = document.getElementById('myLearningOffcanvas');
        var offcanvas = bootstrap.Offcanvas.getInstance(offcanvasEl);
        offcanvas.hide();

        popUpNotification('success', response.message);

        setTimeout(function () { location.reload(); }, 1500);

    } else {

        popUpNotification('error', response.message);

    }
}

function onBatchlessoncompleteCreateFailure() {
    popUpNotification('error', 'Failed to save');
}

function onBatchlessoncompleteCreateComplete() {
    if (completeSaveLadda) {
        completeSaveLadda.stop();
    }
}