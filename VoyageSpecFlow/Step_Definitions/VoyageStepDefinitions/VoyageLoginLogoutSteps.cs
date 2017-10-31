using System;
using TechTalk.SpecFlow;

namespace VoyageSpecFlow
{
    [Binding]
    public class VoyageLoginLogoutSteps
    {
        [Given(@"I do not exist as a user")]
        public void GivenIDoNotExistAsAUser()
        {
            Console.WriteLine("Throw exception stating user doesnt exists, this message should be propogated to user screen");
        }
        
        [Given(@"I exist as a user")]
        public void GivenIExistAsAUser()
        {
            Console.WriteLine("Send welcome greetings and return the Access token");
        }
        
        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            Console.WriteLine("Receive welcome greetings and forward the call to home page");
        }
        
        [When(@"I sign in with valid credentials")]
        public void WhenISignInWithValidCredentials()
        {
            Console.WriteLine("Home page is visible with my items and links based on the premissions");
        }
        
        [When(@"I click sign out button")]
        public void WhenIClickSignOutButton()
        {
            Console.WriteLine("Successful loggout message is displayed");
        }
        
        [Then(@"I see an invalid login message")]
        public void ThenISeeAnInvalidLoginMessage()
        {
            Console.WriteLine("I see a invalid login message and instruction to register as new user or provide a valid credentials");

        }

        [Then(@"I should be signed out")]
        public void ThenIShouldBeSignedOut()
        {
            Console.WriteLine("I see a invalid login message and instruction to register as new user or provide a valid credentials");
        }
        
        [Then(@"I should be signed in")]
        public void ThenIShouldBeSignedIn()
        {
            Console.WriteLine("Receive welcome greetings and forward the call to home page");
        }
        
        [Then(@"I should be on home page")]
        public void ThenIShouldBeOnHomePage()
        {
            Console.WriteLine("Home page is visible with my items and links based onthe premissions");
        }
    }
}
