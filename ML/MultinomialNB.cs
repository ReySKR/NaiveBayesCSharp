using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaiveBayes.ML
{
    public class MultinomialNB
    {
        
        Vectorizer vectorizedData;
        private double priorPositiveFeatures;
        private double priorNegativeFeatures;
        private Vector priorPositiveConditionals;
        private Vector priorNegativeConditionals;

        public MultinomialNB(Vectorizer tVectorizer)
        {
            vectorizedData= tVectorizer;
            priorPositiveConditionals = new Vector(vectorizedData.countVector[0].vector.Length);
            priorNegativeConditionals = new Vector(vectorizedData.countVector[0].vector.Length);

            //Berechnung der Priorwahrscheinlichkeit der Labels
            for (int i = 0; i < vectorizedData.isPositive.Length; i++)
            {
                if (vectorizedData.isPositive[i] == true) {
                    priorPositiveFeatures++;
                }
                else
                {
                    priorNegativeFeatures++;
                }
            }
        }

        public void fit()
        {
            //Zählen aller Wörter, welche positive Features enthalten
            //Für jedes Feature
            for (int i = 0; i < vectorizedData.countVector.Length; i++)
            {
                //Jedes Wort durchgehen und zählen
                for (int k = 0; k < vectorizedData.countVector[i].vector.Length; k++)
                {
                    //Wenn Satz positiv, dann in positiveConditionals zählen
                    if (vectorizedData.isPositive[i] == true)
                    {
                        priorPositiveConditionals.vector[k] = priorPositiveConditionals.vector[k] + vectorizedData.countVector[i].vector[k];
                    }
                    else
                    {
                        priorNegativeConditionals.vector[k] = priorNegativeConditionals.vector[k] + vectorizedData.countVector[i].vector[k];
                    }
                }
            }
        }

        public void predict(string sentence)
        {
            //Satz in Wörter auftrennen. mittels selber Methode, wie in Vectorizer (sonst weichen womöglich Ergebnisse ab
            string[] words = vectorizedData.seperateWordsInSentence(sentence);
            
            //PriorProbablities berechnen
            double positiveProbabilities= priorPositiveFeatures / vectorizedData.isPositive.Length;
            double negativeProbabilities = priorNegativeFeatures / vectorizedData.isPositive.Length;
            


            //Für jedes Wort im gegeben Satz 
            for (int i = 0; i < words.Length; i++)
            {
                //Index des Wortes in jeweilgen Vektoren
                int wordIndex = 0;
                //Im Vector der gezählten Wörter nach jeweiligem Count nachsehen
                for (int k = 0; k < vectorizedData.countVector.Length; k++)
                {
                    if (Vector.componentDefinition[k] == words[i].ToLower())
                    {
                        wordIndex= k;
                    }
                }
                //Neuberechnung der jeweiligen Wahrscheinlichkeit für positive und negative
                double pos = Convert.ToDouble(priorPositiveConditionals.vector[wordIndex]);
                positiveProbabilities *= Convert.ToDouble(priorPositiveConditionals.vector[wordIndex]) / Convert.ToDouble(priorPositiveFeatures);
                double neg = Convert.ToDouble(priorNegativeConditionals.vector[wordIndex]);
                negativeProbabilities *= Convert.ToDouble(priorNegativeConditionals.vector[wordIndex]) / Convert.ToDouble(priorNegativeFeatures);
            }

            //Vergleich der Wahrscheinlichkeiten des Aufkommens

            if(positiveProbabilities > negativeProbabilities)
            {
                Console.WriteLine("Der Satz ist positiv");
            }
            else
            {
                Console.WriteLine("Der Satz ist negativ");
            }

            if(positiveProbabilities == negativeProbabilities)
            {
                Console.WriteLine("Das Ergebnis ist nicht eindeutig");
            }
        }




    }
}
