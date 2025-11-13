//View My Profile Modal
function viewMyProfile(userId) {
    $.ajax({
        url: '/customer/get-my-profile',
        type: 'GET',
        data: { id: userId },
        success: function (response) {
            $('#myProfileContent').html(response);
            $('#myProfileModal').modal('show');
        },
        error: function (xhr, status, error) {
            console.error('Error loading profile:', error);
            Swal.fire({
                icon: "error",
                title: "Error",
                text: "Unable to load profile information"
            });
        }
    });
}

//View Settings Modal
function viewSettings(userId) {
    // Show loading
    $('#settingsContent').html(`
        <div class="text-center py-4">
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
            <p class="mt-2">Loading settings...</p>
        </div>
    `);

    // Show modal
    $('#settingsModal').modal('show');

    // Load settings form
    $.ajax({
        url: '/customer/get-settings',
        type: 'GET',
        data: { id: userId },
        success: function (response) {
            $('#settingsContent').html(response);

            // Initialize validation after loading
            initializeSettingsValidation();
        },
        error: function (xhr, status, error) {
            console.error('Error loading settings:', error);
            Swal.fire({
                icon: "error",
                title: "Error",
                text: "Unable to load settings form"
            });
            $('#settingsModal').modal('hide');
        }
    });
}

// Save Settings Handler
$(document).ready(function () {
    $(document).on('click', '#btnSaveSettings', function (e) {
        e.preventDefault();

        const form = $('#customerSettingsForm');
        const password = $('#customerPassword').val();
        const confirmPassword = $('#customerConfirmPassword').val();

        // Validate password if provided
        if (password && password.length > 0) {
            if (!confirmPassword || confirmPassword.length === 0) {
                Swal.fire({
                    icon: "warning",
                    title: "Missing Confirmation",
                    text: "Please confirm your password."
                });
                return false;
            }

            if (password !== confirmPassword) {
                Swal.fire({
                    icon: "warning",
                    title: "Password Mismatch",
                    text: "Passwords do not match."
                });
                return false;
            }

            const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).{6,}$/;
            if (!passwordRegex.test(password)) {
                Swal.fire({
                    icon: "warning",
                    title: "Weak Password",
                    text: "Password must be at least 6 characters with 1 uppercase, 1 lowercase, 1 number and 1 special character."
                });
                return false;
            }
        }

        // Show loading
        Swal.fire({
            title: 'Updating...',
            text: 'Please wait...',
            allowOutsideClick: false,
            didOpen: () => {
                Swal.showLoading();
            }
        });

        // Submit form
        const formData = new FormData(form[0]);

        $.ajax({
            url: '/customer/update-profile',
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (response) {
                Swal.close();
                if (response.success) {
                    Swal.fire({
                        icon: "success",
                        title: "Success",
                        text: response.message || "Profile updated successfully!",
                        timer: 2000,
                        showConfirmButton: false
                    }).then(() => {
                        $('#settingsModal').modal('hide');
                        location.reload();
                    });
                } else {
                    Swal.fire({
                        icon: "error",
                        title: "Error",
                        text: response.message || "Update failed"
                    });
                }
            },
            error: function (xhr, status, error) {
                Swal.close();
                console.error('Update error:', xhr);

                let errorMessage = "Unable to update profile. Please try again.";

                try {
                    const errorResponse = JSON.parse(xhr.responseText);
                    if (errorResponse.message) {
                        errorMessage = errorResponse.message;
                    }
                } catch (e) {
                    console.log('Could not parse error response');
                }

                Swal.fire({
                    icon: "error",
                    title: "Connection Error",
                    text: errorMessage
                });
            }
        });

        return false;
    });
});

// Initialize validation for settings form
function initializeSettingsValidation() {
    // Real-time password validation
    $('#customerPassword, #customerConfirmPassword').on('input', function () {
        const password = $('#customerPassword').val();
        const confirmPassword = $('#customerConfirmPassword').val();

        if (password && confirmPassword) {
            if (password !== confirmPassword) {
                $('#customerConfirmPassword').addClass('is-invalid');
                $('#customerConfirmPassword').next('.text-danger').text('Passwords do not match');
            } else {
                $('#customerConfirmPassword').removeClass('is-invalid');
                $('#customerConfirmPassword').next('.text-danger').text('');
            }
        }
    });

    // Validate phone number
    $('input[name="PhoneNumber"]').on('input', function () {
        let value = $(this).val();
        value = value.replace(/[^0-9]/g, '');
        if (value.length > 10) {
            value = value.substring(0, 10);
        }
        $(this).val(value);
    });

    // Validate email
    $('input[name="Email"]').on('blur', function () {
        const email = $(this).val();
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

        if (email && !emailRegex.test(email)) {
            $(this).addClass('is-invalid');
            $(this).next('.text-danger').text('Invalid email format');
        } else {
            $(this).removeClass('is-invalid');
            $(this).next('.text-danger').text('');
        }
    });

    // Validate photo upload
    $('input[name="Photo"]').on('change', function (e) {
        const file = e.target.files[0];
        if (file) {
            if (file.size > 5 * 1024 * 1024) {
                Swal.fire({
                    icon: "warning",
                    title: "File Too Large",
                    text: "File size cannot exceed 5MB"
                });
                $(this).val('');
                return;
            }

            const allowedTypes = ['image/jpeg', 'image/jpg', 'image/png', 'image/gif'];
            if (!allowedTypes.includes(file.type)) {
                Swal.fire({
                    icon: "warning",
                    title: "Invalid Format",
                    text: "Only image files are accepted (JPG, PNG, GIF)"
                });
                $(this).val('');
                return;
            }
        }
    });
}