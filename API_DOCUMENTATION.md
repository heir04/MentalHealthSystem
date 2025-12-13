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
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6InVzZXJAZXhhbXBsZS5jb20iLCJyb2xlIjoiVXNlciIsImlkIjoiMTIzZTQ1NjctZTg5Yi0xMmQzLWE0NTYtNDI2NjE0MTc0MDAwIiwibmJmIjoxNzM0MDk2MDAwLCJleHAiOjE3MzQxODI0MDAsImlhdCI6MTczNDA5NjAwMH0.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c"
}
```

**Note:** Use this token in the `Authorization` header for protected endpoints:
```
Authorization: Bearer <token>
```

**Status Codes:**
- `200 OK` - Login successful, returns JWT token
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
    "email": "newuser@example.com",
    "username": "newuser",
    "password": "securepassword"
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

**Response:**
```json
{
  "message": "Password Updated",
  "status": true,
  "data": null
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

**Response:**
```json
{
  "message": "User updated successfully",
  "status": true,
  "data": {
    "email": "updated@example.com",
    "username": "updatedusername"
  }
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
  "message": "Success",
  "status": true,
  "data": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "email": null,
      "username": "username",
      "role": "User",
      "createdOn": "0001-01-01T00:00:00"
    }
  ]
}
```

---

### 6. Get User Profile
**GET** `/api/User/Profile`  
🔒 **Requires Authentication**

Get current authenticated user's profile.

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "email": "user@example.com",
    "username": "username",
    "role": "user",
    "isActive": false,
    "createdOn": "2025-09-11T12:00:00Z"
  }
}
```

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
  "message": "Success",
  "status": true,
  "data": null
}
```
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

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": null
}
```

---

### 3. Delete Story
**POST** `/api/Story/Delete/{id}`  
🔒 **Requires Authentication**

Delete a story.

**Path Parameters:**
- `id` (UUID) - Story ID

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": null
}
```

---

### 4. Get All Stories
**GET** `/api/Story/GetAll`

Retrieve all public stories.

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "userId": "456e7890-e89b-12d3-a456-426614174000",
      "content": "Story content...",
      "createdOn": "2025-09-11T12:00:00Z",
      "userName": "username",
      "comments": null
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

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "userId": "456e7890-e89b-12d3-a456-426614174000",
    "content": "Story content...",
    "createdOn": "2025-09-11T12:00:00Z",
    "userName": "username",
    "comments": [
      {
        "id": "789e0123-e89b-12d3-a456-426614174000",
        "storyId": "123e4567-e89b-12d3-a456-426614174000",
        "userId": "111e2222-e89b-12d3-a456-426614174000",
        "content": "Great story!",
        "createdOn": "2025-09-11T13:00:00Z",
        "userName": "commenter"
      }
    ]
  }
}
```

---

### 6. Get User's Stories
**GET** `/api/Story/GetAllUserStory`  
🔒 **Requires Authentication**

Get all stories created by the authenticated user.

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "userId": "456e7890-e89b-12d3-a456-426614174000",
      "content": "My story...",
      "createdOn": "2025-09-11T12:00:00Z",
      "userName": "myusername",
      "comments": null
    }
  ]
}
```

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

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": null
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

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": null
}
```

---

### 3. Delete Comment
**POST** `/api/Comment/Delete/{id}`  
🔒 **Requires Authentication**

Delete a comment.

**Path Parameters:**
- `id` (UUID) - Comment ID

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": null
}
```

---

### 4. Get Comment
**GET** `/api/Comment/Get/{id}`

Get a specific comment by ID.

**Path Parameters:**
- `id` (UUID) - Comment ID

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": {
    "id": "789e0123-e89b-12d3-a456-426614174000",
    "storyId": "123e4567-e89b-12d3-a456-426614174000",
    "userId": "456e7890-e89b-12d3-a456-426614174000",
    "content": "Great story!",
    "createdOn": "2025-09-11T13:00:00Z",
    "userName": null
  }
}
```

---

## Dashboard

### 1. Get User Dashboard
**GET** `/api/Dashboard/User`  
🔒 **Requires Authentication**

Get dashboard statistics and recent activity for the authenticated user.

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": {
    "user": {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "email": "user@example.com",
      "username": "username",
      "role": "User",
      "isActive": true,
      "createdOn": "2025-09-11T12:00:00Z"
    },
    "totalStories": 15,
    "totalComments": 47,
    "totalReactionsReceived": 234,
    "upcomingSessionsCount": 2,
    "recentStories": [
      {
        "id": "123e4567-e89b-12d3-a456-426614174000",
        "userId": "456e7890-e89b-12d3-a456-426614174000",
        "content": "My recent story...",
        "createdOn": "2025-12-10T12:00:00Z",
        "userName": "username",
        "comments": null
      }
    ],
    "upcomingSessions": [
      {
        "id": "789e0123-e89b-12d3-a456-426614174000",
        "userId": "456e7890-e89b-12d3-a456-426614174000",
        "therapistId": "111e2222-e89b-12d3-a456-426614174000",
        "status": "Scheduled"
      }
    ]
  }
}
```

