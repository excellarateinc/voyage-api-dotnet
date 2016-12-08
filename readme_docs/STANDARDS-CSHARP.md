## C&#35;
This guide includes a select set of best practices when writing C&#35;, specifically when building .NET Web API apps. The coding standards and examples in this guide are not meant to imporve upon existing Microsoft standards, but to clarify and provide additional information to ensure clarity. 

Wherever possible these best practices are enforced with the default Resharper static code analysis plus some modification defined in this guide. 

## References
* [MSDN C&#35; coding conventions](https://msdn.microsoft.com/en-us/library/ff926074.aspx)

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
  
## Layout
* __Good layout uses formatting to emphasize the structure of your code to make it easier to read.__
* Use the default Visual Studio settings (smart indenting, four-character indents, tabs saved as spaces).
* Write only one statement per line.
* Write only one declaration per line.
* If continuation lines are not indented automatically, indent them one tab stop (four spaces).
* Add at least one blank line between method definitions and property definitions.
* Use parentheses to make clauses in an expression apparent.  
     ```
     if ((val1 > val2) && (val1 > val3))
     {
         // Take appropriate action.
     }
     ```

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
* To avoid exceptions and increase performance by skipping unnecessary comparisons, use && instead of & and || instead of | when you perform comparisons, as shown in the following example.
     
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
