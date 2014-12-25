/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("InfoRule.ElementInfoRules");
    var self = InfoRule.ElementInfoRules;
    self.prifix = "Edi_InfoRule_ElementInfoRules_";
    self.loadData = loadData;

    mini.parse();

    var dgInfoRule = mini.get(self.prifix + "dgInfoRule");

    dgInfoRule.on("drawcell", ondrawcell);
    dgInfoRule.on("load", helper.onGridLoad);

    function loadData() {
        if (!dgInfoRule.sortField) {
            dgInfoRule.sortBy("SortCode", "asc");
        }
        dgInfoRule.load(getParams());
    }

    function getParams() {
        if (self.params && self.params.elementId) {
            return self.params;
        }
        else {
            return { elementId: $.deparam.fragment().elementId || $.deparam.querystring().elementId }
        }
    }

    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "IsEnabled":
                case "InfoRuleIsEnabled":
                    if (value == "正常" || value == "1") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "禁用" || value == "0") {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
                case "Title":
                    var url = bootPATH + "../Edi/InfoRule/Details?isTooltip=true&id=" + record.InfoRuleId;
                    e.cellHtml = "<a href='" + url + "' onclick='helper.cellTooltip(this);return false;' rel='" + url + "'>" + value + "</a>";
                    break;
            }
        }
    }
})(window);