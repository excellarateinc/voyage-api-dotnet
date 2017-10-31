Feature: Voyage Login & Logout
	In order to get access to protected sections of the site
	A user
    Should be able to sign in and sing out

Scenario: User is not signed up
      Given I do not exist as a user
      When I sign in with valid credentials
      Then I see an invalid login message
      And I should be signed out

Scenario: User logged in successfully
      Given I exist as a user
      When I sign in with valid credentials
      Then I should be signed in
      And I should be on home page

Scenario: User logged out
      Given I am logged in
      Then I should be on home page
      When I click sign out button
      Then I should be signed out