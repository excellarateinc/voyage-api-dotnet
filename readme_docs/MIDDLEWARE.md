## Custom Middleware
Owin allows a developer to define custom middleware. This middleware is registered during the startup of the application 
and executes in order in which it is registered. Voyage has a number of custom middleware pieces that are described below.

### RewindResponseMiddleware

#### Purpose
This middleware will replace the default stream with a MemoryStream. This will allow downstream services to read and transform 
the response. Once all other middlware has excuted, the MemoryStream contents are written back to the default stream and returned as 
the body. Note: the default stream is write-only. 

#### Registration Order
This middleware should be registered as one of the first pieces in the pipeline. Any dependent middleware such as ActivityAuditMiddleware 
must be registered after. 

### ActivityAuditMiddleware

#### Purpose
This middleware will capture the request and response of API calls.

#### Registration Order
This middleware must be registered after the RewindResponseMiddleware. Additionally, it should be registered early in the pipeline prior 
to any middleware that could terminate the request. This includes the authentication middleware. Failure to register this early in the
pipeline will result in activity not being logged. 
