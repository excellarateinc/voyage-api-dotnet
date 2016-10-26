describe('status.service', function(){
    var $httpBackend, service;

    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function($injector){
        $httpBackend = $injector.get('$httpBackend');
        service = $injector.get('statusService');
    }));

    it('should exist', function(){
        expect(service).toBeDefined();
    });

    describe('getStatus', function(){
        it("should call api and return response on success", function(){
            //arrange
            var status = [{
                name: "1"
            },
            {
                name: "2"
            }];

            var promiseCallback = function(data){
                expect(data).toBeDefined();
                expect(data.length).toBe(2);
            };

            $httpBackend.when('GET', '/api/v2/status')
                .respond(status);

            //act
            var promise = service.getStatus();
            
            //assert
            promise.then(promiseCallback);
            $httpBackend.flush();
        });

        it('should call the api and return error on failure', function(){
            var promiseCallback = function(data){
               expect(data).toBe(404);
            };

            $httpBackend.when('GET', '/api/v2/status')
                .respond(404);

            //act
            var promise = service.getStatus();

            //assert
            promise.then(
                function(){
                    expect(false).toBe(true); //make sure success was not called
                }, 
                promiseCallback);

            $httpBackend.flush();
        });
    });
});