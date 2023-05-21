# UsersAPI


## Installation and Setup:

1- clone this repo

2- install dotnet ef:
> dotnet tool install --global dotnet-ef

3- run dotnet build:
> dotnet build

4- create the database:
> dotnet ef database update

5- run the project:
> dotnet run

Note: to run swagger:
> dotnet watch run

---

## Usage:

UsersAPI provides the following endpoints:

* POST  /api/Users/register - Register a new user in the system.

* POST  /api/Users/signIn - Sign in an existing user.

* GET  /api/Users/user - Retrieves user details by email.

### User Registration (POST /api/Users/register)
Endpoint: /api/Users/register

Request Body:
```
{
  "firstName": "Ali",
  "lastName": "Suliman",
  "email": "ali.suliman@example.com",
  "password": "somePassword",
  "confirmedPassword": "somePassword"
}
```

Response:
```
{
  "data": {
    "firstName": "Ali",
    "lastName": "Suliman",
    "email": "ali.suliman@example.com",
    "token": ""
  },
  "message": "Registered Successfully, please logIn",
  "success": true,
  "statusCode": 200
}
```

### User Sign in (POST /api/Users/signIn)
Endpoint: /api/Users/signIn

Request Body:
```
{
  "email": "ali.suliman@example.com",
  "password": "somePassword"
}
```

Response:
```
{
  "data": {
    "firstName": "Ali",
    "lastName": "Suliman",
    "email": "ali.suliman@example.com",
    "token": "<<generated_token>>"
  },
  "message": "SignedIn Successfully",
  "success": true,
  "statusCode": 200
}
```

### Get User (GET /api/Users/user?email=<<email_here>>)
Endpoint: /api/Users/user?email=<<email_here>>

Request Parameter:
> ?email='ali.suliman@example.com'

Response:
```
{
  "data": {
    "firstName": "Ali",
    "lastName": "Suliman",
    "email": "ali.suliman@example.com",
    "token": ""
  },
  "message": "User Fetched Successfully",
  "success": true,
  "statusCode": 200
}
```