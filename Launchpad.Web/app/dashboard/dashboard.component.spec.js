describe('dashboard.component', function(){
    var scope;
    var $componentController;
    var mockWidgetService;
    var sandbox;
    var controller;
    var widgets;
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
        var service = $injector.get('widgetService');
        mockWidgetService = sandbox.mock(service);
        controller = $componentController('lssDashboard', {$scope: scope, widgetService: service});
        widgets = [{name: "widget1"}, {name: "widget2"}];
    }));

    it('should exist', function(){
        expect(controller).toBeDefined();
    });

    describe("$onInit", function(){
        it("should call the widget service", function(){
            //arrange
            var deferred = $q.defer();
            mockWidgetService.expects('getWidgets').once().returns(deferred.promise);

            //act
            controller.$onInit();
            deferred.resolve(widgets);
            $rootScope.$digest();

            //assert
            
            expect(controller.widgets).toBeDefined();
            expect(controller.widgets.length).toBe(2);
            mockWidgetService.verify();
        });
    });

});