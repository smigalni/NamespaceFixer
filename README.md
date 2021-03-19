# NamespaceFixer

Are you doing some refactoring or restructuring  and forget to buy ReSharper license? Here is solution for you.
This small application changes your using statement and namespaces.

Runs only on Windows (at least for now) and analyzes only C# projects.

How to 
```
cd toYourFavoriteFolder

git clone https://github.com/smigalni/NamespaceFixer.git

cd NamespaceFixer\
 
dotnet publish -o c:\absolute\path\to\publish\folder

cd c:\absolute\path\to\publish\folder

NamespaceFixer.exe c:\absolute\path\to\sln\file

```

Use Source Control for your solution before run analyze.

This application analyzes following:
* Project file folder should have the same name as project file. F.ex. if your project file is `WebApplication1.csproj`
the project folder where you have this file should be `WebApplication1`. Throws exception.

* Either all project files have RootNamespace or none. Warning is given.

* All .cs files should have namespace. Throws exception.
