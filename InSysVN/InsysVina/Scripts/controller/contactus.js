var contactus = {
    ui: {},
    registerControl: function () {
        $('#submit-message').off('click').on('click', function () {
            contactus.createcontactus();
        });
    },
    createcontactus: function () {
        $.ajax({
            url: '/Home/ContactUs',
            type: 'POST',
            data: {
                fullname: $('#mes-name').val(),
                email: $('#mes-email').val(),
                phone: $('#mes-phone').val(),
                content: $('#mes-text').val()
            },
            success: function (res) {
                if (res) {
                    toastr.success('Gửi liên hệ thành công ! Chúng tôi sẽ sớm liên lạc với bạn', 'Thông báo');
                    setTimeout(function () {
                        window.location.href('/');
                    }, 300);
                }
                else {
                    toastr.error('Gửi liên hệ không thành công ! Chúng tôi rất tiếc vì điều này', 'Thông báo');
                }
            }
        });
    }
};
$(document).ready(function () {
    contactus.registerControl();
})