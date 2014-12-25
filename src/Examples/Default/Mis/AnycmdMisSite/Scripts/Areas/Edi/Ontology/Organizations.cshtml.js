/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Ontology.Organizations");
    var self = Ontology.Organizations;
    self.prifix = "Edi_Ontology_Organizations_";
    self.loadData = loadData;
    mini.parse();

    var loaded = false;
    var addOrganizationIds = [];
    var removeOrganizationIds = [];

    var treeOrganization = mini.get(self.prifix + "treeOrganization");
    treeOrganization.on("beforeload", onTreeBeforeload);
    treeOrganization.on("nodeselect", function (e) {
        var node = e.node;
        if (!dgAction.sortField) {
            dgAction.sortBy("Verb", "asc");
        }
        dgAction.load({ ontologyId: node.OntologyId, organizationId: node.Id });
    });
    treeOrganization.on("nodecheck", onNodeCheck);
    var btnSaveAction = mini.get(self.prifix + "btnSaveAction");
    btnSaveAction.on("click", saveOrganizationAction);
    var dgAction = mini.get(self.prifix + "dgAction");
    dgAction.on("load", helper.onGridLoad);
    dgAction.on("drawcell", ondrawcell);

    function loadData() {
        var data = getParams();
        addOrganizationIds.clear();
        removeOrganizationIds.clear();
        if (!data.isEntityOrganized) {
            mini.alert("选中的本体不是组织结构型本体");
        }
        else {
            loadOrganizationTreeNode(data.ontologyId);
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

    function loadOrganizationTreeNode(ontologyId) {
        $.get(bootPATH + "../Edi/Ontology/GetOrganizationNodesByParentId"
            , { ontologyId: ontologyId }
            , function (result) {
                treeOrganization.loadList(result, "Id", "ParentId");
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
        if (addOrganizationIds.length > 0 || removeOrganizationIds.length > 0) {
            var data = {
                ontologyId: getParams().ontologyId,
                addOrganizationIds: addOrganizationIds.join(","),
                removeOrganizationIds: removeOrganizationIds.join(",")
            };
            $.post(bootPATH + "../Edi/Ontology/AddOrRemoveOrganizations",
                data,
                function (result) {
                    helper.response(result, function () {
                        addOrganizationIds.clear();
                        removeOrganizationIds.clear();
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
                    if (addOrganizationIds.indexOf(n.Id) < 0) {
                        addOrganizationIds.push(n.Id);
                    }
                    if (removeOrganizationIds.indexOf(n.Id) >= 0) {
                        removeOrganizationIds.remove(n.Id);
                    }
                }
            }, self);
            tree.cascadeChild(node, function (n) {
                if (n.Id && n.checked) {
                    if (addOrganizationIds.indexOf(n.Id) < 0) {
                        addOrganizationIds.push(n.Id);
                    }
                    if (removeOrganizationIds.indexOf(n.Id) >= 0) {
                        removeOrganizationIds.remove(n.Id);
                    }
                }
            }, self);
        }
        else {
            tree.bubbleParent(node, function (n) {
                if (n.Id && !n.checked) {
                    if (removeOrganizationIds.indexOf(n.Id) < 0) {
                        removeOrganizationIds.push(n.Id);
                    }
                    if (addOrganizationIds.indexOf(n.Id) >= 0) {
                        addOrganizationIds.remove(n.Id);
                    }
                }
            }, self);
            tree.cascadeChild(node, function (n) {
                if (n.Id && !n.checked) {
                    if (removeOrganizationIds.indexOf(n.Id) < 0) {
                        removeOrganizationIds.push(n.Id);
                    }
                    if (addOrganizationIds.indexOf(n.Id) >= 0) {
                        addOrganizationIds.remove(n.Id);
                    }
                }
            }, self);
        }
        saveData();
    }

    function saveOrganizationAction() {
        var data = dgAction.getChanges();
        var json = mini.encode(data);

        dgAction.loading("保存中，请稍后......");
        $.ajax({
            url: bootPATH + "../Edi/Ontology/AddOrUpdateOrganizationActions",
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