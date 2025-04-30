using System;

namespace ludumdare56;

public enum WhiteBloodCellRank
{
    Neutrophil,
    Eosinophil,
    Basophil,
    Lymphocyte,
    Monocyte,
}

public static class WhiteBloodCellExtensions
{
    public static double PercentageInAdultHumans(this WhiteBloodCellRank rank) => rank switch
    {
        WhiteBloodCellRank.Neutrophil => .62,
        WhiteBloodCellRank.Eosinophil => .023,
        WhiteBloodCellRank.Basophil => .004,
        WhiteBloodCellRank.Lymphocyte => .3,
        WhiteBloodCellRank.Monocyte => .053,
        _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null)
    };

    public static string PrimaryResponsibility(this WhiteBloodCellRank rank) => rank switch
    {
        WhiteBloodCellRank.Neutrophil => "Bacteria",
        WhiteBloodCellRank.Eosinophil => "Parasites",
        WhiteBloodCellRank.Basophil => "Allergens",
        WhiteBloodCellRank.Lymphocyte => "Traitors",
        WhiteBloodCellRank.Monocyte => "Repeat Offenders",
        _ => throw new ArgumentOutOfRangeException(nameof(rank), rank, null)
    };
}