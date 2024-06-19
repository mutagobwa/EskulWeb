
//$(document).ready(function () {
//    $('#closeModalBtn').click(function () {
//        $('#modal-default').find('form')[0].reset();
//        $('#modal-lg').find('form')[0].reset();

//        // Reset all dropdowns within modals
//        $('#modal-default select2-drop').val('').trigger('change');
//        $('#modal-lg select2-drop').val('').trigger('change');
//    });
//});
$(document).ready(function () {
    $('#closeModalBtn').click(function () {
        // Reset the forms
        $('#modal-default').find('form')[0].reset();
        $('#modal-lg').find('form')[0].reset();

        // Reset all select2 elements within modals
        $('#modal-default select').val(null).trigger('change');
        $('#modal-lg select').val(null).trigger('change');

        // Reinitialize select2
        $('#modal-default select').select2();
        $('#modal-lg select').select2();

        // Hide the hidden-select div and reset the select2 within it
        $('.hidden-select').addClass('d-none');
    });

    // Ensure modals reset when hidden
    $('#modal-lg').on('hidden.bs.modal', function () {
        $(this).find('form')[0].reset();
        $(this).find('select').val(null).trigger('change');
        $('.hidden-select').addClass('d-none');

        // Reinitialize select2
        $(this).find('select').select2();
    });

    $('#modal-default').on('hidden.bs.modal', function () {
        $(this).find('form')[0].reset();
        $(this).find('select').val(null).trigger('change');

        // Reinitialize select2
        $(this).find('select').select2();
    });
});

