/// <reference path="../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    window.wallpaper = function (imgId, url) {
        function fun() {
            var e = $(imgId);
            if (e) {
                if (!url) {
                    url = e.attr("src");
                }
                if (url) {
                    e.show();
                }
                var cw, ch, iw, ih, img;
                cw = $(window).width();
                ch = $(window).height();
                img = new Image();
                // url 是你的图片地址
                img.src = url;
                img.onload = function () {
                    iw = img.width;
                    ih = img.height;
                    if (cw / ch >= iw / ih) {
                        var new_h = parseInt(cw / iw * ih, 10);
                        var imgTop = parseInt((ch - new_h) / 2, 10);
                        // 假设图片的 id 为 J-img
                        e.css({
                            "width": cw + "px",
                            "height": new_h + "px",
                            "top": imgTop + "px",
                            "left": ""
                        });
                    }
                    if (cw / ch < iw / ih) {
                        var new_w = parseInt(ch / ih * iw, 10);
                        var imgLeft = parseInt((cw - new_w) / 2, 10);
                        // 假设图片的 id 为 J-img
                        e.css({
                            "width": new_w + "px",
                            "height": ch + "px",
                            "top": "",
                            "left": imgLeft + "px"
                        });
                    }
                    e.attr("src", url);
                }
            }
        }
        fun();
        $("body").css({ position: "relative", overflow: "hidden" });
        $(imgId).css({ position: "absolute" });
        $(window).resize(fun);
    }
    $().ready(function () {
        $("#btnSignIn").click(function () {
            doLogin();
            return false;
        });
        $("#LoginName").keydown(onKeydown);
        $("#Password").keydown(onKeydown);
        wallpaper("#bgimg");

        loginResize();
        $(window).resize(loginResize);
    });

    function loginResize() {
        var login = $("#content");
        var cw, ch, iw, ih;
        cw = $(window).width();
        ch = $(window).height();
        iw = login.width();
        ih = login.height();
        login.css({
            left: parseInt((cw - iw) * 0.5) + "px",
            top: parseInt((ch - ih) * 0.20) + "px"
        });
        login.show();
    }

    function onKeydown(event) {
        if (event.which == 13) {
            doLogin();
        }
    }
    function logined() {
        window.location.href = "/";
    }
    function doLogin() {
        var loginName = $("#LoginName").val();
        var password = $("#Password").val();
        if (!loginName) {
            alert("登录名是必须的");
        }
        else if (!password) {
            alert("密码是必须的");
        }
        else {
            var data = { LoginName: loginName, Password: password };
            if ($("#RememberMe").get(0).checked) {
                data.rememberMe = $("#RememberMe").val();
            }
            setTimeout(function () {
                $("#loading").show();
            }, 1000);
            $.post("/Ac/Account/SignIn", data, function (result) {
                $("#loading").hide();
                if (result.success) {
                    logined();
                }
                else
                {
                    $('.not-registered').html(result.msg);
                    //alert(result.msg);
                }
            }, "json");
        }
        return false;
    }
})(window);