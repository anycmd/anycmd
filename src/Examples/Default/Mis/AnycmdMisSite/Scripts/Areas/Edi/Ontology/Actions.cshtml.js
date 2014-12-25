/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Ontology.Actions");
    var self = Ontology.Actions;
    self.prifix = "Edi_Ontology_Actions_";
    self.loadData = loadData;
    self.gridReload = function () {
        grid.reload();
    };
    mini.namespace("Ontology.Actions.Edit");
    var edit = Ontology.Actions.Edit;
    edit.prifix = "Edi_Ontology_Actions_Edit_";
    edit.SetData = SetData;

    mini.parse();

    var win = mini.get(edit.prifix + "win1");
    var form;
    if (win) {
        form = new mini.Form(edit.prifix + "form1");
    }

    var grid = mini.get(self.prifix + "datagrid1");
    var helperDrawcell = helper.ondrawcell(self, "Ontology.Actions");
    grid.on("drawcell", ondrawcell);
    grid.on("load", helper.onGridLoad);

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Edi/Ontology/EditAction",
        bootPATH + "../Edi/Ontology/EditAction",
        bootPATH + "../Edi/Ontology/DeleteAction",
        self);

    function loadData() {
        if (!grid.sortField) {
            grid.sortBy("SortCode", "asc");
        }
        grid.load(getParams());
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
                case "IsAllowed":
                case "IsPersist":
                case "IsAudit":
                    if (value == "正常" || value == "1" || value == "是" || value == "true") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "禁用" || value == "0" || value == "否" || value == "false") {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
            }
        }
        helperDrawcell(e);
    }

    function SetData(data) {
        //跨页面传递的数据对象，克隆后才可以安全使用
        data = mini.clone(data);
        if (data.action == "edit") {
            $.ajax({
                url: bootPATH + "../Edi/Ontology/GetAction?id=" + data.id,
                cache: false,
                success: function (result) {
                    helper.response(result, function () {
                        form.setData(result);
                        form.validate();
                    });
                }
            });
        }
        else if (data.action == "new") {
            data["OntologyId"] = getParams().ontologyId;
            form.setData(data);
        }
    }

    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Edi/Ontology/AddAction",
        bootPATH + "../Edi/Ontology/UpdateAction",
        bootPATH + "../Edi/Ontology/GetAction",
        form, edit);
})(window);