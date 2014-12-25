/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../jquery-tmpl/jquery.tmpl.js" />
/// <reference path="../../../helper.js" />
/// <reference path="../../../jquery-bbq/jquery.ba-bbq.js" />
(function (window) {
    mini.namespace("Entity.Details");
    var self = Entity.Details;
    self.loadData = function () {
        helper.requesting();
        $.getJSON(
            bootPATH + "../Edi/Entity/Get"
            , getParams()
            , function (result) {
                helper.response(result, function () {
                    $("#details").empty();
                    if (result) {
                        $("#detailsTemplate").tmpl(result).appendTo("#details");
                    }
                });
                helper.responsed();
            });
    };

    function getParams() {
        var nodeId = $.deparam.fragment().nodeId || $.deparam.querystring().nodeId || '';
        var data = { translate: true, ontologyCode: $.deparam.fragment().ontologyCode || $.deparam.querystring().ontologyCode, nodeId:nodeId };
        if (self.params && self.params.id) {
            data.id = self.params.id;
        }
        else {
            data.id = $.deparam.fragment().id || $.deparam.querystring().id;
        }
        return data;
    }
})(window);