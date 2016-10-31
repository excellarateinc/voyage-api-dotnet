describe('user.service', function(){
    var $q;
    var $httpBackend;
    var $rootScope;
    var service;
    var sandbox;
  

    afterEach(function(){
        sandbox.restore();
    });

    beforeEach(function(){
        sandbox = sinon.sandbox.create();
       
    });

    beforeEach(module('lss-launchpad'));

    

     beforeEach(inject(function($injector, _$q_, _$rootScope_, _$httpBackend_){
        $q = _$q_;
        $httpBackend = _$httpBackend_;
        $rootScope = _$rootScope_;
        service = $injector.get('userService');
    
    }));

    describe('getClaims', function(){
        it('should call user endpoint and return the active user\'s claims', function(){

            var claims = [{claimValue: "v1", claimType: "t1"}];

            $httpBackend.when('GET', 'api/user/claims')
                .respond(claims);


            var promise = service.getClaims();

            promise.then(function(data){
                expect(data.length).toBe(claims.length);
            });

            $httpBackend.flush();
        });
    });

    describe('getUsers', function(){

        it('should call user endpoint and return users', function(){
            var users = [{name: 'user1'},{name: 'user2'}];

            $httpBackend.when('GET', '/api/user')
                .respond(users);

            var promise = service.getUsers();

            promise.then(function(data){
                expect(data.length).toBe(users.length);
            });

            $httpBackend.flush();
        });

    });

    describe('assign', function(){
        it('should call user endpoint', function(){
            var model = {
                role: {
                    name: 'role1',
                    id: '123'
                },
                user: {
                    name: 'user1',
                    id: '456'
                }
            };

            $httpBackend.when('POST', '/api/user/assign', model)
                .respond(true);
            
            var promise = service.assign(model.role, model.user);

            promise.then(function(data){
                expect(data).toBe(true);
            });

            $httpBackend.flush();
        });
    });

});