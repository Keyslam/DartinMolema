# DartinMolema

## Dependencies
**Required**\
`.NET SDK v6.0.x`

**Optional**\
`NPM any version` 

## Running the application
Run (Watch mode) `dotnet watch --project src/Core/Core.Common`\
Run `dotnet run --project src/Core/Core.Common`\
Test `dotnet test`

Or use the included NPM scripts

## Project structure
```
/DartinMolema
├── /src
|   ├── /Builder        Implementations of the Builder pattern for creating models
|   ├── /Core           Entry point of the application
|   ├── /GameRuler      Business logic for handling a match
|   ├── /Model          Data models for objects in the application
|   ├── /Repository     Implementations of the Repository pattern for saving and loading models
|   └── /View           Graphical User Interfaces
└── /Data               The application data, managed by the application 
    ├── /Players
    |   └── ...
    └── /Matches
        └── ...
```

## Continuous integration
The tests are ran automatically on pull request. If any test fails the pull request may not be approved.\
It is encouraged to make sure all tests pass before making a pull request.

## Branches
`main` contains the "released" code. It must always be in a working state.\
`develop` contains all finished code. At the end of the sprint it is merged into `main`.\
`feature/xxx` is used to develop a feature. It may be merged into `develop` with a pull request when finished.\
`hotfix/xxx` is used to to hotfix a bug. It may be merged into `main` with a pull request when finished.

## Code Conventions

The Code Conventions of VSCode apply. Code is formatted continously.\
Some extra rules apply that can not be linted for. These will be checked during review.

### Naming
|Kind           |Rule          |
|---------------|--------------|
|Private field  |lowerCamelCase
|Public field   |lowerCamelCase
|Protected field|lowerCamelCase
|Internal field |lowerCamelCase
|Property       |UpperCamelCase
|Method         |UpperCamelCase
|Class          |UpperCamelCase
|Interface      |IUpperCamelCase
|Local variable |lowerCamelCase
|Parameter      |lowerCamelCase

### Fields vs Properties
Try to avoid using fields, substituting them for Properties with the applicable get and set.\
`private int myField;` => `private int MyField { get; set; }`\
`public readonly int myField;` => `public int MyField { get; }`

### Field/Property initialization
Initialize fields and properties in the constructor. If the field can not be initialized them, mark it as nullable.
Pure data objects are an exception to this rule.

### Performance
Avoid performing slow actions like fetching objects from the repository, when the result could be cached instead.