# factorygames-hello

FactoryGames hattının **hello-build**'i: şablon zincirinin uçtan uca kanıtı
(0A adım 4). Oyun değildir; ölçüm düzeneğidir.

Norm kaynağı: [FactoryGames/docs/PIPELINE.md](https://github.com/serkankaval52-collab/FactoryGames/blob/arena/019fcd97-factorygames/docs/PIPELINE.md)
· Repo kuralları: [CLAUDE.md](CLAUDE.md)

## Ne kanıtlar

| zincir halkası | kanıt |
|---|---|
| UPM paketi | `com.factorygames.core` git URL + `#tag` ile çekilir, assembly doğar |
| Kod-öncelikli kurulum | sahne şablon varsayılanında kalır; hiyerarşi `[RuntimeInitializeOnLoadMethod]` ile |
| Saf çekirdek ayrımı | `Assets/Scripts/Core` hem Unity EditMode'da hem CI'da `dotnet test` ile koşar |
| C+ makine ilkesi | CI runner'ında Unity **yok**; Android/iOS kanıtı yerelde üretilir |
| Kapılar | `tools/lint` — sahne baseline, tek-kaynak, preset-sapma, secret, ham-artefakt, dil |

## Yerel koşum

```
python tools/lint/lint.py --kok .
python tools/lint/lint_varlik.py --kok .
dotnet test tests/Core.Tests/Core.Tests.csproj -c Release
```

Unity gerektiren kanıtlar (EditMode/PlayMode, APK, xcodeproj) yerelde batchmode ile
üretilir; `Assets/Editor/BuildScript.cs` içindeki menü komutları kullanılır.
