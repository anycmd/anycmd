/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../jquery-tmpl/jquery.tmpl.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Command.EntityHandleFailingCommands");
    var self = Command.EntityHandleFailingCommands;
    self.prifix = "Edi_Command_EntityHandleFailingCommands_";
    self.loadData = loadData;
    var ontologyCode = $.deparam.fragment().ontologyCode || $.deparam.querystring().ontologyCode;

    mini.parse();

    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", ondrawcell);
    grid.on("load", helper.onGridLoad);

    function loadData() {
        if (!grid.sortField) {
            grid.sortBy("CreateOn", "desc");
        }
        grid.load(getParams());
    }

    function getParams() {
        var data = { ontologyCode: ontologyCode };
        if (self.params && self.params.entityId) {
            data.entityId = self.params.entityId;
        }
        else {
            data.entityId = $.deparam.fragment().entityId || $.deparam.querystring().entityId;
        }
        return data;
    }

    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "ClientName":
                    if (value) {
                        if (record.IsSelf) {
                            value = "<span title='自己' class='icon-pkey width16px'></span>" + value;
                        }
                        var url = bootPATH + "../Edi/Node/Details?isTooltip=true&id=" + record.ClientId
                        e.cellHtml = "<a href='" + url + "' onclick='helper.cellTooltip(this);return false;' rel='" + url + "'>" + value + "</a>";
                    }
                    break;
                case "LoginName":
                    if (value) {
                        var url = bootPATH + "../Ac/Account/Details?isTooltip=true&id=" + record.AccountId
                        e.cellHtml = "<a href='" + url + "' onclick='helper.cellTooltip(this);return false;' rel='" + url + "'>" + value + "</a>";
                    }
                    else {
                        e.cellHtml = "外部系统";
                    }
                    break;
            }
        }
    }
})(window);