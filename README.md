# File Import WEB UI

My solution is consist of 2 components, frontend and backend. Backend is developed on Angular 7. Frontend is a .Net Core API.  

## Tech Stack
- Angular 7
- Angular Material
- .Net Core 
- MongoDB
- Newton Soft for JSON conversion

## Backend 

When a request is received by backend, API saves the file to a temporary folder then returns OK with session.Process of file is performed asynchroniously, both conversion of json file and database import.Async processing is developed in case of big file processing use case.  

### API Endpoints

#### POST
**/api/file/import**: is a POST endpoint that accepts FormData and return the following response for success cases;

```javascript
{"session":"20181110200801"}
```

if the file to upload is not valid, the following message will be sent with HTTP Bad Request ;  

```javascript
{"description":"file is invalid"}
```
#### GET
**/api/file/import/status**: is a GET endpoint that returns the status of import process for the given session.  
*Sample request : /api/file/import/status?session=20181110200801*

```javascript
{"importState":"DBImporting","percentage":91}
```

if the import process is failed, it will return the following message;

```javascript
{"importState":"Failed","description":"Root cause of the failure"}
```

**/api/file/data**: is a GET endpoint that returns the data for the given session.Endpoint supports pagination if required query parameters (pageNum and pageSize) are provided.  
*Sample request : /api/file/data?session=20181110200801&pageNum=2&pageSize=10*

```javascript

{"TotalCount":1000,"Items":[{"Key":"2800104","ArtikelCode":"2","ColorCode":"broek",
"Description":"Gaastra","Price":8.0,"DiscountPrice":0.0,"DeliveredIn":"1-3 werkdagen","Q1":"baby","Size":"104","Color":"grijs"},
{"Key":"00000002groe56","ArtikelCode":"2","ColorCode":"broek","Description":"Gaastra",
"Price":8.0,"DiscountPrice":0.0,"DeliveredIn":"1-3 werkdagen","Q1":"baby","Size":"56","Color":"groen"}
...]}
```


## Frontend  



Frontend is an Angular 7 project that is composed of 2 component;  
- **Import-File component**:  Import file component is a single form that user can upload a file.Since on the API side file processing is designed async, import-file component is capable to trace the status of import process.Once a file is submitted for upload, an interval is started and checks the status for every 500 ms till status is returned 'Completed' or 'Failed'.  

![File Upload Form](https://github.com/brcdbrcd/FileImportApp/blob/master/screenshots/FileUpload.PNG "File Upload Form")  

- **File-view component**: File-view component is a simple Angular material table that lists the data. Since on production it is expected to face big files, datatable is configured send request per page.  

![View File Data](https://github.com/brcdbrcd/FileImportApp/blob/master/screenshots/ViewData.PNG "View File Data")  

## How to start   
- **MongoDb**: Go to FileImportApp.API/FileImportApp.API/_MongoDBDocker and execute "docker-compose -f mongodb.yml up -d"  
- **Frontend**: go to FileImportApp-SPA folder and execute "ng serve"  
- **Backend**": go to FileImportApp-API/FileImportApp.API folder and execute "dotnet watch run"  
