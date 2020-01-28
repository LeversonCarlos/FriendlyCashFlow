# Friendly Cash Flow

<img src="./resources/icon/icon.png" title="Application Icon" alt="Application Icon" align="right" />

A straightforward cash flow web application. Set your accounts and categories and then register your incomes and expenses. You will be able follow the flow of your cash through the months, see where it goes and where you could save.

## History goes like this

<a href="./resources/demo/10-mobile.gif">
   <img src="./resources/demo/10-mobile.gif" title="Mobile Demo" alt="Mobile Demo" align="left" width="175" height="292" style="margin-right:5px" />
</a>

This is my **particular** cash flow system. I'm always trying some of the market's application for cash flow. And, as a developer, i always come back to my own implementation.

This is the one app that give me exactly what i want from this kind of application. And if there something missing or broken, i can fix it. Besides that, i'm always using this project as my personal case for **study, learning and practicing** new technologies. 

My endeavour started back on early 2000 with `vb6`, passed through `vb.net` and then `c#`. Started as a desktop app, turned into web site (with `aspnet`) and now a progressive web application (with `aspnet core` and `angular`).

Ever since, my sources were on cloud drivers and **private repositories**. This time i thought will be cool to **open the sources** on a public repository and, who knows, maybe somebody like-it, maybe somebody fix that anoying bug, or even somebody enhance something.

So, feel free to clone-it and make-it better.


[![Backend](https://img.shields.io/:Backend-AspNet%20Core-yellow.svg?style=flat)](https://docs.microsoft.com/aspnet)
[![Frontend](https://img.shields.io/:Frontend-Angular-yellow.svg?style=flat)](https://angular.io)
[![UI](https://img.shields.io/:UI-Material%20Design-yellow.svg?style=flat)](https://material.angular.io) 
[![License](https://img.shields.io/:License-MIT-blue.svg?style=flat)](http://badges.mit-license.org)
<!-- shields.io -->

[![Build Status](https://dev.azure.com/lcjohnny/Playground/_apis/build/status/Cash%20Flow?branchName=v6)](https://dev.azure.com/lcjohnny/Playground/_build/latest?definitionId=11)

## Installation

### Get started
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
- Prepare the **database structure** with 
   ``` shell 
   dotnet ef database update --project ./srcs
   ```
- **Run the application** locally with 
   ``` shell
   dotnet run --project ./srcs
   ```

### Customize application settings
There is a file on the **srcs folder** called **appsettings.json**. On this file some behaviours of the application is defined.
- **BaseHost**: Path used to compose urls when the application is hosted somewhere
- **ConnStr**: The connection string used to connect to the database used by the application
- **Passwords**: Section defining the requirements for the passwords set by the users
- **Token**: Section defining the tokens that will be used to authenticate users on his calls to the backend apis
- **Mail**: Section defining the SMTP server used to send mails to the users
- **AppInsights**: Section where an azure's application insights could be set to monitor app usage

## Try it out
I put a test version online that can be accessed [here](https://friendlycashflow-dev.azurewebsites.net/). You can report bugs [here](https://github.com/LeversonCarlos/FriendlyCashFlow/issues).

## Usage

### Register and access the application
<a href="./resources/demo/01-register.gif"><img src="./resources/demo/01-register.gif" title="Register" width="250" height="170" style="margin:5px" /></a>
<a href="./resources/demo/02-activate.gif"><img src="./resources/demo/02-activate.gif" title="Activate" width="250" height="170" style="margin:5px" /></a>
<a href="./resources/demo/03-login.gif"><img src="./resources/demo/03-login.gif" title="Login" width="250" height="170" style="margin:5px" /></a>

### You cant register entries without domain data
<a href="./resources/demo/04-empty-domains.gif"><img src="./resources/demo/04-empty-domains.gif" title="No domain data" width="250" height="170" style="margin:5px" /></a>

### Defining your domain data
<a href="./resources/demo/05-accounts.gif"><img src="./resources/demo/05-accounts.gif" title="Defining some accounts" width="250" height="170" style="margin:5px" /></a>
<a href="./resources/demo/06-categories.gif"><img src="./resources/demo/06-categories.gif" title="Defining some categories" width="250" height="170" style="margin:5px" /></a>

### Registering some income entries
<a href="./resources/demo/07-income.gif"><img src="./resources/demo/07-income.gif" title="Income" width="350" height="240" style="margin:5px" /></a>

### Registering some transfers
<a href="./resources/demo/08-transfer.gif"><img src="./resources/demo/08-transfer.gif" title="Transfers" width="350" height="240" style="margin:5px" /></a>

### Registering some expense entries
<a href="./resources/demo/09-expenses.gif"><img src="./resources/demo/09-expenses.gif" title="Expenses" width="350" height="240" style="margin:5px" /></a>


## Built With

* [Aspnet Core](https://docs.microsoft.com/aspnet)
* [Angular](https://angular.io)
* [Material Design](https://material.angular.io)

## Authors

* [Leverson Carlos](https://github.com/LeversonCarlos)
* [other contributors](https://github.com/LeversonCarlos/FriendlyCashFlow/graphs/contributors)

## License

MIT License - see the [LICENSE](LICENSE) file for details
