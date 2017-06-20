//"use strict";

/*----------------------------------------------------------------------*/
/* View Model
/*----------------------------------------------------------------------*/
function DataModel(data) {
	var self = this;
	var objKeys = Object.keys(data);
	for (var i = 0, objLen = objKeys.length; i < objLen; i++) {
		self[objKeys[i]] = ko.observable(data[objKeys[i]]);
	}
};

DataModel.prototype.beginEdit = function (transaction) {
	var objKeys = Object.keys(this);
	for (var i = 0, objLen = objKeys.length; i < objLen; i++) {
		this[objKeys[i]].beginEdit(transaction);
	}
};

/*----------------------------------------------------------------------*/
/* View Model
/*----------------------------------------------------------------------*/
function gridViewModel(options) {
	var self = this;
	self.name = options.name;

	self.modalBodyTemplate = function() {
		return options.modalBody || 'modalBody';
	};

	self.modalTitleTemplate = function () {
		return options.modalTitle || 'modalTitle';
	};

	self.tableHeaderTemplate = function () {
		return options.tableHeader || 'tableHeader';
	};

	self.tableBodyTemplate = function () {
		return options.tableBody || 'tableBody';
	};

	self.enableAddItem = ko.observable(options.enableAddItem || false);

	//pager data
	self.page = ko.observable(1);
	self.records = ko.observable(3);
	self.totalPages = ko.observable(1);
	self.rowsPerPage = ko.observable(3);
	self.pagesText = ko.computed(function () {
		return self.page() + " of " + self.totalPages() + " pages";
	});

	self.pageFirst = function pageFirst(item) {
		self.page(1);
		self.getData();
	};

	self.pageLast = function pageLast(item) {
		self.page(self.totalPages());
		self.getData();
	};

	self.pageBack = function (item) {
		self.page(self.page() - 1);
		self.getData();
	};

	self.pageNext = function (item) {
		self.page(self.page() + 1);
		self.getData();
	};

	self.getData = function () {

		//if ((self.records() / self.rowsPerPage() < self.page()) && self.page() > 1) {
		//	self.page(self.page() - 1);
		//}

		var requestData = { rows: self.rowsPerPage(), page: self.page() };
		if (self.editingItem() != null) {
			requestData.data = ko.mapping.toJS(self.editingItem());
		}

		$.ajax({
			type: 'POST',
			url: options.getUrl,
			data: JSON.stringify(requestData),
			contentType: 'application/json;',
			dataType: 'json',
			success: function (result) {
				self.records(result.TotalRowCount);
				self.totalPages(result.TotalPageCount);

				self.data.removeAll();

				for (var i = 0; i < result.Data.length; i++) {
					var item = new DataModel(result.Data[i]);
					self.data.push(item);
				};
			},
			error: function (result) {
				alert("error");
			}
		});
	};

	//  data
	self.data = ko.observableArray([]);
	self.editingItem = ko.observable();

	//  create the transaction for commit and reject
	self.editTransaction = new ko.subscribable();

	//  helpers
	self.isItemEditing = function (item) {
		return item === self.editingItem();
	};

	//  behaviour
	self.addItem = function () {
		var item = new DataModel(ko.mapping.toJS(options.newItem));
		//self.data.push(item);

		//  begin editing the new item straight away
		self.editItem(item);
	};

	self.removeItem = function (item) {
		self.editingItem(null);

		var answer = true; // confirm('Are you sure you want to delete this fruit? ' + fruit.name());
		if (answer) {

			$.ajax({
				type: 'POST',
				url: options.removeUrl,
				data: JSON.stringify(ko.mapping.toJS(item)),
				contentType: 'application/json;',
				dataType: 'json',
				success: function (result) {
					if (result.Success) {
						
						if (self.page() !== 1 && (self.records() % self.rowsPerPage()) <= self.page()) {
							self.page(self.page() - 1);
						}
						self.getData();
					} else {
						alert(result.Message);
					}
				},
				error: function (result) {
					alert("error");
				}
			});

			//self.data.remove(item);
		}
	};

	self.editItem = function (item) {
		if (self.editingItem() == null) {
			// start the transaction

			item.beginEdit(self.editTransaction);

			// shows the edit fields
			self.editingItem(item);
		}
	};

	self.applyEdit = function (item) {
		//  commit the edit transaction
		var isNew = self.data.indexOf(item) === -1;
		var url = isNew ? options.addUrl : options.editUrl;

		$.ajax({
			type: 'POST',
			url: url,
			data: JSON.stringify(ko.mapping.toJS(item)),
			contentType: 'application/json;',
			dataType: 'json',
			success: function (result) {
				if (result.Success) {
					self.editTransaction.notifySubscribers(null, "commit");
					if (isNew) {

						if (self.records() < self.rowsPerPage() || 
							(self.page() === self.totalPages() && 
							(self.records() / self.rowsPerPage()) < self.totalPages())) {
							var item = new DataModel(result.Data);
							self.data.push(item);
						}

						self.records(self.records() + 1);
						if (self.records() > self.rowsPerPage() && self.records() % self.rowsPerPage() !== 0)
							self.totalPages(self.totalPages() + 1);
					}
					self.editingItem(null);
				} else {
					//self.editTransaction.notifySubscribers(null, "rollback");
					alert(result.Message);
				}

			},
			error: function (result) {
				//self.editTransaction.notifySubscribers(null, "rollback");
				if (result.responseJSON != null && result.responseJSON.Message != null) {
					alert(result.responseJSON.Message);
				} else {
					alert('error');
				}
			},
			//complete: function () {
			//	//  hides the edit fields
			//	self.editingItem(null);
			//}
		});
	};

	self.cancelEdit = function (item) {
		//  reject the edit transaction
		self.editTransaction.notifySubscribers(null, "rollback");

		//  hides the edit fields
		self.editingItem(null);
	};

	$(document).keyup(function (e) {
		if (e.keyCode === 27 && self.editingItem() != null) { // escape key maps to keycode `27`
			self.cancelEdit(self.editingItem());
		}
	});

	return self;
};
