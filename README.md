# Clean Architecture
Template to implement CLEAN architecture in .NET 9

## Key Rules
	1. Domain has NO dependencies
	2. Application only depends on Domain
	3. Infrastructure and Persistence depend on Application + Domain
	4. API depends on Application. Also referencens Infrastructure and Persistence for DI
	5. Tests can reference any layer they need to test