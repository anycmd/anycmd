/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../jquery-tmpl/jquery.tmpl.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Command.EntityLocalEvents");
    var self = Command.EntityLocalEvents;
    self.prifix = "Edi_Command_EntityLocalEvents_";
    self.loadData = loadData;
    var ontologyCode = $.deparam.fragment().ontologyCode || $.deparam.querystring().ontologyCode;

    mini.parse();

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
    grid.on("load", helper.onGridLoad);

    function loadData() {
        if (!grid.sortField) {
            grid.sortBy("TimeStamp", "desc");
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