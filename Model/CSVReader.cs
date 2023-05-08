using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaiveBayes.Model
{
    public class CSVReader
    {
        //Möglicherweise intelligenter lösen
        private readonly string _originPath = Directory.GetParent(Directory.GetParent(Directory.GetParent(Directory.GetCurrentDirectory()).ToString()).ToString()).ToString();
        //Ordner in dem sich DataSet befindet
        private string _dataSetFolder;
        //Label mit true als Bool
        private string _labelIdentity;

        public CSVReader(string dataSetFolder, string labelIdentity)
        {
            _dataSetFolder = dataSetFolder;
            _labelIdentity = labelIdentity;
            
        }

        //Jede Zeile lesen
        private string[] csvToStringByRow()
        {
            string[] bla = File.ReadAllLines(Path.Combine(_originPath, _dataSetFolder, "dataset_raw.csv"));

            foreach(string line in bla)
            {
                Console.WriteLine(line);
            }


            return File.ReadAllLines(Path.Combine(_originPath, _dataSetFolder, "dataset_raw.csv"));

        }

        //Zeilen in Dictionary wandeln
        private Dictionary<string, bool> stringArrToDictionary(string[] csvByLines) {
            Dictionary<string, bool> textLabelPair= new Dictionary<string, bool>();
            string[] seperatedString;
            foreach (string line in csvByLines)
            {
                seperatedString = line.ToLower().Split(",");
                try
                {
                    textLabelPair.Add(seperatedString[0], seperatedString[1] == _labelIdentity ? true : false);
                }
                catch (Exception)
                {
                    textLabelPair.Remove(seperatedString[0]);

                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("DUPLICATE FOUND");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                
            }

            return textLabelPair;
        }

        //Dictionary ausgeben
        public Dictionary<string, bool> csvToDictionary()
        {
            return stringArrToDictionary(csvToStringByRow());
        }


        
    }
}
