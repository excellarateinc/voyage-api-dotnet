describe('authorizationInterceptor', function(){
    var $q;
    var $rootScope;
    var service;
    var mockAuthorizationService;
    var sandbox;
    var stubAuthorizationService;
    var stubState;
    afterEach(function(){
        sandbox.restore();
    });

    beforeEach(function(){
        sandbox = sinon.sandbox.create();
        stubAuthorizationService = sandbox.stub({getToken: function(){}});
        stubState = sandbox.stub({go: function(){}});
    });


    beforeEach(module('lss-launchpad'));

    beforeEach( module(function($provide) {
        $provide.value('authorizationService', stubAuthorizationService);
        $provide.value('$state', stubState);
    }));

    beforeEach(inject(function($injector, _$q_, _$rootScope_){
        $q = _$q_;
        $rootScope = _$rootScope_;
        service = $injector.get('authorizationInterceptor');
    
    }));

    it('should exist', function(){
        expect(service).toBeDefined();
    });

    describe('request', function(){
        it('should call authorization service and add authorization header when it exists', function(){
            var config = {headers : {}};
            stubAuthorizationService.getToken.returns("Super Token!");

            service.request(config);

            expect(config.headers.Authorization).toBeDefined();
            expect(config.headers.Authorization).toBe('bearer Super Token!');
            expect(stubAuthorizationService.getToken.calledOnce).toBe(true);
        });

        it('should call authorization service and not add the header when the token does not exist', function(){
       var config = {headers : {}};
            stubAuthorizationService.getToken.returns(null);

            service.request(config);

            expect(config.headers.Authorization).not.toBeDefined();
            expect(stubAuthorizationService.getToken.calledOnce).toBe(true);
            
        });
    });  

    describe('responseError', function(){
        it('should redirect to login on 401', function(){
            var response = {
                status: 401
            };

            var result = service.responseError(response);

            expect(stubState.go.calledOnce).toBe(true);
            expect(stubState.go.calledWith('login')).toBe(true);
            expect(result).toBeDefined();

            result.then(function(){

            }, function(rejectionValue){
                expect(rejectionValue).toBe(response);
            });

            $rootScope.$digest();
        });

        it('should not redirect when the eror is not 401', function(){
            var response = {
                status: 402
            };

            var result = service.responseError(response);

            expect(stubState.go.calledOnce).toBe(false);
            expect(result).toBeDefined();

            result.then(function(){

            }, function(rejectionValue){
                expect(rejectionValue).toBe(response);
            });

            $rootScope.$digest();
        });
    });

});