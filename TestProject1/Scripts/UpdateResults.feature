Feature: UpdateResults

Feature file to confirm user can update the criteria after the inital results shown

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
Scenario: The user has valid results and updates the mortgage details then new results are shown
	Given the user is on the results page
	When the user updates the '<FieldToUpdate>' value to '<ValueToUpdate>'
	And presses update results button
	Then the results are updated

Examples:
| FieldToUpdate | ValueToUpdate |
| propertyValue | 170,000       |
| term          | 25            |
| deposit       | 20,000        |

Scenario: The user has valid results and updates multiple mortgage details term then new results are shown
	Given the user is on the results page
	When the user updates the '<FieldToUpdate1>' value to '<ValueToUpdate1>'
	And the user updates the '<FieldToUpdate2>' value to '<ValueToUpdate2>'
	And presses update results button
	Then the results are updated

Examples:
| FieldToUpdate1 | ValueToUpdate1 | FieldToUpdate2 | ValueToUpdate2 |
| propertyValue  | 170,000        | term           | 35             |
| term           | 25             | deposit        | 25,000         |
| deposit        | 20,000         | propertyValue  | 190,000        |
