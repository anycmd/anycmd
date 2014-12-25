/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Ontology.NodeCares");
    var self = Ontology.NodeCares;
    self.prifix = "Edi_Node_OntologyNodeCares_";
    self.loadData = loadData;

    mini.parse();

    var btnSave = mini.get(self.prifix + "btnSave");
    if (btnSave) {
        btnSave.on("click", saveData);
    }
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", ondrawcell);
    grid.on("load", helper.onGridLoad);
    loadData();

    function loadData() {
        if (!grid.sortField) {
            grid.sortBy("SortCode", "asc");
        }
        var data = getParams();
        grid.load(data);
    }

    function getParams() {
        var data = { };
        if (self.params && self.params.ontologyId) {
            data.ontologyId = self.params.ontologyId;
        }
        else {
            data.ontologyId = $.deparam.fragment().ontologyId || $.deparam.querystring().ontologyId;
        }
        return data;
    }

    function saveData() {
        var data = grid.getChanges();
        var json = mini.encode(data);

        grid.loading("保存中，请稍后......");
        $.ajax({
            url: bootPATH + "../Edi/Ontology/AddOrRemoveNodes",
            data: { data: json },
            type: "post",
            success: function (result) {
                helper.response(result, function () {
                    grid.reload();
                }, function () {
                    grid.unmask();
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                grid.unmask();
                mini.alert(jqXHR.responseText);
            }
        });
    }

    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "IsEnabled":
                    if (value == true || value == "正常" || value == "1") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == false || value == "禁用" || value == "0") {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
                case "Name":
                    var url = bootPATH + "../Edi/Node/Details?isTooltip=true&id=" + record.NodeId
                    e.cellHtml = "<a href='" + url + "' onclick='helper.cellTooltip(this);return false;' rel='" + url + "'>" + value + "</a>";
                    break;
                case "Icon":
                    if (value) {
                        e.cellHtml = "<img src='" + bootPATH + "../Content/icons/16x16/" + value + "'></img>";
                    }
                    break;
            }
        }
    }
})(window);