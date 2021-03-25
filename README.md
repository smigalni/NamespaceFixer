# NamespaceFixer

Are you doing some refactoring or restructuring  and forget to buy ReSharper license? Here is solution for you.
This small application changes your using statement and namespaces.

Runs only on Windows and analyzes only C# projects.

Build your application which you want to analyze. Should be 0 build errors.

Use Source Control for your solution before run analyze and commit everything which is not commited.

How to 
```
cd toYourFavoriteFolder

git clone https://github.com/smigalni/NamespaceFixer.git

cd NamespaceFixer\
 
dotnet publish -o c:\absolute\path\to\publish\folder

cd c:\absolute\path\to\publish\folder

NamespaceFixer.exe c:\absolute\path\to\sln\file

```

This application analyzes following:
* Project file folder should have the same name as project file. F.ex. if your project file is `WebApplication1.csproj`
the project folder where you have this file should be `WebApplication1`. Throws exception.

* Either all project files have RootNamespace or none. Warning is given.

* All .cs files should have namespaces. Throws exception.

All namespaces will be fixed, but some work on using statemets is expected because this application doesn't do any code analyze.
