# UsersAPI


## Installation and Setup:

1- clone the repo
```bash
git clone https://github.com/Ali-y-Suliman/UsersAPI.git
```

2- install dotnet ef:
```bash
dotnet tool install --global dotnet-ef
```

3- run dotnet build:
```bash
dotnet build
```

4- create the database:
```bash
dotnet ef database update
```

5- run the project:
```bash
dotnet run
```

**Note**: to run swagger:
```bash
dotnet watch run
```

**Note**: to run the unit tests:
```bash
dotnet test
```

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