describe('add-claim.component', function(){
    var controller;
    var scope;
    var $q;
    var $rootScope;
    var mockRoleService;

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
    
        var service = $injector.get('roleService');
        mockRoleService = sandbox.mock(service);

        controller = $componentController('lssAddClaim', 
            {
                $scope: scope, 
                roleService: service,
            });
    }));

    it('should be defined', function(){
        expect(controller).toBeDefined();
    });

    
    describe('save', function(){
        it('should call the roleService', function(){
            var deferred = $q.defer();

            controller.roles = [{name: 'Admin', id: '123'}];
            controller.selectedRole = controller.roles[0];
            controller.claimType = 'permission';
            controller.claimValue = 'view.claim';

            mockRoleService.expects('addClaim')
                .once()
                .withArgs(controller.selectedRole, controller.claimType, controller.claimValue)
                .returns(deferred.promise);

            controller.save();
            deferred.resolve(true);
            $rootScope.$digest();

            mockRoleService.verify();

            expect(controller.claimValue).toBe("");
            expect(controller.claimType).toBe("");
        });
    });

    describe('$onInit', function(){
        it('should call role service', function(){
            var deferred = $q.defer();

            var roles = [{name: 'Admin', id: '123'}];

            mockRoleService.expects('getRoles')
                .once()
                .returns(deferred.promise);
            
            controller.$onInit();
            deferred.resolve(roles);
            $rootScope.$digest();

            mockRoleService.verify();

            expect(controller.roles).toBe(roles);
            expect(controller.selectedRole).toBe(roles[0]);

        });
    });
});