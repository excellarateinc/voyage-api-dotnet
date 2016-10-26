describe('status-list.component', function(){
    var scope;
    var $componentController;
    var mockStatusService;
    var sandbox;
    var controller;
    var status;
    var $q;
    var $rootScope;

    afterEach(function(){
        sandbox.restore();
    });

    beforeEach(function(){
        sandbox = sinon.sandbox.create();
    });

    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function(_$rootScope_, $componentController, $injector, _$q_){
       
        $q = _$q_;
        $rootScope = _$rootScope_;
        scope = $rootScope.$new();
        var service = $injector.get('statusService');
        mockWidgetService = sandbox.mock(service);
        controller = $componentController('lssStatusList', {$scope: scope, widgetService: service});
        status = [{name: "1"}, {name: "2"}];
    }));

    it('should exist', function(){
        expect(controller).toBeDefined();
    });

    describe("$onInit", function(){
        it("should call the status service", function(){
            //arrange
            var deferred = $q.defer();
            mockWidgetService.expects('getStatus').once().returns(deferred.promise);

            //act
            controller.$onInit();
            deferred.resolve(status);
            $rootScope.$digest();

            //assert
            
            expect(controller.statuses).toBeDefined();
            expect(controller.statuses.length).toBe(2);
            mockWidgetService.verify();
        });
    });

});