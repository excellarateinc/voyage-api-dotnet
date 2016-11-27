## Database Standards
The following guide covers the best practices for database structures within this application. 

## Table of Contents
* [Vendor Agnostic](#vendor-agnostic)
* [Tables](#tables)
* [Columns](#columns)
* [Indexes](#indexes)
* [Logical Deletes](#logical-deletes)

## Vendor Agnostic
The Launchpad API app has been designed to be a kick starter app that any .NET development team can pick up and apply their their existing environment and infrastructure. In many Microsoft .NET shops, Microsoft SQL Server is the primary database, but there are also many companies that have a diverse technology infrastructure consisting of every flavor of database in existence. Since any backend database could potentially used with this API app, the position taken within Launchpad API is to follow [ANSI Standard SQL 1999](https://en.wikipedia.org/wiki/SQL:1999), which is supported by nearly all of the major database vendors.  

__Rules__
* Use common data types when possible
* SQL must conform to [SQL99](https://en.wikipedia.org/wiki/SQL:1999) standards (do not use vendor specific syntax of any kind)
* Do not use: stored procedures, functions, trigger, or views. 
* Test all SQL / DDL using SQL Server Express, Oracle XE, and MySQL

## Tables

### Naming
* Use [PascalCase](https://en.wikipedia.org/wiki/PascalCase)
* Table names should use the singular form. 
  - A table that stores user records will be called User. 
  - In the case of a join table, such as a table that stores the relationship between a role and claims, use the singular form: RoleClaim.
* When modeling a hierarchy, the parent of the relationship should always come first. For example a user has many phone numbers. The table then should be called UserPhoneNumber.
 
### Primary Keys
Every table should have a primary key. Typically this will be surogate key implemented as an identity column. Using an integer primary key value fits well with the web service pattern of being able to reference a particular record within an entity set with a single value. A multipart natural key will not fit this pattern.

These keys should use the following naming convention:

```
PK_{schema.TableName}

PK_core.LaunchpadLogs

```

### Foreign Keys
Referential integrity should be enforced via foreign key relationships. These keys should use the following naming convention:

```
FK_{schema.ForeignKeyTable}_{schema.PrimaryKeyTable}_{PrimaryKeyColmn}

FK_core.RoleClaims_core.Roles_RoleId

```

#### Cascades
Soft deletes will cascade to child relationships as soft deletes when the related records cannnot exist or stand alone. For example, when a user is deleted, the phone numbers related to that user will also be deleted. Since the user phone numbers cannot exist without a user those records are no longer valid. On the other hand, if a user is related to an organization, the organization will not be deleted. This scenario is different because an organization can exist without a user. 

## Columns 

### Naming
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
| DateTime | datetime2 / datetime / time / datetimeoffset | Choose the appropriate datat type depending on the type of value needs to be stored.(Does the value have a time component).   

For a complete list [see](https://msdn.microsoft.com/en-us/library/cc716729(v=vs.110).aspx)

##### Recommendations

1. Use a date or time column to store temporal information. 
  1. An nvarchar or char column **should not be used to store temporal information.** 
2. Store all time in UTC. 
3. Use bit fields to store boolean values. 
  1. An nvarchar or char column **should not be used to store boolean values.** 
    1. Using characters to represent boolean values can lead to inconsistent values being entered to represent true and false. For example: Y, N, y, n, T, t, F, f, Y, y, N, n
    2. Using characters to represent boolean loses any implicit meaning outside of the English language. (Oui, Si)
   

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

**Need to determine how to enforce these columns - triggers for date columns or expect the client to provide the values**

## Indexes
1. Create a clustered index on the primary key  (default behavior) 

## Logical Deletes
> __FINISH DOCUMENTATION__

The application will follow a soft delete pattern. Every table will have an operational column defined that signifies whether or not the record has been deleted. When a delete is requested, this column value will be set to indicate the record has been deleted.

```
IsDeleted = true/false
```
