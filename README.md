# TestApi
Thank you for taking the time to explore this repository. 

As stated in the Swagger UI located <a href="https://swcoretestapi.azurewebsites.net/swagger/index.html">here</a>. This code is not intended to be production-ready. There are a number of shortcuts and a certain lack of adherence to architectural principles. In my defense, this API is intended more to show my C# skill set rather than my understanding of application architecture. It will be used as the backend for a number of potential frontend tech stacks. The purpose of which is to improve my skills and keep them current as I pursue employment opportunities.

That said, I think it might be important to give an overview of what I did and why.

### Basic Tech Stack
- Target Framework: .NET 6.0 (Core)
- MSSQL \- Originally PostgreSQL
- IDE \- Microsoft Visual Studio Community 2022 
- ORM \- Entity Framework Core (7.0.11)
- Azure \- SQL server, database and app service for publishing
- Git \- Version\\Source Control


## Things done well
All core principles of OOP including abstraction, polymorphism, inheritance and encapsulation (APIE) should have good representations in the code base. 

Use of generics improves extensibility, and code reuse. 

Dependancy injection adheres to IOC. 

The task-based asynchronous pattern (TAP) is used consistently. 

## Things to do better
From an archtectual standpoint this is a tiered application. The request\\response (UI) layer and a service layer (BLL). Normally I would have included a dedicated data access layer as well, but given the limited scope of this app I simply combined the BLL and DAL layers.

Authentication and authorization is nonexistent. I wanted the API to be accessible to the public. So it seemed like an unnecessary complication.

Internationalization and localization (I18n, L10n) are also design concerns. I intentionally ignored this here as unnecessary. 

Code comments were limited to the needs of Swagger. Ideally, every method would be commented not just the ones that impact the Swagger UI. In addition, a robust unit testing framework is ideal but was left out here.

Error handling while present is not quite up to my standards. In particular, post model state validation is lacking. I will likely improve on error handling as the frontends become dependent on the API, and it becomes a more immediate concern.

Logging could be improved.
