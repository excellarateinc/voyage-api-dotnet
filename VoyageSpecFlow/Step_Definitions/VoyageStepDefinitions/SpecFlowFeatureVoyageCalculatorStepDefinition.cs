using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace VoyageSpecFlow.VoyageStepDefinitions
{
    [Binding]
    public sealed class SpecFlowFeatureVoyageStepDefinition
    {
        // For additional details on SpecFlow step definitions see http://go.specflow.org/doc-stepdef

        [Given("I have entered (.*) into the calculator")]
        public void GivenIHaveEnteredSomethingIntoTheCalculator(int numbers)
        {
            Console.WriteLine(numbers);
        }

        [When("I press add")]
        public void WhenIPressAdd()
        {
            Console.WriteLine("Add button pressed");
        }

        [Then("the result should be (.*) on the screen")]
        public void ThenTheResultShouldBe(int result)
        {
            if(result == 120)
                Console.WriteLine("Test Passed");
            Console.WriteLine("Test Failed");
        }
    }
}
