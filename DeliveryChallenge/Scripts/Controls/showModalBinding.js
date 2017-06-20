"use strict";

ko.bindingHandlers.showModal = {
	init: function (element, valueAccessor, allBindings, viewModel, bindingContext) {
		//$(element).draggable({ cancel: ".modal-body,.modal-footer" });

		var modalsDiv = $("body").find("#modalsDiv");

		if (modalsDiv.length === 0) {
			modalsDiv = $("<div id='modalsDiv'></div>");
			$("body").append(modalsDiv);
		}
	},
	update: function (element, valueAccessor) {
		var value = valueAccessor();
		if (ko.utils.unwrapObservable(value)) {
			$(element).modal('show');
		}
		else {
			$(element).modal('hide');
		}
	}
};