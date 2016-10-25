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
})();