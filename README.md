#MongoDbAppender

MongoDbAppender for Log4net.
## How to use
- Add MongoDbAppender.dll to your project, as well as MongoDB.Bson.dll and MongoDB.Driver.dll.
- Config as a normal log4net appender. For example

    ```xml
    <?xml version="1.0"?>
    <log4net>
      <appender name="MongoDbAppender" type="Log4net.Appender.MongoDb.MongoDbAppender, Log4net.Appender.MongoDb">
        <connectionStringName value="Logging"/>
        <collectionName value="test" />
        <bufferSize value="1" />
      </appender>
      <root>
        <level value="All" />
        <appender-ref ref="MongoDbAppender" />
      </root>
    </log4net>
    ```

  - `connectionStringName`: refers to the connection string name in app.config/web.config

      ```xml
        <connectionStrings>
          <add name="Logging" connectionString="mongodb://192.168.122.1/Logs"/>
        </connectionStrings>
      ```
      
  - `collectionName`: corresponding collection name in MongoDB. To avoid name duplication, a prefix `logs_` will be added. So for the example above, a collection `logs_test` will be created.
  - `bufferSize`: Buffer queue length. MongoDbAppender Stores logs in buffer before sending them to MongoDB.
