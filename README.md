<<<<<<< HEAD
```markdown
=======
>>>>>>> 980b1e8fa48ba6838707c7bf7369415d141673a6
# Program Registration API

The **Program Registration API** is a .NET Core project designed to simplify program management and registration. It offers CRUD operations, integrates with Azure Cosmos DB, and provides flexibility for application templates and workflow stages. Built with .NET 8.0, it ensures efficiency and scalability.

### Key Features

- **Effortless Registration:** Simplify the registration process for participants.
- **Program Management:** Easily create, update, and manage various programs, program details, or application forms.
- **Secure Data Handling:** Ensures input data are validated against XML and SQL injection attacks.
- **Customizable Forms:** Ensures forms and program details are customizable.
- **Documentation:** Incorporates Swagger UI endpoints documentation for easy accessibility.
- **Integration Capabilities:** Seamlessly integrate with other systems and databases.

## Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Endpoints](#endpoints)
  - [ProgramController](#programcontroller)
    - [CreateQuestion](#createquestion)
    - [UpdateQuestion](#updatequestion)
  - [CandidateApplicationController](#candidateapplicationcontroller)
    - [GetQuestions](#getquestions)
    - [SubmitApplication](#submitapplication)
- [Deployment](#deployment)
- [Contributing](#contributing)

## Getting Started

### Prerequisites

Before you begin, ensure you have met the following requirements:

- [Install .NET Core](https://dotnet.microsoft.com/download)

### Installation

#### You can use Shell CLI or Visual Studio GUI

1. Clone this repository:
   ```shell
   git clone https://github.com/Meekdavid/APIAccessPro.git
   ```
2. Navigate to the Project Directory:
   ```shell
   cd APIAccessPro
   ```
3. Restore Dependencies:
   ```shell
   dotnet restore
   ```
4. Build Projects:
   ```shell
   dotnet build
   ```

## Usage

To run the API locally, use the following command:
```shell
dotnet run
```
The API will be available at http://localhost:5000 (HTTP) and https://localhost:5001 (HTTPS) by default. Refer to the swagger API documentation for details on available endpoints and usage.
**https://41.203.112.130/APIAccessPro/swagger/index.html**

## Endpoints

### ProgramController

#### CreateQuestion

- **Description**: Adds a new question to a program.
- **URL**: `/api/programs/{programId}/questions`
- **Method**: `POST`
- **Request Body**:
  ```json
  {
    "type": "MultipleChoice",
    "content": "What is your favorite color?",
    "options": ["Red", "Blue", "Green"]
  }
  ```
- **Response**:
  - **Success** (`200 OK`):
    ```json
    {
      "ResponseCode": "00",
      "ResponseMessage": "Question Sucessfully Created",
      "ProgramId": "123",
      "QuestionId": "MultipleChoice-123"
    }
    ```
  - **Program not found** (`400 Bad Request`):
    ```json
    {
      "ResponseCode": "01",
      "ResponseMessage": "Program not found"
    }
    ```
  - **Error** (`500 Internal Server Error`):
    ```json
    {
      "ResponseCode": "06",
      "ResponseMessage": "An error occured, please try again"
    }
    ```

#### UpdateQuestion

- **Description**: Updates an existing question.
- **URL**: `/api/programs/{questionId}`
- **Method**: `PUT`
- **Request Body**:
  ```json
  {
    "type": "MultipleChoice",
    "content": "What is your favorite color?",
    "options": ["Red", "Blue", "Green"]
  }
  ```
- **Response**:
  - **Success** (`200 OK`):
    ```json
    {
      "ResponseCode": "00",
      "ResponseMessage": "Question Successfully Updated"
    }
    ```
  - **Program not found** (`400 Bad Request`):
    ```json
    {
      "ResponseCode": "01",
      "ResponseMessage": "Program not found"
    }
    ```
  - **Question not found** (`400 Bad Request`):
    ```json
    {
      "ResponseCode": "02",
      "ResponseMessage": "Question not found"
    }
    ```
  - **Error** (`500 Internal Server Error`):
    ```json
    {
      "ResponseCode": "06",
      "ResponseMessage": "An error occurred, please try again"
    }
    ```

### CandidateApplicationController

#### GetQuestions

- **Description**: Retrieves questions for a specific program.
- **URL**: `/api/applications/{programId}/questions`
- **Method**: `GET`
- **Response**:
  - **Success** (`200 OK`):
    ```json
    {
      "ResponseCode": "00",
      "ResponseMessage": "Successfully retrieved",
      "programId": "123",
      "Questions": [
        {
          "id": "MultipleChoice-123",
          "type": "MultipleChoice",
          "content": "What is your favorite color?",
          "options": ["Red", "Blue", "Green"]
        }
      ]
    }
    ```
  - **Program not found** (`400 Bad Request`):
    ```json
    {
      "ResponseCode": "01",
      "ResponseMessage": "Program with ID 123 not found."
    }
    ```
  - **Error** (`500 Internal Server Error`):
    ```json
    {
      "ResponseCode": "06",
      "ResponseMessage": "An error occurred, please try again"
    }
    ```

#### SubmitApplication

- **Description**: Submits a new application for a candidate.
- **URL**: `/api/applications/SubmitApplication`
- **Method**: `POST`
- **Request Body**:
  ```json
  {
    "Email": "candidate@example.com",
    "ProgramId": "123",
    "Answers": [
      {
        "QuestionId": "MultipleChoice-123",
        "Answer": "Blue"
      }
    ]
  }
  ```
- **Response**:
  - **Success** (`201 Created`):
    ```json
    {
      "ResponseCode": "00",
      "ResponseMessage": "Application successfully submitted"
    }
    ```
  - **Duplicate application** (`400 Bad Request`):
    ```json
    {
      "ResponseCode": "02",
      "ResponseMessage": "Duplicate application found for the given email."
    }
    ```
  - **Error** (`500 Internal Server Error`):
    ```json
    {
      "ResponseCode": "06",
      "ResponseMessage": "An error occurred, please try again"
    }
    ```

## DTOs

### QuestionDto

```csharp
public class QuestionDto
{
    public string Type { get; set; }
    public string Content { get; set; }
    public List<string> Options { get; set; }
}
```

### CandidateApplicationDto

```csharp
public class CandidateApplicationDto
{
    public string Email { get; set; }
    public string ProgramId { get; set; }
    public List<AnswerDto> Answers { get; set; }
}
```

### AnswerDto

```csharp
public class AnswerDto
{
    public string QuestionId { get; set; }
    public string Answer { get; set; }
}
```

## Responses

### CreateQuestionResponse

```csharp
public class CreateQuestionResponse
{
    public string ResponseCode { get; set; }
    public string ResponseMessage { get; set; }
    public string ProgramId { get; set; }
    public string QuestionId { get; set; }
}
```

### BaseResponse

```csharp
public class BaseResponse
{
    public string ResponseCode { get; set; }
    public string ResponseMessage { get; set; }
}
```

### RetreiveQuestionResponse

```csharp
public class RetreiveQuestionResponse : BaseResponse
{
    public string ProgramId { get; set; }
    public IEnumerable<QuestionDto> Questions { get; set; }
}
```

### NotSuccessfulResponse

```csharp
public class NotSuccessfulResponse
{
    public string ResponseCode { get; set; }
    public string ResponseMessage { get; set; }
}
```

## Error Codes

- **00**: Success
- **01**: Program not found
- **02**: Question not found / Duplicate application found
- **06**: Internal server error

## Deployment

- **Publish API:** Build and publish the API project into a folder on your development machine.
- **Copy the Published API to the Server:** Transfer the published API folder to your Windows server using methods like FTP, SCP, or shared network drives.
- **Create an IIS Site:** Open IIS Manager, expand the server node, right-click on "Sites," select "Add Website," configure with a unique name, physical path, binding details, and click "OK" to create the site.
- **Install the .NET Core Hosting Bundle:** If not already installed, install from [here](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-aspnetcore-6.0.0-windows-hosting-bundle-installer)
- **Configure the Application Pool:** In IIS

 Manager, access "Application Pools," right-click your site's associated pool, choose "Advanced Settings," and set ".NET CLR Version" to "No Managed Code."
- **Test the API:** In IIS Manager, choose your site in the left pane, then click "Browse" to test your API in a web browser using the configured URL.
- **Set Up CosmosDB using an Emulator:** Set up the CosmosDB using an Emulator, details can be found on the appsettings.json file.
- **Troubleshoot:** In case of issues, check logs for errors and verify all required dependencies and configurations, including database connections and app settings.
### **N/B:** You can deploy the .NET Core API to various platforms based on your requirements: Azure App Service, AWS Lambda, Docker, Et Cetera.

## Contributing

Contributions are welcome! If you'd like to contribute to this project, please follow these guidelines:
- Fork the repository.
- Create a new branch for your changes.
- Make your changes and commit them.
- Create a pull request, explaining the purpose of your changes.

<details>
  <summary><b>🛠️&nbsp;&nbsp;More&nbsp;About&nbsp;Project</b></summary>
  
### Author  
  * David Mboko | [Youtube](https://www.youtube.com/@davidmboko6502/featured) | [LinkedIn](https://www.linkedin.com/mwlite/in/david-mboko-25bb9019b) | [Academia](https://aksu.academia.edu/DavidMboko) |

### Resources
- [Click to View](https://dotnet.microsoft.com/en-us/learn)
</details>
