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
            views: {
                content: {
                    // Using component: instead of template:
                    component: 'lssLogin'
                },
                header: {
                    component: 'lssHeader'
                },
                nav: {
                    templateUrl: '/app/nav/empty-nav.component.html'
                }
            }
             
        },
        { 
            name: 'register', 
            url: '/register',
            views: {
                content: {
                    // Using component: instead of template:
                    component: 'lssRegister'
                },
                header: {
                    component: 'lssHeader'
                },
                nav: {
                    templateUrl: '/app/nav/empty-nav.component.html'
                }
            }
             
        },
        {
            name: 'dashboard',
            url: '/dashboard',
            views: {
                content: {
                    // Using component: instead of template:
                    component: 'lssDashboard'
                },
                header: {
                    component: 'lssSecureHeader'
                },
                nav: {
                    templateUrl: 'app/nav/secure-nav.component.html'
                }
            }
        },
        {
            name: 'addRole',
            url: '/addRole',
            views: {
                content: {
                    // Using component: instead of template:
                    component: 'lssAddRole'
                },
                header: {
                    component: 'lssSecureHeader'
                },
                nav: {
                    templateUrl: 'app/nav/secure-nav.component.html'
                }
            }
        }
        ];

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
            login:login,
            register: register
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

        function register(username, password){
            var deferred = $q.defer();

            var user = {
                email: username,
                password: password,
                confirmPassword: password
            };

            $http.post("/api/account/register", user)
                .then(function(response){
                    deferred.resolve(response.data);
                }, 
                function(response){
                    deferred.reject(response.data);
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
                        $state.go('login');
                    },
                    function(failure){
                        var errors = errorService.getModelStateErrors(failure);
                        vm.registrationErrors = errors;
                    }
                );
            }
            else{
                vm.registrationErrors = ['Passwords must match'];
            }
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

    angular
        .module('lss-launchpad')
        .factory('errorService', ErrorService);
    
    //This could be an interceptor
    function ErrorService() {
        var service = {
            getModelStateErrors:getModelStateErrors
        };
        
        return service;

        ////////////////
        function getModelStateErrors(failure) { 
            var errors = [];
            if(failure.modelState){
                for(var key in failure.modelState){
                    var states = failure.modelState[key];
                    for(var i = 0; i < states.length; ++i){
                        errors.push(states[i]);
                    }
                }
            }
            return errors;
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
})();;(function() {
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
})();;(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('lssAddRole', {
            templateUrl:'/app/role/add-role.component.html',
            controller: AddRoleController,
            controllerAs: 'vm',
            bindings: {
                //Binding: '=',
            },
        });

    AddRoleController.$inject = ['roleService'];
    function AddRoleController(roleService) {
        var vm = this;
        
        vm.roleName = "";
        vm.save = save;

        ////////////////

        vm.$onInit = function() { };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };

        function save(){
            roleService.addRole(vm.roleName)
                .then(
                  function(data){
                      vm.roleName = "";
                  },
                  function(err){

                  }
                );
        }
    }
})();;(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('roleService', RoleService);

    RoleService.$inject = ['$http', '$q'];
    function RoleService($http, $q) {
        var service = {
            addRole:addRole
        };
        
        return service;

        ////////////////
        function addRole(roleName) { 
            var deferred = $q.defer();

            var role = {
                name: roleName
            };

            $http.post("/api/role", role)
                .then(function(response){
                    deferred.resolve(true);
                }, 
                function(response){
                    deferred.reject(response.data);
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
        .component('lssStatusList', {
            //template:'htmlTemplate',
            templateUrl: '/app/status/status-list.component.html',
            controller: StatusListController,
            controllerAs: 'vm',
            bindings: {
                Binding: '=',
            },
        });

    StatusListController.$inject = ['statusService'];
    function StatusListController(statusService) {
        var vm = this;
        vm.statuses = [];

        ////////////////

        vm.$onInit = function() { 
            statusService.getStatus()
                .then(
                    function(statuses){
                        vm.statuses = statuses;
                    },
                    function(error){

                    }
                );
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
        .component('lssStatus', {
            //template:'htmlTemplate',
            templateUrl: '/app/status/status.component.html',
            controller: StatusController,
            controllerAs: 'vm',
            bindings: {
                status: '=',
            },
        });

    function StatusController() {
        var vm = this;
        

        ////////////////

        vm.$onInit = function() { };
        vm.$onChanges = function(changesObj) { };
        vm.$onDestory = function() { };
    }
})();;(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .factory('statusService', StatusService);

    StatusService.$inject = ['$http', '$q'];
    function StatusService($http, $q) {
        var service = {
            getStatus: getStatus
        };
        
        return service;

        ////////////////
        function getStatus() { 
             var deferred = $q.defer();
            $http.get("/api/v2/status")
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