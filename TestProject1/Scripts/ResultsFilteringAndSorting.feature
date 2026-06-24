Feature: Results Filtering And Sorting


Background: Sets up valid results
	Given the user loads the webpage "https://www.mortgageadvicebureau.com/find-a-mortgage/"
	When the user enters the following mortgage data
	| Key           | Value  |
	| propertyValue | 150000 |
	| term          | 30     |
	| deposit       | 15000  |
	| income        | 50000  |
	Then the user sees mortgage options with data

Scenario: The user has valid results and updates the mortgage term then new results are shown
	Given the user is on the results page
	When the user updates the fixed term value to '<TermValue>'
	Then the fixed term results are updated and contain '<Months>'
	
Examples:
| TermValue | Months |
| 2 years   | 24     |
| 3 years   | 36     |
| 5 years   | 60     |


Scenario: The user has valid results and updates the sort by, then the correct sort order is applied
	Given the user is on the results page
	When the user updates the sort by value to '<SortBy>'
	Then the results are updated and sorted by '<SortBy>' 
	
Examples:
| SortBy                |
| Total Cost            |
| Initial Interest Rate |
| Product Fees          |
| Initial Monthly Cost  |


Scenario: The user has valid results and updates the sort by and fixed term filters, then the correct filtering is applied
	Given the user is on the results page
	When the user updates the sort by value to '<SortBy>'
	And the user updates the fixed term value to '<TermValue>'
	Then the results are updated and sorted by '<SortBy>'
	And the fixed term results are updated and contain '<Months>'
	
Examples:
| SortBy                | TermValue | Months |
| Total Cost            | 3 years   | 36     |
| Initial Interest Rate | 5 years   | 60     |
| Initial Monthly Cost  | 3 years   | 36     |

