/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
// 接口：edit、remove
(function (window) {
    mini.namespace("Ac.AppSystem.Index");
    var self = Ac.AppSystem.Index;
    self.prifix = "Ac_AppSystem_Index_";
    self.sortUrl = bootPATH + "../Ac/AppSystem/UpdateSortCode";
    self.help = { appSystemCode: "Anycmd", areaCode: "Ac", resourceCode: "AppSystem", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    self.gridReload = function () {
        grid.reload();
    };
    mini.namespace("AppSystem.Edit");
    var edit = AppSystem.Edit;
    edit.prifix = "Ac_AppSystem_Index_Edit_";

    var tabConfigs = {
        infoTab: {
            url: bootPATH + "../Ac/AppSystem/Details",
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "AppSystem.Details"
        },
        functionTab: {
            url: bootPATH + "../Ac/Function/Index",
            params: [{ "pName": 'appSystemCode', "pValue": "Code" }, { "pName": 'appSystemId', "pValue": "Id" }],
            namespace: "Ac.Function.Index"
        },
        uiViewTab: {
            url: bootPATH + "../Ac/UiView/Index",
            params: [{ "pName": 'appSystemCode', "pValue": "Code" }, { "pName": 'appSystemId', "pValue": "Id" }],
            namespace: "Ac.UiView.Index"
        },
        menuTab: {
            url: bootPATH + "../Ac/Menu/Index",
            params: [{ "pName": 'appSystemCode', "pValue": "Code" }, { "pName": 'appSystemId', "pValue": "Id" }],
            namespace: "Ac.Menu.Index"
        },
        operationLogTab: {
            url: bootPATH + "../Ac/OperationLog/Index",
            params: [{ "pName": 'targetId', "pValue": "Id" }],
            namespace: "Ac.OperationLog.Index"
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
    grid.on("drawcell", helper.ondrawcell(self, "Ac.AppSystem.Index"));
    grid.on("load", helper.onGridLoad);
    grid.load();
    grid.sortBy("SortCode", "asc");

    helper.index.allInOne(
        edit,
        grid,
        bootPATH + "../Ac/AppSystem/Edit",
        bootPATH + "../Ac/AppSystem/Edit",
        bootPATH + "../Ac/AppSystem/Delete",
        self);
    helper.index.tabInOne(grid, tabs1, tabConfigs, self);

    helper.edit.allInOne(
        self,
        win,
        bootPATH + "../Ac/AppSystem/Create",
        bootPATH + "../Ac/AppSystem/Update",
        bootPATH + "../Ac/AppSystem/Get",
        form, edit);
})(window);