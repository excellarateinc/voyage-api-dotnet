describe('secure-nav.component', function(){

    var controller;
    var scope;
    var mockAuthorizationService;
    
    
    afterEach(function(){
        sandbox.restore();
    });

    beforeEach(function(){
        sandbox = sinon.sandbox.create();
    });


    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function($rootScope, $componentController, $injector){
        scope = $rootScope.$new();
        var authorizationService = $injector.get('authorizationService');
        mockAuthorizationService = sandbox.mock(authorizationService);
        controller = $componentController('lssSecureNav', {
            $scope: scope,
            authorizationService: authorizationService
        });
    }));

    it('should exist', function(){
        expect(controller).toBeDefined();
    });

    describe('$onInit', function(){
        it('should initialize the view model states array for only states that have corresponding claims', function(){

            mockAuthorizationService.expects('hasClaim')
                .once()
                .withArgs('lss.permission', 'create.claim')
                .returns(true);

            mockAuthorizationService.expects('hasClaim')
                .once()
                .withArgs('lss.permission', 'assign.role')
                .returns(false);

            mockAuthorizationService.expects('hasClaim')
                .once()
                .withArgs('lss.permission', 'create.role')
                .returns(false);

            controller.$onInit();

            expect(controller.states.length).toBe(1);
            expect(controller.states[0].state).toBe('claimDashboard');
                
        });
    });
});