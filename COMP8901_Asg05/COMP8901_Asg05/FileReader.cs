/*========================================================================================
    FileReader                                                                       *//**
	
    Reads in the content of a file for the AI decision tree builder.
	
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
using SysRegex = System.Text.RegularExpressions.Regex;
using SysGeneric = System.Collections.Generic;
using SysConsole = System.Console;

namespace COMP8901_Asg05
{
    /*====================================================================================
        FileReader
    ====================================================================================*/
    /**
        Reads in the content of a file for the AI decision tree builder.
    */
    public static class FileReader
    {
        /*--------------------------------------------------------------------------------
            Instance Fields
        --------------------------------------------------------------------------------*/
        private const char LINE_BREAK = '\n';
        private const char ATTRIBUTE_SEPARATOR = ' ';
        public const string HORIZONTAL_RULE = "-----------------------------------";
        private const char DATA_SEPARATOR = ' ';

        /*--------------------------------------------------------------------------------
            Instance Properties
        --------------------------------------------------------------------------------*/


        /*--------------------------------------------------------------------------------
            Constructors & Destructors
        --------------------------------------------------------------------------------*/


        /*--------------------------------------------------------------------------------
            Class Methods
        --------------------------------------------------------------------------------*/
        public static SysGeneric.List<Individual> ReadDataFile(string filePath)
        {
            string fileContents = System.IO.File.ReadAllText(filePath);

            /* Remove carriage returns and replace all whitespace sequences with single spaces. */
            fileContents = SysRegex.Replace(fileContents, "\r", "");
            fileContents = SysRegex.Replace(fileContents, @"[\t\f\v ]+", " ");

            /* Split into lines. */
            string[] lines = fileContents.Split(LINE_BREAK);

            /* Get rid of blank and comment lines. */
            SysGeneric.List<string> dataLines = new SysGeneric.List<string>();
            foreach (string eachLine in lines)
            {
                if (eachLine.Length < 1)
                {
                    continue;
                }

                if (eachLine[0].Equals('/') &&
                    eachLine[1].Equals('/'))
                {
                    continue;
                }

                /* If the line is neither blank nor commented, then it is data. */
                dataLines.Add(eachLine);
            }

            /* Extract data from the data lines. */
            bool areClassificationsRead = false;
            bool areAttributesRead = false;
            bool isDataRead = false;
            int attributeCount = 0;
            int dataCount = 0;
            SysGeneric.List<Individual> individuals = new SysGeneric.List<Individual>();

            SysConsole.Write(System.String.Format("{0}\n\tReading Classifications\n{0}\n", HORIZONTAL_RULE));
            foreach (string eachLine in dataLines)
            {
                /* Read the classifications. */
                if (!areClassificationsRead)
                {
                    /* Are we done reading the classifications? */
                    if (int.TryParse(eachLine, out attributeCount))
                    {
                        areClassificationsRead = true;
                        SysConsole.Write(System.String.Format("{0} classifications read.\n\n", COMP8901_Asg05._classifications.Count));
                        SysConsole.Write(System.String.Format("{0}\n\tReading Attributes\n{0}\n", HORIZONTAL_RULE));
                        SysConsole.Write(System.String.Format("Attribute count: {0}\n", attributeCount));
                        continue;
                    }

                    /* If we have not recorded this classificiation yet, do so. */
                    if (!COMP8901_Asg05._classifications.Contains(eachLine))
                    {
                        COMP8901_Asg05._classifications.Add(eachLine);
                        SysConsole.Write(eachLine + "\n");
                    }
                    continue;
                }

                /* Read the attributes. */
                if (!areAttributesRead)
                {
                    /* Are we done reading the attributes? */
                    if (int.TryParse(eachLine, out dataCount))
                    {
                        areAttributesRead = true;
                        SysConsole.Write(System.String.Format("{0} attributes read.\n\n", COMP8901_Asg05._attributes.Count));
                        SysConsole.Write(System.String.Format("{0}\n\tReading Individuals\n{0}\n", HORIZONTAL_RULE));
                        SysConsole.Write(System.String.Format("Individual count: {0}\n\n", dataCount));
                        continue;
                    }

                    /* Parse each attribute. */
                    string[] attributeParts = eachLine.Split(ATTRIBUTE_SEPARATOR);

                    /* If we have not recorded this attribute yet, do so. */
                    if (!COMP8901_Asg05._attributes.Contains(attributeParts[0]))
                    {
                        SysGeneric.List<string> attributeValues = new SysGeneric.List<string>();

                        for (int i = 1; i < attributeParts.Length; i++)
                        {
                            attributeValues.Add(attributeParts[i]);
                        }

                        /* Record each attribute. */
                        COMP8901_Asg05._attributes.Add(attributeParts[0]);
                        COMP8901_Asg05._attributeValues[attributeParts[0]] = attributeValues;
                        SysConsole.Write(eachLine + "\n");
                    }
                    continue;
                }

                /* Read the data. */
                if (!isDataRead && 
                    dataCount > 0)
                {
                    /* Parse each individual. */
                    string[] dataParts = eachLine.Split(DATA_SEPARATOR);

                    Individual eachIndividual = new Individual(dataParts[0], dataParts[1]);
                    SysGeneric.Dictionary<string, string> eachIndividualAttributes = new SysGeneric.Dictionary<string, string>();

                    /* Get the attribute values for this individual. */
                    int index = 2;
                    foreach (string eachAttribute in COMP8901_Asg05._attributes)
                    {
                        eachIndividualAttributes.Add(eachAttribute, dataParts[index]);
                        index++;
                    }

                    /* Add the individual to the return list. */
                    eachIndividual._attributes = eachIndividualAttributes;
                    individuals.Add(eachIndividual);
                    SysConsole.Write(eachIndividual + "\n");
                    dataCount--;
                    continue;
                }

                /* If we have read all of the lines of data we wanted, stop reading the file. */
                break;
            }

            SysConsole.Write("\nFinished reading data from file!\n\n");
            return individuals;
        }
    }
}
