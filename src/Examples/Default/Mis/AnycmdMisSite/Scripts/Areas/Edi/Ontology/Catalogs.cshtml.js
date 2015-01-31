/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Ontology.Catalogs");
    var self = Ontology.Catalogs;
    self.prifix = "Edi_Ontology_Catalogs_";
    self.loadData = loadData;
    mini.parse();

    var loaded = false;
    var addCatalogIds = [];
    var removeCatalogIds = [];

    var treeCatalog = mini.get(self.prifix + "treeCatalog");
    treeCatalog.on("beforeload", onTreeBeforeload);
    treeCatalog.on("nodeselect", function (e) {
        var node = e.node;
        if (!dgAction.sortField) {
            dgAction.sortBy("Verb", "asc");
        }
        dgAction.load({ ontologyId: node.OntologyId, catalogId: node.Id });
    });
    treeCatalog.on("nodecheck", onNodeCheck);
    var btnSaveAction = mini.get(self.prifix + "btnSaveAction");
    btnSaveAction.on("click", saveCatalogAction);
    var dgAction = mini.get(self.prifix + "dgAction");
    dgAction.on("load", helper.onGridLoad);
    dgAction.on("drawcell", ondrawcell);

    function loadData() {
        var data = getParams();
        addCatalogIds.clear();
        removeCatalogIds.clear();
        if (!data.isEntityOrganized) {
            mini.alert("选中的本体不是目录型本体");
        }
        else {
            loadCatalogTreeNode(data.ontologyId);
        }
        dgAction.clearRows();
    }

    function onTreeBeforeload(e) {
        var tree = e.sender;    //树控件
        var node = e.node;      //当前节点
        var params = e.params;  //参数对象

        params.parentId = node.Id;
        params.ontologyId = getParams().ontologyId;
    }

    function loadCatalogTreeNode(ontologyId) {
        $.get(bootPATH + "../Edi/Ontology/GetCatalogNodesByParentId"
            , { ontologyId: ontologyId }
            , function (result) {
                treeCatalog.loadList(result, "Id", "ParentId");
            });
    }

    function getParams() {
        if (self.params && self.params.ontologyId) {
            return self.params;
        }
        else {
            return {
                ontologyId: $.deparam.fragment().ontologyId || $.deparam.querystring().ontologyId,
                isEntityOrganized: $.deparam.fragment().isEntityOrganized || $.deparam.querystring().isEntityOrganized
            }
        }
    }
    function saveData() {
        if (addCatalogIds.length > 0 || removeCatalogIds.length > 0) {
            var data = {
                ontologyId: getParams().ontologyId,
                addCatalogIds: addCatalogIds.join(","),
                removeCatalogIds: removeCatalogIds.join(",")
            };
            $.post(bootPATH + "../Edi/Ontology/AddOrRemoveCatalogs",
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

    function saveCatalogAction() {
        var data = dgAction.getChanges();
        var json = mini.encode(data);

        dgAction.loading("保存中，请稍后......");
        $.ajax({
            url: bootPATH + "../Edi/Ontology/AddOrUpdateCatalogActions",
            data: { data: json },
            type: "post",
            success: function (result) {
                dgAction.reload();
            },
            error: function (jqXHR, textStatus, errorThrown) {
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
                case "ActionIsAllowed":
                case "ActionIsAudit":
                    if (value == "正常" || value == "1" || value == "是" || value == "true") {
                        e.cellHtml = "<span class='icon-enabled width16px'></span>";
                    } else if (value == "禁用" || value == "0" || value == "否" || value == "false") {
                        e.cellHtml = "<span class='icon-disabled width16px'></span>";
                    } break;
            }
        }
    }
})(window);