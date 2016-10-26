describe('register.component', function(){
    var controller;
    var scope;
    var stubState;
    var mockErrorService;
    var mockAccountService;
    var $q;
    var $rootScope;

    afterEach(function(){
        sandbox.restore();
    });

    beforeEach(function(){
        sandbox = sinon.sandbox.create();
        stubState = sinon.stub({go: function(){}});
    });

    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function(_$rootScope_, _$q_, $componentController, $injector){
        $rootScope = _$rootScope_;
        $q = _$q_;
        scope = $rootScope.$new();
    
        var service = $injector.get('accountService');
        mockAccountService = sandbox.mock(service);

        var errorService = $injector.get('errorService');
        mockErrorService = sandbox.mock(errorService);

        controller = $componentController('lssRegister', 
            {
                $scope: scope, 
                accountService: service,
                errorService: errorService, 
                $state: stubState
            });
    }));

    it('should be defined', function(){
        expect(controller).toBeDefined();
    });

    describe('register', function(){
        it('should call account service and go to state login when sucessful', function(){
            var deferred = $q.defer();

            controller.username = 'myname';
            controller.password = 'p1';
            controller.confirmPassword = 'p1';

           mockAccountService.expects('register')
                .once()
                .withArgs(controller.username, controller.password)
                .returns(deferred.promise);

            controller.register();
            deferred.resolve(true);
            $rootScope.$digest();


            mockAccountService.verify();
            expect(stubState.go.calledOnce).toBe(true);
            expect(stubState.go.calledWith('login')).toBe(true);
        });

          it('should call account service and display errors on failure', function(){
            var deferred = $q.defer();
            var failure = {};
            var errors = ["test"];
            controller.username = 'myname';
            controller.password = 'p1';
            controller.confirmPassword = 'p1';

           mockAccountService.expects('register')
                .once()
                .withArgs(controller.username, controller.password)
                .returns(deferred.promise);

            mockErrorService.expects('getModelStateErrors')
                .once()
                .withArgs(failure)
                .returns(errors);

            controller.register();
            deferred.reject(failure);
            $rootScope.$digest();

            mockAccountService.verify();
            expect(stubState.go.callCount).toBe(0);
            expect(controller.registrationErrors).toBe(errors);
        });

        it('should set error when password and confirm password does not match', function(){
            controller.username = 'myname';
            controller.password = 'p1';
            controller.confirmPassword = 'p2';

            controller.register();

            expect(controller.registrationErrors.length).toBe(1);
        });
    });

});