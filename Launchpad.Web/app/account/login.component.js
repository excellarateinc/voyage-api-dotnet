(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssLogin', {
            //template:'login.component.html',
            templateUrl: '/app/account/login.component.html',
            controller: LoginController,
            controllerAs: 'vm',
            bindings: {
                Binding: '='
            }
        });

    LoginController.$inject = ['accountService', '$state', 'authorizationService', 'lssConstants', '$log', 'userService'];
    function LoginController(accountService, $state, authorizationService, constants, $log, userService) {
        var vm = this;
        vm.login = login;
        vm.username = 'admin@admin.com';
        vm.password = 'Hello123!';
        
        ////////////////

        vm.$onInit = function() { };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };

        function login(){
            
            accountService.login(vm.username, vm.password)
                .then(_loginCallback);
        }

        function _loginCallback(result){
            userService.getClaimsMap()
                .then(function(claimsMap){
                    authorizationService.setClaims(claimsMap);
                    if(authorizationService.hasClaim(constants.lssClaimType, constants.claims.login)){
                        $state.go('dashboard');
                    }else{
                        $log.info('User does not have required claim: ' + constants.claims.login);
                    }
                });
        }
    }
})();