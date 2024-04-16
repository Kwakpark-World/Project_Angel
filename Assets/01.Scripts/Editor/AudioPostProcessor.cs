using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class AudioPostProcessor : AssetPostprocessor
{
    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach (string movedAsset in movedAssets)
        {
            if (movedAsset.Contains("12.Sounds"))
            {
                AssetDatabase.ImportAsset(movedAsset);
            }
        }
    }

    private void OnPreprocessAudio()
    {
        AudioImporter audioImporter = assetImporter as AudioImporter;
        AudioImporterSampleSettings defaultSampleSettings = audioImporter.defaultSampleSettings;
        string path = audioImporter.assetPath;

        if (path.Contains("BGM"))
        {
            audioImporter.forceToMono = true;
            audioImporter.loadInBackground = true;
            defaultSampleSettings.loadType = AudioClipLoadType.Streaming;
            defaultSampleSettings.compressionFormat = AudioCompressionFormat.Vorbis;
            defaultSampleSettings.quality = 100f;
        }

        if (path.Contains("ENV"))
        {
            audioImporter.forceToMono = true;
            audioImporter.loadInBackground = true;
            defaultSampleSettings.loadType = AudioClipLoadType.Streaming;
            defaultSampleSettings.compressionFormat = AudioCompressionFormat.Vorbis;
            defaultSampleSettings.quality = 100f;
        }

        if (path.Contains("SFX"))
        {
            audioImporter.forceToMono = true;
            audioImporter.loadInBackground = true;
            defaultSampleSettings.loadType = AudioClipLoadType.DecompressOnLoad;
            defaultSampleSettings.preloadAudioData = true;
            defaultSampleSettings.compressionFormat = AudioCompressionFormat.PCM;
        }

        audioImporter.defaultSampleSettings = defaultSampleSettings;

        Debug.LogFormat("{0} has been imported to {1}.\nforce to mono - {2}\nload type - {3}\ncompression format - {4}", Path.GetFileName(path), Path.GetDirectoryName(path), audioImporter.forceToMono.ToString(), defaultSampleSettings.loadType.ToString(), defaultSampleSettings.compressionFormat.ToString());
    }
}
