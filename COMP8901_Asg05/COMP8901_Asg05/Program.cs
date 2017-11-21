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

        /*--------------------------------------------------------------------------------
            Main Method
        --------------------------------------------------------------------------------*/
        static void Main(string[] args)
        {
            Init();

            if (ReadArgs(args) > 0)
            {
                SysConsole.Write("ERROR: Error parsing arguments.\n\n");
                return;
            }

            BuildDecisionTree(_trainingData);

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
        }

        /**
            Parses the arguments to the program.
        */
        private static int ReadArgs(string[] args)
        {
            if (args.Length < 2)
            {
                SysConsole.Write("ERROR: Not enough arguments.\n\n");
                return 1;
            }

            _trainingFilePath = args[0];
            _testFilePath = args[1];

            /* Get the data from the training and test files. */
            SysConsole.Write(System.String.Format("{0}\n\tReading Training File\n{0}\n", FileReader.HORIZONTAL_RULE));
            _trainingData = FileReader.ReadDataFile(_trainingFilePath);
            SysConsole.Write(System.String.Format("{0}\n\tReading Testing File\n{0}\n", FileReader.HORIZONTAL_RULE));
            _testData = FileReader.ReadDataFile(_testFilePath);

            return 0;
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

            SysConsole.Write(System.String.Format("The entropy of the root node is {0}.\n\n", _learnedTree._root._entropy));
        }
    }
}