---

### 2. Get Admin Dashboard
**GET** `/api/Dashboard/Admin`  
🔒 **Requires Authentication** (Admin only)

Get comprehensive system-wide statistics for administrators.

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": {
    "totalUsers": 1250,
    "totalStories": 4580,
    "totalComments": 12340,
    "totalReactions": 35670,
    "totalTherapists": 45,
    "approvedTherapists": 38,
    "pendingTherapists": 7,
    "totalSessions": 890,
    "pendingFlaggedContent": 12,
    "newUsersThisMonth": 87,
    "newStoriesThisMonth": 234,
    "recentUsers": [
      {
        "id": "123e4567-e89b-12d3-a456-426614174000",
        "email": null,
        "username": "newuser",
        "role": "User",
        "isActive": true,
        "createdOn": "2025-12-12T15:30:00Z"
      }
    ],
    "pendingReports": [
      {
        "id": "456e7890-e89b-12d3-a456-426614174000",
        "storyId": "789e0123-e89b-12d3-a456-426614174000",
        "commentId": "00000000-0000-0000-0000-000000000000",
        "reportedByUserId": "111e2222-e89b-12d3-a456-426614174000",
        "reason": "Inappropriate content",
        "flaggedAt": "0001-01-01T00:00:00",
        "isReviewed": false,
        "adminResponse": null,
        "reportedByUserName": null
      }
    ]
  }
}
```

---

### 3. Get Reports Dashboard
**GET** `/api/Dashboard/Reports`  
🔒 **Requires Authentication** (Admin only)

Get detailed content moderation statistics and flagged content analytics.

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": {
    "totalFlaggedContent": 156,
    "pendingReview": 12,
    "reviewedContent": 144,
    "flaggedStories": 98,
    "flaggedComments": 58,
    "contentRemovedThisMonth": 23,
    "recentFlags": [
      {
        "id": "123e4567-e89b-12d3-a456-426614174000",
        "storyId": "456e7890-e89b-12d3-a456-426614174000",
        "commentId": "00000000-0000-0000-0000-000000000000",
        "reportedByUserId": "789e0123-e89b-12d3-a456-426614174000",
        "reason": "Spam",
        "flaggedAt": "2025-12-13T10:00:00Z",
        "isReviewed": false,
        "adminResponse": null,
        "reportedByUserName": null
      }
    ],
    "flagReasonBreakdown": {
      "Inappropriate content": 45,
      "Spam": 32,
      "Harassment": 28,
      "Misinformation": 18,
      "Other": 33
    }
  }
}
```

---

## Reaction System

### 1. React to Story
**POST** `/api/Reaction/React/{storyId}`  
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

**Response:**
```json
{
  "message": "Reaction added successfully",
  "status": true,
  "data": {
    "id": "111e2222-e89b-12d3-a456-426614174000",
    "userId": "456e7890-e89b-12d3-a456-426614174000",
    "userName": null,
    "storyId": "123e4567-e89b-12d3-a456-426614174000",
    "commentId": null,
    "type": "Like",
    "createdOn": "2025-12-13T12:00:00Z"
  }
}
```

---

### 2. React to Comment
**POST** `/api/Reaction/React/{commentId}`  
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

**Available Reaction Types:**
- `Like`
- `Love`
- `Laugh`
- `Sad`
- `Angry`
- `Wow`

**Response:**
```json
{
  "message": "Reaction added successfully",
  "status": true,
  "data": {
    "id": "222e3333-e89b-12d3-a456-426614174000",
    "userId": "456e7890-e89b-12d3-a456-426614174000",
    "userName": null,
    "storyId": null,
    "commentId": "789e0123-e89b-12d3-a456-426614174000",
    "type": "Love",
    "createdOn": "2025-12-13T12:00:00Z"
  }
}
```

