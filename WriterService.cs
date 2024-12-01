using Geotab.Checkmate.ObjectModel;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace geotab_challenge
{
    public interface IWriterService
    {
        void Write(IEnumerable<VehicleData> odometerReadings, string timestamp);
    }

    public class WriterCSVService: IWriterService
    {

        // Writes a CSV file
        public void Write(IEnumerable<VehicleData> odometerReadings, string timestamp)
        {

            foreach (var odometerReading in odometerReadings)
            {
                using (var writer = new StreamWriter(odometerReading.Name + ".csv", true))
                {
                    writer.WriteLine(
                        $"{odometerReading.Id}\t" +
                        $"{odometerReading.SerialNumber}\t" +
                        $"{odometerReading.Name}\t" +
                        $"{odometerReading.VIN}\t" +
                        $"{Math.Round(RegionInfo.CurrentRegion.IsMetric ? odometerReading.Mileage : Distance.ToImperial(odometerReading.Mileage / 1000), 0)}\t" +
                        $"{odometerReading.Longitude}\t" +
                        $"{odometerReading.Latitude}\t" +
                        $"{timestamp}");
                }
            }

        }

    }

    public class WriterXMLService : IWriterService
    {
       
        // Writes a XML file
        public void Write(IEnumerable<VehicleData> odometerReadings, string timestamp)
        {
            var isMetric = RegionInfo.CurrentRegion.IsMetric;

            foreach (var odometerReading in odometerReadings)
            {
                var filename = odometerReading.Name + ".xml";

                if (!File.Exists(filename))
                {                    

                    using (var writer = new XmlTextWriter(filename, Encoding.Unicode))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("DataVehicleExtract");
                        writer.WriteStartElement("Vehicle");
                        writer.WriteStartElement("Id");
                        writer.WriteString(odometerReading.Id != null ? odometerReading.Id.ToString() : "");
                        writer.WriteEndElement();
                        writer.WriteStartElement("SerialNumber");
                        writer.WriteString(odometerReading.SerialNumber);
                        writer.WriteEndElement();
                        writer.WriteStartElement("Name");
                        writer.WriteString(odometerReading.Name);
                        writer.WriteEndElement();
                        writer.WriteStartElement("VIN");
                        writer.WriteString(odometerReading.VIN);
                        writer.WriteEndElement();
                        writer.WriteStartElement("Odometer");
                        writer.WriteString(Math.Round(isMetric ? odometerReading.Mileage : Distance.ToImperial(odometerReading.Mileage / 1000), 0).ToString(CultureInfo.InvariantCulture));
                        writer.WriteEndElement();
                        writer.WriteStartElement("Longitude");
                        writer.WriteString(odometerReading.Longitude.ToString());
                        writer.WriteEndElement();
                        writer.WriteStartElement("Latitude");
                        writer.WriteString(odometerReading.Latitude.ToString());
                        writer.WriteEndElement();
                        writer.WriteStartElement("TimeStamp");
                        writer.WriteString(timestamp);
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndElement();
                        writer.WriteEndDocument();

                        writer.Flush();
                        writer.Close();

                    }
                }
                else
                {
                    XDocument xDocument = XDocument.Load(filename);
                    XElement root = xDocument.Element("DataVehicleExtract");
                    IEnumerable<XElement> rows = root.Descendants("Vehicle");
                    XElement firstRow = rows.First();
                    firstRow.AddBeforeSelf(
                       new XElement("Vehicle",
                       new XElement("Id", odometerReading.Id != null ? odometerReading.Id.ToString() : ""),
                       new XElement("SerialNumber", odometerReading.SerialNumber),
                       new XElement("Name", odometerReading.Name),
                       new XElement("VIN", odometerReading.VIN),
                       new XElement("Odometer", Math.Round(isMetric ? odometerReading.Mileage : Distance.ToImperial(odometerReading.Mileage / 1000), 0).ToString(CultureInfo.InvariantCulture)),
                       new XElement("Longitude", odometerReading.Longitude.ToString()),
                       new XElement("Latitude", odometerReading.Latitude.ToString()),
                       new XElement("TimeStamp", timestamp)));
                    xDocument.Save(filename);
                }


            }



        }

    }


}
