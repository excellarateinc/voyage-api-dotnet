## Setup tips for Flyway ##

- SQL Server Connection Example

	In the flyway.conf file include 

	flyway.url=jdbc:jtds:sqlserver://[host]:[port]/[database name];instance=[instance name]

	flyway.user=[user name]

	flyway.password=[password]

	flyway.locations=filesystem:[location of scripts]

- TCP/IP has to be enabled in SQL Server Configuration Manager -> SQL Server Network Configuration.

- SQL Server Browser service has to be started in order for the named SQL Server instance reference to be understood.

- If flyway.user and/or flyway.password is not set, then you'll be prompted from the command line.

- To use Windows Authentication, press enter when prompted for user and password.

- Flyway 4.0.3 comes with the jdbc jtds driver located in the /drivers folder.

## Script Naming Convention ##

### Project Management ###

- All Flyway SQL scripts are located in the Launchpad.Data/Migrations directory.

### DDL Scripts ###

- DDL script naming convention: V[mmddyyyyhhmm]__[Schema-ObjectName].sql

- Each DDL script must target only one SQL Server object, such as V053020161704__Core-Users.sql

- Steps for determining the timestamp (mmddyyyyhhmm) value:
			
		1. Get lastest on master
		2. Locate the DDL (V) script with the same Schema-ObjectName and note the timestamp of the lastest version.
		3. The timestamp for the new version must be greater and should be the current date/time.
		4. Make a comment in the launchpad-database channel, stating the full file name of the new version. This will provide version sequence coordination and awareness among team members.
		5. Include a brief description of the changes being applied.
		6. Include the name of the branch that contains the new version.
		7. Commit the change.

### Seed Data Scripts ###

- Seed script naming convention: R__[GlobalSequence#]_[TableGroupName].sql

- Each seed script is applied to one or more group of tables. A table group name is a logical name for a related set of tables, such as Users, UserAddresses, UserPhones.

- An example of a seed script name is R__01_Users.sql

- The order of application for (R)epeatable scripts is the global sequence #. R__01_Users.sql will be applied before R__02_Accounts.sql, the TableGroupName is irrelevant to order of application. The global sequence # is not used to version seed scripts, they are just re-merged with the same name.

- The order of application is important for seeding data, because of identity relationships.

- Steps for determining the global sequence #:

		1. Get latest on master
		2. Locate the seed script (R) with the highest global sequence #.
		3. The global sequence # for the new seed script should be higher than the current highest #.
		4. Make a comment in the launchpad-database channel, stating the full file name of the new version. This will provide version sequence coordination and awareness among team members.
		5. Include a brief description of the changes being applied.
		6. Include the name of the branch that contains the new version.
		7. Commit the change.

## Build Instructions ##

- When developing locally, execute the Flyway command line instruction "flyway migrate".

- For CI builds, Jenkins has Flyway and it's own flyway.conf file on the build server and will target the Migrations directory. Jenkins can pass environment parameters that map to variables in the flyway.conf file.

