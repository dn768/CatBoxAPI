# CatBoxAPI

This is a sample project using a requirement document I was provided. The requirements have been documented in the Issues on this repository, and have been completed with some assumptions made. I've tried to document my assumptions and reasoning either in the Issue comments or in source code comments.

## Building
I was planning to set up a CI/CD pipeline to a docker container registry and enable simple installation through docker, but I just don't have that kind of time this week.

Please downlownload the source code for this project from the master branch, either by downloading a zip file and extracting it, or by cloning this repo:
https://github.com/dn768/CatBoxAPI.git

For a graphical representation of the branches in the repository, I recommend sourcetree, or GitKraken if you'd like even more functionality.

## Configuring a database
I built this using SQL Server 2022 Express, intending to either provide a docker command or using docker compose, but for getting started quickly, I used a local server that was already installed and added a new database.

<details><summary>MS SQL Server Docker Container</summary>
If you have a preference for a containerized database for whatever reason, here is a command to get started with (replace with your preferred password):

```docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=<your-password-here>" -p 1433:1433 --name sqlserverdb -h mysqlserver -d mcr.microsoft.com/mssql/server:2022-latest```

If you have a SQL Server instance already installed, you may need to update the port from 1433 to one that is open.
</details>

<details open><summary>MS SQL Server Installed Locally</summary>
For the easiest experience installing a database for this project, I recommend installing SQL Server Dev or Express edition, available from https://www.microsoft.com/en-us/sql-server/sql-server-downloads
</details>

<details><summary>SQLite by Request</summary>
If I knew that I wouldn't have time to finish the docker version, I probably would have opted to use SQLite, and I'm still open to updating this project to use it if that would be easier to try it out with.
</details>

Regardless of the database installation method you choose, please create a database and update appsettings.json with an appropriate connection string to access that database, updating this section:

```
"ConnectionStrings": {
  "CatBoxDatabase": "Server=localhost\\SQLEXPRESS;Database=CatBoxDB;Trusted_Connection=True;TrustServerCertificate=true;"
}
```
If this were a real project, I would suggest creating a new SQL Server login with appropriately limited permissions, but in this case, you can simply delete the database after testing.

If you have any issues, please don't hesitate to let me know, by phone, email, or in an Issue as mentioned below.

## Project Status
This is not meant to ever be deployed, since it doesn't handle user accounts, authentication, or authorization whatsoever.

## Support
If you have any questions or trouble installing, please open an Issue on this repository, and I will respond as soon as possible. Thank you!
