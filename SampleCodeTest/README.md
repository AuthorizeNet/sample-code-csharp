# Unit Test Project for testing C# Sample Codes for Authorize.Net
[![Build Status](https://travis-ci.org/AuthorizeNet/sample-code-csharp.png?branch=master)](https://travis-ci.org/AuthorizeNet/sample-code-csharp)

This project is a unit test project which tests whether the C# sample codes are working as expected. If any of the unit tests fail the 
travis build will fail.


## Using the Sample Code Test project

The samples are all completely independent and self-contained so you can look at them to get a gist of how the method works, you can use the snippets to try in your own sample project, or you can run each sample from the command line.


## Running the Samples
- Clone sample code repository.  <br />
- Open the sample code project in visual studio.<br />
- Include the AuthorizeNet.dll present in bin/debug folder.<br />
- Include the SampleCode.dll present in bin/debug folder.<br />
- Include the Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll present in bin/debug folder.<br />
- Build the solution to produce latest SampleCode.dll.  <br />
- Then go to test explorer where you will see the list of all the test cases.<br />
- Select "Run all tests" and monitor the output of tests.<br />
