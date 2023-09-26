namespace Skatteberegner;

public static class GaveListExtensions
{
    private static double _fradragsberettigetBeloeb = 1200;
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
        
        source.UdregnAaretsGavePriser(gave, personNavn);
    }
    
    public static double UdregnSkattepligtigtBeloeb(this List<Gave> source)
    {
        var juleGave = (Julegave?)source.FirstOrDefault(g =>
            g.GaveType == Gave.GaveTypeEnum.Julegave && g.DatoGivet.Year.Equals(DateTime.Now.Year));

        // Sum uden julegaver
        var sum = source
            .Where(g => g.GaveType != Gave.GaveTypeEnum.Julegave && g.DatoGivet.Year.Equals(DateTime.Now.Year))
            .Sum(g => g.Pris);
        
        // Der er ingen julegave og vi er under det fradragsberettigede beløb
        if (juleGave != null || !(sum < _fradragsberettigetBeloeb))
            return juleGave == null ? sum : UdregnJulegaveBeskatningFraSum(juleGave, sum);
        
        Console.WriteLine(
            $"Der er ikke givet nogen julegave og vi er under det fradragsberettigede beløb og det skattepligtige beløb er derfor: {sum} DKK");
        return sum;

    }
    
    private static double UdregnJulegaveBeskatningFraSum(Julegave julegave, double sum)
    {
        if (julegave.SkattepligtigGraenseOverskredet && _fradragsberettigetBeloeb < julegave.Pris + sum)
        {
            var skattepligtigtBeloeb = sum + julegave.Pris;
            Console.WriteLine($"Både julegavens skattepligtige beløb og det fradragsberettigede beløb er overskredet, det skattepligtige beløb er derfor: {skattepligtigtBeloeb} DKK");
            return skattepligtigtBeloeb;
        }
        
        if (!julegave.SkattepligtigGraenseOverskredet && _fradragsberettigetBeloeb < julegave.Pris + sum)
        {
            Console.WriteLine(
                $"Julegaven er under den skattepligtige grænse, men det samlede beløb overskrider det fradragsberettigede beløb, det skattepligtige beløb er derfor: {sum} DKK");
            return sum;
        }

        Console.WriteLine("Det fradragsberettigede beløb på 1200 DKK er ikke overskredet og det skattepligtige beløb er derfor 0.0 DKK");
        
        return 0.0;
    }
    
    private static void UdregnAaretsGavePriser(this List<Gave> source, Gave gave, string personNavn)
    {
        var sum = source
            .Where(g => g.DatoGivet.Year.Equals(DateTime.Now.Year))
            .Sum(g => g.Pris);

        Console.WriteLine(
            $"{personNavn} har modtaget en {gave.GaveType} for en værdi af {gave.Pris} DKK og har modtaget for i alt {sum} DKK i gaver i år");
    }
}