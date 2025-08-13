# Todo Web API - Usage Guide

## API Information
- **Base URL**: `http://localhost:5236`
- **Swagger UI**: `http://localhost:5236/swagger`
- **Database**: SQLite (`todoapi.db`)
- **Authentication**: JWT Bearer Token

## Default Test Accounts

### Admin Account
- **Email**: `admin@todoapi.com`
- **Password**: `Admin123!`
- **Role**: Admin

### User Account
- **Email**: `user@todoapi.com`
- **Password**: `User123!`
- **Role**: User

## API Endpoints

### Authentication

#### 1. Register New User
```http
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "Password123!",
  "confirmPassword": "Password123!"
}
```

#### 2. Login
```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@todoapi.com",
  "password": "Admin123!"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiration": "2024-08-20T08:53:27.123Z",
    "user": {
      "id": "user-id-here",
      "firstName": "Admin",
      "lastName": "User",
      "email": "admin@todoapi.com",
      "roles": ["Admin"]
    }
  }
}
```

#### 3. Get Current User Info
```http
GET /api/auth/me
Authorization: Bearer YOUR_JWT_TOKEN
```

### Todo Items

> **Note**: All todo endpoints require JWT authentication. Include the token in the Authorization header: `Bearer YOUR_JWT_TOKEN`

#### 1. Get All Todos (with filtering and pagination)
```http
GET /api/todos?isCompleted=false&priority=High&pageNumber=1&pageSize=10
Authorization: Bearer YOUR_JWT_TOKEN
```

**Query Parameters:**
- `isCompleted` (bool): Filter by completion status
- `priority` (string): Filter by priority (Low, Medium, High)
- `category` (string): Filter by category
- `pageNumber` (int): Page number (default: 1)
- `pageSize` (int): Items per page (default: 10)

#### 2. Get Todo by ID
```http
GET /api/todos/{id}
Authorization: Bearer YOUR_JWT_TOKEN
```

#### 3. Create New Todo
```http
POST /api/todos
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "title": "Complete project documentation",
  "description": "Write comprehensive API documentation",
  "priority": "High",
  "category": "Work",
  "dueDate": "2024-08-20T17:00:00Z"
}
```

#### 4. Update Todo
```http
PUT /api/todos/{id}
Authorization: Bearer YOUR_JWT_TOKEN
Content-Type: application/json

{
  "title": "Updated task title",
  "description": "Updated description",
  "isCompleted": true,
  "priority": "Medium",
  "category": "Personal",
  "dueDate": "2024-08-21T12:00:00Z"
}
```

#### 5. Delete Todo
```http
DELETE /api/todos/{id}
Authorization: Bearer YOUR_JWT_TOKEN
```

#### 6. Get Todo Statistics
```http
GET /api/todos/stats
Authorization: Bearer YOUR_JWT_TOKEN
```

**Response:**
```json
{
  "success": true,
  "data": {
    "totalTodos": 15,
    "completedTodos": 8,
    "pendingTodos": 7,
    "overdueTodos": 2,
    "todayTodos": 3,
    "thisWeekTodos": 5,
    "highPriorityTodos": 4
  }
}
```

## Response Format

All API responses follow this standardized format:

### Success Response
```json
{
  "success": true,
  "message": "Operation successful",
  "data": { /* actual data */ }
}
```

### Error Response
```json
{
  "success": false,
  "message": "Error description",
  "errors": ["Detailed error messages"]
}
```

### Paginated Response
```json
{
  "success": true,
  "data": {
    "items": [/* array of items */],
    "totalCount": 25,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 3,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

## Authentication Flow

1. **Register** a new account or use existing test accounts
2. **Login** to get JWT token
3. **Include token** in Authorization header for protected endpoints
4. **Token expires** in 7 days, login again when needed

## Validation Rules

### User Registration
- First Name: Required, 2-50 characters
- Last Name: Required, 2-50 characters
- Email: Required, valid email format
- Password: Required, minimum 6 characters, must contain uppercase, lowercase, and digit

### Todo Item
- Title: Required, 3-200 characters
- Description: Optional, max 1000 characters
- Priority: Must be "Low", "Medium", or "High"
- Category: Optional, max 50 characters
- Due Date: Optional, must be future date

## Error Handling

The API returns appropriate HTTP status codes:

- `200 OK`: Success
- `201 Created`: Resource created successfully
- `400 Bad Request`: Validation errors
- `401 Unauthorized`: Missing or invalid token
- `403 Forbidden`: Insufficient permissions
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

## Testing with Swagger UI

1. Open `http://localhost:5236/swagger` in your browser
2. Click "Authorize" button
3. Login using `/api/auth/login` endpoint
4. Copy the token from response
5. Enter token as `Bearer YOUR_TOKEN` in authorization
6. Test all endpoints with interactive UI

## Example Workflow

```bash
# 1. Login
curl -X POST "http://localhost:5236/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"admin@todoapi.com","password":"Admin123!"}'

# 2. Create Todo (use token from login response)
curl -X POST "http://localhost:5236/api/todos" \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{"title":"Test Todo","description":"Test description","priority":"High"}'

# 3. Get All Todos
curl -X GET "http://localhost:5236/api/todos" \
  -H "Authorization: Bearer YOUR_TOKEN"
```

