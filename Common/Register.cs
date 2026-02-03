public class Register
{
    public string Name;
    
    private int m_value;

    public int Value
    {
        get{return m_value;}
        set
        {
            m_value=value;
            if (m_value > m_highestValue)
            {
                m_highestValue=m_value;
            }
        }
    }

    private int m_highestValue;
    public int HighestValue
    {
        get { return m_highestValue; }
    }
    
}

public class LongRegister
{
    public string Name;
    
    private long m_value;

    public long Value
    {
        get{return m_value;}
        set
        {
            m_value=value;
            if (m_value > m_highestValue)
            {
                m_highestValue=m_value;
            }
        }
    }

    private long m_highestValue;
    public long HighestValue
    {
        get { return m_highestValue; }
    }
    
}



public class RegisterSet
{
    public Dictionary<string,Register> Registers = new Dictionary<string, Register>();

    public Register GetRegister(string name)
    {
        if (!Registers.TryGetValue(name, out var result))
        {
            result = new Register();
            result.Name = name;
            Registers[name] = result;
        }
        return result;
    }

    public int GetValue(string token)
    {
        if (!int.TryParse(token, out var value))
        {
            Register register = GetRegister(token);
            if (register != null)
            {
                value = register.Value;
            }
        }
        return value;
    }
    
}

public class LongRegisterSet
{
    public Dictionary<string,LongRegister> Registers = new Dictionary<string, LongRegister>();

    public LongRegister GetRegister(string name)
    {
        if (!Registers.TryGetValue(name, out var result))
        {
            result = new LongRegister();
            result.Name = name;
            Registers[name] = result;
        }
        return result;
    }

    public long GetValue(string token)
    {
        if (!long.TryParse(token, out var value))
        {
            LongRegister register = GetRegister(token);
            if (register != null)
            {
                value = register.Value;
            }
        }
        return value;
    }
    
}