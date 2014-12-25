/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window, $) {
    mini.namespace("Edi.Archive.Index");
    var self = Edi.Archive.Index;
    self.prifix = "Edi_Archive_Index_";
    self.search = search;
    self.gridReload = function () {
        grid.reload();
    };
    self.help = { appSystemCode: "Anycmd", areaCode: "Edi", resourceCode: "Archive", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    mini.namespace("Archive.Edit");
    var edit = Archive.Edit;
    edit.SaveData = SaveData;
    edit.prifix = "Edi_Archive_Index_Edit_";
    var faceInitialized = false;
    var ontologyCode = $.deparam.fragment().ontologyCode || $.deparam.querystring().ontologyCode;
    var ontologyId = $.deparam.fragment().ontologyId || $.deparam.querystring().ontologyId;

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Edi/Archive/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "Archive.Details"
        },
        dataTab: {
            url: bootPATH + "../Edi/Entity/Index?ontologyCode=" + ontologyCode + "&isArchive=true",
            params: [{ "pName": 'archiveId', "pValue": "Id" }],
            namespace: "Edi.Entity.Index"
        }
    };
    self.filters = {
        Title: {
            type: 'string',
            comparison: 'like'
        }
    };

    mini.parse();

    var win = mini.get(edit.prifix + "win1");
    var form;
    if (win) {
        form = new mini.Form(edit.prifix + "form1");
    }

    var tabs1 = mini.get(self.prifix + "tabs1");
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("drawcell", helper.ondrawcell(self, "Edi.Archive.Index"));
    grid.on("load", helper.onGridLoad);
    grid.sortBy("ArchiveOn", "desc");

    function search() {
        var data = {};
        var filterArray = [];
        for (var k in self.filters) {
            var filter = self.filters[k];
            if (filter.value) {
                filterArray.push({ field: k, type: filter.type, comparison: filter.comparison, value: filter.value });
            }
        }
        data.filters = JSON.stringify(filterArray);
        grid.load(data);
    }

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Edi/Archive/Edit",
        bootPATH + "../Edi/Archive/Edit",
        bootPATH + "../Edi/Archive/Delete",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    function SaveData() {
        var messageid;
        var m = $(form.el);

        var data = m.serialize();
        form.validate();
        if (form.isValid() == false) return;

        var id = m.find("input[name='Id']").val();
        var url = bootPATH + "../Edi/Archive/Create?ontologyCode=" + ontologyCode + "&ontologyId=" + ontologyId;
        if (id) {
            url = bootPATH + "../Edi/Archive/Update";
        }
        else {
            messageid = mini.loading("请等待...", "归档中");
        }

        $.post(url, data, function (result) {
            helper.response(result, function () {
                edit.CloseWindow("save");
            });
            if (messageid) {
                mini.hideMessageBox(messageid);
            }
        }, "json");
    }

    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Edi/Archive/Create?ontologyCode=" + ontologyCode + "&ontologyId=" + ontologyId,
        bootPATH + "../Edi/Archive/Update",
        bootPATH + "../Edi/Archive/Get",
        form, edit);
})(window, jQuery);