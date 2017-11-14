Feature: Voyage resource assessed & access denied
	In order to get access to protected sections of the site
	A user
    Should be able to get an access to a resource based on the access it possess

Scenario: User do not have access to a resource
      Given I do not have access to resource
      When I access the resource url 'http://url'
	  Then I see access denied to resource message

Scenario: User logged in successfully and I have access to the requested resource
      Given I exist as a user
      When I sign in with valid credentials
      Then I should be signed in
      And I should be on home page
	  When I access the resource url 'http://url'
	  Then I get access to the resouces and I see the requested resource

Scenario: User logged in successfully and I dont have access to the requested resource
      Given I exist as a user
      When I sign in with valid credentials
      Then I should be signed in
      And I should be on home page
	  When I request for an access to a resource
	  Then I get access denied 402 exception and the exception is propogated to the UI