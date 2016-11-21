## Setup tips for Flyway ##

- SQL Server Connection Example

	In the flyway.conf file include 

	flyway.url=jdbc:jtds:sqlserver://localhost:1433/FlywayTest;instance=SQLEXPRESS

- TCP/IP has to be enabled in SQL Server Configuration Manager -> SQL Server Network Configuration.

- SQL Server Browser service has to be started in order for the named SQL Server instance reference is understood.

- If flyway.user and/or flyway.password is not set, then you'll be prompted from the command line.

- To use Windows Authentication, press enter when prompted for user and password.

- Flyway 4.0.3 comes with the jdbc jtds driver located in the /drivers folder.