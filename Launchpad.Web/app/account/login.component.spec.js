describe('login.component', function(){
    var scope;
    var $componentController;
    var mockAccountService;
    var mockAuthorizationService;
    var mockUserService;
    var sandbox;
    var controller;
    var $q;
    var $rootScope;
    var stubState;

    afterEach(function(){
        sandbox.restore();
    });

    beforeEach(function(){
        sandbox = sinon.sandbox.create();
    });


    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function(_$rootScope_, $componentController, $injector, _$q_){
       
        $q = _$q_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        var service = $injector.get('accountService');
        var authorizationService = $injector.get('authorizationService');
        var userService = $injector.get('userService');
        mockUserService = sandbox.mock(userService);
        mockAuthorizationService = sandbox.mock(authorizationService);
        mockAccountService = sandbox.mock(service);
        stubState = sinon.stub({go: function(){}});

        
        controller = $componentController('lssLogin', 
            {   
                $scope: scope, 
                accountService: service, 
                $state: stubState,
                authorizationService: authorizationService,
                userService: userService
            });
        
    }));

    it('should exist', function(){
        expect(controller).toBeDefined();
    });

    describe('login', function(){

        it('should not transition when login claim is missing', function(){
            var deferred = $q.defer();
            var claimsDeferred = $q.defer();

            var claims =   
                {
                    'lss.permission->login': true
                };

            controller.username = 'testuser1';
            controller.password = 'testpassword';
            
            mockAccountService.expects('login')
                .once()
                .withArgs(controller.username, controller.password)
                .returns(deferred.promise);

            mockUserService.expects('getClaimsMap')
                .once()
                .returns(claimsDeferred.promise);
              
            mockAuthorizationService.expects('setClaims')
                .once()
                .withArgs(claims);

            mockAuthorizationService.expects('hasClaim')
                .once()
                .withArgs('lss.permission', 'login')
                .returns(false);
              

            //act
            controller.login();
            deferred.resolve(true);
            claimsDeferred.resolve(claims);
            $rootScope.$digest();

            //assert
            expect(stubState.go.calledOnce).toBe(false, "state transition called");
            mockAccountService.verify();
        });

        it('should call accountService and user service and transition to dashboard on success', function(){
            var deferred = $q.defer();
            var claimsDeferred = $q.defer();

            var claims =   
                {
                    'lss.permission->login': true
                };

            controller.username = 'testuser1';
            controller.password = 'testpassword';
            
            mockAccountService.expects('login')
                .once()
                .withArgs(controller.username, controller.password)
                .returns(deferred.promise);

            mockUserService.expects('getClaimsMap')
                .once()
                .returns(claimsDeferred.promise);
              
             mockAuthorizationService.expects('setClaims')
                .once()
                .withArgs(claims);
                  
            mockAuthorizationService.expects('hasClaim')
                .once()
                .withArgs('lss.permission', 'login')
                .returns(true);
              

            //act
            controller.login();
            deferred.resolve(true);
            claimsDeferred.resolve(claims);
            $rootScope.$digest();

            //assert
            expect(stubState.go.calledOnce).toBe(true, "state transition called");
            expect(stubState.go.calledWith('dashboard')).toBe(true);
            mockAccountService.verify();
        });
    });

});