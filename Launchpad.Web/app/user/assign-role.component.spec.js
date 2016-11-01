describe('userService', function(){
     var controller;
    var scope;
    var $q;
    var $rootScope;
    var mockRoleService;
    var mockUserService;

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

        var userService = $injector.get('userService');
        mockUserService = sandbox.mock(userService);

        controller = $componentController('lssAssignRole', 
            {
                $scope: scope, 
                roleService: roleService,
                userService: userService
            });
    }));

    it('should be defined', function(){
        expect(controller).toBeDefined();
    });

    describe('$onInit', function(){

        it('should call the roleService and userService and initialize arrays', function(){
            var deferred = $q.defer();
            var deferred2 = $q.defer();
            var roles = [{name: 'Admin', id: '123'}];
            var users = [{name: 'Fred', id: '234'}];

            mockRoleService.expects('getRoles')
                .once()
                .returns(deferred.promise);
            
            mockUserService.expects('getUsers')
                .once()
                .returns(deferred2.promise);

            controller.$onInit();
            deferred.resolve(roles);
            deferred2.resolve(users);
            $rootScope.$digest();

            mockRoleService.verify();

            expect(controller.users).toBe(users);
            expect(controller.selectedUser).toBe(users[0]);
            expect(controller.roles).toBe(roles);
            expect(controller.selectedRole).toBe(roles[0]);
        });
    });

});