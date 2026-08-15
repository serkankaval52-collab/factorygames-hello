using System;
using Game.Core;
using NUnit.Framework;

namespace Game.Core.Tests
{
    /// <summary>
    /// AYNI cekirdek, Unity OLMADAN. Bu dosya CI'da kosar (C+ ilkesi: runner'da Unity
    /// yok); Unity tarafindaki EditMode takimi ayni kurallari sahne baglaminda surer.
    /// Ikisinin ayni kaynagi (Assets/Scripts/Core) tuketmesi tek-kaynak kanitidir.
    /// </summary>
    public sealed class HelloRulesTests
    {
        [Test]
        public void RenkIndeksi_Dongusel()
        {
            Assert.That(HelloRules.RenkIndeksi(0), Is.EqualTo(0));
            Assert.That(HelloRules.RenkIndeksi(HelloRules.RENK_SAYISI), Is.EqualTo(0));
            Assert.That(HelloRules.RenkIndeksi(HelloRules.RENK_SAYISI + 1), Is.EqualTo(1));
        }

        [Test]
        public void RenkIndeksi_Negatif_Reddedilir()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => HelloRules.RenkIndeksi(-1));
        }

        [Test]
        public void Arttir_TasmaKorumali()
        {
            Assert.That(HelloRules.Arttir(41), Is.EqualTo(42));
            Assert.That(HelloRules.Arttir(int.MaxValue), Is.EqualTo(int.MaxValue));
        }

        [Test]
        public void KareKenari_KisaKenardan_Turetilir()
        {
            Assert.That(HelloRules.KareKenari(20f, 10f), Is.EqualTo(2.5f).Within(0.0001f));
            Assert.That(HelloRules.KareKenari(10f, 20f), Is.EqualTo(2.5f).Within(0.0001f));
        }

        [Test]
        public void KareKenari_GecersizAlan_Reddedilir()
        {
            Assert.Throws<ArgumentException>(() => HelloRules.KareKenari(-1f, 10f));
        }
    }
}
