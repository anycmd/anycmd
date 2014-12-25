/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window, $) {
    mini.namespace("Role.Menus");
    var self = Role.Menus;
    self.prifix = "Ac_Role_Menus_";
    self.loadData = loadData;

    var loaded = false;
    var addMenuIds = [];
    var removeMenuIds = [];

    mini.parse();

    var btnRefresh = mini.get(self.prifix + "btnRefresh");
    if (btnRefresh) {
        btnRefresh.on("click", function () {
            loadData();
        });
    }
    var tree1 = mini.get(self.prifix + "tree1");
    tree1.on("nodecheck", onNodeCheck);
    tree1.on("drawnode", onDrawNode);

    function loadData() {
        loadTree();
    }

    function loadTree() {
        var data = getParams();
        helper.requesting();
        if (!tree1.url) {
            tree1.url = bootPATH + "../Ac/Menu/GetNodesByRoleId";
        }
        $.get(bootPATH + "../Ac/Menu/GetNodesByRoleId",
            data,
            function (result) {
                tree1.loadList(result, "MenuId", "ParentId");
                helper.responsed();
            });
    }

    function getParams() {
        data = {};
        if (self.params && self.params.roleId) {
            data.roleId = self.params.roleId;
        }
        else {
            data.roleId = $.deparam.fragment().roleId || $.deparam.querystring().roleId
        }
        return data;
    }

    function onNodeCheck(e) {
        var tree = e.sender;
        var node = e.node;
        var isLeaf = e.isLeaf;
        if (!e.checked) {
            tree.bubbleParent(node, function (n) {
                if (n.MenuId && n.checked) {
                    if (addMenuIds.indexOf(n.MenuId) < 0) {
                        addMenuIds.push(n.MenuId);
                    }
                    if (removeMenuIds.indexOf(n.MenuId) >= 0) {
                        removeMenuIds.remove(n.MenuId);
                    }
                }
            }, self);
            tree.cascadeChild(node, function (n) {
                if (n.MenuId && n.checked) {
                    if (addMenuIds.indexOf(n.MenuId) < 0) {
                        addMenuIds.push(n.MenuId);
                    }
                    if (removeMenuIds.indexOf(n.MenuId) >= 0) {
                        removeMenuIds.remove(n.MenuId);
                    }
                }
            }, self);
        }
        else {
            tree.bubbleParent(node, function (n) {
                if (n.MenuId && !n.checked) {
                    if (removeMenuIds.indexOf(n.MenuId) < 0) {
                        removeMenuIds.push(n.MenuId);
                    }
                    if (addMenuIds.indexOf(n.MenuId) >= 0) {
                        addMenuIds.remove(n.MenuId);
                    }
                }
            }, self);
            tree.cascadeChild(node, function (n) {
                if (n.MenuId && n.checked) {
                    if (removeMenuIds.indexOf(n.MenuId) < 0) {
                        removeMenuIds.push(n.MenuId);
                    }
                    if (addMenuIds.indexOf(n.MenuId) >= 0) {
                        addMenuIds.remove(n.MenuId);
                    }
                }
            }, self);
        }
        saveData();
    }
    function saveData() {
        if (addMenuIds.length > 0 || removeMenuIds.length > 0) {
            var data = {
                roleId: getParams().roleId,
                addMenuIds: addMenuIds.join(","),
                removeMenuIds: removeMenuIds.join(",")
            };
            $.post(bootPATH + "../Ac/Role/AddOrRemoveMenus",
                data,
                function (result) {
                    helper.response(result, function () {
                        addMenuIds.clear();
                        removeMenuIds.clear();
                    });
                }, "json");
        }
    }
    function onDrawNode(e) {
        var tree = e.sender;
        var node = e.node;
        if (!node.Id) {
            e.showCheckBox = false;
        }
    }
    Array.prototype.remove = function (b) {
        var a = this.indexOf(b);
        if (a >= 0) {
            this.splice(a, 1);
            return true;
        }
        return false;
    };
})(window, jQuery);