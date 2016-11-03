describe('claim-dashboard.component', function(){
    var controller;
    var scope;

    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function($rootScope, $componentController){
        scope = $rootScope.$new();
        controller = $componentController('lssClaimDashboard', {$scope: scope});
    }));

     it('should exist', function(){
        expect(controller).toBeDefined();
    });

});