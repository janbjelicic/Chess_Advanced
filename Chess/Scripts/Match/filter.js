$("#reset-button").on("click", function () {
    $("#rating-from-editor").val(0);
    $("#rating-to-editor").val(3000);
    $("#minutes-editor").val(5);
    $("#gain-editor").val(0);
    $("#is-rated-editor").prop('checked', true);
});

