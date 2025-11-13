function token() {
    return $('input[name=__RequestVerificationToken]').val();
}

function loginAccount(userInput) {
    userInput.__RequestVerificationToken = token();
    Swal.fire({
        title: 'Đang đăng nhập...',
        text: 'Vui lòng chờ trong giây lát.',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    $.ajax({
        type: "POST",
        url: "/Account/LoginToSystem",
        data: userInput,
        dataType: 'json',
        success: function (res) {
            Swal.close();
            if (res.status === 'success' && res.success === true) {
                Swal.fire({
                    icon: "success",
                    title: "Đăng nhập thành công!",
                    text: res.message,
                    timer: 1500,
                    showConfirmButton: false
                }).then(() => {
                    location.href = res.redirectUrl || '/';
                });
            }
            else {
                Swal.fire({
                    icon: "error",
                    title: "Lỗi đăng nhập",
                    text: res.message
                });
            }
        },
        error: function (xhr, status, error) {
            Swal.close();
            console.error('Login error:', xhr.responseText);
            Swal.fire({
                icon: "error",
                title: "Lỗi kết nối",
                text: "Không thể kết nối đến server"
            });
        }
    });
}

function signupAccount(userInput) {
    userInput.__RequestVerificationToken = token();
    Swal.fire({
        title: 'Đang đăng ký...',
        text: 'Vui lòng chờ trong giây lát.',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
    $.ajax({
        type: "POST",
        url: "/Account/SignUpUser",
        data: userInput,
        dataType: 'json',
        success: function (res) {
            Swal.close();
            if (res.success) {
                Swal.fire({
                    icon: "success",
                    title: "Đăng ký thành công!",
                    text: res.message,
                    timer: 2000,
                    showConfirmButton: false
                }).then(() => {
                    window.location.href = '/login';
                });
            } else {
                let errorMessage = res.message;
                if (res.errors && res.errors.length > 0) {
                    errorMessage += '\n\n' + res.errors.join('\n');
                }
                Swal.fire({
                    icon: "error",
                    title: "Đăng ký thất bại",
                    text: errorMessage
                });
            }
        },
        error: function (xhr, status, error) {
            Swal.close();
            console.error('Signup error:', xhr.responseText);
            Swal.fire({
                icon: "error",
                title: "Lỗi kết nối",
                text: "Không thể kết nối đến server. Vui lòng thử lại."
            });
        }
    });
}

// Gửi OTP
function sendOtp(email) {
    var tokenValue = token();
    Swal.fire({
        title: 'Đang gửi OTP...',
        text: 'Vui lòng chờ trong giây lát.',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });

    $.ajax({
        type: "POST",
        url: "/Account/SendOtp",
        data: {
            email: email,
            __RequestVerificationToken: tokenValue
        },
        dataType: 'json',
        success: function (res) {
            Swal.close();
            if (res.success) {
                Swal.fire({
                    icon: "success",
                    title: "Thành công",
                    text: res.message,
                    timer: 2000,
                    showConfirmButton: false
                }).then(() => {
                    sessionStorage.setItem('resetEmail', email);
                    window.location.href = '/otp';
                });
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Lỗi",
                    text: res.message
                });
            }
        },
        error: function (xhr, status, error) {
        
            try {
                var errorResponse = JSON.parse(xhr.responseText);
                console.log('Parsed Error Response:', errorResponse);
            } catch (e) {
                console.log('Could not parse error response');
            }

            Swal.close();
            // Hiển thị thông tin chi tiết lỗi
            var errorMessage = "Không thể kết nối đến server";
            Swal.fire({
                icon: "error",
                title: "Lỗi kết nối",
                text: errorMessage
            });
        }
    });
}

// Xác thực OTP
function verifyOtp(email, otpCode) {
    Swal.fire({
        title: 'Đang xác thực OTP...',
        text: 'Vui lòng chờ trong giây lát.',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });

    $.ajax({
        type: "POST",
        url: "/Account/VerifyOtp",
        data: {
            email: email,
            otpCode: otpCode,
            __RequestVerificationToken: token()
        },
        dataType: 'json',
        success: function (res) {
            Swal.close();
            if (res.success) {
                Swal.fire({
                    icon: "success",
                    title: "Thành công",
                    text: res.message,
                    timer: 1500,
                    showConfirmButton: false
                }).then(() => {
                    sessionStorage.setItem('otpCode', otpCode);
                    window.location.href = '/reset-password';
                });
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Lỗi",
                    text: res.message
                });
            }
        },
        error: function (xhr, status, error) {
            Swal.close();
            console.error('Verify OTP error:', xhr.responseText);
            Swal.fire({
                icon: "error",
                title: "Lỗi kết nối",
                text: "Không thể kết nối đến server"
            });
        }
    });
}

