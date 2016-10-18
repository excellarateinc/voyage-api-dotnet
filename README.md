# launchpad-dotnet-api
Basic .Net Web API 

# Setup
Once you have the source code:

1. Package restore
2. Switch to Package Manager Console
3. Set Default project to Launchpad.Data
4. Run Update-Database from the console
  1. The connection string in LaunchPad.Web determines where the database will be created
  2. The default is localhost/sqlexpress, initial catalog Launchpad
  
# API Endpoints

## Widget
### Sample - Get Widgets
#### URL [http://localhost:52431/api/widget](http://localhost:52431/api/widget)
#### Output

```
[
  {
    "id": 3,
    "name": "Large Widget"
  },
  {
    "id": 7,
    "name": "Medium Widget"
  }
]
```

### Sample - Get Widget
#### URL [http://localhost:52431/api/widget/3](http://localhost:52431/api/widget/3)
#### Output
```
{
  "id": 3,
  "name": "Large Widget"
}
```

  
# Unit Testing
The unit testing libraries are:

1. [Xunit] (https://xunit.github.io/)
2. AutoFixture
3. FluentAssertions
4. MOQ
