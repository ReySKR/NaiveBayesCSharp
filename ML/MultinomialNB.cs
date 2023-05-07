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
        
        private Vectorizer vectorizedData;
        //Anzahl der Sätze je Condition
        private double priorPositiveFeatures;
        private double priorNegativeFeatures;
        //Anzahl der Wörter je Condition
        private Vector priorPositiveConditionals;
        private Vector priorNegativeConditionals;
        //Anzahl der Wörter gesamt je Condition
        private int countPositiveWords;
        private int countNegativeWords;



        public MultinomialNB(Vectorizer tVectorizer)
        {
            vectorizedData= tVectorizer;
            priorPositiveConditionals = new Vector(vectorizedData.countVector[0].vector.Length);
            priorNegativeConditionals = new Vector(vectorizedData.countVector[0].vector.Length);

            //Berechnung der Priorwahrscheinlichkeit der Features
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
            //Zählen aller Wörter
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

            
            //Es wird jeweils ein alpha = 1 addiert, somit erhalten wir nie eine Wahrscheinlichkeit von 0 bei beibehaltung des Verhältnisses
            //Sofern im folgenden natürlich Gesamtanzahl angepasst wird -> dh. tatsächliche Anzahl + Anzahl der Wörter (da auf jedes Wort eines Addiert wird)
            
                for (int k = 0; k < Vector.componentDefinition.Length; k++)
                {
                    priorPositiveConditionals.vector[k]++;
                    priorNegativeConditionals.vector[k]++;
                }
            
            

            //Zählen aller Wörter im Gesamten
            for (int i = 0; i < priorPositiveConditionals.vector.Length; i++)
            {
                countPositiveWords += priorPositiveConditionals.vector[i];
                countNegativeWords += priorNegativeConditionals.vector[i];
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
                int wordIndex = -1;
                //Im Vector der gezählten Wörter nach jeweiligem Count nachsehen
                for (int k = 0; k < Vector.componentDefinition.Length; k++)
                {
                    
                    if (Vector.componentDefinition[k] == words[i].ToLower())
                    {
                        wordIndex= k;
                    }
                }
                //Falls wort nicht gefunden
                if (wordIndex == -1) {
                    continue;
                }

                //Neuberechnung der jeweiligen Wahrscheinlichkeit für positive und negative
                //Es könnte jeweils ein alpha = 1 addiert, somit erhalten wir nie eine Wahrscheinlichkeit von 0 bei beibehaltung des Verhältnisses
                double posCount = Convert.ToDouble(priorPositiveConditionals.vector[wordIndex]);
                positiveProbabilities *= posCount / Convert.ToDouble(countPositiveWords);
                double negCount = Convert.ToDouble(priorNegativeConditionals.vector[wordIndex]);
                negativeProbabilities *= negCount / Convert.ToDouble(countNegativeWords);
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
                Console.WriteLine("Das Ergebnis ist ungenau");
            }
        }




    }
}
