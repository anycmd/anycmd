/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Edi.Entity.Index");
    var self = Edi.Entity.Index;
    self.loadData = loadData;
    self.prifix = "Edi_Entity_Index_";
    self.help = { appSystemCode: "Anycmd", areaCode: "Edi", resourceCode: "Entity", functionCode: "Index" };
    helper.helperSplitterInOne(self);
    window.onFilterChanged = onFilterChanged;
    self.gridReload = function () {
        grid.reload();
    };
    self.edit = function (record) {
        var records = grid.getSelecteds();
        if (records && records.length != 1) {
            mini.showTips({
                content: "请选中一行",
                state: "warning",
                x: "center",
                y: "top",
                timeout: 3000
            });
            return;
        }
        entityTabs1.activeTab(entityTabs1.getTab("editTab"));
    }
    self.add = function () {
        var newRow = grid.getRow(0);
        if (treeCatalog && !currentNode) {
            mini.alert("请选择要添加到的目录");
            return;
        }
        if (!newRow || !newRow.isNewRow) {
            grid.deselectAll();
            newRow = { isNewRow: true, Id: "-1" };
            if (treeCatalog) {
                newRow.ZZJGM = currentNode.Name;
                newRow.CatalogCode = currentNode.Code;
            }
            grid.addRow(newRow, 0);
            grid.select(newRow);
        }
    };

    var gridParams;
    var currentNode;
    var filterData = {};
    var filterBoxs = {};
    var fregment = $.deparam.fragment();
    var queryString = $.deparam.querystring();
    var ontologyCode = fregment.ontologyCode || queryString.ontologyCode;
    var ontologyId = fregment.ontologyId || queryString.ontologyId;
    var isArchive = fregment.isArchive || queryString.isArchive;
    var archiveId = fregment.archiveId || queryString.archiveId;

    var tabConfigs = {
        entityTab: {
        },
        distributeCommandTab: {
            url: bootPATH + "../Edi/Command/DistributeMessage?entityTypeCode=DistributeMessage&ontologyCode=" + ontologyCode,
            namespace: "Command.DistributeMessage"
        },
        distributeFailingCommandTab: {
            url: bootPATH + "../Edi/Command/DistributeFailingMessage?entityTypeCode=DistributeFailingMessage&ontologyCode=" + ontologyCode,
            namespace: "Command.DistributeFailingMessage"
        },
        distributedCommandTab: {
            url: bootPATH + "../Edi/Command/DistributedMessage?entityTypeCode=DistributedMessage&ontologyCode=" + ontologyCode,
            namespace: "Command.DistributedMessage"
        },
        transmitCommandTab: {
            url: bootPATH + "../Edi/Command/TransmitCommand?entityTypeCode=TransmitCommand&ontologyCode=" + ontologyCode,
            namespace: "Command.TransmitCommand"
        },
        transmitedCommandTab: {
            url: bootPATH + "../Edi/Command/TransmitedCommand?entityTypeCode=TransmitedCommand&ontologyCode=" + ontologyCode,
            namespace: "Command.TransmitedCommand"
        },
        executedCommandTab: {
            url: bootPATH + "../Edi/Command/HandledCommand?entityTypeCode=HandledCommand&ontologyCode=" + ontologyCode,
            namespace: "Command.HandledCommand"
        },
        executeFailingCommandTab: {
            url: bootPATH + "../Edi/Command/HandleFailingCommand?entityTypeCode=HandleFailingCommand&ontologyCode=" + ontologyCode,
            namespace: "Command.HandleFailingCommand"
        },
        receivedCommandTab: {
            url: bootPATH + "../Edi/Command/ReceivedMessage?entityTypeCode=ReceivedMessage&ontologyCode=" + ontologyCode,
            namespace: "Command.ReceivedMessage"
        },
        unacceptedMessageTab: {
            url: bootPATH + "../Edi/Command/UnacceptedMessage?entityTypeCode=UnacceptedMessage&ontologyCode=" + ontologyCode,
            namespace: "Command.UnacceptedMessage"
        },
        localEventTab: {
            url: bootPATH + "../Edi/Command/LocalEvent?entityTypeCode=LocalEvent&ontologyCode=" + ontologyCode,
            namespace: "Command.LocalEvent"
        },
        clientEventTab: {
            url: bootPATH + "../Edi/Command/ClientEvent?entityTypeCode=ClientEvent&ontologyCode=" + ontologyCode,
            namespace: "Command.ClientEvent"
        }
    };
    var entityTabConfigs = {
        infoTab: {
            url: bootPATH + "../Edi/Entity/Details?ontologyCode=" + ontologyCode,
            params: [{ "pName": 'id', "pValue": "Id" }],
            namespace: "Entity.Details"
        },
        editTab: {
            url: bootPATH + "../Edi/Entity/Edit?ontologyCode=" + ontologyCode,
            params: [{ "pName": 'id', "pValue": "Id" }, { "pName": 'catalogCode', "pValue": "CatalogCode" }],
            namespace: "Entity.Edit"
        },
        nodeTab: {
            url: bootPATH + "../Edi/Node/EntityNodes?ontologyCode=" + ontologyCode,
            params: [{ "pName": 'entityId', "pValue": "Id" }],
            namespace: "Entity.EntityNodes"
        },
        executedCommandTab: {
            url: bootPATH + "../Edi/Command/EntityHandledCommands?ontologyCode=" + ontologyCode,
            params: [{ "pName": "entityId", "pValue": "Id" }],
            namespace: "Command.EntityHandledCommands"
        },
        executeFailingCommandTab: {
            url: bootPATH + "../Edi/Command/EntityHandleFailingCommands?ontologyCode=" + ontologyCode,
            params: [{ "pName": "entityId", "pValue": "Id" }],
            namespace: "Command.EntityHandleFailingCommands"
        },
        receivedCommandTab: {
            url: bootPATH + "../Edi/Command/EntityReceivedMessages?ontologyCode=" + ontologyCode,
            params: [{ "pName": "entityId", "pValue": "Id" }],
            namespace: "Command.EntityReceivedMessages"
        },
        distributeCommandTab: {
            url: bootPATH + "../Edi/Command/EntityDistributeMessages?ontologyCode=" + ontologyCode,
            params: [{ "pName": "entityId", "pValue": "Id" }],
            namespace: "Command.EntityDistributeMessages"
        },
        distributedCommandTab: {
            url: bootPATH + "../Edi/Command/EntityDistributedMessages?ontologyCode=" + ontologyCode,
            params: [{ "pName": "entityId", "pValue": "Id" }],
            namespace: "Command.EntityDistributedMessages"
        },
        distributeFailingCommandTab: {
            url: bootPATH + "../Edi/Command/EntityDistributeFailingMessages?ontologyCode=" + ontologyCode,
            params: [{ "pName": "entityId", "pValue": "Id" }],
            namespace: "Command.EntityDistributeFailingMessages"
        },
        localEventTab: {
            url: bootPATH + "../Edi/Command/EntityLocalEvents?ontologyCode=" + ontologyCode,
            params: [{ "pName": "entityId", "pValue": "Id" }],
            namespace: "Command.EntityLocalEvents"
        },
        clientEventTab: {
            url: bootPATH + "../Edi/Command/EntityClientEvents?ontologyCode=" + ontologyCode,
            params: [{ "pName": "entityId", "pValue": "Id" }],
            namespace: "Command.EntityClientEvents"
        }
    };

    mini.parse();

    var btnImportExcel = mini.get(self.prifix + "btnImportExcel");
    var btnExportExcel = mini.get(self.prifix + "btnExportExcel");
    if (btnImportExcel) {
        if (window.top.topShowTab) {
            btnImportExcel.on("click", function () {
                var tab = {};
                tab.text = "数据导入" + ontologyCode;
                tab.url = bootPATH + '../Edi/Entity/Import?ontologyCode=' + ontologyCode;
                window.top.topShowTab(tab);
            });
        }
        else {
            btnImportExcel.setHref(bootPATH + '../Edi/Entity/Import?ontologyCode=' + ontologyCode);
            btnImportExcel.setTarget("_blank");
        }
    }
    var Export_win1 = mini.get(self.prifix + "Export_win1");

    if (btnExportExcel) {
        btnExportExcel.on("click", function () {
            Export_win1.show();
        });
    }
    var lbtnArchive = mini.get(self.prifix + "lbtnArchive");
    if (lbtnArchive) {
        if (window.top.topShowTab) {
            lbtnArchive.on("click", function () {
                var tab = {};
                tab.text = lbtnArchive.text;
                tab.url = bootPATH + '../Edi/Archive/Index?ontologyCode=' + ontologyCode + "&ontologyId=" + ontologyId;
                window.top.topShowTab(tab);
            });
        }
        else {
            lbtnArchive.setHref(bootPATH + '../Edi/Archive/Index?ontologyCode=' + ontologyCode + "&ontologyId=" + ontologyId);
        }
    }
    var cblExportElement = mini.get(self.prifix + "cblExportElement");
    $("." + self.prifix + "btnExportCancel").click(function () {
        Export_win1.hide();
    });
    var cblImportElement = mini.get(self.prifix + "cblImportElement");
    $("." + self.prifix + "btnExportAll").click(function () {
        gridParams.elements = cblExportElement.getValue();
        gridParams.exportType = "allPage";
        exportExcel();
    });
    $("." + self.prifix + "btnExportCurrentPage").click(function () {
        gridParams.elements = cblExportElement.getValue();
        gridParams.exportType = "currentPage";
        exportExcel();
    });
    $("." + self.prifix + "btnDownloadTemplate").click(function () {
        gridParams.elements = cblImportElement.getValue();
        gridParams.exportType = "temp";
        exportExcel();
    });
    var limit = mini.get(self.prifix + "limit");
    var btnSearchCatalog = mini.get(self.prifix + "btnSearchCatalog");
    if (btnSearchCatalog) {
        btnSearchCatalog.on("click", searchCatalog);
    }
    var btnSearchClear = mini.get(self.prifix + "btnSearchClear");
    if (btnSearchClear) {
        btnSearchClear.on("click", clearSearch);
    }
    var keyCatalog = mini.get(self.prifix + "keyCatalog");
    if (keyCatalog) {
        keyCatalog.on("enter", searchCatalog);
    }
    var treeCatalog = mini.get(self.prifix + "treeCatalog");
    if (treeCatalog) {
        treeCatalog.on("nodeselect", onCatalogNodeSelect);
        treeCatalog.on("beforeload", onCatalogTreeBeforeload);
    }
    var chkbIncludedescendants = mini.get(self.prifix + "chkbIncludedescendants");
    if (chkbIncludedescendants) {
        chkbIncludedescendants.on("checkedchanged", onCheckedChanged);
    }

    var tabs1 = mini.get("tabs1");
    tabs1.on("tabload", ontabload);
    tabs1.on("activechanged", onactivechanged);

    var entityTabs1 = mini.get(self.prifix + "entityTabs1");
    var grid = mini.get(self.prifix + "datagrid1");
    grid.on("beforeload", function (e) {
        if (!isArchive) {
            e.sender.__total = 0;
        }
        gridParams = e.data;
    });
    grid.on("drawcell", helper.ondrawcell(self, "Edi.Entity.Index"));
    grid.on("load", helper.onGridLoad);
    loadData();

    function openImportView() {
        var win = mini.open({
            title: '导入',
            url: bootPATH + '../Edi/Entity/ImportView?ontologyCode=' + ontologyCode,
            showModal: true,
            width: 800,
            height: 600
        });

        win.showAtPos('center', 'middle');
    }

    function loadData() {
        if (isArchive && archiveId != getParams().archiveId) {
            archiveId = getParams().archiveId;
            search();
        }
        else if (treeCatalog) {
            currentNode = treeCatalog.getRootNode();
            if (!currentNode.Id) {
                currentNode = treeCatalog.getChildNodes(currentNode)[0];
            }
            if (currentNode.Id) {
                treeCatalog.selectNode(currentNode);
            }
        }
        else {
            search();
        }
    }

    function getParams() {
        data = {};
        if (isArchive) {
            if (self.params && self.params.archiveId) {
                data.archiveId = self.params.archiveId;
            }
            else {
                var fragment = $.deparam.fragment();
                var querystring = $.deparam.querystring();
                data.archiveId = fragment.archiveId || querystring.archiveId;
            }
        }
        return data;
    }

    function searchCatalog() {
        var k = keyCatalog.getValue().trim();
        if (k == "") {
            treeCatalog.clearFilter();
            $("#" + self.prifix + "msg").hide()
        } else {
            k = k.toLowerCase();
            var anyIsTrue = false;
            treeCatalog.filter(function (node) {
                var name = node.Name ? node.Name.toLowerCase() : "";
                if (!node.expanded && !node.isLeaf && !node.IsCategory) {
                    return false;
                }
                if (name.indexOf(k) != -1) {
                    anyIsTrue = true;
                    return true;
                }
            });
            if (anyIsTrue) {
                $("#" + self.prifix + "msg").hide()
            }
            else {
                $("#" + self.prifix + "msg").show();
            }
        }
        treeCatalog.expandAll();
    }

    function onCheckedChanged(e) {
        search();
    }

    helper.index.allInOne(
        null,
        grid,
        bootPATH + "../Edi/Entity/Edit?ontologyCode=" + ontologyCode,
        bootPATH + "../Edi/Entity/Edit?ontologyCode=" + ontologyCode,
        bootPATH + "../Edi/Entity/DeleteEntity?ontologyCode=" + ontologyCode,
        self);
    helper.index.tabInOne(grid, entityTabs1, entityTabConfigs, self);

    function onCatalogNodeSelect(e) {
        var tree = e.sender;
        var node = e.node;
        var isLeaf = e.isLeaf;
        currentNode = node;
        var text = "";
        if (node.Code) {
            text = "目录码：" + node.Code;
        }
        $("#" + self.prifix + "spanOrgCode").text(text);
        filterData.pageIndex = 0;
        loadTabData("refresh", tabs1.getTab(tabs1.activeIndex));
    }

    function onCatalogTreeBeforeload(e) {
        var tree = e.sender;
        var node = e.node;
        var params = e.params;

        params.parentId = node.Id;
    }

    function onFilterChanged(e) {
        var textbox = e.sender;
        var value = textbox.getValue();
        var name = textbox.name;
        var oldValue = filterData[name];
        filterBoxs[name] = textbox;
        filterData[name] = value;
        if (!textbox.onEnter) {
            textbox.on("enter", onKeyEnter);
        }
        if ((!oldValue && value) || (oldValue && oldValue != value)) {
            search();
        }
    }

    function search() {
        if (treeCatalog && !currentNode.Id) {
            return;
        }
        var data = { translate: true };
        if (isArchive) {
            data.archiveId = getParams().archiveId;
        }
        var filterArray = [];
        for (var k in filterData) {
            if (filterData[k]) {
                filterArray.push({ field: k, value: filterData[k] });
            }
        }
        data.filters = JSON.stringify(filterArray);
        if (currentNode) {
            data["catalogId"] = currentNode.Id;
            data["catalogCode"] = currentNode.Code;
            if (chkbIncludedescendants.getValue() == "1") {
                if (currentNode) {
                    if (currentNode.isLeaf) {
                        data["includedescendants"] = false;
                    }
                    else {
                        data["includedescendants"] = true;
                    }
                }
            }
            else {
                data["includedescendants"] = false;
            }
        }
        grid.__total = 0;
        if (!grid.sortField) {
            grid.sortBy("CreateOn", "desc");
        }
        grid.load(data);
    }

    function clearSearch() {
        for (var key in filterBoxs) {
            if (filterBoxs[key].setValue) {
                filterBoxs[key].setValue("");
            }
        }
        filterData = {};
        search();
    }

    function onKeyEnter(e) {
        search();
    }

    function loadTabData(refresh, tab) {
        var tabName = tab.name;
        var tabConfig = tabConfigs[tabName];
        if (!tabConfig) {
            mini.alert("意外的tabName:" + tabName);
        }
        if (tabName == "entityTab") {
            if (treeCatalog) {
                if (tab.catalogCode != currentNode.Code) {
                    tab.catalogCode = currentNode.Code;
                    search();
                }
                return;
            }
        }
        if (refresh || !tabConfig['initialized']) {
            var tabBody = $(tabs1.getTabBodyEl(tab));
            var isInner = tabBody.hasClass("inner");
            var params = {};
            if (currentNode) {
                params.catalogCode = currentNode.Code;
            }
            if (isInner) {
                mini.namespace(tabConfig.namespace);
                var module = eval(tabConfig.namespace);
                module.params = params;
                if (tabConfig["isLoaded"]) {
                    module.loadData();
                }
                else {
                    params.isInner = true;
                    tabBody.load(tabConfig.url, params, function () {
                        module.loadData();
                    });
                    tabConfig["isLoaded"] = true;
                }
            }
            else {
                if (tabConfig['iframe']) {
                    var iframe = tabConfig['iframe'];
                    if (tabConfig.namespace) {
                        var module = iframe.contentWindow.eval(tabConfig.namespace);
                        if (module) {
                            module.params = params;
                            module.loadData();
                        }
                        else {
                            mini.alert("未找到命名空间" + tabConfig.namespace);
                        }
                    }
                    else {
                        mini.alert("未指定命名空间" + tabConfig.namespace);
                    }
                }
                else {
                    var href = $.param.fragment(tabConfig.url, params);
                    tabs1.loadTab(href, tab, function () {
                        var module = tabs1.getTabIFrameEl(tab).contentWindow.eval(tabConfig.namespace);
                        if (module) {
                            module.params = params;
                            module.loadData();
                        }
                    });
                }
            }

            tabConfig['initialized'] = true;
        }

        if (refresh) {
            for (var key in tabConfigs) {
                tabConfigs[key]['initialized'] = false;
            }
            tabConfig['initialized'] = true;
        }
    }

    function ontabload(e) {
        var tabs = e.sender;
        var tab = e.tab;
        var tabName = tab.name;
        var iframe = tabs.getTabIFrameEl(e.tab);
        var tabConfig = tabConfigs[tabName];
        if (tabConfig) {
            tabConfig['iframe'] = iframe;
        }
    }

    function onactivechanged(e) {
        loadTabData(null, e.tab);
    }
    function exportExcel() {
        var submitfrm = document.createElement("form");
        submitfrm.action = "Export";
        submitfrm.method = "post";
        submitfrm.target = "_blank";
        document.body.appendChild(submitfrm);

        if (gridParams) {
            gridParams.ontologyCode = ontologyCode;
            gridParams.limit = limit.getValue();
            for (var p in gridParams) {
                var input = mini.append(submitfrm, "<input type='hidden' name='" + p + "'>");
                var v = gridParams[p];
                if (typeof v != "string") v = mini.encode(v);
                input.value = v;
            }
        }

        submitfrm.submit();
        setTimeout(function () {
            submitfrm.parentNode.removeChild(submitfrm);
            Export_win1.hide();
        }, 1000);
    }
})(window);