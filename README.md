# judge.net
Programming contests system.

Can be used for organization local programming competitions.

## Features
- Problems archive
- Scheduling of contests with different rules:
  - ACM-like
  - Points-based
- Support non-compilable languages
- Dynamically add new languages

## Components
- Web application
- Windows host to store tests and run solutions
- Console application for checking solutions
- SQL Server database

## Installing and configuration
- Judge.Web. Set connection string to SQL Server in `Web.config`:
    ```
    <connectionStrings>
      <add name="DataBaseConnection" connectionString="Data Source=.;Initial Catalog=Judge;Integrated Security=True" providerName="System.Data.SqlClient" />
    </connectionStrings>
    ```
- Judge.JudgeService. Set `appSettings`:
  - `WorkingDirectory` - path to directory, where Judge.JudgeService will run solutions,
  - `StoragePath` - root path to directory with tests.
  - `RunnerPath` - path to `run.exe` application - runs solutions and checks limits (can be found in judge.net\Judge\Judge.Runner\run-x64\run.exe).

### Add new problem
- Create new folder in `StoragePath`
  - Add input files as `01`, `02`, `03`, ...
  - Optional add output files as `01.a`, `02.a`, `03.a`, ...
  - Add solution checker as `check.exe`. You can create your own checker with [testlib](https://github.com/MikeMirzayanov/testlib).
- Add new problem in web application from admin panel.

# Contributing
[CONTRIBUTING.md](CONTRIBUTING.md)

# Help
If you need any help, feel free to write [me](mailto:rogatnev.sergey@gmail.com).