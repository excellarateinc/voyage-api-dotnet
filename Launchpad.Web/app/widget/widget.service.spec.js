describe('widget.service', function(){
    var $httpBackend, service;

    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function($injector){
        $httpBackend = $injector.get('$httpBackend');
        service = $injector.get('widgetService');
    }));

    it('should exist', function(){
        expect(service).toBeDefined();
    });

    describe('getWidgets', function(){
        it("should call api and return response on success", function(){
            //arrange
            var widgets = [{
                name: "widget1"
            },
            {
                name: "widget2"
            }];

            var promiseCallback = function(data){
                expect(data).toBeDefined();
                expect(data.length).toBe(2);
            };

            $httpBackend.when('GET', '/api/v2/widget')
                .respond(widgets);

            //act
            var promise = service.getWidgets();
            
            //assert
            promise.then(promiseCallback);
            $httpBackend.flush();
        });

        it('should call the api and return error on failure', function(){
            var promiseCallback = function(data){
               expect(data).toBe(404);
            };

            $httpBackend.when('GET', '/api/v2/widget')
                .respond(404);

            //act
            var promise = service.getWidgets();

            //assert
            promise.then(function(){}, promiseCallback);

            $httpBackend.flush();
        });
    });
});