---

### 3. Remove Reaction from Story
**POST** `/api/Reaction/RemoveReaction/{storyId}`  
🔒 **Requires Authentication**

Remove user's reaction from a story.

**Path Parameters:**
- `storyId` (UUID) - Story ID

**Response:**
```json
{
  "message": "Reaction removed successfully",
  "status": true,
  "data": true
}
```

---

### 4. Remove Reaction from Comment
**POST** `/api/Reaction/RemoveReaction/{commentId}`  
🔒 **Requires Authentication**

Remove user's reaction from a comment.

**Path Parameters:**
- `commentId` (UUID) - Comment ID

**Response:**
```json
{
  "message": "Reaction removed successfully",
  "status": true,
  "data": true
}
```

---

### 5. Get Story Reactions
**GET** `/api/Reaction/GetReactions/{storyId}`

Get all reactions for a story.

**Path Parameters:**
- `storyId` (UUID) - Story ID

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": [
    {
      "id": "111e2222-e89b-12d3-a456-426614174000",
      "userId": "456e7890-e89b-12d3-a456-426614174000",
      "userName": "username",
      "storyId": "123e4567-e89b-12d3-a456-426614174000",
      "commentId": null,
      "type": "Like",
      "createdOn": "2025-12-13T12:00:00Z"
    }
  ]
}
```

---

### 6. Get Comment Reactions
**GET** `/api/Reaction/GetReactions/{commentId}`

Get all reactions for a comment.

**Path Parameters:**
- `commentId` (UUID) - Comment ID

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": [
    {
      "id": "222e3333-e89b-12d3-a456-426614174000",
      "userId": "456e7890-e89b-12d3-a456-426614174000",
      "userName": "username",
      "storyId": null,
      "commentId": "789e0123-e89b-12d3-a456-426614174000",
      "type": "Love",
      "createdOn": "2025-12-13T12:00:00Z"
    }
  ]
}
```

---

### 7. Get Story Reaction Summary
**GET** `/api/Reaction/GetReactions/{storyId}/Summary`

Get reaction summary (counts by type) for a story.

**Path Parameters:**
- `storyId` (UUID) - Story ID

**Response:**
```json
{
  "message": "Success",
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
**GET** `/api/Reaction/GetReactions/{commentId}/Summary`

Get reaction summary (counts by type) for a comment.

**Path Parameters:**
- `commentId` (UUID) - Comment ID

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": [
    {
      "type": "Love",
      "count": 5,
      "userReacted": true
    },
    {
      "type": "Like",
      "count": 3,
      "userReacted": false
    }
  ]
}
```

---

## Therapist Management

### 1. Create Therapist
**POST** `/api/Therapist/Create`

Register a new therapist (requires approval).

**Request Body:**
```json
{
  "fullName": "Dr. Jane Smith",
  "specialization": "Anxiety and Depression",
  "email": "dr.smith@example.com",
  "certificationLink": "https://example.com/cert.pdf",
  "bio": "Experienced therapist specializing in anxiety and depression.",
  "contactLink": "https://calendly.com/dr-smith"
}
```

**Response:**
```json
{
  "message": "Therapist created successfully",
  "status": true,
  "data": {
    "fullName": "Dr. Jane Smith",
    "specialization": "Anxiety and Depression",
    "email": "dr.smith@example.com",
    "certificationLink": "https://example.com/cert.pdf",
    "bio": "Experienced therapist specializing in anxiety and depression.",
    "contactLink": "https://calendly.com/dr-smith"
  }
}
```

---

### 2. Update Therapist
**PUT** `/api/Therapist/Update/{id}`  
🔒 **Requires Authentication**

Update therapist information.

**Path Parameters:**
- `id` (UUID) - Therapist ID

**Request Body:**
```json
{
  "fullName": "Dr. Jane Smith Updated",
  "specialization": "Anxiety, Depression and PTSD",
  "bio": "Updated bio...",
  "contactLink": "https://calendly.com/dr-smith-updated",
  "availability": 0
}
```

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": null
}
```

---

### 3. Approve Therapist
**PUT** `/api/Therapist/Approve/{id}`  
🔒 **Requires Authentication** (Admin only)

Approve a therapist's registration.

**Path Parameters:**
- `id` (UUID) - Therapist ID

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": null
}
```

---

### 4. Delete Therapist
**POST** `/api/Therapist/Delete/{id}`  
🔒 **Requires Authentication**

