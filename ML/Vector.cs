using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaiveBayes.ML
{
    //Zur Berechnung der Wahrscheinlichkeiten könnte man eventuell zu generic wandeln
    public class Vector
    {
        //Vektoren deklarieren
        public int[] vector { get; set; }
        //Komponentdefinitionen deklarieren
        public static string[]? componentDefinition { get; set; }
        public Vector(int dimension)
        {
            //Vector mit Dimension intialisieren
            vector = new int[dimension];
        }

        public static void initializeComponents(List<string> uniqueWords)
        {
            //Komponentendefintion initialisieren
            componentDefinition = new string[uniqueWords.Count];

            //Definitionen hinzufügen
            for (int i = 0; i < uniqueWords.Count; i++)
            {
                componentDefinition[i] = uniqueWords[i];
            }
        }

    }
}
