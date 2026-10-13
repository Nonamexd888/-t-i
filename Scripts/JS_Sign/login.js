document.getElementById("loginForm").onsubmit = function () {

    var email = document.getElementById("email").value;
    var password = document.getElementById("password").value;

    var message = document.getElementById("message");


    // Kiểm tra email
    if (email == "") {

        message.innerHTML = "Vui lòng nhập email";
        message.style.color = "red";

        return false;
    }


    // Kiểm tra định dạng email
    if (email.indexOf("@") == -1 || email.indexOf(".") == -1) {

        message.innerHTML = "Email không đúng định dạng";
        message.style.color = "red";

        return false;
    }


    // Kiểm tra mật khẩu
    if (password == "") {

        message.innerHTML = "Vui lòng nhập mật khẩu";
        message.style.color = "red";

        return false;
    }


    // Đăng nhập thành công
    message.innerHTML = "Đăng nhập thành công!";
    message.style.color = "green";

    return false;
};