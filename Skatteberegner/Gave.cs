namespace Skatteberegner;

public class Gave
{
    public virtual GaveTypeEnum GaveType { get; set; }
    public double Pris { get; set; }
    public DateOnly DatoGivet { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public enum GaveTypeEnum
    {
        Julegave,
        Tingsgave
    }
}

public class Julegave : Gave
{
    public override GaveTypeEnum GaveType => GaveTypeEnum.Julegave;
    public bool SkattepligtigGraenseOverskredet => Pris > 900;

    public override string ToString()
    {
        return nameof(Julegave);
    }
}

public class Tingsgave : Gave
{
    public override GaveTypeEnum GaveType => GaveTypeEnum.Tingsgave;
    
    public override string ToString()
    {
        return nameof(Tingsgave);
    }
}