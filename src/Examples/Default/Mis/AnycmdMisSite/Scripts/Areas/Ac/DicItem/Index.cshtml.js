/// <reference path="../../../jquery-1.8.3.intellisense.js" />
/// <reference path="../../../miniui/miniui.js" />
(function (window) {
	mini.namespace("Ac.DicItem.Index");
	var self = Ac.DicItem.Index;
	self.prifix = "Ac_DicItem_Index_";
	self.sortUrl = bootPATH + "../Ac/DicItem/UpdateSortCode";
	self.help = { appSystemCode: "Anycmd", areaCode: "Ac", resourceCode: "DicItem", functionCode: "Index" };
	helper.helperSplitterInOne(self);
	self.add = add;
	self.search = search;

	self.gridReload = function () {
		grid.reload();
	};
	mini.namespace("DicItem.Edit");
	var edit = DicItem.Edit;
	edit.prifix = "Ac_DicItem_Index_Edit_";

	var currentDicRecord;
	var tabConfigs = {
		infoTab: {
			url: bootPATH + "../Ac/DicItem/Details",
			params: [{ "pName": 'id', "pValue": "Id" }],
			namespace: "DicItem.Details"
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

	var key = mini.get(self.prifix + "key");
	key.on("enter", search);
	var tabs1 = mini.get(self.prifix + "tabs1");
	var grid = mini.get(self.prifix + "dgDicItem");
	var dgDic = mini.get(self.prifix + "dgDic");
	dgDic.on("load", helper.onGridLoad);
	dgDic.on("selectionchanged", function (e) {
		currentDicRecord = dgDic.getSelected();
		grid.load({ dicId: currentDicRecord.Id });
		grid.setPageIndex(0);
		grid.sortBy("SortCode", "asc");
	});
	dgDic.load();
	dgDic.sortBy("SortCode", "asc");
	grid.on("drawcell", helper.ondrawcell(self, "Ac.DicItem.Index"));
	grid.on("load", helper.onGridLoad);
	
	helper.index.allInOne(
		edit,
		grid,
		bootPATH + "../Ac/DicItem/Edit",
		bootPATH + "../Ac/DicItem/Edit",
		bootPATH + "../Ac/DicItem/Delete",
		self);
	helper.index.tabInOne(grid, tabs1, tabConfigs, self);

	function add() {
		var data = { action: "new" };
		if (win) {
			win.setTitle("添加");
			win.setIconCls("icon-add");
			win.show();
			if (currentDicRecord && currentDicRecord.Id) {
				data.DicId = currentDicRecord.Id;
			}
			edit.SetData(data);
		}
	}

	function search() {
		var data = { key: key.getValue() };
		if (currentDicRecord && currentDicRecord.Id) {
			data.dicId = currentDicRecord.Id;
		}
		grid.load(data);
	}

	helper.edit.allInOne(
		self,
		win,
		bootPATH + "../Ac/DicItem/Create",
		bootPATH + "../Ac/DicItem/Update",
		bootPATH + "../Ac/DicItem/Get",
		form, edit);
})(window);