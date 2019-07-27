MERGE INTO v5_sysTranslation AS Target 

USING (VALUES 

   /* AUTH */
   ('en-US', 'TITLE_AUTH_LOGIN', 'Authentication'),
   ('en-US', 'TITLE_AUTH_REGISTER', 'Access Register'),
   ('en-US', 'TITLE_AUTH_PASSWORD', 'Password Change'),
   ('en-US', 'TITLE_AUTH_REGISTER_RESULT', 'Welcome'),
   ('en-US', 'MENU_AUTH_PASSWORD', 'Password Change'),
   ('en-US', 'MENU_AUTH_LOGOUT', 'Logout'),
   ('en-US', 'LABEL_AUTH_EMAIL', 'Email'),
   ('en-US', 'LABEL_AUTH_USERNAME', 'Username'),
   ('en-US', 'LABEL_AUTH_PASSWORD', 'Password'),
   ('en-US', 'LABEL_AUTH_PASSWORD_CURRENT', 'Current Password'),
   ('en-US', 'LABEL_AUTH_PASSWORD_NEW', 'New Password'),
   ('en-US', 'LABEL_AUTH_PASSWORD_CONFIRMATION', 'Confirm Password'),
   ('en-US', 'LABEL_AUTH_ROLE', 'Role'),
   ('en-US', 'MSG_AUTH_EMAIL_REQUIRED', 'Enter your email'),
   ('en-US', 'MSG_AUTH_USERNAME_REQUIRED', 'Enter your username'),
   ('en-US', 'MSG_AUTH_PASSWORD_REQUIRED', 'Enter your password'),
   ('en-US', 'MSG_AUTH_PASSWORD_CURRENT_REQUIRED', 'Enter your current password'),
   ('en-US', 'MSG_AUTH_PASSWORD_NEW_REQUIRED', 'Enter your new password'),
   ('en-US', 'MSG_AUTH_PASSWORD_CONFIRMATION_REQUIRED', 'Confirm your password'),
   ('en-US', 'MSG_AUTH_ROLE_REQUIRED', 'Enter the role''s description'),
   ('en-US', 'MSG_AUTH_EMAIL_INVALID', 'Enter a valid email address'),
   ('en-US', 'MSG_AUTH_USERNAME_INVALID', 'Use at least 3 characters for your username'),
   ('en-US', 'MSG_AUTH_PASSWORD_INVALID', 'Use at least 6 characters for your password including numbers, symbols and letters'),
   ('en-US', 'MSG_AUTH_PASSWORD_CONFIRMATION_INVALID', 'Your password and confirmation doesn''t match'),
   ('en-US', 'MSG_AUTH_ROLE_INVALID', 'Use at least 2 characters for role''s description'),
   ('en-US', 'MSG_AUTH_INVALID_LOGIN', 'Invalid authentication'),
   ('en-US', 'MSG_AUTH_REGISTER_RESULT', '<p>We are nearly finished.</p> <p>Make confirmation of your registration through the instructions sent to you in your email.</p>'),
   ('en-US', 'LABEL_AUTH_LOGIN', 'Login'),
   ('en-US', 'LINK_AUTH_REGISTER_RESULT_LOGIN', 'Click to login after you have confirmed your registration'),
   ('en-US', 'MSG_AUTH_LOGIN', 'Click if you already have access to the system'),
   ('en-US', 'MSG_AUTH_REGISTER', 'Click to register yourself on the system'),
   ('en-US', 'MSG_AUTH_RECOVER', 'Click to recover your password'),

   /* ACCOUNT */
   ('en-US', 'TITLE_ACCOUNTS_MAIN', 'Accounts'),
   ('en-US', 'TITLE_ACCOUNTS_MAIN_ACTIVE', 'Active Accounts'),
   ('en-US', 'TITLE_ACCOUNTS_MAIN_ALL', 'All Accounts'),
   ('en-US', 'TITLE_ACCOUNTS_GENERAL', 'General'),
   ('en-US', 'TITLE_ACCOUNTS_BANK', 'Bank'),
   ('en-US', 'TITLE_ACCOUNTS_CREDITCARD', 'Credit Card'),
   ('en-US', 'TITLE_ACCOUNTS_INVESTMENT', 'Investment'),
   ('en-US', 'TITLE_ACCOUNTS_SERVICE', 'Service'),
   ('en-US', 'TITLE_ACCOUNTS_EDITION', 'Edition of account details'),
   ('en-US', 'LABEL_ACCOUNTS_IDACCOUNT', 'Account'),
   ('en-US', 'LABEL_ACCOUNTS_TEXT', 'Description'),
   ('en-US', 'MSG_ACCOUNTS_TEXT_REQUIRED', 'Enter the description of the account'),
   ('en-US', 'MSG_ACCOUNTS_TEXT_MAXLENGTH', 'Use 500 characters or fewer for description'),
   ('en-US', 'MSG_ACCOUNTS_TEXT_DUPLICITY', 'Your already have account with this description'),   
   ('en-US', 'LABEL_ACCOUNTS_TYPE', 'Type'),
   ('en-US', 'LABEL_ACCOUNTS_CLOSINGDAY', 'Closing Day'),
   ('en-US', 'LABEL_ACCOUNTS_DUEDAY', 'Due Day'),
   ('en-US', 'LABEL_ACCOUNTS_ACTIVE', 'Active'),      
   ('en-US', 'ENUM_ACCOUNTTYPE_GENERAL', 'General'),
   ('en-US', 'ENUM_ACCOUNTTYPE_BANK', 'Bank'),
   ('en-US', 'ENUM_ACCOUNTTYPE_CREDITCARD', 'Credit Card'),
   ('en-US', 'ENUM_ACCOUNTTYPE_INVESTMENT', 'Investment'),
   ('en-US', 'ENUM_ACCOUNTTYPE_SERVICE', 'Service'),

   /* CATEGORY */
   ('en-US', 'TITLE_CATEGORIES_MAIN', 'Categories'),
   ('en-US', 'LABEL_CATEGORIES_INCOME', 'Income'),
   ('en-US', 'LABEL_CATEGORIES_EXPENSE', 'Expense'),
   ('en-US', 'LABEL_CATEGORIES_IDCATEGORY', 'Category'),
   ('en-US', 'LABEL_CATEGORIES_TEXT', 'Description'),
   ('en-US', 'MSG_CATEGORIES_TEXT_REQUIRED', 'Enter the description of the category'),
   ('en-US', 'MSG_CATEGORIES_TEXT_MAXLENGTH', 'Use 500 characters or fewer for description'),
   ('en-US', 'MSG_CATEGORIES_TEXT_DUPLICITY', 'You already have category with this description'),   
   ('en-US', 'LABEL_CATEGORIES_TYPE', 'Type'),
   ('en-US', 'LABEL_CATEGORIES_PARENT', 'Parent Category'),
   ('en-US', 'ENUM_CATEGORYTYPE_NONE', 'None'),
   ('en-US', 'ENUM_CATEGORYTYPE_INCOME', 'Income'),
   ('en-US', 'ENUM_CATEGORYTYPE_EXPENSE', 'Expense'),

   /* ENTRY */
   ('en-US', 'LABEL_ENTRIES_IDENTRY', 'Entry'),
   ('en-US', 'LABEL_ENTRIES_TYPE', 'Type'),
   ('en-US', 'MSG_ENTRIES_IDCATEGORY_REQUIRED', 'Select the category of the entry'),
   ('en-US', 'MSG_ENTRIES_IDACCOUNT_REQUIRED', 'Select the account of the entry'),
   ('en-US', 'LABEL_ENTRIES_DUEDATE', 'Due Date'),
   ('en-US', 'MSG_ENTRIES_DUEDATE_REQUIRED', 'Enter the due date of the entry'),
   ('en-US', 'LABEL_ENTRIES_PAYDATE', 'Pay Date'),
   ('en-US', 'MSG_ENTRIES_PAYDATE_REQUIRED', 'Enter the pay date of the entry'),
   ('en-US', 'LABEL_ENTRIES_VALUE', 'Value'),
   ('en-US', 'MSG_ENTRIES_VALUE_REQUIRED', 'Enter the value of the entry'),
   ('en-US', 'MSG_ENTRIES_PAID', 'Paid'),
   ('en-US', 'MSG_ENTRIES_SORTING', 'Sorting'),
   ('en-US', 'MSG_ENTRIES_ENTRY_NOTFOUND', 'The entry wasn''t found'),

   /* COMMANDS */
   ('en-US', 'COMMAND_CANCEL', 'Cancel'),
   ('en-US', 'COMMAND_REMOVE', 'Remove'),
   ('en-US', 'COMMAND_SAVE', 'Save')
) 
AS Source (idLanguage, idTranslation, Text) 

ON Target.idLanguage = Source.idLanguage And Target.idTranslation = Source.idTranslation

-- UPDATE 
WHEN MATCHED THEN UPDATE SET Text = Source.Text

-- INSERT
WHEN NOT MATCHED BY TARGET THEN 
   INSERT (idLanguage, idTranslation, Text) 
   VALUES (idLanguage, idTranslation, Text) 

-- DELETE 
-- WHEN NOT MATCHED BY SOURCE THEN DELETE
;