(function() {
'use strict';

    angular
        .module('lss-launchpad')
        .constant('lssConstants', {
            lssClaimType : 'lss.permission',
            claims: {
                login: 'login',
                createClaim: 'create.claim',
                assignRole: 'assign.role'
            }    
        });
})();

    