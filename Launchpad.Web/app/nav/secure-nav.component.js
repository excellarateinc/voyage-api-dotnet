(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssSecureNav', {
            templateUrl: '/app/nav/secure-nav.component.html',
            controller: SecureNavController,
            bindings: {

            },
        });

    function SecureNavController() {
        var $ctrl = this;
        

        ////////////////

        $ctrl.$onInit = function() { };
        $ctrl.$onChanges = function(changesObj) { };
        $ctrl.$onDestory = function() { };
    }
})();