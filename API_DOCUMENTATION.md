# Mental Health System API Documentation

## Overview

The Mental Health System API is a comprehensive RESTful web service designed to support mental health management and therapy services. It provides endpoints for user management, story sharing, commenting, reactions, therapist management, therapy sessions, and content moderation.

**Base URL:** 
- Development: `http://localhost:5158`

**API Version:** v1  
**Content-Type:** `application/json`

## Authentication

The API uses JWT (JSON Web Token) based authentication. Most endpoints require authentication via the `Authorization` header.

### Authentication Header
```
Authorization: Bearer <your-jwt-token>
```

### Getting a Token
Obtain a JWT token by calling the login endpoint with valid credentials.

---

## Response Format

All API responses follow a standard format:

```json
{
  "message": "Response message",
  "status": true/false,
  "data": {} // Response data (nullable)
}
```

---

## User Management

### 1. User Login
**POST** `/api/User/Login`

Authenticate a user and receive a JWT token.

**Request Body:**
```json
{
  "email": "user@example.com",
  "password": "your-password"
}
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Status Codes:**
- `200 OK` - Login successful
- `400 Bad Request` - Invalid credentials
- `401 Unauthorized` - Authentication failed

---

### 2. Create User
**POST** `/api/User/Create`

Register a new user account.

**Request Body:**
```json
{
  "email": "newuser@example.com",
  "username": "newuser",
  "password": "securepassword"
}
```

**Response:**
```json
{
  "message": "User created successfully",
  "status": true,
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "email": "newuser@example.com",
    "username": "newuser",
    "role": "User",
    "isActive": true,
    "createdOn": "2025-09-11T12:00:00Z"
  }
}
```

---

### 3. Update Password
**PUT** `/api/User/UpdatePassword`  
🔒 **Requires Authentication**

Change user password.

**Request Body:**
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "currentPassword": "oldpassword",
  "newPassword": "newpassword",
  "confirmPassword": "newpassword"
}
```

---

### 4. Update User
**PUT** `/api/User/Update/{id}`  
🔒 **Requires Authentication**

Update user information.

**Path Parameters:**
- `id` (UUID) - User ID

**Request Body:**
```json
{
  "email": "updated@example.com",
  "username": "updatedusername",
  "isActive": true
}
```

---

### 5. Get All Users
**GET** `/api/User/GetAll`  
🔒 **Requires Authentication**

Retrieve all users (admin functionality).

**Response:**
```json
{
  "message": "Users retrieved successfully",
  "status": true,
  "data": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "email": "user@example.com",
      "username": "username",
      "role": "User",
      "isActive": true,
      "createdOn": "2025-09-11T12:00:00Z"
    }
  ]
}
```

---

### 6. Get User Profile
**GET** `/api/User/Profile`  
🔒 **Requires Authentication**

Get current authenticated user's profile.

---

## Story Management

### 1. Create Story
**POST** `/api/Story/Create`  
🔒 **Requires Authentication**

Create a new story/post.

**Request Body:**
```json
{
  "content": "This is my mental health journey story..."
}
```

**Response:**
```json
{
  "message": "Story created successfully",
  "status": true,
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "userId": "456e7890-e89b-12d3-a456-426614174000",
    "content": "This is my mental health journey story...",
    "createdOn": "2025-09-11T12:00:00Z",
    "userName": "username",
    "comments": []
  }
}
```

---

### 2. Update Story
**PUT** `/api/Story/Update/{id}`  
🔒 **Requires Authentication**

Update an existing story.

**Path Parameters:**
- `id` (UUID) - Story ID

**Request Body:**
```json
{
  "content": "Updated story content..."
}
```

---

### 3. Delete Story
**POST** `/api/Story/Delete/{id}`  
🔒 **Requires Authentication**

Delete a story.

**Path Parameters:**
- `id` (UUID) - Story ID

---

### 4. Get All Stories
**GET** `/api/Story/GetAll`

Retrieve all public stories.

