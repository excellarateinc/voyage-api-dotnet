describe('status.component', function(){

    var controller;
    var scope;
    var status = {
        name: "status1"
    };

    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function($rootScope, $componentController){
        scope = $rootScope.$new();
        controller = $componentController('lssStatus', {$scope: scope}, {status: status});
    }));

    it('should exist', function(){
        expect(controller).toBeDefined();
    });

    it("should have a status", function(){
        expect(controller.status).toBeDefined();
    });

});