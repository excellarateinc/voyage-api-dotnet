describe('user-list.component', function(){
    var controller;
    var scope;
    var $q;
    var $rootScope;
    var mockUserService;
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
    
        var userService = $injector.get('userService');
        mockUserService = sandbox.mock(userService);


        controller = $componentController('lssUserList', 
            {
                $scope: scope, 
                userService: userService,
               
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

   

     describe('refresh', function(){
        it('should call userService and initialize user array', function(){
           //Arrange
            var deferred = $q.defer();

            var users = [{id: 1}];

            mockUserService
                .expects('getUsersWithRoles')
                .once()
                .returns(deferred.promise);

            //Act
            controller.refresh();
            deferred.resolve(users);
            $rootScope.$digest();

            //Assert
            mockUserService.verify();
            expect(controller.users.length).toBe(1);
            
        });
    });
});