**Response:**
```json
{
  "message": "Stories retrieved successfully",
  "status": true,
  "data": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "userId": "456e7890-e89b-12d3-a456-426614174000",
      "content": "Story content...",
      "createdOn": "2025-09-11T12:00:00Z",
      "userName": "username",
      "comments": []
    }
  ]
}
```

---

### 5. Get Story by ID
**GET** `/api/Story/Get/{id}`

Get a specific story by ID.

**Path Parameters:**
- `id` (UUID) - Story ID

---

### 6. Get User's Stories
**GET** `/api/Story/GetAllUserStory`  
🔒 **Requires Authentication**

Get all stories created by the authenticated user.

---

## Comment Management

### 1. Create Comment
**POST** `/api/Comment/Create/{storyId}`  
🔒 **Requires Authentication**

Add a comment to a story.

**Path Parameters:**
- `storyId` (UUID) - Story ID

**Request Body:**
```json
{
  "content": "Great story! Thanks for sharing."
}
```

---

### 2. Update Comment
**PUT** `/api/Comment/Update/{id}`  
🔒 **Requires Authentication**

Update an existing comment.

**Path Parameters:**
- `id` (UUID) - Comment ID

**Request Body:**
```json
{
  "content": "Updated comment content..."
}
```

---

### 3. Delete Comment
**POST** `/api/Comment/Delete/{id}`  
🔒 **Requires Authentication**

Delete a comment.

**Path Parameters:**
- `id` (UUID) - Comment ID

---

### 4. Get Comment
**GET** `/api/Comment/Get/{id}`

Get a specific comment by ID.

**Path Parameters:**
- `id` (UUID) - Comment ID

---

## Reaction System

### 1. React to Story
**POST** `/api/Reaction/Story/{storyId}`  
🔒 **Requires Authentication**

Add a reaction to a story.

**Path Parameters:**
- `storyId` (UUID) - Story ID

**Request Body:**
```json
{
  "type": "Like"
}
```

**Available Reaction Types:**
- `Like`
- `Love`
- `Laugh`
- `Sad`
- `Angry`
- `Wow`

---

### 2. React to Comment
**POST** `/api/Reaction/Comment/{commentId}`  
🔒 **Requires Authentication**

Add a reaction to a comment.

**Path Parameters:**
- `commentId` (UUID) - Comment ID

**Request Body:**
```json
{
  "type": "Love"
}
```

---

### 3. Remove Reaction from Story
**DELETE** `/api/Reaction/Story/{storyId}`  
🔒 **Requires Authentication**

Remove user's reaction from a story.

**Path Parameters:**
- `storyId` (UUID) - Story ID

---

### 4. Remove Reaction from Comment
**DELETE** `/api/Reaction/Comment/{commentId}`  
🔒 **Requires Authentication**

Remove user's reaction from a comment.

**Path Parameters:**
- `commentId` (UUID) - Comment ID

---

### 5. Get Story Reactions
**GET** `/api/Reaction/Story/{storyId}`

Get all reactions for a story.

**Path Parameters:**
- `storyId` (UUID) - Story ID

---

### 6. Get Comment Reactions
**GET** `/api/Reaction/Comment/{commentId}`

Get all reactions for a comment.

**Path Parameters:**
- `commentId` (UUID) - Comment ID

---

### 7. Get Story Reaction Summary
**GET** `/api/Reaction/Story/{storyId}/Summary`

Get reaction summary (counts by type) for a story.

**Path Parameters:**
- `storyId` (UUID) - Story ID

**Response:**
```json
{
  "message": "Reaction summary retrieved successfully",
  "status": true,
  "data": [
    {
      "type": "Like",
      "count": 15,
      "userReacted": true
    },
    {
      "type": "Love",
      "count": 8,
      "userReacted": false
    }
  ]
}
```

---

### 8. Get Comment Reaction Summary
**GET** `/api/Reaction/Comment/{commentId}/Summary`

Get reaction summary (counts by type) for a comment.

**Path Parameters:**
- `commentId` (UUID) - Comment ID

---

## Therapist Management

### 1. Create Therapist
**POST** `/api/Therapist/Create`

Register a new therapist (requires approval).

