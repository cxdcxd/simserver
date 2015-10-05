[System.Serializable]
public class PID 
{
    public float dFactor;
    public float iFactor;
    private float integral;
    private float lastError;
    public float pFactor;

    public PID(float pFactor, float iFactor, float dFactor)
    {
        this.pFactor = pFactor;
        this.iFactor = iFactor;
        this.dFactor = dFactor;
    }

    public void updata_k(float a, float b, float c)
    {
        this.pFactor = a;
        this.iFactor = b;
        this.dFactor = c;
    }

    public float Update(float setpoint, float actual, float timeFrame)
    {
        float num = setpoint - actual;
        this.integral += num * timeFrame;
        float num2 = (num - this.lastError) / timeFrame;
        this.lastError = num;
        return (((num * this.pFactor) + (this.integral * this.iFactor)) + (num2 * this.dFactor));
    }
}
