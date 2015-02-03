/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Node.Catalogs");
    var self = Node.Catalogs;
    self.prifix = "Edi_Node_Catalogs_";
    self.loadData = loadData;
    mini.parse();

    var loaded = false;
    var addCatalogIds = [];
    var removeCatalogIds = [];
    var currentOntology;

    var dgOntology = mini.get(self.prifix + "dgOntology");
    dgOntology.on("drawcell", ondrawcell);
    dgOntology.on("load", helper.onGridLoad);
    dgOntology.on("selectionchanged", function (e) {
        var data = getParams();
        addCatalogIds.clear();
        removeCatalogIds.clear();
        var record = dgOntology.getSelected();
        currentOntology = record;
        if (!record.IsCataloguedEntity) {
            mini.alert("选中的本体不是目录型本体");
        }
        else {
            loadCatalogTreeNode(record.Id, data.nodeId);
        }
        $("#" + self.prifix + "ontologyName").text(record.Name);
    });
    var treeCatalog = mini.get(self.prifix + "treeCatalog");
    treeCatalog.on("beforeload", onTreeBeforeload);
    treeCatalog.on("nodecheck", onNodeCheck);
    treeCatalog.on("cellclick", function (e) {
        if (e.field == "IsAudit") {
            if (!e.node.checked) {
                return false;
            }
            $.post(bootPATH + "../Edi/Node/UpdateCatalog",
                { id: e.node.Id, nodeId: getParams().nodeId, ontologyId: currentOntology.Id, isAudit: e.node.IsAudit },
                function (result) {
                    helper.response(result, function () {
                    });
                }, "json");
        }
    });
    function loadData() {
        dgOntology.load({ nodeId: getParams().nodeId });
        if (!dgOntology.sortField) {
            dgOntology.sortBy("SortCode", "asc");
        }
    }

    function onTreeBeforeload(e) {
        var tree = e.sender;    //树控件
        var node = e.node;      //当前节点
        var params = e.params;  //参数对象
        var data = getParams();
        params.parentId = node.Id;
        params.ontologyId = currentOntology.Id;
        params.nodeId = data.nodeId;
    }

    function loadCatalogTreeNode(ontologyId, nodeId) {
        $.get(bootPATH + "../Edi/Node/GetCatalogNodesByParentId"
            , { ontologyId: ontologyId, nodeId: nodeId }
            , function (result) {
                treeCatalog.loadList(result, "Id", "ParentId");
            });
    }

    function getParams() {
        if (self.params && self.params.nodeId) {
            return self.params;
        }
        else {
            return {
                nodeId: $.deparam.fragment().nodeId || $.deparam.querystring().nodeId
            }
        }
    }
    function saveData() {
        if (addCatalogIds.length > 0 || removeCatalogIds.length > 0) {
            var data = {
                ontologyId: currentOntology.Id,
                nodeId: getParams().nodeId,
                addCatalogIds: addCatalogIds.join(","),
                removeCatalogIds: removeCatalogIds.join(",")
            };
            $.post(bootPATH + "../Edi/Node/AddOrRemoveCatalogs",
                data,
                function (result) {
                    helper.response(result, function () {
                        addCatalogIds.clear();
                        removeCatalogIds.clear();
                    });
                }, "json");
        }
    }
    function onNodeCheck(e) {
        var tree = e.sender;
        var node = e.node;
        var isLeaf = e.isLeaf;
        if (!e.checked) {
            tree.bubbleParent(node, function (n) {
                if (n.Id && n.checked) {
                    if (addCatalogIds.indexOf(n.Id) < 0) {
                        addCatalogIds.push(n.Id);
                    }
                    if (removeCatalogIds.indexOf(n.Id) >= 0) {
                        removeCatalogIds.remove(n.Id);
                    }
                }
            }, self);
            tree.cascadeChild(node, function (n) {
                if (n.Id && n.checked) {
                    if (addCatalogIds.indexOf(n.Id) < 0) {
                        addCatalogIds.push(n.Id);
                    }
                    if (removeCatalogIds.indexOf(n.Id) >= 0) {
                        removeCatalogIds.remove(n.Id);
                    }
                }
            }, self);
        }
        else {
            tree.bubbleParent(node, function (n) {
                if (n.Id && !n.checked) {
                    if (removeCatalogIds.indexOf(n.Id) < 0) {
                        removeCatalogIds.push(n.Id);
                    }
                    if (addCatalogIds.indexOf(n.Id) >= 0) {
                        addCatalogIds.remove(n.Id);
                    }
                }
            }, self);
            tree.cascadeChild(node, function (n) {
                if (n.Id && !n.checked) {
                    if (removeCatalogIds.indexOf(n.Id) < 0) {
                        removeCatalogIds.push(n.Id);
                    }
                    if (addCatalogIds.indexOf(n.Id) >= 0) {
                        addCatalogIds.remove(n.Id);
                    }
                }
            }, self);
        }
        saveData();
    }

    function ondrawcell(e) {
        var field = e.field;
        var value = e.value;
        var record = e.record;
        if (field) {
            switch (field) {
                case "ActionIsAllowed":
                case "ActionIsAudit":
                    if (value == "正常" || value == "1" || value == "是" || value == "true") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "禁用" || value == "0" || value == "否" || value == "false") {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
                case "IsEnabled":
                case "IsAudit":
                    if (e.sender == dgOntology) {
                        if (value == "正常" || value == "1" || value == "是" || value == "true") {
                            e.cellHtml = "<span class='icon-enabled width16px'></span>";
                        } else if (value == "禁用" || value == "0" || value == "否" || value == "false") {
                            e.cellHtml = "<span class='icon-disabled width16px'></span>";
                        } 
                    }
                    break;
            }
        }
    }
})(window);