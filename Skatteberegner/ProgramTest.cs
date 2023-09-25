using Xunit;
public class BeregningTest {
    [Fact]
    public void Billig_julegave_uden_andet_beskattes_ikke() {
        Beregning beregning = new Beregning();

        double skat = beregning.SkatVedJulegave(900, 0);

        Assert.Equal(0, skat);
    }
    
    [Fact]
    public void Maks_julegave_uden_andet_beskattes_ikke() {
        Beregning beregning = new Beregning();

        double skat = beregning.SkatVedJulegave(1200, 0);

        Assert.Equal(0, skat);
    }

    [Fact]
    public void Billig_julegave_og_billigt_andet_beskattes_ikke() {
        Beregning beregning = new Beregning();

        double skat = beregning.SkatVedJulegave(900, 300);

        Assert.Equal(0, skat);
    }

    [Fact]
    public void Overskredet_julegavegraense_og_bagatelgraense_giver_beskatning() {
        Beregning beregning = new Beregning();

        double skat = beregning.SkatVedJulegave(910, 300);

        Assert.Equal(1210, skat);
    }

    [Fact]
    public void Billig_julegave_og_overskredet_bagatelgraense_giver_delvis_beskatning() {
        Beregning beregning = new Beregning();

        double skat = beregning.SkatVedJulegave(900, 500);

        Assert.Equal(500, skat);
    }

}