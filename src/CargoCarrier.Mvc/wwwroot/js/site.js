// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function() {
    $('#calculateCostForm').on('submit', function(e) {
        e.preventDefault(); // Prevent the default form submission

        var formData = $(this).serialize(); // Serialize form data

        $.ajax({
            url: '/Finance/CalculateCost', // Your controller action URL
            type: 'POST',
            data: formData,
            success: function(data) {
                // Update the page content with the result
                $('#result').html('Cost: ' + data.result + ' €');
            },
            error: function() {
                $('#result').html('Error calculating cost.');
            }
        });
    });
});