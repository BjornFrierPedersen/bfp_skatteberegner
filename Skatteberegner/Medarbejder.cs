namespace Skatteberegner;

public class Medarbejder
{
    public required string Navn { get; init; }
    public List<Gave> Gaver { get; set; } = new();

    public void GivGave(Gave gave) => Gaver.AddGave(gave, Navn);
}