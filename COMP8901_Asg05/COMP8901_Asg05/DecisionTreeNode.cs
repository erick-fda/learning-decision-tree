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
        private SysGeneric.List<Individual> _pIndividuals;

        /*------------------------------------------------------------------------------------
            Instance Properties
        ------------------------------------------------------------------------------------*/
        public SysGeneric.List<Individual> _individuals
        {
            get { return _pIndividuals; }

            /* Update entropy when setting individuals. */
            set
            {
                _pIndividuals = value;
                _entropy = CalculateEntropy();
            }
        }

        public DecisionTree _tree { get; set; }
        public DecisionTreeNode _parent { get; set; }
        public SysGeneric.List<DecisionTreeNode> _children { get; set; }
        public SysGeneric.KeyValuePair<string, string> _parentSplitCondition { get; set; }
        public string _childrenSplitCondition { get; set; }
        public double _entropy { get; private set; }

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

        /**
            Returns the entropy of the collection of individuals contained by this node.
            
            This method should be used to update the entropy of the node every time the 
            collection of individuals changes.
        */
        private double CalculateEntropy()
        {
            /* If the collection has no members or is null, entropy is zero. */
            if (_individuals.Count < 1 || 
                _individuals == null)
            {
                return 0;
            }

            /* Determine how many individuals of each classification are present in the 
                collection. */
            int nPositive = 0;
            int nNegative = 0;

            foreach (Individual eachIndividual in _individuals)
            {
                if (eachIndividual._classification == COMP8901_Asg05._classifications[0])
                {
                    nPositive++;
                }
                else
                {
                    nNegative++;
                }
            }

            return Entropy(nPositive, nNegative);
        }

        /**
            Return the result of an entropy calculation on the given n+ and n- values.
        */
        private double Entropy(double nPositive, double nNegative)
        {
            double n = nPositive + nNegative;
            double nPosN = nPositive / n;
            double nNegN = nNegative / n;

            return (-System.Math.Log(nPosN, 2) * nPosN) - (System.Math.Log(nNegN, 2) * nNegN);
        }
    }
}
