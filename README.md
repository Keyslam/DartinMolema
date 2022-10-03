# DartinMolema

## Dependencies
**Required**\
`.NET SDK v6.0.x`

**Optional**\
`NPM any version` 

## Running the application
Run (Watch mode) `dotnet watch --project App`\
Run `dotnet run --project App`\
Test `dotnet test`

Or use the included NPM scripts

## Project structure
```
/DartinMolema
├── /App            The source code of the project
|   └── ...
├── /App.Tests      The tests for the source code of the project
|   └── ...
└── /Data           The application data, managed by the application 
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