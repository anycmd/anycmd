/// <reference path="../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../miniui/miniui.js" />
/// <reference path="../../jquery-bbq/jquery.ba-bbq.js" />
(function (window, $) {
    KindEditor.ready(function (K) {
        var editor = K.create('#txtTooltip', {
            allowFileManager: true,
            width: "100%",
            height: (document.body.clientHeight - 100) + "px",
            uploadJson: bootPATH + '../Scripts/kindeditor/asp.net/upload_json.ashx',
            fileManagerJson: bootPATH + '../Scripts/kindeditor/asp.net/file_manager_json.ashx'
        });
        $(window).resize(function () {
            editor.edit.setHeight((document.body.clientHeight - 160) + "px");
        });
        $(".btnOk").click(function () {
            hdTooltip.setValue(editor.html());
            var o = $("#form1").serialize();
            form.validate();
            if (form.isValid() == false) return;
            var url = bootPATH + "../Ac/UiView/SaveTooltip";
            $.ajax({
                url: url,
                type: "post",
                data: o,
                cache: false,
                success: function (result) {
                    helper.response(result, function () {
                        mini.alert("保存成功");
                    }, function () {
                        mini.alert(result.msg);
                    });
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    alert(jqXHR.responseText);
                }
            });
        });
    });

    mini.parse();

    var win1 = mini.get("win1");
    var form = new mini.Form("form1");
    var hdTooltip = mini.get("hdTooltip");
    win1.show();
    win1.max();
})(window, jQuery);
