## Design Priciples
The database should be platform agnostic. As a result, all scripts should be written using the |insert standard here| SQL standard. This will ensure that the migrations can be deployed to a compliant platform. 

The |insert standard here| includes support for:

* Feature list here
* Merge?

Avoid writing migrations that use platform specific SQL dialects such as T-SQL. 

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

### Deletes
The application will follow a soft delete pattern. Every table will have an operational column defined that signifies whether or not the record has been deleted. When a delete is requested, this column value will be set to indicate the record has been deleted.

**Need to determine what the column will be**

#### Cascades
Soft deletes will cascade to child relationships as soft deletes when the related records cannnot exist or stand alone. For example, when a user is deleted, the phone numbers related to that user will also be deleted. Since the user phone numbers cannot exist without a user those records are no longer valid. On the other hand, if a user is related to an organization, the organization will not be deleted. This scenario is different because an organization can exist without a user. 

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

## Stored Procedures

### Names
1. Do not prefix store procedures with sp_ 

## Indexes
1. Create a clustered index on the primary key  (default behavior) 
