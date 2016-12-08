## C#
> These guidelines are cherry picked from the official MSDN C# coding conventions document located [here](https://msdn.microsoft.com/en-us/library/ff926074.aspx).

1. Layout
   - __Good layout uses formatting to emphasize the structure of your code and to make the code easier to read.__
   - Use the default Visual Studio settings (smart indenting, four-character indents, tabs saved as spaces).
   - Write only one statement per line.
   - Write only one declaration per line.
   - If continuation lines are not indented automatically, indent them one tab stop (four spaces).
   - Add at least one blank line between method definitions and property definitions.
   - Use parentheses to make clauses in an expression apparent.
   
     ```
     if ((val1 > val2) && (val1 > val3))
     {
         // Take appropriate action.
     }
     ```
2. Commenting
   - Place the comment on a separate line, not at the end of a line of code.
   - Begin comment text with an uppercase letter.
   - End comment text with a period.
   - Insert one space between the comment delimiter (//) and the comment text.
   
     ```
     // This is a comment.
     ```
   - Do not create formatted blocks of asterisks around comments.
   
     ```
     // Avoid
     // ****************
     // * Comment here *
     // ****************
     ```
3. Language
   - Strings
     * Use the + operator to concatenate short strings.
     
       ```
       string displayName = nameList[n].LastName + ", " + nameList[n].FirstName;
       ```
     * To append strings in loops, especially when you are working with large amounts of text, use a StringBuilder object.
     
       ```
       var phrase = "my phrase";
       var manyPhrases = new StringBuilder();
       for (var i = 0; i < 10000; i++)
       {
           manyPhrases.Append(phrase);
       }
       ```
   - Implicit Types
     * Use implicit typing for local variables when the type of the variable is obvious from the right side of the assignment, or when the precise type is not important.
     
       ```
       // When the type of a variable is clear from the context, use var 
       // in the declaration.
       var myString = "This is clearly a string.";
       var myNumber = 27;
       var myInteger = Convert.ToInt32(Console.ReadLine());
       ```
     * Do not use var when the type is not apparent from the right side of the assignment.
     
       ```
       // When the type of a variable is not clear from the context, use an
       // explicit type.
       int result = ExampleClass.ResultSoFar();       
       ```
     * Do not rely on the variable name to specify the type of the variable. It might not be correct.
     
       ```
       // Naming the following variable inputInt is misleading. 
       // It is a string.
       var inputInt = Console.ReadLine();
       Console.WriteLine(inputInt);
       ```
   - Unsigned Data Type
     * In general, use int rather than unsigned types. The use of int is common throughout C#, and it is easier to interact with other libraries when you use int.
   - Arrays
     * Use the concise syntax when you initialize arrays on the declaration line.
     
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
   - Exceptions
     * Use a try-catch statement for most exception handling.
     
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
   - && and || Operators
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
   - New Operator
     * Use the concise form of object instantiation, with implicit typing, as shown in the following declaration.
    
       ```
       var instance1 = new ExampleClass();
       ```
     * Use object initializers to simplify object creation.
     
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
   - Static Members
     * Call static members by using the class name: ClassName.StaticMember. This practice makes code more readable by making static access clear. Do not qualify a static member defined in a base class with the name of a derived class. While that code compiles, the code readability is misleading, and the code may break in the future if you add a static member with the same name to the derived class.
   - LINQ Queries
     * Use meaningful names for query variables. The following example uses seattleCustomers for customers who are located in Seattle.
     
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
     * Rename properties when the property names in the result would be ambiguous. For example, if your query returns a customer name and a distributor ID, instead of leaving them as Name and ID in the result, rename them to clarify that Name is the name of a customer, and ID is the ID of a distributor.
     
       ```
       var localDistributors2 =
                       from cust in customers
                       join dist in distributors on cust.City equals dist.City
                       select new { CustomerName = cust.Name, DistributorID = dist.ID };       
       ```
     * Use where clauses before other query clauses to ensure that later query clauses operate on the reduced, filtered set of data.
     
       ```
       var seattleCustomers2 = from cust in customers
                               where cust.City == "Seattle"
                               orderby cust.Name
                               select cust;       
       ```
     * Use multiple from clauses instead of a join clause to access inner collections. For example, a collection of Student objects might each contain a collection of test scores. When the following query is executed, it returns each score that is over 90, along with the last name of the student who received the score.
     
       ```
       // Use a compound from to access the inner sequence within each element.
       var scoreQuery = from student in students
                        from score in student.Scores
                        where score > 90
                        select new { Last = student.LastName, score };       
       ```
