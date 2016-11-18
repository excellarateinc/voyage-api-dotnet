## IIS Hosting

### Application Pool
The application pool should be configued as follows:

* Framework => .Net Framework v4.0.30319
* Managed pipeline mode => Integrated

### Connection Strings
The application assumes that the connectionString will be called LaunchpadDataContext 
and this setting will be inherited. This can be placed in the machine or a parent site configuration. 
Please note that the default IIS editor does not appear to add the required providerName attribute to the connectionString. 
As a result, it may be necessary to edit the web.config directly in order to add the attribute. 
Failure to do so will result in a 500 error.
