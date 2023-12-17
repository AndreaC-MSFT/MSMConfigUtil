using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using MSMConfigUtil;
using System.CommandLine;

class Program
{
    // TODO Enter your Dataverse environment's URL and logon info.
    static string url = "https://org68addb97.crm11.dynamics.com/"; // https://psomsm.crm.dynamics.com/";
    //static string userName = "you@yourorg.onmicrosoft.com";
    //static string password = "yourPassword";
    //AppId = 51f81489-12ee-4a9e-aaae-a2591f45987d;
    // This service connection string uses the info provided above.
    // The AppId and RedirectUri are provided for sample code testing.
    static string connectionString = $@"
   AuthType = OAuth;
   Url = {url};
   AppId = 51f81489-12ee-4a9e-aaae-a2591f45987d;
   RedirectUri = http://localhost;
   LoginPrompt=Auto;
   RequireNewInstance = True";

    static async Task Main(string[] args)
    {
        var rootCommand = new RootCommand();
        var cliController = new CalculationModelControllerCLI(null);
        cliController.ConfigureCommands(rootCommand);
        await rootCommand.InvokeAsync(args);
        /*
        //ServiceClient implements IOrganizationService interface
        IOrganizationService service = new ServiceClient(connectionString);

        OrganizationServiceContext serviceContext = new OrganizationServiceContext(service);

        var query_contains3 = from c in serviceContext.CreateQuery("msdyn_emissioncalculation")
                              where (string)c["msdyn_name"] == "To delete"
                              select new
                              {
                                  name = c.Attributes["msdyn_name"],
                                  calcjson = c.Attributes["msdyn_calculationflowjson"]
                              };
        foreach (var c in query_contains3)
        {
            System.Console.WriteLine(c.name + " " + c.calcjson);
        }


        //var response = (WhoAmIResponse)service.Execute(new WhoAmIRequest());

        //Console.WriteLine($"User ID is {response.UserId}.");


        // Pause the console so it does not close.
        Console.WriteLine("Press the <Enter> key to exit.");
        Console.ReadLine();
        */
    }
}
