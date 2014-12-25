/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Ontology.Elements");
    var self = Ontology.Elements;
    self.loadData = loadData;

    mini.parse();

    var dgElement = mini.get("dgElement");

    dgElement.on("drawcell", ondrawcell);
    dgElement.on("load", helper.onGridLoad);

    function loadData() {
        if (!dgElement.sortField) {
            dgElement.sortBy("SortCode", "asc");
        }
        dgElement.load(getParams());
    }

    function getParams() {
        if (self.params && self.params.ontologyId) {
            return self.params;
        }
        else {
            return { ontologyId: $.deparam.fragment().ontologyId || $.deparam.querystring().ontologyId }
        }
    }

    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "IsEnabled":
                case "IsAudit":
                    if (value == "正常" || value == "1" || value == "true" || value == "是") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "禁用" || value == "0" || value == "false" || value == "否") {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
                case "Icon":
                    if (value) {
                        e.cellHtml = "<img src='" + bootPATH + "../Content/icons/16x16/" + value + "'></img>";
                    }
                    break;
                case "Name":
                    var url = bootPATH + "../Edi/Element/Details?isTooltip=true&id=" + record.Id
                    e.cellHtml = "<a href='" + url + "' onclick='helper.cellTooltip(this);return false;' rel='" + url + "'>" + value + "</a>";
                    break;
            }
        }
    }
})(window);