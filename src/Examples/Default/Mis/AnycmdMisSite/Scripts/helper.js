/// <reference path="jquery-1.8.3.intellisense.js" />
/// <reference path="miniui/miniui.js" />
String.prototype.trim = function () {
	// 用正则表达式将前后空格  
	// 用空字符串替代。  
	return this.replace(/(^\s*)|(\s*$)/g, "");
}
helper = {};
helper.edit = {};
helper.index = {};
helper.details = {};
helper.showTips = function (result) {
	if (result && result.resultType) {
		if (result.resultType == "success") {
			mini.showTips({
				content: result.msg || '成功',
				state: result.state || "success",
				x: "center",
				y: "top",
				timeout: 3000
			});
		}
		else if (result.resultType == "error") {
			helper.error(result.msg, "系统异常", null);
		}
		else {
			mini.alert(result.msg);
		}
	}
}
helper.cellTooltip = function (e) {
	if (!$(e).attr("tooltiped")) {
		$(e).qtip(
		{
			content: {
				text: '<img class="throbber" src="/Content/img/throbber.gif" alt="Loading..." />',
				ajax: {
					url: $(e).attr('rel') // Use the rel attribute of each element for the url to load
				},
				title: {
					text: '详细 - ' + $(e).text(),
					button: '关闭'
				}
			},
			position: {
				at: 'bottom center', // Position the tooltip above the link
				my: 'top center',
				viewport: $(window), // Keep the tooltip on-screen at all times
				effect: false // Disable positioning animation
			},
			show: {
				event: 'click',
				solo: true // Only show one tooltip at a time
			},
			hide: 'unfocus',
			style: {
				classes: 'qtip-cellTooltip qtip-tipped qtip-shadow'
			}
		});
		$(e).attr("tooltiped", "true");
		e.click();
	}
}
helper.refreshTooltip = function (e) {
	var div = $(e).parent().first();
	div.load(div.attr("rel"));
	return false;
}
helper.toggleFieldSet = function (ck) {
	var e = $(ck).parent().parent().parent();
	var dom = e.get(0);
	dom.className = !ck.checked ? "hideFieldset" : "";
}
helper.requesting = function (msg) {
	document.body.showLoading = true;
	setTimeout(function () {
		if (document.body.showLoading) {
			mini.mask({
				el: document.body,
				cls: 'mini-mask-loading',
				html: msg || '使劲加载中...'
			});
		}
	}, 1000);
};
helper.responsed = function () {
	document.body.showLoading = false;
	mini.unmask(document.body);
}
helper.error = function (message, title, callback) {
	mini.showMessageBox({
		title: title || "错误",
		buttons: ["ok"],
		message: message,
		iconCls: 'mini-messagebox-error',
		callback: callback
	});
};
helper.response = function (result, success, fail) {
	if (result) {
		if (result.success != undefined && !result.success) {
			if (fail) fail();
		}
		else {
			if (success) success();
		}
	}
	else {
		if (fail) fail();
	}
	helper.showTips(result);
}
helper.onGridLoad = function (e) {
	var el = $(e.sender.getEl());
	var content = el.find(".mini-panel-body");
	if (e.result.success === false) {
	    content.addClass("errorGridBg");
	    alert(e.xhr.responseText);
	}
	else if (e.data.length == 0) {
		content.addClass("emptyGridBg");
		content.removeClass("errorGridBg");
	}
	else {
		content.removeClass("emptyGridBg");
		content.removeClass("errorGridBg");
	}
};
helper.getParameter = function (name) {
	name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
	var regexS = "[\\?&]" + name + "=([^&#]*)";
	var regex = new RegExp(regexS);
	var results = regex.exec(window.location.search);
	if (results == null)
		return "";
	else
		return decodeURIComponent(results[1].replace(/\+/g, " "));
};
helper.ondrawcell = function (self, namespace) {
	var btnEdit = mini.get(self.prifix + "btnEdit");
	var btnDelete = mini.get(self.prifix + "btnRemove");
	var editPermission = btnEdit && btnEdit.enabled;
	var deletePermission = btnDelete && btnDelete.enabled;

	return function (e) {
		var field = e.field;
		var value = e.value;
		var columnName = e.column.name;
		if (field) {
			switch (field) {
				case "IsEnabled":
					if (value == "正常" || value == "是" || value == "1" || value == true) {
						e.cellHtml = "<span class='icon-enabled width16px'></span>";
					} else if (value == "禁用" || value == "否" || value == "0" || value == false) {
						e.cellHtml = "<span class='icon-disabled width16px'></span>";
					} break;
			}
		}
		if (columnName && columnName == "action") {
			var record = e.record;
			var html = "";
			if (editPermission) {
				html += '<a title="双击行也可以编辑" href="javascript:' + namespace + '.edit(\'' + record.Id + '\')"><img alt="编辑" border="0" src="' + bootPATH + '../Scripts/miniui/themes/icons/edit.gif" /></a>';
			}
			if (deletePermission) {
				html += '&nbsp;&nbsp;<a title="删除" href="javascript:' + namespace + '.remove(\'' + record.Id + '\')"><img alt="删除" border="0" src="' + bootPATH + '../Scripts/miniui/themes/icons/remove.gif" /></a>';
			}
			if (html) {
				e.cellHtml = html;
			}
		}
	};
};
helper.edit.CloseWindow = function (window, win) {
	return function (action) {
		if (win) {
			win.hide();
			if (action == "save" && window.gridReload) {
				window.gridReload();
			}
		}
		else if (window.CloseOwnerWindow) window.CloseOwnerWindow(action);
		else window.close();
	};
};
helper.edit.keyDown = function (win, action) {
	var fn = function (e) {
		var currKey = 0;
		e = e || event;
		currKey = e.keyCode || e.which || e.charCode;
		if (currKey == 13 && win.visible) {
			action();
		}
	};
	document.onkeydown = fn;
	return fn;
};
helper.edit.GetData = function (form) {
	var o = form.getData();
	return o;
};
helper.edit.SetData = function (url, form) {
	return function (data) {
		//跨页面传递的数据对象，克隆后才可以安全使用
		data = mini.clone(data);
		if (data.action == "edit") {
			$.ajax({
				url: url,
				data: { id: data.id },
				cache: false,
				success: function (result) {
					helper.response(result, function () {
						form.setData(result);
						form.validate();
					});
				}
			});
		}
		else if (data.action == "new") {
			form.setData(data);
		}
	};
};
helper.edit.SaveData = function (addConfig, updateUrl, form, edit) {
	return function () {
		var m = $(form.el);
		var id = m.find("input[name='Id']").val();
		var url;
		if (id) {
			url = updateUrl;
		}
		else if (addConfig.url) {
			url = addConfig.url;
			for (var i = 0; i < addConfig.params.length; i++) {
				var param = addConfig.params[i];
				var id = m.find("input[name='" + param.pName + "']").val(param.getValue());
			}
		}
		else {
			url = addConfig;
		}
		var data = m.serialize();
		form.validate();
		if (form.isValid() == false) return;
		helper.requesting();
		$.post(url, data, function (result) {
			helper.response(result, function () {
				edit.CloseWindow("save");
			});
			helper.responsed();
		}, "json");
	};
};
helper.edit.allInOne = function (context, win, addConfig, updateUrl, setDataUrl, form, edit) {
	if (!win || !form) {
		return;
	}
	if (!edit.GetData) {
		edit.GetData = helper.edit.GetData(form);
	}
	if (!edit.CloseWindow) {
		edit.CloseWindow = helper.edit.CloseWindow(context, win);
	}
	if (!edit.SaveData) {
		edit.SaveData = helper.edit.SaveData(addConfig, updateUrl, form, edit);
	}
	if (!edit.SetData) {
		edit.SetData = helper.edit.SetData(setDataUrl, form);
	}

	$().ready(function () {
		edit.keyDown = helper.edit.keyDown(win, edit.SaveData);
		var btnOk = "btnOk";
		var btnCancel = "btnCancel";
		if (edit.prifix) {
			btnOk = edit.prifix + btnOk;
			btnCancel = edit.prifix + btnCancel;
		}
		$("#" + win.id + " ." + btnOk).click(edit.SaveData);
		$("#" + win.id + " ." + btnCancel).click(function () {
			edit.CloseWindow("cancel");
		});
	});
};