// Reset password
function resetPassword(email, otpCode, newPassword, confirmPassword) {
    Swal.fire({
        title: 'Đang đặt lại mật khẩu...',
        text: 'Vui lòng chờ trong giây lát.',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });

    $.ajax({
        type: "POST",
        url: "/Account/ResetPassword",
        data: {
            email: email,
            otpCode: otpCode,
            newPassword: newPassword,
            confirmPassword: confirmPassword,
            __RequestVerificationToken: token()
        },
        dataType: 'json',
        success: function (res) {
            Swal.close();
            if (res.success) {
                Swal.fire({
                    icon: "success",
                    title: "Thành công",
                    text: res.message,
                    timer: 2000,
                    showConfirmButton: false
                }).then(() => {
                    sessionStorage.removeItem('resetEmail');
                    sessionStorage.removeItem('otpCode');
                    window.location.href = '/login';
                });
            } else {
                Swal.fire({
                    icon: "error",
                    title: "Lỗi",
                    text: res.message
                });
            }
        },
        error: function (xhr, status, error) {
            Swal.close();
            console.error('Reset password error:', xhr.responseText);
            Swal.fire({
                icon: "error",
                title: "Lỗi kết nối",
                text: "Không thể kết nối đến server"
            });
        }
    });
}

// Xử lý OTP inputs
function initOtpInputs() {
    const otpInputs = $('.otp-input');

    // Xử lý input
    otpInputs.on('input', function () {
        const input = $(this);
        const value = input.val();

        // Chỉ cho phép nhập số
        if (!/^\d*$/.test(value)) {
            input.val('');
            return;
        }

        // Tự động focus sang ô tiếp theo
        if (value.length === 1) {
            const nextIndex = parseInt(input.attr('data-index')) + 1;
            if (nextIndex < 6) {
                $(`.otp-input[data-index="${nextIndex}"]`).focus();
            }
        }

        // Cập nhật hidden input
        updateOtpCode();
    });

    // Xử lý backspace
    otpInputs.on('keydown', function (e) {
        if (e.key === 'Backspace' && $(this).val() === '') {
            const prevIndex = parseInt($(this).attr('data-index')) - 1;
            if (prevIndex >= 0) {
                $(`.otp-input[data-index="${prevIndex}"]`).focus();
            }
        }
    });

    // Xử lý paste
    otpInputs.first().on('paste', function (e) {
        e.preventDefault();
        const pastedData = e.originalEvent.clipboardData.getData('text');
        const digits = pastedData.match(/\d/g);

        if (digits && digits.length === 6) {
            otpInputs.each(function (index) {
                $(this).val(digits[index]);
            });
            updateOtpCode();
            otpInputs.last().focus();
        }
    });

    function updateOtpCode() {
        let otp = '';
        otpInputs.each(function () {
            otp += $(this).val();
        });
        $('#otp-code').val(otp);
    }
}

// Countdown timer cho resend OTP
let countdownInterval = null;

function startResendCountdown(seconds) {
    const resendLink = $('#resend-otp-link');
    const countdownSpan = $('#countdown');
    let timeLeft = seconds;

    resendLink.css({
        'pointer-events': 'none',
        'opacity': '0.5'
    });

    // Clear interval cũ nếu có
    if (countdownInterval) {
        clearInterval(countdownInterval);
    }

    countdownInterval = setInterval(() => {
        timeLeft--;
        countdownSpan.text(timeLeft);

        if (timeLeft <= 0) {
            clearInterval(countdownInterval);
            countdownInterval = null;
            resendLink.css({
                'pointer-events': 'auto',
                'opacity': '1'
            });
            resendLink.html('Resend It');
        }
    }, 1000);
}

