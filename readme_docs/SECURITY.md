## Security
Overview of the Security features and configurations that have been implemented within the Voyage API. 

## Table of Contents
* [Secure Programming](#secure-programming)
* [Security Features](#security-features)
  - [Authentication: Force OAuth Tokens Expired](#authentication-force-oauth-tokens-expired)
  - [Authentication: OAuth2 (default)](#authentication-oauth2-default)
  - [Authentication: Spring Security](#authentication-spring-security)
  - [Authorization: Permission Based](#authorization-permission-based)
  - [2-Factor Authentication](#2-factor-authentication)
  - [Cross Origin Resource Sharing (CORS)](#cross-origin-resource-sharing-cors)
  - [Cross Site Request Forgery (CSRF)](#cross-site-request-forgery-csrf)
  - [Disable API Consumers (Users and Clients)](#disable-api-consumers-users-and-clients)
  - [OWASP Top 10](#owasp-top-10)
  - Password Attempts Tracking
  - Password Policy
  - [Password Recovery](#password-recovery)
  - [User Verification](#user-verification)
* [Security Configuration](#security-configuration)
  - [CORS Configuration](#cors-configuration)
  - [Environment Specific Application Properties](#environment-specific-application-properties)
  - [Encryption Public/Private Key Configuration](#encryption-publicprivate-key-configuration)
  - [JWT Public/Private Key Configuration](#jwt-publicprivate-key-configuration)
  - [Public Resources](#public-resources)
  - [User Verification Configuration](#user-verification-configuration)
* [Audit Logging](#audit-logging)
  - [Action Logs](#action-logs)
  - [Change Logs](#change-logs)
  - [User & Date Stamps](#user--date-stamps)
  - [Logical Deletes](#logical-deletes)

## Secure Programming
The majority of technology security breaches that occur are through software applications. As developers create new software they need to be very mindful of secure programming principals in order to protect the users and companies that use the software. A developer needs to simply Google "Secure Coding" or "Secure Programming" to find many articles on best practices for secure programming. We've included a number of references below that are a great start. 

All programmers working on this app should at least read through the reference materials and take the introductory courses on secure programming offered by the [OWASP Academy](http://owasp-academy.teachable.com) with course title 'OWASP AppSec Tutorials' by Jerry Hoff. While the Voyage API lays down many protections within the frameworks and features it uses, every developer is responsible for understanding why these frameworks were put in place, how these frameworks fortify security, and what to do in their own code when they are building new features. 

#### References
* https://www.owasp.org/index.php/OWASP_Secure_Coding_Practices_-_Quick_Reference_Guide
* https://www.securecoding.cert.org/confluence/display/seccode/Top+10+Secure+Coding+Practices
* https://www.securecoding.cert.org/confluence/display/java/Java+Coding+Guidelines 
* http://owasp-academy.teachable.com
* https://www.lynda.com/Programming-Foundations-tutorials/Techniques-Developing-Secure-Software/418266-2.html

:arrow_up: [Back to Top](#table-of-contents)


## Security Features

### Authentication: Force OAuth Tokens Expired

#### Overview
OAuth2 is configured within this API to use JWT to generate tokens. By design, JWT embeds user information within the token so that the resource API can use the data to pre-load the session with an authenticated user. One of these bits of information is a token expiration date that is added to the token based on the client's max token validity time period `Client.accessTokenValiditySeconds`. The OAuth2 resource server will examine the JWT expiration date embedded within the token and reject the request if the token has exceeded the expiration date. 

One downside of JWT and the default Spring Security OAuth2 resource API is that neighter one support expiring the token before the expiration date is effective. If there is a security event that requires issued tokens to be expired for a given User or Client, then without an explicit way to invalidate JWT tokens the system could remain vulnerable to attack until the tokens naturally expired. 

This API implements the ability to invalidate Client or User tokens that have not reached their JWT expiration date. 

#### User Tokens - Force Tokens Expired Date
To expire the tokens of a particular User, update the User.forceTokensExpiredDate field with the overriding expiration date. Any tokens that were created before the User.forceTokensExpiredDate will be treated as expired tokens. If all tokens created before the current point in time needs to be expired, then set the User.forceTokensExpiredDate value to the current date and time. Tokens created after the User.forceTokensExpiredDate will be valid until the expiration date in the JWT token naturally expires. 

#### Client Tokens - Force Tokens Expired Date
To expire the tokens of a particular Client, update the `Client.forceTokensExpiredDate` field with the overriding expiration date. Any tokens that were created before the `Client.forceTokensExpiredDate` will be treated as expired tokens. If all tokens created before the current point in time needs to be expired, then set the `Client.forceTokensExpiredDate` value to the current date and time. Tokens created after the `Client.forceTokensExpiredDate` will be valid until the expiration date in the JWT token naturally expires. 

#### InvalidateOAuthTokensServletFilter
The InvalidateOAuthTokensServletFilter is a servlet filter that examines the authenticated user on the incoming request to see if the valid JWT token should be expired before it's natural expiration date. The InvalidateOAuthTokensServletFilter is required to run after Spring Security authentication filters has authenticated the Client or User and assumes the JWT is valid.

##### Token created date
The first check that the filter performs is to verify that a `created` attribute is embedded within the token. If the `created` attribute is not set, then an exception is thrown and the consumer will receive a 500 error. 

The `created` date is not a attribute that is embedded into a JWT token by default. The `created` date attribute is added into the token within the OAuth2Config.groovy configuration file.

##### Client.forceTokensExpiredDate examined first
The Client is examined first to see if tokens generated by the client before the `client.forceTokensExpiredDate` should be expired. If the token was generated before the `client.forceTokensExpiredDate`, then the request is aborted immediately and an "Access Token Expired" error message is returned to the consumer. The effect of expiring tokens at the client level will be that any User who used this client to interact with the API will need to re-authenticate to obtain a new access token.

If no `client.forceTokensExpiredDate` is set for the Client, then the Client examination will be skipped. 

##### User.forceTokensExpiredDate examined second
If the Client doesn't require the token to be expired, then the `User.forceTokensExpiredDate` is compared with the token `created` date. If the token was created before the `User.forceTokensExpiredDate`, then the request is aborted immediately and an "Access Token Expired" error message is returned to the consumer. 

If no `User.forceTokensExpiredDate` is set for the User, then the User examination will be skipped and the filter will pass the request on to the next filter in the filter chain. Effectively, if the Client and User do not need to force the token to be expired, then nothing else is done. 

In cases where a client is making a server-to-server connection, then a User object will be not be loaded into the session and the User examination will be skipped. 

:arrow_up: [Back to Top](#table-of-contents)


### Authentication: OAuth2 (default)
![OAuth2 Implicit Authentication Workflow](./images/SECURITY_OAUTH2.png)
 
#### Overview
The default security configuration of Voyage API is OAuth2 with the Implicit Authentication and Client Credentials authenticaiton workflows implemented. OAuth2 was chosen as the default authentication mechanism over a simple username/password workflow because it provides a common pattern implemented by many enterprises, allows for a more secure login process when using Implicit Authentication, and enables the API to be a branded Authorization server should it desire to allow third-party apps to interface with its web services. 

Voyage API implements OAuth2 natively within the application using [Spring Security OAuth2](https://projects.spring.io/spring-security-oauth/docs/oauth2.html) framework. 
 
Walk through accessing secured web services using both Implicit Authentication and Client Credentials in the [Development: Access Secured Web Services](./DEVELOPMENT.md#access-secured-web-services) section. 
 
#### Authentication Server
The Authentication Server is an independent component of OAuth2 that is responsible for authenticating users and returning secure tokens for accessing the Resource Server. The Authentication Server can be a third-party entity (ie Google, Facebook) or a privately hosted server. Voyage API implements its own Authentication Server following the [Spring Security OAuth2](https://projects.spring.io/spring-security-oauth/docs/oauth2.html) defined structure.  

The configuration for the Authentication Server is discussed in more detail within the [Security Configuration](#security-configuration) section. The implementation of the Authentication Server can be found at `/src/main/groovy/voyage/config/OAuth2Config.groovy`. Within the config class, both of the Authorization Server and the Resource Server are defined. 

##### Highlights of the Authorization Server implementation:
```
@Configuration
class OAuth2Config {

    /**
     * Configures the OAuth2 Authorization server to use a custom ClientDetailsService and to govern access to
     * authorization endpoints.
     */
    @Configuration
    @EnableAuthorizationServer
    class AuthorizationServerConfig extends AuthorizationServerConfigurerAdapter {

        @Value('${security.jwt.key-store-filename}')
        private String keyStoreFileName

        @Value('${security.jwt.key-store-password}')
        private String keyStorePassword

        @Value('${security.jwt.private-key-name}')
        private String privateKeyName

        @Value('${security.jwt.private-key-password}')
        private String privateKeyPassword

        @Autowired
        private AuthenticationManager authenticationManager

        @Autowired
        private PermissionBasedClientDetailsService permissionBasedClientDetailsService

        @Autowired
        private WebResponseExceptionTranslator apiWebResponseExceptionTranslator

        @Bean
        JwtAccessTokenConverter accessTokenConverter() {
            KeyStoreKeyFactory keyFactory = new KeyStoreKeyFactory(new ClassPathResource(keyStoreFileName), keyStorePassword.toCharArray())
            KeyPair keyPair = keyFactory.getKeyPair(privateKeyName, privateKeyPassword.toCharArray())
            JwtAccessTokenConverter converter = new JwtAccessTokenConverter()
            converter.keyPair = keyPair
            return converter
        }

        @Override
        void configure(AuthorizationServerSecurityConfigurer oauthServer) throws Exception {
            oauthServer

                // Expose the verifier key endpoint "/oauth/token_key" to the public for validation of the JWT token
                .tokenKeyAccess('permitAll()')

                // Require users to be authenticated before accessing "/oauth/check_token"
                .checkTokenAccess('isAuthenticated()')
        }

        @Override
        void configure(AuthorizationServerEndpointsConfigurer endpoints) throws Exception {
            endpoints
                    .authenticationManager(authenticationManager)
                    .accessTokenConverter(accessTokenConverter())
                    .exceptionTranslator(apiWebResponseExceptionTranslator)
        }

        @Override
        void configure(ClientDetailsServiceConfigurer clients) throws Exception {
            clients.withClientDetails(permissionBasedClientDetailsService)
        }
    }
...
```
1. Extends the stock Spring Security OAuth2 framework
2. Implements stateless JSON Web Token (JWT) as the token provider with a changeable public/private key for encoding the token. 
   - See [Security Configuration](#security-configuration) for instructions on how to configure the JWT public/private key by environment
3. Implements a custom exception translator to ensure all expected or unexptected issues are handled consistently
4. Implements a custom Permission Based Client authorization service that grants access to resources based on the Permission records associated with their profile. 
   - See [Authorization: Permission Based](#authorization-permission-based) for more information. 
   - Inspect `/src/main/groovy/voyage/security/PermissionBasedClientDetailsService` for implementation details.

#### Resource Server
The Resource Server is an independent component of OAuth2 that is responsible for facilitating secured access to the web services provided by the API (ie HTTP GET /api/users). The Resource service is always implemented by the API application as it hosts the web services that comprise the API product. The Resource Server shouldn't need to know anything about the Authentication Server's location or how it operates other than the method by which to validate tokens and certify that they originated from the Authentication Server. Voyage API implements the Resource Server following the [Spring Security OAuth2](https://projects.spring.io/spring-security-oauth/docs/oauth2.html) defined structure.  

The configuration for the Resource Server is discussed in more detail within the [Security Configuration](#security-configuration) section. The implementation of the Resource Server can be found at `/src/main/groovy/voyage/config/OAuth2Config.groovy`. Within the config class, both of the Authorization Server and the Resource Server are defined. 

##### Highlights of the Resource Server config:
```
@Configuration
class OAuth2Config {
...

    @Configuration
    @EnableResourceServer
    class ResourceServerConfig extends ResourceServerConfigurerAdapter {
        private static final String ANY_PATH = '/**'
        private static final String API_PATH = '/api/**'
        private static final String READ = "#oauth2.hasScope('Read Data')"
        private static final String WRITE = "#oauth2.hasScope('Write Data')"

        @Value('${security.permitAll}')
        private String[] permitAllUrls

        @Autowired
        private WebResponseExceptionTranslator apiWebResponseExceptionTranslator

        @Override
        void configure(HttpSecurity http) throws Exception {
            http

                // Limit this Config to only handle /api requests. This will also disable authentication filters on
                // /api requests and enable the OAuth2 token filter as the only means of stateless authentication.
                .requestMatchers()
                    .antMatchers(API_PATH)
                    .and()

                // Bypass URLs that are public endpoints, like /api/v1/forgotPassword
                .authorizeRequests()
                    .antMatchers(permitAllUrls).permitAll()
                    .and()

                // Enforce client 'scope' permissions on all authenticated requests
                .authorizeRequests()
                    .antMatchers(HttpMethod.GET, ANY_PATH).access(READ)
                    .antMatchers(HttpMethod.POST, ANY_PATH).access(WRITE)
                    .antMatchers(HttpMethod.PUT, ANY_PATH).access(WRITE)
                    .antMatchers(HttpMethod.PATCH, ANY_PATH).access(WRITE)
                    .antMatchers(HttpMethod.DELETE, ANY_PATH).access(WRITE)
                    .and()
        }

        @Override
        void configure(ResourceServerSecurityConfigurer resources) throws Exception {
            resources
                // Override exception formatting by injecting the accessDeniedHandler & authenticationEntryPoint
                .accessDeniedHandler(accessDeniedHandler())
                .authenticationEntryPoint(authenticationEntryPoint())
        }
        ...
    }
}
```
1. Extends the stock Spring Security OAuth2 framework
2. Implements a stateless JWT token for authentication
3. Intercepts all `/api` requests and authenticates the user based on the JWT token
   - All other requests are ignored and naturally picked up by the base Spring Security frameowrk for processing.
   - Base Spring Security implementation is located at `/src/main/groovy/voyage/security/WebSecurityConfig.groovy`
4. Defaults all `/api` requests to be secured
5. Allows for exposure of publicly accessible `/api` access by specifying public URL paths in an external configuration file
6. Implements a custom exception translator to ensure all expected or unexptected issues are handled consistently
7. Enforces client Authorization based on the grants associated with their profile
   - The only grants supported are 'READ' and 'WRITE'
   - READ grant maps to HTTP GET
   - WRITE grant maps to HTTP POST, PUT, PATCH, DELETE
   - Client's that do not have these grants configured in their profile stored in the database cannot perform these operations. 

#### Implicit Authentication
An authentication workflow that essentially is initiated by the client app (ie mobile app with embedded client ID) where the end-user is transfered over to the server-side authentication form(s). The Authentication server will validate the incoming client ID (no password is given), and then facilitate one or more secure login pages that accept and validate the end-user. Once the end-user authentication has completed successfully, then the user is redirected back to the client using the "redirect_url" from the client's profile in the database. Redirecting the user back to the client URL is a safe and secure way of getting the user back to a known / registered app. 

> NOTE: The Spring Security implicit authentication currently supports multiple redirect URLs. OAuth2 requires that the client provide the redirect_url in the initial hand-off of the end-user. Spring Security OAuth2 framework will validate that the given redirect_url matches a value within the client's profile in the database. If the given redirect_url value doesn't match a client redirect_url in the database exactly, then the authentication process will throw an error and stop. 

Key points:

1. The user instructs the client 'app' to make API requests on the user's behalf. 
2. The client initiates the authentication using their client ID, but does not provide a password because the user will be required to enter their own username and password to authorize the client. 
3. The API will load both the Client and User objects into the session
4. This authentication method is the preferred method for a web or mobile app

Walk through accessing secured web services using both Implicit Authentication and Client Credentials in the [Development: Access Secured Web Services](./DEVELOPMENT.md#access-secured-web-services) section. 

#### Client Credentials
A server-to-server authentication workflow where a client passes to the authenticaiton server a Client ID and Client Secret to authenticate. Upon successful authentication, the client is given an access token that can be used for web service requests. The Client Credentials workflow should only be used in situations where the client can guarantee secure storage of the Client Secret, which would reserve this communication method to server-side integration with the authentication server. Client-side apps, like Javascript clients (AngularJS), native mobile apps, or hybrid mobile apps are not considered secure and should not have an embedded Client Secret within the source code. Therefore, client apps should NOT use Client Credentials as an authentication method. 

> NOTE: Some might argue that compiling a secure password into the native mobile app binary and deploying that to a mobile device is secure. This is definitely not the case! There are many examples on the Internet on [decompiling Java/Android](https://infosecguide.wordpress.com/2013/12/17/step-by-step-guide-to-decompiling-android-apps/) and [iOS native mobile apps](http://reverseengineering.stackexchange.com/questions/4096/decompiling-iphone-app) to reveal source code and to grab passwords. 

Key points:

1. The client accesses the API directly without a user and uses a secure password to authenticate.
2. The client is the only actor using the API and must provide a client ID and password
3. The API will not load a User object into the session unless the client ID maps to a User username
4. API services that require a User object loaded into memory will not function with this authentication method
5. This authentication method is reserved for testing and for server-to-server exchanges

Walk through accessing secured web services using both Implicit Authentication and Client Credentials in the [Development: Access Secured Web Services](./DEVELOPMENT.md#access-secured-web-services) section. 

:arrow_up: [Back to Top](#table-of-contents)



### Authentication: Spring Security
[Spring Security](https://projects.spring.io/spring-security/) is the foundational security framework implemented within the Voyage API. Since Voyage API utilizes [Spring Boot](https://projects.spring.io/spring-boot/) to bootstrap the app, the [Spring Security Spring Boot](https://projects.spring.io/spring-boot/) module was added to the project to provide base level Spring Security features. 

Even though the default authentication method is OAuth2, Spring Security does support other authentication methods such as direct user Basic Auth and Forms Login. In fact, Spring Security can support any authentication pattern required through add-on or custom extensions.  

To support an alternative Spring Security configuration, visit the main [Spring Security project](https://projects.spring.io/spring-security/) for an overview of the standard configurations for supported authentication. Once familiar with how Spring Security works, particularly for [Spring Boot](http://docs.spring.io/spring-boot/docs/current/reference/html/boot-features-security.html) projects, then there are two configuration files that must be modified: OAuth2Config.groovy, WebSecurityConfig.groovy. `OAuth2Config.groovy` simply needs to be removed if OAuth2 is no longer supported (along with supporting OAuth2 classes and config). `WebSecurityConfig.groovy` is where the base Spring Security configuration is located and simply needs to be updated to support the features desired. 

> NOTE: Be sure to read the Spring Security documentation clearly before attempting to make any changes to the security configuration. An uninformed security configuration change might expose the app to the public in unexpected ways and jeapordize the security of the data that the app represents. 

:arrow_up: [Back to Top](#table-of-contents)


### Authorization: Permission Based
#### Overview
Authorization is the process by which an authenticated user is granted access rights to specific areas of the application. Spring Security comes standard with Role Based security and many examples where a developer should embed the role name into areas of the source code. Role based security then allows for a User to be associated with one or more Roles. The challenge with this approach is that roles might need to change or expand over time and with every change required the source code will need to be modified. For example, a healthcare application might be originally written with roles Doctor, Nurse, Patient but after 6 months if the new role of Pharmacist needs to be added, then the role will need to be added to the source code in every place that a pharmacist can have access. Any time that the source code needs to be augmented to modify roles there is a risk that new bugs or security issues might be introduced. 

The less invasive and more dynamic method for securing methods within an application is to apply a unique Permission name to the method and register the Permission within a `permission` database table. With each permission registered within the database, Roles are then associated with one or more permissions within the `role_permission` database table. Extending or changing a role is as simple as updating the role with permission changes. If a new permission is added to a new feature in the application, then once the permission is registered in the `permission` table of the database, then it can be associated within the roles that require the permission. 

Finally, User records are associated with one or more Roles, which are associated with one or more Permissions. When Spring Security loads a User record for authorization, it examines the User's total set of permissions against the secured object or method being requested to provide a valid or invalid result. 

#### Spring Security Extension
Since Spring Security comes preconfigured with only Role based authorization, the Voyage API extends Spring Security with a Permission based authorization extension. In short, the primary extension implemented is an extension of the Spring Security `UserDetailsService` interface with overridden methods that describe how to load a user and how to generate the list of GrantedAuthorities for the User. 

/src/main/groovy/voyage/security/PermissionBasedUserDetailsService.groovy
```
@Service
@Transactional(readOnly = true)
class PermissionBasedUserDetailsService implements UserDetailsService {
    private final UserService userService
    private final PermissionService permissionService

    PermissionBasedUserDetailsService(UserService userService, PermissionService permissionService) {
        this.userService = userService
        this.permissionService = permissionService
    }

    @Override
    PermissionBasedUserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        User user = userService.findByUsername(username)
        if (!user || !user.isEnabled) {
            throw new UsernameNotFoundException("User ${username} was not found.")
        }
        return new PermissionBasedUserDetails(user, getAuthorities(user))
    }

    private Collection<? extends GrantedAuthority> getAuthorities(User user) {
        Set<SimpleGrantedAuthority> authorities = [] as Set<SimpleGrantedAuthority>
        Iterable<Permission> permissions = permissionService.findAllByUser(user.id)
        permissions?.each { permission ->
            authorities.add(new SimpleGrantedAuthority(permission.name))
        }
        return authorities
    }
}
```
Highlights
* PermissionBasedUserDetailsService is configured within the `/src/main/groovy/voyage/config/WebSecurityConfig.groovy` file
* loadByUsername creates a PermissionBasedUserDetails object with a listing of GrantedAuthority objects
* getAuthorities(User user) fetches all permissions associated with the user

Example of a service protected using Permission based authorization: UserController
```
@RestController
@RequestMapping(['/api/v1/users'])
class UserController {
    private final UserService userService

    @Autowired
    UserController(UserService userService) {
        this.userService = userService
    }

    @GetMapping
    @PreAuthorize("hasAuthority('api.users.list')")
    ResponseEntity list() {
        Iterable<User> users = userService.listAll()
        return new ResponseEntity(users, HttpStatus.OK)
    }
}
```
Highlights
* @PreAuthorize is SpringSecurity annotation that uses Spring Expression Language (SpEL) to define rules before the method can be executed
* hasAuthority('api.users.list') executes the SpEL hasAuthority to verify if the given 'api.users.list' is within the currently logged in User's GrantedAuthority list.
* If the user has the matching granted authority, then the method will be executed. If the user does not have the granted authority, then an access denied exception will be thrown by Spring Security. 

:arrow_up: [Back to Top](#table-of-contents)

### 2-Factor Authentication
#### Overview
2-Factor Authentication, in short, is validating a user by something they know AND by something they have in their possession. Voyage implements a 2-factor authentication workflow that requires user authentication through a username and password, and then requires the user to be able to receive a code via SMS and enter it into the app as the second form of authentication. If a user forgets their username or password, then a set of security questions are asked of the user that require the user to demonstrate "something they know". Once again, after the user answers the security questions successfully, then they are required to verify their identity by entering a code sent to their mobile phone via SMS. Only after validating both of these data from the user will the user have fully authenticated themselves.

The API enforces 2-factor authentication by triggering the [User Verification](#user-verification) process for the authenticated user. Read more about how User Verification features works within the APi within the [User Verification](#user-verification) documentation. 

#### References 
* [What is Two Factor Authentication](https://www.securenvoy.com/two-factor-authentication/what-is-2fa.shtm)
* [Testing Multiple Factors Authentication](https://www.owasp.org/index.php/Testing_Multiple_Factors_Authentication_(OWASP-AT-009))

:arrow_up: [Back to Top](#table-of-contents)


### Cross Origin Resource Sharing (CORS)
#### Overview
CORS is a feature built into web browsers and web servers that allow for bi-directional communication on the allowance for a web page to make calls to other servers other than the originator of the content. Browsers have for a long time restricted web sites from making calls out to sites that are not from the web page origin. 

Voyage API implements server-side CORS instructions for consumers operating out of web browsers, such as an AngularJS app. 

#### CORS Vulnerabilities
Even though CORS provides valuable protection from hackers, it also exposes a fundamental architecture flaw that hackers are able to exploit. The 'CORS Abuse' reference link below describes the situation in detail. In short, a hacker can send to the web server an Origin header value containing any information that the hacker wants to send. For example:
```
Origin: imahacker.com
```
When the server application enables `Access-Control-Allow-Credentials: true`, then the CORS spec doesn't allow a public wildcard `Access-Control-Allow-Origin: *`, instead the server must return back a specific domain. Many HTTP Servers and frameworks that offer CORS support (include Spring Security CORS add-on), will simply echo back the value that is provided in the `Origin` request header. Anytime a server echos back values given to it, that echo'd response becomes a hacker foothold for all sorts of mischief. 

#### Custom CORS Filter for Voyage
Voyage API provides it's own implementation of the CORS filter at `/src/main/groovy/voyage/security/CorsServletFilter`. Features of this custom CORS filter are:
* Integrated with the OAuth 'client' invoking the request
* if the 'client' requesting access to the API is authenticated, then the given Origin on the request is matched to the Client Origins in the database (client_origin table)
  - if a match is found, then return the value _in the database_ as the value for `Access-Control-Allow-Origin` header response
  - if no match is found, then default to being permissive and return a public wildcard `Access-Control-Allow-Origin: *`
* if the request is anonymous (client not logged in)
  - default to being permissive and return a public wildcard `Access-Control-Allow-Origin: *`

> NOTE: Defaulting to permissive origin in CorsServletFilter because an assumption is made that the security framework will catch unauthorized requests and prevent access. For a more restrictive implementation, consider extending this class or replacing it with a different implementation.

#### References
* [OWASP: Cross Origin Resource Sharing - Origin Header Scrutiny](https://www.owasp.org/index.php/CORS_OriginHeaderScrutiny)
* [CORS Abuse](https://blog.secureideas.com/2013/02/grab-cors-light.html)


:arrow_up: [Back to Top](#table-of-contents)


### Cross-Site Request Forgery (CSRF)
#### Overview
CSRF is the ability of a hacker to hijack session information stored within a web browser by invoking a request to the website where the session information was generated. The hijacker may not be able to access the session information in the browser, but they can impersonate a prior session and get valuable information back from the website. For example, if the end-user logged into a banking website and a Session Cookie was pushed to the end-user's browser to keep them logged in, then a hijacker could invoke banking requests as an authenticated user without having to know the user's login credentials. 

The common way to thwart this attack is by including a web server generated code that is embedded into each page displayed to the end-user. When the user submits information back to the server, the web server generated code must be given back to the server where it is validated before any actions are processed.

Web service APIs are typicaly single transactions, in fact, good APIs strive to be a simple request/response to complete a task. Requiring a consumer to call a web service to get a CSRF token to then submit to another web service request seems a bit much. Even still, if a web service API maintains state between web service requests via a Cookie or persistent Basic Auth, then a web service is open to a possible CSRF attack. 

The initial construction of Voyage API strongly discourages the use of the Servlet Session or anything that would retain state beyond the HTTP Request. The current authentication and authorization of the `/api` resource server uses JWT tokens transmitted through the HTTP Request Headers, which must be placed into the header for each request. No Cookies are supported in the `/api` resource server and HTTP Basic Auth is disabled. 

Given the architecture of Voyage API, no CSRF controls are built into the API. Please revise this section if the web services API for this app requires the use of Cookies and/or Sessions that span multiple requests. 

#### References
* [OWASP: CSRF Prevention Sheet](https://www.owasp.org/index.php/Cross-Site_Request_Forgery_(CSRF)_Prevention_Cheat_Sheet)

:arrow_up: [Back to Top](#table-of-contents)


### Password Recovery

* Can be disabled within the application.yaml file for internal applications with a centralized Password Recovery process. 

Workflow
1. User initiates the password recovery process from a link within the web app
2. User is required to enter their username and mobile phone used to create the account
3. Upon successful verification of the username and mobile phone number, the user is presented with a set of security questions
4. The user must answer each question exactly as they had entered it within their account profile and submit
5. If the answers are correct, then the user is displayed a

Security Questions
- Security questions are presented at random

Technical Notes:
* Need to do this without authenticating a user. If we authenticate a user, then they would get a token and have free reign to attempt other attacks


1. Once a user enters a valid username and phone number, then return a Recovery Verify Token
   - POST /recover/password { username: blah, password: ***** }
   - Anonymous access web service
   - > 3 attempts from for a given username (existing or not) will disable attempts for 10 minutes
   - > 6 attempts from for a given username (existing or not) within a 60 minute period will disable attempts for the username for 24-hours
   - Track all attempts within the action_log so that non-existing usernames can be tracked as well  
   - The Recovery Verify Token is only good for 20 minutes (shorter?) and should be stored on the user profile for date expiration
   - Use a long hash token that wont likely be replicated between multiple users ("account recovery" + userid + datetime)
   - Send verification code to mobile phone on record
     * Sets user.is_verify_required with code and expiration
     * Next time user logs in with their username and password, they will be required to verify
2. Accept verification code 
   - POST /recover/verify  { recovery_verify_token: ANDJDUS*#*, code: 3423432 }
   - anonymous access web service 
   - > 3 attempts from for a given recovery code or IP address (existing or not) will disable attempts for 10 minutes
   - Check that the recovery token has not expired
   - Follows User Verification process
   - Upon successful verification, return a Recovery Questions Token
3. Request security questions
   - POST /recover/questions { recovery_questions_token: EOIUWORJSDFN#373432 }
   - Requires the account recovery token
   - Requires user.is_verify_required = false (otherwise returns an error status code with message)
   - > 3 attempts with an invalid account recovery token from a given IP address will ban the IP Address for 60 minutes (configurable)
   - Returns 3 of the 5 security questions (2 canned, 1 custom / out of 3 canned and 2 custom) rotated
   - Each question should have a question ID hash key that is generated using the question_code or the question text (and stored in the database or on-the-fly?)
     ```
     [
        {"question_id": "LKSJFSDJ*#&SAAHANDL*", "question": "What is the avg airspeed velocity of an unladen swallow?", "question_code": "avg_airspeed_swallow"},
        {"question_id": "&#$DSDJ356", "question": "What was your favorite teacher's last name?", "question_code": "favorite_teacher"},
        {"question_id": ")(SKSDFJLH#$", "question": "When is my favorite day of the year?", "question_code": "favorite_day_of_year"}
     ]
     ```
4. Verify security questions
   - POST /recover/questions/answers { recover_questions_token: EOIUWORJSDFN#373432, questions: [ {question_id: &#$DSDJ356, answer: "my answer", question_id: ... }]
   - Requires the account recovery token
   - Requires user.is_verify_required = false (otherwise returns an error status code with message)
   - > 3 attempts with an invalid account recovery token from a given IP address will ban the IP Address for 60 minutes (configurable)
   - All questions must have exact answers that match one-way hashed answers in the database
   - > 3 attempts with an invalid security answers will ban the recovery token for 10 minutes
   - RETURNS a Recovery Change Password Token
5. Change Password
   - POST /recover/password/change { recovery_change_password_token: 34987DHKWJHWERNAHQH, new_password: "changeme" }
   - sets user.is_credentials_expired = false (if it was set to true)
6. Redirect user to the login page
   - Should be able to login fine without any sort of user verification since this was handled during the extensive password recovery process. 

NOTE: UserPasswordExpiredServletFilter will intercept this and force password reset process. Unauthenticated users will use this path, authenticated users will be able to change their password at any time if they provider their current password first)


#### References
* [Forgot Password Cheat Sheet](https://www.owasp.org/index.php/Forgot_Password_Cheat_Sheet)
* [Choosing and Using Security Questions Cheat Sheet](https://www.owasp.org/index.php/Choosing_and_Using_Security_Questions_Cheat_Sheet)

:arrow_up: [Back to Top](#table-of-contents)


### Disable API Consumers (Users and Clients)
#### Overview
In the [OAuth2 workflow](#authentication-oauth2-default), there can be up to 2 actors that are involved when authenticating with the API: Client, User. A server-to-server connection using the OAuth2 "client_credentials" authorization type will only authenticate the Client credentials provided. A user-to-server connection using the "implicit" authorization type will require a validated Client and will only authenticate the User using the credentials provided directly from the user. 

In any given request, the API administrators might want to invalidate a Client or a User immediately. The following sections describe how each actor can be disabled to prevent access to the API. 

#### Disable Clients
Disabling a client record will reject any incoming API request associated with the client immediately when the client is disabled. When a client is disabled, a user cannot authenticate from the referring client because the client will be disabled. 

To disable a Client, update the `client` table within the database and set the `client.is_enabled` to false.

#### Disable Users
Disablig a user record will reject any incoming API request associated with the user immediately when the user is disabled. 

To disbale a User, update the `user` table within the database with any of the following attributes:
* `user.is_enabled = false` - Disables the user account.
* `user.is_account_expired = false` - Alternative way to disable the user account with a notice that the account has expired
* `user.is_account_locked = false` - Alternative way to disable the user account with a notice that the account has been locked

:arrow_up: [Back to Top](#table-of-contents)


### OWASP Top 10
#### Overview
The most recent Open Web Association of Secure Programmers (OWASP) [Top 10 most exploited custom app vulnerabilities](https://www.owasp.org/index.php/OWASP_Top_Ten_Cheat_Sheet) are as follows:

1. [Injection](#1-injection)
2. [Weak authentication and session management](#2-weak-authentication-and-session-management)
3. [XSS](#3-xss)
4. [Insecure Direct Object References](#4-insecure-direct-object-references)
5. [Security Misconfiguration](#5-security-misconfiguration)
6. [Sensitive Data Exposure](#6-sensitive-data-exposure)
7. [Missing Function Level Access Control](#7-missing-function-level-access-control)
8. [Cross Site Request Forgery](#8-cross-site-request-forgery-csrf)
9. [Using Components with Known Vulnerabilities](#9-using-components-with-known-vulnerabilities)
10. [Unvalidated Redirects and Forwards](#10-unvalidated-redirects-and-forwards)

#### 1. Injection
SQL/HQL, OS, and LDAP injection occur when untrusted data is passed in to the API request and applied to a command that is then sent to an interpreter like a SQL or LDAP engine. If the command that is sent to the interpreter is not properly screened, then the attacker can trick the interpreter to perform tasks that were not originally intended. 

A simple SQL injection attack: 

Vulnerable code:
```
String sql = "SELECT * FROM user WHERE username='" + username + "' AND password='" + password + "'
Result result = database.query(sql)
```

Attacker parameters:
```
username=admin
password=1' OR 1=1
```

When the final SQL is constructed and stored in the `sql` variable in the app, the attacker SQL will look like:
```
SELECT * FROM user WHERE username='admin' AND password='1' OR 1=1
```

Every SQL interpreter will translate this command to return any user where the username is 'admin' and the password is '1' or where 1=1, which is always true. The end result of this query is to return all user records. If the application is only expecting one record back, sometimes the database API will simply return the first record in the list as a fail-safe option, which would then authenticate the user. 

##### SQL Injection
SQL injection is prevented within the API by following a strict rule to always using a parameterized SQL builder such as the [Spring JDBC template](https://docs.spring.io/spring/docs/current/spring-framework-reference/html/jdbc.html#jdbc-core). The Spring JDBC template provides a parameter replacement feature that will also assume all parameter values are untrusted and will escape all special characters that would interfere with a SQL query, such as a single quote. 

```
int countOfActorsNamedJoe = this.jdbcTemplate.queryForObject(
        "select count(*) from t_actor where first_name = ?", Integer.class, "Joe");
```

The parameter `first_name` has a question mark (?) as the parameter place holder. The following two parameters of the `queryForObject` method include the paramter datatype and the parameter value. If the parameter value contained a value like `Joe' or 1=1`, then the Spring JDBC Template processor would escape the single tick character with a second single tick and translate that into the following query:
```
select count(*) from t_actor where first_name = 'Joe'' or 1=1'
```
The SQL query above would be interpreted to look for any first name containing "Joe'' or 1=1", which is not likely going to be any first name in the table. 
  
  
##### HQL Injection
Hibernate Query Language (HQL), also known as Java Persistence Query Language (JPQL), is just as vulnerable to attackers as SQL when a query is constructed using bare string concatination. Just like SQL injection, HQL must utilizing the Hibernate query builders to SET parameters into the query for processing and replacement into the final query to the database. 

Utilzing the [Repository interface for Spring Hibernate](https://docs.spring.io/spring-data/data-commons/docs/1.6.1.RELEASE/reference/html/repositories.html) makes the task easy by simply defining the query with parameter placeholders as an annotation. Spring + Hibernate take care of the rest without concer for HQL injection. 

```
interface UserRepository extends CrudRepository<User, Long> {

    @Query('FROM User u WHERE u.username = ?1 AND u.isDeleted = false')
    User findByUsername(String username)

    @Query('FROM User u WHERE u.id = ?1 AND u.isDeleted = false')
    User findOne(Long id)

    @Query('FROM User u WHERE u.isDeleted = false')
    Iterable<User> findAll()
}
```

In the UserRepository interface define in the above code snippet, two of the queries have parameter placeholders defined with a question mark (?) and a number indicating which method argument to use. Spring does the work to translate the method argument and set it into a parameterized HQL query so that the parameter is handled and processed as if it were untrusted data. 

##### References
* [OWASP Top 10 - A1-Injection](https://www.owasp.org/index.php/Top_10_2013-A1-Injection)

:arrow_up: [Back to Top](#table-of-contents)


#### 2. Weak authentication and session management
While this is a broad topic, OWASP evaluates if an app is vulnerable by the following interogation (not inclusive of every possible attack vector):
1. User credentials stored using strong hashing or encryption? 
2. Accounts can be guessed or overwritten if account creation is weak, change/recover password is weak, weak session IDs, etc.
3. Session IDs are exposed in the URL
4. Session IDs are vulnerable to [session fixation](https://www.owasp.org/index.php/Session_fixation) attacks. 
5. Session IDs, access tokens, or SSO is not properly invalidated during logout
6. Session IDs are not rotated after successful login
7. Passwords, session IDs, and other credentials are sent over unencrypted connections. 

##### Password storage
Per [OWASP recommendations](https://www.owasp.org/index.php/Cryptographic_Storage_Cheat_Sheet), the API utilizes the [bcrypt](https://en.wikipedia.org/wiki/Bcrypt) one-way hashing module based on the blowfish cipher. All passwords (user and client) and security question answers are stored as one-way hashes that require the user to enter the values exactly as they were originally entered in order for the hash to be reproduced and matched with the hash stored in the database. 

##### Session Management
The current position of the API is that sessions are to be avoided if at all possible as they are not that useful within a stateless API and can expose severe vulnerabilities if not handled properly. The [OWASP documentation on Session Management](https://www.owasp.org/index.php/Session_Management_Cheat_Sheet) does a good job of explaining how Sessions should be handled if/when they need to be used within an application. 

The only area within the application that currently utilizes a session is during the [OAuth2 implicit authentication workflow](#implicit-authentication). The OAuth2 implicit authentication workflow receives a request from a Client (ie mobile app) to authenticate with the API. When the authentication request is received, OAuth2 stores the client credentials, returns a JSESSIONID cookie for storage into the user's browser, and then redirects the user's browsers to the login page. The user will then enter their login credentials and upon successful authentication of credentials, the JSESSIONID will be used to lookup the client credentials for validation before redirecting the user back to the client application. 

Notes about the security in place during the JSESSIONID session ID stored within the user's Cookies:
1. The entire transaction must be conducted over SSL
   - This must be enforced by the system administrators when routing requests to the API via a web server or load balancer
   - Exposing the JSESSIONID through unencrypted traffic would allow for an attacker to impersonate the end-user
2. The JSESSIONID is used only to store the client credentials until the user successfully authenticates. 
3. Once the user authenticates, then the JSESSIONID is invalidated
4. All requests made to the API do not use a JSSESSIONID, which makes the session ID useless for attacks on any other part of the system

##### SSL / Encrypted Channel
The API doesn't implement controls to enforce SSL for every transaction by default. Spring Security offers a featuer in which the API will detect if the HTTP Protocol is not HTTPS and will redirect to an HTTPS port (default 443). Should the system administrators wish to enforce SSL within the application, then the following example from [Spring Security - Requires Channel](http://docs.spring.io/spring-security/site/docs/3.2.0.RC2/apidocs/org/springframework/security/config/annotation/web/builders/HttpSecurity.html#requiresChannel%28%29) can be implemented within the API `WebSecurityConfig.groovy` file.

```
public class WebSecurityConfig extends WebSecurityConfigurerAdapter {
    protected void configure(HttpSecurity http) {
        http.requiresChannel().anyRequest().requiresSecure();
    }
}
```

##### References
* [OWASP Top 10 - A2-Broken Authentication and Session Management](https://www.owasp.org/index.php/Top_10_2013-A2-Broken_Authentication_and_Session_Management)


:arrow_up: [Back to Top](#table-of-contents)


#### 3. XSS
Cross-Site Scripting (XSS) is perhaps one of the most common web app exploits on the Internet. In short, XSS is when an attacker inputs malicious coding into an HTTP request (usually via an input field) and the malicious coding is sent back to a user for execution. The malicious coding is executed because, like SQL Injection, the coding terminates the ending of a string and then provides executable code for the web browser interperter to consume and execute. 

A web services API is a server-side component of a web or mobile application and is the hub by which a XSS malicious code is either saved to the database and sent back upon request to an unsuspecting user, or the XSS malicious code is immediately echoed back to the user through an error message or something similar. The OWASP group has a solid article that provides great detail called [OWASP XSS Prevention Cheat Sheet](https://www.owasp.org/index.php/XSS_(Cross_Site_Scripting)_Prevention_Cheat_Sheet). In short, in order to keep the API secure from XSS attacks, the following precautions must always be followed: 

1. Validate incoming data
   - Ensure the data complies with the allowable range of information (numbers, money, text, lists, ...)
   - When updating objects/records, explicitly get the data from the request that is expected, validate the range of data, then explicitly set the validated data into the database object/record. 
   - Do not blindly trust incoming data. Do not dynamically set data directly from the request into a database object/record
   - ASSUME that any request could be a potentially attacker, including authenticated and authorized users!
2. Encode request content before handling or validating within the app
   - Understand that storing code within the database that will be executed is a potentially dangerous feature!
   - See the [OWASP XSS Prevention Cheat Sheet](https://www.owasp.org/index.php/XSS_(Cross_Site_Scripting)_Prevention_Cheat_Sheet) for a detailed description on specific content types and vulnerabilities.
   - Do not rely on the response output to handle escaping data values on-the-fly because other systems might use the database directly
   - Encode for: HTML, CSS, JS, XML, SQL, URL, LDAP, VBScript, OS Commands
3. Properly escape special characters in all response header and body values
   - When returning data back to the user, ensure that data values are properly escaped so as to prevent HTML/CSS/JS injection
   - Escape all special characters in request parameters echoed back in the response

The API currently employs the following configuration and patterns to prevent XSS attacks:

##### Encoded Request Headers & Parameters
The API encodes all incoming request data (headers & parameters) to escape potentially malicious content BEFORE the API handles the data. The encoded content supported is as follows: HTML, CSS, Javascript, VBScript, SQL, OS Commands, LDAP, XPATH, XML, URL.

Encoding is handled using the [OWASP Enterprise Security API (ESAPI)](https://www.owasp.org/index.php/Category:OWASP_Enterprise_Security_API), which provides out-of-the-box encoding. 

The encoding is handled in `/src/main/groovy/voyage/security/XssServletFilter.groovy` within the `stripXSS(String value)` method
```
private static String stripXSS(String value) {
   if (value) {
      // Avoid encoded attacks by parsing the value using ESAPI
      value = ESAPI.encoder().canonicalize(value)

      // Avoid null characters
      value = value.replaceAll("", "")

      // Remove all sections that match a pattern
      for (Pattern pattern : patterns) {
         value = pattern.matcher(value).replaceAll("")
      }
   }
   return value
}
```

The `stringXSS(String value)` method is called whenever the API gets request header values or request parameters. See the full details of how the XssServlet filter works within the source referenced above. 

##### XSS Request Header & Parameter Pattern 
The API examines all incoming request data (headers & parameters) to identify and remove suspicious XSS content before the API logic uses the data. 

XSS patterns are defined within `/src/main/groovy/voyage/security/XssServletFilter.groovy` with the actualy bulk of the work being done in the `stripXSS(String value)` method (see previous section for code details). Embedded within the XssServletFilter class is a list of predefined [regex](http://regexr.com) patterns that are examined whenever the API requests data from the incoming Request. 

Recognized XSS patterns:
```
<script>(.*?)</script>
</script>
<script(.*?)>
src[\r\n]*=[\r\n]*\\\'
eval\\((.*?)\\)
expression\\((.*?)\\)
javascript:
vbscript:
onload(.*?)=
```

These parameters may be expanded by adding additional parameters within the `XssServletFilter` class. 

##### XSS Response Headers
Spring Security is pre-configured by default to always add `X-XSS-Protection` header, which is supported by [modern web browsers](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-XSS-Protection) and essentially stops pages from loading if they detect reflected XSS attacks. Reflected data is when the user submits data and it is echoed back from the server verbatim and the data contains potentially malicious coding. 

```
X-XSS-Protection: 1; mode=block
```

The default setting from Spring Security is to [enable X-XSS-Protection](http://docs.spring.io/spring-security/site/docs/current/reference/html/headers.html#headers-xss-protection) by returning a value of `1`, and then instructing the web browser with `mode=block` to not render the page if it detects a XSS attack. 


##### Validate Incoming Data
When request data comes into the app, it's important that the header and parameter values are validated against what is expected within the app. Be quick to reject invalid data with an error message to the user explaining what happened. 

> NOTE: Do NOT echo back in the error message the failed data as this might create a XSS vulnerability. 

Use [Spring Validation framework](https://docs.spring.io/spring/docs/current/spring-framework-reference/html/validation.html) following examples like [validation of an input form](https://spring.io/guides/gs/validating-form-input/) to apply annotations to domain object properties to enforce field nullability, size range, type, minimum value, maximum value, and more. Utilize the [@Pattern](http://stackoverflow.com/questions/17481029/pattern-for-alphanumeric-string-bean-validation) validator to create your own custom validation. 

For reference validation within this API, see `/src/main/groovy/voyage/security/user/User.groovy`.

##### References
* [OWASP Top 10 - A3-Cross-Site Scripting (XSS)](https://www.owasp.org/index.php/Top_10_2013-A3-Cross-Site_Scripting_(XSS))

:arrow_up: [Back to Top](#table-of-contents)


#### 4. Insecure Direct Object References
A direct object reference is a data record identifier embedded into a URL, HTTP header, or POST parameters. An insecure direct object reference is when a user manually changes the identifier to a value that references a data record that should not be accessible by the user. 

Most API applications expose direct object references through the request parameters or POST parameters to indicate which specific record is being requested. The "object references" are typically in the form of a unique identifier. For example:
```
http://some-app/api/v1/users/1
```
The URL is meant to be human friendly and easy to read. In this case, the URL reads "requesting user ID=1 from API version 1 at some-app". 

Other examples might be even more explicit and include multiple identifiers, like:
```
http://some-app/api/v1/users?id=1

http://some-app/api/v1/users?site_id=3&user_id=1
```

It's also pretty easy for a hacker to change any of these parameters to see what happens. In the case of an "insecure direct object reference" the attacker would be able to change the `/users?id=1` to `/users?id=2` or any number to gain access to other records. This type of attack is very easy to try out by simply manipulating the URL in your browser on a website. 

##### Public access
The first line of defense is to ensure that all API endpoints that have non-public data are behind the security perimeter. By default, this API assumes that every request requires an authenticated user. If an anonymous user attempts to access an API resource, and the resource is not on a whitelist, then the request will be rejected. 

The second line of defence is to ensure that all API endpoints are restricted to security groups so that authenticated users are only eligible to access API resources that are associated with the security group(s) of the users. 

If anonymous and unauthorized requests are rejected, then there is no chance that the API resource will even be accessible to attempt this attack. 

##### Authenticated access
Having a strong security perimeter requiring authentication and verifying permissions to access API endpoints isn't enough for this type of attack. If an API allows for open registration of accounts, then any anonymous attacker can get behind the security perimeter by simply creating an account. Even if open registration is not enabled, every app should assume that its users will have malicous intent or perhaps a healthy amount of curiousity. 

Once the attacker is inside the security perimeter, then they can observe the URLs that are displayed within the web app in the browser and the URLs that are being requested to the server-side API. Any visible URL can be modified and resent with different parameters to try to get different results back. 

##### Prevention 
The question that every software developer must ask themselves when they write a web service API (or any user accessible code) "What would happen if an identifier was passed in that doesn't 'belong' to the user?". 

It's not practical or possible to create a global filter for all API endpoints that would prevent insecure direct object references because each web service endpoint may be unique in the data it provides. The most practical solution to prevent this type of attack is to educate software developers to write secure code that programmatically checks each user and their access to the data that is being requested.

See the Developer Recipe [Prevent Insecure Direct Object References](DEVELOPMENT-RECIPES.md#prevent-insecure-direct-object-references) for educational and practical short recipe for developers to create awareness of this type of attack with example scenarios on how to write secure code to prevent the app from being vulnerable. 

##### References
* [OWASP Top 10 - A4 Insecure Direct Object References](https://www.owasp.org/index.php/Top_10_2013-A4-Insecure_Direct_Object_References)

:arrow_up: [Back to Top](#table-of-contents)


#### 5. Security Misconfiguration
##### Overview
Intentionally configuring every hardware and software component within a system is vitally important to creating a hacker-proof environment. Many successful security attacks are the result of software installers not configuring the software, which defaults to whatever the software was pre-packaged to use for configuration. In many software products, a default administrative user is created with a default password like "password". If an attacker is able to obtain the user manual or source code of the software, it's faily simple to try the default account and password as a first line attack. 

##### Prevention
There are a number of prevention techniques that can be followed. Most of the prevention is through education, awareness, and strict protocol that enforce system administrators to know each configuraiton option and to set a standard configuration plan. 

* Require all requests to be secured by default
  - Default all incoming requests to require an authenticated user
  - Any resource request that should be public must update a configuration file to add an exception
  - Prevents attackers from being able to freely snoop around without at least having a user account, which records their actions within the action log. 
* Update default configuration values, especially default admin passwords
  - The API comes pre-configured with default accounts and passwords, and a default configuration setup
  - Override the account username and passwords within `/src/main/resources/db.changelog/v1-0/user.yaml`
  - Override the default configuration within `/src/main/resources/application.yaml` by following the [various ways of overriding properties](DEPLOY.md#4-apacht-tomcat-setup--override-parameters-by-environment)
* Follow security best practices for hardening the OS, network, web server, etc
  - There are many guides online for how to harden the security of hardware and software stacks. Research each technology and create a written security protcol for how each one is meant to be configured for each environment. 
* Do not display configuration or any type of application or data specifics in error messages
  - Ensure that all error messages display just enough information for the consumer of the API to know what to do next. 
  - Do not reveal additional information that could provide attackers insight into how they could exploit any part of the application, including configuration settings or default properties. 
* Remove API web service endpoints and code that are not used (these can easily be forgotten and left out-of-date)
  - Keep the application clean at all times by removing excess code, modules, frameworks, and configurations that are no longer used or needed. 
  - When starting an app using Voyage API, be sure to assess the components that are included and determine if any of them are not needed by the new app being built. 
* Update frameworks with the latest versions whenever a security patch is available
  - Attackers scour commonly used libraries and publicized security exploits to built predatory tools
  - Once an exploit is known, then attackers have a small window of opportunity to use the exploit before security conscious apps close the hole. 
  - Apps that do not close the exploitable holes quickly are susceptible for as long as the exploit is not patched!!
  - Rely on services like the Voyage subscription service to receive regular security vulnerabilty information, security patches, framework upgrades, and custom component upgrades.

##### References
* [OWASP Top 10 - A5 Security Misconfiguration](https://www.owasp.org/index.php/Top_10_2013-A5-Security_Misconfiguration)

:arrow_up: [Back to Top](#table-of-contents)


#### 6. Sensitive Data Exposure
##### Overview
The OWASP Top 10 description of Sensitive Data Exposure is sensitive data that is essentially unencrypted while being transported or at rest. Common exploits of sensitive data exposure include transporting sensitive data over unencrypted channels, like HTTP (instead of HTTPS), not encrypting sensitive data in the database allowing a SQL Injection attack to reveal private data, old cryptographic libraries used that are weak and/or have known vulnerabilities, and other situations like these. See the links in the References section below for the full description. 

##### Prevention
Many of the prevention techniques come down to consistent implementation by software developers and system administrators. There are no quick-fix frameworks or tools that can know what is sensitive data and what is not. Each component built by a software development must have a secure programming approach where every piece of data is analyzed and handled appropriately. 

* Always use HTTPS when transferring data internally or externally
  - Prevent attackers from capturing clear text data in transit by using HTTPS 
  - Use HTTPS when communicating within the LAN to prevent internal attacks looking to steal clear text data transmissions
* Encrypt all sensitive data
  - Create an organizational policy on what constitutes 'sensitive data'. The policy should include government or industry requirements such as HIPAA or PCI.
  - Educate all developers on the Sensitive Data policy and ensure security and quality assurance have procedures to validate all data storage. 
  - Use the `/src/main/groovy/voyage/security/crypto/CryptoService` to encrypt passwords and text (as well as decrypt)
* Only store sensitive data when necessary (ie credit card security code)
  - Do not blindly store all data as some sensitive data should not or cannot be stored. 
  - Understand the sensitive data policy within your organization
  - Example: PCI credit card data storage requires that the security code on the back of a credit card should never be stored in the database. 
* Use strong encryption
  - Follow the [OWASP Top 10 Cryptographic Cheat Sheet](https://www.owasp.org/index.php/Cryptographic_Storage_Cheat_Sheet) to determine the best encryption protocols for your organization
  - Default password encoding using [bCrypt](https://en.wikipedia.org/wiki/Bcrypt)
  - Default encryption using RSA public/private key encryption from the Java Cryptography Extension bundled within the JRE
  - Use the `/src/main/groovy/voyage/security/crypto/CryptoService` to encrypt passwords and text (as well as decrypt)
* Store passwords with a strong one-way hash encryption
  - Default password encoding using [bCrypt](https://en.wikipedia.org/wiki/Bcrypt)
  - Use the `/src/main/groovy/voyage/security/crypto/CryptoService` to encode passwords

##### References
* [OWASP Top 10 - A6-Sensitive Data Exposure](https://www.owasp.org/index.php/Top_10_2013-A6-Sensitive_Data_Exposure)

:arrow_up: [Back to Top](#table-of-contents)


#### 7. Missing Function Level Access Control
##### Overview
The "missing function level access control" is when a resource (aka function) is supposed to restrict access but it doesn't. The cause for why the resource is not restricted usually is that a software developer didn't implement the restriction configuration or business logic to the resource. While many web apps will hide features or functions if the user doesn't have access, many web app developers fail to secure the server-side functions for retrieving data or performing the actual task. Attackers are wise to this oversight by developers and will look in the web app browser HTML and Javascript code to find clues as to which features are hidden from view and how the back-end function is executed. When hidden functions and backend function calls are found, they are targeted for security vulnerabilities. Needless to say, an backend web service API that is missing security permission is a wide open door for an attacker to not only steal data, but potentially exploit the entire server-side infrastructure. 

##### Prevention
* Require all developement work tickets to include a requirement for the level of security
   - Be explicit to the developer what the security requirements are for each task
   - Writing down the requirement communicates to Quality Assurance and/or Security Quality Assurance that they need to test the security requirement
* Prevent Unauthenticated Users
   - The Voyage API comes pre-configured with Spring Security and a policy that forces all requests to `/api/` to be authenticated. 
   - Developers can exempt specific web services from being automatically protected in the [Public Resources](#public-resources) application properties.
   - Web services not defined in `/api` are not protected by default. Update `/src/main/groovy/voyage/config/OAuth2Config` to include additional URL patterns. 
* Prevent Unauthorized Users
   - Once a user is authenticated, there is still a vulnerability of an authenticated user accessing a web service endpoint that _doesn't_ have a permission restriction placed on it. 
   - Web service endpoints that do not have a permission restriction on them are defaulted to being accessible to any authenticated user
   - The responsibility is on the developer to ensure that a web service endpoint is secured with a permission. See the Developer Recipe [Securing a Web Service Endpoint](DEVELOPMENT-RECIPES.md#securing-a-web-service-endpoint)
* Test For Security
   - Require automated unit / integration tests that validate the security permission before the feature is considered "done"
   - Quality Assurance team should write a manual or automated regression test that asserts the security permission required in order to execute the function. 

##### References
* [OWASP Top 10 - A7-Missing Function Level Access Control](https://www.owasp.org/index.php/Top_10_2013-A7-Missing_Function_Level_Access_Control)
* [Voyage API - Authorization: Permission Based](SECURITY.md#authorization-permission-based)
* [Voyage API - Securing a Web Service Endpoint](DEVELOPMENT-RECIPES.md#securing-a-web-service-endpoint)

:arrow_up: [Back to Top](#table-of-contents)


#### 8. Cross-Site Request Forgery (CSRF)
##### Overview
CSRF attacks occur when a user authenticates with a website (ie bank) and the website places a Cookie into the web browser as an identity token for subsequent requests. The identity token is a way for the website to identify the authenticated user and their session data without requiring the user to authenticate again. Whenever the user makes a request (GET, POST, ...) to a web server, the web browser will automatically look for all stored Cookies that match the web server address and bundle them into the request. 

While it's difficult to get access to Cookies, it's not terribly difficult for an attacker to force the web browser to submit a request blindly to a known web server address. For example:

1. User logs into http://my-bank with a Cookie of "Secure User ID: 123" that is stored in the web browser cache
2. User does their banking tasks and does not log out
3. Attacker injects their exploit onto the user's web browser that POSTs a "get account details" AJAX request within the web browser window
   - Web browser submits the request and sends the user identity Cookie along with the request
4. Web server sees the Cookie, verifies the request as valid, and returns the secure information
5. Attacker receives the response from the bank and is in possession of restricted data

##### Prevention
The Voyage API comes out of the box as a JSON web services API with stateless authentication and authorization security. Cookies are not used to access `/api` resources, so the risk of a CSRF attack is very low to not possible with the base Voyage API install. There is a potential for a CSRF attack within the OAuth2 login process where a Cookie session ID reference is used to temporary data during the login process. After interrogation, we have yet to find a way to exploit the login process using a CSRF attack and strongly encourage the initiated white-hat hackers to pursue this area of the app for CSRF exploits.  

The following are best practices for preventing CSRF attacks. If the API requires the use of Sessions for authentication between requests, then utilize [CSRF tokens provided by the Spring Security framework](https://docs.spring.io/spring-security/site/docs/current/reference/html/csrf.html). As of this writing, CSRF tokens are not implemented since there is no risk of CSRF attacks with a stateless security API (as far as we have been able to determine in our extensive testing). 

1. Avoid using Cookies for storing authentication identifiers
   - Cookies are automatically sent to the server without programmatic intervention
   - CSRF leverages this automatic Cookie delivery feature in browsers to exploit server-side infrastructure
2. In a web app, programmatically set authentication credentials into the request header before every request is made to the API 
   - Don't leave it to the browser to auto-set these values
3. If Cookies are required for authentication between requests, then use CSRF Tokens
   - Spring Security provides a CSRF mechanism that generates a CSRF token on every response
   - CSRF tokens are required to be passed back to the server on every POST, PUT, DELETE, PATCH request
   - The CSRF token must match the token given to the user in the most recent response

##### References
* [OWASP Top 10 - A8-Cross-Site Request Forgery (CSRF)](https://www.owasp.org/index.php/Top_10_2013-A8-Cross-Site_Request_Forgery_(CSRF))
* [Spring Security CSRF](https://docs.spring.io/spring-security/site/docs/current/reference/html/csrf.html)

:arrow_up: [Back to Top](#table-of-contents)


#### 9. Using Components with Known Vulnerabilities
##### Overview
Very rarely are software applications written completely from scratch without any third-party frameworks or components. One of the major benefits of the Internet and the Free Open Source Software (FOSS) community is the abundance of frameworks available for routine to highly complex tasks. With the many constraints on time and budget, it's not realistic, or advisable, to build a complete application from scratch and not depend on third-party libraries. 

Building apps using third-party libraries has been much easier with public library repositories built upon library dependency tools like Maven, NPM, NuGet, etc... When a dependency is included within an application, like Spring Framework, the dependency management framework will also resolve the dependencies of the included framework, which will review all of the dependencies of the dependencies until all required third-party libraries are downloaded. In an application that simply uses one or two large popular frameworks (like Spring and Hibernate), it's very easy to have well over 100 third-party libraries required in order to run your application. 

To illustrate the point, the following dependencies are required for the Voyage API (Java) to function. Each one of these dependent libraries has a potential to contain a security exploit that might possibly compromise the whole application. In all likelihood, the most vulnerable libraries will be the ones handling the public exchange of information, like a file upload utility. Libraries that are used to calculate date ranges or format text in the lower parts of the data layer are less likely to be exploited even if they have a security vulnerability. The degree of difficulty in exploiting a security vulnerability increases with every layer of software code between the attacker and the vulnerability. 

```
> gradle dependencies --configuration runtime

------------------------------------------------------------
Root project
------------------------------------------------------------

runtime - Runtime dependencies for source set 'main'.
+--- org.springframework.boot:spring-boot-starter-web:1.4.2.RELEASE
|    +--- org.springframework.boot:spring-boot-starter:1.4.2.RELEASE
|    |    +--- org.springframework.boot:spring-boot:1.4.2.RELEASE
|    |    |    +--- org.springframework:spring-core:4.3.4.RELEASE
|    |    |    |    \--- commons-logging:commons-logging:1.2
|    |    |    \--- org.springframework:spring-context:4.3.4.RELEASE
|    |    |         +--- org.springframework:spring-aop:4.3.4.RELEASE
|    |    |         |    +--- org.springframework:spring-beans:4.3.4.RELEASE
|    |    |         |    |    \--- org.springframework:spring-core:4.3.4.RELEASE (*)
|    |    |         |    \--- org.springframework:spring-core:4.3.4.RELEASE (*)
|    |    |         +--- org.springframework:spring-beans:4.3.4.RELEASE (*)
|    |    |         +--- org.springframework:spring-core:4.3.4.RELEASE (*)
|    |    |         \--- org.springframework:spring-expression:4.3.4.RELEASE
|    |    |              \--- org.springframework:spring-core:4.3.4.RELEASE (*)
|    |    +--- org.springframework.boot:spring-boot-autoconfigure:1.4.2.RELEASE
|    |    |    \--- org.springframework.boot:spring-boot:1.4.2.RELEASE (*)
|    |    +--- org.springframework.boot:spring-boot-starter-logging:1.4.2.RELEASE
|    |    |    +--- ch.qos.logback:logback-classic:1.1.7
|    |    |    |    +--- ch.qos.logback:logback-core:1.1.7
|    |    |    |    \--- org.slf4j:slf4j-api:1.7.20 -> 1.7.21
|    |    |    +--- org.slf4j:jcl-over-slf4j:1.7.21
|    |    |    |    \--- org.slf4j:slf4j-api:1.7.21
|    |    |    +--- org.slf4j:jul-to-slf4j:1.7.21
|    |    |    |    \--- org.slf4j:slf4j-api:1.7.21
|    |    |    \--- org.slf4j:log4j-over-slf4j:1.7.21
|    |    |         \--- org.slf4j:slf4j-api:1.7.21
|    |    +--- org.springframework:spring-core:4.3.4.RELEASE (*)
|    |    \--- org.yaml:snakeyaml:1.17
|    +--- org.springframework.boot:spring-boot-starter-tomcat:1.4.2.RELEASE
|    |    +--- org.apache.tomcat.embed:tomcat-embed-core:8.5.6
|    |    +--- org.apache.tomcat.embed:tomcat-embed-el:8.5.6
|    |    \--- org.apache.tomcat.embed:tomcat-embed-websocket:8.5.6
|    |         \--- org.apache.tomcat.embed:tomcat-embed-core:8.5.6
|    +--- org.hibernate:hibernate-validator:5.2.4.Final
|    |    +--- javax.validation:validation-api:1.1.0.Final
|    |    +--- org.jboss.logging:jboss-logging:3.2.1.Final -> 3.3.0.Final
|    |    \--- com.fasterxml:classmate:1.1.0 -> 1.3.3
|    +--- com.fasterxml.jackson.core:jackson-databind:2.8.4
|    |    +--- com.fasterxml.jackson.core:jackson-annotations:2.8.0 -> 2.8.4
|    |    \--- com.fasterxml.jackson.core:jackson-core:2.8.4
|    +--- org.springframework:spring-web:4.3.4.RELEASE
|    |    +--- org.springframework:spring-aop:4.3.4.RELEASE (*)
|    |    +--- org.springframework:spring-beans:4.3.4.RELEASE (*)
|    |    +--- org.springframework:spring-context:4.3.4.RELEASE (*)
|    |    \--- org.springframework:spring-core:4.3.4.RELEASE (*)
|    \--- org.springframework:spring-webmvc:4.3.4.RELEASE
|         +--- org.springframework:spring-aop:4.3.4.RELEASE (*)
|         +--- org.springframework:spring-beans:4.3.4.RELEASE (*)
|         +--- org.springframework:spring-context:4.3.4.RELEASE (*)
|         +--- org.springframework:spring-core:4.3.4.RELEASE (*)
|         +--- org.springframework:spring-expression:4.3.4.RELEASE (*)
|         \--- org.springframework:spring-web:4.3.4.RELEASE (*)
+--- org.springframework.boot:spring-boot-starter-data-jpa:1.4.2.RELEASE
|    +--- org.springframework.boot:spring-boot-starter:1.4.2.RELEASE (*)
|    +--- org.springframework.boot:spring-boot-starter-aop:1.4.2.RELEASE
|    |    +--- org.springframework.boot:spring-boot-starter:1.4.2.RELEASE (*)
|    |    +--- org.springframework:spring-aop:4.3.4.RELEASE (*)
|    |    \--- org.aspectj:aspectjweaver:1.8.9
|    +--- org.springframework.boot:spring-boot-starter-jdbc:1.4.2.RELEASE
|    |    +--- org.springframework.boot:spring-boot-starter:1.4.2.RELEASE (*)
|    |    +--- org.apache.tomcat:tomcat-jdbc:8.5.6
|    |    |    \--- org.apache.tomcat:tomcat-juli:8.5.6
|    |    \--- org.springframework:spring-jdbc:4.3.4.RELEASE
|    |         +--- org.springframework:spring-beans:4.3.4.RELEASE (*)
|    |         +--- org.springframework:spring-core:4.3.4.RELEASE (*)
|    |         \--- org.springframework:spring-tx:4.3.4.RELEASE
|    |              +--- org.springframework:spring-beans:4.3.4.RELEASE (*)
|    |              \--- org.springframework:spring-core:4.3.4.RELEASE (*)
|    +--- org.hibernate:hibernate-core:5.0.11.Final
|    |    +--- org.jboss.logging:jboss-logging:3.3.0.Final
|    |    +--- org.hibernate.javax.persistence:hibernate-jpa-2.1-api:1.0.0.Final
|    |    +--- org.javassist:javassist:3.18.1-GA -> 3.20.0-GA
|    |    +--- antlr:antlr:2.7.7
|    |    +--- org.jboss:jandex:2.0.0.Final
|    |    +--- dom4j:dom4j:1.6.1
|    |    |    \--- xml-apis:xml-apis:1.0.b2 -> 1.4.01
|    |    \--- org.hibernate.common:hibernate-commons-annotations:5.0.1.Final
|    |         \--- org.jboss.logging:jboss-logging:3.3.0.Final
|    +--- org.hibernate:hibernate-entitymanager:5.0.11.Final
|    |    +--- org.jboss.logging:jboss-logging:3.3.0.Final
|    |    +--- org.hibernate:hibernate-core:5.0.11.Final (*)
|    |    +--- dom4j:dom4j:1.6.1 (*)
|    |    +--- org.hibernate.common:hibernate-commons-annotations:5.0.1.Final (*)
|    |    +--- org.hibernate.javax.persistence:hibernate-jpa-2.1-api:1.0.0.Final
|    |    \--- org.javassist:javassist:3.18.1-GA -> 3.20.0-GA
|    +--- javax.transaction:javax.transaction-api:1.2
|    +--- org.springframework.data:spring-data-jpa:1.10.5.RELEASE
|    |    +--- org.springframework.data:spring-data-commons:1.12.5.RELEASE
|    |    |    +--- org.springframework:spring-core:4.2.8.RELEASE -> 4.3.4.RELEASE (*)
|    |    |    +--- org.springframework:spring-beans:4.2.8.RELEASE -> 4.3.4.RELEASE (*)
|    |    |    +--- org.slf4j:slf4j-api:1.7.21
|    |    |    \--- org.slf4j:jcl-over-slf4j:1.7.21 (*)
|    |    +--- org.springframework:spring-orm:4.2.8.RELEASE -> 4.3.4.RELEASE
|    |    |    +--- org.springframework:spring-beans:4.3.4.RELEASE (*)
|    |    |    +--- org.springframework:spring-core:4.3.4.RELEASE (*)
|    |    |    +--- org.springframework:spring-jdbc:4.3.4.RELEASE (*)
|    |    |    \--- org.springframework:spring-tx:4.3.4.RELEASE (*)
|    |    +--- org.springframework:spring-context:4.2.8.RELEASE -> 4.3.4.RELEASE (*)
|    |    +--- org.springframework:spring-aop:4.2.8.RELEASE -> 4.3.4.RELEASE (*)
|    |    +--- org.springframework:spring-tx:4.2.8.RELEASE -> 4.3.4.RELEASE (*)
|    |    +--- org.springframework:spring-beans:4.2.8.RELEASE -> 4.3.4.RELEASE (*)
|    |    +--- org.springframework:spring-core:4.2.8.RELEASE -> 4.3.4.RELEASE (*)
|    |    +--- org.slf4j:slf4j-api:1.7.21
|    |    \--- org.slf4j:jcl-over-slf4j:1.7.21 (*)
|    \--- org.springframework:spring-aspects:4.3.4.RELEASE
|         \--- org.aspectj:aspectjweaver:1.8.9
+--- org.springframework.boot:spring-boot-starter-security:1.4.2.RELEASE
|    +--- org.springframework.boot:spring-boot-starter:1.4.2.RELEASE (*)
|    +--- org.springframework:spring-aop:4.3.4.RELEASE (*)
|    +--- org.springframework.security:spring-security-config:4.1.3.RELEASE
|    |    +--- aopalliance:aopalliance:1.0
|    |    +--- org.springframework.security:spring-security-core:4.1.3.RELEASE
|    |    |    +--- aopalliance:aopalliance:1.0
|    |    |    +--- org.springframework:spring-aop:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
|    |    |    +--- org.springframework:spring-beans:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
|    |    |    +--- org.springframework:spring-context:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
|    |    |    +--- org.springframework:spring-core:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
|    |    |    \--- org.springframework:spring-expression:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
|    |    +--- org.springframework:spring-aop:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
|    |    +--- org.springframework:spring-beans:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
|    |    +--- org.springframework:spring-context:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
|    |    \--- org.springframework:spring-core:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
|    \--- org.springframework.security:spring-security-web:4.1.3.RELEASE
|         +--- aopalliance:aopalliance:1.0
|         +--- org.springframework.security:spring-security-core:4.1.3.RELEASE (*)
|         +--- org.springframework:spring-beans:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
|         +--- org.springframework:spring-context:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
|         +--- org.springframework:spring-core:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
|         +--- org.springframework:spring-expression:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
|         \--- org.springframework:spring-web:4.3.2.RELEASE -> 4.3.4.RELEASE (*)
+--- org.springframework.security:spring-security-jwt:1.0.7.RELEASE
|    \--- org.bouncycastle:bcpkix-jdk15on:1.55
|         \--- org.bouncycastle:bcprov-jdk15on:1.55
+--- org.springframework.security.oauth:spring-security-oauth2:2.0.12.RELEASE
|    +--- org.springframework:spring-beans:4.0.9.RELEASE -> 4.3.4.RELEASE (*)
|    +--- org.springframework:spring-core:4.0.9.RELEASE -> 4.3.4.RELEASE (*)
|    +--- org.springframework:spring-context:4.0.9.RELEASE -> 4.3.4.RELEASE (*)
|    +--- org.springframework:spring-webmvc:4.0.9.RELEASE -> 4.3.4.RELEASE (*)
|    +--- org.springframework.security:spring-security-core:3.2.8.RELEASE -> 4.1.3.RELEASE (*)
|    +--- org.springframework.security:spring-security-config:3.2.8.RELEASE -> 4.1.3.RELEASE (*)
|    +--- org.springframework.security:spring-security-web:3.2.8.RELEASE -> 4.1.3.RELEASE (*)
|    \--- commons-codec:commons-codec:1.9 -> 1.10
+--- org.codehaus.groovy:groovy-all:2.4.7
+--- org.liquibase:liquibase-core: -> 3.5.3
|    \--- org.yaml:snakeyaml:1.17
+--- com.h2database:h2:1.4.192
+--- mysql:mysql-connector-java:5.1.40
+--- org.hibernate:hibernate-envers:5.0.11.Final
|    +--- org.jboss.logging:jboss-logging:3.3.0.Final
|    +--- org.hibernate:hibernate-core:5.0.11.Final (*)
|    \--- org.hibernate:hibernate-entitymanager:5.0.11.Final (*)
+--- org.apache.tomcat.embed:tomcat-embed-jasper: -> 8.5.6
|    +--- org.apache.tomcat.embed:tomcat-embed-core:8.5.6
|    +--- org.apache.tomcat.embed:tomcat-embed-el:8.5.6
|    \--- org.eclipse.jdt.core.compiler:ecj:4.5.1
+--- javax.servlet:jstl: -> 1.2
+--- org.webjars:webjars-locator:0.30
|    +--- org.webjars:webjars-locator-core:0.30
|    |    +--- org.slf4j:slf4j-api:1.7.7 -> 1.7.21
|    |    +--- org.apache.commons:commons-lang3:3.1 -> 3.5
|    |    \--- org.apache.commons:commons-compress:1.9
|    +--- com.fasterxml.jackson.core:jackson-databind:2.3.3 -> 2.8.4 (*)
|    \--- org.apache.commons:commons-lang3:3.4 -> 3.5
+--- org.webjars:bootstrap:3.3.7
|    \--- org.webjars:jquery:1.11.1
+--- org.apache.commons:commons-lang3:3.5
+--- commons-codec:commons-codec:1.10
\--- org.owasp.esapi:esapi:2.1.0.1
     +--- commons-configuration:commons-configuration:1.10
     |    +--- commons-lang:commons-lang:2.6
     |    \--- commons-logging:commons-logging:1.1.1 -> 1.2
     +--- commons-beanutils:commons-beanutils-core:1.8.3
     |    \--- commons-logging:commons-logging:1.1.1 -> 1.2
     +--- commons-fileupload:commons-fileupload:1.3.1
     |    \--- commons-io:commons-io:2.2
     +--- commons-collections:commons-collections:3.2.2
     +--- log4j:log4j:1.2.17
     +--- xom:xom:1.2.5
     |    +--- xml-apis:xml-apis:1.3.03 -> 1.4.01
     |    +--- xerces:xercesImpl:2.8.0 -> 2.11.0
     |    |    \--- xml-apis:xml-apis:1.4.01
     |    \--- xalan:xalan:2.7.0
     |         \--- xml-apis:xml-apis:2.0.2 -> 1.4.01
     +--- org.beanshell:bsh-core:2.0b4
     +--- org.owasp.antisamy:antisamy:1.5.3
     |    +--- org.apache.xmlgraphics:batik-css:1.7 -> 1.8
     |    |    +--- org.apache.xmlgraphics:batik-ext:1.8
     |    |    |    \--- xml-apis:xml-apis:1.3.04 -> 1.4.01
     |    |    +--- org.apache.xmlgraphics:batik-util:1.8
     |    |    +--- xml-apis:xml-apis:1.3.04 -> 1.4.01
     |    |    \--- xml-apis:xml-apis-ext:1.3.04
     |    +--- net.sourceforge.nekohtml:nekohtml:1.9.16 -> 1.9.22
     |    |    \--- xerces:xercesImpl:2.11.0 (*)
     |    \--- commons-httpclient:commons-httpclient:3.1
     |         +--- commons-logging:commons-logging:1.0.4 -> 1.2
     |         \--- commons-codec:commons-codec:1.2 -> 1.10
     \--- org.apache.xmlgraphics:batik-css:1.8 (*)

(*) - dependencies omitted (listed previously)

```

##### Prevention
* Active awareness of dependencies
  - Catalog in a database or spreadsheet the library dependencies that are used within each application
  - Obtain the library dependency tree by using the dependency management tool (Maven, NuGet, NPM, ...) to generate a dependency tree
* Continuously review security on dependent libraries using vulnerability databases
  - There a few government sponsored databases that track security vulnerabilities such as [CVE](http://cve.mitre.org/) and [NVD](https://nvd.nist.gov/), which are both provided by the United States Dept of Homeland Security. 
  - Use a tool like [OWASP Dependency-Check](https://www.owasp.org/index.php/OWASP_Dependency_Check#tab=Main) to compare your app dependencies with the National Vulnerability Database
  - NOTE: The vulnerabilitiy databases are not perfectly complete and may not contain all of the latest FOSS libraries. Be aware of what might be missing from the database, just as much as what is in the database.
  - NOTE: Dependency checking tools like the OWASP Dependency-Check are not perfect because the look-up process with the NVD is not perfect due to no "unique key" between the two systems. Do not rely solely on the depedency checker for the complete source of truth. 
* Create a Security QA Team
  - Assign a full-time person or team to reviewing all of the dependent third-party libraries periodically for new vulnerabilities. 
  - Continuously review security publications for new vulnerabilities and exploits and document them in a shared database
  - Aggressively perform security quality assurance reviews on every new or updated application before being deployed to production (Intranet or Internet). 

##### References
* [OWASP Top 10 - A9-Using Components with Known Vulnerabilities](https://www.owasp.org/index.php/Top_10_2013-A9-Using_Components_with_Known_Vulnerabilities)
* [OWASP Dependency-Check](https://www.owasp.org/index.php/OWASP_Dependency_Check#tab=Main)
* [National Vulnerability Database](https://nvd.nist.gov/)

:arrow_up: [Back to Top](#table-of-contents)


#### 10. Unvalidated Redirects and Forwards
##### Overview
When redirecting/forwarding to a new location, do not allow the end-user the option of specifying the URL to redirect/forward to. The concern is that if a request has a parameter of "URL" and the web or mobile app blindly redirects to that URL, then an attacker might be able to inject a malicious URL into the parameter field and redirect unsuspecting users into a trap. 

The OAuth2 authentication pattern is an example that accepts a URL parameter, but validates before redirecting the user. You can see this pattern implemented within the Voyage API. See the [OAuth2](https://github.com/lssinc/voyage-api-java/blob/master/readme_docs/SECURITY.md#authentication-oauth2-default) section within this security document. 
* Mobile app provides a "Login" button that contains a link like `http://secure-api/ouath/authorize?client_id=1&redirect_url=http://mobile-app/oauth?token=`
* When the user clicks on the "Login" button, the OAuth2 process receives the redirect_url (including the other params)
* The OAuth2 process then requires the user to authenticate with their username and password
* Upon successful authentication, the OAuth2 process compares the redirect_url on the request with the URL stored in the database for the given client_id. 
  - If the two URLs match, then the user is redirected to the URL in the database. The URL given on the request is assumed untrusted, so the database URL is what is used to do the actual redirect
  - If the two URLs do not match, then an error is returned to the end user notifying them that something went wrong. 
  
The OAuth2 URL redirect is considered a validated URL redirect approach and is safe.
  
##### Prevention
This particular security vulnerability might not be applicable to an API given that an API is typically a request -> response transaction that wouldn't handle redirecting or forwarding the user to a different address. Even still, it's very important to be aware that a web service should never use request data to build a location for a redirect or forward. 

* Avoid using redirects and forwards with exposed URL parameters
  - Most web applications that serve JSP or ASP or PHP pages are doing internal redirects on nearly every request, which is fine. 
  - Do not store redirect parameters into hidden fields in a web page for use later as a redirect location.
  - Do not allow external / public parameters to be concatinated to the redirect/forward location
* When redirecting/forwarding with user provided values, verify the values before executing a redirect
  - If URLs or URL parameters are to be accepted by the user, then create a policy that restricts what is allowable on the URL or URL parameters. 
  - Encode the URL so that special characters cannot be used malicious to instruct a web browser to do extra tasks beside redirecting or forwarding. 
  - Store an allowable set of redirects in a file or the database so that the choices are limited
  - Assume that an attacker will exploit the URL redirect/forward functionality!

##### References
* [OWASP Top 10 - A10-Unvalidated Redirects and Forwards](https://www.owasp.org/index.php/Top_10_2013-A10-Unvalidated_Redirects_and_Forwards)

:arrow_up: [Back to Top](#table-of-contents)




### User Verification
#### Overview 
User Verification is a feature that will essentially block user access to the API until they have gone through a verification process. The verification process can be triggered at any time for any reason. The primary uses for the User Verification feature is to validate the user's identity after account registration or password recovery process. 

#### SMS Verification
The only method of User Verification currently implemented is a code delivered to the user via SMS text message. When the user account is created, a mobile phone is required in order to receive SMS messages for the completion of the user verification process. 

#### Trigger User Verification
User Verification is an independent component within the Voyage API and can be triggered by the following methods:

1. Direct database update
   - Insert/update the `user` database table column `is_verify_required` to `true`
2. User update via the UserService with the API source code
   - Get the User object to update
   - Set user.isVerifyRequired = true
   - Call the UserService.save(...) method to persist the changes to the database

When an authenticated user makes a call to the API, their user profile is examined to see if they require User Verification. If the User.isVerifyRequired = true, then the API returns an HTTP 403 Forbidden response with JSON body:
```
{
   error:'403_verify_user',
   errorDescription:'User verification is required',
}
```

This response should be anticipated by the consumer of the API and should initiate the proper process to notify the user that a verification is required and be able to capture the verification code from the user to complete the verification process. 

Once the verification code is validated, then the User Verification process will update the User record with value `isVerifyRequired = false`.

#### Workflow

Account Creation Workflow
1. User is presented with a New Account page where they can fill out the required account information
2. User is required to enter their mobile phone number for identity verification
3. User is required to enter in a unique username and secure password (according to password policy rules)
4. User submits the required information and is redirected to the login page (no auto-login allowed)

Login Workflow
1. User is presented with a login page to enter their username and password
2. User submits their username and password
3. User is presented with a "Send verification code to your mobile phone" using the mobile phone entered upon account creation.
4. User clicks "Send code now"... and an SMS code is delivered to their mobile phone within their account
5. User is presented with an "Enter Code" form to validate the SMS code
6. User receives the validate code from their device and enters it into the form
7. User is granted access to the site once the code is validated successfully.

#### Web Services

##### HTTP GET /api/v1/verify/send
A secure web service endpoint that requires the user in need of verification to be authenticated by either a username/password or through security questions. Invoking the /verify/send web services initiates the delivery of a verification code to the mobile phone on the user's account. The code that is delivered will be a 6-digit code that will be stored securely in the database using the same password hashing method (bcrypt). A mobile phone is required at account creation. 

Parameters: none

Possible Results
* HTTP 204 No Content

##### HTTP POST /api/v1/verify
A secure web service endpoint that requires the user in need of verification to be authenticated by either a username/password or through security questions. Invoking the /verify POST web service requires that the body contains the code delivered to the user's mobile phone via SMS. The code provided in the web service will be verified against the code stored in the User's account. 

Post Body: 
```
{
   code: 53432
}
```

Possible Results: 
* HTTP 204 No Content
* HTTP 400 Bad Request

#### Technical Notes

##### AWS SMS Service Integration
By default, Voyage API integrates with Amazon [AWS SNS](http://docs.aws.amazon.com/sns/latest/dg/SMSMessages.html) as the text message provider. In order for the API to faciliate SMS deliveries, an AWS account must be provided within the configuration of the API. See the [Deploy](#deploy) section for instructions on how to apply the AWS credentials. 

##### VerificationServletFilter
The VerificationServletFilter located at `/src/main/groovy/voyage/security/VerificationServletFilter.groovy` intercepts incoming requests by authenticated users and examines the User account to see if the `User.isVerifyRequired` is true. If the isVerifyRequired is true, then the request is immediately stopped and an error message is returned to the consumer notifying them that the user must complete the User Verification process. 

#### References 
* [What is Two Factor Authentication](https://www.securenvoy.com/two-factor-authentication/what-is-2fa.shtm)
* [Testing Multiple Factors Authentication](https://www.owasp.org/index.php/Testing_Multiple_Factors_Authentication_(OWASP-AT-009))


:arrow_up: [Back to Top](#table-of-contents)



## Security Configuration

### CORS Configuration

#### Source Code
The CORS filter logic is located in `/src/main/groovy/voyage/security/CorsServletFilter`. The filter is fairly basic with the only configurable variable being the "Access-Control-Allow-Headers".

#### Access-Control-Allow-Headers
When a browser app makes a request to the API, a CORS OPTION request will be made to the API to get parameters on what the API server allows for the CORS interaction. The stock responses are one of the following:

Anonymous Access Request
```
Headers:
   Access-Control-Allow-Origin: *
   Access-Control-Allow-Headers: Accept, Authorization, Content-Type, Cookie, Origin, User-Agent
```

Authenticated User Request
```
Headers:
   Vary: Origin
   Access-Control-Allow-Origin: [origin for the client from the database]
   Access-Control-Allow-Credentials: true
   Access-Control-Allow-Headers: Accept, Authorization, Content-Type, Cookie, Origin, User-Agent
```

##### Configure Access-Control-Allow-Headers
In /src/main/resources/application.yaml, update the security.cors.access-control-allow-headers section with the appropriate headers that should be supported by the API.
```
security:
  cors:
    access-control-allow-headers: Accept, Authorization, Content-Type, Cookie, Origin, User-Agent
```

:arrow_up: [Back to Top](#table-of-contents)



### Environment Specific Application Properties

#### Overview
The API application externalizes properties into an application.yaml file located within `/src/main/resources/application.yaml`. The properties contained within the application.yaml file are values that had a high likelyhood of being changed depending application use or the environment in which the application was running in. For example: database connection parameters, security settings, logg file locations, etc...

#### YAML
Spring supports two type of property file formats: .properties, .yaml. 

Properties files follow a simple key=value per line format. When there are multiple properties for an entity, then dots are used as a way to categorize properties. For example:
```
security.private-key=ALKJSLKFJS)(*)(*#$#FDSFS
security.public-key=IOUERJLN)*#JNLu80w348r0u2
```

Properties file are fine, but limiting in its ability to group properties. 

YAML is an acronym for Yet Another Markup Language or YAML Ain't Markup Language. More information about how YAML formats files can be read online at [yaml.org](http://www.yaml.org). In short, YAML extends properties file functionality by adding in nested properties through its indendtation structure. YAML files are generally easier to read and accomodate structures like lists much better than the properties file format. 

YAML is the preferred property file format of choice for this API. 

#### Embedded application.yaml
An embedded application.yaml file has been created and stored within `/src/main/resources/application.yaml`. The properties that are included in this file are meant only for local development or testing lab environments. These properties will get the application up and running quickly on default settings. Also, this application.yaml file is embedded within the application distribution .WAR file and will be used as the default settings at application runtime. It is important to override these default properties when deploying the API to "upper environments" such as Quality Assurance (QA), User Acceptance Testing (UAT), and especially Production (PROD).

#### Overriding default properties
[Spring Framework](STANDARDS-SPRING.md) provides for various methods to override properties that are defined within the `/src/main/resources/application.yaml` application properties file. Read more about [Spring's Externalized Configuration](https://docs.spring.io/spring-boot/docs/current/reference/html/boot-features-external-config.html). 

Two common methods used by the Voyage development team for overriding Spring properties are as follows. 

1. Apache Tomcat + `application.yaml` external to the .WAR file/folder stored securely on the server filesystem
   - Copy the `application.yaml` from `/src/main/resources/application.yaml` to a secure location within a server that will host Voyage API like `/etc/voyage-api/application.yaml` or `D:\voyage-api-config\application.yaml` 
   - Change the values within the server `application.yaml` file to suit the needs of the particular server environment
   - Secure the file to be readable by the Apache Tomcat process and system administrators
   - Create a .WAR specific Apache Tomcat context in `/conf/Catalina/localhost/[war-file-name-here].xml`
   - In the [WAR-specific context file](https://tomcat.apache.org/tomcat-8.0-doc/config/context.html), add an [<Environment>](https://tomcat.apache.org/tomcat-5.5-doc/config/context.html#Environment_Entries) property to override the location where Spring will look for the `application.yaml` file. 
   - The [spring.config.location](http://docs.spring.io/spring-boot/docs/current/reference/html/boot-features-external-config.html#boot-features-external-config-application-property-files) property adds a filesystem location that Spring will inspect first for a file named `application.yaml`. 
     ```
     <?xml version='1.0' encoding='utf-8'?>
     <Context>
     
     	<Environment 
     		name="spring.config.location" 
     		value="d:/Users/tmichalski/voyage-config/" 
     		type="java.lang.String" 
     		override="false"/>
     
     	<ResourceLink
     		name="jdbc/voyage"
     		global="jdbc/voyage"
     		type="javax.sql.DataSource" />
     	
     </Context>
     ```
   - Read a more detailed description of how to implement this tactic at [DEPLOY: App Build & Test](https://github.com/lssinc/voyage-api-java/blob/master/readme_docs/DEPLOY.md#4-apache-tomcat-setup--override-parameters-by-environment). 
2. Apache Tomcat Environment Properties
   - Override individual Spring config or `application.yaml` properties within the WAR-specific Apache Tomcat context file 
   - Update the Apache Tomcat [WAR-specific context file](https://tomcat.apache.org/tomcat-8.0-doc/config/context.html) in `/conf/Catalina/localhost/[war-file-name-here].xml` 
   - Add an [<Environment>](https://tomcat.apache.org/tomcat-5.5-doc/config/context.html#Environment_Entries) property for each Spring or application property to override 
     ```
     <Environment name="spring.mail.host" value="localhost" type="java.lang.String" override="false"/>
     <Environment name="spring.mail.port" value="3025" type="java.lang.Integer" override="false"/>
     ```
   - NOTE: When defining properties outside of a .yaml file, default to using normal .properties file notation. See the YAML section above for more details on the .properties file format. 
   - Read more about this method on the [Apache Tomcat docs website](https://tomcat.apache.org/tomcat-7.0-doc/jndi-resources-howto.html#context.xml_configuration)


:arrow_up: [Back to Top](#table-of-contents)


### Encryption Public/Private Key Configuration

#### Generate Private/Public keys for asymmetric encryption
```
keytool -genkeypair -alias asymmetric -keyalg RSA -keysize 2048 \
-dname "CN=Web Server,OU=Unit,O=Organization,L=City,S=State,C=US" \
-keypass changeme -keystore keystore.jks -storepass changeme
```

* Revise the keytool statement above with your own personalized parameters
* For development, copy the keystore.jks to your /src/main/resources folder so that it is available on the classpath when running the app locally and for tests. 

> NOTE: These are the default settings. Be sure to document any changes in the "keypass" or "storepass" in a _secure location_
(ie not .MD files in source control) so that you don't lose these!  

#### Configure the keystore parameters within the application.yaml
Update the /src/main/resources/application.yaml file or override the properties inside application.yaml using the [externalized configuration options](DEPLOY.md#override-applicationyaml-properties). 

```
security:
  # FOR PRODUCTION: The following MUST be overridden to ensure secrecy of the passwords for the keystore and private
  # See where you can override at https://docs.spring.io/spring-boot/docs/current/reference/html/boot-features-external-config.html
  key-store:
    filename: keystore.jks
    password: changeme

  crypto:
    private-key-name: asymmetric
    private-key-password: changeme
```

Replace or override the values in the application.yaml to match the configuration settings when the private/public key was generated.


:arrow_up: [Back to Top](#table-of-contents)



### JWT Public/Private Key Configuration
Following the example found in https://beku8.wordpress.com/2015/03/31/configuring-spring-oauth2-with-jwt-asymmetric-rsa-keypair/

#### Generate Private/Public keys for OAUTH2 JWT
Reuse the keystore generated with above for asymmetric encryption.

```
keytool -genkeypair -alias jwt -keyalg RSA -keysize 2048 \
-dname "CN=Web Server,OU=Unit,O=Organization,L=City,S=State,C=US" \
-keypass changeme -keystore keystore.jks -storepass changeme
```

* Revise the keytool statement above with your own personalized parameters
* For development, copy the keystore.jks to your /src/main/resources folder (if it's not there already) so that it is available on the classpath when running the app locally and for tests. 

> NOTE: These are the default settings. Be sure to document any changes in the "keypass" or "storepass" in a _secure location_
(ie not .MD files in source control) so that you don't lose these!  

#### Export the Public Key
```
keytool -export -keystore keystore.jks -alias jwt -file jwt.cer
```
* Enter the password used to generate the keystore (ie changeme)
* The key will be exported to jwt.cer
 
```
openssl x509 -inform der -in jwt.cer -pubkey -noout
```

The output will be the public key, which should look something like:

```
-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA4REj5EYufU5OUnv9nij+
j9irwALL3BwX9XxB7oDx3uj93P5h8rzTTdG/suaG3aBqRr5rqXpmTgwG1nf6FBfR
8kiPp9R196cAT9g4OInsdNbux7oy5akUVsRo9pagEL0JB7eGbASi0z5A38QkpbjB
MhIN0W9zwghsGNbf7N6wTVQN1NFHDW9zMdWUS9VBPeEGUZAMkKElGltHVhCdJGBf
OdriLIO2KdimjO5q9Q9+qG2B96DFGNYvmuDlDLM11Q2fsre305CV1HN0vQulLhlr
MJo9QdZt1g2d1VN5uIKid5dxWTAuUvJhgla6yCaTfYeV1OGq5C3DFV7tKDGNAIXL
TQIDAQAB
-----END PUBLIC KEY-----
```

Copy the public key into the /src/main/resources/application.yaml file along with the JWT keystore
and private key information, like:

```
security:

  # FOR PRODUCTION: The following MUST be overridden to ensure secrecy of the passwords for the keystore and private
  # See where you can override at https://docs.spring.io/spring-boot/docs/current/reference/html/boot-features-external-config.html
  key-store:
    filename: keystore.jks
    password: changeme

  # FOR PRODUCTION: The following MUST be overridden to ensure secrecy of the passwords for the keystore and private
  # See where you can override at https://docs.spring.io/spring-boot/docs/current/reference/html/boot-features-external-config.html
  jwt:
    private-key-name: jwt
    private-key-password: changeme    
    
  oauth2:
    resource:
      id: voyage
      jwt:
        key-value: |
          -----BEGIN PUBLIC KEY-----
          MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA4REj5EYufU5OUnv9nij+
          j9irwALL3BwX9XxB7oDx3uj93P5h8rzTTdG/suaG3aBqRr5rqXpmTgwG1nf6FBfR
          8kiPp9R196cAT9g4OInsdNbux7oy5akUVsRo9pagEL0JB7eGbASi0z5A38QkpbjB
          MhIN0W9zwghsGNbf7N6wTVQN1NFHDW9zMdWUS9VBPeEGUZAMkKElGltHVhCdJGBf
          OdriLIO2KdimjO5q9Q9+qG2B96DFGNYvmuDlDLM11Q2fsre305CV1HN0vQulLhlr
          MJo9QdZt1g2d1VN5uIKid5dxWTAuUvJhgla6yCaTfYeV1OGq5C3DFV7tKDGNAIXL
          TQIDAQAB
          -----END PUBLIC KEY-----   
```

### Public Resources
By default Spring Security protects all resources within the API. In order to expose certain resources as public with no authentication or authorization required, then these resources must be called out explicitly within an application.yaml file. Following are two ways that to grant public access to resources. 

#### Ignored
The "security.ignored" parameter accepts a list of url paths in the [AntPath syntax](http://ant.apache.org/manual/dirtasks.html#patterns). Whenever a request comes in, Spring Security will check to see if the request URL matches any of the items on the ignored list. If the incoming request URL does match the ignored list, then Spring Security will completely ignore the URL and will not apply any authentication or authorization to the request. 

Ignoring URL paths should only be used for resources that do not require a Spring SecurityContext to be loaded, especially if the resource needs an authorized User to perform it's function. Ignored URLs are typically used for static resources like CSS, images, JS, and other static documents.  

application.yaml
```
security:
  ignored: /resources/**, /webjars/**, /docs/**
```

#### Permit All
The "security.permitAll" parameter accepts a list of url paths in the [AntPath syntax](http://ant.apache.org/manual/dirtasks.html#patterns). Much like the "security.ignored" parameter, when a request comes in, Spring Security will check to see if the request URL matches any of the patterns in the permitAll list. If the incoming request URL does match a permitAll pattern, then Spring Security will grant the user with full access to the resource and will create a Spring SecurityContext for the request. If the request contains valid authentication credentials, then the authenticated User will be added to the Spring SecurityContext. If the User cannot be determined, then an Anonymous user will be placed into the Spring SecurityContext. 

The primary difference between "ignored" and "permitAll" is that "ignored" URLs do not have a SecurityContext and "permitAll" URLs do have a SecurityContext. Use the "permitAll" parameter when a resource needs to be made public and requires the use of the Spring SecurityContext. 

application.yaml
```
security:
  permitAll: /login, /api/hello
```

### User Verification Configuration

#### Excluded Resources
The user verification servlet filter can be configured to exclude API resources so that an authenticated user is still able to access a litmited set of resources. To modify the the exclusion list, update the comma delimited list of ant-path values within the `application.yaml` file in section `security.user-verification.exclude-resources`.

Example:
```
security:
  user-verification:
    exclude-resources: /oauth/**, /resources/**, /webjars/**, /docs/**, /login, /api/**/verify, /api/**/verify/send, /WEB-INF/jsp/**.jsp, /api/**/profile
```

By default, authentication resources are excluded by the verification filter so that a user is able to login even tho their account requires verification. These resources include:
* /login
* /oauth/**
* /WEB-INF/jsp/**.jsp (this might need to be locked down more specifically if JSPs are used for more than just authentication)

Of course, the verification endpoints are excluded so that end users are able to use the /verify services to verify their account. These resources include:
* /api/**/verify
* /api/**/verify/send

Other endpoints that are excluded by default are CSS, JS, and image resources, as well as access to the authenticated user's profile. These resources include:
* /resources/** (embedded css, js, and imagine)
* /webjars/** (css, js libraries)
* /docs/** (access to the apiDoc, which is public)
* /api/**/profile

#### Verify Code Expiration
A "verify code" value is an API generated 6 digit code that is sent to a user's mobile device. Once the user receives the verify code, they are instructed to enter the code within the app. The verify code is only valid for a set period of time before it will expire. To configure the number of minutes before the verify code expired, update the `security.user-verification.verify-code-expire-minutes` within the `application.yaml` file. By default, the verify code will expire within 30 minutes.
```
security:
  user-verification:
    verify-code-expire-minutes: 30
```

:arrow_up: [Back to Top](#table-of-contents)



## Audit Logging
All enterprise level applications require audit logging of activities and data changes within each of their applications. Some business verticals legally require auditing, such as US banking regulations and US healthcare HIPAA law. While audit logging is required in many industries, every application should include audit logging as a rule because of the many benefits, such as: 
* Monitoring user activity & volume with specificity
* Track data changes to the database
* Analyze activity for suspicious behavior
* Be alerted when errors occur (before your user notifies you)
* Retrospectively analyze user's who report bugs, errors, or is having trouble with the app
* Measure Request/Response duration and track slow processing

### Action Logs
Action logs track individual actions that were requested by users. When the API receives an API request, the details of the request are inserted into the database table `action_log`. Before the APi returns back the response to the consumer, details about the response are updated within the action log record to complete the transaction details. 

Action logs are important because they track all requests made of the system from anonymous and authenticated users. With this informaiton, many security features can be built to detect malicious behaviors (IP getting too many 401 Unauthorized requests in a small period of time) that can block activity for a specific IP address or user. Also, action logs that track duration of the complete request/response transaction allow for tracking slow requests for continual performance enhancements. In addition to providing significant awareness to the administrators of the app, action logging is a required component for knowing if a user made a request to a resource, was granted access to fulfill the request, and what was the user given back as a response. In a security breach situation, it's important to be able to quickly identify the level of exposure and be able to perform forensic operations to identify how the breach occurred. 

The following information is stored in the database:

* IP Address - the IP address of the originator of the request (aka user). If the API is being hosted behind a load balancer, then the IP Address will be retrieved from the request header `X-Forwarded-For`. 
* Protocol - the request protocol used by the user. HTTP or HTTPS in most cases. 
* URL - the URL of the HTTP request
* HTTP Method - the method used for the request: GET, POST, PUT, PATCH, DELETE
* HTTP Status - the http response status of the request. Example: 200, 401, 404
* Principal - the authenticated username associated with the request
* Client ID - the ID of the client that initiated the request
* User ID - the ID of the authenticated user
* Request Headers - a listing of headers that were sent by the consumer
* Request Body - the POST/PUT/PATCH content provided within the request
* Response Body - the content response from the API
* Created Date - the date and time the action log was created
* Last Modified Date - the date and time the action log was updated, which would be with an update with the response content

:arrow_up: [Back to Top](#table-of-contents)


### Change Logs
Change logs track the state of a database record over time. Every insert, update, delete that occurs within the database will have a record written to the change log that tracks the data modified, the authenticated user that made the change, and the time that the change occurred. 

The API utilizes [Hibernate Envers framework](https://docs.jboss.org/envers/docs/) to handle data change tracking. Envers data change tracking follows a complete database state model where every database change increments the "version" of the database state. Envers provides tools that allow for an app or database administrator to query the database change logs to see what the data looked like at any state version in the past. For example, if the current database state version was at 850, an admin could ask the Envers framework what the database state looked like at version 450. In order for Envers to provide this detailed level of change tracking and state versioning, each database table that requires auditing must have a mirror table with the same name + "\_AUD" extension. The mirror table also requires two added fields to track the revision (aka version) of the data: REV, REVTYPE. REV holds the revision number of the data change, and REVTYPE describes the type of change (0-INSERT, 1-UPDATE, 2-DELETE). 

```
user
- id
- name
```

```
user_aud
- id 
- name
- REV
- REVTYPE
```

Add the `@Audited` annotation above the class signature of all application domain objects that require change logging in the database. The Hibernate Envers framework scans for domain objects with the `@Auditied` annotation and will "wire it up" so that when Hibernate persists changes to the database, the Envers framework will be triggered to submit the changes into the `\_AUD` table. 

```
@Entity
@Audited
class User extends AuditableEntity {
    @NotNull
    String firstName

    @NotNull
    String lastName
}
```

All domain objects that implement @Auditing annotation must configure the \_AUD mirror tables within [Liquibase migration scripts](./DEVELOPMENT-RECIPES.md#add-database-structure-changes) in `/src/main/resources/db.changelog/`. 

:arrow_up: [Back to Top](#table-of-contents)


### User & Date Stamps
Each record within the API ought to have a User and Date stamp for when the record was created and who the last user was to modify the record. While these user & date stamps do not provide a complete picture, they do provide some helpful information, in particular with the last modified user and date. Combined with Change Log auditiing, these fields are very helpful to see the last modified user and date over time. 

To enable User & Date timestamps within the API domain objects, simply extend AuditableEntity like:
```
@Entity
@Audited
class User extends AuditableEntity {
    @NotNull
    String firstName

    @NotNull
    String lastName
}
```

AuditableEntity simply contains the fields that are required on every domain object within the API
```
@MappedSuperclass
@EntityListeners(AuditingEntityListener)
@EqualsAndHashCode
class AuditableEntity {
    @Id
    @GeneratedValue(strategy=GenerationType.AUTO)
    Long id

    @CreatedBy
    @Audited
    @JsonIgnore
    String createdBy

    @CreatedDate
    @Audited
    @JsonIgnore
    Date createdDate

    @LastModifiedBy
    @Audited
    @JsonIgnore
    String lastModifiedBy

    @LastModifiedDate
    @Audited
    @JsonIgnore
    Date lastModifiedDate

    @NotNull
    @Audited
    @JsonIgnore
    Boolean isDeleted = Boolean.FALSE
}
```

Extending AuditableEntity for a domain object requires adding the fields defined in AuditableEntity as columns on the domain object table. For example, with the user object shown in the code sample above, the `user` table will require columns `created_by`, `created_date`, `last_modified_by`, and `last_modified_date`. 

All domain objects that extend AuditableEntity are required to have the column `is_deleted` added to the table (along with the other AuditableEntity fields). These columns should [added to the Liquibase migration scripts](./DEVELOPMENT-RECIPES.md#add-database-structure-changes) in `/src/main/resources/db.changelog/`. 

:arrow_up: [Back to Top](#table-of-contents)


### Logical Deletes
A logical delete (sometimes referred to as a soft delete) is when a record in the database is not physically deleted from the database, but instead "marked" as deleted. Records that are "marked" as deleted will be filtered out of all results from the application, effectively ignoring the "marked" record. Logical deletes might not be immediately seens as an auditing feature, but it is vitally important that every piece of data entered into the database is preserved for historical and audit reporting reasons. Forensics are difficult to perform on deleted data since the data is difficult to recover (if even possible).

A nice side benefit of logical deletes is that if data is accidentally marked as deleted by the application, then it's a simple record update by a DBA to restore the data for the user. 

To enable logical deletes for a domain object, simply extend the `AuditableEntity` class to include the base auditable fields, which includes a field titled `isDeleted`.

```
@MappedSuperclass
@EntityListeners(AuditingEntityListener)
@EqualsAndHashCode
class AuditableEntity {
    @Id
    @GeneratedValue(strategy=GenerationType.AUTO)
    Long id

    @CreatedBy
    @Audited
    @JsonIgnore
    String createdBy

    @CreatedDate
    @Audited
    @JsonIgnore
    Date createdDate

    @LastModifiedBy
    @Audited
    @JsonIgnore
    String lastModifiedBy

    @LastModifiedDate
    @Audited
    @JsonIgnore
    Date lastModifiedDate

    @NotNull
    @Audited
    @JsonIgnore
    Boolean isDeleted = Boolean.FALSE
}
```

All domain objects that extend AuditableEntity are required to have the column `is_deleted` added to the table (along with the other AuditableEntity fields). These columns should [added to the Liquibase migration scripts](./DEVELOPMENT-RECIPES.md#add-database-structure-changes) in `/src/main/resources/db.changelog/`. 

:arrow_up: [Back to Top](#table-of-contents)