**Request Body:**
```json
{
  "name": "Dr. Jane Smith",
  "specialization": "Anxiety and Depression",
  "email": "dr.smith@example.com",
  "phoneNumber": "+1234567890",
  "licenseNumber": "LIC123456",
  "experience": 5,
  "education": "PhD in Clinical Psychology"
}
```

---

### 2. Update Therapist
**PUT** `/api/Therapist/Update/{id}`  
🔒 **Requires Authentication**

Update therapist information.

**Path Parameters:**
- `id` (UUID) - Therapist ID

---

### 3. Approve Therapist
**PUT** `/api/Therapist/Approve/{id}`  
🔒 **Requires Authentication** (Admin only)

Approve a therapist's registration.

**Path Parameters:**
- `id` (UUID) - Therapist ID

---

### 4. Delete Therapist
**POST** `/api/Therapist/Delete/{id}`  
🔒 **Requires Authentication**

Delete a therapist profile.

**Path Parameters:**
- `id` (UUID) - Therapist ID

---

### 5. Get All Therapists
**GET** `/api/Therapist/GetAll`

Get all approved therapists.

**Response:**
```json
{
  "message": "Therapists retrieved successfully",
  "status": true,
  "data": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "name": "Dr. Jane Smith",
      "specialization": "Anxiety and Depression",
      "email": "dr.smith@example.com",
      "isApproved": true,
      "availability": "Available",
      "rating": 4.8
    }
  ]
}
```

---

### 6. Get Therapist by ID
**GET** `/api/Therapist/Get/{id}`

Get specific therapist details.

**Path Parameters:**
- `id` (UUID) - Therapist ID

---

## Therapy Session Management

### 1. Book Session
**POST** `/api/TherapySession/BookSession/{therapistId}`  
🔒 **Requires Authentication**

Book a therapy session with a therapist.

**Path Parameters:**
- `therapistId` (UUID) - Therapist ID

**Response:**
```json
{
  "message": "Session booked successfully",
  "status": true,
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "userId": "456e7890-e89b-12d3-a456-426614174000",
    "therapistId": "789e0123-e89b-12d3-a456-426614174000",
    "scheduledDate": "2025-09-15T10:00:00Z",
    "status": "Scheduled",
    "createdOn": "2025-09-11T12:00:00Z"
  }
}
```

---

### 2. Get Session
**GET** `/api/TherapySession/Get/{id}`  
🔒 **Requires Authentication**

Get specific therapy session details.

**Path Parameters:**
- `id` (UUID) - Session ID

---

### 3. Get Sessions by Therapist
**GET** `/api/TherapySession/GetAllByTherapist`  
🔒 **Requires Authentication** (Therapist only)

Get all sessions for the authenticated therapist.

---

### 4. Get Sessions by User
**GET** `/api/TherapySession/GetAllByActiveUser`  
🔒 **Requires Authentication**

Get all sessions for the authenticated user.

---

### 5. Update Session Status
**PUT** `/api/TherapySession/UpdateSessionStatus/{id}`  
🔒 **Requires Authentication**

Update the status of a therapy session.

**Path Parameters:**
- `id` (UUID) - Session ID

**Request Body:**
```json
"Completed"
```

**Available Status Values:**
- `Scheduled`
- `Completed` 
- `Cancelled`
- `Pending`

---

## Content Moderation

### 1. Report Story
**POST** `/api/FlaggedContent/ReportStory/{storyId}`  
🔒 **Requires Authentication**

Report inappropriate content in a story.

**Path Parameters:**
- `storyId` (UUID) - Story ID

**Request Body:**
```json
{
  "reason": "Inappropriate content",
  "description": "This story contains harmful content that violates community guidelines."
}
```

---

### 2. Report Comment
**POST** `/api/FlaggedContent/ReportComment/{commentId}`  
🔒 **Requires Authentication**

Report inappropriate content in a comment.

**Path Parameters:**
- `commentId` (UUID) - Comment ID

**Request Body:**
```json
{
  "reason": "Spam",
  "description": "This comment is spam and not relevant to the discussion."
}
```

---

### 3. Review Flagged Content
**PUT** `/api/FlaggedContent/Review/{id}`  
🔒 **Requires Authentication** (Admin/Moderator only)

