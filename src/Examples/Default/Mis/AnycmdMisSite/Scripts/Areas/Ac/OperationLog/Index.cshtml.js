/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
// 接口：edit、remove
(function (window, $) {
    mini.namespace("Ac.OperationLog.Index");
    var self = Ac.OperationLog.Index;
    self.prifix = "Ac_OperationLog_Index_"
    self.help = { appSystemCode: "Anycmd", areaCode: "Ac", resourceCode: "OperationLog", functionCode: "Index" };
    self.loadData = loadData;
    helper.helperSplitterInOne(self);

    mini.parse();

    var key = mini.get(self.prifix + "key");
    key.on("valuechanged", loadData);
    var leftCreateOn = mini.get(self.prifix + "leftCreateOn");
    leftCreateOn.on("valuechanged", loadData);
    leftCreateOn.setMinDate(new Date(2013, 1, 1, 0, 0, 0, 0));
    leftCreateOn.setMaxDate(new Date());
    var rightCreateOn = mini.get(self.prifix + "rightCreateOn");
    rightCreateOn.on("valuechanged", loadData);
    rightCreateOn.setMinDate(new Date(2013, 1, 1, 0, 0, 0, 0));
    var d = new Date();
    rightCreateOn.setMaxDate(new Date(d.getFullYear(), d.getMonth(), d.getDate() + 1, 0, 0, 0, 0));
    var btnSearch = mini.get(self.prifix + "btnSearch");
    btnSearch.on("click", function () {
        loadData();
    });
    var grid = mini.get(self.prifix + "datagrid1");

    grid.on("drawcell", ondrawcell);
    grid.on("load", helper.onGridLoad);

    loadData();

    function loadData() {
        var data = {
            leftCreateOn: leftCreateOn.getFormValue(),
            rightCreateOn: rightCreateOn.getFormValue(),
            key: key.getValue()
        };
        if (data.leftCreateOn && data.rightCreateOn) {
            if (data.leftCreateOn > data.rightCreateOn) {
                mini.alert("左时间戳不能大于右时间戳");
                return;
            }
        }
        data.targetId = getParams().targetId;
        if (!grid.sortField) {
            grid.sortBy("CreateOn", "desc");
        }

        grid.load(data);
    }

    function getParams() {
        data = {};
        if (self.params && self.params.targetId) {
            data.targetId = self.params.targetId;
        }
        else {
            data.targetId = $.deparam.fragment().targetId || $.deparam.querystring().targetId
        }
        return data;
    }
    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "LoginName":
                    if (value) {
                        var url = bootPATH + "../Ac/Account/Details?isTooltip=true&id=" + record.AccountId
                        e.cellHtml = "<a href='" + url + "' onclick='helper.cellTooltip(this);return false;' rel='" + url + "'>" + value + "</a>";
                    }
                    break;
            }
        }
    }
})(window, jQuery);