using System;
using Game.Core;
using NUnit.Framework;

namespace Game.Tests.EditMode
{
    /// <summary>
    /// Cekirdek mantik — Unity sahnesi YOK. Ayni kaynak CI'da `dotnet test` ile de
    /// kosar (C+ ilkesi: runner'da Unity yok).
    /// </summary>
    public sealed class HelloRulesTests
    {
        [Test]
        public void RenkIndeksi_Dongusel()
        {
            Assert.AreEqual(0, HelloRules.RenkIndeksi(0));
            Assert.AreEqual(3, HelloRules.RenkIndeksi(3));
            Assert.AreEqual(0, HelloRules.RenkIndeksi(4), "dongu basa donmedi");
            Assert.AreEqual(1, HelloRules.RenkIndeksi(9));
        }

        [Test]
        public void RenkIndeksi_NegatifDokunus_Reddedilir()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => HelloRules.RenkIndeksi(-1));
        }

        [Test]
        public void Arttir_TasmaKorumali()
        {
            Assert.AreEqual(1, HelloRules.Arttir(0));
            Assert.AreEqual(int.MaxValue, HelloRules.Arttir(int.MaxValue), "tasma korunmadi");
        }

        [Test]
        public void KareKenari_KisaKenardan_Turetilir()
        {
            // Ham Pos/boyut tek cihazdan yazilmaz (kod-standardi §3): formulden gelir.
            Assert.AreEqual(2.5f, HelloRules.KareKenari(20f, 10f), 0.0001f);
            Assert.AreEqual(2.5f, HelloRules.KareKenari(10f, 20f), 0.0001f, "kisa kenar secilmedi");
        }

        [Test]
        public void KareKenari_GecersizAlan_Reddedilir()
        {
            Assert.Throws<ArgumentException>(() => HelloRules.KareKenari(0f, 10f));
        }
    }
}
