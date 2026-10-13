document.getElementById("registerForm").onsubmit = function () {

    var fullname = document.getElementById("fullname").value;
   
    var email = document.getElementById("email").value;


    var password = document.getElementById("password").value;

    var repassword = document.getElementById("repassword").value;

    var agree = document.getElementById("agree").checked;

    var message = document.getElementById("message");


    // Kiểm tra họ và tên
    if (fullname == "") {

        message.innerHTML = "Vui lòng nhập họ và tên";
        message.style.color = "red";

        return false;
    }
   
    

    // Kiểm tra email
    if (email == "") {

        message.innerHTML = "Vui lòng nhập email";
        message.style.color = "red";

        return false;
    }
    // Kiểm tra định dạng email
    // Nếu email không có @hoặc không có. → báo email sai định dạng.

    if (email.indexOf("@") == -1 || email.indexOf(".") == -1) {

         message.innerHTML = "Email không đúng định dạng";
        message.style.color = "red";

        return false;
    }

    // Kiểm tra mật khẩu trống
    if (password == "") {

        message.innerHTML = "Vui lòng nhập mật khẩu";
        message.style.color = "red";

        return false;
    }


    // Kiểm tra độ dài mật khẩu
    if (password.length < 6) {

        message.innerHTML = "Mật khẩu phải có ít nhất 6 ký tự";
        message.style.color = "red";

        return false;
    }


    // Kiểm tra nhập lại mật khẩu
    if (repassword == "") {

        message.innerHTML = "Vui lòng nhập lại mật khẩu";
        message.style.color = "red";

        return false;
    }


    // Kiểm tra 2 mật khẩu có giống nhau không
    if (password != repassword) {

        message.innerHTML = "Mật khẩu nhập lại không đúng";
        message.style.color = "red";

        return false;
    }


    // Kiểm tra điều khoản
    if (agree == false) {

        message.innerHTML = "Vui lòng đồng ý với điều khoản";
        message.style.color = "red";

        return false;
    }


    // Nếu tất cả đều đúng
    message.innerHTML = "Đăng ký thành công!";
    message.style.color = "green";

    return false;

};