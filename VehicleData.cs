using Geotab.Checkmate.ObjectModel;

namespace geotab_challenge
{
    /// <summary>
    /// Models a <see cref="Device"/> with mileage data.
    /// </summary>
    public class VehicleData
    {

        /// <summary>
        /// The Vehicle Id
        /// </summary>
        public readonly Id? Id;

        /// <summary>
        /// The Vehicle SerialNumber
        /// </summary>
        public readonly string SerialNumber;

        /// <summary>
        /// The Vehicle Name
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// The Vehicle VIN
        /// </summary>
        public readonly string VIN;

        /// <summary>
        /// The mileage
        /// </summary>
        public readonly double Mileage;

        /// <summary>
        /// The Longitude
        /// </summary>
        public readonly double Longitude;

        /// <summary>
        /// The Latitude
        /// </summary>
        public readonly double Latitude;


        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleData"/> class.
        /// </summary>
        /// <param name="id">The Vehicle Id</param>
        /// <param name="mileage">The mileage</param>
        /// 
        public VehicleData(Id id, string serialNumber, string name, string vin, double mileage, double longitude, double latitude)
        {
            Id = id;
            SerialNumber = serialNumber;
            Name = name;
            VIN = vin;
            Mileage = mileage;
            Longitude = longitude;
            Latitude = latitude;
        }
    }
}
