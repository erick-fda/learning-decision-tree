/*========================================================================================
    DecisionTree                                                                     *//**
	
    A decision tree containing the results of an ID3 algorithm executed on an initial 
    collection of Individuals.
	
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


/*========================================================================================
    DecisionTree
========================================================================================*/
/**
    A decision tree containing the results of an ID3 algorithm executed on an initial 
    collection of Individuals.
*/
namespace COMP8901_Asg05
{
    public class DecisionTree
    {
        /*------------------------------------------------------------------------------------
            Instance Fields
        ------------------------------------------------------------------------------------*/


        /*------------------------------------------------------------------------------------
            Instance Properties
        ------------------------------------------------------------------------------------*/
        private DecisionTreeNode _root { get; set; }

        /*------------------------------------------------------------------------------------
            Constructors & Destructors
        ------------------------------------------------------------------------------------*/
        /**
            Default DecisionTree constructor.
        */
        public DecisionTree()
        {
            _root = null;
        }

        /**
            Constructor taking the root node as a parameter.
        */
        public DecisionTree(DecisionTreeNode newRoot)
        {
            _root = newRoot;
            newRoot._tree = this;
        }

        /*------------------------------------------------------------------------------------
            Instance Methods
        ------------------------------------------------------------------------------------*/

    }
}
