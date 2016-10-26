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
    });
});