/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.parse();

    var loginWindow = mini.get("loginWindow");
    var isHomeLogInWin = loginWindow.showCloseButton;
    if (!isHomeLogInWin) {
        if (loginWindow) {
            loginWindow.show();
        }
    }

    window.onresize = function () {
        if (loginWindow && loginWindow.visible) {
            loginWindow.show();
        }
    }
    var form = new mini.Form("#loginWindow");
    var loginName = mini.get("LoginName");
    var password = mini.get("Password");
    loginName.on("enter", doLogin);
    password.on("enter", doLogin);

    $().ready(function () {
        $("#btnSignIn").click(doLogin);
        $("#btnReset").click(function () {
            form.clear();
        });
        $("#retrieve").click(function () {
            mini.alert("暂不支持密码自主找回，请联系管理员修改密码");
            return false;
        });
        wallpaper("#bgimg");
    });
    function logined() {
        var isHomeLogOnPage = window.isHomeLogOnPage;
        if (isHomeLogOnPage) {
            window.location.href = bootPATH + "../";
        }
        else {
            if (window.top.onLogined) {
                window.top.onLogined();
            }
            if (isHomeLogInWin) {
                if (loginWindow) {
                    loginWindow.hide();
                }
            }
            else {
                window.location.reload();
            }
        }
    }
    function doLogin() {
        form.validate();
        if (form.isValid() == false) return;
        helper.requesting();
        var data = { LoginName: loginName.getValue(), Password: password.getValue() };
        $.post(bootPATH + "../Ac/Account/SignIn", data, function (result) {
            if (result.success) {
                logined();
            }
            else {
                helper.responsed();
                mini.alert(result.msg);
            }
            helper.responsed();
        }, "json");
    }
})(window);