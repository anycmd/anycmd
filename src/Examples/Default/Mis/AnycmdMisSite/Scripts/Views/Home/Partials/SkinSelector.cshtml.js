/// <reference path="../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../miniui/miniui.js" />
/// <reference path="../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    $().ready(function () {
        $("#selectSkin").change(function () {
            onSkinChange($(this).val());
        });
        var skin = getCookie("miniuiSkin");
        if (skin) {
            onSkinChange(skin);
            $("#selectSkin").val(skin);
        }
        else {
            onSkinChange($("#selectSkin").val());
        }
    });
})(window);