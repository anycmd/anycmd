/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    var ontologyCode = $.deparam.fragment().ontologyCode || $.deparam.querystring().ontologyCode;

    mini.namespace("Entity.Edit");
    var self = Entity.Edit;
    self.loadData = loadData;

    mini.parse();

    var form = new mini.Form("form1");
    var btnSave = mini.get("Edi_Entity_Edit_btnSave");
    if (btnSave) {
        btnSave.on("click", SaveData);
    }

    labelModel();
    function labelModel() {
        var fields = form.getFields();
        for (var i = 0, l = fields.length; i < l; i++) {
            var c = fields[i];
            if ($(c.el).hasClass("asLabel")) {
                if (c.setReadOnly) c.setReadOnly(true);     //只读
                if (c.setIsValid) c.setIsValid(true);      //去除错误提示
                if (c.addCls) c.addCls("asLabel");          //增加asLabel外观
            }
        }
    }
    // Make sure to only match links to wikipedia with a rel tag
    $('a.elementTooltip[rel]').each(function () {
        // We make use of the .each() loop to gain access to each element via the "this" keyword...
        $(this).qtip(
        {
            content: {
                text: '<img class="throbber" src="/Content/img/throbber.gif" alt="Loading..." />',
                ajax: {
                    url: $(this).attr('rel') // Use the rel attribute of each element for the url to load
                },
                title: {
                    text: '帮助 - ' + $(this).attr('title'),
                    button: '关闭'
                }
            },
            position: {
                at: 'bottom center', // Position the tooltip above the link
                my: 'top center',
                viewport: $(window), // Keep the tooltip on-screen at all times
                effect: false // Disable positioning animation
            },
            show: {
                event: 'click',
                solo: true // Only show one tooltip at a time
            },
            hide: 'unfocus',
            style: {
                classes: 'qtip-elementHelp qtip-shadow'
            }
        });
    }).click(function (event) { event.preventDefault(); });

    function loadData() {
        var data = getParams();
        if (data["id"] && data["id"] != "-1") {
            data.action = 'edit';
        }
        else {
            data.action = 'new';
        }
        SetData(data);
    }

    function getParams() {
        var data = {};
        if (self.params && self.params.id) {
            data.id = self.params.id;
            data.ZZJGM = self.params.catalogCode;
        }
        else {
            data.id = $.deparam.fragment().id || $.deparam.querystring().id;
            data.ZZJGM = $.deparam.fragment().catalogCode || $.deparam.querystring().catalogCode;
        }
        return data;
    }

    function SaveData() {
        var data = $("#form1").serialize();

        form.validate();
        if (form.isValid() == false) return;
        // 在此可以比较name带__前缀的input和不带前缀的input的值是否不同，从而得知表单是否有变化，
        // 但发现使用javascript检测表单是否有变化性能太差。
        helper.requesting();
        $.post(bootPATH + "../Edi/Entity/AddOrUpdate?ontologyCode=" + ontologyCode, data, function (result) {
            helper.responsed();
            helper.response(result, function () {
                SetData({ action: "edit", id: result.id });
                Edi.Entity.Index.gridReload();
            });
        }, "json");
    }

    ////////////////////
    //标准方法接口定义
    function SetData(data) {
        //跨页面传递的数据对象，克隆后才可以安全使用
        data = mini.clone(data);
        if (data.action == "edit") {
            $.ajax({
                url: bootPATH + "../Edi/Entity/Get?ontologyCode=" + ontologyCode + "&id=" + data.id,
                cache: false,
                success: function (result) {
                    helper.response(result, function () {
                        form.setData(result);
                        var fields = form.getFields();
                        for (var i = 0, l = fields.length; i < l; i++) {
                            var c = fields[i];
                            if (c.name.substring(0, 2) != "__") {
                                for (var j = 0; j < l; j++) {
                                    var f = fields[j];
                                    if (f.name == "__" + c.name) {
                                        f.setValue(c.getValue());
                                    }
                                }
                            }
                        }
                        form.validate();
                    });
                }
            });
        }
        else if (data.action == "new") {
            form.setData(data);
        }
    }
})(window);