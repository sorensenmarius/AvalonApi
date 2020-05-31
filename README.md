# AvalonApi

## Setting up the database
### Installing software
1. Install Visual Studio 2019 Community from https://visualstudio.microsoft.com/downloads/
2. Install SQL Server Express from https://www.microsoft.com/nb-no/sql-server/sql-server-downloads
    * Write down your connection string when the installation is done
3. Install SQL Server Management Studio (SSMS) from https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms

### Setting up
1. Clone this repository
2. Open the solution in Visual Studio
3. Input the SQL Server ConnectionString in appsettings.json (in MultiplayerAvalon.Web.Host)
4. Set MultiplayerAvalon.EntityFrameworkCore as your startup project
5. Open the Package Manager Console
6. Run "Update-Database" in the Package Manager Console
7. The database should now be setup and usable in SSMS
