describe('widget.component', function(){

    var controller;
    var scope;
    var widget = {
        name: "widget1"
    };

    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function($rootScope, $componentController){
        scope = $rootScope.$new();
        controller = $componentController('widget', {$scope: scope}, {widget: widget});
    }));

    it('should exist', function(){
        expect(controller).toBeDefined();
    });

    it("should have a widget", function(){
        expect(controller.widget).toBeDefined();
    });

});