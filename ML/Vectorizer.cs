using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaiveBayes.ML
{
    public class Vectorizer
    {
        //Vektoren und Arrays deklarieren (isPositive nur zur sauberen Trennung und Erklärung)
        public Vector[] countVector { get; }
        public bool[] isPositive { get; }
        //Dictionary zur Prozessierung der CSV Datei
        private Dictionary<string, bool> textLabelPair;
        

        public Vectorizer(Dictionary<string, bool> textLabelPair)
        {
            //Daten aus CSV Datei
            this.textLabelPair = textLabelPair;
            //Tupel bestehend aus (maxUniqueWordsCount Anzahl, UniqueWords Liste) initialisieren
            (int, List<string>) initializer = countNonUniqueWords(textLabelPair);

            countVector = new Vector[textLabelPair.Count];

            for (int i = 0; i < countVector.Length; i++)
            {
                countVector[i] = new Vector(initializer.Item1);
            }
            //Initilieseren des Statischen Vektor zur Erkennung der Wörter
            Vector.initializeComponents(initializer.Item2);
            //isPositive Array für Zuordnung intialisieren 
            isPositive = new bool[textLabelPair.Count];
            //Zählen der Wörter und Rezensionsart zuordnen
            transform();
        }

        public String[] seperateWordsInSentence(string sentence)
        {
            string toProcess = sentence;

            var charsToRemove = new string[] { "@", ",", ".", ";", "'", "!", ":", "\"" };
            foreach (var c in charsToRemove)
            {
                toProcess = toProcess.Replace(c, string.Empty);
            }

            string[] words = toProcess.Split(" ");
            return words;
        }

        private String[] seperateWordsInDictionary(KeyValuePair<string,bool> pair)
        {
            string toProcess = pair.Key;

            return seperateWordsInSentence(toProcess);
        }

        private (int, List<string>) countNonUniqueWords(Dictionary<string, bool> textLabelPair)
        {
            //Liste für Komponentenbezeichnung des Vektors initialisieren
            List<string> uniqueWords = new List<string>();
            //Maximale Anzahl an unterschiedlichen Wörtern initialisieren
            int maxWordCount = 0;
            foreach (var pair in textLabelPair)
            {
                string[] words = seperateWordsInDictionary(pair);

                
                for (int i = 0; i < words.Length; i++)
                {
                    if (!uniqueWords.Contains(words[i]))
                    {
                        maxWordCount++;
                        uniqueWords.Add(words[i]);
                    }
                }
            }
            return (maxWordCount, uniqueWords);
        }

        private void transform()
        {
            //Index des processedDataSetVectors initialisieren
            int indexOfSentenceInArray = 0;
            foreach(var pair in textLabelPair)
            {
                //Wörter in Satz separieren
                string[] words = seperateWordsInDictionary(pair);
                //Für jedes Wort Index des Wortes in CountVector bestimmen und jeweils an dieser Stelle Häufigkeit erhöhen
                for (int i = 0; i < words.Length; i++)
                {
                    int indexOfWordInDefinition = Array.IndexOf(Vector.componentDefinition, words[i]);
                    countVector[indexOfSentenceInArray].vector[indexOfWordInDefinition]++;
                }
                //Ermitteln auf Satz Eigenschaft (positive, negative)
                isPositive[indexOfSentenceInArray] = pair.Value == true ? true : false;
                //SatzIndex erhöhen, da nächster Satz
                indexOfSentenceInArray++;
            }
        }
    }
}