helper.index.winReady = function (edit, func) {
	var win1 = "win1";
	var popWin = "popWin";
	if (edit.prifix) {
		win1 = edit.prifix + win1;
		popWin = edit.prifix + popWin;
	}
	var win = mini.get(win1);
	if (!win) {
		var popWin = $("#" + popWin);
		var popWinUrl = popWin.attr("url");
		popWin.load(popWinUrl, null, function () {
			win = mini.get(win1);
			func(win);
		});
	}
	else {
		func(win);
	}
}
helper.index.add = function (edit, url, grid) {
	return function () {
		var data = { action: "new" };
		grid.loading("加载中...");
		helper.index.winReady(edit, function (win) {
			win.setTitle("添加");
			win.setIconCls("icon-add");
			win.show();
			if (edit && edit.SetData) {
				edit.SetData(data);
			}
			grid.unmask();
		});
	};
};
helper.index.edit = function (edit, grid) {
	return function (record) {
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
		var id;
		if (record.Id) {
			id = record.Id;
		}
		else {
			id = record;
		}

		var data = { action: "edit", id: id };
		grid.loading("加载中...");
		helper.index.winReady(edit, function (win) {
			win.setTitle("编辑");
			win.setIconCls("icon-edit");
			win.show();
			if (edit && edit.SetData) {
				edit.SetData(data);
			}
			grid.unmask();
		});
	};
};
helper.index.remove = function (directRemove) {
	return function (records) {
		var id;
		if (typeof records == typeof []) {
			var ids = [];
			for (var i = 0, l = records.length; i < l; i++) {
				var r = records[i];
				ids.push(r.Id);
			}
			id = ids.join(',');
			directRemove(id);
		}
		else {
			mini.confirm("确定删除选中记录？", "确定？", function (action) {
				if (action == "ok") {
					if (typeof records == "string") {
						id = records;
					}
					else if (records && records.Id) {
						id = records.Id;
					}
					directRemove(id);
				}
			});
		}
	};
};
helper.index.directRemove = function (grid, url) {
	return function (id) {
		if (id) {
			grid.loading("操作中，请稍后......");
			$.post(url, { id: id }, function (result) {
				helper.response(result, function () {
					grid.reload();
				}, function () {
					grid.unmask();
				});
			}, "json");
		}
	};
};
helper.index.clearSearch = function (filters, calback) {
	for (var k in filters) {
		var filter = filters[k];
		var filterBox = filter.filterBox;
		if (filterBox && filterBox.setValue) {
			filterBox.setValue("");
			filter.value = "";
		}
	}
	if (calback) {
		calback();
	}
}
helper.index.onFilterChanged = function (filters, calback) {
	return function (e) {
		var textbox = e.sender;
		var value = textbox.getValue();
		var name = textbox.name;
		var oldValue = filters[name].value;
		filters[name].value = value;
		if (!textbox.onEnter) {
			if (calback) {
				textbox.on("enter", function (e) {
					calback();
				});
			}
		}
		if (calback) {
			if ((!oldValue && value) || (oldValue && oldValue != value)) {
				calback();
			}
		}
	};
}
helper.index.allInOne = function (edit, grid, addUrl, editUrl, directRemoveUrl, self) {
	if (!self.add && edit) {
		self.add = helper.index.add(edit, addUrl, grid);
	}
	if (!self.edit && edit) {
		self.edit = helper.index.edit(edit, grid);
	}
	if (!self.directRemove) {
		self.directRemove = helper.index.directRemove(grid, directRemoveUrl);
	}
	if (!self.remove) {
		self.remove = helper.index.remove(self.directRemove);
	}
	if (self.filters) {
		var btnSearchClear = "btnSearchClear";
		if (self.prifix) {
			btnSearchClear = self.prifix + btnSearchClear;
		}
		var btnSearchClear = mini.get(btnSearchClear);
		if (btnSearchClear) {
		    btnSearchClear.on("click", function () {
		        helper.index.clearSearch(self.filters, self.search);
		    });
		}
		for (var k in self.filters) {
			var id = k + "Filter";
			if (self.prifix) {
				id = self.prifix + id;
			}
			var filterBox = mini.get(id);
			self.filters[k].filterBox = filterBox;
			filterBox.on("valuechanged", helper.index.onFilterChanged(self.filters, self.search));
		}
	}
	var splitter = "splitter";
	var btnVSplit = "btnVSplit";
	var btnHSplit = "btnHSplit";
	if (self.prifix) {
		splitter = self.prifix + splitter;
		btnVSplit = self.prifix + btnVSplit;
		btnHSplit = self.prifix + btnHSplit;
	}
	var splitter = mini.get(splitter);
	var btnVSplit = mini.get(btnVSplit);
	var btnHSplit = mini.get(btnHSplit);
	helper.index.splitterInOne(splitter, btnVSplit, btnHSplit);

	$().ready(function () {
		var btnAdd = "btnAdd";
		var btnEdit = "btnEdit";
		var btnRemove = "btnRemove";
		var btnProSearch = "btnProSearch";
		var btnSearch = "btnSearch";
		var btnEnable = "btnEnable";
		var btnDisable = "btnDisable";
		var btnMoveUp = "btnMoveUp";
		var btnMoveDown = "btnMoveDown";
		if (self.prifix) {
			btnAdd = self.prifix + btnAdd;
			btnEdit = self.prifix + btnEdit;
			btnRemove = self.prifix + btnRemove;
			btnProSearch = self.prifix + btnProSearch;
			btnSearch = self.prifix + btnSearch;
			btnEnable = self.prifix + btnEnable;
			btnDisable = self.prifix + btnDisable;
			btnMoveUp = self.prifix + btnMoveUp;
			btnMoveDown = self.prifix + btnMoveDown;
		}
		var miniBtnEdit = mini.get(btnEdit);
		var editPermission = miniBtnEdit && miniBtnEdit.enabled;
		if (self.add) {
			var btn = mini.get(btnAdd);
			if (btn) {
				btn.on("click", self.add);
			}
		}
		if (self.edit) {
			if (miniBtnEdit) {
				miniBtnEdit.on("click", function () {
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
					var record = records[0];
					if (record) {
						self.edit(record);
					} else {
						mini.showTips({
							content: "没有选中记录",
							state: "warning",
							x: "center",
							y: "top",
							timeout: 3000
						});
					}
				});
			}
		}
		if (self.remove) {
			var btn = mini.get(btnRemove);
			if (btn) {
				btn.on("click", function () {
					var records = grid.getSelecteds();
					if (records.length > 0) {
						mini.confirm("确定删除选中记录？", "确定？", function (action) {
							if (action == "ok") {
								self.remove(records);
							}
						});
					} else {
						mini.showTips({
							content: "没有选中记录",
							state: "warning",
							x: "center",
							y: "top",
							timeout: 3000
						});
					}
				});
			}
		}
		if (self.search) {
			var btn = mini.get(btnSearch);
			if (btn) {
				btn.on("click", self.search);
			}
		}
		if (self.enable) {
			var btn = mini.get(btnEnable);
			if (btn) {
				btn.on("click", self.enable);
			}
		}
		if (self.disable) {
			var btn = mini.get(btnDisable);
			if (btn) {
				btn.on("click", self.disable);
			}
		}
		if (self.edit && editPermission) {
			grid.on("rowdblclick", function (e) {
				self.edit(e.record);
			});
		}
		var btnMoveUp = mini.get(btnMoveUp);
		if (self.sortUrl) {
			if (btnMoveUp) {
				btnMoveUp.on("click", function (e) {
					var record = grid.getSelected();
					if (record) {
						var currentIndex = grid.indexOf(record);
						var preIndex = currentIndex - 1;
						var preRecord = grid.getRow(preIndex);
						if (preRecord) {
							if (record.SortCode != preRecord.SortCode) {
								var preSortCode = preRecord.SortCode;
								var sortCode = record.SortCode;
								$.post(self.sortUrl
									, { id1: preRecord.Id, sortCode1: sortCode, id2: record.Id, sortCode2: preSortCode }
									, function (result) {
										if (result.success) {
											grid.moveRow(record, preIndex);
											grid.updateRow(record, { SortCode: preSortCode });
											grid.updateRow(preRecord, { SortCode: sortCode });
										}
									}, "json");
							}
							else {
								mini.alert("向上移动受阻，因为上一条记录和本条记录的排序码相等，请调整");
							}
						}
					}
				});
			}
			var btnMoveDown = mini.get(btnMoveDown);
			if (btnMoveDown) {
				btnMoveDown.on("click", function (e) {
					var record = grid.getSelected();
					if (record) {
						var currentIndex = grid.indexOf(record);
						var nextIndex = currentIndex + 1;
						var nextRecord = grid.getRow(nextIndex);
						if (nextRecord) {
							if (record.SortCode != nextRecord.SortCode) {
								var nextSortCode = nextRecord.SortCode;
								var sortCode = record.SortCode;
								$.post(self.sortUrl
									, { id1: nextRecord.Id, sortCode1: sortCode, id2: record.Id, sortCode2: nextSortCode }
									, function (result) {
										if (result.success) {
											grid.moveRow(nextRecord, nextIndex - 1);
											grid.updateRow(record, { SortCode: nextSortCode });
											grid.updateRow(nextRecord, { SortCode: sortCode });
										}
									}, "json");
							}
							else {
								mini.alert("向下移动受阻，因为下一条记录和本条记录的排序码相等，请调整");
							}
						}
					}
				});
			}
		}
	});
};
helper.index.onSelectionChanged = function (tabs1, loadTabData, self) {
	return function (e) {
		var splitter = "splitter";
		var btnVSplit = "btnVSplit";
		var btnHSplit = "btnHSplit";
		if (self.prifix) {
			splitter = self.prifix + splitter;
			btnVSplit = self.prifix + btnVSplit;
			btnHSplit = self.prifix + btnHSplit;
		}
		var splitter = mini.get(splitter);
		var btnVSplit = mini.get(btnVSplit);
		var btnHSplit = mini.get(btnHSplit);
		if (splitter && btnVSplit && btnHSplit) {
			var pane1 = splitter.getPane(1);
			var pane2 = splitter.getPane(2);
			var $pane2El = $(splitter.getPaneEl(2));
			if (!$pane2El.width() || !$pane2El.height()) {
				return false;
			}
		}

		var grid = e.sender;
		var record;
		if (grid.getSelecteds) {
			var records = grid.getSelecteds();
			if (records.length == 1) {
				record = records[0];
			}
		}
		else if (grid.getSelectedNode) {
			record = grid.getSelectedNode();
		}
		if (records.length == 1) {
			record = records[0];
		}
		if (record) {
			var currentRecord = record;
			var isNewRow = currentRecord.isNewRow;
			var editTab;
			var allTabs = tabs1.getTabs();
			for (var i = 0; i < allTabs.length; i++) {
				var tab = allTabs[i];
				if (tab.name == "editTab") {
					if (isNewRow && tab.title == "编辑") {
						tabs1.updateTab(tab, { title: "添加" });
					}
					if (!isNewRow && tab.title == "添加") {
						tabs1.updateTab(tab, { title: "编辑" });
					}
					editTab = tab;
				}
				else {
					if (isNewRow && tab.visible) {
						tabs1.updateTab(tab, { visible: false });
					}

					if (!isNewRow && !tab.visible) {
						tabs1.updateTab(tab, { visible: true });
					}
				}
			}
			if (isNewRow) {
				tabs1.activeTab(editTab);
				loadTabData("refresh", editTab, currentRecord);
			}
			else {
				loadTabData("refresh", tabs1.getActiveTab(), currentRecord);
			}
			tabs1.show();
		}
		else {
			tabs1.hide();
		}
	};
};
helper.index.loadTabData = function (tabConfigs, tabs1) {
	return function (refresh, tab, currentRecord) {
		var tabName = tab.name;
		var tabConfig = tabConfigs[tabName];
		if (!tabConfig) {
			helper.error("意外的tabName:" + tabName);
		}
		if (refresh || !tabConfig['initialized']) {
			var urlParams = {};
			if (tabConfig.entityTypeCode) {
				urlParams.entityTypeCode = tabConfig.entityTypeCode;
			}
			if (tabConfig.controller) {
				urlParams.controller = tabConfig.controller;
			}
			window.location.href = $.param.fragment(window.location.href, urlParams);
			if (currentRecord && currentRecord.Id) {
				var tabBody = $(tabs1.getTabBodyEl(tab));
				var isInner = tabBody.hasClass("inner");
				var params = {};
				for (var i = 0; i < tabConfig.params.length; i++) {
					if (currentRecord[(tabConfig.params)[i].pValue]) {
						params[(tabConfig.params)[i].pName] = currentRecord[(tabConfig.params)[i].pValue];
					}
					else {
						params[(tabConfig.params)[i].pName] = (tabConfig.params)[i].pValue;
					}
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
			}
			tabConfig['initialized'] = true;
		}

		if (refresh) {
			for (var key in tabConfigs) {
				tabConfigs[key]['initialized'] = false;
			}
			tabConfig['initialized'] = true;
		}
	};
};
helper.index.ontabload = function (tabConfigs) {
	return function (e) {
		var tabs = e.sender;
		var tab = e.tab;
		var tabName = tab.name;
		if ($(tabs.getTabBodyEl(tab)).hasClass("inner")) {

		}
		else {
			var iframe = tabs.getTabIFrameEl(e.tab);
			var tabConfig = tabConfigs[tabName];
			if (tabConfig) {
				tabConfig['iframe'] = iframe;
			}
		}
	};
};
helper.index.onactivechanged = function (grid, self) {
	return function (e) {
		var record;
		if (grid.getSelecteds) {
			var records = grid.getSelecteds();
			if (records.length == 1) {
				record = records[0];
			}
		}
		else if (grid.getSelectedNode) {
			record = grid.getSelectedNode();
		}
		if (record) {
			var currentRecord = record;
			self.loadTabData(null, e.tab, currentRecord);
		}
	};
};
helper.index.tabInOne = function (grid, tabs1, tabConfigs, self) {
	if (!self.loadTabData) {
		self.loadTabData = helper.index.loadTabData(tabConfigs, tabs1);
	}
	if (!self.onSelectionChanged) {
		self.onSelectionChanged = helper.index.onSelectionChanged(tabs1, self.loadTabData, self);
	}
	if (!self.ontabload) {
		self.ontabload = helper.index.ontabload(tabConfigs);
	}
	if (!self.onactivechanged) {
		self.onactivechanged = helper.index.onactivechanged(grid, self);
	}
	if (grid.getSelected) {
		grid.on("selectionchanged", self.onSelectionChanged);
		grid.on("beforeload", function (e) {
			if (grid.__total) {
				e.params.total = grid.__total
			}
		});
		grid.on("load", function (result) {
			grid.__total = result.total;
			var record = grid.getSelected();
			if (!record) {
				tabs1.hide();
			}
			else {
				tabs1.show();
			}
		});
	}
	else if (grid.getSelectedNode) {
		grid.on("nodeclick", self.onSelectionChanged);
	}
	tabs1.on("tabload", self.ontabload);
	tabs1.on("activechanged", self.onactivechanged);
	tabs1.hide();
};
helper.index.splitterInOne = function (splitter, btnVSplit, btnHSplit) {
	if (splitter && btnVSplit && btnHSplit) {
		var pane1 = splitter.getPane(1);
		var pane2 = splitter.getPane(2);
		var size = pane1.size;
		btnVSplit.on("checkedchanged", function () {
			pane2.size = "";
			pane1.size = size;
			splitter.setVertical(true);
		});
		btnHSplit.on("checkedchanged", function () {
			pane1.size = "";
			pane2.size = 400;
			splitter.setVertical(false);
		});
	}
};
helper.helperSplitterInOne = function (self) {
	$().ready(function () {
		var helperSplitter = "helperSplitter";
		var btnCollapse = "btnCollapse";
		var helpContent = "helpContent";
		var btnHelp = "btnHelp";
		var btnEditHelp = "btnEditHelp";
		var btnRefreshHelp = "btnRefreshHelp";
		if (self.prifix) {
			helperSplitter = self.prifix + helperSplitter;
			btnCollapse = self.prifix + btnCollapse;
			helpContent = self.prifix + helpContent;
			btnHelp = self.prifix + btnHelp;
			btnEditHelp = self.prifix + btnEditHelp;
			btnRefreshHelp = self.prifix + btnRefreshHelp;
		}
		$("#" + helperSplitter).children("div.mini-splitter-border")
			.css({ "border-top": "0px", "border-right": "0px", "border-left": "0px", "border-bottom": "0px" });
		var splitter = mini.get(helperSplitter);
		var helpline = mini.get(helpContent);
		var editHelp = mini.get(btnEditHelp);
		var refreshHelp = mini.get(btnRefreshHelp);
		if (splitter) {
			var pane1 = splitter.getPane(2);
			var body = $(helpline.getBodyEl());
			self.loadHelp = function () {
				helper.requesting();
				splitter.updatePane(2, { visible: true });
				splitter.expandPane(2);
				self.help.isInner = true;
				$.get(bootPATH + "../Ac/Help/Helpline", self.help, function (result) {
					helper.responsed();
					body.html(result);
				});
			};
			self.toggleHelp = function () {
				if (pane1.visible) {
					if (pane1.expanded) {
						splitter.collapsePane(2);
					}
					else {
						splitter.expandPane(2);
					}
				}
				else {
					self.loadHelp();
				}
			};
			var help = $("." + btnHelp);
			help.click(self.toggleHelp);
			refreshHelp.on("click", self.loadHelp);
			editHelp.on("click", function (e) {
				var btn = e.sender;
				var editHref = bootPATH + "../Ac/Help/Edit?appSystemCode=" + self.help.appSystemCode + "&areaCode=" + self.help.areaCode + "&resourceCode=" + self.help.resourceCode + "&functionCode=" + self.help.functionCode;
				var tab = {};
				tab.text = btn.getTooltip();
				tab.url = editHref;
				window.top.topShowTab(tab);
				return false;
			});
		}
	});
};
