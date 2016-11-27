## Authorization Standards
The following section covers securing a web api endpoint (controller method). All authorization will be handled via authorize filter attributes. 

## Table of Contents
* [Anonymous Access](#anonymous-access)
* [Require Authentication](#require-authentication)
* [Require Security Claim](#require-security-claim)
* [Claim Management](#claim-management)
* [Securing A New Controller](#securing-a-new-controller)

## Anonymous Access
__Attribute:__ AllowAnonymousAttribute

The endpoint is publically available. There is no authorization requirement. This is an out of box [attribute](https://msdn.microsoft.com/en-us/library/system.web.http.allowanonymousattribute(v=vs.118).aspx).

Applying this attribute will override any previously set authorization requirements. As a result, this should be applied at the controller method to avoid accidently exposing end point with a higher security requirement. Two examples of this are the register service and the status service.

```
  [AllowAnonymous]
  [Route("account/register")]
  public async Task<IHttpActionResult> Register(RegistrationModel model)
```

:arrow_up: [Back to Top](#table-of-contents)

## Require Authentication
__Attribute:__ AuthorizeAttribute

The authorization requirement is an authenticated user. This is an out of box [attribute](https://msdn.microsoft.com/en-us/library/system.web.http.authorizeattribute(v=vs.118).aspx).

Applying this attribute will require that the user is authenticated. Additionally, it can perform role based security checks. However, the prefence is to secure end points with claims. This attribute can be applied at the class level as a safeguard though - if a claim requirement is not applied to a controller method, the end point will still require an authenticated user.

```
    [Authorize]
    [RoutePrefix(RoutePrefixes.V1)]
    public class RoleController : ApiController
```

:arrow_up: [Back to Top](#table-of-contents)

## Require Security Claim
__Attribute:__ ClaimAuthorizeAttribute

The authorization requirement is the authenticated user possess the specified claim. This is a custom attribute.

Applying this attribute wil require that the user is authenticated and the user possess the specified claims. Claims will primarily be associated to roles and then the user associated to roles. There is support for user specific claims, however this is discouaraged.

```
  [ClaimAuthorize(ClaimValue =LssClaims.ViewRole)]
  [HttpGet]
  [Route("roles/{roleId}", Name = "GetRoleById")]
  public IHttpActionResult GetRoleById(string roleId)
```

In the above example, GetRoleById requires that the requesting user has a claim of Type=lss.permission / Value=view.role. The claim type defaults to lss.permission. When the the filter executes, the identity of the principle must pass the following checks:

1. IsAuthenticated 
   * Failure results in a 401
2. HasClaim
   * Failure results in 403. Identity claims are a combination of all user claims and user's role claims. When the identity is constructed, the claims are loaded from UserClaims and RoleClaims (based on the user's role membership.)
   
When both checks are succesful, the user is considered authorized to perform the action.

:arrow_up: [Back to Top](#table-of-contents)

## Claim Management
Currently, claim strings exist in 3 places:

1. RoleClaims table in the database
   * This table is not normalized. Therefore, the claim is repeated for every role. For instance, the claim of login could exist for both the Basic Role and the Administrator Role.
2. Launchpad.Data.Migrations.Configuration.cs
   * This class contains the seed user/claims for the database. This seed is applied whenever Update-Database is run. Note: When the database 
   versioning tool is identified, this code should be migrated.
3. Launchpad.Web.Constants.LssClaims
   * This class contains the constant strings that are used when configuring the ClaimAuthorizeAttribute. Note: The strings in this class must match
   the RoleClaims.ClaimValue otherwise the authorization will fail. If you are receiving a 403, double check for typos.

:arrow_up: [Back to Top](#table-of-contents)

## Securing a New Controller
To secure a new controller do the following:

1. Add the AuthorizeAttribute to the class
   * This will safeguard the methods from being publically exposed if any other steps are missed
2. Create a new claim constant in LssClaims
   * This claim will be used for seeding and the filter attribute
3. Add the new claim to the seed method of Configuration.cs
   * This will allow the method to be tested, otherwise the result will be a 403.
4. Add a ClaimAuthorizeAttribute to the controller method
   * The ClaimValue should be set using the LssClaims constant
   
:arrow_up: [Back to Top](#table-of-contents)
