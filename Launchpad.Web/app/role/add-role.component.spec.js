describe('add-role.component', function(){
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

        //var errorService = $injector.get('errorService');
        //mockErrorService = sandbox.mock(errorService);

        controller = $componentController('lssAddRole', 
            {
                $scope: scope, 
                roleService: service,
                //errorService: errorService, 
                //$state: stubState
            });
    }));

    it('should be defined', function(){
        expect(controller).toBeDefined();
    });

    describe('addRole', function(){
        it('should call the roleService', function(){
            var deferred = $q.defer();

            controller.roleName = 'Super Role';

            mockRoleService.expects('addRole')
                .once()
                .withArgs(controller.roleName)
                .returns(deferred.promise);

            controller.save();
            deferred.resolve(true);
            $rootScope.$digest();

            mockRoleService.verify();

            expect(controller.roleName).toBe("");
        });
    });



});