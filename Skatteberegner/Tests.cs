using Skatteberegner;
using Xunit;
public class Tests {
    [Fact]
    public void Billig_julegave_uden_andet_beskattes_ikke()
    {
        var Tom = new Medarbejder { Navn = "Tom" };
        var tingsGave = new Julegave { Pris = 900 };
        var juleGave = new Tingsgave { Pris = 0 };

        Tom.GivGave(tingsGave);
        Tom.GivGave(juleGave);

        var skat = Tom.Gaver.UdregnSkattepligtigtBeloeb();

        Assert.Equal(0, skat);
    }
    
    [Fact]
    public void Maks_julegave_uden_andet_beskattes_ikke() 
    {
        var Sine = new Medarbejder { Navn = "Sine" };
        var juleGave = new Julegave { Pris = 1200 };
        var tingGave = new Tingsgave { Pris = 0 };

        Sine.GivGave(juleGave);
        Sine.GivGave(tingGave);

        var skat = Sine.Gaver.UdregnSkattepligtigtBeloeb();

        Assert.Equal(0, skat);
    }
    
    [Fact]
    public void Billig_julegave_og_billigt_andet_beskattes_ikke() 
    {
        var Freja = new Medarbejder { Navn = "Freja" };
        var juleGave = new Julegave { Pris = 900 };
        var tingGave = new Tingsgave { Pris = 300 };

        Freja.GivGave(juleGave);
        Freja.GivGave(tingGave);

        var skat = Freja.Gaver.UdregnSkattepligtigtBeloeb();

        Assert.Equal(0, skat);
    }
    
    [Fact]
    public void Overskredet_julegavegraense_og_bagatelgraense_giver_beskatning() 
    {
        var Lenard = new Medarbejder { Navn = "Lenard" };
        var juleGave = new Julegave { Pris = 910 };
        var tingGave = new Tingsgave { Pris = 300 };

        Lenard.GivGave(juleGave);
        Lenard.GivGave(tingGave);

        var skat = Lenard.Gaver.UdregnSkattepligtigtBeloeb();

        Assert.Equal(1210, skat);
    }
    
    [Fact]
    public void Billig_julegave_og_overskredet_bagatelgraense_giver_delvis_beskatning() {
        var Max = new Medarbejder { Navn = "Max" };
        var juleGave = new Julegave { Pris = 900 };
        var tingGave = new Tingsgave { Pris = 500 };

        Max.GivGave(juleGave);
        Max.GivGave(tingGave);

        var skat = Max.Gaver.UdregnSkattepligtigtBeloeb();

        Assert.Equal(500, skat);
    }

}