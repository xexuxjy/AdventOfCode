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