"use strict";

$(document)
	.ready(function () {
		var self = new gridViewModel({
			getUrl: 'delivery/get',
			editUrl: 'delivery/edit',
			addUrl: 'delivery/add',
			removeUrl: 'delivery/remove',
			enableAddItem: true,
			name: 'deliveries',
			newItem: { Id: 0, Name: null, Type: { Id: 1, Name: 'Tes' }, DeliveryTypeId: 1 , Skills: [], Employees: [], Details: [] }
		});

		self.skillGrid = ko.observable();
		self.deliverySkillGrid = ko.observable();
		self.employeeGrid = ko.observable();
		self.deliveryEmployeeGrid = ko.observable();
		self.deliveryTypes = ko.observableArray();

		self.selectDefaultType = function (option, item) {
			if (item.Id === self.editingItem().DeliveryTypeId()) {
				ko.applyBindingsToNode(option.parentElement, { value: item }, item);
			}
		};

		self.editItem = (function (originalEditItem) {
			return function (item) {
				item.DeliveryTypeId(item.Type().Id);
				originalEditItem(item);

				if (!self.deliveryTypes().length) {
					$.get(
						'deliveryType/Get',
						function (data) {
							self.deliveryTypes(data);
						},
						'json'
					);
				}

				var pushItme = function (items, item) {
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
					self.deliverySkillGrid().getData();
				};

				var skillViewModel = new gridViewModel({
					getUrl: 'skill/GetAvailableDeliverySkills',
					tableHeader: 'skillTableHeader',
					tableBody: 'skillTableBody',
					enableAddItem: false,
					name: 'skills'
				});
				skillViewModel.editItem = selectSkill;
				skillViewModel.editingItem(self.editingItem());
				skillViewModel.getData();
				self.skillGrid(skillViewModel);

				var deliverySkillViewModel = new gridViewModel({
					getUrl: 'skill/GetDeliverySkills',
					tableHeader: 'skillTableHeader',
					tableBody: 'skillTableBody',
					enableAddItem: false,
					name: 'deliverySkills'
				});
				deliverySkillViewModel.editItem = selectSkill;
				deliverySkillViewModel.editingItem(self.editingItem());
				deliverySkillViewModel.getData();
				self.deliverySkillGrid(deliverySkillViewModel);

				var selectEmployee = function (item) {
					item = pushItme(self.editingItem().Employees(), ko.mapping.toJS(item));

					item.EntityState = this.name === 'employees' ? 4 : 8;

					self.employeeGrid().getData();
					self.deliveryEmployeeGrid().getData();
				};

				var employeeViewModel = new gridViewModel({
					getUrl: 'employee/GetAvailableDeliveryEmployee',
					tableHeader: 'employeeTableHeader',
					tableBody: 'employeeTableBody',
					enableAddItem: false,
					name: 'employees'
				});
				employeeViewModel.editItem = selectEmployee;
				employeeViewModel.editingItem(self.editingItem());
				employeeViewModel.getData();
				self.employeeGrid(employeeViewModel);

				var deliveryEmployeeViewModel = new gridViewModel({
					getUrl: 'employee/GetDeliveryEmployees',
					tableHeader: 'employeeTableHeader',
					tableBody: 'employeeTableBody',
					enableAddItem: false,
					name: 'deliveryEmployees'
				});
				deliveryEmployeeViewModel.editItem = selectEmployee;
				deliveryEmployeeViewModel.editingItem(self.editingItem());
				deliveryEmployeeViewModel.getData();
				self.deliveryEmployeeGrid(deliveryEmployeeViewModel);
			}
		})(self.editItem);

		self.editingItem.subscribeChanged(function () {
			if (self.editingItem() == null) {
				self.getData();
			}
		});

		self.getData();
		ko.applyBindings(self);
	});
