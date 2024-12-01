using Geotab.Checkmate;
using Geotab.Checkmate.ObjectModel;
using Geotab.Checkmate.ObjectModel.Engine;


namespace geotab_challenge
{
    public class GeotabService
    {

        private string _server = "";
        private string _database = "";
        private string _username = "";
        private string _password = "";
        private API? api;
        public GeotabService(string server, string database, string username, string password)
        {
            _server = server;
            _database = database;
            _username = username;
            _password = password;
        }

        public bool Connect()
        {
           
            try
            {
                api = new API(_username, _password, null, _database, _server);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        // Method to obtain vehicle data
        public async Task<List<VehicleData>> GetVehicleDataAsync(int minutes)
        {

            var toDate = DateTime.UtcNow;
            var fromDate = toDate.AddMinutes(-minutes);


            if (api == null)
            {
                return new List<VehicleData>();
            }


            //
            // Make API call for all devices
            var devices = await api.CallAsync<IList<Device>>("Get", typeof(Device));
            if(devices == null)
            {
                return new List<VehicleData>();
            }
            //


            // The list of all vehicle vehicle readings
            var vehiculeReadings = new List<VehicleData>(devices.Count);

            Console.Write(" ");
            for (int i = 0; i < devices.Count; i++)
            {
                Device device = devices[i];

                //
                // Search for status data based on the current device and the odometer reading
                var statusDataSearch = new StatusDataSearch
                {
                    DeviceSearch = new DeviceSearch(device.Id),
                    DiagnosticSearch = new DiagnosticSearch(KnownId.DiagnosticOdometerAdjustmentId),
                    FromDate = fromDate,
                    ToDate = toDate
                };
                // Retrieve the odometer status data
                IList<StatusData>? statusData = await api.CallAsync<IList<StatusData>>("Get", typeof(StatusData), new { search = statusDataSearch });
                double odometerReading = 0;
                if (statusData != null && statusData.Count() > 0)
                {
                    odometerReading = (int)(statusData[0].Data ?? 0);
                }
                //


                //
                // Search for LogRecord data based on the current device and the position reading
                LogRecordSearch logRecordSearch = new()
                {
                    DeviceSearch = new DeviceSearch(device.Id),
                    FromDate = fromDate,
                    ToDate = toDate
                };
                // Retrieve the position data
                IList<LogRecord>? logRecords = await api.CallAsync<IList<LogRecord>>("Get", typeof(LogRecord), new { search = logRecordSearch });
                double latitudeReading = 0;
                double longitudeReading = 0;
                if (logRecords != null && logRecords.Count > 0)
                {
                    latitudeReading = logRecords[0].Latitude ?? 0;
                    longitudeReading = logRecords[0].Longitude ?? 0;
                }
                //
                GoDevice? myVehicle = device as GoDevice;

                vehiculeReadings.Add(new VehicleData(device.Id, device.SerialNumber ?? "", device.Name ?? "", myVehicle.VehicleIdentificationNumber ?? "", odometerReading, longitudeReading, latitudeReading));

                Console.Write(".");
            }

            return vehiculeReadings;

        }
    }
}
