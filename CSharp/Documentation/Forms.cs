﻿namespace Microsoft.Bot.Builder.Form
{
    /// \page Forms 
    /// \tableofcontents
    /// 
    /// \section forms Form Flow
    /// \ref dialogs are very powerful and flexible, but handling a guided conversation like ordering a sandwich
    /// can require a lot of effort.  At each point in the dialog, there are many possibilities for what happens
    /// next.  You may need to clarify an ambiguity, provide help, go back or show progress so far.  
    /// In order to simplify building guided conversations the framework provides a very powerful 
    /// dialog building block known as a form.  A form sacrifices some of the flexibility provided by dialogs, 
    /// but in a way that requires much less effort.  Even better, you can combine forms and other kinds of dialogs
    /// like a <a href="http://luis.ai">LUIS</a> dialog to get the best of both worlds.  The dialog guides the user through filling in the form
    /// while provding help and guidance along the way.
    /// 
    /// The clearest way to understand this is to take a look at the SimpleSandwichBot sample. 
    /// In that sample you define the form you want using C# classes, fields and properties. 
    /// That is all you need to do to get a pretty good dialog that supports help, clarification, status and 
    /// navigation without any more effort.  Once you have that dialog you can make use of simple annotations
    /// to improve your bot in a straightforward way.  
    /// 
    /// \subsection fields Forms and Fields
    /// A form is made up of fields that you want to fill in through a conversation with the user.  
    /// The simplest way to describe a form is through a C# class.  
    /// Within a class, a "field" is any public field or property with one of the following types:
    /// * Integral -- sbyte, byte, short, ushort, int, uint, long, ulong
    /// * Floating point -- float, double
    /// * String
    /// * DateTime
    /// * Enum
    /// * List of enum
    /// 
    /// Any of the data types can also be nullable which is a good way to model that the field does not have a value.
    /// If a field is based on an enum and it is not nullable, then the 0 value in the enum is considered to be null and you should start your enumeration at 1.
    /// Any other fields, properties or methods are ignored by the form code.
    /// It is also possible to define a form directly by implementing Advanced.IField or using Form.Advanced.Field and populating the dictionaries within it. 
    /// In order to better understand Form Flow and its capabilities we will work through two examples, \ref simplesandwichbot where 
    /// everything is automatically generated and \ref annotatedSandwich where the form is extensively customized.
    /// 
    /// \section simplesandwichbot Simple Sandwich Bot
    /// As an example of %Form Flow in action, this will outline a simple sandwich ordering form that we will elaborate
    /// to show various features.  To start with the form stuff you need to create a C# class to represent your form.  Like this:
    /// \include SimpleSandwichBot/sandwich.cs
    /// 
    /// In order to connect your form to the bot framework you need to add it to your controller like this:
    /// \dontinclude SimpleSandwichBot/Controllers/MessagesController.cs
    /// \skip Post
    /// \until }
    /// 
    /// The combination of your C# class and connecting it to the Bot Framework is enough to automatically create a conversation.  
    /// Here is an example interaction that demonstrates some of the features offered by Form Flow.  A <b>></b> symbol shows 
    /// When a response is expected from the user.
    /// ~~~{.txt}
    ///  Please select a sandwich
    ///  1. BLT
    ///  2. Black Forest Ham
    ///  3. Buffalo Chicken
    ///  4. Chicken And Bacon Ranch Melt
    ///  5. Cold Cut Combo
    ///  6. Meatball Marinara
    ///  7. Over Roasted Chicken
    ///  8. Roast Beef
    ///  9. Rotisserie Style Chicken
    ///  10. Spicy Italian
    ///  11. Steak And Cheese
    ///  12. Sweet Onion Teriyaki
    ///  13. Tuna
    ///  14. Turkey Breast
    ///  15. Veggie
    ///  >
    /// ~~~ 
    /// 
    /// Here you can see the field SandwichOrder.Sandwich being filled in.  First off you can see the automatically generated
    /// prompt "Please select a sandwich".  The word "sandwich" came from the name of the field.  The SandwichOptions enumeration provided the 
    /// choices that make up the list.  Each enumeration was broken into words based on case and _.  
    /// 
    /// Now what are the possible responses?  If you ask for "help" you can see the possibilities like this:
    /// ~~~{.txt}
    /// > help
    /// * You are filling in the sandwich field.Possible responses:
    /// * You can enter a number 1-15 or words from the descriptions. (BLT, Black Forest Ham, Buffalo Chicken, Chicken And Bacon Ranch Melt, Cold Cut Combo, Meatball Marinara, Over Roasted Chicken, Roast Beef, Rotisserie Style Chicken, Spicy Italian, Steak And Cheese, Sweet Onion Teriyaki, Tuna, Turkey Breast, and Veggie)
    /// * Back: Go back to the previous question.
    /// * Help: Show the kinds of responses you can enter.
    /// * Quit: Quit the form without completing it.
    /// * Reset: Start over filling in the form. (With defaults of your previous entries.)
    /// * Status: Show your progress in filling in the form so far.
    /// * You can switch to another field by entering its name. (Sandwich, Length, Bread, Cheese, Toppings, and Sauce).
    /// ~~~  
    /// 
    /// The possibilities include responding with the number of the choice you want, or you can also use the words that
    /// are found in the choice descriptions.  There are also a number of commands that let you back up a step, get help, quit, start over 
    /// or get the progress so far.  Let's enter "2" to select "Black Forest Ham".
    /// ~~~{.txt}
    ///  Please select a sandwich
    ///  1. BLT
    ///  2. Black Forest Ham
    ///  3. Buffalo Chicken
    ///  4. Chicken And Bacon Ranch Melt
    ///  5. Cold Cut Combo
    ///  6. Meatball Marinara
    ///  7. Over Roasted Chicken
    ///  8. Roast Beef
    ///  9. Rotisserie Style Chicken
    ///  10. Spicy Italian
    ///  11. Steak And Cheese
    ///  12. Sweet Onion Teriyaki
    ///  13. Tuna
    ///  14. Turkey Breast
    ///  15. Veggie
    /// > 2
    /// Please select a length(1. Six Inch, 2. Foot Long)
    /// > 
    /// ~~~
    /// 
    /// Now we get the next prompt which is for the SandwichOrder.Length property.  If you wanted to back up to check
    /// on your change your sandwich type you could type 'back' like this:
    /// ~~~{.txt}
    /// > back
    /// Please select a sandwich(current choice: Black Forest Ham)
    ///  1. BLT
    ///  2. Black Forest Ham
    ///  3. Buffalo Chicken
    ///  4. Chicken And Bacon Ranch Melt
    ///  5. Cold Cut Combo
    ///  6. Meatball Marinara
    ///  7. Over Roasted Chicken
    ///  8. Roast Beef
    ///  9. Rotisserie Style Chicken
    ///  10. Spicy Italian
    ///  11. Steak And Cheese
    ///  12. Sweet Onion Teriyaki
    ///  13. Tuna
    ///  14. Turkey Breast
    ///  15. Veggie
    /// ~~~
    /// 
    /// Now we can see that we selected "Black Forest Ham" and we can continue by typing "c" to keep the current choice and
    /// "1" to select a six inch sandwich.
    /// ~~~{.txt}
    /// Please select a sandwich (current choice: Black Forest Ham)
    ///  1. BLT
    ///  2. Black Forest Ham
    ///  3. Buffalo Chicken
    ///  4. Chicken And Bacon Ranch Melt
    ///  5. Cold Cut Combo
    ///  6. Meatball Marinara
    ///  7. Over Roasted Chicken
    ///  8. Roast Beef
    ///  9. Rotisserie Style Chicken
    ///  10. Spicy Italian
    ///  11. Steak And Cheese
    ///  12. Sweet Onion Teriyaki
    ///  13. Tuna
    ///  14. Turkey Breast
    ///  15. Veggie
    /// > c
    /// Please select a length(1. Six Inch, 2. Foot Long)
    /// > 1
    /// ~~~
    /// 
    /// In addition to typing numbers and commands you can also type in words from the choices.  Here we have typed "nine grain" which 
    /// is ambiguous and Form Flow system automatically asks for clarification.
    /// ~~~{.txt}
    /// Please select a bread
    ///  1. Nine Grain Wheat
    ///  2. Nine Grain Honey Oat
    ///  3. Italian
    ///  4. Italian Herbs And Cheese
    ///  5. Flatbread
    /// > nine grain
    /// By "nine grain" bread did you mean(1. Nine Grain Honey Oat, 2. Nine Grain Wheat)
    /// > 1
    /// ~~~
    /// 
    /// What happens if you type in something which is not understood or a mixture of understood and not understood?  In the below
    /// you can see both something that is not understood at all and also a mixture of understood and not understood things.
    /// ~~~{.txt}
    /// Please select a cheese (1. American, 2. Monterey Cheddar, 3. Pepperjack)
    /// > amercan
    /// "amercan" is not a cheese option.
    /// > american smoked
    /// For cheese I understood American. "smoked" is not an option.
    /// ~~~
    /// 
    /// Some fields like SandiwchOrder.Toppings allow multiple possibilities. Here we are entering multiple possibilities and showing 
    /// another example of clarification.
    /// ~~~{.txt}
    /// Please select one or more toppings
    ///  1. Banana Peppers
    ///  2. Cucumbers
    ///  3. Green Bell Peppers
    ///  4. Jalapenos
    ///  5. Lettuce
    ///  6. Olives
    ///  7. Pickles
    ///  8. Red Onion
    ///  9. Spinach
    ///  10. Tomatoes
    /// > peppers, lettuce and tomatoe
    /// By "peppers" toppings did you mean(1. Green Bell Peppers, 2. Banana Peppers)
    /// > 1
    /// ~~~
    /// 
    /// At this point, I might wonder how much is left and I can ask about my progress so far by typing "status".  When
    /// I do so, I see my form and all that is left is the SandwichOrder.Sauce.
    /// ~~~{.txt}
    /// Please select one or more sauce
    ///  1. Honey Mustard
    ///  2. Light Mayonnaise
    ///  3. Regular Mayonnaise
    ///  4. Mustard
    ///  5. Oil
    ///  6. Pepper
    ///  7. Ranch
    ///  8. Sweet Onion
    ///  9. Vinegar
    /// > status
    /// * Sandwich: Black Forest Ham
    /// * Length: Six Inch
    /// * Bread: Nine Grain Honey Oat
    /// * Cheese: American
    /// * Toppings: Lettuce, Tomatoes, and Green Bell Peppers
    /// * Sauce: Unspecified  
    /// ~~~
    /// 
    /// I select "1" for "Honey Mustard" and I've reached the end and I'm asked to confirm my order.
    /// ~~~{.txt}
    /// Please select one or more sauce
    ///  1. Honey Mustard
    ///  2. Light Mayonnaise
    ///  3. Regular Mayonnaise
    ///  4. Mustard
    ///  5. Oil
    ///  6. Pepper
    ///  7. Ranch
    ///  8. Sweet Onion
    ///  9. Vinegar
    /// > 1
    /// Is ths your selection?
    /// * Sandwich: Black Forest Ham
    /// * Length: Six Inch
    /// * Bread: Nine Grain Honey Oat
    /// * Cheese: American
    /// * Toppings: Lettuce, Tomatoes, and Green Bell Peppers
    /// * Sauce: Honey Mustard
    /// >
    /// ~~~
    /// 
    /// If I say "no", then I get the option to change any part of the form.  In this case I change the length and then say "y"
    /// which then returns the completed form to the caller.
    /// ~~~{.txt}
    /// Is ths your selection?
    /// * Sandwich: Black Forest Ham
    /// * Length: Six Inch
    /// * Bread: Nine Grain Honey Oat
    /// * Cheese: American
    /// * Toppings: Lettuce, Tomatoes, and Green Bell Peppers
    /// * Sauce: Honey Mustard
    /// > no
    /// What do you want to change?
    ///  1. Sandwich(Black Forest Ham)
    ///  2. Length(Six Inch)
    ///  3. Bread(Nine Grain Honey Oat)
    ///  4. Cheese(American)
    ///  5. Toppings(Lettuce, Tomatoes, and Green Bell Peppers)
    ///  6. Sauce(Honey Mustard)
    /// > 2
    /// Please select a length(current choice: Six Inch) (1. Six Inch, 2. Foot Long)
    /// > 2
    /// Is ths your selection?
    /// * Sandwich: Black Forest Ham
    /// * Length: Foot Long
    /// * Bread: Nine Grain Honey Oat
    /// * Cheese: American
    /// * Toppings: Lettuce, Tomatoes, and Green Bell Peppers
    /// * Sauce: Honey Mustard
    ///
    /// > y
    /// ~~~
    /// At this point, the form is completed and will be returned to the parent dialog.  Throughout this interaction you can
    /// see that the automatically generated conversation:
    /// * Provided clear guidance and help  
    /// * Understands both numbers and textual entries  
    /// * Gives feedback on what is understood and what is not.  
    /// * Asks clarifying questions when needed.  
    /// * Allows navigating between the steps.  
    /// 
    /// All of this is pretty amazing for not having to do any of the work!  
    /// However, not every interaction was as
    /// good as you might want it to be.  That is why there are easy ways to provide:
    /// * Messages during the process of filling in a form.  
    /// * Custom prompts per field.  
    /// * Templates to use when automatically generating prompts or help.  
    /// * Terms to match on.  
    /// * Whether to show choices and numbers or not.  
    /// * Fields that are optional.  
    /// * Conditional fields.  
    /// * Value validation  
    /// * and much more...
    /// 
    /// The next example shows how to improve the sandwich bot.
    ///
    /// \section annotatedSandwich Annotated Sandwich Bot
    /// This example builds on the previous one by:
    /// * Adding  attributes to add descriptions, terms, prompts and templates.  
    /// * Switching from fields to properties to incorporate business logic.  
    /// * Adding messages, flow control and nice confirmations.  
    /// 
    /// \subsection attributes Attributes
    /// Form Flow includes some attributes you can add to your class to better control the dialog.  Here are the five attributes:
    /// 
    /// Attribute | Purpose
    /// ----------| -------
    /// Describe | Change how a field or a value is shown in text.
    /// Numeric | Provide limits on the values accepted in a numeric field.
    /// Optional | Mark a field as optional which means that one choice is not to supply a value.
    /// Prompt | Define a prompt to use when asking for a field.
    /// Template | Define a template that is used to generate prompts or values in prompts.
    /// Terms | Define the input terms that match a field or value.
    /// 
    /// Let's look at how these attributes can improve your Bot.  First off, we might want to change the the prompt for SandwichOrder.Sandwich from the
    /// automatically generated "Please select a sandwich" to "What kind of sandwich would you like?". To do this we would use the Prompt attribute like this:
    /// \dontinclude AnnotatedSandwichBot/AnnotatedSandwich.cs
    /// \skip Prompt
    /// \until ]
    /// \skip SandwichOptions
    /// \until ;
    /// 
    /// One thing you will notice is the the prompt contains the funny strings "{&}" and "{||}".  
    /// That is because prompt and message strings are written in \ref patterns where you can
    /// have parts of the string that are filled in when the actual prompt or message is generated.  
    /// In this case "{&}" means fill in the description of the field and "{||}" means 
    /// show the choices that are possible.  The description of a field is automatically generated from the field name, 
    /// but you could also use a Describe attribute to override that.  
    /// By adding this attribute, the prompt for SandwichOrder.Sandwich now looks like:
    /// ~~~{.txt}
    /// What kind of sandwich would you like?
    /// 1. BLT
    /// 2. Black Forest Ham
    /// 3. Buffalo Chicken
    /// 4. Chicken And Bacon Ranch Melt
    /// 5. Cold Cut Combo
    /// 6. Meatball Marinara
    /// 7. Over Roasted Chicken
    /// 8. Roast Beef
    /// 9. Rotisserie Style Chicken
    /// 10. Spicy Italian
    /// 11. Steak And Cheese
    /// 12. Sweet Onion Teriyaki
    /// 13. Tuna
    /// 14. Turkey Breast
    /// 15. Veggie
    /// >
    /// ~~~
    /// 
    /// There are lots of things you can control when specifiying a prompt.  For example with this prompt attribute:
    /// ~~~
    /// [Prompt("What kind of {&} would you like? {||}", ChoiceFormat="{1}")]
    /// ~~~
    /// The prompt would now no longer show numbers and the only accepted input would be words from the choices.
    /// ~~~{.txt}
    /// What kind of sandwich would you like?
    /// * BLT
    /// * Black Forest Ham
    /// * Buffalo Chicken
    /// * Chicken And Bacon Ranch Melt
    /// * Cold Cut Combo
    /// * Meatball Marinara
    /// * Over Roasted Chicken
    /// * Roast Beef
    /// * Rotisserie Style Chicken
    /// * Spicy Italian
    /// * Steak And Cheese
    /// * Sweet Onion Teriyaki
    /// * Tuna
    /// * Turkey Breast
    /// * Veggie
    /// >
    /// ~~~
    /// 
    /// Going a step further, you could decide not to show the choices at all like this:
    /// ~~~
    /// [Prompt("What kind of {&} would you like?")]
    /// ~~~
    /// The prompt then would not show the choices at all, but they would still be available in help.
    /// ~~~{.txt}
    /// What kind of sandwich would you like?
    /// > ?
    /// * You are filling in the sandwich field.Possible responses:
    /// * You can enter in any words from the descriptions. (BLT, Black Forest Ham, Buffalo Chicken, Chicken And Bacon Ranch Melt, Cold Cut Combo, Meatball Marinara, Over Roasted Chicken, Roast Beef, Rotisserie Style Chicken, Spicy Italian, Steak And Cheese, Sweet Onion Teriyaki, Tuna, Turkey Breast, and Veggie)
    /// * Back: Go back to the previous question.
    /// * Help: Show the kinds of responses you can enter.
    /// * Quit: Quit the form without completing it.
    /// * Reset: Start over filling in the form. (With defaults of your previous entries.)
    /// * Status: Show your progress in filling in the form so far.
    /// * You can switch to another field by entering its name. (Sandwich, Length, Bread, Cheese, Sauces, and Toppings).
    /// ~~~
    /// 
    /// It was great if you wanted to replace just one prompt, but you can also replace
    /// the templates that are used for autommatically generating your prompts.  Here we have redefined
    /// the default template used when you want to select one results from a set of choices to a different string and asked choices to always
    /// be listed one per line.  
    /// \dontinclude AnnotatedSandwichBot/AnnotatedSandwich.cs
    /// \skip EnumSelectOne
    /// \until class
    /// With this change, here is how the cheese and bread prompts look.
    /// ~~~{.txt}
    /// What kind of bread would you like on your sandwich?
    ///  1. Nine Grain Wheat
    ///  2. Nine Grain Honey Oat
    ///  3. Italian
    ///  4. Italian Herbs And Cheese
    ///  5. Flatbread
    /// >
    /// What kind of cheese would you like on your sandwich? 
    ///  1. American
    ///  2. Monterey Cheddar
    ///  3. Pepperjack
    /// > 
    /// ~~~
    /// As you can see, both used the new template.
    /// 
    /// Now, there is still a problem with the SandwichOrder.Cheese field--how does someone indicate that they do not want cheese at all?
    /// One option would be to add an explicit NoCheese value to your enumeration, but a second option is to mark the field as optional using the
    /// Optional attribute.  An optional field has a "special" no preference value.  Below is an example of the Optional attribute.
    /// \dontinclude AnnotatedSandwichBot/AnnotatedSandwich.cs
    /// \skip Optional
    /// \until CheeseOptions
    /// With this change the cheese prompt now looks like this:
    /// ~~~{.txt}
    ///What kind of cheese would you like on your sandwich? (current choice: No Preference)
    ///  1. American
    ///  2. Monterey Cheddar
    ///  3. Pepperjack
    /// >
    /// ~~~
    /// And if you have a current value it looks like this:
    /// ~~~{.txt}
    /// What kind of cheese would you like on your sandwich? (current choice: American)
    ///  1. American
    ///  2. Monterey Cheddar
    ///  3. Pepperjack
    ///  4. No Preference
    /// >
    /// ~~~
    /// 
    /// One way to interject some variation in the prompts and messages you generate is to define multiple ]ref patterns patterns to 
    /// randomly select between. Here we have redefined the TemplateUsage.NotUnderstood template so that there are two patterns for how
    /// to respond to unknown text.
    /// \dontinclude AnnotatedSandwichBot/AnnotatedSandwich.cs
    /// \skip NotUnderstood
    /// \until class
    /// Now when the user types something that is not understood, one of the two patterns will be randomly selected like this:
    /// ~~~{.txt}
    /// What size of sandwich do you want? (1. Six Inch, 2. Foot Long)
    /// > two feet
    /// I do not understand "two feet".
    /// > two feet
    /// Try again, I don't get "two feet"
    /// > 
    /// ~~~
    /// 
    /// One of the things you can override are the terms used to match user input to a field or a value in a field.  When matching user input,
    /// terms are used to identify possible meanings for what was typed.  
    /// 
    /// By default, terms are generated by taking the field or value name and following these steps:
    /// * Break on case changes and _.  
    /// * Generate each n-gram up to a maximum length.
    /// * Add s? to the end to support plurals.  
    /// So for example, the value AngusBeefAndGarlicPizza would generate: 'angus?', 'beefs?', 'garlics?', 'pizzas?', 'angus? beefs?', 'garlics? pizzas?' and 'angus beef and garlic pizza'.
    /// The word "rotisserie" is one that is highly likely to be misspelled so here we have used a regular expression to make it more likely
    /// we will match what the user types.  Because we specify Terms.MaxPhrase, Language.GenerateTerms will also generate variations for us.
    /// 
    /// \dontinclude AnnotatedSandwichBot/AnnotatedSandwich.cs
    /// \skip Terms
    /// \until Rotisserie
    /// 
    /// Given the terms now we can match input like this.
    /// ~~~{.txt}
    /// What kind of sandwich would you like?
    ///  1. BLT
    ///  2. Black Forest Ham
    ///  3. Buffalo Chicken
    ///  4. Chicken And Bacon Ranch Melt
    ///  5. Cold Cut Combo
    ///  6. Meatball Marinara
    ///  7. Over Roasted Chicken
    ///  8. Roast Beef
    ///  9. Rotisserie Style Chicken
    ///  10. Spicy Italian
    ///  11. Steak And Cheese
    ///  12. Sweet Onion Teriyaki
    ///  13. Tuna
    ///  14. Turkey Breast
    ///  15. Veggie
    /// > rotissary chechen
    /// For sandwich I understood Rotisserie Style Chicken. "chechen" is not an option.
    /// ~~~
    /// 
    /// \subsection logic Adding Business Logic
    /// Sometimes there are complex interdependencies between fields or you need to 
    /// add logic to setting or getting a value.  In this example we want to add support
    /// for including all toppings except some of them.  To do this, we change toppings
    /// from a property to a field and add some logic to complement the selected toppings.
    /// 
    /// \dontinclude AnnotatedSandwichBot/AnnotatedSandwich.cs
    /// \skip List<ToppingOptions>
    /// \until private
    ///
    /// In addition to the processing we also need to add some terms to match expressions like
    /// "everything" and "not".  
    /// \dontinclude AnnotatedSandwichBot/AnnotatedSandwich.cs
    /// \skip ToppingOptions
    /// \until AllExcept
    ///
    /// Here is what the interaction with toppings looks like:
    /// ~~~{.txt}
    /// ~~~
    /// 
    /// Here is the SandwichOrder with attributes added and some business logic.
    /// \include AnnotatedSandwichBot/sandwich.cs
    /// 
    /// \subsection ControlFlow Controlling Flow
    /// 
    /// \section patterns Pattern Language
    /// One of the keys to creating a Bot is being able to generate text that is clear and
    /// meaningful to the bot user.  This framework supports a pattern language with  
    /// elements that can be filled in at runtime.  Everything in a pattern that is not surrounded by curly braces
    /// is just passed straight through.  Anything in curly braces is substitued with values to make a string that can be
    /// shown to the user. Once substitution is done, some additional processing to remove double spaces and
    /// use the proper form of a/an is also done.
    /// 
    /// Possible curly brace pattern elements are outline in the table below.  Within a pattern element, "<field>" refers to the  path within your form class to get
    /// to the field value.  So if I had a class with a field named "Size" you would refer to the size value with the pattern element {Size}.  
    /// "..." within a pattern element means multiple elements are allowed.  "<format>" within a pattern element means that you
    /// can optionally specify a regular C# format specifier, i.e. if "Rating" were a double field I could show it with
    /// two digits of precision by using the pattern element "{Rating:F2}".  "<n>" shows where you can specify a reference to the nth
    /// argument of a template.  (See TemplateUsage to see what arguments each template can use.)
    /// 
    /// Pattern Element | Description
    /// --------------- | -----------
    /// {<format>} | Value of the current field.
    /// {&} | Description of the current field.
    /// {<field><format>} | Value of a particular field. 
    /// {&<field>} | Description of a particular field.
    /// {\|\|} | Show the current choices for enumerated fields.
    /// {[{<field><format>} ...]} | Create a list with all field values together utilizing Form.TemplateBase.Separator and Form.TemplateBase.LastSeparator to separate the individual values.
    /// {*} | Show one line for each active field with the description and current value.
    /// {*filled} | Show one line for each active field that has an actual value with the description and current value.
    /// {<nth><format>} | A regular C# format specifier that refers to the nth arg.  See Form.TemplateUsage to see what args are available.
    /// {?<textOrPatternElement>...} | Conditional substitution.  If all referred to pattern elements have values, the values are substituted and the whole expression is used.
    ///
    /// Patterns are used in Form.Prompt and Form.Template annotations.  
    /// Form.Prompt defines a prompt to the user for a particular field or confirmation.  
    /// Form.Template is used to automatically construct prompts and other things like help.
    /// There is a built-in set of templates defined in Form.FormConfiguration.Templates.
    /// A good way to see examples of the pattern language is to look at the templates defined there.
    /// A Form.Prompt can be specified by annotating a particular field or property or implicitly defined through Form.IField<T>.Field.
    /// A default Form.Template can be overridden on a class or field basis.  
    /// Both prompts and templates support the formatting parameters outlined below.
    /// 
    /// Usage | Description
    /// ------|------------
    /// Form.TemplateBase.AllowDefault | When processing choices using {\|\|} controls whether the current value should be showed as a choice.
    /// Form.TemplateBase.AllowNumbers | When processing choices using {\|\|} controls whether or not you can enter numbers for choices. If set to false, you should also set Form.TemplateBase.ChoiceFormat.
    /// Form.TemplateBase.ChoiceFormat | When processing choices using {\|\|} controls how each choice is formatted. {0} is the choice number and {1} the choice description.
    /// Form.TemplateBase.ChoiceStyle | When processing choices using {\|\|} controls whether the choices are presented in line or per line.
    /// Form.TemplateBase.Feedback | For Form.Prompt only controls feedback after user entry.
    /// Form.TemplateBase.FieldCase | Controls case normalization when displaying a field description.
    /// Form.TemplateBase.LastSeparator | When lists are constructed for {[]} or in line choices from {\|\|} provides the separator before the last item.
    /// Form.TemplateBase.Separator | When lists are constructed for {[]} or in line choices from {\|\|} provides the separator before every item except the last.
    /// Form.TemplateBase.ValueCase | Controls case normalization when displaying a field value.
    /// 
}