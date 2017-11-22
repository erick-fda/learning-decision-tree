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
using SysGeneric = System.Collections.Generic;

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
        public DecisionTreeNode _root { get; set; }
        public SysGeneric.HashSet<string> _splitConditions { get; set; }
        private System.Random _randomGenerator { get; set; }

        /*------------------------------------------------------------------------------------
            Constructors & Destructors
        ------------------------------------------------------------------------------------*/
        /**
            Default DecisionTree constructor.
        */
        public DecisionTree()
        {
            _root = null;
            _splitConditions = new SysGeneric.HashSet<string>();
            _randomGenerator = new System.Random();
        }

        /**
            Constructor taking the root node as a parameter.
        */
        public DecisionTree(DecisionTreeNode newRoot)
        {
            _root = newRoot;
            _root._tree = this;
            _root._parent = null;
            _root._pastSplitConditions = new SysGeneric.Dictionary<string, string>();
            _splitConditions = new SysGeneric.HashSet<string>();
            _randomGenerator = new System.Random();
        }

        /*------------------------------------------------------------------------------------
            Instance Methods
        ------------------------------------------------------------------------------------*/
        /**
            Generate the optimal decision tree based on the root node.
        */
        public void GenerateOptimalTree()
        {
            if ( _root == null )
            {
                throw new System.Exception("Root node is null.");
            }

            _root.SplitDown();
        }

        /**
            Returns the projected classification for the given individual based on the tree.
        */
        public string PredictClassfication(Individual individual)
        {
            string classification = null;
            DecisionTreeNode currentNode = _root;

            /* Travel down through the tree based on the individual's attributes and determine 
                the best guess for its classification. */
            while ( true )
            {
                /* If this is a leaf node, classify the individual and break out of the loop. */
                if ( currentNode.IsLeafNode() )
                {
                    /* If there were no samples with these attributes in the classification data, 
                        return the most common classification in the training data. */
                    if ( currentNode._classificationRatio < 0 )
                    {
                        classification = COMP8901_Asg05._commonClassification;
                    }
                    /* If there were matching samples, classify based on that. */
                    else
                    {
                        classification = (currentNode._classificationRatio >= 0.5) ?
                                            COMP8901_Asg05._classifications[0] :
                                            COMP8901_Asg05._classifications[1];
                    }

                    break;
                }

                /* Determine the next attribute to branch on and get the value for this individual. */


                /* Determine which child node to branch into. */

            }

            return classification;
        }
    }
}
