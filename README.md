# Developers Hub API

This is the API that provides the data for the [Developers Hub project](https://github.com/LBHackney-IT/developers-hub-frontend).

## Stack

- .NET Core as a web framework.
- nUnit as a test framework.
- DynamoDB as a datastore.

## Contributing

### Setup

1. Install [Docker][docker-download].
2. Install [AWS CLI][AWS-CLI].
3. Clone this repository.
4. Open it in your IDE.

### Development

To serve the application, run it using your IDE of choice, we use Visual Studio CE and JetBrains Rider on Mac.

**Note**
When running locally the appropriate database conneciton details are still needed.
##### DynamoDb
To use a local instance of DynamoDb, this will need to be installed. This is most easily done using [Docker](https://www.docker.com/products/docker-desktop).
Run the following command, specifying the local path where you want the container's shared volume to be stored.
```
docker run --name dynamodb-local -p 8000:8000 -v <PUT YOUR LOCAL PATH HERE>:/data/ amazon/dynamodb-local -jar DynamoDBLocal.jar -sharedDb -dbPath /data
```

If you would like to see what is in your local DynamoDb instance using a simple gui, then [this admin tool](https://github.com/aaronshaf/dynamodb-admin) can do that.

The application can also be served locally using docker:
1.  Add you security credentials to AWS CLI.
```sh
$ aws configure
```
2. Log into AWS ECR.
```sh
$ aws ecr get-login --no-include-email
```
3. Build and serve the application. It will be available in the port 3000.
```sh
$ make build && make serve
```

### Creating A PR

To help with making changes to code easier to understand when being reviewed, we've added a PR template.
When a new PR is created on a repo that uses this API template, the PR template will automatically fill in the `Open a pull request` description textbox.
The PR author can edit and change the PR description using the template as a guide.

### Run the tests

```sh
$ make test
```

## Contacts

### Active Maintainers

- **Selwyn Preston**, Lead Developer at London Borough of Hackney (selwyn.preston@hackney.gov.uk)
- **Mirela Georgieva**, Lead Developer at London Borough of Hackney (mirela.georgieva@hackney.gov.uk)
- **Matt Keyworth**, Lead Developer at London Borough of Hackney (matthew.keyworth@hackney.gov.uk)

### Other Contacts

- **Rashmi Shetty**, Product Owner at London Borough of Hackney (rashmi.shetty@hackney.gov.uk)

[docker-download]: https://www.docker.com/products/docker-desktop
[AWS-CLI]: https://aws.amazon.com/cli/