Review and update the status of flagged content.

**Path Parameters:**
- `id` (UUID) - Flagged content ID

**Request Body:**
```json
{
  "reviewStatus": "Approved",
  "reviewNotes": "Content is appropriate and follows community guidelines.",
  "actionTaken": "No action required"
}
```

---

### 4. Get Flagged Content
**GET** `/api/FlaggedContent/Get/{id}`  
🔒 **Requires Authentication**

Get specific flagged content details.

**Path Parameters:**
- `id` (UUID) - Flagged content ID

---

### 5. Get All Flagged Content
**GET** `/api/FlaggedContent/GetAll`  
🔒 **Requires Authentication** (Admin/Moderator only)

Get all flagged content for review.

---

## Error Handling

### HTTP Status Codes

| Status Code | Description |
|-------------|-------------|
| 200 | OK - Request successful |
| 400 | Bad Request - Invalid request data |
| 401 | Unauthorized - Authentication required |
| 403 | Forbidden - Insufficient permissions |
| 404 | Not Found - Resource not found |
| 500 | Internal Server Error - Server error |

### Error Response Format

```json
{
  "message": "Error description",
  "status": false,
  "data": null
}
```

---

## Data Models

### User Model
```json
{
  "id": "uuid",
  "email": "string",
  "username": "string",
  "role": "string",
  "isActive": "boolean",
  "createdOn": "datetime"
}
```

### Story Model
```json
{
  "id": "uuid",
  "userId": "uuid",
  "content": "string",
  "createdOn": "datetime",
  "userName": "string",
  "comments": []
}
```

### Comment Model
```json
{
  "id": "uuid",
  "storyId": "uuid",
  "userId": "uuid",
  "content": "string",
  "createdOn": "datetime",
  "userName": "string"
}
```

### Reaction Model
```json
{
  "id": "uuid",
  "userId": "uuid",
  "userName": "string",
  "storyId": "uuid (nullable)",
  "commentId": "uuid (nullable)",
  "type": "string",
  "createdOn": "datetime"
}
```

### Therapist Model
```json
{
  "id": "uuid",
  "name": "string",
  "specialization": "string",
  "email": "string",
  "phoneNumber": "string",
  "licenseNumber": "string",
  "experience": "number",
  "education": "string",
  "isApproved": "boolean",
  "availability": "Available|Unavailable|Busy",
  "rating": "number"
}
```

### Therapy Session Model
```json
{
  "id": "uuid",
  "userId": "uuid",
  "therapistId": "uuid",
  "scheduledDate": "datetime",
  "status": "Scheduled|Completed|Cancelled|Pending",
  "notes": "string",
  "createdOn": "datetime"
}
```

---

## Rate Limiting

The API implements rate limiting to prevent abuse:
- **General endpoints:** 100 requests per minute per IP
- **Authentication endpoints:** 10 requests per minute per IP
- **Report endpoints:** 20 requests per hour per user

---

## Security Considerations

1. **Authentication:** All protected endpoints require valid JWT tokens
2. **Authorization:** Role-based access control for admin/moderator functions
3. **Input Validation:** All inputs are validated and sanitized
4. **HTTPS:** Use HTTPS in production environments
5. **CORS:** Properly configured for allowed origins

---

## Getting Started

### 1. Prerequisites
- .NET 9.0 SDK
- SQL Server or compatible database
- Valid connection string

### 2. Setup
1. Clone the repository
2. Update connection string in `appsettings.json`
3. Run database migrations
4. Start the application

### 3. Testing
Use tools like Postman, curl, or any HTTP client to test the endpoints.

### Example curl request:
```bash
curl -X POST "https://localhost:7286/api/User/Login" \
  -H "Content-Type: application/json" \
  -d '{"email":"user@example.com","password":"password"}'
```

---

## Support

For API support and questions:
- **Email:** support@mentalhealthsystem.com
- **Documentation:** This document
- **Status Page:** Check application health at `/health` endpoint

---

**Last Updated:** September 11, 2025  
**API Version:** 1.0.0
