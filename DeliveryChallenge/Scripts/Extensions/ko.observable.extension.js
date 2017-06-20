"use strict";

/*----------------------------------------------------------------------*/
/* Observable Extention for Editing
/*----------------------------------------------------------------------*/
ko.observable.fn.beginEdit = function (transaction) {

	var self = this;
	var commitSubscription;
    var rollbackSubscription;

	// get the current value and store it for editing
	if (self.slice)
		self.originalValue = ko.observableArray(self.slice());
	else
		self.originalValue = ko.mapping.toJS(self);

	self.dispose = function () {
		// kill this subscriptions
		commitSubscription.dispose();
		rollbackSubscription.dispose();
	};

	self.commit = function () {
		// update the actual value with the edit value
		self.originalValue = self();

		// dispose the subscriptions
		self.dispose();
	};

	self.rollback = function () {
		// rollback the edit value
		self(self.originalValue);

		// dispose the subscriptions
		self.dispose();
	};

	//  subscribe to the transation commit and reject calls
	commitSubscription = transaction.subscribe(self.commit,
                                                self,
                                                "commit");

	rollbackSubscription = transaction.subscribe(self.rollback,
                                                    self,
                                                    "rollback");

	return self;
}

ko.subscribable.fn.subscribeChanged = function (callback) {
	var savedValue = this.peek();
	return this.subscribe(function (latestValue) {
		var oldValue = savedValue;
		savedValue = latestValue;
		callback(latestValue, oldValue);
	});
};