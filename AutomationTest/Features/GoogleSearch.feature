Feature: GoogleSearch

Scenario: User able to search text in google url

When User navigates to google
And Search Test in search text field 
And User clicks on First link 
Then User should be able to navigate to the result page 

 