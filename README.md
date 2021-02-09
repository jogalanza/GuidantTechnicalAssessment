# GuidantEngineering

1)	Download Visual Studio 2019 Community Edition, ensure that you’ve selected the Azure Functions Workload for the installation profile
2)	Install Azure Storage Explorer
3)	Clone this project
4)  Build/Run this project

The solution will run on Visual Studio using the Azure Storage Emulator to allow it to interact with Queues and Tables.  There is no need to create queues or tables or any other objects before running on your local machine.

This is a loan broker backend application: 

Users submit API requests to an endpoint (http://localhost:7071/api/ApiFunction), which are modified using proprietary logic to calculate approximate loan dates and likelihood of loan realization.

The loan requests are then formatted into a standard prose format for submission to the lender via email.  The final data points are written to Table Storage, and a background job (not part of this project) will prepare them for submission to lenders and send the requestor a summary of their request and the likelihood of realization and expected schedule.

This code is unfinished and contains many TODOs which indicate areas in which the original developer did not complete execution.

Use Azure Storage Explorer to visualize the Table and Queues that you are interacting with.

Please submit a pull request when your proposed implementation is finished.

Complete solutions will run without errors, properly converting API requests to Queue messages, and from there to Table Entities in Azure Storage Emulator on a neutral machine after cloning, and address the business objectives set out in the TODOs.

The code could also benefit from better error handling, and possibly enhanced logging.

Evaluation criteria include:
1) completeness of implementation
2) consistency of style, organization, brevity
3) resilience of implementation (error-handling)
4) creativity

Bonus points for Pull Requests that include unit tests to demonstrate completeness.
