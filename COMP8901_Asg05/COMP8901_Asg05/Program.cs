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
        public static SysGeneric.Dictionary<string, SysGeneric.List<string>> _attributes { get; set; }

        /*--------------------------------------------------------------------------------
            Main Method
        --------------------------------------------------------------------------------*/
        static void Main(string[] args)
        {
            Init();
            ReadArgs(args);

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
            _attributes = new SysGeneric.Dictionary<string, SysGeneric.List<string>>();
        }

        /**
            Parses the arguments to the program.
        */
        private static void ReadArgs(string[] args)
        {
            if (args.Length < 2)
            {
                SysConsole.Write("ERROR: Not enough arguments.\n\n");
            }

            _trainingFilePath = args[0];
            _testFilePath = args[1];

            /* Get test data from training file. */
            SysConsole.Write("Reading training file...\n\n");
            FileReader.ReadDataFile(_trainingFilePath);
        }
    }
}

