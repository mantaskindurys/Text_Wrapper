using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeleSoft_Assignment
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please enter arguments needed. Input file path & maximum line legth.");
                Console.ReadLine();
                return;
            }

            if (!File.Exists(args[0]))
            {
                Console.WriteLine("File not found, make sure the 1st argument is the correct file path.");
                Console.ReadLine();
                return;
            }
            int maxLength;
            if (!Int32.TryParse(args[1],out maxLength)){
                Console.WriteLine("Incorrect max length, 2nd argument has to be number.");
                Console.ReadLine();
                return;
            }

            String inputFile = args[0];
            String outputFile = Path.GetDirectoryName(args[0]) + "\\Output" + GetTimestamp(DateTime.Now) + ".txt";

            String line;
            StreamReader file = new StreamReader(inputFile);
            StringBuilder linebuilder = new StringBuilder();
            StreamWriter outputStream = new StreamWriter(outputFile);
            while (true)
            {
                line = file.ReadLine();
                if (line != null)
                {
                    linebuilder.Insert(0, line);
                    outputStream.WriteLine(ParseLine(linebuilder, maxLength), true);
                    linebuilder.Clear();
                    outputStream.Flush();
                }
                else
                {
                    break;
                }
            }
            outputStream.Close();

        }

        private static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmss");
        }

        private static String ParseLine(StringBuilder initialLine, int maxLength)
        {
            String parsedLine = "";
            int splitIndex = maxLength - 1;
            StringBuilder parsedBuilder = new StringBuilder(parsedLine);
            initialLine.Replace("{", "{{"); // { and } symbols break the StringBuilder and need to be replaced.
            initialLine.Replace("}", "}}"); 

            while (initialLine.Length > maxLength)
            {
                String CurrentSplit = initialLine.ToString().Substring(0, maxLength);
                if (CurrentSplit.Contains(" ")) //check if there is white space we can use as a split position
                {
                    int breakindex = CurrentSplit.LastIndexOf(" ");
                    CurrentSplit = CurrentSplit.Substring(0, breakindex);
                    parsedBuilder.Append(CurrentSplit + "\n");
                    initialLine.Remove(0, CurrentSplit.Length);
                }
                else // split the text if there isnt any useable whitespace
                {
                    parsedBuilder.Append(CurrentSplit + "\n");
                    initialLine.Remove(0, maxLength);
                }

                initialLine = CleanWhiteSpace(initialLine);
            }

            if (initialLine.Length > 0) // left over text after last split is added if there is any
            {
                parsedBuilder.Append(initialLine.ToString());
            }
            parsedLine = parsedBuilder.ToString();
            return parsedLine;
        }

        private static StringBuilder CleanWhiteSpace(StringBuilder line)
        {
            if (line.ToString().StartsWith(" "))
            {
                line.Remove(0, 1);
            }

            return line;
        }
    }
}
