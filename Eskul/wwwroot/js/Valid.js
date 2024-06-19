(function () {
    'use strict';

    // Fetch all the forms we want to apply custom Bootstrap validation styles to
    var forms = document.querySelectorAll('.needs-validation');

    // Loop over them and prevent submission
    Array.prototype.slice.call(forms).forEach(function (form) {
        form.addEventListener('submit', function (event) {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });

    // Custom validation for phone number beginning with "07" and only digits
    var phoneNumberInput = document.getElementById('Contact1');
    phoneNumberInput.addEventListener('input', function () {
        var phoneNumber = phoneNumberInput.value;
        phoneNumber = phoneNumber.replace(/\D/g, ''); // Remove non-digit characters
        phoneNumberInput.value = phoneNumber; // Update the input value

        if (!/^07/.test(phoneNumber) || phoneNumber.length < 10) {
            phoneNumberInput.setCustomValidity('Phone number must start with "07"');
        } else {
            phoneNumberInput.setCustomValidity('');
        }
    });

    // Custom validation for date of birth (DOB) not being the current date and above 18 years old
    var dobInput = document.getElementById('Dob');
    dobInput.addEventListener('input', function () {
        var dob = new Date(dobInput.value);
        var today = new Date();
        today.setHours(0, 0, 0, 0); // Set current time to midnight to compare only dates

        if (dob.toDateString() === today.toDateString()) {
            dobInput.setCustomValidity('Date of birth cannot be the current date.');
        } else {
            dobInput.setCustomValidity('');
        }
    });

    // Trigger the input event for the dob field initially to perform the validation
    dobInput.dispatchEvent(new Event('input'));
})();

//(function () {
//    'use strict'
//    // Fetch all the forms we want to apply custom Bootstrap validation styles to
//    var forms = document.querySelectorAll('.needs-validation')
//    // Loop over them and prevent submission
//    Array.prototype.slice.call(forms).forEach(function (form) {
//        form.addEventListener('submit', function (event) {
//            if (!form.checkValidity()) {
//                event.preventDefault()
//                event.stopPropagation()
//            }
//            form.classList.add('was-validated')
//        }, false)
//    });
//    // Custom validation for phone number beginning with "07" and only digits
//    var phoneNumberInput = document.getElementById('Contact1');
//    phoneNumberInput.addEventListener('input', function () {
//        var phoneNumber = phoneNumberInput.value;
//        phoneNumber = phoneNumber.replace(/\D/g, ''); // Remove non-digit characters
//        phoneNumberInput.value = phoneNumber; // Update the input value

//        if (!/^07/.test(phoneNumber) || phoneNumber.length < 10) {
//            phoneNumberInput.setCustomValidity('Phone number must start with "07"');
//        } else {
//            phoneNumberInput.setCustomValidity('');
//        }
//    });

//    // Custom validation for date of birth (DOB) not being the current date and above 18 years old
//    var dobInput = document.getElementById('Dob');
//    dobInput.addEventListener('input', function () {
//        var dob = new Date(dobInput.value);
//        console.log('Date of Birth', dob);
//        var today = new Date();
//        var age = today.getFullYear() - dob.getFullYear();
//        console.log('Age of Birth', age);
//        if (dob > today || (age < 18)) {
//            dobInput.setCustomValidity('Staff should be at least 18 years old.');
//        } else {
//            dobInput.setCustomValidity('');
//        }
//    });

//    // Trigger the input event for the dob field initially to perform the validation
//    dobInput.dispatchEvent(new Event('input'));
//    var dobInputs = document.getElementById('Dobs');
//    dobInputs.addEventListener('input', function () {
//        var dob1 = new Date(dobInputs.value);
//        console.log('Date of Birth', dob1);
//        var today1 = new Date();
//        var age1 = today1.getFullYear() - dob1.getFullYear();
//        console.log('Age of Birth', age1);
//        if (dob1 > today1 || (age1 < 10)) {
//            dobInputs.setCustomValidity('Student should be at least 10 years old.');
//        } else {
//            dobInputs.setCustomValidity('');
//        }
//    });

//    // Trigger the input event for the dob field initially to perform the validation
//    dobInputs.dispatchEvent(new Event('input'));
//})()





