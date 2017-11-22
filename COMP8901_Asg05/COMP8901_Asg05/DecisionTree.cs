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
    }
}
