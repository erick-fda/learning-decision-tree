/*========================================================================================
    Individual                                                                          *//**
	
    A single individual to be used by the decision tree AI.
	
    Copyright 2017 Erick Fernandez de Arteaga. All rights reserved.
        https://www.linkedin.com/in/erick-fda
        https://bitbucket.org/erick-fda
        
	@author Erick Fernandez de Arteaga
    @version 0.0.0
	@file
	
*//*====================================================================================*/

/*========================================================================================
	Dependencies
========================================================================================*/
using SysGeneric = System.Collections.Generic;

/*========================================================================================
    Individual
========================================================================================*/
/**
    A single individual to be used by the decision tree AI.
*/
namespace COMP8901_Asg05
{
    public class Individual
    {
        /*------------------------------------------------------------------------------------
            Instance Fields
        ------------------------------------------------------------------------------------*/


        /*------------------------------------------------------------------------------------
            Instance Properties
        ------------------------------------------------------------------------------------*/
        public string _name { get; set; }
        public string _classification { get; set; }
        public SysGeneric.Dictionary<string, string> _attributes { get; set; }

        /*------------------------------------------------------------------------------------
            Constructors & Destructors
        ------------------------------------------------------------------------------------*/
        /**
            Individual constructor taking parameters for name and classification.
        */
        public Individual(string newName, string newClassification)
        {
            _name = newName;
            _classification = newClassification;
            _attributes = new SysGeneric.Dictionary<string, string>();

            foreach (string eachAttribute in COMP8901_Asg05._attributes)
            {
                _attributes.Add(eachAttribute, "");
            }
        }

        /*------------------------------------------------------------------------------------
            Instance Methods
        ------------------------------------------------------------------------------------*/
        /**
            Prints the individual's name, classification, and attribute values.
        */
        public override string ToString()
        {
            string output = System.String.Format(
                "{0}\n" +
                "\tClassificiation: {1}", 
                _name, 
                _classification);

            foreach (SysGeneric.KeyValuePair<string, string> attribute in _attributes)
            {
                output += System.String.Format("\n\t{0}: {1}", attribute.Key, attribute.Value);
            }

            return output;
        }
    }
}
