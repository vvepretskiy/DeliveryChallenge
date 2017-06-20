"use strict";

$(document)
	.ready(function () {
		var viewModel = new gridViewModel({
			getUrl: 'skill/get',
			editUrl: 'skill/edit',
			addUrl: 'skill/add',
			enableAddItem: true,
			newItem: { Id: 0, Name: null }
		});
		viewModel.getData();
		ko.applyBindings(viewModel);
	});
