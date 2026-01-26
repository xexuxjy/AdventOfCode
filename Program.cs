
using System.Numerics;

public class Program
{

    static void Main()
    {
        //new GenerateYear().Create(2017);

        string day = "14";
        string year = "2017";
        
        var type = Type.GetType($"Test{day}_{year}");
        BaseTest test = (BaseTest)Activator.CreateInstance(type);
        
        //test.SetTestInput().RunTest();
        //test.RunTest();
        //test.SetTestInput().SetPart2().RunTest();
        test.SetPart2().RunTest();
        
    }

    
    

}