Delete a therapist profile.

**Path Parameters:**
- `id` (UUID) - Therapist ID

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": null
}
```

---

### 5. Get All Therapists
**GET** `/api/Therapist/GetAll`

Get all approved therapists.

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "userId": "456e7890-e89b-12d3-a456-426614174000",
      "fullName": "Dr. Jane Smith",
      "specialization": "Anxiety and Depression",
      "certificationLink": "https://example.com/cert.pdf",
      "bio": "Experienced therapist...",
      "contactLink": "https://calendly.com/dr-smith",
      "availability": 0,
      "userName": "drjanesmith"
    }
  ]
}
```
```

---

### 6. Get Therapist by ID
**GET** `/api/Therapist/Get/{id}`

Get specific therapist details.

**Path Parameters:**
- `id` (UUID) - Therapist ID

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "userId": "456e7890-e89b-12d3-a456-426614174000",
    "fullName": "Dr. Jane Smith",
    "specialization": "Anxiety and Depression",
    "certificationLink": "https://example.com/cert.pdf",
    "bio": "Experienced therapist...",
    "contactLink": "https://calendly.com/dr-smith",
    "availability": 0,
    "userName": "drjanesmith"
  }
}
```

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
  "message": "Success",
  "status": true,
  "data": null
}
```
```

---

### 2. Get Session
**GET** `/api/TherapySession/Get/{id}`  
🔒 **Requires Authentication**

Get specific therapy session details.

**Path Parameters:**
- `id` (UUID) - Session ID

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "userId": "456e7890-e89b-12d3-a456-426614174000",
    "therapistId": "789e0123-e89b-12d3-a456-426614174000",
    "status": "Pending"
  }
}
```

---

### 3. Get Sessions by Therapist
**GET** `/api/TherapySession/GetAllByTherapist`  
🔒 **Requires Authentication** (Therapist only)

Get all sessions for the authenticated therapist.

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "userId": "456e7890-e89b-12d3-a456-426614174000",
      "therapistId": "789e0123-e89b-12d3-a456-426614174000",
      "status": "Scheduled"
    },
    {
      "id": "222e3333-e89b-12d3-a456-426614174000",
      "userId": "555e6666-e89b-12d3-a456-426614174000",
      "therapistId": "789e0123-e89b-12d3-a456-426614174000",
      "status": "Pending"
    }
  ]
}
```

---

### 4. Get Sessions by User
**GET** `/api/TherapySession/GetAllByActiveUser`  
🔒 **Requires Authentication**

Get all sessions for the authenticated user.

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "userId": "456e7890-e89b-12d3-a456-426614174000",
      "therapistId": "789e0123-e89b-12d3-a456-426614174000",
      "status": "Scheduled"
    }
  ]
}
```

---

### 5. Update Session Status
**PUT** `/api/TherapySession/UpdateSessionStatus/{id}`  
🔒 **Requires Authentication**

Update the status of a therapy session.

**Path Parameters:**
- `id` (UUID) - Session ID

**Request Body:**
```json
1
```

**Available Status Values:**
- `0` - Pending
- `1` - Scheduled
- `2` - Completed
- `3` - Cancelled

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "userId": "456e7890-e89b-12d3-a456-426614174000",
    "therapistId": "789e0123-e89b-12d3-a456-426614174000",
    "status": "Scheduled"
  }
}
```

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
  "storyId": "00000000-0000-0000-0000-000000000000",
  "commentId": "00000000-0000-0000-0000-000000000000",
  "reason": "This story contains harmful content that violates community guidelines."
}
```

**Response:**
```json
{
  "message": "Story reported successfully",
  "status": true,
  "data": {
    "storyId": "00000000-0000-0000-0000-000000000000",
    "commentId": "00000000-0000-0000-0000-000000000000",
    "reason": "This story contains harmful content that violates community guidelines."
  }
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
  "storyId": "00000000-0000-0000-0000-000000000000",
  "commentId": "00000000-0000-0000-0000-000000000000",
  "reason": "This comment is spam and not relevant to the discussion."
}
```

**Response:**
```json
{
  "message": "Comment reported successfully",
  "status": true,
  "data": {
    "storyId": "00000000-0000-0000-0000-000000000000",
    "commentId": "00000000-0000-0000-0000-000000000000",
    "reason": "This comment is spam and not relevant to the discussion."
  }
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
  "isReviewed": true,
  "adminResponse": "Content is appropriate and follows community guidelines. No action required."
}
```

**Response:**
```json
{
  "message": "Flagged content reviewed successfully",
  "status": true,
  "data": null
}
```

---

### 4. Get Flagged Content
**GET** `/api/FlaggedContent/Get/{id}`  
🔒 **Requires Authentication**

Get specific flagged content details.

**Path Parameters:**
- `id` (UUID) - Flagged content ID

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": {
    "id": "123e4567-e89b-12d3-a456-426614174000",
    "storyId": "456e7890-e89b-12d3-a456-426614174000",
    "commentId": "00000000-0000-0000-0000-000000000000",
    "reportedByUserId": "789e0123-e89b-12d3-a456-426614174000",
    "reason": "Inappropriate content",
    "flaggedAt": "0001-01-01T00:00:00",
    "isReviewed": false,
    "adminResponse": null,
    "reportedByUserName": null
  }
}
```

