define({ "api": [
  {
    "type": "get",
    "url": "/widget/:id",
    "title": "Get a widget",
    "version": "0.1.1",
    "name": "GetWidget",
    "group": "Widget",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "Number",
            "optional": false,
            "field": "id",
            "description": "<p>Widget's unique ID.</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "name",
            "description": "<p>Name of the Widget.</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": false,
            "field": "id",
            "description": "<p>ID of the Widget.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n   \"id\": 3,\n   \"name\": \"Large Widget\"\n}",
          "type": "json"
        }
      ]
    },
    "error": {
      "fields": {
        "Error 404": [
          {
            "group": "Error 404",
            "optional": false,
            "field": "WidgetNotFound",
            "description": "<p>The Widget with the requested id was not found.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 Not Found\n{\n  \"message\": \"Widget with ID 33 not found\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "Controllers/API/WidgetController.cs",
    "groupTitle": "Widget",
    "sampleRequest": [
      {
        "url": "/api/widget/:id"
      }
    ]
  },
  {
    "type": "get",
    "url": "/widget/:id",
    "title": "Request Widget information",
    "version": "0.1.0",
    "name": "GetWidget",
    "group": "Widget",
    "parameter": {
      "fields": {
        "Parameter": [
          {
            "group": "Parameter",
            "type": "Number",
            "optional": false,
            "field": "id",
            "description": "<p>Widget's unique ID!.</p>"
          }
        ]
      }
    },
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "name",
            "description": "<p>Name of the Widget.</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": false,
            "field": "id",
            "description": "<p>ID of the Widget.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n{\n   \"id\": 3,\n   \"name\": \"Large Widget\"\n}",
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
            "field": "WidgetNotFound",
            "description": "<p>The Widget with the requested id was not found.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Error-Response:",
          "content": "HTTP/1.1 404 Not Found\n{\n  \"message\": \"Widget with ID 33 not found\"\n}",
          "type": "json"
        }
      ]
    },
    "filename": "Controllers/_apidoc.js",
    "groupTitle": "Widget",
    "sampleRequest": [
      {
        "url": "/api/widget/:id"
      }
    ]
  },
  {
    "type": "get",
    "url": "/widget",
    "title": "Get all widgets",
    "version": "0.1.1",
    "name": "GetWidgets",
    "group": "Widget",
    "success": {
      "fields": {
        "Success 200": [
          {
            "group": "Success 200",
            "type": "Object[]",
            "optional": false,
            "field": "widgets",
            "description": "<p>List of widgets.</p>"
          },
          {
            "group": "Success 200",
            "type": "String",
            "optional": false,
            "field": "widgets.name",
            "description": "<p>Name of the Widget.</p>"
          },
          {
            "group": "Success 200",
            "type": "Number",
            "optional": false,
            "field": "widgets.id",
            "description": "<p>ID of the Widget.</p>"
          }
        ]
      },
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n {\n    \"id\": 3,\n    \"name\": \"Large Widget\"\n },\n {\n    \"id\": 7,\n    \"name\": \"Medium Widget\"\n }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "Controllers/API/WidgetController.cs",
    "groupTitle": "Widget",
    "sampleRequest": [
      {
        "url": "/api/widget"
      }
    ]
  },
  {
    "type": "get",
    "url": "/widget",
    "title": "Get all widgets",
    "version": "0.1.0",
    "name": "GetWidgets",
    "group": "Widget",
    "success": {
      "examples": [
        {
          "title": "Success-Response:",
          "content": "HTTP/1.1 200 OK\n[\n {\n    \"id\": 3,\n    \"name\": \"Large Widget\"\n },\n {\n    \"id\": 7,\n    \"name\": \"Medium Widget\"\n }\n]",
          "type": "json"
        }
      ]
    },
    "filename": "Controllers/_apidoc.js",
    "groupTitle": "Widget",
    "sampleRequest": [
      {
        "url": "/api/widget"
      }
    ]
  }
] });
