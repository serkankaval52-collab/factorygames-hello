using System;

namespace Game.Core
{
    /// <summary>
    /// Hello-build'in saf cekirdegi: Unity API'sine DOKUNMAZ (kod-standardi §1).
    /// Ayni dosya hem Unity EditMode takiminda hem CI'daki `dotnet test`te kosar —
    /// C+ ilkesinin somut karsiligi budur.
    /// </summary>
    public static class HelloRules
    {
        public const int RENK_SAYISI = 4;

        /// <summary>Dokunus sayisindan renk indeksi (formul tek kaynak).</summary>
        public static int RenkIndeksi(int dokunusSayisi)
        {
            if (dokunusSayisi < 0) throw new ArgumentOutOfRangeException(nameof(dokunusSayisi));
            return dokunusSayisi % RENK_SAYISI;
        }

        /// <summary>Sayac artisi — tasma korumali.</summary>
        public static int Arttir(int mevcut)
        {
            return mevcut == int.MaxValue ? int.MaxValue : mevcut + 1;
        }

        /// <summary>Kare kenar uzunlugu: guvenli alanin kisa kenarinin orani.</summary>
        public static float KareKenari(float guvenliGenislik, float guvenliYukseklik, float oran = 0.25f)
        {
            if (guvenliGenislik <= 0f || guvenliYukseklik <= 0f)
                throw new ArgumentException("guvenli alan pozitif olmali");
            float kisaKenar = Math.Min(guvenliGenislik, guvenliYukseklik);
            return kisaKenar * oran;
        }
    }
}
