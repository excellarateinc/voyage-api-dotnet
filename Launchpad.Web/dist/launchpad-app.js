(function () {
    'use strict';

    angular
        .module("lss-launchpad", ['ui.router']);
        
})();;(function(){
    angular
        .module("lss-launchpad")
        .config(['$stateProvider', '$urlRouterProvider', '$httpProvider', configure]);

    function configure($stateProvider, $urlRouterProvider, $httpProvider){

        //Configure http interceptors
        $httpProvider.interceptors.push('authorizationInterceptor');

        //Configure default route
        $urlRouterProvider.otherwise('/login');

        //Configure ui states
        var states = [
        { 
            name: 'login', 
            url: '/login', 
            // Using component: instead of template:
            component: 'lssLogin'  
        },
        {
            name: 'dashboard',
            url: '/dashboard',
            component: 'lssDashboard'
        }];

        // Loop over the state definitions and register them
        states.forEach(function(state) {
            $stateProvider.state(state);
        });
    }
})();;(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('accountService', AccountService);

    AccountService.$inject = ['$q', '$http', 'authorizationService'];
    function AccountService($q, $http, authorizationService) {
        var service = {
            login:login
        };
        
        return service;

        ////////////////
        function login(username, password) { 
            var deferred = $q.defer();
 
            var content = "grant_type=password&username=" + username + "&password=" + password;  
         
            $http.post("/Token", content, {
                headers: { 'Content-Type' :  'application/x-www-form-urlencoded'  }
            })
            .success(function(response){
                authorizationService.setToken(response.access_token);
                deferred.resolve(true);
            });
            return deferred.promise;
        }
    }
})();;(function() {
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
                Binding: '=',
            },
        });

    LoginController.$inject = ['accountService', '$state'];
    function LoginController(accountService, $state) {
        var vm = this;
        vm.login = login;
        vm.username = 'fred@fred.com';
        vm.password = 'Hello123!';
        
        ////////////////

        vm.$onInit = function() { };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };

        function login(){
            
            accountService.login(vm.username, vm.password)
            .then(function(result){
                $state.go('dashboard');
            });
        }
    }
})();;(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('authorizationInterceptor', AuthorizationInterceptor);


    AuthorizationInterceptor.$inject = ['$q', 'authorizationService', '$state'];
    function AuthorizationInterceptor($q, authorizationService, $state) {
    
        var service = {
            request: request,
            responseError: responseError
        };
        return service;

        ////////////////
        function request(config) { 
            var token = authorizationService.getToken();
            if(token){
                config.headers.Authorization = 'bearer ' + token;
            }
            return config;
        }

        function responseError(response){
            var deferred = $q.defer();
            if(response.status == 401){
                $state.go('login');
            }
            deferred.reject(response);
            return deferred.promise;
        }

    }
})();;(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('authorizationService', AuthorizationService);

    function AuthorizationService() {
        var accessToken = null;

        var service = {
           setToken : setToken,
           getToken : getToken
        };
        
        return service;

        ////////////////
        function setToken(token) { 
            accessToken = token;
        }
        function getToken() {
            return accessToken;
        }
    }
})();;(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssDashboard', {
            //template:'htmlTemplate',
            templateUrl: '/app/dashboard/dashboard.component.html',
            controller: DashboardController,
            controllerAs: 'vm',
            bindings: {
                Binding: '=',
            },
        });

    DashboardController.$inject = ['widgetService'];
    function DashboardController(widgetService) {
        var vm = this;
     

        ////////////////

        vm.$onInit = function() { 
            widgetService.getWidgets()
                .then(function(response){
                    vm.widgets = response;
                });
        };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };
    }
})();;(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('widget', {
            //template:'htmlTemplate',
            templateUrl: '/app/widget/widget.component.html',
            controller: WidgetController,
            controllerAs: 'vm',
            bindings: {
                widget: '=',
            },
        });

    //WidgetController.$inject = ['dependency1'];
    function WidgetController() {
        var $ctrl = this;
        

        ////////////////

        $ctrl.$onInit = function() { };
        $ctrl.$onChanges = function(changesObj) { };
        $ctrl.$onDestory = function() { };
    }
})();;(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('widgetService', WidgetService);

    WidgetService.$inject = ['$http', '$q'];
    function WidgetService($http, $q) {
        var service = {
            getWidgets: getWidgets
        };
        
        return service;

        ////////////////
        function getWidgets() { 
            var deferred = $q.defer();
            $http.get("/api/v2/widget")
                .then(function(response){
                    deferred.resolve(response.data);
                },
                function(response){
                    deferred.reject(response.status);
                }
                );
            return deferred.promise;
        }
    }
})();