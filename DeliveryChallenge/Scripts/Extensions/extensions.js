'use strict';

String.prototype.addParameterToURL = function (param) {
	var self = this;
	self += (self.split('?')[1] ? '&' : '?') + param;
	return self;
};