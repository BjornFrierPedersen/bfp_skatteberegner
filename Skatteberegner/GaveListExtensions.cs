namespace Skatteberegner;

public static class GaveListExtensions
{
    public static void AddGave(this List<Gave> source, Gave gave, string personNavn)
    {
        switch (gave.GaveType)
        {
            case Gave.GaveTypeEnum.Julegave:
                var eksisterende = source.FirstOrDefault(g => g.GaveType == Gave.GaveTypeEnum.Julegave);
                if (eksisterende == null || eksisterende.DatoGivet.Year != DateTime.Now.Year)
                    source.Add(gave);
                else throw new InvalidOperationException($"{personNavn} kan ikke modtage flere julegaver i år");
                break;
            case Gave.GaveTypeEnum.Tingsgave:
                source.Add(gave);
                break;
        }
        
        source.UdregnAaretsGavePriser(gave.Pris, personNavn);
    }
    
    public static double UdregnSkattepligtigtBeloeb(this List<Gave> source)
    {
        var fradragsberettigetBeloeb = 1200;
        var juleGave = (Julegave?)source.FirstOrDefault(g =>
            g.GaveType == Gave.GaveTypeEnum.Julegave && g.DatoGivet.Year.Equals(DateTime.Now.Year));

        // Sum uden julegaver
        var sum = source
            .Where(g => g.GaveType != Gave.GaveTypeEnum.Julegave && g.DatoGivet.Year.Equals(DateTime.Now.Year))
            .Sum(g => g.Pris);
        
        // Der er ingen julegave og vi er under det fradragsberettigede beløb
        if (juleGave == null && sum < fradragsberettigetBeloeb) return sum;

        return juleGave == null ? sum : UdregnJulegaveBeskatningFraSum(juleGave, sum);
    }
    
    private static double UdregnJulegaveBeskatningFraSum(Julegave julegave, double sum)
    {
        var fradragsberettigetBeloeb = 1200;
        
        // Hvis julegaven skal beskattes og vi ryger over det fradragsberettigede beløb
        if (julegave.Pris > 900 && fradragsberettigetBeloeb < julegave.Pris + sum)
            return sum + julegave.Pris;
        
        return (julegave.SkattepligtigGraenseOverskredet) switch
        {
            // Hvis julegaven ikke skal beskattes, men vi stadig er over det fradragsberettigede beløb
            false when fradragsberettigetBeloeb < sum => sum,
            // Hvis julegaven ikke skal beskattes, men vi stadig er over det fradragsberettigede beløb
            false when fradragsberettigetBeloeb < julegave.Pris + sum => sum,
            _ => 0.0
        };
    }
    
    private static void UdregnAaretsGavePriser(this List<Gave> source, double gavePris, string personNavn)
    {
        var sum = source
            .Where(g => g.DatoGivet.Year.Equals(DateTime.Now.Year))
            .Sum(g => g.Pris);

        Console.WriteLine(
            $"{personNavn} har modtaget en gave for en værdi af {gavePris} og har modtaget for i alt {sum} DKK i gaver i år");
    }
}