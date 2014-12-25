/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
/// <reference path="../../../helper.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Node.NodeStateCodes");
    var self = Node.NodeStateCodes;
    self.prifix = "Edi_Node_NodeStateCodes_";
    self.loadData = loadData;
    var nodeId = $.deparam.fragment().nodeId || $.deparam.querystring().nodeId || '';

    mini.parse();
    
    function loadData() {
        var data = getParams();
    }

    function getParams() {
        data = { key: key.getValue() };
        if (self.params && self.params.nodeId) {
            data.nodeId = self.params.nodeId;
        }
        else {
            var fragment = $.deparam.fragment();
            var querystring = $.deparam.querystring();
            data.nodeId = fragment.nodeId || querystring.nodeId;
        }
        return data;
    }
})(window);