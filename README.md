# MonacoRoslynCompletionProvider
Provides C# Code Completion for a Monaco Editor Component

# How to run:
	- cd Sample\wwwroot
	- npm i
	- cd..
	- dotnet run
	- open webpage in browser (normaly http://localhost:5280/index.html) but it is displayed in the console

# ToDo's :
	- Show Method Declarations
	- Better Tooltips for Hover
	- Completition after (, when entering a new keyword, ...
	- More suggestions were possible
	- Perfomance, at The Moment Everything is created for one Info and Destroyed after.
	  It's developed in some way so parts could be reused, but it should be better

# Help :
Use https://sharplab.io/ to view AST of C#, wich would help in development

# Sample
![image](https://user-images.githubusercontent.com/364896/140825032-1b7fecae-b3ba-434c-9a8d-c36278dccc91.png)
