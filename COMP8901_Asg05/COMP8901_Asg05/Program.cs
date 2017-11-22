/*========================================================================================
    COMP 8901 Assignment 05                                                          *//**
	
    A simple learning AI that builds a decision tree.
	
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
	COMP8901_Asg05
========================================================================================*/
/**
    A simple learning AI that builds a decision tree.
*/
namespace COMP8901_Asg05
{
    class COMP8901_Asg05
    {
        /*--------------------------------------------------------------------------------
            Class Fields
        --------------------------------------------------------------------------------*/


        /*--------------------------------------------------------------------------------
            Class Properties
        --------------------------------------------------------------------------------*/
        private static string _trainingFilePath { get; set; }
        private static string _testFilePath { get; set; }
        public static SysGeneric.List<string> _classifications { get; set; }
        public static SysGeneric.List<string> _attributes { get; set; }
        public static SysGeneric.Dictionary<string, SysGeneric.List<string>> _attributeValues { get; set; }
        private static SysGeneric.List<Individual> _trainingData { get; set; }
        private static SysGeneric.List<Individual> _testData { get; set; }
        private static DecisionTree _learnedTree { get; set; }
        public static string _commonClassification { get; private set; }
        private static SysGeneric.List<Individual> _errorList { get; set; }

        /*--------------------------------------------------------------------------------
            Main Method
        --------------------------------------------------------------------------------*/
        static void Main(string[] args)
        {
            Init();
            ReadArgs(args);
            BuildDecisionTree(_trainingData);
            ClassifyTestData();
            ReportErrors();

            SysConsole.Write("Press any key to exit...");
            SysConsole.ReadKey();
        }

        /*--------------------------------------------------------------------------------
            Class Methods
        --------------------------------------------------------------------------------*/
        /**
            Initializes class fields and properties.
        */
        private static void Init()
        {
            _classifications = new SysGeneric.List<string>();
            _attributes = new SysGeneric.List<string>();
            _attributeValues = new SysGeneric.Dictionary<string, SysGeneric.List<string>>();
            _testData = new SysGeneric.List<Individual>();
            _errorList = new SysGeneric.List<Individual>();
        }

        /**
            Parses the arguments to the program.
        */
        private static void ReadArgs(string[] args)
        {
            if (args.Length < 2)
            {
                throw new System.ArgumentException("ERROR: Not enough arguments.");
            }

            _trainingFilePath = args[0];
            _testFilePath = args[1];

            /* Get the data from the training file. */
            SysConsole.Write(System.String.Format("{0}\n\tReading Training File\n{0}\n", FileReader.HORIZONTAL_RULE));
            _trainingData = FileReader.ReadDataFile(_trainingFilePath);

            int nPositive = 0;
            int nNegative = 0;
            foreach ( Individual eachIndividual in _trainingData )
            {
                if ( eachIndividual._classification.Equals(_classifications[0]) )
                {
                    nPositive++;
                }
                else
                {
                    nNegative++;
                }
            }

            _commonClassification = (nPositive >= nNegative) ? _classifications[0] : _classifications[1];

            /* Get the data from the test file. */
            SysConsole.Write(System.String.Format("{0}\n\tReading Testing File\n{0}\n", FileReader.HORIZONTAL_RULE));
            _testData = FileReader.ReadDataFile(_testFilePath);
        }

        /**
            Builds an ID3-optimized decision tree using the given collection of 
            individuals as the root node.
        */
        private static void BuildDecisionTree(SysGeneric.List<Individual> root)
        {
            SysConsole.Write(System.String.Format("{0}\n\tBuilding Decision Tree\n{0}\n", FileReader.HORIZONTAL_RULE));

            /* Initialize the learned tree with the given node. */
            DecisionTreeNode rootNode = new DecisionTreeNode(root);
            _learnedTree = new DecisionTree(rootNode);

            SysConsole.Write(System.String.Format("The most common classification is {0}.\n\n", _commonClassification));
            SysConsole.Write(System.String.Format("The entropy of the root node is {0}.\n\n", _learnedTree._root._entropy));

            /* Generate the optimal decision tree from the training data. */
            _learnedTree.GenerateOptimalTree();
        }

        /**
            Classifies the test data based on the tree generated from the training data.
        */
        private static void ClassifyTestData()
        {
            SysConsole.Write(System.String.Format("{0}\n\tClassifying Test Data\n{0}\n", FileReader.HORIZONTAL_RULE));

            foreach ( Individual eachIndividual in _testData )
            {
                eachIndividual._predictedClassification = _learnedTree.PredictClassfication(eachIndividual);
            }
        }

        /**
            Report any error cases when classifing test data.
        */
        private static void ReportErrors()
        {
            SysConsole.Write(System.String.Format("{0}\n\tReporting Errors\n{0}\n", FileReader.HORIZONTAL_RULE));

            /* Get the total number and rate of errors. */
            foreach ( Individual eachIndividual in _testData )
            {
                if ( !eachIndividual._classification.Equals(eachIndividual._predictedClassification) )
                {
                    _errorList.Add(eachIndividual);
                }
            }

            SysConsole.Write(System.String.Format(
                "The total number of classification errors was {0}.\n\n",
                _errorList.Count));

            SysConsole.Write(System.String.Format(
                "The error rate was {0}.\n\n",
                (double) _errorList.Count / _testData.Count));

            SysConsole.Write("Error Cases: \nIndividual\tIncorrect Classification\tCorrect Classification\n");

            foreach (Individual eachIndividual in _errorList)
            {
                SysConsole.Write(System.String.Format(
                    "{0}\t\t{1}\t\t{2}\n",
                    eachIndividual._name, eachIndividual._predictedClassification, eachIndividual._classification));
            }
        }
    }
}

