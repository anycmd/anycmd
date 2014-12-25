/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window) {
    mini.namespace("Ac.ExceptionLog.Index");
    var self = Ac.ExceptionLog.Index;
    self.search = search;
    self.prifix = "Ac_ExceptionLog_Index_";
    self.help = { appSystemCode: "Anycmd", areaCode: "Ac", resourceCode: "ExceptionLog", functionCode: "Index" };
    helper.helperSplitterInOne(self);

    window.gridReload = function () {
        grid.reload();
    };
    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Ac/ExceptionLog/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "ExceptionLog.Details"
        }
    };

    mini.parse();

    var btnClear = mini.get(self.prifix + "btnClear");
    if (btnClear) {
        btnClear.on("click", function () {
            mini.confirm("确定清空异常日志吗？", "确定？", function (action) {
                if (action == "ok") {
                    grid.loading("操作中，请稍后......");
                    $.post(bootPATH + "../Ac/ExceptionLog/ClearExceptionLog", null, function (result) {
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
    var comboLevel = mini.get(self.prifix + "comboLevel");
    comboLevel.on("valuechanged", search);
    var key = mini.get(self.prifix + "key");
    key.on("enter", search);
    var tabs1 = mini.get(self.prifix + "tabs1");
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", helper.ondrawcell(self, "Ac.ExceptionLog.Index"));
    grid.on("load", helper.onGridLoad);
    grid.load();
    grid.sortBy("LogOn", "desc");

    helper.index.allInOne(
        null,
        grid,
        bootPATH + "../Ac/ExceptionLog/Edit",
        bootPATH + "../Ac/ExceptionLog/Edit",
        bootPATH + "../Ac/ExceptionLog/Delete",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    function search() {
        var data = { key: key.getValue(), level: comboLevel.getValue() };
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
})(window);