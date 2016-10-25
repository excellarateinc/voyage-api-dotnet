describe('account.service', function(){
    var $q;
    var $httpBackend;
    var $rootScope;
    var service;
    var sandbox;
    var stubAuthorizationService;

    afterEach(function(){
        sandbox.restore();
    });

    beforeEach(function(){
        sandbox = sinon.sandbox.create();
        stubAuthorizationService = sandbox.stub({setToken: function(){}});
    });

    beforeEach(module('lss-launchpad'));

    beforeEach( module(function($provide) {
        $provide.value('authorizationService', stubAuthorizationService);
        $provide.value('authorizationInterceptor', function(){});
    }));

     beforeEach(inject(function($injector, _$q_, _$rootScope_, _$httpBackend_){
        $q = _$q_;
        $httpBackend = _$httpBackend_;
        $rootScope = _$rootScope_;
        service = $injector.get('accountService');
    
    }));

    it('should exist', function(){
        expect(service).toBeDefined();
    });

    describe("login", function(){
        it('should call /Token endpoint', function(){
            var token = 'a token';
            var user = 'user';
            var password = 'word';

            $httpBackend.when('POST', '/Token', 
                "grant_type=password&username=user&password=word", 
                function(headers){
                    expect(headers['Content-Type']).toBe('application/x-www-form-urlencoded');
                    return true;
                })
                .respond({access_token: token});

            var promise = service.login(user, password);

            promise.then(function(value){
                expect(stubAuthorizationService.setToken.calledOnce).toBe(true);
                expect(stubAuthorizationService.setToken.calledWith(token)).toBe(true);
                expect(value).toBe(true);
            });

            $httpBackend.flush();
        });
    });

});