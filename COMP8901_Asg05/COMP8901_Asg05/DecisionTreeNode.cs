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
using SysConsole = System.Console;

/*========================================================================================
	Dependencies
========================================================================================*/
public enum DtnSplitResult
{
    Split,
    ReachedLeafNode
}

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

            /* Update entropy and classification ratio when setting individuals. */
            set
            {
                _pIndividuals = value;
                _entropy = CalculateEntropy();
                _classificationRatio = CalculateClassificationRatio();
            }
        }

        public DecisionTree _tree { get; set; }
        public DecisionTreeNode _parent { get; set; }
        public SysGeneric.List<DecisionTreeNode> _children { get; set; }
        public string _childrenSplitCondition { get; set; }
        public double _entropy { get; private set; }
        public SysGeneric.Dictionary<string, string> _pastSplitConditions { get; set; }
        public double _classificationRatio { get; set; }

        /*------------------------------------------------------------------------------------
            Constructors & Destructors
        ------------------------------------------------------------------------------------*/
        /**
            Default DecisionTreeNode constructor.
        */
        public DecisionTreeNode(SysGeneric.List<Individual> newIndividuals = null)
        {
            _individuals = ( newIndividuals == null ) ? new SysGeneric.List<Individual>() : newIndividuals;
            _tree = null;
            _parent = null;
            _children = new SysGeneric.List<DecisionTreeNode>();
            _pastSplitConditions = new SysGeneric.Dictionary<string, string>();
            _childrenSplitCondition = "";
        }

        /*------------------------------------------------------------------------------------
            Instance Methods
        ------------------------------------------------------------------------------------*/
        /**
            Sets the given DecisionTreeNode as a child of this one.
            If addToTree is true, the parameter node will be added to this node's list of 
            children.
        */
        public void SetChild(DecisionTreeNode child, bool addToTree)
        {
            child._tree = _tree;
            child._parent = this;
            child._pastSplitConditions = new SysGeneric.Dictionary<string, string>(this._pastSplitConditions);

            if (addToTree)
            {
                this._children.Add(child);
            }
        }

        /**
            Returns the entropy of the given list of individuals.
        */
        private static double CalculateEntropy(SysGeneric.List<Individual> collection)
        {
            /* If the collection has less than 2 members or is null, entropy is zero. */
            if (collection.Count < 2 ||
                collection == null)
            {
                return 0;
            }

            /* Determine how many individuals of each classification are present in the 
                collection. */
            int nPositive = 0;
            int nNegative = 0;

            foreach (Individual eachIndividual in collection)
            {
                if ( eachIndividual._classification.Equals(COMP8901_Asg05._classifications[0]) )
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
            Returns the entropy of the list of individuals contained by this node.

            This method should be used to update the entropy of the node every time the 
            collection of individuals changes.
        */
        private double CalculateEntropy()
        {
            return CalculateEntropy(_individuals);
        }

        /**
            Return the result of an entropy calculation on the given n+ and n- values.
        */
        private static double Entropy(double nPositive, double nNegative)
        {
            /* If either of the given values is zero, entropy is zero. */
            if (nPositive <= 0 || 
                nNegative <= 0)
            {
                return 0;
            }

            double n = nPositive + nNegative;
            double nPosN = nPositive / n;
            double nNegN = nNegative / n;

            return (-System.Math.Log(nPosN, 2) * nPosN) - (System.Math.Log(nNegN, 2) * nNegN);
        }

        /**
            Calculate the average entropy of the child nodes that would result from splitting 
            this node on the given attribute.
        */
        public double ExpectedEntropyFromSplit(string splitCondition)
        {
            if (!COMP8901_Asg05._attributes.Contains(splitCondition))
            {
                throw new System.ArgumentException(
                    System.String.Format("{0} is not a valid attribute.", splitCondition));
            }

            /* Split this node's individual's on the given trait. */
            SysGeneric.List<Individual> positives = new SysGeneric.List<Individual>();
            SysGeneric.List<Individual> negatives = new SysGeneric.List<Individual>();
            string positiveSplitValue = COMP8901_Asg05._attributeValues[splitCondition][0];

            foreach (Individual eachIndividual in _individuals)
            {
                if ( eachIndividual._attributes[splitCondition].Equals(positiveSplitValue) )
                {
                    positives.Add(eachIndividual);
                }
                else
                {
                    negatives.Add(eachIndividual);
                }
            }

            /* Calculate the weighted entropy for the result sets. */
            double entropy = CalculateEntropy(positives) * positives.Count / _individuals.Count;
            entropy += CalculateEntropy(negatives) * negatives.Count / _individuals.Count;
            return entropy;
        }

        /**
            Calculate the information gain that would result from splitting this node on 
            the given attribute.
        */
        public double ExpectedUtilityFromSplit(string splitCondition)
        {
            return _entropy - ExpectedEntropyFromSplit(splitCondition);
        }

        /**
            Returns the optimal next attribute to split this node on.
        */
        public string DetermineBestSplitCondition()
        {
            /* Only consider attributes that have not already been split on. */
            SysGeneric.List<string> attributesToTest = new SysGeneric.List<string>();

            foreach (string eachAttribute in COMP8901_Asg05._attributes)
            {
                if (!_pastSplitConditions.ContainsKey(eachAttribute))
                {
                    attributesToTest.Add(eachAttribute);
                }
            }

            /* Calculate the information gain of splitting on each attribute 
                and keep the best one. */
            string bestAttribute = null;
            double bestAttributeUtility = -1;

            foreach (string eachAttribute in attributesToTest)
            {
                double eachUtility = ExpectedUtilityFromSplit(eachAttribute);

                if (eachUtility > bestAttributeUtility)
                {
                    bestAttribute = eachAttribute;
                    bestAttributeUtility = eachUtility;
                }
            }
            
            if ( bestAttribute != null )
            {
                return bestAttribute;
            }
            else
            {
                throw new System.Exception("Could not determine a best split attribute.");
            }
        }

        /**
            Split this node on the given attribute if it has not already been split on that attribute.
        */
        public void SplitOnAttribute(string splitCondition)
        {
            /* Throw exception if the given attribute doesn't exist. */
            if ( !COMP8901_Asg05._attributes.Contains(splitCondition) )
            {
                throw new System.ArgumentException(
                    System.String.Format("\"{0}\" is not a valid attribute.", splitCondition));
            }

            /* Throw exception if the current branch of the tree has already been split on the given attribute. */
            if ( _pastSplitConditions.ContainsKey(splitCondition) )
            {
                throw new System.ArgumentException(
                    System.String.Format("This branch of the tree has already been split on attribute \"{0}\".", splitCondition));
            }

            /* Initialize the new child nodes. */
            DecisionTreeNode positiveChild = new DecisionTreeNode();
            DecisionTreeNode negativeChild = new DecisionTreeNode();
            SetChild(positiveChild, true);
            SetChild(negativeChild, true);

            /* Set the child nodes' individuals. */
            string positiveSplitValue = COMP8901_Asg05._attributeValues[splitCondition][0];

            foreach ( Individual eachIndividual in _individuals )
            {
                if ( eachIndividual._attributes[splitCondition].Equals(positiveSplitValue) )
                {
                    positiveChild._individuals.Add(eachIndividual);
                }
                else
                {
                    negativeChild._individuals.Add(eachIndividual);
                }
            }

            /* Update split conditions for nodes and tree. */
            _tree._splitConditions.Add(splitCondition);
            _childrenSplitCondition = splitCondition;
            positiveChild._pastSplitConditions.Add(splitCondition, positiveSplitValue);
            negativeChild._pastSplitConditions.Add(splitCondition, COMP8901_Asg05._attributeValues[splitCondition][1]);

            /* Update entropies and classification ratios for children. */
            positiveChild._entropy = positiveChild.CalculateEntropy();
            positiveChild._classificationRatio = positiveChild.CalculateClassificationRatio();
            negativeChild._entropy = negativeChild.CalculateEntropy();
            negativeChild._classificationRatio = negativeChild.CalculateClassificationRatio();
        }

        /**
            Determine the most utile attribute to split this node on and split it on that attribute.
            Returns a DecisionTreeNodeSplitResult indicating the result of the split attempt.
        */
        public DtnSplitResult SplitOnBestAttribute()
        {
            /* If this is a leaf node, return without splitting. */
            if ( IsLeafNode() )
            {
                return DtnSplitResult.ReachedLeafNode;
            }

            /* Determine the best attribute to split on. */
            string bestAttribute = DetermineBestSplitCondition();

            /* Split on the best attribute. */
            SplitOnAttribute(bestAttribute);
            return DtnSplitResult.Split;
        }

        /**
            Returns whether this node is a leaf node.
            A node is a leaf node if its branch has been split on every possible attribute 
            or if its entropy is zero.
        */
        public bool IsLeafNode()
        {
            if ( _pastSplitConditions.Count >= COMP8901_Asg05._attributes.Count || 
                _entropy <= 0 )
            {
                return true;
            }

            return false;
        }

        /**
            Returns the classification ratio for a given list of Individuals.
            
            The classification ratio indicates the fraction of individuals in the given 
            list who fall into the first classification defined in the classification 
            source.

            If the given list is empty or null, this method returns a negative number.
        */
        public static double CalculateClassificationRatio(SysGeneric.List<Individual> collection)
        {
            /* If the collection has no members or is null, classification ratio is negative. */
            if (collection.Count < 1 ||
                collection == null)
            {
                return -1;
            }

            double nPositive = 0;

            foreach ( Individual eachIndividual in collection )
            {
                if ( eachIndividual._classification.Equals(COMP8901_Asg05._classifications[0]) )
                {
                    nPositive++;
                }
            }

            return nPositive / collection.Count;
        }

        /**
            Returns the classification ratio for the list of individuals contained by this node.

            This method should be used to update the node's classification ratio every time 
            this node's list of individuals changes.
        */
        public double CalculateClassificationRatio()
        {
            return CalculateClassificationRatio(_individuals);
        }

        /**
            Splits this node and its resulting descendants on their most utile attributes 
            recursively until each resulting child branch has met at least one of the 
            following conditions:
            
            - The branch has been split on every possible attribute.
            - The branch's leaf node has an entropy of zero.

            At that point, the complete optimal tree from the branch this method was 
            originally called on down will have been generated.
        */
        public void SplitDown()
        {
            /* If this is a leaf node, we're done building this branch. */
            if ( IsLeafNode() )
            {
                SysConsole.Write(System.String.Format(
                    "A leaf node has entropy {0} after splitting on the following attributes:\n",
                    _entropy));

                foreach ( SysGeneric.KeyValuePair<string, string> eachSplit in _pastSplitConditions)
                {
                    SysConsole.Write(System.String.Format(
                        "\t{0} : {1}\n",
                        eachSplit.Key, eachSplit.Value));
                }

                SysConsole.Write("\n");

                return;
            }

            /* If this is not a leaf node, split it on its best attribute. */
            SplitOnBestAttribute();

            /* Split down on each of this node's children. */
            foreach ( DecisionTreeNode eachChild in _children )
            {
                eachChild.SplitDown();
            }
        }
    }
}
