# MSMConfigUtil
## What it is
MSMConfigUtil is a command line utility that enables manual and automated transfer of Microsoft Sustainability Manager configuration items between environments.
Currently, only Calculation Models are supported.
Users can leverage MSMConfigUtil to manually copy one or all custom Calculation Models from one environment to another.
MSMConfigUtil can also be integrated into deployment pipelines to automate the promotion of Calculation Models from dev/test environments to production.

## Disclaimer
This is not an official tool and it is not officially supported.

## Get started
1. Get the latest version of MSMConfigUtil
    For Windows x64 users:
    - Download the single-file self-containing executable [MSMConfigUtil-win-x64.zip](https://github.com/AndreaC-MSFT/MSMConfigUtil/releases/latest/download/MSMConfigUtil-win-x64.zip) (this is the single-file self-containing executable)

    For all other users (including non-windows or arm):
    - Download [MSMConfigUtil-portable.zip](https://github.com/AndreaC-MSFT/MSMConfigUtil/releases/latest/download/MSMConfigUtil-portable.zip) 

1. Unzip the content
1. Open a terminal in the MSMConfigUtil directory
1. Type the following command to get guidance on available commands and options

    ```bash
    ./MSMConfigUtil migrate-calculation-models --help
    ```
### Usage examples
The following example copies a calculation model with name _My calculation model_ from `environment-A.crm.dynamics.com` to `environment-B.crm.dynamics.com`.
```bash
./MSMConfigUtil migrate-calculation-models --name "My calculation model" --source-uri https://environment-A.crm.dynamics.com/ --destination-uri https://environment-A.crm.dynamics.com/
```
For safety reasons, if a calculation model with the same name already exists in the destination environment, MSMConfigUtil does NOT replace it by default. If you want to replace existing models, you need to specify `--replace-existing` as in the following example.
```bash
./MSMConfigUtil migrate-calculation-models --name "My calculation model" --source-uri https://environment-A.crm.dynamics.com/ --destination-uri https://environment-A.crm.dynamics.com/ --replace-existing
```
Instead of specifying the name of a calculation model, you can specify the `--all` option. In this case, MSMConfigUtil will attempt to migrate all _custom_ calculation models available in the source environment.
> MSMConfigUtil only migrates **custom** calculation models. It does NOT migrate **standard** and **demo** models.
Here is an example that copies all custom calculation models from `environment-A.crm.dynamics.com` to `environment-B.crm.dynamics.com`.
```bash
./MSMConfigUtil migrate-calculation-models --all --source-uri https://environment-A.crm.dynamics.com/ --destination-uri https://environment-A.crm.dynamics.com/
```

## Authentication
MSMConfigUtil supports **interactive OAuth authentication** (the default) and **client secret authentication**.

### Interactive authentication
When interactive authentication is used, MSMConfigUtil opens two browser windows (one for the source environment and one for the destination one) so that user can authenticate using their Entra Id (formerly known as Azure Active Directory) credentials.

> Since interactive authentication relies on user interaction, it cannot be used in unattended automation scenario such as a deployment pipeline.

Interactive authentication is the default but you can also set it explicitly by specifying the following:
```bash
--auth-type Interactive
```
MSMConfigUtil uses a built-in app registration (app id) by default. However, you can also use your own app registration by specifying the `--client-id` option as following.
```bash
--auth-type Interactive --client-id "replace with your app id"
```
### Client secret authentication
This is also known as _OAuth Client Credential Flow_ or _Service Principal Auth_. When this authentication method is specified, the user must specify the client id and the secret which is the used to obtain the authentication tokens. The same credentials are used to authenticate toward the source and destination environments.
This is the preferred method for unattended execution such as in deployment pipeline integration scenarios.
```bash
./MSMConfigUtil migrate-calculation-models --all --source-uri https://environment-A.crm.dynamics.com/ --destination-uri https://environment-A.crm.dynamics.com/ --auth-type ClientIdAndSecret --client-id "replace with your app id" --client-secret "replace with your secret"
```
## Full usage and options
### Usage
```
MSMConfigUtil migrate-calculation-models [options]
```
### Options

| Option | Description |
|:-----|:----|
|-n, --calculation-model-name, --name \<calculation-model-name\>|The name of the calculation model|
|--all|Migrate all custom calculation models found in the source system|
|--replace-existing|Replace the calculation model if it already exists in the destination environment [default: False]|
|-s, --source-environment-uri, --source-uri \<source-environment-uri\> (REQUIRED)|The Dataverse URI of the source environment|
|-d, --destination-environment-uri, --destination-uri \<destination-environment-uri\> (REQUIRED)|The Dataverse URI of the destination environment|
|--auth-type \<ClientIdAndSecret|Interactive\>|The authentication type [default: Interactive]|
|--client-id \<client-id\>|The OAuth client ID (or App registration Id). Only used with --auth-type ClientIdAndSecret.|
|--client-secret \<client-secret\>|The client secret. Only used with --auth-type ClientIdAndSecret.|
|-?, -h,  --help| Show help and usage information|


## Limitations
- Currently MSMConfigUtil only supports Calculation Models. Support for additional configuration items might be added in the future.
- MSMConfigUtil only migrates **custom** calculation models. It does NOT migrate **standard** and **demo** models.
- Only interactive and client secret authentication is currently supported. Certificate authentication is currently not supported.
