(function() {
'use strict';

    // Usage:
    // 
    // Creates:
    // 

    angular
        .module('lss-launchpad')
        .component('widget', {
            //template:'htmlTemplate',
            templateUrl: '/app/widget/widget.component.html',
            controller: WidgetController,
            controllerAs: 'vm',
            bindings: {
                widget: '=',
            },
        });

    //WidgetController.$inject = ['dependency1'];
    function WidgetController() {
        var $ctrl = this;
        

        ////////////////

        $ctrl.$onInit = function() { };
        $ctrl.$onChanges = function(changesObj) { };
        $ctrl.$onDestory = function() { };
    }
})();