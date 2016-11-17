## General Naming Conventions

1. Use PascalCase


## Schemas
Schemas should be used to organize related database objects such as tables. For example, all database objects related to the framework are in the [core] schema.

### Names
* Use names that represent a feature or a logical group of functionality.

## Tables


### Names
1. Table names should use the singular form. 
 1. A table that stores user records will be called User. 
 2. In the case of a join table, such as a table that stores the relationship between a role and claims, use the singular form: RoleClaim.
2. When modeling a hierarchy, the parent of the relationship should always come first. For example a user has many phone numbers. The table then should be called UserPhoneNumber.

 
### Keys

#### Primary Keys
Every table should have a primary key. Typically this will be surogate key implemented as an identity column. Using an integer primary key value fits well with the web service pattern of being able to reference a particular record within an entity set with a single value. A multipart natural key will not fit this pattern.

These keys should use the following naming convention:


```
PK_{schema.TableName}

PK_core.LaunchpadLogs

```

#### Foreign Keys
Referential integrity should be enforced via foreign key relationships. These keys should use the following naming convention:

```
FK_{schema.ForeignKeyTable}_{schema.PrimaryKeyTable}_{PrimaryKeyColmn}

FK_core.RoleClaims_core.Roles_RoleId

```


## Columns 

### Names
1. Column names should be descriptive
2. Column names should avoid abbreviations
3. Column names should not contain underscores
4. Columns in different tables that represent the same type of data should be named the same.
1. For example a column that holds a zip code should be called ZipCode in all tables. 


### Data Types
Columns should try to adhere to a standard set of data types based on their content.

|Primitive Type | Column Data Type | Additional Information
|:----|:----|:----|
|String | nvarchar | Enforce a maximum length when the possible set of values is well known|
| Bool | bit | Entityframework maps bit columns into a true/false boolean. Avoid using char(1) or other textual representations of bool values due to inconsistenancy in input as well as lack of meaning in other languages.|
| xml | xml | SQL 2012 introduces support for XML column types|


For a complete list [see](https://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx)



### Operational Columns
Operational columns will be added to every table. The standard columns are:

| Column Name | Data Type | Nullable | Description
| ------------- |:-------------:| :-----:| :-----|
| CreateDate | DateTime | No | Row create date|
| CreateUser | nvarchar(50) | No | Create user |
| ModifyDate | DateTime | No | Row modify date | 
| ModifyUser | nvarchar(50) | No | Modify user |
| Deleted (Option A)    | bit | No    | Indicates if the record has been soft deleted|
| DeletedAt (Option B) | DateTime | Yes | Indicates the date the record was deleted|

## Stored Procedures

### Names
1. Do not prefix store procedures with sp_ 

## Indexes
1. Create a clustered index on the primary key  (default behavior) 
