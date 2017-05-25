define({ "api": [
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
    "filename": "../docs/main.js",
    "group": "C__Projects_voyage_api_dotnet_Voyage_Api_docs_main_js",
    "groupTitle": "C__Projects_voyage_api_dotnet_Voyage_Api_docs_main_js",
    "name": ""
  },
  {
    "type": "post",
    "url": "/v1/profile",
    "title": "Create profile",
    "version": "1.0.0",
    "name": "CreateProfile",
    "group": "Profile",
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
          },
          {
            "group": "Parameter",
            "type": "Object[]",
            "optional": false,
            "field": "users.phones",
            "description": "<p>User phone numbers</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "users.phones.phoneNumber",
            "description": "<p>Phone number</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "users.phones.phoneType",
            "description": "<p>Phone type</p>"
          }
        ]
      }
    },
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
        ]
      },
      "examples": [
        {
          "title": "Location-Example",
          "content": "{\n   \"Location\": \"http://voyageframework.com/api/v1/users/b78ae241-1fa6-498c-aa48-9742245d0d2f\"\n}",
          "type": "json"
        }
      ]
    },
    "success": {
      "examples": [
        {
          "title": "Success-Response:        ",
          "content": "\nHTTP/1.1 201 Created\n{\n   \"id\": \"f9d69894-7908-4606-918e-410dca8c3238\",\n   \"firstName\": \"FirstName\",\n   \"lastName\": \"LastName\",\n   \"username\": \"FirstName3@app.com\",\n   \"email\": \"FirstName3@app.com\",\n   \"phones\": [\n       {\n           \"id\": 3,\n           \"userId\": \"f9d69894-7908-4606-918e-410dca8c3238\",\n           \"phoneNumber\": \"5555551212\",\n           \"phoneType\": \"Mobile\"\n       }\n   ],\n   \"isActive\": true\n}",
          "type": "json"
        }
      ]
    },
    "filename": "../API/v1/AccountController.cs",
    "groupTitle": "Profile",
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
    "url": "/v1/roles",
    "title": "Create a role",
    "version": "1.0.0",
    "name": "CreateRole",
    "group": "Role",
    "permission": [
      {
        "name": "api.roles.create"
      }
    ],
    "parameter": {
      "examples": [
        {
          "title": "Request-Example:",
          "content": "{\n  \"name\": \"Billing\"\n  \"description\": \"Billing Department\"\n}",
          "type": "json"
        }
      ],
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
            "field": "role.name",
            "description": "<p>Name of the role</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "role.description",
            "description": "<p>Description for this role</p>"
          }
        ]
      }
    },
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
    "success": {
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 201 CREATED\n{\n    \"id\": \"34d87057-fafa-4e5d-822b-cddb1700b507\",\n    \"name\": \"Billing\",\n    \"description\": \"Billing Department\"\n    \"claims\": []\n}",
          "type": "json"
        }
      ]
    },
    "filename": "../API/v1/RoleController.cs",
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
    "url": "/v1/roles/:roleId",
    "title": "Get a role",
    "version": "1.0.0",
    "name": "GetRoleById",
    "group": "Role",
    "permission": [
      {
        "name": "api.roles.get"
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
    "filename": "../API/v1/RoleController.cs",
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
    "url": "/v1/roles",
    "title": "Get all roles",
    "version": "1.0.0",
    "name": "GetRoles",
    "group": "Role",
    "permission": [
      {
        "name": "api.roles.list"
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
    "filename": "../API/v1/RoleController.cs",
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
    "version": "1.0.0",
    "name": "RemoveRole",
    "group": "Role",
    "permission": [
      {
        "name": "api.roles.delete"
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
          "content": "HTTP/1.1 204 NO CONTENT",
          "type": "json"
        }
      ]
    },
    "filename": "../API/v1/RoleController.cs",
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
    "type": "post",
    "url": "/v1/roles/:roleId/permissions",
    "title": "Create a role permission",
    "version": "1.0.0",
    "name": "AddRolePermission",
    "group": "Role_Permission",
    "permission": [
      {
        "name": "api.roles.permission.add"
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
          "content": "{\n    \"Location\": \"http://localhost:52431/api/v1/roles/6d1d5caf-d29d-4bf2-a581-0a35081a1240/permissions/219\"\n}",
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
            "field": "permission",
            "description": "<p>Permission</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "permission.permissionType",
            "description": "<p>Type of the permission</p>"
          },
          {
            "group": "Parameter",
            "type": "String",
            "optional": false,
            "field": "permission.permissionValue",
            "description": "<p>Value of the permission</p>"
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
            "field": "permission",
            "description": "<p>Permission</p>"
          },
          {
            "group": "Success 200",
            "type": "Integer",
            "optional": false,
            "field": "permission.id",
            "description": "<p>Claim ID</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "permission.permissionType",
            "description": "<p>Type</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "permission.permissionValue",
            "description": "<p>Value</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 201 Created\n{\n    \"permissionType\": \"app.permission\",\n    \"permissionValue\": \"list.newPermission\",\n    \"id\": 219\n}",
          "type": "json"
        }
      ]
    },
    "filename": "../API/v1/RoleController.cs",
    "groupTitle": "Role_Permission",
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
    "url": "/v1/roles/:roleId/permissions/:permissionId",
    "title": "Get a permission",
    "version": "1.0.0",
    "name": "GetPermissionById",
    "group": "Role_Permission",
    "permission": [
      {
        "name": "api.roles.permission.get"
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
            "field": "permissionId",
            "description": "<p>Permission ID</p>"
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
            "field": "permission",
            "description": "<p>Permission</p>"
          },
          {
            "group": "Success 200",
            "type": "Integer",
            "optional": false,
            "field": "permission.id",
            "description": "<p>Permission ID</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "permission.permissionType",
            "description": "<p>Type</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "permission.permissionValue",
            "description": "<p>Value</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n    \"permissionType\": \"app.permission\",\n    \"permissionValue\": \"list.newPermission\",\n    \"id\": 219\n}",
          "type": "json"
        }
      ]
    },
    "filename": "../API/v1/RoleController.cs",
    "groupTitle": "Role_Permission",
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
    "url": "/v1/roles/:roleId/permissions",
    "title": "Get role permissions",
    "version": "1.0.0",
    "name": "GetRolePermissions",
    "group": "Role_Permission",
    "permission": [
      {
        "name": "api.roles.permission.list"
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
            "field": "permissions",
            "description": "<p>Permissions associated to the role</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "permissions.permissionType",
            "description": "<p>Type of the claim</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "permissions.permissionValue",
            "description": "<p>Value of the claim</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n    {\n        \"permissionType\": \"app.permission\",\n        \"permissionValue\": \"list.newPermission\",\n        \"id\": 17\n    }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "../API/v1/RoleController.cs",
    "groupTitle": "Role_Permission",
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
    "url": "/v1/roles/:roleId/permissions/:permissionId",
    "title": "Remove a role permission",
    "version": "1.0.0",
    "name": "RemoveRolePermission",
    "group": "Role_Permission",
    "permission": [
      {
        "name": "api.roles.permission.delete"
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
            "field": "permissionId",
            "description": "<p>Permission ID</p>"
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
    "filename": "../API/v1/RoleController.cs",
    "groupTitle": "Role_Permission",
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
    "version": "1.0.0",
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
    "filename": "../API/v1/ApplicationInfoController.cs",
    "groupTitle": "Status"
  },
  {
    "type": "post",
    "url": "/v1/users",
    "title": "Create user",
    "version": "1.0.0",
    "name": "CreateUser",
    "group": "User",
    "permission": [
      {
        "name": "api.users.create"
      }
    ],
    "parameter": {
      "examples": [
        {
          "title": "Request-Example:",
          "content": "{\n    \"userName\": \"John\",\n    \"email\": \"John@John.com\",\n    \"firstName\": \"John FirstName\",\n    \"lastName\": \"John LastName\",\n    \"phones\":\n    [\n       {\n           \"phoneNumber\": \"123-123-1233\",\n           \"phoneType\": \"mobile\"\n       }\n    ]\n }",
          "type": "json"
        }
      ]
    },
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
    "success": {
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n   {\n       \"id\": \"A8DCF6EA-85A9-4D90-B722-3F4B9DE6642A\",\n       \"userName\": \"John\",\n       \"email\": \"John@John.com\",\n       \"firstName\": \"John FirstName\",\n       \"lastName\": \"John LastName\",\n       \"isActive\": true,\n       \"isVerifyRequired\": true,\n       \"phones\":\n       [\n          {\n              \"phoneNumber\": \"123-123-1233\",\n              \"phoneType\": \"mobile\"\n          }\n       ]\n    }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "../API/v1/UserController.cs",
    "groupTitle": "User",
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
    "version": "1.0.0",
    "name": "DeleteUserAsync",
    "group": "User",
    "permission": [
      {
        "name": "api.users.delete"
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
          "content": "HTTP/1.1 204 NO CONTENT",
          "type": "json"
        }
      ]
    },
    "filename": "../API/v1/UserController.cs",
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
    "version": "1.0.0",
    "name": "GetUserAsync",
    "group": "User",
    "permission": [
      {
        "name": "api.users.get"
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
          "content": "HTTP/1.1 200 OK\n[\n   {\n       \"id\": \"A8DCF6EA-85A9-4D90-B722-3F4B9DE6642A\",\n       \"userName\": \"John\",\n       \"email\": \"John@John.com\",\n       \"firstName\": \"John FirstName\",\n       \"lastName\": \"John LastName\",\n       \"isActive\": true,\n       \"isVerifyRequired\": true,\n       \"phones\":\n       [\n          {\n              \"phoneNumber\": \"123-123-1233\",\n              \"phoneType\": \"mobile\"\n          }\n       ]\n    }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "../API/v1/UserController.cs",
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
    "url": "/v1/users",
    "title": "Get all users",
    "version": "1.0.0",
    "name": "GetUsers",
    "group": "User",
    "permission": [
      {
        "name": "api.users.list"
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
          "content": "HTTP/1.1 200 OK\n[\n   {\n       \"id\": \"A8DCF6EA-85A9-4D90-B722-3F4B9DE6642A\",\n       \"userName\": \"admin\",\n       \"email\": \"admin@admin.com\",\n       \"firstName\": \"Admin_First\",\n       \"lastName\": \"Admin_Last\",\n       \"isActive\": true,\n       \"isVerifyRequired\": true,\n       \"phones\":\n       [\n          {\n              \"phoneNumber\": \"123-123-1233\",\n              \"phoneType\": \"mobile\"\n          }\n       ]\n    }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "../API/v1/UserController.cs",
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
    "type": "put",
    "url": "/v1/users/:userId",
    "title": "Update user",
    "version": "1.0.0",
    "name": "UpdateUserAsync",
    "group": "User",
    "permission": [
      {
        "name": "api.users.update"
      }
    ],
    "filename": "../API/v1/UserController.cs",
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
    "url": "/v1/users/:userId/permissions",
    "title": "Get user permissions",
    "version": "1.0.0",
    "name": "Claims",
    "group": "User_Permission",
    "permission": [
      {
        "name": "api.users.permissions.list"
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
            "field": "permissions",
            "description": "<p>List of user permissions</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "permissions.permissionType",
            "description": "<p>Type of the permission</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "permissions.permissionValue",
            "description": "<p>Value of the permission</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n    {\n        \"permissionType\": \"authorities\",\n        \"permissionValue\": \"api.users.create\"\n    },\n    {\n        \"permissionType\": \"authorities\",\n        \"permissionValue\": \"api.users.list\"\n    }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "../API/v1/UserController.cs",
    "groupTitle": "User_Permission",
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
    "url": "/v1/users/:userId/roles",
    "title": "Assign role to user",
    "version": "1.0.0",
    "name": "AssignRole",
    "group": "User_Role",
    "permission": [
      {
        "name": "api.users.roles.assign"
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
    "filename": "../API/v1/UserController.cs",
    "groupTitle": "User_Role",
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
    "url": "/v1/users/:userId/roles/:roleId",
    "title": "Get role",
    "version": "1.0.0",
    "name": "GetUserRoleById",
    "group": "User_Role",
    "permission": [
      {
        "name": "api.users.roles.get"
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
    "filename": "../API/v1/UserController.cs",
    "groupTitle": "User_Role",
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
    "version": "1.0.0",
    "name": "RevokeRole",
    "group": "User_Role",
    "permission": [
      {
        "name": "api.users.roles.delete"
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
    "filename": "../API/v1/UserController.cs",
    "groupTitle": "User_Role",
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
    "url": "/v1/users/:userId/roles",
    "title": "Get user roles",
    "version": "1.0.0",
    "name": "User",
    "group": "User_Role",
    "permission": [
      {
        "name": "api.users.roles.list"
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
            "field": "permissions",
            "description": "<p>List of permissions</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "permissions.permissionType",
            "description": "<p>Type of permission</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "permissions.permissionValue",
            "description": "<p>Value of permission</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n    {\n        \"id\": \"7ec91144-a60e-4240-8878-ccba3c4c2ef4\",\n        \"name\": \"Basic\",\n        \"permissions\": [\n            {\n                \"permissionType\": \"authorities\",\n                \"permissionValue\": \"api.roles.get\"\n            },\n            {\n                \"permissionType\": \"authorities\",\n                \"permissionValue\": \"api.permission.get\"\n            }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "../API/v1/UserController.cs",
    "groupTitle": "User_Role",
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
  }
] });
