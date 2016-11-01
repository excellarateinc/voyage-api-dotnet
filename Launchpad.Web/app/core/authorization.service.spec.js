describe('authorization.service', function(){
    var service;

    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function($injector){
        service = $injector.get('authorizationService');
    }));

    describe('getToken', function(){
        it('should return the token value', function(){
            var token = "my super secure token";

            service.setToken(token);

            var result = service.getToken();

            expect(result).toBe(token);
        });
    });

    describe('setToken', function(){
        it('should set the token value', function(){

            var token = "my super secure token";

            service.setToken(token);

            var result = service.getToken();

            expect(result).toBe(token);
        });

        it('should clear claims map when token is null', function(){

            var claimsMap = {
                "lss.permission->login": true
            };
             var token = "my super secure token";

            service.setToken(token);
            service.setClaims(claimsMap);

            expect(service.hasClaim("lss.permission", "login")).toBe(true);

            //Now clear token and claims should be false

            service.setToken(null);

            expect(service.hasClaim("lss.permission", "login")).toBe(false);

        });
    });

    describe('setClaims', function(){
        it('should set the claims map that is used for hasClaim calls', function(){
            var claimsMap = {
                "lss.permission->login": true
            };

            service.setClaims(claimsMap);

            expect(service.hasClaim("lss.permission", "login")).toBe(true);
        });
    });

    describe('hasClaim', function(){
        it('should return true when claim is found', function(){
            var claimsMap = {
                "lss.permission->login": true
            };

            service.setClaims(claimsMap);

            expect(service.hasClaim("lss.permission", "login")).toBe(true);
       
        });

        it('should return false when claim is not found', function(){
            var claimsMap = {
                "lss.permission->login": true
            };

            service.setClaims(claimsMap);

            expect(service.hasClaim("lss.permission", "assign.role")).toBe(false);
       
        });
    });
});