/*========================================================================================
    DecisionTreeNode                                                                 *//**
	
    A single node in a DecisionTree.
	
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
    DecisionTreeNode
========================================================================================*/
/**
    A single node in a DecisionTree.
*/
namespace COMP8901_Asg05
{
    public class DecisionTreeNode
    {
        /*------------------------------------------------------------------------------------
            Instance Fields
        ------------------------------------------------------------------------------------*/


        /*------------------------------------------------------------------------------------
            Instance Properties
        ------------------------------------------------------------------------------------*/
        public SysGeneric.List<Individual> _individuals { get; set; }
        public DecisionTree _tree { get; set; }
        public DecisionTreeNode _parent { get; set; }
        public SysGeneric.List<DecisionTreeNode> _children { get; set; }
        public SysGeneric.KeyValuePair<string, string> _parentSplitCondition { get; set; }
        public string _childrenSplitCondition { get; set; }

        /*------------------------------------------------------------------------------------
            Constructors & Destructors
        ------------------------------------------------------------------------------------*/
        /**
            Default DecisionTreeNode constructor.
        */
        public DecisionTreeNode(SysGeneric.List<Individual> newIndividuals = null)
        {
            _individuals = newIndividuals;
            _tree = null;
            _parent = null;
            _children = null;
            _parentSplitCondition = new SysGeneric.KeyValuePair<string, string>("", "");
            _childrenSplitCondition = "";
        }

        /*------------------------------------------------------------------------------------
            Instance Methods
        ------------------------------------------------------------------------------------*/
        /**
            Adds the given DecisionTreeNode as a child of this one.
        */
        public void AddChild(DecisionTreeNode child)
        {
            child._tree = _tree;
            child._parent = this;
            child._parentSplitCondition = new SysGeneric.KeyValuePair<string, string>(_childrenSplitCondition, "");
        }
    }
}
