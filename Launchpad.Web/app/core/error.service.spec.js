describe("error.service", function(){
    var service;

    beforeEach(module('lss-launchpad'));

    beforeEach(inject(function($injector){
        service = $injector.get('errorService');
    }));

    describe('getModelStateErrors', function(){
        it('should return an empty array when there are no errors', function(){
            var errors = service.getModelStateErrors({});
            expect(errors).toBeDefined();
            expect(errors.length).toBe(0);
        });

        it('should return validation messages when there are errors', function(){
            var failure = {
                modelState: { 
                    password: ["message 1", "message 2"],
                    name: ["message 3", "message 4"]
                }
            };

            var errors = service.getModelStateErrors(failure);

            expect(errors).toBeDefined();
            expect(errors.length).toBe(4);
            expect(errors[0]).toBe("message 1");
            expect(errors[1]).toBe("message 2");
            expect(errors[2]).toBe("message 3");
            expect(errors[3]).toBe("message 4");
        });
    });
});