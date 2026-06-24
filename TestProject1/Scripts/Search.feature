Feature: Search

Feature file to confirm searching of mortgage data is returned


Scenario: Search Purchase Morgage, Check Results
	Given the user loads the webpage "https://www.mortgageadvicebureau.com/find-a-mortgage/"
	When the user enters the following mortgage data
	| Key           | Value  |
	| propertyValue | 150000 |
	| term          | 30     |
	| deposit       | 15000  |
	| income        | 50000  |
	Then the user sees mortgage options with data

Scenario: Confirm API and UI results totals match
	Given the user loads the webpage "https://www.mortgageadvicebureau.com/find-a-mortgage/"
	When the user enters the following mortgage data
	| Key           | Value  |
	| propertyValue | 150000 |
	| term          | 30     |
	| deposit       | 15000  |
	| income        | 50000  |
	Then the user is given the correct amount of results

	


	