# NamespaceFixer

Are you doing some refactoring or restructoring and forget to buy Resharper lisense. Here is solution for you. This small application change your using statement and namespaces.

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

Use Source Controll for your solution before run analyze.
