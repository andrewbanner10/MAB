Feature: UpdateResults

The update results tests on the results page

Background: Sets up valid results
	Given the user loads the webpage "https://www.mortgageadvicebureau.com/find-a-mortgage/"
	When the user enters the following mortgage data
	| Key           | Value  |
	| propertyValue | 150000 |
	| term          | 30     |
	| deposit       | 15000  |
	| income        | 50000  |
	Then the user sees mortgage options with data

@UpdateValues(NotTERMs)
Scenario: The user has valid results and updates the mortgage term then new results are shown
	Given the user is on the results page
	When the user updates the '<FieldToUpdate>' value to '<ValueToUpdate>'
	And presses update results button
	Then the results are updated

Examples:
| FieldToUpdate | ValueToUpdate |
| propertyValue | 170,000       |
| term          | 25            |
| deposit       | 20,000        |
