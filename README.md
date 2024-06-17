# GameLeaderboard
##### By Simonas Sabaliauskas
This is the final project for the C# course from Vilnius Coding School, started on 2024-05-15.
The program is far from completed, but for the task it should be sufficient.
Ideally it should be tested more and added to a website.
I plan to use this system for a 2D game I am making with Unity (draft name: Dimensional Adventure).
The website could contain the game itself.

## Project Structure

### LeaderboardBackEnd
Relational database: MSSQL. Manipulations using Entity Framework.
Non-relational (No-SQL) database (cache) MongoDB. Manipulations using MongoDB.Driver.

#### Contracts
##### ICreationService
##### IDatabaseRepository
##### ILeaderboardService
##### IMongoDBRepository

#### Databases
##### LeaderboardDBContext
For Entity Framework.

#### Enums
##### MongoDBCollectionName
For choosing only from existing MongoDB collections.
##### RandomUsername
For creating test data quickly.

#### Migrations
Entity Framework automatically generated table information from Models objects.
Invoked by console commands:
##### "dotnet EF migrations add InitialCreate"
##### "dotnet "dotnet EF database update"

#### Models
##### Level
The simplest of the three, having just ID and MaxScore.
##### Player
Holding ID, Username, TimePlayed, CreationDateTime and non-encrypted potentially sensitive data (Password and Email).
Future updates will implement encryption/decryption.
##### Score
Has ID, Points, Time, CreationDateTime.
Connects Levels and Players via PlayerID and LevelID.

#### Repositories
##### DatabaseRepository
Access to MSSQL database.
##### MongoDBRepository
Access to MongoDB cache.

#### Services
##### CreationService
For education purposes a couple of functions were separated from the main service, adding a bit of organization.
##### LeaderboardService
The main service for all actions the API can perform.

### LeaderboardAPI
API template: Swagger
#### Controllers
##### LeaderboardServiceController
Created endpoints for working with LeaderboardService.

### LeaderboardTests
Testsfor functions of the primary service class LeaderboardService.