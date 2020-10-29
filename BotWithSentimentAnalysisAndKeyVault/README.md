# BotWithSentimentAnalysisAndKeyVault

https://github.com/shahedc/GruutChatbot/blob/master/GruutChatbot/Bots/EchoBot.cs

https://daveabrock.com/2020/07/28/azure-bot-service-cognitive-services

https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-tutorial-basic-deploy?view=azure-bot-service-4.0&tabs=csharp%2Cvs


This bot has been created using [Bot Framework](https://dev.botframework.com), it shows the minimum code required to build a bot.

## Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) version 3.1

  ```bash
  # determine dotnet version
  dotnet --version
  ```

## To try this sample

- In a terminal, navigate to `BotWithSentimentAnalysisAndKeyVault`

    ```bash
    # change into project folder
    cd BotWithSentimentAnalysisAndKeyVault
    ```

- Run the bot from a terminal or from Visual Studio, choose option A or B.

  A) From a terminal

  ```bash
  # run the bot
  dotnet run
  ```

  B) Or from Visual Studio

  - Launch Visual Studio
  - File -> Open -> Project/Solution
  - Navigate to `BotWithSentimentAnalysisAndKeyVault` folder
  - Select `BotWithSentimentAnalysisAndKeyVault.csproj` file
  - Press `F5` to run the project

## Testing the bot using Bot Framework Emulator

[Bot Framework Emulator](https://github.com/microsoft/botframework-emulator) is a desktop application that allows bot developers to test and debug their bots on localhost or running remotely through a tunnel.

- Install the Bot Framework Emulator version 4.3.0 or greater from [here](https://github.com/Microsoft/BotFramework-Emulator/releases)

### Connect to the bot using Bot Framework Emulator

- Launch Bot Framework Emulator
- File -> Open Bot
- Enter a Bot URL of `http://localhost:3978/api/messages`

## Deploy the bot to Azure

To learn more about deploying a bot to Azure, see [Deploy your bot to Azure](https://aka.ms/azuredeployment) for a complete list of deployment instructions.

az login
az account set --subscription d86c71a8-aa44-48b1-9bdb-1f27e3f30bd0

Create AAD App Registration (Microsoft App Id + Password)
az ad app create --display-name "MibonBotWithSentimentAndKV" --password "AtLeastSixteenCharacters_0"

az deployment sub create --template-file "template-with-new-rg.json" --location westus --parameters appId="521048b1-56ec-40c5-b03f-3bf66be8fed9" appSecret="AtLeastSixteenCharacters_0" botId="MibonSentimentKVBot" botSku=F0 newAppServicePlanName="MibonBotServicePlan" newWebAppName="MibonSentimentKVBotWebApp" groupName="MibonSentimentKVbot" groupLocation="westus" newAppServicePlanLocation="westus" --name "MibonSentimentKV Deployment"

## Further reading

- [Bot Framework Documentation](https://docs.botframework.com)
- [Bot Basics](https://docs.microsoft.com/azure/bot-service/bot-builder-basics?view=azure-bot-service-4.0)
- [Activity processing](https://docs.microsoft.com/en-us/azure/bot-service/bot-builder-concept-activity-processing?view=azure-bot-service-4.0)
- [Azure Bot Service Introduction](https://docs.microsoft.com/azure/bot-service/bot-service-overview-introduction?view=azure-bot-service-4.0)
- [Azure Bot Service Documentation](https://docs.microsoft.com/azure/bot-service/?view=azure-bot-service-4.0)
- [.NET Core CLI tools](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x)
- [Azure CLI](https://docs.microsoft.com/cli/azure/?view=azure-cli-latest)
- [Azure Portal](https://portal.azure.com)
- [Language Understanding using LUIS](https://docs.microsoft.com/en-us/azure/cognitive-services/luis/)
- [Channels and Bot Connector Service](https://docs.microsoft.com/en-us/azure/bot-service/bot-concepts?view=azure-bot-service-4.0)
