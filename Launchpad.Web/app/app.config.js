(function(){
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
                    component: 'lssEmptyNav'
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
                    component: 'lssEmptyNav'
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
                    component: 'lssSecureNav'
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
                    component: 'lssSecureNav'
                }
            }
        }
        ];

        // Loop over the state definitions and register them
        states.forEach(function(state) {
            $stateProvider.state(state);
        });
    }
})();