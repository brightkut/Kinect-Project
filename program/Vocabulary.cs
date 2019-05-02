using System;
using System.Collections.Generic;
using Microsoft.Speech.Recognition;
using System.Windows.Media.Imaging;

namespace Kinect_v2_Learning
{
    public class Vocabulary
    {

        /// <summary>
        /// Speech Dictionary
        /// </summary>
        public Choices Speech_Dictionary = new Choices();


        public Vocabulary()
        {
            // Speech_Dictionary.Add(new SemanticResultValue("How are you", "How are you"));
            Speech_Dictionary.Add(new SemanticResultValue("Home", "Home"));
            Speech_Dictionary.Add(new SemanticResultValue("Hello", "Hello"));
            Speech_Dictionary.Add(new SemanticResultValue("Yes", "Yes"));
            Speech_Dictionary.Add(new SemanticResultValue("No", "No"));
            Speech_Dictionary.Add(new SemanticResultValue("Banana", "Banana"));
     
            Speech_Dictionary.Add(new SemanticResultValue("Sleep", "Sleep"));
            Speech_Dictionary.Add(new SemanticResultValue("Apple", "Apple"));
            Speech_Dictionary.Add(new SemanticResultValue("Morning", "Morning"));
            Speech_Dictionary.Add(new SemanticResultValue("Love", "Love"));

        }

    }
    
}
