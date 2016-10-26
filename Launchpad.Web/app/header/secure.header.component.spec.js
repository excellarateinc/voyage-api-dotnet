describe('secure.header.component', function(){
    var controller;
    var scope;
    var stubState;
    var stubAuthorizationService;


    afterEach(function(){
        sandbox.restore();
    });

    beforeEach(function(){
        sandbox = sinon.sandbox.create();
        stubAuthorizationService = sandbox.stub({setToken: function(){}});
        stubState = sinon.stub({go: function(){}});
    });

    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function($rootScope, $componentController){
        scope = $rootScope.$new();
    

        controller = $componentController('lssSecureHeader', {$scope: scope, authorizationService: stubAuthorizationService, $state: stubState});
    }));

     it('should exist', function(){
        expect(controller).toBeDefined();
    });

    describe('logout', function(){
        it('should call authorization service and then go to login state', function(){


            controller.logout();

            expect(stubAuthorizationService.setToken.calledOnce).toBe(true);
            expect(stubAuthorizationService.setToken.calledWith(null)).toBe(true);
            expect(stubState.go.calledOnce).toBe(true);
            expect(stubState.go.calledWith("login")).toBe(true);
        });
    });

});