/// <reference path="Libs/jquery-1.7.2.js" />
/// <reference path="Libs/jquery-ui-1.8.20.js" />
/// <reference path="Libs/jquery.validate.js" />
/// <reference path="Libs/jquery.validate.unobtrusive.js" />
/// <reference path="Libs/knockout-2.0.0.debug.js" />
/// <reference path="Libs/modernizr-2.0.6-development-only.js" />
//alert("inside avitrade-startup.js");
var dataService = new function () {
	var purchasesMonthlyAverageSpends = null,
		purchasesMonthlyTotalSpends = null,
		serviceBase = '/DataService/',
		getPurchasesMonthlyAverageSpendsDataPoints = function (callback, error) {
			$.getJSON(serviceBase + 'RetrievePurchaseMonthlyAverageSpends', {}, function (data) {
				// Cache the data until further call
				purchasesMonthlyAverageSpends = data;
				callback(data);
			}).error(error);
		},
		getPurchasesMonthlyAverageSpendsDataPointsFromCache = function (callback) {
			// return the cached data immediately
			callback(purchasesMonthlyAverageSpends);
		},
		getPurchasesMonthlyAverageSpendsByMerchantDataPoints = function (id, callback, error) {
			$.getJSON(serviceBase + 'RetrievePurchaseMonthlyAverageSpendsByMerchant', {id: id}, function (data) {
				// Cache the data until further call
				purchasesMonthlyAverageSpends = data;
				callback(data);
			}).error(error);
		},
		getPurchasesMonthlyAverageSpendsByMembershipDataPoints = function (id, callback, error) {
			$.getJSON(serviceBase + 'RetrievePurchaseMonthlyAverageSpendsByMembership', { id: id }, function (data) {
				// Cache the data until further call
				purchasesMonthlyAverageSpends = data;
				callback(data);
			}).error(error);
		},
		getPurchasesMonthlyTotalSpendsDataPoints = function (callback, error) {
			$.getJSON(serviceBase + 'RetrievePurchaseMonthlyTotalSpends', {}, function (data) {
				// Cache the data until further call
				purchasesMonthlyTotalSpends = data;
				callback(data);
			}).error(error);
		},
		getPurchasesMonthlyTotalSpendsDataPointsFromCache = function (callback) {
			// return the cached data immediately
			callback(purchasesMonthlyTotalSpends);
		},
		getPurchasesMonthlyTotalSpendsByMerchantDataPoints = function (id, callback, error) {
			$.getJSON(serviceBase + 'RetrievePurchaseMonthlyTotalSpendsByMerchant', {id: id}, function (data) {
				// Cache the data until further call
				purchasesMonthlyTotalSpends = data;
				callback(data);
			}).error(error);
		},
		getPurchasesMonthlyTotalSpendsByMembershipDataPoints = function (id, callback, error) {
			$.getJSON(serviceBase + 'RetrievePurchaseMonthlyTotalSpendsByMembership', { id: id }, function (data) {
				// Cache the data until further call
				purchasesMonthlyTotalSpends = data;
				callback(data);
			}).error(error);
		},
		getMerchantsDistributionByCashDiscountDataPoints = function (callback, error) {
			$.getJSON(serviceBase + 'RetrieveMerchantsDistributionByCashDiscount', {}, function (data) {
				callback(data);
			}).error(error);
		},
		getMerchantsDistributionByTotalDiscountDataPoints = function (callback, error) {
			$.getJSON(serviceBase + 'RetrieveMerchantsDistributionByTotalDiscount', {}, function (data) {
				callback(data);
			}).error(error);
		},
		getMerchantsDistributionByTypeDataPoints = function (callback, error) {
			$.getJSON(serviceBase + 'RetrieveMerchantsDistributionByType', {}, function (data) {
				callback(data);
			}).error(error);
		},
		getMembersDistributionByCityDataPoints = function (callback, error) {
			$.getJSON(serviceBase + 'RetrieveMembersDistributionByCity', { }, function(data) {
				callback(data);
			}).error(error);
		},
		getMembersDistributionByChannelDataPoints = function(callback, error) {
			$.getJSON(serviceBase + 'RetrieveMembersDistributionByEnrollmentChannel', {}, function (data) {
				callback(data);
			}).error(error);
		},
		getMembersDistributionByCitizenshipCountryDataPoints = function (callback, error) {
			$.getJSON(serviceBase + 'RetrieveMembersDistributionByCitizenshipCountry', {}, function (data) {
				callback(data);
			}).error(error);
		},
		getMonthlyEnrollmentsByMerchantDataPoints = function (id, callback, error) {
			$.getJSON(serviceBase + 'RetrieveMonthlyEnrollmentsByMerchant', {id: id}, function (data) {
				callback(data);
			}).error(error);
		},
		getMonthlyCashDiscountVsReferralBonusByMerchant = function (id, callback, error) {
			$.getJSON(serviceBase + 'RetrieveMonthlyCashDiscountVsReferralBonusByMerchant', { id: id }, function (data) {
				callback(data);
			}).error(error);
		},
        getMonthlyTotalBondAmountsBySponsor = function (id, callback, error) {
            $.getJSON(serviceBase + 'RetrieveMonthlyTotalBondAmountsBySponsor', { id: id }, function (data) {
                callback(data);
            }).error(error);
        };

	return {
		getPurchasesMonthlyAverageSpendsDataPoints: getPurchasesMonthlyAverageSpendsDataPoints,
		getPurchasesMonthlyAverageSpendsDataPointsFromCache: getPurchasesMonthlyAverageSpendsDataPointsFromCache,
		getPurchasesMonthlyAverageSpendsByMerchantDataPoints: getPurchasesMonthlyAverageSpendsByMerchantDataPoints,
		getPurchasesMonthlyAverageSpendsByMembershipDataPoints: getPurchasesMonthlyAverageSpendsByMembershipDataPoints,
		getPurchasesMonthlyTotalSpendsDataPoints: getPurchasesMonthlyTotalSpendsDataPoints,
		getPurchasesMonthlyTotalSpendsDataPointsFromCache: getPurchasesMonthlyTotalSpendsDataPointsFromCache,
		getPurchasesMonthlyTotalSpendsByMerchantDataPoints: getPurchasesMonthlyTotalSpendsByMerchantDataPoints,
		getPurchasesMonthlyTotalSpendsByMembershipDataPoints: getPurchasesMonthlyTotalSpendsByMembershipDataPoints,
		getMerchantsDistributionByCashDiscountDataPoints: getMerchantsDistributionByCashDiscountDataPoints,
		getMerchantsDistributionByTotalDiscountDataPoints: getMerchantsDistributionByTotalDiscountDataPoints,
		getMerchantsDistributionByTypeDataPoints: getMerchantsDistributionByTypeDataPoints,
		getMembersDistributionByCityDataPoints: getMembersDistributionByCityDataPoints,
		getMembersDistributionByChannelDataPoints: getMembersDistributionByChannelDataPoints,
		getMembersDistributionByCitizenshipCountryDataPoints: getMembersDistributionByCitizenshipCountryDataPoints,
		getMonthlyEnrollmentsByMerchantDataPoints: getMonthlyEnrollmentsByMerchantDataPoints,
		getMonthlyCashDiscountVsReferralBonusByMerchant: getMonthlyCashDiscountVsReferralBonusByMerchant,
		getMonthlyTotalBondAmountsBySponsor: getMonthlyTotalBondAmountsBySponsor
	};
} ();


