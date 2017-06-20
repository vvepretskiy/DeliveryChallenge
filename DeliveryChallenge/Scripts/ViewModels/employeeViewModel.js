"use strict";

$(document)
	.ready(function () {
		var self = new gridViewModel({
			getUrl: 'employee/get',
			editUrl: 'employee/edit',
			addUrl: 'employee/add',
			removeUrl: 'employee/remove',
			enableAddItem: true,
			name: 'employees',
			newItem: { Id: 0, FirstName: null, SecondName: null, Skills: [], Deliveries: [] }
		});

		self.skillGrid = ko.observable();
		self.employeeSkillGrid = ko.observable();
		self.deliveryGrid = ko.observable();
		self.employeeDeliveryGrid = ko.observable();

		self.editItem = (function (originalEditItem) {
			return function(item) {
				originalEditItem(item);

				var pushItme = function(items, item) {
					for (var i = 0, itemsLen = items.length; i < itemsLen; i++) {
						if (items[i].Id === item.Id) {
							return items[i];
						}
					}
					items.push(item);
					return item;
				};

				var selectSkill = function (item) {
					item = pushItme(self.editingItem().Skills(), ko.mapping.toJS(item));

					item.EntityState = this.name === 'skills' ? 4 : 8;

					self.skillGrid().getData();
					self.employeeSkillGrid().getData();
				};

				var skillViewModel = new gridViewModel({
					getUrl: 'skill/GetAvailableEmployeeSkills',
					tableHeader: 'skillTableHeader',
					tableBody: 'skillTableBody',
					enableAddItem: false,
					name: 'skills'
				});
				skillViewModel.editItem = selectSkill;
				skillViewModel.editingItem(self.editingItem());
				skillViewModel.getData();
				self.skillGrid(skillViewModel);

				var employeeSkillViewModel = new gridViewModel({
					getUrl: 'skill/GetEmployeeSkills',
					tableHeader: 'skillTableHeader',
					tableBody: 'skillTableBody',
					enableAddItem: false,
					name: 'employeeSkills'
				});
				employeeSkillViewModel.editItem = selectSkill;
				employeeSkillViewModel.editingItem(self.editingItem());
				employeeSkillViewModel.getData();
				self.employeeSkillGrid(employeeSkillViewModel);

				var selectDelivery = function (item) {
					item = pushItme(self.editingItem().Deliveries(), ko.mapping.toJS(item));

					item.EntityState = this.name === 'deliveries' ? 4 : 8;

					self.deliveryGrid().getData();
					self.employeeDeliveryGrid().getData();
				};

				var deliveryViewModel = new gridViewModel({
					getUrl: 'delivery/GetAvailableEmployeeDeliveries',
					tableHeader: 'deliveryTableHeader',
					tableBody: 'deliveryTableBody',
					enableAddItem: false,
					name: 'deliveries'
				});
				deliveryViewModel.editItem = selectDelivery;
				deliveryViewModel.editingItem(self.editingItem());
				deliveryViewModel.getData();
				self.deliveryGrid(deliveryViewModel);

				var employeeDeliveryViewModel = new gridViewModel({
					getUrl: 'delivery/GetEmployeeDeliveries',
					tableHeader: 'deliveryTableHeader',
					tableBody: 'deliveryTableBody',
					enableAddItem: false,
					name: 'employeeDeliveries'
				});
				employeeDeliveryViewModel.editItem = selectDelivery;
				employeeDeliveryViewModel.editingItem(self.editingItem());
				employeeDeliveryViewModel.getData();
				self.employeeDeliveryGrid(employeeDeliveryViewModel);
			}
		})(self.editItem);

		self.getData();
		ko.applyBindings(self);
	});
