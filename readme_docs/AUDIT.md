## Audit
Most company security policies require embedded surveilence of user actity to monitor data access and data changes in the event that an unauthorized event occurs and auditors need to know exactly what happened in the application by a specific user. The audit capabilities included within this web services API include user request auditing as well as data change auditing. 

### Table of Contents
* [Activity Audit](#activity-audit)
* [Data Audit](#data-audit)

### Activity Audit
Activity auditing captures all requests made to a particular API endpoint and writes a record with metadata to the database. The metadata that is collected would include the authenticated user, requestor's IP Address, current date and time, request URL, request body (if POST, PUT, PATCH), and any other pertinent information the developer has accessible and determines should be in the activity audit log. 

** Talk about response status code and result. How is this logged?

Activity auditing is implemented as a piece of custom Owin middleware. To ensure that every request is logged, 
the audit middleware is registered as the first step in the pipeline. When the middleware is activated, it will record 
an ActivityAudit to capture the request and then invoke the next piece of middleware. Once the subsequent middleware executes, 
another activity audit record is generated to record the response.

#### Data Audit
The activity auditing uses the core.ActivityAudit table.

| Column | Description | 
|:----|:----|
|Id | Primary Key |
|RequestId | Correlation ID that can be used to match the request/response records |
|Method | HTTP Method of the request | 
|Path | Requested resource | 
|IpAddress | Remote IP address of the request |
|Date| DateTime of the record |
|StatusCode | The response status code. **Note:** On requests this will be 200. It is the initial response status code at the start of the pipeline. |
|Error| Error (if applicable) |
|Username| The request user name. **Note:** On requests this will be 'No Identity' since the audit record is created before authentication occurs.|

### Database Auditing
Database auditing is implemented using the [Tracker Enabled DbContext](https://github.com/bilal-fazlani/tracker-enabled-dbcontext)
nuget package. This package includes a custom DbContext called TrackerIdentityContext. The VoyageDataContext inherits from this class. 
When save changes is called on the context, the ChangeTracker is used to create audit records. 

Each entity must be configured for auditing. In the application, this is done by creating an IAuditConfiguration. There should be a 
configuration per entity that should be audited. There is a base class called BaseAuditConfiguration which by default will track
all properties on the entity. Overriding Configure allows a user to ignore certain properties on the entity. The configurations will be invoked
from the DataModule at application start.

```
public class ApplicationUserAuditConfiguration : BaseAuditConfiguration<ApplicationUser>
{
        public override void Configure()
        {
            EntityTracker
                .TrackAllProperties<ApplicationUser>()
                .Except(_ => _.PasswordHash);
        }
}
```

#### Soft Deletes
Auditing will track soft deletes if the model implements ISoftDeleteable. When the Deleted flag is changed to true, it will be tracked as SoftDeleted and when the flag is changed to false it will be tracked as UnDeleted. Additional information can be found [here](https://github.com/bilal-fazlani/tracker-enabled-dbcontext/wiki/8.-Soft-Deletable)

#### Database Tables
The nuget package defines the tables that store the auditing records.

| Column | Description | 
|:----|:----|
| core.AuditLog | Creates the header record for the auditing operation |
| core.AuditLogDetail | Creates the change details |
| core.LogMetadata| Stores custom metedata (unused) |

The event types map to the following enumeration:

| enum  | Value |
|:----|:----|
|Added|0|
|Deleted|1|
|Modified|2|
|SoftDeleted|3|
|UnDeleted|4|
