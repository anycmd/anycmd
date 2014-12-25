/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window, $) {
    mini.namespace("Ac.VisitingLog.Index");
    var self = Ac.VisitingLog.Index;
    self.prifix = "Ac_VisitingLog_Index_";
    self.search = search;
    self.help = { appSystemCode: "Anycmd", areaCode: "Ac", resourceCode: "VisitingLog", functionCode: "Index" };
    helper.helperSplitterInOne(self);

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Ac/VisitingLog/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "VisitingLog.Details"
        }
    };

    mini.parse();

    var win = mini.get("win1");
    var key = mini.get(self.prifix + "key");
    key.on("valuechanged", search);
    var leftVisitOn = mini.get(self.prifix + "leftVisitOn");
    leftVisitOn.on("valuechanged", search);
    leftVisitOn.setMinDate(new Date(2013, 1, 1, 0, 0, 0, 0));
    leftVisitOn.setMaxDate(new Date());
    var rightVisitOn = mini.get(self.prifix + "rightVisitOn");
    rightVisitOn.on("valuechanged", search);
    rightVisitOn.setMinDate(new Date(2013, 1, 1, 0, 0, 0, 0));
    var d = new Date();
    rightVisitOn.setMaxDate(new Date(d.getFullYear(), d.getMonth(), d.getDate() + 1, 0, 0, 0, 0));
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", ondrawcell);
    grid.on("load", helper.onGridLoad);
    grid.load();
    grid.sortBy("VisitOn", "desc");

    helper.index.allInOne(
        null,
        grid,
        bootPATH + "../Ac/VisitingLog/Edit",
        bootPATH + "../Ac/VisitingLog/Edit",
        bootPATH + "../Ac/VisitingLog/DeleteVisitingLog",
        self);

    function search() {
        var data = { key: key.getValue(), leftVisitOn: leftVisitOn.getFormValue(), rightVisitOn: rightVisitOn.getFormValue() };
        if (data.leftVisitOn && data.rightVisitOn) {
            if (data.leftVisitOn > data.rightVisitOn) {
                mini.alert("左时间戳不能大于右时间戳");
                return;
            }
        }
        grid.load(data);
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
                case "IsEnabled":
                    if (value == "正常" || value == "1") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "禁用" || value == "0") {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
                case "LoginName":
                    if (value) {
                        if (record.AccountId) {
                            var url = bootPATH + "../Ac/Account/Details?isTooltip=true&id=" + record.AccountId
                            e.cellHtml = "<a href='" + url + "' onclick='helper.cellTooltip(this);return false;' rel='" + url + "'>" + value + "</a>";
                        }
                        else {
                            e.cellHtml = value;
                        }
                    }
                    break;
            }
        }
    };
})(window, jQuery);