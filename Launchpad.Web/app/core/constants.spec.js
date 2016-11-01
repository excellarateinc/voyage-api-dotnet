describe('constants', function(){
    var lssConstants;

    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function($injector){
        lssConstants = $injector.get('lssConstants');
    }));

    it('should have known structure', function(){
        expect(lssConstants.lssClaimType).toBeDefined();
        expect(lssConstants.claims).toBeDefined();
        expect(lssConstants.claims.login).toBeDefined();
        expect(lssConstants.claims.createClaim).toBeDefined();
        expect(lssConstants.claims.assignRole).toBeDefined();
    });

});