document.addEventListener('DOMContentLoaded', function () {
    // Login form handler
    $('#login_form').off('submit').on('submit', function (e) {
        e.preventDefault();
        console.log('Login form submitted');
        loginAccount({
            username: $('#email').val(),
            password: $('#password').val(),
            rememberMe: $('#flexCheckDefault').is(':checked')
        });
        return false;
    });

    // Sign Up form handler
    $('#signup_form').off('submit').on('submit', function (e) {
        e.preventDefault();
        console.log('Signup form submitted');

        // Tạo form data để gửi lên server
        const formData = {
            FullName: $('input[name="FullName"]').val().trim(),
            Email: $('input[name="Email"]').val().trim(),
            Password: $('input[name="Password"]').val(),
            ConfirmPassword: $('input[name="ConfirmPassword"]').val()
        };

        console.log('Signup data:', formData);
        signupAccount(formData);
        return false;
    });

    // Forgot Password form handler
    $('#forgot_password_form').off('submit').on('submit', function (e) {
        e.preventDefault();
        console.log('Forgot password form submitted');

        const email = $('#forgot-email').val();

        if (!email) {
            Swal.fire({
                icon: "warning",
                title: "Thông báo",
                text: "Vui lòng nhập email"
            });
            return false;
        }

        sendOtp(email);
        return false;
    });

    // Khởi tạo OTP inputs nếu đang ở trang OTP
    if ($('#verify_otp_form').length > 0) {
        // Hiển thị email đã mask
        const email = sessionStorage.getItem('resetEmail');
        if (email) {
            const maskedEmail = email.substring(0, 3) + '*****' + email.substring(email.indexOf('@'));
            $('#masked-email').text(maskedEmail);
        }

        // Khởi tạo OTP inputs
        initOtpInputs();

        // Bắt đầu countdown
        startResendCountdown(60);

        // Focus vào ô đầu tiên
        $('.otp-input').first().focus();

        // Xử lý submit form OTP
        $('#verify_otp_form').off('submit').on('submit', function (e) {
            e.preventDefault();
            console.log('Verify OTP form submitted');

            const email = sessionStorage.getItem('resetEmail');
            const otpCode = $('#otp-code').val();

            if (!email) {
                Swal.fire({
                    icon: "error",
                    title: "Lỗi",
                    text: "Không tìm thấy email. Vui lòng thử lại từ đầu."
                }).then(() => {
                    window.location.href = '/forgot-password';
                });
                return false;
            }

            if (!otpCode || otpCode.length !== 6) {
                Swal.fire({
                    icon: "warning",
                    title: "Thông báo",
                    text: "Vui lòng nhập đầy đủ mã OTP (6 chữ số)"
                });
                return false;
            }

            verifyOtp(email, otpCode);
            return false;
        });

        // Resend OTP handler
        $('#resend-otp-link').off('click').on('click', function (e) {
            e.preventDefault();

            if ($(this).css('pointer-events') === 'none') {
                return;
            }

            const email = sessionStorage.getItem('resetEmail');

            if (!email) {
                Swal.fire({
                    icon: "error",
                    title: "Lỗi",
                    text: "Không tìm thấy email. Vui lòng thử lại từ đầu."
                }).then(() => {
                    window.location.href = '/forgot-password';
                });
                return;
            }

            Swal.fire({
                title: 'Đang gửi lại OTP...',
                text: 'Vui lòng chờ trong giây lát.',
                allowOutsideClick: false,
                didOpen: () => {
                    Swal.showLoading();
                }
            });

            $.ajax({
                type: "POST",
                url: "/Account/SendOtp",
                data: {
                    email: email,
                    __RequestVerificationToken: token()
                },
                dataType: 'json',
                success: function (res) {
                    Swal.close();
                    if (res.success) {
                        Swal.fire({
                            icon: "success",
                            title: "Thành công",
                            text: "Mã OTP mới đã được gửi đến email của bạn",
                            timer: 2000,
                            showConfirmButton: false
                        });
                        // Restart countdown
                        startResendCountdown(60);
                        // Clear OTP inputs
                        $('.otp-input').val('');
                        $('.otp-input').first().focus();
                        $('#otp-code').val('');
                    } else {
                        Swal.fire({
                            icon: "error",
                            title: "Lỗi",
                            text: res.message
                        });
                    }
                },
                error: function (xhr, status, error) {
                    Swal.close();
                    console.error('Resend OTP error:', xhr.responseText);
                    Swal.fire({
                        icon: "error",
                        title: "Lỗi kết nối",
                        text: "Không thể kết nối đến server"
                    });
                }
            });
        });
    }

    // Reset Password form handler
    $('#reset_password_form').off('submit').on('submit', function (e) {
        e.preventDefault();
        console.log('Reset password form submitted');

        const email = sessionStorage.getItem('resetEmail');
        const otpCode = sessionStorage.getItem('otpCode');
        const newPassword = $('#new-password').val();
        const confirmPassword = $('#confirm-password').val();

        if (!email || !otpCode) {
            Swal.fire({
                icon: "error",
                title: "Lỗi",
                text: "Phiên làm việc đã hết hạn. Vui lòng thử lại từ đầu."
            }).then(() => {
                window.location.href = '/forgot-password';
            });
            return false;
        }

        if (!newPassword || !confirmPassword) {
            Swal.fire({
                icon: "warning",
                title: "Thông báo",
                text: "Vui lòng nhập đầy đủ thông tin"
            });
            return false;
        }

        if (newPassword !== confirmPassword) {
            Swal.fire({
                icon: "warning",
                title: "Thông báo",
                text: "Mật khẩu xác nhận không khớp"
            });
            return false;
        }

        resetPassword(email, otpCode, newPassword, confirmPassword);
        return false;
    });

    // External login buttons
    $('#google-signup-button').on('click', function (e) {
        e.preventDefault();
        $('#google-signup-form').submit();
    });

    $('#google-login-button').on('click', function (e) {
        e.preventDefault();
        $('#google-login-form').submit();
    });

    $('#facebook-signup-button').on('click', function (e) {
        e.preventDefault();
        $('#facebook-signup-form').submit();
    });

    $('#facebook-login-button').on('click', function (e) {
        e.preventDefault();
        $('#facebook-login-form').submit();
    });
});
//end Login and Sign Up

//Logout
document.addEventListener("DOMContentLoaded", function () {
    const logoutBtn = document.getElementById("btnLogout");
    if (!logoutBtn) return;

    logoutBtn.addEventListener("click", function (e) {
        e.preventDefault(); // Ngăn chuyển trang ngay

        Swal.fire({
            title: 'Bạn có chắc muốn đăng xuất?',
            text: "Phiên làm việc của bạn sẽ kết thúc.",
            icon: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Đăng xuất',
            cancelButtonText: 'Hủy'
        }).then((result) => {
            if (result.isConfirmed) {
                window.location.href = logoutBtn.getAttribute("href");
            }
        });
    });
});
//end logout
