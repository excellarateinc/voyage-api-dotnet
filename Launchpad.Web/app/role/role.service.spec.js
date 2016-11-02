describe('roleService', function(){
    var $q;
    var $httpBackend;
    var $rootScope;

    var service;

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
        service = $injector.get('roleService');
    
    }));

    describe('getRoles', function(){

        it('should call roles endpoint and return roles', function(){
            var roles = [{name: 'role1'},{name: 'role2'}];

            $httpBackend.when('GET', '/api/role')
                .respond(roles);

            var promise = service.getRoles();

            promise.then(function(data){
                expect(data.length).toBe(roles.length);
            });

            $httpBackend.flush();
        });

    });

    it('should exist', function(){
        expect(service).toBeDefined();
    });

    describe('addClaim', function(){
        it('should call role endpoint and call resolve on success', function(){
            var roleClaim = {
                    claim: {
                        claimType: 'claimType1',
                        claimValue: 'claimValue1'
                    },
                    role: {
                        name: 'role1',
                        id: 'id1'
                    }
                };

            $httpBackend.when('POST', '/api/role/claim', roleClaim)
                .respond(true);
            
            var promise = service.addClaim(roleClaim.role, roleClaim.claim.claimType, roleClaim.claim.claimValue);

            promise.then(function(value){
                expect(value).toBe(true);
            }, function(err){
                expect(false).toBe(true);
            });

            $httpBackend.flush();
        });

        it('should call role endpoint and call reject on failure', function(){
             var roleClaim = {
                    claim: {
                        claimType: 'claimType1',
                        claimValue: 'claimValue1'
                    },
                    role: {
                        name: 'role1',
                        id: 'id1'
                    }
                };

             $httpBackend.when('POST', '/api/role/claim', roleClaim)
                .respond(400, {data: "error"});

            var promise = service.addClaim(roleClaim.role, roleClaim.claim.claimType, roleClaim.claim.claimValue);

               promise.then(function(){
                expect(false).toBe(true);
            }, function(value){
                expect(value.data).toBe("error");
            });

            $httpBackend.flush();
        });
    });

    describe('addRole', function(){
        it('should call role endpoint and call resolve on success', function(){
            var role = {
                name: 'Terrific Role'
            };

            $httpBackend.when('POST', '/api/role', role)
                .respond(true);
            
            var promise = service.addRole(role.name);

            promise.then(function(value){
                expect(value).toBe(true);
            }, function(err){
                expect(false).toBe(true);
            });

            $httpBackend.flush();
        });

        it('should call role endpoint and call reject on failure', function(){
            var role = {
                name: "Terrific Role"
            };

             $httpBackend.when('POST', '/api/role', role)
                .respond(400, {data: "error"});

            var promise = service.addRole(role.name);

               promise.then(function(){
                expect(false).toBe(true);
            }, function(value){
                expect(value.data).toBe("error");
            });

            $httpBackend.flush();
        });
    });

    describe('removeClaim', function(){
        it('should call role endpoint and resolve on success', function(){
            var role = { 
                name: 'role1',
                id: 'id'
            };

            var claim = {
                claimType: 't1',
                claimValue: 'v1'
            };

            $httpBackend.when('DELETE', '/api/role/claim?claimType=t1&claimValue=v1&roleName=role1')
                .respond(200, true);
            
            var promise = service.removeClaim(role, claim);

            promise.then(function(data){
                expect(data).toBe(true);
            });

            $httpBackend.flush();
        });

        it('should call role endpoint and resolve on success', function(){
            var role = { 
                name: 'role1',
                id: 'id'
            };

            var claim = {
                claimType: 't1',
                claimValue: 'v1'
            };

            $httpBackend.when('DELETE', '/api/role/claim?claimType=t1&claimValue=v1&roleName=role1')
                .respond(400, 'Failure');
            
            var promise = service.removeClaim(role, claim);

            promise.then(function(data){
                expect(data).toBe(false);
            }, function(data){
                expect(data).toBe('Failure');
            });

            $httpBackend.flush();
        });
    });

    describe('removeRole', function(){
        it('should call role endpoint and call resolve on success', function(){
            var role = { 
                name: 'role1',
                id: 'id'
            };

            $httpBackend.when('DELETE', 'api/role?id=id&name=role1')
                .respond(200);
            
            var promise = service.removeRole(role);

            promise.then(function(data){
                expect(data).toBe(true);
            });

            $httpBackend.flush();
        });

        it('should call role endpoint and call reject on failure', function(){
            var role = { 
                name: 'role1',
                id: 'id'
            };

            $httpBackend.when('DELETE', 'api/role?id=id&name=role1')
                .respond(400, 'Failure');
            
            var promise = service.removeRole(role);

            promise.then(function(data){
                expect(data).toBe(false);
            }, function(data){
                expect(data).toBe('Failure');
            });

            $httpBackend.flush();
        });
    });

});