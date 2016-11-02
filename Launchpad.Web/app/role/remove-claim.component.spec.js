describe('remove-claim.component', function(){
    var controller;
    var scope;
    var $q;
    var $rootScope;
    var mockRoleService;
    var sandbox;

    afterEach(function(){
        sandbox.restore();
    });

    beforeEach(function(){
        sandbox = sinon.sandbox.create();
    });

    beforeEach(module('lss-launchpad'));

     beforeEach(inject(function(_$rootScope_, _$q_, $componentController, $injector){
        $rootScope = _$rootScope_;
        $q = _$q_;
        scope = $rootScope.$new();
    
        var roleService = $injector.get('roleService');
        mockRoleService = sandbox.mock(roleService);


        controller = $componentController('lssRemoveClaim', 
            {
                $scope: scope, 
                roleService: roleService,
               
            });
    }));

     it('should be defined', function(){
        expect(controller).toBeDefined();
    });

    describe('$onInit', function(){
        it('should call refresh', function(){
            var stub = sandbox.stub(controller, 'refresh');

            
            controller.$onInit();

            expect(stub.calledOnce).toBe(true);

        });
    });

    describe('save', function(){
        it('should call role service and refresh', function(){
           //Arrange
           var stub = sandbox.stub(controller, 'refresh');
           var deferred = $q.defer();
           var role = {};
           var claim = {};
           controller.selectedRole = role;
           controller.selectedClaim = claim;
           $rootScope.$digest();

           mockRoleService.expects('removeClaim')
                .once()
                .withArgs(role, claim)
                .returns(deferred.promise);
            
            //Act
            controller.save();
            deferred.resolve();
            $rootScope.$digest();

            //Assert
            mockRoleService.verify();
            expect(stub.calledOnce).toBe(true);
        });
    });

     describe('refresh', function(){
        it('should call roleService and initialize role array', function(){
           //Arrange
            var deferred = $q.defer();

            var roles = [{id: 1}];

            mockRoleService
                .expects('getRoles')
                .once()
                .returns(deferred.promise);

            //Act
            controller.refresh();
            deferred.resolve(roles);
            $rootScope.$digest();

            //Assert
            mockRoleService.verify();
            expect(controller.roles.length).toBe(1);
            expect(controller.selectedRole).toBe(roles[0]);
        });
    });
});