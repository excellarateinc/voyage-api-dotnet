(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssEmptyNav', {
            templateUrl: '/app/nav/empty-nav.component.html',
            controller: EmptyNavController,
            bindings: {
            },
        });

    function EmptyNavController(dependency1) {
        var $ctrl = this;
        

        ////////////////

        $ctrl.$onInit = function() { };
        $ctrl.$onChanges = function(changesObj) { };
        $ctrl.$onDestory = function() { };
    }
})();