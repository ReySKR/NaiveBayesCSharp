﻿

using NaiveBayes.ML;
using NaiveBayes.Model;

namespace NaiveBayes
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Vlt fixen mit den Schrägstrichen
            CSVReader reader = new CSVReader("dataset", "\"positive\"");
            Dictionary<string, bool> result = reader.csvToDictionary();
            Vectorizer vc = new Vectorizer(result);
            MultinomialNB nb = new MultinomialNB(vc);
            nb.fit();
            while (true)
            {
                Console.WriteLine("Gebe Satz ein:");
                nb.predict(Console.ReadLine());
            }


        }
    }
}