using geotab_challenge;

namespace Geotab_Challenge
{

    // Define the callback function signature to write data a localfile
    delegate void CallbackFunction(IEnumerable<VehicleData> odometerReadings, string timestamp);

    /// <summary>
    /// Main program
    /// </summary>
    class Program
     {
        /// <summary>
        /// This is a Geotab API program to download vehicle mileage and position to a CSV or XML.
        ///
        /// Steps:
        /// 1) Create Geotab API object from supplied arguments and authenticate.
        /// 2) Get the odometer readings and position of each device and set a  VehiculeData object.
        /// 3) Output the information to a CSV file.
        ///
        /// </summary>
        /// <param name="args">The command line arguments for the application.</param>
        /// 

        static async Task Main(string[] args)
        {
            try
            {
                Console.WriteLine();
                Console.WriteLine(" Geotab SDK");

                if (args.Length != 5)
                {
                    Console.WriteLine();
                    Console.WriteLine(" Command line parameters:");
                    Console.WriteLine(" dotnet run <server> <database> <username> <password> <.csv or .xml>");
                    Console.WriteLine();
                    Console.WriteLine(" Example: dotnet.run server database username password extensionfile");
                    Console.WriteLine();
                    Console.WriteLine(" server     - Sever host name (Example: my.geotab.com)");
                    Console.WriteLine(" database   - Database name (Example: G560)");
                    Console.WriteLine(" username   - Geotab user name");
                    Console.WriteLine(" password   - Geotab password");
                    Console.WriteLine(" extensionfile - Extension of the output file (.csv or .xml)");

                    return;
                }

                // Command line argument variables
                var server = args[0];
                var database = args[1];
                var username = args[2];
                var password = args[3];
                var extensionfile = args[4];

                // Check the extension file
                if (!extensionfile.Equals(".csv", StringComparison.OrdinalIgnoreCase) && !extensionfile.Equals(".xml", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(" Unknown file type");
                    return;
                }

                Console.WriteLine(" Ctrl + C to finish");
                Console.WriteLine();
                Console.WriteLine(" Creating API...");
                GeotabService _servicio = new GeotabService(server, database, username, password);


                // Conect to Geotab API.
                Console.WriteLine(" Connecting...");
                _servicio.Connect();

                //Init function to write into a local file
                IWriterService functionWrite;
                functionWrite = (extensionfile.Equals(".csv", StringComparison.OrdinalIgnoreCase)) ? new WriterCSVService() : new WriterXMLService();

                //Get data from Geotab SDK and write into a local file
                await GetDataFromGeoTabAsync(_servicio, extensionfile, functionWrite.Write);


            }
            catch (Exception exception)
            {
                Console.WriteLine($" Exception: {exception.Message}\n\n{exception.StackTrace}");
            }
            finally
            {
                Console.WriteLine();
                Console.Write(" Press any key to close...");
                Console.ReadKey(true);
            }
        }


        public static async Task GetDataFromGeoTabAsync(GeotabService _servicio, string extensionfile, CallbackFunction callback)
        {

            while (true)
            {

                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");

                Console.WriteLine(" Retrieving devices...");
                Console.Write(" ");
                Console.ForegroundColor = ConsoleColor.White;
                var vehiclesData = await _servicio.GetVehicleDataAsync(1);
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.WriteLine();

                Console.Write(" ");
                Console.ForegroundColor = ConsoleColor.White;

                callback(vehiclesData, timestamp);

                Console.WriteLine();
                Console.WriteLine(" Extract complete");
                Console.WriteLine(" Next attempt, in 1 minute");

                Thread.Sleep(60000);

            }

        }

    }
}
