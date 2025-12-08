# Sudoku Puzzle Game

A full-stack web-based Sudoku game with user authentication, game tracking, and performance analytics.

## Description
Sudoku is a logic-based number puzzle where you fill a 9x9 grid so that each row, column, and 3x3 subgrid contains all the digits from 1 to 9 without repetition. The game starts with some cells pre-filled, and players use deductive reasoning to complete the grid.

## Installation & Setup

### Prerequisites
- .NET 9.0 SDK or later
- Node.js

### Backend Setup
1. Navigate to the backend directory:
   cd back-end/Sudoku.Api

2. Restore dependencies:
   dotnet restore

3. Apply database migrations:
   dotnet ef database update

4. Run the application:
   dotnet run

   The API will be available at `http://localhost:5288`

### Frontend Setup
The frontend is served automatically by the ASP.NET Core backend as static files.

1. Ensure all frontend files are in the `front-end` directory
2. Start the backend server (see above)
3. Open your browser and navigate to `http://localhost:5288`

## API Endpoints

### Authentication
- `POST /api/auth/register` - Register a new user
- `POST /api/auth/login` - Login and receive JWT token
- `POST /api/auth/refresh` - Refresh JWT token

### Game Management
- `GET /api/games` - Get all games for authenticated user
- `GET /api/games/{id}` - Get specific game by ID
- `POST /api/games` - Save a completed game
- `PUT /api/games/{id}` - Update game progress
- `DELETE /api/games/{id}` - Delete a game

### Statistics
- `GET /api/games/stats/general` - Get aggregated statistics (public endpoint)

## Technology Stack

### Frontend
- **HTML5 & CSS3** - Game interface and styling
- **JavaScript** - Game logic and UI interactions
- **Canvas API** - Sudoku grid rendering

### Backend
- **ASP.NET Core 9.0** - RESTful API server
- **Entity Framework Core** - ORM for database operations
- **SQLite** - Lightweight database for data storage
- **JWT Authentication** - Secure user authentication and authorization

## Features

### Game Features
1. **Multiple Difficulty Levels** - Easy, Medium, and Hard
2. **Timer System** - Track completion time for each puzzle
3. **Start/Pause Controls** - Start button with blur overlay and pause functionality
4. **Hint System** - Get hints for selected cells (tracked for analytics)
5. **Input Validation** - Real-time feedback on incorrect entries
6. **Auto-Completion Detection** - Automatically detects when puzzle is solved
7. **Randomized Puzzles** - Unique puzzle generation for each game

### User Management
1. **User Registration** - Create new accounts
2. **User Login/Logout** - Secure authentication with JWT tokens
3. **Session Persistence** - Stay logged in across browser sessions
4. **Token Refresh** - Automatic token renewal

### Analytics & Statistics
1. **Personal Performance Dashboard**
   - Best completion time per difficulty level
   - Average completion time per difficulty
   - Total hints used per difficulty
   - Average hints per game

2. **General Statistics** (for non-logged-in users)
   - Aggregated statistics from all users
   - Community best times
   - Average performance metrics

3. **Game History Tracking**
   - All completed games saved to database
   - Completion time tracking
   - Hints used per game
   - Difficulty level tracking

## Database Schema

### Users Table
- Id (Primary Key)
- Username (Unique)
- PasswordHash
- Role
- CreatedAt, UpdatedAt

### GameProgresses Table
- Id (Primary Key)
- UserId (Foreign Key)
- Puzzle (81-character string)
- Solution (81-character string)
- ElapsedSeconds
- Difficulty (easy/medium/hard)
- HintsUsed
- IsCompleted
- LastPlayedAt
- Name
- CreatedAt, UpdatedAt

### RefreshTokens Table
- Id (Primary Key)
- UserId (Foreign Key)
- Token
- CreatedAt, ExpiresAt, RevokedAt

## Configuration

### JWT Settings (appsettings.json)
```json
{
  "Jwt": {
    "Key": "your-secret-key-here",
    "Issuer": "sudoku-api",
    "Audience": "sudoku-api-users"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=sudoku.db"
  }
}
```

## How To Play

### Getting Started
1. **Optional:** Register/Login to track your progress
2. Select a difficulty level (Easy/Medium/Hard)
3. Click "New Game" to generate a new puzzle
4. Click the green "START" button to begin

### Playing the Game
1. Click on an empty cell to select it
2. Click a number button (1-9) to fill the cell
3. Use the **Hint** button to reveal the correct number for selected cell
4. Use the **Clear** button to remove your answer from selected cell
5. Use the **Pause** button to pause the timer and blur the puzzle
6. The game automatically completes when all cells are filled correctly

### Game Controls
- **New Game** - Generate a new puzzle
- **START** - Begin the timer and enable puzzle interaction
- **Pause/Resume** - Pause/resume the game (puzzle becomes blurred when paused)
- **Hint** - Get a hint for the selected cell (counts toward statistics)
- **Check** - Manually verify your solution
- **Clear** - Clear the selected cell

## Game Rules
1. Fill the 9x9 grid with numbers 1 to 9
2. Each row must contain digits 1-9 without repetition
3. Each column must contain digits 1-9 without repetition
4. Each 3x3 subgrid must contain digits 1-9 without repetition

## Performance Dashboard
The dashboard shows:
- **Best Time** - Your fastest completion for each difficulty
- **Average Time** - Your average completion time
- **Total Hints Used** - Total hints used across all games
- **Average Hints Used** - Average hints per completed game

For non-logged-in users, the dashboard shows aggregated statistics from all players.

## Security Features
- Password hashing
- JWT-based authentication
- Token expiration and refresh mechanism
- Authorization required for personal game data
- CORS enabled for local development

## Future Enhancements
1. Pencil marks for candidate numbers
2. Sound effects
3. Leaderboards

## License
This project is for educational purposes.
