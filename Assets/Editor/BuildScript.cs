using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Game.EditorTools
{
    /// <summary>
    /// C+ duzeni: Unity CI runner'inda KOSMAZ. Android APK'si ve iOS Xcode projesi
    /// YERELDE uretilir; kanit metin olarak depoya girer, xcodeproj ise macOS
    /// runner'a gecici bir Release asset'i uzerinden tasinir.
    ///
    /// Sablonun productName'i rakamla baslayabildigi icin (sonda bulgusu) uygulama
    /// kimligi burada KODDA verilir — kaynagi olan durum.
    /// </summary>
    public static class BuildScript
    {
        private const string SCENE = "Assets/Scenes/SampleScene.unity";
        private const string APPLICATION_ID = "com.factorygames.hello";

        [MenuItem("FactoryGames/Build Android")]
        public static void BuildAndroid()
        {
            string cikti = Hazirla("android");
            string apk = Path.Combine(cikti, "hello.apk");

            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android, APPLICATION_ID);
            BuildReport rapor = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { SCENE },
                locationPathName = apk,
                target = BuildTarget.Android,
                targetGroup = BuildTargetGroup.Android,
                options = BuildOptions.None
            });

            Ozetle("android", rapor);
            if (rapor.summary.result != BuildResult.Succeeded)
                throw new Exception("Android build basarisiz: " + rapor.summary.result);
            if (!File.Exists(apk))
                throw new Exception("build 'Succeeded' dedi ama APK yok — kanit yok");

            Debug.Log($"[hello-build] APK boyut={new FileInfo(apk).Length} bayt yol={apk}");
        }

        [MenuItem("FactoryGames/Build iOS Xcode Project")]
        public static void BuildIos()
        {
            string cikti = Hazirla("ios");

            PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.iOS, APPLICATION_ID);
            BuildReport rapor = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { SCENE },
                locationPathName = cikti,
                target = BuildTarget.iOS,
                targetGroup = BuildTargetGroup.iOS,
                options = BuildOptions.None
            });

            Ozetle("ios", rapor);
            if (rapor.summary.result != BuildResult.Succeeded)
                throw new Exception("iOS proje uretimi basarisiz: " + rapor.summary.result);

            string[] proj = Directory.GetDirectories(cikti, "*.xcodeproj", SearchOption.AllDirectories);
            if (proj.Length == 0)
                throw new Exception("build 'Succeeded' dedi ama *.xcodeproj yok — kanit yok");

            Debug.Log($"[hello-build] xcodeproj={string.Join(";", proj.Select(Path.GetFileName))}");
        }

        private static string Hazirla(string ad)
        {
            if (!File.Exists(SCENE)) throw new FileNotFoundException("sahne yok: " + SCENE);
            string dizin = Path.Combine(Directory.GetCurrentDirectory(), "Build", ad);
            if (Directory.Exists(dizin)) Directory.Delete(dizin, true);
            Directory.CreateDirectory(dizin);
            return dizin;
        }

        private static void Ozetle(string ad, BuildReport rapor)
        {
            BuildSummary s = rapor.summary;
            Debug.Log($"[hello-build] {ad}: sonuc={s.result} sure={s.totalTime} " +
                      $"boyut={s.totalSize} hata={s.totalErrors} uyari={s.totalWarnings}");
        }
    }
}
