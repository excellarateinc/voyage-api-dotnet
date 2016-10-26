describe('login.component', function(){
    var scope;
    var $componentController;
    var mockAccountService;
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
        mockAccountService = sandbox.mock(service);
        stubState = sinon.stub({go: function(){}});

        
        controller = $componentController('lssLogin', {$scope: scope, accountService: service, $state: stubState});
        
    }));

    it('should exist', function(){
        expect(controller).toBeDefined();
    });

    describe('login', function(){
        it('should call accountService', function(){
            var deferred = $q.defer();

            controller.username = 'testuser1';
            controller.password = 'testpassword';
            
            mockAccountService.expects('login')
                .once()
                .withArgs(controller.username, controller.password)
                .returns(deferred.promise);

            //act
            controller.login();
            deferred.resolve(true);
            $rootScope.$digest();

            //assert
            expect(stubState.go.calledOnce).toBe(true);
            expect(stubState.go.calledWith('dashboard')).toBe(true);
            mockAccountService.verify();
        });
    });

});