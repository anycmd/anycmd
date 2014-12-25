/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
// 接口：edit、remove
(function (window) {
    mini.namespace("Ac.UiView.Index");
    var self = Ac.UiView.Index;
    self.prifix = "Ac_UiView_Index_";
    self.sortUrl = bootPATH + "../Ac/UiView/UpdateSortCode";
    self.add = add;
    self.search = search;
    self.loadData = loadData;
    self.help = { appSystemCode: "Anycmd", areaCode: "Ac", resourceCode: "AppSystem", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    self.gridReload = function () {
        grid.reload();
    };
    mini.namespace("UiView.Edit");
    var edit = UiView.Edit;
    edit.prifix = "Ac_UiView_Index_Edit_";
    edit.SetData = SetData;
    var faceInitialized = false;

    self.areaFilters = {};

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Ac/UiView/Details",
            entityTypeCode: 'UiView',
            controller: 'UiView',
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "UiView.Details"
        },
        uiViewButtonTab: {
            url: bootPATH + "../Ac/UiView/UiViewButtons",
            params: [
                { "pName": 'uiViewId', "pValue": "Id" },
                { "pName": 'appSystemId', "pValue": "AppSystemId" },
                { "pName": 'controller', "pValue": "ResourceCode" }
            ],
            namespace: "UiView.UiViewButtons"
        },
        operationLogTab: {
            url: bootPATH + "../Ac/OperationLog/Index",
            params: [{ "pName": 'targetId', "pValue": "Id" }],
            namespace: "Ac.OperationLog.Index"
        }
    };
    self.filters = {
        Description: {
            type: 'string',
            comparison: 'like'
        },
        ResourceCode: {
            type: 'string',
            comparison: 'like'
        },
        Code: {
            type: 'string',
            comparison: 'like'
        },
        DeveloperId: {
            type: 'guid',
            comparison: 'eq'
        }
    };

    mini.parse();

    var win = mini.get(edit.prifix + "win1");
    var form;
    if (win) {
        form = new mini.Form(edit.prifix + "form1");
    }

    var tabs1 = mini.get(self.prifix + "tabs1");
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("selectionchanged", function (e) {
        var record = e.sender.getSelected();
        if (record) {
            tabs1.show();
        }
    });
    grid.on("drawcell", ondrawcell);
    var helperDrawcell = helper.ondrawcell(self, "Ac.UiView.Index");
    grid.on("load", function (e) {
        var record = e.sender.getSelected();
        if (!record) {
            tabs1.hide();
        }
        else {
            tabs1.show();
        }
        helper.onGridLoad(e);
    });
    function add() {
        var viewRecord = grid.getSelected();
        var data = { action: "new", AppSystemId: getParams().appSystemId };
        grid.loading("使劲加载中...");
        helper.index.winReady(edit, function (win) {
            win.setTitle("添加");
            win.setIconCls("icon-add");
            win.show();
            edit.SetData(data);
            grid.unmask();
        });
    }

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Ac/UiView/Edit",
        bootPATH + "../Ac/UiView/Edit",
        bootPATH + "../Ac/UiView/Delete",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    function loadData() {
        search();
    }

    function getParams() {
        data = {};
        if (self.params && self.params.appSystemId) {
            data.appSystemId = self.params.appSystemId;
        }
        else {
            data.appSystemId = $.deparam.fragment().appSystemId || $.deparam.querystring().appSystemId
        }
        return data;
    }

    function search() {
        var data = getParams()
        var filterArray = [];
        for (var k in self.filters) {
            var filter = self.filters[k];
            if (filter.value) {
                filterArray.push({ field: k, type: filter.type, comparison: filter.comparison, value: filter.value });
            }
        }
        filterArray.push({ field: 'AppSystemId', type: 'guid', comparison: 'eq', value: data.appSystemId });
        data.filters = JSON.stringify(filterArray);
        if (!grid.sortField) {
            grid.sortBy("ResourceCode", "asc");
        }
        grid.load(data, function () {
            var record = grid.getSelected();
            if (!record) {
                tabs1.hide();
            }
        });
    }

    function onKeyEnter(e) {
        search();
    }
    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "Icon":
                    e.cellHtml = "<img src='" + bootPATH + "../Content/icons/16x16/" + (value || "default.png") + "'></img>";
                    break;
            }
        }
        helperDrawcell(e);
    }
    function SetData(data) {
        //跨页面传递的数据对象，克隆后才可以安全使用
        data = mini.clone(data);
        if (data.action == "edit") {
            $.ajax({
                url: bootPATH + "../Ac/UiView/Get",
                data: { id: data.id },
                cache: false,
                success: function (result) {
                    helper.response(result, function () {
                        form.setData(result);
                        form.validate();
                        if (result.Icon) {
                            $("#msg").html("<img style='margin-top:3px;' src='" + bootPATH + "../Content/icons/16x16/" + result.Icon + "' alt='图标' />");
                        } else {
                            $("#msg").html("");
                        }
                    });
                }
            });
        }
        else if (data.action == "new") {
            form.setData(data);
        }
        if (!faceInitialized) {
            $.getJSON(bootPATH + "../Home/GetIcons", null, function (result) {
                $("#message_face").jqfaceedit({ txtAreaObj: $("#msg"), containerObj: $(mini.get('faceWindow').getBodyEl()), emotions: result, top: 25, left: -27, width: 658, height: 420 });
            });
            faceInitialized = true;
        }
    }

    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Ac/UiView/Create",
        bootPATH + "../Ac/UiView/Update",
        bootPATH + "../Ac/UiView/Get",
        form, edit);
})(window);