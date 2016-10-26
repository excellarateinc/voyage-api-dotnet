(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssRegister', {
            //template:'htmlTemplate',
            templateUrl: '/app/account/register.component.html',
            controller: RegisterController,
            controllerAs: 'vm',
            bindings: {
                Binding: '=',
            },
        });

    RegisterController.$inject = ['accountService', '$state', 'errorService'];
    function RegisterController(accountService, $state, errorService) {
        var vm = this;
        vm.username = '';
        vm.password = '';
        vm.confirmPassword = '';
        vm.register = register;
        vm.registrationErrors = [];

        ////////////////

        vm.$onInit = function() { };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };

        function register(){
            if(vm.password == vm.confirmPassword){
                accountService.register(vm.username, vm.password)
                .then(
                    function(result){
                        vm.registrationErrors = [];
                        $state.go("login");
                    },
                    function(failure){
                        var errors = errorService.getModelStateErrors(failure);
                        vm.registrationErrors = errors;
                    }
                );
            }
            else{
                vm.registrationErrors = ["Passwords must match"];
            }
        }
    }
})();