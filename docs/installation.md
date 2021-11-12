# Cash Flow - Instalation
[GoBack](../README.md)  

## Get started
- **Clone this repo** to your local machine using 
   ``` shell
   git clone https://github.com/LeversonCarlos/FriendlyCashFlow.git
   ```
- Install **client packages** with 
   ``` shell 
   npm install ./srcs/ClientApp
   ```
- **Build** the backend project with 
   ``` shell 
   dotnet build ./srcs
   ```
- **Run the application** locally with 
   ``` shell
   dotnet run --project ./srcs
   ```

## Customize application settings
There is a file on the **srcs folder** called **appsettings.json**. On this file some behaviours of the application is defined.
- **BaseHost**: Path used to compose urls when the application is hosted somewhere
- **ConnStr**: The connection string used to connect to the database used by the application
- **Passwords**: Section defining the requirements for the passwords set by the users
- **Token**: Section defining the tokens that will be used to authenticate users on his calls to the backend apis
- **Mail**: Section defining the SMTP server used to send mails to the users
- **AppInsights**: Section where an azure's application insights could be set to monitor app usage

