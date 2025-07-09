# User Management Technical Exercise

The exercise is an ASP.NET Core web application backed by Entity Framework Core, which faciliates management of some fictional users.
We recommend that you use [Visual Studio (Community Edition)](https://visualstudio.microsoft.com/downloads) or [Visual Studio Code](https://code.visualstudio.com/Download) to run and modify the application. 

**The application uses an in-memory database, so changes will not be persisted between executions.**

## The Exercise
Complete as many of the tasks below as you feel comfortable with. These are split into 4 levels of difficulty 
* **Standard** - Functionality that is common when working as a web developer
* **Advanced** - Slightly more technical tasks and problem solving
* **Expert** - Tasks with a higher level of problem solving and architecture needed
* **Platform** - Tasks with a focus on infrastructure and scaleability, rather than application development.

### 1. Filters Section (Standard)

The users page contains 3 buttons below the user listing - **Show All**, **Active Only** and **Non Active**. Show All has already been implemented. Implement the remaining buttons using the following logic:
* Active Only – This should show only users where their `IsActive` property is set to `true`
* Non Active – This should show only users where their `IsActive` property is set to `false`

### 2. User Model Properties (Standard)

Add a new property to the `User` class in the system called `DateOfBirth` which is to be used and displayed in relevant sections of the app.

### 3. Actions Section (Standard)

Create the code and UI flows for the following actions
* **Add** – A screen that allows you to create a new user and return to the list
* **View** - A screen that displays the information about a user
* **Edit** – A screen that allows you to edit a selected user from the list  
* **Delete** – A screen that allows you to delete a selected user from the list

Each of these screens should contain appropriate data validation, which is communicated to the end user.

### 4. Data Logging (Advanced)

Extend the system to capture log information regarding primary actions performed on each user in the app.
* In the **View** screen there should be a list of all actions that have been performed against that user. 
* There should be a new **Logs** page, containing a list of log entries across the application.
* In the Logs page, the user should be able to click into each entry to see more detail about it.
* In the Logs page, think about how you can provide a good user experience - even when there are many log entries.

### 5. Extend the Application (Expert)

Make a significant architectural change that improves the application.
Structurally, the user management application is very simple, and there are many ways it can be made more maintainable, scalable or testable.
Some ideas are:
* Re-implement the UI using a client side framework connecting to an API. Use of Blazor is preferred, but if you are more familiar with other frameworks, feel free to use them.
* Update the data access layer to support asynchronous operations.
* Implement authentication and login based on the users being stored.
* Implement bundling of static assets.
* Update the data access layer to use a real database, and implement database schema migrations.

### 6. Future-Proof the Application (Platform)

Add additional layers to the application that will ensure that it is scaleable with many users or developers. For example:
* Add CI pipelines to run tests and build the application.
* Add CD pipelines to deploy the application to cloud infrastructure.
* Add IaC to support easy deployment to new environments.
* Introduce a message bus and/or worker to handle long-running operations.

## Additional Notes

* Please feel free to change or refactor any code that has been supplied within the solution and think about clean maintainable code and architecture when extending the project.
* If any additional packages, tools or setup are required to run your completed version, please document these thoroughly.

#####################################################################################################################################

* #### User Test Submission
* # Inflo User Management Application

This is a full-stack user management system developed for the Inflo technical test. It includes two implementations based on the tasks set out:

1. **Initial version** built with ASP.NET MVC and Razor Pages (Tasks 1-4)
2. **Expert version** restructured using Blazor WebAssembly and a Web API backend (Task 5)

---

## Technologies Used

- **ASP.NET MVC (Initial Build)**
- **Blazor WebAssembly + ASP.NET Web API (Expert Build)**
- Entity Framework Core (In-Memory)
- Bootstrap, Custom CSS, Google Fonts (Poppins)
- xUnit (Testing)
- GitHub for version control

---

## How to Run the Application

### 1. Original Razor Pages Version (MVC-style)

To run the initial implementation:

- Open the solution in Visual Studio
- Set the **`UserManagement.Web`** project as the startup project
- Press `F5` to run

**Functionality Available:**
- User listing
- Create, edit, delete users
- View user detail
- Full action logging with dashboard and filters
- Unit tests for controllers, services, and logging

---

### 2. Expert-Level Rebuild (Blazor + Web API)

The second part of the project involved a significant architectural change — replacing Razor Pages with a **Blazor WebAssembly frontend** that communicates with a **separate Web API backend**.

To run this version:

- Set both of these projects to start:
  - `UserManagement.API`
  - `UserManagement.Blazor`
- Press `F5` to run the solution
- The Blazor frontend will launch and connect to the API

**Functionality Available:**
- Styled in line with Inflo branding
- Navigation menu with icons and animation
- Table hover effects and status badges
- API endpoints tested and verified

---

## Styling and Branding

The application has been styled to reflect **Inflo’s platform branding**:

- Custom logo placement in the sidebar
- Navigation hover effects with smooth transitions
- Responsive table and form layouts
- Colour palette derived from Inflo’s brand site
- Font: [Poppins](https://fonts.google.com/specimen/Poppins)

---

## Testing

Unit tests have been created for:

- User services
- User controller logic
- Logging functionality

Run all tests via:

```bash
dotnet test

##  Completed Tasks Summary
Task	Description
- 1	Setup of MVC app with user CRUD (Standard)
- 2	Logging with full filters and dashboard (Standard)
- 3	Unit testing across services and controller (Standard)
- 4	Logging user actions and separate dashboard metrics (Advanced)
- 5	Rebuild using Blazor WebAssembly and Web API (Expert)

## About
This project was created as part of a technical evaluation. It demonstrates:

Clean architectural practices

Modern .NET front and back end development

Reusable services, DTOs, and data models

Fully tested and accessible UI

Built by Nadia Chinnery, 2025.

