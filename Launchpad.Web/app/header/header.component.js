(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssHeader', {
            //template:'htmlTemplate',
            templateUrl: '/app/header/header.component.html',
            controller: HeaderController,
            controllerAs: 'vm',
            bindings: {
                Binding: '=',
            },
        });

    function HeaderController() {
        var vm = this;
        

        ////////////////

        vm.$onInit = function() { };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };
    }
})();