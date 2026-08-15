using System.Collections;
using Game.Core;
using Game.Runtime;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Game.Tests.PlayMode
{
    /// <summary>
    /// Zincirin ucdan uca kaniti: sahne dosyasina DOKUNULMADAN, kod-oncelikli
    /// kurulumun gercekten calistigini olcer. Girdi katmani yerine dogrudan
    /// Dokun() cagrilir — scripted input, deterministik.
    /// </summary>
    public sealed class BootstrapTests
    {
        private int _hataSayisi;

        [SetUp]
        public void SetUp()
        {
            _hataSayisi = 0;
            Application.logMessageReceived += OnLog;
        }

        [TearDown]
        public void TearDown() => Application.logMessageReceived -= OnLog;

        private void OnLog(string c, string s, LogType t)
        {
            if (t == LogType.Error || t == LogType.Exception || t == LogType.Assert) _hataSayisi++;
        }

        [UnityTest]
        public IEnumerator Bootstrap_Sahneye_Dokunmadan_Kurulur()
        {
            yield return null;

            Assert.IsNotNull(HelloBootstrap.View,
                "RuntimeInitializeOnLoadMethod calismadi — kod-oncelikli kurulum kirik");

            // `GameObject.Find` KULLANILMAZ (kural 8) — koke acik referanstan gidilir.
            GameObject kok = HelloBootstrap.View.gameObject;
            Assert.AreEqual(HelloBootstrap.ROOT_NAME, kok.name, "kok nesne adi beklenenden farkli");
            Assert.IsNotNull(kok.GetComponentInChildren<Camera>(), "kamera kurulmadi");
            Assert.AreEqual(0, _hataSayisi, "kurulumda hata/exception logu uretildi");
        }

        [UnityTest]
        public IEnumerator Dokunus_Sayaci_Ve_Rengi_Ilerletir()
        {
            yield return null;
            HelloView view = HelloBootstrap.View;
            Assert.IsNotNull(view);

            int baslangic = view.DokunusSayisi;
            for (int i = 0; i < HelloRules.RENK_SAYISI; i++)
            {
                view.Dokun();
                yield return null;
            }

            Assert.AreEqual(baslangic + HelloRules.RENK_SAYISI, view.DokunusSayisi);
            Assert.AreEqual(HelloRules.RenkIndeksi(view.DokunusSayisi), view.RenkIndeksi,
                "gorunum ile cekirdek ayristi — tek kaynak ihlali");
            Assert.AreEqual(0, _hataSayisi, "dokunus dongusunde hata logu uretildi");
        }
    }
}
