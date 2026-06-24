I have used reqnroll (was specflow as now depreciated) and selenium with the page object model of By instead of IwebElement object model
as it allows for slightly better reliability when the user is doing waits or dynamic pages as the objects are
retrieved when they are required. (I am familiar with both). Reason for reqnroll is that it allows for easy to read tests, simple reuse of test data
without having to write a new test, i can paramterise the test and pass in the value in a table that sits in the test. So 1 test with the table of 
data will run until that table of data is finished .

playwright was also another option to use however I have only created a proof of concept using that so decided to stick with Selenium and C# .Net
I do know that playwright has better automatic waits which overall means it is slightly faster in comparison to selenium (which does have slighly better support as
it is older).

The page objects for each page are the element locators and then any methods that are required for testing that page.
Then each of the feature files created will have its own script file that will call any methods on that page object file.
This keeps all the code out of both the feature files and the scripts / steps that the feature files use.

I have also within the feature files use background scenarios to have data pre setup using the UI (getting results) 
so that each feature (test) within that file will run that background scenario.
Also some tests use "Examples" which is a table of data so with 1 scenario you have run X amount of tests using the
given information and gives reuse to the steps easily.

I would ideally liked to have run a API call to setup any test data for the getting of the mortgage results
but that is not something that i have access too within your system. (if it is even possible) this would make the tests
quicker instead of having the UI doing the setup

I would also like to ideally make the searching spinner more robust in terms of it appearing and disappearing
so that once it has appeared and then disappeared the test continues (it works at the moment but it is a little slow)
It is tricky because even though the searching spinner has disappeared it still sits in the page (DOM).

I am also assuming that the data returned on the searches is correct 
(I would assume there is some form of validation in your code to check the offers returned are correct?).
And also that the need for validation / negative testing on the searches / updates is not required for this exercise.
For example you set the Term to 0, validation error is shown.

Point of note - There could be an improvement to some elements to have different classes etc based on state (some do, some dont).
such as getting the Inital Rate as an example also returns hidden elements with no value so that has to be coded around. Some elements also have Ids
others dont.

I also have never do any API / network intercepting, I followed guide online which was slightly outdated but gave me and good starting point
plugged the guide code into ai and it gave me an updated (for the latest selenium) code snippet which returns the response for the getresults api
call and then public (int TotalPages, int TotalCount, int PageItemCount) ExtractPayloadMetrics() grabs the data we are interested in (this could be improved)
and then is used in my method to check the API and UI match. (for the pagination,click the > arrow until it disappears, record how many times (to comapre page count)
and each time you press > get the count of items of the page and add to total UI count then once the > arrow is gone we can stop and check that the API and UI match.
There is no TotalCount within the API response, so I get the initial totalPages and PageItemCount then set the inital API count to be TotalPages-1
as the last page might not have 10 items. then multiply by the PageItemCount. Once we know we are on the last page, we do another call to the API to get the PageItemCount and 
then add it to the API total count and then compare with the value in the UI.

Tests are reported in a folder (it currently gets created on the desktop or can be created itself called TestRuns), 
if the test fails it reports the error and a screenshot of the UI (test error is also shown in the IDE when running locally, 
ideally this would be reported out to the cloud or machine running the test).
Some asserts also capture the error such as if Value A is not bigger than Value B and the values for both.
This is not on all the asserts as some are already self explanitary (such as Assert.xyz.Count > 0)

If the test pass it creats a log file that simply states the test passed and all asserts were correct.
I would use extent reports for this as it gives clearer reporting. You could also take screenshots when
asserts pass if required.

Flaky tests - Within alot of dev ops pipelines that run tests you can see the history of a test date
ran and result and time taken. You can look at reporting out if a test fails on Run 1 (on a build) but then
passes a re run. (you can also do this with time for the test to run such as taking 1minute on average to over 2 minutes)
although this might not indicate a flaky test it would aleast alert the user that something is happening (system has gotten slower)

From a google you can use AI to help with self healing of tests. There are ways for a the AI to match
objects within the DOM. For example
If driver.FindElement() fails, the framework activates a fallback algorithm that scores nearby DOM elements 
based on that metadata map. 
If it finds an element with an 85% contextual match 
(e.g., the class changed from product-initial-terms to product-initial-term), 
it heals the test live, 
routes the click to the new element, 
saves the test from failing, 
and automatically generates a pull request to update the test script.

Reading on how to do this there is some effort involved and it looks like you need to have a history of the tests too.
However if it is able to stop the test fail and make the required changes after then that saves alot of time
in the long run debugging / investigating and making the changes manually.
However we need to ensure that good practice is still undertaken and that the changes made are relevant and accurate.
If there going to be making changes to get a test to pass but actually its an incorrect change (such as a bug) we
wont want the changes being made and would need to have that test correctly fail.

Finally, I would have liked to add parallel test running into the solution this makes the test runs alot easier however it would require making the automation framework "thread" safe
and would require updated logging and screenshot changes too.






