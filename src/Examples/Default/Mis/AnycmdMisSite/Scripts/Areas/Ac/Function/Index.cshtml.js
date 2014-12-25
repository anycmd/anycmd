/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window) {
    mini.namespace("Ac.Function.Index");
    var self = Ac.Function.Index;
    self.search = search;
    self.loadData = loadData;
    self.prifix = "Ac_Function_Index_";
    self.sortUrl = bootPATH + "../Ac/Function/UpdateSortCode";
    self.help = { appSystemCode: "Anycmd", areaCode: "Ac", resourceCode: "Function", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    self.gridReload = function () {
        grid.reload();
    };
    mini.namespace("Function.Edit");
    var edit = Function.Edit;
    edit.prifix = "Ac_Function_Index_Edit_";

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Ac/Function/Details",
            entityTypeCode: 'Function',
            controller: 'Function',
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "Function.Details"
        },
        menuTab: {
            url: bootPATH + "../Ac/Function/Menus",
            params: [{ "pName": 'functionId', "pValue": "Id" }],
            namespace: "Function.Menus"
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
        IsUiView: {
            type: 'boolean',
            comparison: 'eq'
        },
        IsManaged: {
            type: 'boolean',
            comparison: 'eq'
        },
        ResourceCode: {
            type: 'string',
            comparison: 'like'
        },
        IsEnabled: {
            type: 'numeric',
            comparison: 'eq'
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

    var btnClearCache = mini.get(self.prifix + "btnClearCache");
    if (btnClearCache) {
        btnClearCache.on("click", function () {
            mini.confirm("暂时支持的是导入本系统的功能列表，数据源是通过反射控制器层得到的，<br />后续支持以xml文件的方式导入其它系统的功能列表？", "确定导入功能吗？", function (action) {
                if (action == "ok") {
                    $.post(bootPATH + "../Ac/Function/Refresh", null, function (result) {
                        helper.response(result);
                        grid.reload();
                    }, "json");
                }
            });
        });
    }
    var btnManage = mini.get(self.prifix + "btnManage");
    if (btnManage) {
        btnManage.on("click", function () {
            if (grid.getSelected()) {
                var id;
                var records = grid.getSelecteds();
                var ids = [];
                for (var i = 0, l = records.length; i < l; i++) {
                    var r = records[i];
                    ids.push(r.Id);
                }
                id = ids.join(',');
                mini.confirm("确定托管吗？", "确定？", function (action) {
                    if (action == "ok") {
                        $.post(bootPATH + "../Ac/Function/Manage", { id: id }, function (result) {
                            helper.response(result);
                            grid.reload();
                        }, "json");
                    }
                });
            }
        });
    }
    var btnUnManage = mini.get(self.prifix + "btnUnManage");
    if (btnUnManage) {
        btnUnManage.on("click", function () {
            if (grid.getSelected()) {
                var id;
                var records = grid.getSelecteds();
                var ids = [];
                for (var i = 0, l = records.length; i < l; i++) {
                    var r = records[i];
                    ids.push(r.Id);
                }
                id = ids.join(',');
                mini.confirm("确定取消托管吗？", "确定？", function (action) {
                    if (action == "ok") {
                        $.post(bootPATH + "../Ac/Function/UnManage", { id: id }, function (result) {
                            helper.response(result);
                            grid.reload();
                        }, "json");
                    }
                });
            }
        });
    }
    var win = mini.get(edit.prifix + "win1");
    var form;
    if (win) {
        form = new mini.Form(edit.prifix + "form1");
    }

    var tabs1 = mini.get(self.prifix + "tabs1");
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", ondrawcell);
    var helperDrawcell = helper.ondrawcell(self, "Ac.Function.Index");
    grid.on("load", helper.onGridLoad);
    search();

    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var columnName = e.column.name;
        if (field) {
            switch (field) {
                case "IsUiView":
                case "IsManaged":
                    if (value == "已托管" || value == "1" || value == "是") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "未托管" || value == "0" || vlaue == '否') {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
            }
        }
        helperDrawcell(e);
    };

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
        var data = {};
        var filterArray = [];
        for (var k in self.filters) {
            var filter = self.filters[k];
            if (filter.value) {
                filterArray.push({ field: k, type: filter.type, comparison: filter.comparison, value: filter.value });
            }
        }
        data.filters = JSON.stringify(filterArray);
        if (!grid.sortField) {
            grid.sortBy("SortCode", "asc");
        }
        grid.load(data);
    }

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Ac/Function/Edit",
        bootPATH + "../Ac/Function/Edit",
        bootPATH + "../Ac/Function/Delete",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Ac/Function/Create",
        bootPATH + "../Ac/Function/Update",
        bootPATH + "../Ac/Function/Get",
        form, edit);
})(window);