---

### 5. Get All Flagged Content
**GET** `/api/FlaggedContent/GetAll`  
🔒 **Requires Authentication** (Admin/Moderator only)

Get all flagged content for review.

**Response:**
```json
{
  "message": "Success",
  "status": true,
  "data": [
    {
      "id": "123e4567-e89b-12d3-a456-426614174000",
      "storyId": "456e7890-e89b-12d3-a456-426614174000",
      "commentId": "00000000-0000-0000-0000-000000000000",
      "reportedByUserId": "789e0123-e89b-12d3-a456-426614174000",
      "reason": "Inappropriate content",
      "flaggedAt": "0001-01-01T00:00:00",
      "isReviewed": false,
      "adminResponse": null,
      "reportedByUserName": null
    }
  ]
}
```

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

All error responses follow the standard format:

```json
{
  "message": "Error description",
  "status": false,
  "data": null
}
```

### Common Error Examples

#### 1. Authentication Failed (400 Bad Request)
**Scenario:** Invalid login credentials

```json
{
  "message": "Incorrect email or password!",
  "status": false,
  "data": null
}
```

#### 2. User Already Exists (400 Bad Request)
**Scenario:** Email or username already registered

```json
{
  "message": "User with email: user@example.com already exist",
  "status": false,
  "data": null
}
```

```json
{
  "message": "User with Username: username already exist",
  "status": false,
  "data": null
}
```

#### 3. Missing Authentication Token (401 Unauthorized)
**Scenario:** Accessing protected endpoint without token

```json
{
  "message": "Unauthorized",
  "status": false,
  "data": null
}
```

#### 4. Resource Not Found (400 Bad Request)
**Scenario:** Story, comment, or user not found

```json
{
  "message": "Not found",
  "status": false,
  "data": null
}
```

```json
{
  "message": "Story not found",
  "status": false,
  "data": null
}
```

```json
{
  "message": "User not found",
  "status": false,
  "data": null
}
```

#### 5. Unauthorized Action (400 Bad Request)
**Scenario:** Trying to update/delete content you don't own

```json
{
  "message": "Not Authorized",
  "status": false,
  "data": null
}
```

#### 6. Duplicate Action (400 Bad Request)
**Scenario:** Already reported content

```json
{
  "message": "Comments already reported by you",
  "status": false,
  "data": null
}
```

**Scenario:** Session already exists

```json
{
  "message": "You have a pending or scheduled session with the Therapist",
  "status": false,
  "data": null
}
```

#### 7. Therapist Not Approved (400 Bad Request)
**Scenario:** Booking session with unapproved therapist

```json
{
  "message": "Therapist is not approved",
  "status": false,
  "data": null
}
```

#### 8. Invalid Password (400 Bad Request)
**Scenario:** Current password is incorrect

```json
{
  "message": "Current password is incorrect",
  "status": false,
  "data": null
}
```

#### 9. Reaction Not Found (400 Bad Request)
**Scenario:** Removing non-existent reaction

```json
{
  "message": "Reaction not found",
  "status": false,
  "data": null
}
```

### Error Handling Best Practices

1. **Always check the `status` field** - `false` indicates an error
2. **Display the `message` field** to users - It contains user-friendly error descriptions
3. **Handle 401 errors** - Redirect to login page or refresh token
4. **Implement retry logic** for 500 errors
5. **Validate input client-side** to reduce 400 errors

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
curl -X POST "http://localhost:5158/api/User/Login" \
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

**Last Updated:** December 13, 2025  
**API Version:** 1.0.0
