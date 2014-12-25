/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Ontology.InfoGroups");
    var self = Ontology.InfoGroups;
    self.prifix = "Edi_Ontology_InfoGroups_";
    self.loadData = loadData;
    self.gridReload = function () {
        grid.reload();
    };
    mini.namespace("Ontology.InfoGroups.Edit");
    var edit = Ontology.InfoGroups.Edit;
    edit.prifix = "Edi_Ontology_InfoGroups_Edit_";
    edit.SetData = SetData;

    mini.parse();

    var win = mini.get(edit.prifix + "win1");
    var form;
    if (win) {
        form = new mini.Form(edit.prifix + "form1");
    }

    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", helper.ondrawcell(self, "Ontology.InfoGroups"));
    grid.on("load", helper.onGridLoad);

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Edi/Ontology/EditInfoGroup",
        bootPATH + "../Edi/Ontology/EditInfoGroup",
        bootPATH + "../Edi/Ontology/DeleteInfoGroup",
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

    function SetData(data) {
        //跨页面传递的数据对象，克隆后才可以安全使用
        data = mini.clone(data);
        if (data.action == "edit") {
            $.ajax({
                url: bootPATH + "../Edi/Ontology/GetInfoGroup?id=" + data.id,
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
        bootPATH + "../Edi/Ontology/AddInfoGroup",
        bootPATH + "../Edi/Ontology/UpdateInfoGroup",
        bootPATH + "../Edi/Ontology/GetInfoGroup",
        form, edit);
})(window);