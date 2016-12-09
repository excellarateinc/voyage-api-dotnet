## C&#35;
This guide includes a select set of best practices when writing C&#35;, specifically when building .NET Web API apps. The coding standards and examples in this guide are not meant to imporve upon existing Microsoft standards, but to clarify and provide additional information to ensure clarity. 

Wherever possible these best practices are enforced with the default Resharper static code analysis plus some modification defined in this guide. 

## References
* [MSDN C&#35; coding conventions](https://msdn.microsoft.com/en-us/library/ff926074.aspx)
* [Resharper Code Inspections Reference](https://www.jetbrains.com/help/resharper/2016.3/Reference_Code_Inspections.html)

## Table of Contents
* [Layout](#layout)
* [Commenting](#commenting)
  * [Place the comment on a separate line, not at the end of a line of code](#place-the-comment-on-a-separate-line-not-at-the-end-of-a-line-of-code)
  * [Begin comment text with an uppercase letter](#begin-comment-text-with-an-uppercase-letter)
  * [End comment text with a period](#end-comment-text-with-a-period)
  * [Insert one space between the comment delimiter (//) and the comment text](#insert-one-space-between-the-comment-delimiter--and-the-comment-text)
  * [Do not create formatted blocks of asterisks around comments](#do-not-create-formatted-blocks-of-asterisks-around-comments)
* [Strings](#strings)
  * [Use the + operator to concatenate short strings](#use-the--operator-to-concatenate-short-strings)
  * [Use a StringBuilder when concatenating more than a few strings](#use-a-stringbuilder-when-concatenating-more-than-a-few-strings)
* [Implicit Types](#implicit-types)
  * [Use 'var' when the type of the variable is obvious](#use-var-when-the-type-of-the-variable-is-obvious)
  * [Do not use 'var' when the type is not apparent from the right side of the assignment](#do-not-use-var-when-the-type-is-not-apparent-from-the-right-side-of-the-assignment)
  * [Do not rely on the variable name to specify the type of the variable](#do-not-rely-on-the-variable-name-to-specify-the-type-of-the-variable)
  * [Avoid unsigned data types](#avoid-unsigned-data-types)
* [Arrays](#arrays)
  * [Use the concise syntax when you initialize arrays on the declaration line](#use-the-concise-syntax-when-you-initialize-arrays-on-the-declaration-line)
* [Exceptions](#exceptions)
  * [Use a try-catch statement for most exception handling](#use-a-try-catch-statement-for-most-exception-handling)
  * [&& and || Operators](#-and--operators)
* [New Operator](#new-operator)
  * [Use the concise form of object instantiation, with implicit typing](#use-the-concise-form-of-object-instantiation--with-implicit-typing)
  * [Use object initializers to simplify object creation](#use-object-initializers-to-simplify-object-creation)
* [Static Members](#static-members)
  * [Call static members by using the class name: ClassName.StaticMember](#call-static-members-by-using-the-class-name-classnamestaticmember)
* [LINQ Queries](#linq-queries)
  * [Use meaningful names for query variables](#use-meaningful-names-for-query-variables)
  * [Rename properties when the property names in the result would be ambiguous](#rename-properties-when-the-property-names-in-the-result-would-be-ambiguous)
  * [Use 'where' clauses before other 'query' clauses to avoid low-performing queries](#use-where-clauses-before-other-query-clauses-to-avoid-low-performing-queries)
  * [Use multiple 'from' clauses instead of a 'join' clause to access inner collections](#use-multiple-from-clauses-instead-of-a-join-clause-to-access-inner-collections)
* [Code Style](#code-style)
    * [Add Parentheses to Avoid Non Obvious Precedence](#add-parentheses-to-avoid-non-obvious-precedence)
    * [Remove Redundant 'this' Qualifier](#remove-redundant-this-qualifier)
    * [Put Access Modifier First](#put-access-modifier-first)
    * [Convert Nullable of T to 'T?'](#convert-nullable-of-t-to-t)
    * [Convert Property to Auto Property](#convert-property-to-auto-property)
    * [Convert to Property with Expression Body](#convert-to-property-with-expression-body)
    * [Use Readonly Fields](#use-readonly-fields)
    * [Use Consistent Naming](#use-consistent-naming)
    * [Use Optional Parameters](#use-optional-parameters)
    * [Invert 'if' Statement to Reduce Nesting](#invert-if-statement-to-reduce-nesting)
    * [Join Local Variable Declaration and Assignment](#join-local-variable-declaration-and-assignment)
    * [Separate Attributes In Section](#separate-attributes-in-section)
    * [Use the null-conditional operator](#use-the-null-conditional-operator)
    * [Merge Sequential Checks in AND or OR Expressions](#merge-sequential-checks-in-and-or-or-expressions)
    * [Ensure Namespace Corresponds to File Location](#ensure-namespace-corresponds-to-file-location)
    * [Make Parameter Type IEnumerable of T When the parameter implements IEnumerable](#parameter-type-can-be-ienumerable-of-t)
    * [Avoid Possible multiple enumeration of IEnumerable](#avoid-possible-multiple-enumeration-of-ienumerable)
    * [Avoid Redundant 'else' Keyword](#avoid-redundant-else-keyword)
    * [Remove Redundant Parentheses](#remove-redundant-parentheses)
    * [Replace Built in Type Reference with a CLR Type Name or a Keyword](#replace-built-in-type-reference-with-a-clr-type-name-or-a-keyword)
    * [Use String.IsNullOrEmpty](#use-string.isnullorempty)
    
    
## Layout
* __Good layout uses formatting to emphasize the structure of your code to make it easier to read.__
* Use the default Visual Studio settings (smart indenting, four-character indents, tabs saved as spaces).
* Write only one statement per line.
* Write only one declaration per line.
* If continuation lines are not indented automatically, indent them one tab stop (four spaces).
* Add at least one blank line between method definitions and property definitions.

**[⬆ back to top](#table-of-contents)**

## Commenting
#### Place the comment on a separate line, not at the end of a line of code
> Why? Most people read top-to-bottom left-to-right. Keeping comments in line with the code is much easier to read and also hard to miss when scanning code. It's also more work when refactoring code to not accidentally move or delete comments along with code. 

    ```
    // Increment foo due to requirement LP-1234 http://ticket.mycompany.com/LP-1234 stating that 
    // 'foo' can never be larger than 'bar'
    if (foo > bar)
    {
        bar += foo;
    }
    ```

**[⬆ back to top](#table-of-contents)**

#### Begin comment text with an uppercase letter
> Why? TBD

**[⬆ back to top](#table-of-contents)**

#### End comment text with a period
> Why? TBD

**[⬆ back to top](#table-of-contents)**

#### Insert one space between the comment delimiter (//) and the comment text
> Why? TBD

     ```
     // This is a comment.
     ```
     
**[⬆ back to top](#table-of-contents)**     
     
#### Do not create formatted blocks of asterisks around comments
> Why? TBD

     ```
     // Avoid
     // ****************
     // * Comment here *
     // ****************
     ```

**[⬆ back to top](#table-of-contents)**

## Strings

#### Use the + operator to concatenate short strings
> Why? TBD

```
string displayName = nameList[n].LastName + ", " + nameList[n].FirstName;
```
       
**[⬆ back to top](#table-of-contents)**
       
#### Use a StringBuilder when concatenating more than a few strings
> Why? Strings are immutable, so whenever a string is combined within another string then a new string is created in memory. The prior two strings remain in memory until garbage collection occurs. If hundreds of concatonations occur in a loop, then hundreds of strings will be left orphaned and consuming memory until the next schedule memory garbage collection. StringBuilder was created to prevent many string objects from being orphaned when merging text many times over. 
     
       ```
       var phrase = "my phrase";
       var manyPhrases = new StringBuilder();
       for (var i = 0; i < 10000; i++)
       {
           manyPhrases.Append(phrase);
       }
       ```
       
**[⬆ back to top](#table-of-contents)**

## Implicit Types

#### Use 'var' when the type of the variable is obvious
> Why? Use implicit typing for local variables when the type of the variable is obvious from the right side of the assignment, or when the precise type is not important. Not repeating types in a single line is much faster to read and equally as intuitive. 
     
       ```
       // When the type of a variable is clear from the context, use var 
       // in the declaration.
       var myString = "This is clearly a string.";
       var myNumber = 27;
       var myInteger = Convert.ToInt32(Console.ReadLine());
       ```

**[⬆ back to top](#table-of-contents)**

#### Do not use 'var' when the type is not apparent from the right side of the assignment
> Why? TBD

       ```
       // When the type of a variable is not clear from the context, use an
       // explicit type.
       int result = ExampleClass.ResultSoFar();       
       ```

**[⬆ back to top](#table-of-contents)**

#### Do not rely on the variable name to specify the type of the variable
> Why? The variable name might not provide enough information, or perhaps misleading information.
     
       ```
       // Naming the following variable inputInt is misleading. 
       // It is a string.
       var inputInt = Console.ReadLine();
       Console.WriteLine(inputInt);
       ```

**[⬆ back to top](#table-of-contents)**

#### Avoid unsigned data types
> Why? In general, use 'int' rather than unsigned types because in most cases unsigned numbers are not needed, it's not a common practice, and it is easier to interact with other libraries when you use int.

**[⬆ back to top](#table-of-contents)**

## Arrays
#### Use the concise syntax when you initialize arrays on the declaration line
> Why? TBD

       ```
       // Preferred syntax. Note that you cannot use var here instead of string[].
       string[] vowels1 = { "a", "e", "i", "o", "u" };

       // If you use explicit instantiation, you can use var.
       var vowels2 = new string[] { "a", "e", "i", "o", "u" };

       // If you specify an array size, you must initialize the elements one at a time.
       var vowels3 = new string[5];
       vowels3[0] = "a";
       vowels3[1] = "e";
       ```

**[⬆ back to top](#table-of-contents)**

## Exceptions

#### Use a try-catch statement for most exception handling
> Why? TBD

       ```
       static string GetValueFromArray(string[] array, int index)
       {
           try
           {
               return array[index];
           }
           catch (System.IndexOutOfRangeException ex)
           {
               Console.WriteLine("Index is out of range: {0}", index);
               throw;
           }
       }       
       ```

**[⬆ back to top](#table-of-contents)**

#### && and || Operators
> Why? To avoid exceptions and increase performance by skipping unnecessary comparisons, use && instead of & and || instead of | when you perform comparisons, as shown in the following example.
     
       ```
       // If the divisor is 0, the second clause in the following condition
       // causes a run-time error. The && operator short circuits when the
       // first expression is false. That is, it does not evaluate the
       // second expression. The & operator evaluates both, and causes 
       // a run-time error when divisor is 0.
       if ((divisor != 0) && (dividend / divisor > 0))
       {
           Console.WriteLine("Quotient: {0}", dividend / divisor);
       }
       else
       {
           Console.WriteLine("Attempted division by 0 ends up here.");
       }     
       ```

**[⬆ back to top](#table-of-contents)**

## New Operator

#### Use the concise form of object instantiation, with implicit typing
> Why? TBD

       ```
       var instance1 = new ExampleClass();
       ```

**[⬆ back to top](#table-of-contents)**

#### Use object initializers to simplify object creation
> Why? TBD

       ```
       // Avoid
       var myClass = new ExampleClass();
       myClass.Name = "Desktop";
       myClass.ID = 37414;
       myClass.Location = "Redmond";
       myClass.Age = 2.3;

       // Preferred
       var myClass = new ExampleClass 
       { 
           Name = "Desktop", 
           ID = 37414, 
           Location = "Redmond", 
           Age = 2.3 
       };
       ```

**[⬆ back to top](#table-of-contents)**

## Static Members

#### Call static members by using the class name: ClassName.StaticMember
> Why? This practice makes code more readable by making static access clear. Do not qualify a static member defined in a base class with the name of a derived class. While that code compiles, the code readability is misleading, and the code may break in the future if you add a static member with the same name to the derived class.

**[⬆ back to top](#table-of-contents)**

## LINQ Queries

#### Use meaningful names for query variables
> Why? TBD

The following example uses seattleCustomers for customers who are located in Seattle.
     
       ```
       var seattleCustomers = from cust in customers
                              where cust.City == "Seattle"
                              select cust.Name;       
       ```
     * Use aliases to make sure that property names of anonymous types are correctly capitalized, using Pascal casing.
     
       ```
       var localDistributors =
           from customer in customers
           join distributor in distributors on customer.City equals distributor.City
           select new { Customer = customer, Distributor = distributor };       
       ```

**[⬆ back to top](#table-of-contents)**

#### Rename properties when the property names in the result would be ambiguous
> Why? TBD

For example, if your query returns a customer name and a distributor ID, instead of leaving them as Name and ID in the result, rename them to clarify that Name is the name of a customer, and ID is the ID of a distributor.
     
       ```
       var localDistributors2 =
                       from cust in customers
                       join dist in distributors on cust.City equals dist.City
                       select new { CustomerName = cust.Name, DistributorID = dist.ID };       
       ```

**[⬆ back to top](#table-of-contents)**

#### Use 'where' clauses before other 'query' clauses to avoid low-performing queries
> Why? 

Ensure that 'where' clauses are added before any later query clauses so that the initial query is reduced and the subsequent queries are acting on a filtered set of data. Bundling multiple queries together without proper where clauses can create large datasets that perform very poorly. 
     
       ```
       var seattleCustomers2 = from cust in customers
                               where cust.City == "Seattle"
                               orderby cust.Name
                               select cust;       
       ```

**[⬆ back to top](#table-of-contents)**

#### Use multiple 'from' clauses instead of a 'join' clause to access inner collections
> Why? TBD

For example, a collection of Student objects might each contain a collection of test scores. When the following query is executed, it returns each score that is over 90, along with the last name of the student who received the score.
     
       ```
       // Use a compound from to access the inner sequence within each element.
       var scoreQuery = from student in students
                        from score in student.Scores
                        where score > 90
                        select new { Last = student.LastName, score };       
       ```

**[⬆ back to top](#table-of-contents)**

## Code Style

#### Add Parentheses to Avoid Non-Obvious Precedence
> Why? It makes is much easier to determine the order of operations in the expression.

     
       ```
       // Avoid.
       if (a & b | c)
       
       // Recommended.
       if ((a & b) | c)
       ```

**[⬆ back to top](#table-of-contents)**

#### Remove Redundant 'this' Qualifier
> Why? It doesn't serve any practical purpose. It makes the code harder to read.

     
       ```
       // Avoid.
       this._service = service;
       
       // Recommended.
       _service = service;
       ```

**[⬆ back to top](#table-of-contents)**

#### Put Access Modifier First
> Why? It makes it easy to see the accessibility of the item. Also, arranging them in a similar way throughout your code is a good practice, which improves code readability.

     
       ```
       // Avoid.
       static private int count;
       
       // Recommended.
       private static int count;
       ```

**[⬆ back to top](#table-of-contents)**

#### Convert Nullable of T to 'T?'
> Why? 'T?' is built into the language as a shorthand for Nullable<T>. It is easier to quickly see that the object is nullable.

       ```
       // Avoid.
       Nullable<int> count;
       
       // Recommended.
       int? count;
       ```

**[⬆ back to top](#table-of-contents)**

#### Convert Property to Auto Property
> Why? Auto properties are simpler to read as well as write.

     
       ```
       // Avoid.
       private Color bgColor;
       public Color BackgroundColor
       {
           get { return bgColor; }
           set { bgColor = value; }
       }
       
       // Recommended.
       public Color BackgroundColor { get; set; }
       ```

**[⬆ back to top](#table-of-contents)**


#### Convert to Property with Expression Body
> Why? Expression-bodied properties, introduced in C# 6 are both more concise and readable

     
       ```
       // Avoid.
       private string _name;
       public int NameLength
       {
         get
         {
           return string.IsNullOrEmpty(_name) ? 0 : _name.Length;
         }
       }
       
       // Recommended.
       private string _name;
       public int NameLength => string.IsNullOrEmpty(_name) ? 0 : _name.Length;
       ```

**[⬆ back to top](#table-of-contents)**


#### Use Readonly Fields
> Why? To ensure that this class will not inadvertently assign fields anywhere within its methods.

     
       ```
       // Avoid.
       public class MyClass
       {
           private string _name;
           public MyClass(string name) 
           {
               _name = name
           }
       }
       
       // Recommended.       
       public class MyClass
       {
           private readonly string _name;
           public MyClass(string name) 
           {
               _name = name
           }
       }
       ```

**[⬆ back to top](#table-of-contents)**

#### Use Consistent Naming
> Why? This ensures consistency and readability throughout the application. 

  - The PascalCasing convention, used for all identifiers except parameter names, capitalizes the first character of each word (including acronyms over two letters in length), as shown in the following examples:
    * PropertyDescriptor
    * HtmlTag
  A special case is made for two-letter acronyms in which both letters are capitalized, as shown in the following identifier:
  IOStream
  - The camelCasing convention, used only for parameter names, capitalizes the first character of each word except the first word, as shown in the following examples. As the example also shows, two-letter acronyms that begin a camel-cased identifier are both lowercase.
    * propertyDescriptor
    * ioStream
    * htmlTag

**[⬆ back to top](#table-of-contents)**

#### Use Optional Parameters
> Why? Allows you to write less code and is easier to read. 

     
       ```
       // Avoid.
       void Foo(object value)
       {
          Foo(value, true);
       }
       void Foo(object value, bool flag)
       {
          //implementation
       }       
       
       // Recommended.      
       void Foo(object value, bool flag = true)
       {
          //implementation
       }       
       ```

**[⬆ back to top](#table-of-contents)**

#### Invert if Statement to Reduce Nesting
> Why? Without inversion, If blocks can encompass the whole body of the method. Inversion makes the code more readable by getting rid of the nested scope.

     
       ```
       // Avoid.
       void PrintName(Person p)
       {
           if (p != null)
           {
               if (p.Name != null)
               {
                 Console.WriteLine(p.Name);
               }
           }
       }       
       
       // Recommended.
       void PrintName(Person p)
       {
           if (p == null) return
           if (p.Name == null) return;
           Console.WriteLine(p.Name);
       }       
       ```

**[⬆ back to top](#table-of-contents)**

#### Join Local Variable Declaration and Assignment
> Why? Removes an unnecessary line and improves readability of your code.

     
       ```
       // Avoid.
       int Bar()
       {
           int myInt;
           myInt = 3;
           return myInt - 1;
       }       
       
       // Recommended.
       int Bar()
       {
           var myInt = 3;
           return myInt - 1;
       }       
       ```

**[⬆ back to top](#table-of-contents)**

#### Separate Attributes In Section
> Why? Makes attributes easier to read. Easier to keep separate attributes from appearing coupled.

     
       ```
       // Avoid.
       [Attribute1, Attribute2]
       public class MyClass { }
       
       // Recommended.
       [Attribute1]
       [Attribute2]
       public class MyClass { }
       ```

**[⬆ back to top](#table-of-contents)**

#### Use the null-conditional operator
> Why? C# 6 introduced this shorthand notation to test for null before member access. It is easier to read and less code to write.

     
       ```
       // Avoid.
       string GetAttr(XElement node, string attrName)
       {
           var attrNode = node.Attribute(attrName);
           return attrNode == null ? null : attrNode.Value;
       }       
       
       // Recommended.
       string GetAttr(XElement node, string attrName)
       {
           var attrNode = node.Attribute(attrName);
           return attrNode?.Value;
       }
       ```

**[⬆ back to top](#table-of-contents)**

#### Merge Sequential Checks in AND or OR Expressions
> Why? Makes the code footprint smaller by utilizing shorthand notation introduced in C# 6.

     
       ```
       // Avoid.
       if (p == null || p.Arguments == null) { } 
       
       // Recommended.
       if (p?.Arguments == null) { }
       ```

**[⬆ back to top](#table-of-contents)**

#### Ensure Namespace Corresponds to File Location
> Why? This makes it miuch easier to see where a code file exists in the solution, which in turn makes importing classes easier.

     
       ```
       // Assume we are in a class called MyClass in a project called MyProject.Services.
       // Avoid.
       namespace Random.Namespace
       {
           public class MyClass { }
       }
       
       // Recommended.
       namespace MyProject.Services
       {
           public class MyClass { }
       }
       ```

**[⬆ back to top](#table-of-contents)**

#### Make Parameter Type IEnumerable of T When the parameter implements IEnumerable
> Why? Makes the method agnostic with respect to the type of collection you give it, making it more resusable.

     
       ```
       // Avoid.
       public void DoStuff(List<Person> personList) { }
       
       // Recommended.
       public void DoStuff(IEnumerable<Person> personList) { }
       ```

**[⬆ back to top](#table-of-contents)**

#### Avoid Possible multiple enumeration of IEnumerable
> Why? Multiple enumeration could lead to a major performance penalty, especially when using LINQ to SQL.

     
       ```
       // Avoid.
       IEnumerable<string> names = GetNames();
       foreach (var name in names)
       {
           Console.WriteLine("Found " + name);
       }         
       var allNames = new StringBuilder();
       foreach (var name in names)
       {
            allNames.Append(name + " ");   
       }  
       
       // Recommended.
       IEnumerable<string> names = GetNames().ToList();
       foreach (var name in names)
       {
           Console.WriteLine("Found " + name);
       }         
       var allNames = new StringBuilder();
       foreach (var name in names)
       {
            allNames.Append(name + " ");   
       }         
       ```

**[⬆ back to top](#table-of-contents)**

#### Avoid Redundant Else Keyword
> Why? Removes unnecessary boilerplate from the method, making it easier to read.

     
       ```
       // Avoid.
       public int Sign(double d)
       {
           if (d > 0.0)
           {
               return 1;
           }
           else 
           {
               return -1;
           }                    
       }       
       
       // Recommended.
       public int Sign(double d)
       {
           if (d > 0.0)
           {
               return 1;
           }
           return -1;                   
       }          
       ```

**[⬆ back to top](#table-of-contents)**

#### Remove Redundant Parentheses
> Why? They do not help at all and just add up to dead code as well as adding complexity.

     
       ```
       // Avoid.
       Thing item = ((Thing))myThing;
       // Recommended.
       Thing item = (Thing)myThing;
       ```

**[⬆ back to top](#table-of-contents)**

#### Replace Built in Type Reference with a CLR Type Name or a Keyword
> Why? The C# keywords are easier to read and allow more consistent code, instead of having a mix of keywords and built in types.

     
       ```
       // Avoid.
       Boolean hasCount;
       
       // Recommended.
       bool hasCount;
       ```

**[⬆ back to top](#table-of-contents)**

#### Use String.IsNullOrEmpty
> Why? This avoids redundant null and length checks for strings, allowing for cleaner code.

     
       ```
       // Avoid.
       public void SetName(string name)
       {
           if (name != null && name.Length > 0)
           {
               myName = name;
           }        
       }       
       
       // Recommended.
       public void SetName(string name)
       {
           if (!string.IsNullOrEmpty(name))
           {
               myName = name;
           }        
       }            
       ```

**[⬆ back to top](#table-of-contents)**

