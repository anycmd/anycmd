/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window) {
    mini.namespace("Ac.AnyLog.Index");
    var self = Ac.AnyLog.Index;
    self.search = search;
    self.prifix = "Ac_AnyLog_Index_";
    self.sortUrl = bootPATH + "../Ac/AnyLog/UpdateSortCode";
    self.help = { appSystemCode: "Anycmd", areaCode: "Ac", resourceCode: "AnyLog", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    self.gridReload = function () {
        grid.reload();
    };
    mini.namespace("AnyLog.Edit");
    var edit = AnyLog.Edit;
    edit.prifix = "Ac_AnyLog_Index_Edit_";

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Ac/AnyLog/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "AnyLog.Details"
        }
    };
    self.filters = {
        Machine: {
            type: 'string',
            comparison: 'like'
        },
        Process: {
            type: 'string',
            comparison: 'like'
        },
        BaseDirectory: {
            type: 'string',
            comparison: 'like'
        },
        Req_MessageType: {
            type: 'string',
            comparison: 'like'
        },
        Req_Verb: {
            type: 'string',
            comparison: 'like'
        },
        Req_Ontology: {
            type: 'string',
            comparison: 'like'
        },
        Res_ReasonPhrase: {
            type: 'string',
            comparison: 'like'
        },
        Res_Description: {
            type: 'string',
            comparison: 'like'
        }
    };

    mini.parse();

    var btnClear = mini.get(self.prifix + "btnClear");
    if (btnClear) {
        btnClear.on("click", function () {
            mini.confirm("确定清空运行日志吗？", "确定？", function (action) {
                if (action == "ok") {
                    grid.loading("操作中，请稍后......");
                    $.post(bootPATH + "../Ac/AnyLog/ClearAnyLog", null, function (result) {
                        helper.response(result, function () {
                            grid.reload();
                        }, function () {
                            grid.unmask();
                        });
                    }, "json");
                }
            });
        });
    }
    var tabs1 = mini.get(self.prifix + "tabs1");
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", ondrawcell);
    var helperDrawcell = helper.ondrawcell(self, "Ac.AnyLog.Index");
    grid.on("load", helper.onGridLoad);
    search();

    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var columnName = e.column.name;
        if (field) {
            switch (field) {
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
            grid.sortBy("CreateOn", "desc");
        }
        grid.load(data);
    }

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Ac/AnyLog/Edit",
        bootPATH + "../Ac/AnyLog/Edit",
        bootPATH + "../Ac/AnyLog/DeleteAnyLog",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);
})(window);
