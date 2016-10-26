(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssSecureHeader', {
            //template:'htmlTemplate',
            templateUrl: '/app/header/secure.header.component.html',
            controller: HeaderController,
            controllerAs: 'vm',
            bindings: {
                Binding: '=',
            },
        });

    HeaderController.$inject = ['authorizationService', '$state'];
    function HeaderController(authorizationService, $state) {
        var vm = this;
        
        vm.logout = logout;

        ////////////////

        vm.$onInit = function() { };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };

        function logout(){
            authorizationService.setToken(null);
            $state.go("login");
        }
    }
})();