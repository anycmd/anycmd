/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    var currentNode;

    mini.parse();

    $(".Select_btnSelectOk").click(selectOk);
    $(".Select_btnSelectOkClose").click(function () {
        if (selectOk()) {
            hideWin();
        }
        return false;
    });
    $(".Select_btnSelectCancel").click(function () {
        hideWin();
    });
    $("#Select_btnSelectSearch").click(search);

    var treeOrganization = mini.get("Select_treeOrganization");
    treeOrganization.on("nodeselect", onOrganizationNodeSelect);
    treeOrganization.on("beforenodeselect", function (e) {
        var tree = e.sender;
        var node = e.node;
        if (node.IsCategory) {
            e.cancel = true;
            if (node.expanded) {
                tree.collapseNode(node);
            }
            else {
                tree.expandNode(node);
            }
        }
    });
    treeOrganization.on("beforeload", onOrganizationTreeBeforeload);
    var chkbIncludedescendants = mini.get("Select_chkbIncludedescendants");
    chkbIncludedescendants.on("checkedchanged", onCheckedChanged);
    var key = mini.get("Select_selectKey");
    key.on("enter", search);
    var grid = mini.get("Select_dgOrgChildren");
    grid.on("drawcell", helper.ondrawcell(window));
    grid.on("load", helper.onGridLoad);
    var data = {};
    grid.load(data);
    grid.sortBy("CreateOn", "desc");

    function onOrganizationNodeSelect(e) {
        var tree = e.sender;
        var node = e.node;
        var isLeaf = e.isLeaf;
        currentNode = node;
        search();
    }

    function onOrganizationTreeBeforeload(e) {
        var tree = e.sender;
        var node = e.node;
        var params = e.params;

        params.parentID = node.Id;
    }

    function onCheckedChanged(e) {
        search();
    }

    function selectOk() {
        var rows = grid.getSelecteds();
        var s = "";
        var n = "";
        for (var i = 0, l = rows.length; i < l; i++) {
            var row = rows[i];
            s += row.Id;
            n += row.Name;
            if (i != l - 1) {
                s += ",";
                n += ",";
            }
        }
        if (s) {
            if (window.onSelectOk) {
                window.onSelectOk({ organizationIDs: s, organizationNames: n });
            }
            return true;
        }
        else {
            mini.alert("没有选中组织结构");
            return false;
        }
    }

    function hideWin() {
        var win = mini.get("Select_win1");
        win.hide();
    }

    function search() {
        var data = { key: key.getValue() };
        if (currentNode && currentNode.Id) {
            data.organizationID = currentNode.Id;
            data.organizationCode = currentNode.Code;
        }
        if (chkbIncludedescendants.getValue() == "1") {
            data.includedescendants = true;
        }
        else {
            data.includedescendants = false;
        }
        grid.load(data);
    }
})(window);