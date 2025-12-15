
using System.Numerics;

public class Program
{

    static void Main()
    {
        //new GenerateYear().Create(2025);

        string day = "11";
        string year = "2025";
        
        var type = Type.GetType($"Test{day}_{year}");
        BaseTest test = (BaseTest)Activator.CreateInstance(type);
        
        //test.SetTestInput().RunTest();
        //test.RunTest();
        //test.SetTestInput().SetPart2().RunTest();
        test.SetPart2().RunTest();
        
    }

    
    

}

