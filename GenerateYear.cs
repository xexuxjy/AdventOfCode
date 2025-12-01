using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GenerateYear
{
    
    
    public void Create(int year)
    {
        string pathBase = "../../../";

        string codePath = pathBase + year.ToString();
        if (!Directory.Exists(codePath))
        {
            Directory.CreateDirectory(codePath);
        }

        for (int i = 0; i < 25; ++i)
        {
            int testId = i + 1;
            string fileName = Path.Combine(codePath, $"Test{testId}_{year}.cs");
            if (!File.Exists(fileName))
            {
                using (StreamWriter outputFile = new StreamWriter(fileName))
                {

                    outputFile.WriteLine("using System;");
                    outputFile.WriteLine("using System.Collections.Generic;");
                    outputFile.WriteLine($"public class Test{testId}_{year} : BaseTest");
                    outputFile.WriteLine($"{{");
                    outputFile.WriteLine($"public override void Initialise()");
                    outputFile.WriteLine($"{{");
                    outputFile.WriteLine($"Year = {year};");
                    outputFile.WriteLine($"TestID = {testId};");
                    outputFile.WriteLine($"}}");
                    outputFile.WriteLine($"public override void Execute()");
                    outputFile.WriteLine($"{{");
                    outputFile.WriteLine($"}}");
                    outputFile.WriteLine($"}}");

                }
            }
        }


        string dataPath = pathBase + "Data/" + year.ToString();
        if (!Directory.Exists(dataPath))
        {
            Directory.CreateDirectory(dataPath);
        }
        for (int i = 0; i < 25; ++i)
        {
            int testId = i + 1;
            if (!File.Exists(dataPath + $"/puzzle-{testId}-test-input.txt"))
            {

                File.Create(dataPath + $"/puzzle-{testId}-test-input.txt");
            }
            if (!File.Exists(dataPath + $"/puzzle-{testId}-input.txt"))
            {

                File.Create(dataPath + $"/puzzle-{testId}-input.txt");
            }
        }
    }
}


