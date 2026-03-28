using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Xml;
using System.Xml.Schema;
/**
* This template file is created for ASU CSE445 Distributed SW Dev Assignment 4.
* Please do not modify or delete any existing class/variable/method names.
However, you can add more variables and functions.
* Uploading this file directly will not pass the autograder's compilation check,
resulting in a grade of 0.
* **/
namespace ConsoleApp1
{
    public class Submission
    {
        public static string xmlURL = "https://angelaprojtz.github.io/cse445_a4/NationalParks.xml";
        public static string xmlErrorURL = "https://angelaprojtz.github.io/cse445_a4/NationalParksErrors.xml";
        public static string xsdURL = "https://angelaprojtz.github.io/cse445_a4/NationalParks.xsd";

        public static string exceptionMessage = string.Empty; //to be used by both ValidationCallBack and Verification
        public static void Main(string[] args)
        {
            string result = Verification(xmlURL, xsdURL);
            Console.WriteLine(result);
            result = Verification(xmlErrorURL, xsdURL);
            Console.WriteLine(result);
            result = Xml2Json(xmlURL);
            Console.WriteLine(result);
        }
        // Q2.1
        public static string Verification(string xmlUrl, string xsdUrl)
        {
            //return "No Error" if XML is valid. Otherwise, return the desired
            //exception message.
            //uses example at p. 226 in book

            exceptionMessage = string.Empty; //we need to reset the exception message for each call
            XmlSchemaSet parkSchema = new XmlSchemaSet();

            parkSchema.Add(null, xsdUrl);
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = parkSchema;
            settings.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

            XmlReader reader = XmlReader.Create(xmlUrl, settings);

            try //exception handling for certain errors that are not handled by the ValidationCallBack (NationalParksErrors.xml)
            {
                while (reader.Read()) ;
            }
            catch (XmlException e)
            {
                exceptionMessage += e.Message + "\n";
            }

            if (string.IsNullOrEmpty(exceptionMessage))
            {
                exceptionMessage = "No errors are found";
                return exceptionMessage;
            }
            else
            {
                return exceptionMessage;
            }
        }

        private static void ValidationCallBack(object sender, ValidationEventArgs e)
        {
            exceptionMessage += e.Message + "\n";
        }
        public static string Xml2Json(string xmlUrl)
        {
            string xmlString = DownloadContent(xmlUrl); //xml contents are currently in string format

            //https://learn.microsoft.com/en-us/dotnet/api/system.xml.xmldocument.loadxml?view=net-10.0
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString); //load the xml string into an XmlDocument

            // The returned jsonText needs to be de-serializable by Newtonsoft.Json

            //https://stackoverflow.com/questions/814001/how-to-convert-json-to-xml-or-xml-to-json-in-c
            string jsonText = JsonConvert.SerializeXmlNode(doc);

            return jsonText;
        }
        // Helper method to download content from URL
        private static string DownloadContent(string url)
        {
            using (System.Net.WebClient client = new System.Net.WebClient())
            {
                return client.DownloadString(url);
            }
        }
    }
}