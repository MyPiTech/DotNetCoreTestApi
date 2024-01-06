# TestApi
Thank you for taking the time to explore this repository. 

As stated in the Swagger UI located [here](https://swcoretestapi.azurewebsites.net/swagger/index.html). This code is not intended to be production-ready. There are a number of shortcuts and a certain lack of adherence to architectural principles. In my defense, this API is intended more to show my C# skill set rather than my understanding of application architecture. It will be used as the backend for a number of potential frontend tech stacks. The purpose of which is to keep my skill set current and to improve my skills as I pursue employment opportunities.

That said, I think it might be important to give an overview of what I did and why.

### Basic Tech Stack
- Target Framework: .NET 6.0 (Core)
- MSSQL \- Originally PostgreSQL
- IDE \- Microsoft Visual Studio Community 2022 
- ORM \- Entity Framework Core (7.0.11)
- Azure \- SQL server, database and app service for publishing
- Git \- Version\\Source Control


## Things done well
All core principles of OOP including abstraction, polymorphism, inheritance and encapsulation (APIE) should have good representations in the code base. Use of generics in inheritance and polymorphism gives a good example of how these coupled with the DRY principle can improve extensibility, code reuse and best practices. IOC is used consistently both to maintain loose coupling and to adhere to the explicit dependency principle. The async multithreading pattern is used and extended to EF's asynchronous methods to protect performance. 

## Things to do better
Were this intended as a production application it would include a few things I left out. From an archtectual standpoint this is a 2-tiered application. The request\\response layer and a service layer. Normally I would have included a dedicated data access layer as well, but given the limited scope of this app I simply combined the service and DAL layers. Not the best approach for separation of concerns, persistence ignorance, or single responsibility adherence. 

From a design standpoint, the first thing missing is authentication and authorization control. While leaving this out does simplify the implementation. I mainly wanted the API to be accessible to the public. So it seemed like an unnecessary complication. Again, given the scope and intended use of the API.

Internationalization and localization (I18n, L10n) are also design concerns in most production applications these days. APIs are often but not always immune to these concerns. I intentionally ignored this here as unnecessary. 

Developer documentation was limited to the needs of Swagger. Ideally, every method and property would be documented not just the ones that impact the Swagger UI. In addition, a robust unit testing implementation is ideal but was left out here.

Error handling while present is not quite up to my standards. In particular, post-model state validation is lacking. I will likely improve on error handling  as the frontends become dependent on the API, and it becomes a more immediate concern.

Explicitly defining database transactions around insert, update, and delete operations is often necessary in a production implementations. EF does a fairly good job of handling this in simple cases, and it seemed unnecessary here. 

Logging could be improved.
