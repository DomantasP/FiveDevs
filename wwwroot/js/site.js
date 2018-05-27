// Write your JavaScript code.

$('#searchField').on('input', function() {
    if ($(this).val().length > 0) $('#searchButton').prop('disabled', false);
    else $('#searchButton').prop('disabled', true);
});