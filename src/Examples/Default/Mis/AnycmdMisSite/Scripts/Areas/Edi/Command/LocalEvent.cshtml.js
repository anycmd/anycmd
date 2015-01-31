/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window) {
    mini.namespace("Command.LocalEvent");
    var self = Command.LocalEvent;
    self.prifix = "Edi_Command_LocalEvent_";
    var ontologyCode = $.deparam.fragment().ontologyCode || $.deparam.querystring().ontologyCode;
    self.search = search;
    self.gridReload = function () {
        grid.reload();
    };
    self.loadData = loadData;

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Edi/Command/Details?entityTypeCode=LocalEvent",
            entityTypeCode: 'LocalEvent',
            controller: "Command",
            params: [
                { "pName": 'commandType', "pValue": "LocalEvent" },
                { "pName": 'id', "pValue": "Id" },
                { "pName": 'ontologyCode', "pValue": "Ontology" }],
            namespace: "LocalEvent.Details"
        }
    };

    mini.parse();

    var actionCode = mini.get(self.prifix + "actionCode");
    var nodeId = mini.get(self.prifix + "nodeId");
    actionCode.on("valuechanged", search);
    if (nodeId) {
        nodeId.on("valuechanged", search);
    }
    var tabs1 = mini.get(self.prifix + "tabs1");
    var btnAuditApproved = mini.get(self.prifix + "btnAuditApproved");
    if (btnAuditApproved) {
        btnAuditApproved.on("click", function () {
            var records = grid.getSelecteds();
            if (!records || records.length == 0) {
                mini.showTips({
                    content: "没有选中记录",
                    state: "warning",
                    x: "center",
                    y: "top",
                    timeout: 3000
                });
                return;
            }
            approved(records);
        });
    }
    var btnAuditUnapproved = mini.get(self.prifix + "btnAuditUnapproved");
    if (btnAuditUnapproved) {
        btnAuditUnapproved.on("click", function () {
            var records = grid.getSelecteds();
            if (!records || records.length == 0) {
                mini.showTips({
                    content: "没有选中记录",
                    state: "warning",
                    x: "center",
                    y: "top",
                    timeout: 3000
                });
                return;
            }
            unapproved(records);
        });
    }
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", ondrawcell);
    var helperDrawcell = helper.ondrawcell(self, "Command.LocalEvent");
    grid.on("load", helper.onGridLoad);
    grid.load({ ontologyCode: ontologyCode });
    grid.sortBy("TimeStamp", "desc");

    helper.index.allInOne(
        null,
        grid,
        bootPATH + "../Edi/Command/Edit?ontologyCode=" + ontologyCode,
        bootPATH + "../Edi/Command/Edit?ontologyCode=" + ontologyCode,
        bootPATH + "../Edi/Command/DeleteLocalEvent",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    function approved(records) {
        var id;
        if (typeof records == typeof []) {
            var ids = [];
            for (var i = 0, l = records.length; i < l; i++) {
                var r = records[i];
                ids.push(r.Id);
            }
            id = ids.join(',');
            $.post(bootPATH + "../Edi/Command/AuditApproved", { id: id, ontologyCode: ontologyCode }, function (result) {
                if (!result.success) {
                    mini.alert(result.msg);
                }
                else {
                    grid.reload();
                }
            }, "json");
        }
        else {
            $.post(bootPATH + "../Edi/Command/AuditApproved", { id: id, ontologyCode: ontologyCode }, function (result) {
                if (!result.success) {
                    mini.alert(result.msg);
                }
            }, "json");
        }
    }

    function unapproved(records) {
        var id;
        if (typeof records == typeof []) {
            var ids = [];
            for (var i = 0, l = records.length; i < l; i++) {
                var r = records[i];
                ids.push(r.Id);
            }
            id = ids.join(',');
            $.post(bootPATH + "../Edi/Command/AuditUnapproved", { id: id, ontologyCode: ontologyCode }, function (result) {
                if (!result.success) {
                    mini.alert(result.msg);
                }
                else {
                    grid.reload();
                }
            }, "json");
        }
        else {
            mini.confirm("确定删除选中记录？", "确定？", function (action) {
                if (action == "ok") {
                    if (typeof records == "string") {
                        id = records;
                    }
                    else if (records && records.Id) {
                        id = records.Id;
                    }
                    $.post(bootPATH + "../Edi/Command/AuditUnapproved", { id: id, ontologyCode: ontologyCode }, function (result) {
                        if (!result.success) {
                            mini.alert(result.msg);
                        }
                    }, "json");
                }
            });
        }
    }

    function search() {
        var data = {
            ontologyCode: ontologyCode,
            actionCode: actionCode.getValue(),
            catalogCode: getParams().catalogCode
        };
        if (nodeId) {
            data.nodeId = nodeId.getValue();
        }
        grid.load(data, function () {
            var record = grid.getSelected();
            if (!record) {
                tabs1.hide();
            }
        });
    }

    function loadData() {
        search();
    }

    function getParams() {
        if (self.params && self.params.catalogCode) {
            return self.params;
        }
        else {
            return { catalogCode: $.deparam.fragment().catalogCode || $.deparam.querystring().catalogCode }
        }
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
                case "ClientName":
                    var url = bootPATH + "../Edi/Node/Details?isTooltip=true&id=" + record.ClientId
                    e.cellHtml = "<a href='" + url + "' onclick='helper.cellTooltip(this);return false;' rel='" + url + "'>" + value + "</a>";
                    break;
            }
        }
        helperDrawcell(e);
    }
})(window);