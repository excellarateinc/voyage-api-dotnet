describe('header.component', function(){
    var controller;
    var scope;

    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function($rootScope, $componentController){
        scope = $rootScope.$new();
        controller = $componentController('lssHeader', {$scope: scope});
    }));

     it('should exist', function(){
        expect(controller).toBeDefined();
    });

});