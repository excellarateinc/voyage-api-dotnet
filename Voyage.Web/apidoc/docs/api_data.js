define({ "api": [
  {
    "type": "post",
    "url": "/v1/account/register",
    "title": "Register a new account",
    "version": "0.1.0",
    "name": "CreateAccount",
    "group": "Account",
    "permission": [
      {
        "name": "none"
      }
    ],
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "email",
            "description": "<p>User's email</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "password",
            "description": "<p>User's password</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "confirmPassword",
            "description": "<p>User's password (x2)</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "firstName",
            "description": "<p>First name</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "lastName",
            "description": "<p>Last name</p>"
          }
        ]
      }
    },
    "success": {
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 204 NO CONTENT",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/AccountController.cs",
    "groupTitle": "Account",
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "BadRequest",
            "description": "<p>The input did not pass the model validation.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400: Bad Request\n[\n    {\n        \"code\": \"user.lastName.required\",\n        \"description\": \"User lastName is a required field\"\n    },\n    {\n        \"code\": \"user.email.invalidFormat\",\n        \"description\": \"User email format is invalid. Required format is text@example.com\"\n    }\n]",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "post",
    "url": "/v1/login",
    "title": "Login a user",
    "version": "0.1.0",
    "name": "Login",
    "group": "Account",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "grant_type",
            "defaultValue": "password",
            "description": "<p>Authentication method</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "password",
            "description": "<p>User's password</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "username",
            "description": "<p>User's login name</p>"
          }
        ]
      }
    },
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Content-Type",
            "defaultValue": "application/x-www-form-urlencoded",
            "description": "<p>Expected content type of the params</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n  \"Content-Type\": \"application/x-www-form-urlencoded\"\n}",
          "type": "json"
        }
      ]
    },
    "permission": [
      {
        "name": "none"
      }
    ],
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "access_token",
            "description": "<p>Authentication token for secure web service requests</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "token_type",
            "description": "<p>Type of the authentication token</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": false,
            "field": "expires_in",
            "description": "<p>Time to live for the token</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "userName",
            "description": "<p>Name of the authenticated user</p>"
          },
          {
            "group": "Success 200",
            "type": "Date",
            "optional": false,
            "field": ".issued",
            "description": "<p>Date the token was issued</p>"
          },
          {
            "group": "Success 200",
            "type": "Date",
            "optional": false,
            "field": ".expires",
            "description": "<p>Date the token expires</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n     \"access_token\": \"5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\",\n     \"token_type\": \"bearer\",\n     \"expires_in\": 1209599,\n     \"userName\": \"admin@admin.com\",\n     \".issued\": \"Thu, 03 Nov 2016 14:38:29 GMT\",\n     \".expires\": \"Thu, 17 Nov 2016 14:38:29 GMT\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "../AuthProviders/LoginOrchestrator.cs",
    "groupTitle": "Account",
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "BadRequest",
            "description": "<p>The input did not pass the model validation.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400: Bad Request\n[\n    {\n        \"code\": \"user.lastName.required\",\n        \"description\": \"User lastName is a required field\"\n    },\n    {\n        \"code\": \"user.email.invalidFormat\",\n        \"description\": \"User email format is invalid. Required format is text@example.com\"\n    }\n]",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "post",
    "url": "/v1/roles/:roleId/claims",
    "title": "Create a role claim",
    "version": "0.1.0",
    "name": "AddRoleClaim",
    "group": "Role",
    "permission": [
      {
        "name": "app.permission->create.claim"
      }
    ],
    "header": {
      "fields": {
        "Response Headers": [
          {
            "group": "Response Headers",
            "type": "String",
            "optional": false,
            "field": "location",
            "description": "<p>Location of the newly created resource</p>"
          }
        ],
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Location-Example",
          "content": "{\n    \"Location\": \"http://localhost:52431/api/v1/roles/6d1d5caf-d29d-4bf2-a581-0a35081a1240/claims/219\"\n}",
          "type": "json"
        },
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "string",
            "optional": false,
            "field": "roleId",
            "description": "<p>Role ID</p>"
          },
          {
            "group": "Parameter",
            "type": "Object",
            "optional": false,
            "field": "claim",
            "description": "<p>Claim</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "claim.claimType",
            "description": "<p>Type of the claim</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "claim.claimValue",
            "description": "<p>Value of the claim</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object",
            "optional": false,
            "field": "claim",
            "description": "<p>Claim</p>"
          },
          {
            "group": "Success 200",
            "type": "Integer",
            "optional": false,
            "field": "claim.id",
            "description": "<p>Claim ID</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "claim.claimType",
            "description": "<p>Type</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "claim.claimValue",
            "description": "<p>Value</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 201 Created\n{\n    \"claimType\": \"app.permission\",\n    \"claimValue\": \"list.newClaim\",\n    \"id\": 219\n}",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/RoleController.cs",
    "groupTitle": "Role",
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "BadRequest",
            "description": "<p>The input did not pass the model validation.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400: Bad Request\n[\n    {\n        \"code\": \"user.lastName.required\",\n        \"description\": \"User lastName is a required field\"\n    },\n    {\n        \"code\": \"user.email.invalidFormat\",\n        \"description\": \"User email format is invalid. Required format is text@example.com\"\n    }\n]",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "post",
    "url": "/v1/roles",
    "title": "Create a role",
    "version": "0.1.0",
    "name": "CreateRole",
    "group": "Role",
    "permission": [
      {
        "name": "app.permission->create.role"
      }
    ],
    "header": {
      "fields": {
        "Response Headers": [
          {
            "group": "Response Headers",
            "type": "String",
            "optional": false,
            "field": "location",
            "description": "<p>Location of the newly created resource</p>"
          }
        ],
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Location-Example",
          "content": "{\n    \"Location\": \"http://localhost:52431/api/v1/roles/34d87057-fafa-4e5d-822b-cddb1700b507\"\n}",
          "type": "json"
        },
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "Object",
            "optional": false,
            "field": "role",
            "description": "<p>Role</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "role.id",
            "description": "<p>Role ID</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "role.name",
            "description": "<p>Name of the role</p>"
          }
        ]
      }
    },
    "success": {
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 201 CREATED\n{\n    \"id\": \"34d87057-fafa-4e5d-822b-cddb1700b507\",\n    \"name\": \"New Role 2\",\n    \"claims\": []\n}",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/RoleController.cs",
    "groupTitle": "Role",
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "BadRequest",
            "description": "<p>The input did not pass the model validation.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400: Bad Request\n[\n    {\n        \"code\": \"user.lastName.required\",\n        \"description\": \"User lastName is a required field\"\n    },\n    {\n        \"code\": \"user.email.invalidFormat\",\n        \"description\": \"User email format is invalid. Required format is text@example.com\"\n    }\n]",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "get",
    "url": "/v1/roles/:roleId/claims/:claimId",
    "title": "Get a claim",
    "version": "0.1.0",
    "name": "GetClaimById",
    "group": "Role",
    "permission": [
      {
        "name": "app.permission->view.claim"
      }
    ],
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roleId",
            "description": "<p>Role ID</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "claimId",
            "description": "<p>Claim ID</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object",
            "optional": false,
            "field": "claim",
            "description": "<p>Claim</p>"
          },
          {
            "group": "Success 200",
            "type": "Integer",
            "optional": false,
            "field": "claim.id",
            "description": "<p>Claim ID</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "claim.claimType",
            "description": "<p>Type</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "claim.claimValue",
            "description": "<p>Value</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n    \"claimType\": \"app.permission\",\n    \"claimValue\": \"list.newClaim\",\n    \"id\": 219\n}",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/RoleController.cs",
    "groupTitle": "Role",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>The requested resource was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 404: Not Found\n{\n    \"code\": \"error.404_not_found\",\n    \"description\": \"Could not locate entity with ID 11bef1e7-3ba3-4669-861e-54e91fd8db79\"\n}",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "get",
    "url": "/v1/roles/:roleId",
    "title": "Get a role",
    "version": "0.1.0",
    "name": "GetRoleById",
    "group": "Role",
    "permission": [
      {
        "name": "app.permission->view.role"
      }
    ],
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roleId",
            "description": "<p>Role ID</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object",
            "optional": false,
            "field": "role",
            "description": ""
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "role.id",
            "description": "<p>Role ID</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "role.name",
            "description": "<p>Name of the role</p>"
          },
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "role.claims",
            "description": "<p>Claims associated to the role</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "role.claims.claimType",
            "description": "<p>Type of the claim</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "role.claims.claimValue",
            "description": "<p>Value of the claim</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n   {\n        \"id\": \"76d216ab-cb48-4c5f-a4ba-1e9c3bae1fe6\",\n        \"name\": \"New Role 1\",\n        \"claims\": [\n            {\n                claimType: \"app.permission\",\n                claimValue: \"view.role\"\n            }\n        ]\n    }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/RoleController.cs",
    "groupTitle": "Role",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>The requested resource was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 404: Not Found\n{\n    \"code\": \"error.404_not_found\",\n    \"description\": \"Could not locate entity with ID 11bef1e7-3ba3-4669-861e-54e91fd8db79\"\n}",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "get",
    "url": "/v1/roles/:roleId/claims",
    "title": "Get role claims",
    "version": "0.1.0",
    "name": "GetRoleClaims",
    "group": "Role",
    "permission": [
      {
        "name": "app.permission->list.role-claims"
      }
    ],
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roleId",
            "description": "<p>Role ID</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "claims",
            "description": "<p>Claims associated to the role</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "claims.claimType",
            "description": "<p>Type of the claim</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "claims.claimValue",
            "description": "<p>Value of the claim</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n    {\n        \"claimType\": \"app.permission\",\n        \"claimValue\": \"list.newClaim\",\n        \"id\": 17\n    }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/RoleController.cs",
    "groupTitle": "Role",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "get",
    "url": "/v1/roles",
    "title": "Get all roles",
    "version": "0.1.0",
    "name": "GetRoles",
    "group": "Role",
    "permission": [
      {
        "name": "app.permission->list.roles"
      }
    ],
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "roles",
            "description": "<p>List of roles</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "roles.id",
            "description": "<p>Role ID</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "roles.name",
            "description": "<p>Name of the role</p>"
          },
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "roles.claims",
            "description": "<p>Claims associated to the role</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "roles.claims.claimType",
            "description": "<p>Type of the claim</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "roles.claims.claimValue",
            "description": "<p>Value of the claim</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n              {\n                  \"id\": \"c1a44325-5ece-4ff4-8a41-6b5729e8e65d\",\n                  \"name\": \"Administrator\",\n                  \"claims\": [\n                    {\n                      \"claimType\": \"app.permission\",\n                      \"claimValue\": \"assign.role\"\n                    },\n                    {\n                      \"claimType\": \"app.permission\",\n                      \"claimValue\": \"create.claim\"\n                    }\n                  ]\n                }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/RoleController.cs",
    "groupTitle": "Role",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "delete",
    "url": "/v1/roles/:roleId",
    "title": "Delete a role",
    "version": "0.1.0",
    "name": "RemoveRole",
    "group": "Role",
    "permission": [
      {
        "name": "app.permission->delete.role"
      }
    ],
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roleId",
            "description": "<p>Role ID</p>"
          }
        ]
      }
    },
    "success": {
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/RoleController.cs",
    "groupTitle": "Role",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>The requested resource was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 404: Not Found\n{\n    \"code\": \"error.404_not_found\",\n    \"description\": \"Could not locate entity with ID 11bef1e7-3ba3-4669-861e-54e91fd8db79\"\n}",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "delete",
    "url": "/v1/roles/:roleId/claims/:claimId",
    "title": "Remove a role claim",
    "version": "0.1.0",
    "name": "RemoveRoleClaim",
    "group": "Role",
    "permission": [
      {
        "name": "app.permission->delete.role-claim"
      }
    ],
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roleId",
            "description": "<p>Role ID</p>"
          },
          {
            "group": "Parameter",
            "type": "Integer",
            "optional": false,
            "field": "claimId",
            "description": "<p>Claim ID</p>"
          }
        ]
      }
    },
    "success": {
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/RoleController.cs",
    "groupTitle": "Role",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "get",
    "url": "/v1/statuses",
    "title": "Get application info",
    "version": "0.1.0",
    "name": "GetStatuses",
    "group": "Status",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "version",
            "description": "<p>Version Number</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n    {\n        \"buildNumber\": \"some_number\"\n    }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/ApplicationInfoController.cs",
    "groupTitle": "Status"
  },
  {
    "type": "post",
    "url": "/v1/users/:userId/roles",
    "title": "Assign role to user",
    "version": "0.1.0",
    "name": "AssignRole",
    "group": "User",
    "permission": [
      {
        "name": "app.permission->assign.role"
      }
    ],
    "header": {
      "fields": {
        "Response Headers": [
          {
            "group": "Response Headers",
            "type": "String",
            "optional": false,
            "field": "location",
            "description": "<p>Location of the newly created resource</p>"
          }
        ],
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Location-Example",
          "content": "{\n    \"Location\": \"http://localhost:52431/api/v1/users/ceee08c8-9b3b-4fde-a234-86cc04993309/roles/76d216ab-cb48-4c5f-a4ba-1e9c3bae1fe6\"\n}",
          "type": "json"
        },
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "userId",
            "description": "<p>User ID</p>"
          },
          {
            "group": "Parameter",
            "type": "Object",
            "optional": false,
            "field": "role",
            "description": "<p>Role for the association</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "role.id",
            "description": "<p>Role ID</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "role.name",
            "description": "<p>Name of the role</p>"
          }
        ]
      }
    },
    "success": {
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 201 CREATED\n{\n    \"id\": \"76d216ab-cb48-4c5f-a4ba-1e9c3bae1fe6\",\n    \"name\": \"New Role 1\",\n    \"claims\": []\n}",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/UserController.cs",
    "groupTitle": "User",
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "BadRequest",
            "description": "<p>The input did not pass the model validation.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400: Bad Request\n[\n    {\n        \"code\": \"user.lastName.required\",\n        \"description\": \"User lastName is a required field\"\n    },\n    {\n        \"code\": \"user.email.invalidFormat\",\n        \"description\": \"User email format is invalid. Required format is text@example.com\"\n    }\n]",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "get",
    "url": "/v1/users/:userId/claims",
    "title": "Get user claims",
    "version": "0.1.0",
    "name": "Claims",
    "group": "User",
    "permission": [
      {
        "name": "app.permission->list.user-claims"
      }
    ],
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "userId",
            "description": "<p>Id of user</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "claims",
            "description": "<p>List of user claims</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "claims.claimType",
            "description": "<p>Type of the claim</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "claims.claimValue",
            "description": "<p>Value of the claim</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n    {\n        \"claimType\": \"app.permission\",\n        \"claimValue\": \"login\"\n    },\n    {\n        \"claimType\": \"app.permission\",\n        \"claimValue\": \"list.user-claims\"\n    }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/UserController.cs",
    "groupTitle": "User",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "post",
    "url": "/v1/users",
    "title": "Create user",
    "version": "0.1.0",
    "name": "CreateUser",
    "group": "User",
    "permission": [
      {
        "name": "app.permission->create.user"
      }
    ],
    "header": {
      "fields": {
        "Response Headers": [
          {
            "group": "Response Headers",
            "type": "String",
            "optional": false,
            "field": "location",
            "description": "<p>Location of the newly created resource</p>"
          }
        ],
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Location-Example",
          "content": "{\n    \"Location\": \"http://localhost:52431/api/v1/users/b78ae241-1fa6-498c-aa48-9742245d0d2f\"\n}",
          "type": "json"
        },
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/UserController.cs",
    "groupTitle": "User",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "Object",
            "optional": false,
            "field": "user",
            "description": "<p>User</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "user.userName",
            "description": "<p>Username of the user</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "user.email",
            "description": "<p>Email</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "user.firstName",
            "description": "<p>First name</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "user.lastName",
            "description": "<p>Last name</p>"
          },
          {
            "group": "Parameter",
            "type": "Object[]",
            "optional": false,
            "field": "user.phones",
            "description": "<p>User phone numbers</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "user.phones.phoneNumber",
            "description": "<p>Phone number</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "user.phones.phoneType",
            "description": "<p>Phone type</p>"
          }
        ]
      }
    },
    "examples": [
      {
        "title": "Example body:",
        "content": "{\n    \"firstName\": \"FirstName\",\n    \"lastName\": \"LastName\",\n    \"username\": \"FirstName3@app.com\",\n    \"email\": \"FirstName3@app.com\",\n    \"phones\":\n    [\n        {\n            \"phoneType\": \"mobile\",\n            \"phoneNumber\" : \"5555551212\"\n        }\n    ],\n    \"isActive\": true\n}",
        "type": "json"
      }
    ],
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object",
            "optional": false,
            "field": "user",
            "description": "<p>User</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.id",
            "description": "<p>User ID</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.userName",
            "description": "<p>Username of the user</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.email",
            "description": "<p>Email</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.firstName",
            "description": "<p>First name</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.lastName",
            "description": "<p>Last name</p>"
          },
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "user.phones",
            "description": "<p>User phone numbers</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.phones.phoneNumber",
            "description": "<p>Phone number</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.phones.phoneType",
            "description": "<p>Phone type</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "{\n    \"id\": \"f9d69894-7908-4606-918e-410dca8c3238\",\n    \"firstName\": \"FirstName\",\n    \"lastName\": \"LastName\",\n    \"username\": \"FirstName3@app.com\",\n    \"email\": \"FirstName3@app.com\",\n    \"phones\":\n    [\n        {\n            \"id\": 3,\n            \"userId\": \"f9d69894-7908-4606-918e-410dca8c3238\",\n            \"phoneNumber\": \"5555551212\",\n            \"phoneType\": \"Mobile\"\n        }\n    ],\n    \"isActive\": true\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "delete",
    "url": "/v1/users/:userId",
    "title": "Delete a user",
    "version": "0.1.0",
    "name": "DeleteUserAsync",
    "group": "User",
    "permission": [
      {
        "name": "app.permission->delete.user"
      }
    ],
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "userId",
            "description": "<p>User ID</p>"
          }
        ]
      }
    },
    "success": {
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/UserController.cs",
    "groupTitle": "User",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "BadRequest",
            "description": "<p>The input did not pass the model validation.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400: Bad Request\n[\n    {\n        \"code\": \"user.lastName.required\",\n        \"description\": \"User lastName is a required field\"\n    },\n    {\n        \"code\": \"user.email.invalidFormat\",\n        \"description\": \"User email format is invalid. Required format is text@example.com\"\n    }\n]",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "get",
    "url": "/v1/users/:userId",
    "title": "Get user",
    "version": "0.1.0",
    "name": "GetUserAsync",
    "group": "User",
    "permission": [
      {
        "name": "app.permission->view.user"
      }
    ],
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "userId",
            "description": "<p>User ID</p>"
          }
        ]
      }
    },
    "filename": "../Controllers/API/v1/UserController.cs",
    "groupTitle": "User",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object",
            "optional": false,
            "field": "user",
            "description": "<p>User</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.id",
            "description": "<p>User ID</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.userName",
            "description": "<p>Username of the user</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.email",
            "description": "<p>Email</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.firstName",
            "description": "<p>First name</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.lastName",
            "description": "<p>Last name</p>"
          },
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "user.phones",
            "description": "<p>User phone numbers</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.phones.phoneNumber",
            "description": "<p>Phone number</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.phones.phoneType",
            "description": "<p>Phone type</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "{\n    \"id\": \"f9d69894-7908-4606-918e-410dca8c3238\",\n    \"firstName\": \"FirstName\",\n    \"lastName\": \"LastName\",\n    \"username\": \"FirstName3@app.com\",\n    \"email\": \"FirstName3@app.com\",\n    \"phones\":\n    [\n        {\n            \"id\": 3,\n            \"userId\": \"f9d69894-7908-4606-918e-410dca8c3238\",\n            \"phoneNumber\": \"5555551212\",\n            \"phoneType\": \"Mobile\"\n        }\n    ],\n    \"isActive\": true\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "NotFound",
            "description": "<p>The requested resource was not found</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 404: Not Found\n{\n    \"code\": \"error.404_not_found\",\n    \"description\": \"Could not locate entity with ID 11bef1e7-3ba3-4669-861e-54e91fd8db79\"\n}",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "get",
    "url": "/v1/users/:userId/roles/:roleId",
    "title": "Get role",
    "version": "0.1.0",
    "name": "GetUserRoleById",
    "group": "User",
    "permission": [
      {
        "name": "app.permission->view.role"
      }
    ],
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "userId",
            "description": "<p>User ID</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roleId",
            "description": "<p>Role ID</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object",
            "optional": false,
            "field": "role",
            "description": "<p>Role</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "role.id",
            "description": "<p>Role ID</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "role.name",
            "description": "<p>Name of the role</p>"
          },
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "role.claims",
            "description": "<p>List of claims</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "role.claims.claimType",
            "description": "<p>Type of claim</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "role.claims.claimValue",
            "description": "<p>Value of claim</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n    {\n        \"id\": \"7ec91144-a60e-4240-8878-ccba3c4c2ef4\",\n        \"name\": \"Basic\",\n        \"claims\": [\n            {\n                \"claimType\": \"app.permission\",\n                \"claimValue\": \"login\"\n            },\n            {\n                \"claimType\": \"app.permission\",\n                \"claimValue\": \"list.user-claims\"\n            }\n    }",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/UserController.cs",
    "groupTitle": "User",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "get",
    "url": "/v1/users",
    "title": "Get all users",
    "version": "0.1.0",
    "name": "GetUsers",
    "group": "User",
    "permission": [
      {
        "name": "app.permission->list.users"
      }
    ],
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "users",
            "description": "<p>List of users</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "users.id",
            "description": "<p>User ID</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "users.userName",
            "description": "<p>Username of the user</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "users.email",
            "description": "<p>Email</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "users.firstName",
            "description": "<p>First name</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "users.lastName",
            "description": "<p>Last name</p>"
          },
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "users.phones",
            "description": "<p>User phone numbers</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "users.phones.phoneNumber",
            "description": "<p>Phone number</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "users.phones.phoneType",
            "description": "<p>Phone type</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n   {\n       \"id\": \"A8DCF6EA-85A9-4D90-B722-3F4B9DE6642A\",\n       \"userName\": \"admin\",\n       \"email\": \"admin@admin.com\",\n       \"firstName\": \"Admin_First\",\n       \"lastName\": \"Admin_Last\",\n       \"phones\":\n       [\n          {\n              \"phoneNumber\": \"123-123-1233\",\n              \"phoneType\": \"mobile\"\n          }\n       ]\n    }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/UserController.cs",
    "groupTitle": "User",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "delete",
    "url": "/v1/users/:userId/roles/:roleId",
    "title": "Remove role from user",
    "version": "0.1.0",
    "name": "RevokeRole",
    "group": "User",
    "permission": [
      {
        "name": "app.permission->revoke.role"
      }
    ],
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "roleId",
            "description": "<p>Role ID</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "userId",
            "description": "<p>User ID</p>"
          }
        ]
      }
    },
    "success": {
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/UserController.cs",
    "groupTitle": "User",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          },
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "BadRequest",
            "description": "<p>The input did not pass the model validation.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        },
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 400: Bad Request\n[\n    {\n        \"code\": \"user.lastName.required\",\n        \"description\": \"User lastName is a required field\"\n    },\n    {\n        \"code\": \"user.email.invalidFormat\",\n        \"description\": \"User email format is invalid. Required format is text@example.com\"\n    }\n]",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "put",
    "url": "/v1/users/:userId",
    "title": "Update user",
    "version": "0.1.0",
    "name": "UpdateUserAsync",
    "group": "User",
    "permission": [
      {
        "name": "app.permission->update.user"
      }
    ],
    "filename": "../Controllers/API/v1/UserController.cs",
    "groupTitle": "User",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "Object",
            "optional": false,
            "field": "user",
            "description": "<p>User</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "user.userName",
            "description": "<p>Username of the user</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "user.email",
            "description": "<p>Email</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "user.firstName",
            "description": "<p>First name</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "user.lastName",
            "description": "<p>Last name</p>"
          },
          {
            "group": "Parameter",
            "type": "Object[]",
            "optional": false,
            "field": "user.phones",
            "description": "<p>User phone numbers</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "user.phones.phoneNumber",
            "description": "<p>Phone number</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "user.phones.phoneType",
            "description": "<p>Phone type</p>"
          }
        ]
      }
    },
    "examples": [
      {
        "title": "Example body:",
        "content": "{\n    \"firstName\": \"FirstName\",\n    \"lastName\": \"LastName\",\n    \"username\": \"FirstName3@app.com\",\n    \"email\": \"FirstName3@app.com\",\n    \"phones\":\n    [\n        {\n            \"phoneType\": \"mobile\",\n            \"phoneNumber\" : \"5555551212\"\n        }\n    ],\n    \"isActive\": true\n}",
        "type": "json"
      }
    ],
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object",
            "optional": false,
            "field": "user",
            "description": "<p>User</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.id",
            "description": "<p>User ID</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.userName",
            "description": "<p>Username of the user</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.email",
            "description": "<p>Email</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.firstName",
            "description": "<p>First name</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.lastName",
            "description": "<p>Last name</p>"
          },
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "user.phones",
            "description": "<p>User phone numbers</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.phones.phoneNumber",
            "description": "<p>Phone number</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "user.phones.phoneType",
            "description": "<p>Phone type</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "{\n    \"id\": \"f9d69894-7908-4606-918e-410dca8c3238\",\n    \"firstName\": \"FirstName\",\n    \"lastName\": \"LastName\",\n    \"username\": \"FirstName3@app.com\",\n    \"email\": \"FirstName3@app.com\",\n    \"phones\":\n    [\n        {\n            \"id\": 3,\n            \"userId\": \"f9d69894-7908-4606-918e-410dca8c3238\",\n            \"phoneNumber\": \"5555551212\",\n            \"phoneType\": \"Mobile\"\n        }\n    ],\n    \"isActive\": true\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        }
      ]
    }
  },
  {
    "type": "get",
    "url": "/v1/users/:userId/roles",
    "title": "Get user roles",
    "version": "0.1.0",
    "name": "User",
    "group": "User",
    "permission": [
      {
        "name": "app.permission->list.users"
      }
    ],
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "userId",
            "description": "<p>ID of the user</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "role",
            "description": "<p>List of roles</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "role.id",
            "description": "<p>Role ID</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "role.name",
            "description": "<p>Name of the role</p>"
          },
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "claims",
            "description": "<p>List of claims</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "claims.claimType",
            "description": "<p>Type of claim</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "claims.claimValue",
            "description": "<p>Value of claim</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n    {\n        \"id\": \"7ec91144-a60e-4240-8878-ccba3c4c2ef4\",\n        \"name\": \"Basic\",\n        \"claims\": [\n            {\n                \"claimType\": \"app.permission\",\n                \"claimValue\": \"login\"\n            },\n            {\n                \"claimType\": \"app.permission\",\n                \"claimValue\": \"list.user-claims\"\n            }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "../Controllers/API/v1/UserController.cs",
    "groupTitle": "User",
    "header": {
      "fields": {
        "Header": [
          {
            "group": "Header",
            "type": "String",
            "optional": false,
            "field": "Authorization",
            "description": "<p>Authentication token returned from the /api/Token service</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Header-Example:",
          "content": "{\n    \"Authorization\": \"bearer 5_leQJfkDVIFxRgt-YZcnRGvzlrsFdNXClRaZEaTf3jzToaoFAUtOyneKNhtYo2cCSC9_fYXXBMS3UHwY3MZuzRbWmLqG_H1T5Cm6YHUzzQHPpKCCUqcYMx6m3NMLQoGVX9_f2svSSY4HieOTzwT-LTfxYbxHyuiRY0sJrmoGnQOtWGGC9GhdUXPCpM9_1L8p0vdAC1wnnTKtkDkxulAUkIxj3jJIUc9lUhqLoh5BRYJ8VsABsKEZYouLWvYfL5eqkL7EjwtxRNBhy9rJSU7nc_6lNCk5PEpsFg-laLYWeVO10gVFBkI3Ml7I8KgJA2Bs3vKKccIxuOwqoWjU_XIhV3f9Lhjm34OqSrwcThfx940k5TEWjHG9CHqxnItRvgHtqahc2rP8q0QuZ0qw6Ph5kxEE18YVGa8IVX7zAPW8V6WtXnVnR7SbZwnwiNTuh3i0t0Ib_gOKZAwUZTrVhsTqnFe20zMvJIsOzTtijl80bJMF7ZBAbKKL6zKfJcQ_DwYJaM0CISapXGjbJYMNWPaOzg6kTKtVuHb4akxfxlsAUk0QFqxZ793TT-KCdh-c0ppUNb3brPMh8wGpJDhwLbnhUfpBVnuxdC-sDrGUtzpSnMrPavKivovVT-sXBbqzeg6udOhjN3JXIv2ZzctisLYTw\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 4xx": [
          {
            "group": "Error 4xx",
            "optional": false,
            "field": "Unauthorized",
            "description": "<p>The user is not authenticated.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response",
          "content": "HTTP/1.1 400: Unauthorized\n{\n    \"code\": \"error.400_unauthorized\",\n    \"description\": \"Authorization has been denied for this request.\"\n}",
          "type": "json"
        }
      ]
    }
  },
  {
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "optional": false,
            "field": "varname1",
            "description": "<p>No type.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "varname2",
            "description": "<p>With type.</p>"
          }
        ]
      }
    },
    "type": "",
    "url": "",
    "version": "0.0.0",
    "filename": "../apidoc/docs/main.js",
    "group": "c__Source_launchpad_dotnet_api_Voyage_Web_apidoc_docs_main_js",
    "groupTitle": "c__Source_launchpad_dotnet_api_Voyage_Web_apidoc_docs_main_js",
    "name": ""
  }
] });
