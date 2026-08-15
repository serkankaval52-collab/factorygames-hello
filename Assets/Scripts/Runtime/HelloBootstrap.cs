using FactoryGames.Core;
using Game.Core;
using UnityEngine;

namespace Game.Runtime
{
    /// <summary>
    /// Hello-build: sablon + UPM cekirdegi + kod-oncelikli kurulum zincirinin
    /// ucdan uca kaniti. Sahne dosyasi sablon varsayilaninda KALIR; her sey burada
    /// runtime'da kurulur (Sozlesme-2).
    /// </summary>
    public static class HelloBootstrap
    {
        public const string ROOT_NAME = "HelloRoot";
        private const float WORLD_HALF_HEIGHT = 5f;

        private static readonly Color[] RENKLER =
        {
            new Color(0.25f, 0.80f, 0.95f),
            new Color(0.98f, 0.82f, 0.15f),
            new Color(0.95f, 0.45f, 0.35f),
            new Color(0.45f, 0.85f, 0.55f),
        };

        public static HelloView View { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Boot()
        {
            if (View != null) return;   // idempotent

            GameObject root = RuntimeInitBootstrap.CreateRoot(ROOT_NAME);
            RuntimeInitBootstrap.CreateOrthographicCamera(
                root.transform, WORLD_HALF_HEIGHT, new Color(0.07f, 0.09f, 0.13f));

            View = root.AddComponent<HelloView>();
            View.Kur(RENKLER, WORLD_HALF_HEIGHT);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            Debug.Log($"[hello] boot ok — {RENKLER.Length} renk, dunya yari yuksekligi {WORLD_HALF_HEIGHT}");
#endif
        }

        /// <summary>Testlerin temiz baslamasi icin (PlayMode).</summary>
        internal static void Sifirla() => View = null;
    }

    /// <summary>Uygulayici katman: IS KURALI YOK — tum karar HelloRules'ta.</summary>
    public sealed class HelloView : MonoBehaviour
    {
        private Color[] _renkler;
        private SpriteRenderer _kare;
        private string _sayacMetni = "0";

        public int DokunusSayisi { get; private set; }
        public int RenkIndeksi => HelloRules.RenkIndeksi(DokunusSayisi);

        internal void Kur(Color[] renkler, float dunyaYariYuksekligi)
        {
            _renkler = renkler;

            var go = new GameObject("Kare");
            go.transform.SetParent(transform, false);
            _kare = go.AddComponent<SpriteRenderer>();
            _kare.sprite = BirimKare();
            _kare.color = _renkler[0];

            Rect guvenli = RuntimeInitBootstrap.SafeAreaNormalized();
            float kenar = HelloRules.KareKenari(
                guvenli.width * dunyaYariYuksekligi * 2f,
                guvenli.height * dunyaYariYuksekligi * 2f);
            go.transform.localScale = new Vector3(kenar, kenar, 1f);
        }

        /// <summary>Girdi katmani da bot da BURAYI cagirir — tek yol (deterministik).</summary>
        public void Dokun()
        {
            DokunusSayisi = HelloRules.Arttir(DokunusSayisi);
            _kare.color = _renkler[RenkIndeksi];
            _sayacMetni = DokunusSayisi.ToString();   // Update'te allokasyon yok (kural 16)
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 &&
                Input.GetTouch(0).phase == TouchPhase.Began))
            {
                Dokun();
            }
        }

        private void OnGUI()
        {
            // TextMeshPro font asset'i ScriptableObject'tir ve kural 9 ile celisir.
            // Etiket metni YOK: kullaniciya gorunen sozcuk yerellestirmede yasar
            // (A3.6), hello-build'in yerellestirme katmani yoktur — bu yuzden
            // yalnizca SAYI gosterilir.
            GUI.Label(new Rect(16f, 12f, 320f, 28f), _sayacMetni);
        }

        private static Sprite BirimKare()
        {
            var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false)
            {
                filterMode = FilterMode.Point,
                hideFlags = HideFlags.HideAndDontSave
            };
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();
            var s = Sprite.Create(tex, new Rect(0f, 0f, 1f, 1f), new Vector2(0.5f, 0.5f), 1f);
            s.hideFlags = HideFlags.HideAndDontSave;
            return s;
        }
    }
}
