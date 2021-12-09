using System;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using UnityEditor;

namespace SDG.Unturned.Tools
{
	public static partial class EditorAssetBundleHelper
	{
		/// <summary>
		/// Build an asset bundle by name.
		/// </summary>
		/// <param name="assetBundleName">Name of an asset bundle registered in the editor.</param>
		/// <param name="outputPath">Absolute path to directory to contain built asset bundle.</param>
		/// <param name="multiplatform">Should mac and linux variants of asset bundle be built as well?</param>
		public static void Build(string assetBundleName, string outputPath, bool multiplatform)
		{
			string[] assetPaths = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
			if(assetPaths.Length < 1)
			{
				Debug.LogWarning("No assets in: " + assetBundleName);
				return;
			}
			
			// Saves some perf by disabling these unused loading options.
			// If changing remember to update the CI build process.
			BuildAssetBundleOptions assetBundleOptions = BuildAssetBundleOptions.DisableLoadAssetByFileName | BuildAssetBundleOptions.DisableLoadAssetByFileNameWithExtension;

			if(multiplatform)
			{
				AssetBundleBuild[] linuxBuilds = new AssetBundleBuild[1];
				linuxBuilds[0].assetBundleName = MasterBundleHelper.getLinuxAssetBundleName(assetBundleName);
				linuxBuilds[0].assetNames = assetPaths;
				BuildPipeline.BuildAssetBundles(outputPath, linuxBuilds, assetBundleOptions, BuildTarget.StandaloneLinux64);

				AssetBundleBuild[] macBuilds = new AssetBundleBuild[1];
				macBuilds[0].assetBundleName = MasterBundleHelper.getMacAssetBundleName(assetBundleName);
				macBuilds[0].assetNames = assetPaths;
				BuildPipeline.BuildAssetBundles(outputPath, macBuilds, assetBundleOptions, BuildTarget.StandaloneOSX);
			}

			// Windows... finally done!
			AssetBundleBuild[] windowsBuilds = new AssetBundleBuild[1];
			windowsBuilds[0].assetBundleName = assetBundleName;
			windowsBuilds[0].assetNames = assetPaths;
			BuildPipeline.BuildAssetBundles(outputPath, windowsBuilds, assetBundleOptions, BuildTarget.StandaloneWindows64);

			CleanupAfterBuildingAssetBundle(outputPath);
			HashAssetBundle(outputPath + '/' + assetBundleName);

#if GAME
			if(string.Equals(assetBundleName, "core.masterbundle"))
			{
				PostBuildCoreMasterBundle(outputPath);
			}
#endif
		}

		/// <summary>
		/// Unity (sometimes?) creates an empty bundle with the same name as the folder, so we delete it.
		/// </summary>
		public static void CleanupAfterBuildingAssetBundle(string outputPath)
		{
			string directoryName = Path.GetFileName(outputPath);
			string emptyBundlePath = Path.Combine(outputPath, directoryName);
			if (File.Exists(emptyBundlePath))
			{
				File.Delete(emptyBundlePath);
			}
			string emptyManifestPath = emptyBundlePath + ".manifest";
			if (File.Exists(emptyManifestPath))
			{
				File.Delete(emptyManifestPath);
			}
		}

		/// <summary>
		/// Combine per-platform hashes into a file for the server to load.
		/// </summary>
		public static void HashAssetBundle(string windowsFilePath)
		{
			string linuxFilePath = MasterBundleHelper.getLinuxAssetBundleName(windowsFilePath);
			string macFilePath = MasterBundleHelper.getMacAssetBundleName(windowsFilePath);

			if(!File.Exists(linuxFilePath) || !File.Exists(macFilePath))
			{
				Debug.Log("Skipping hash");
				return;
			}
			
			SHA1CryptoServiceProvider hashAlgo = new SHA1CryptoServiceProvider();
			byte[] windowsHash = hashAlgo.ComputeHash(File.ReadAllBytes(windowsFilePath));
			byte[] linuxHash = hashAlgo.ComputeHash(File.ReadAllBytes(linuxFilePath));
			byte[] macHash = hashAlgo.ComputeHash(File.ReadAllBytes(macFilePath));

			byte[] hashes = new byte[60];
			Array.Copy(windowsHash, 0, hashes, 0, 20);
			Array.Copy(linuxHash, 0, hashes, 20, 20);
			Array.Copy(macHash, 0, hashes, 40, 20);
			
			//Debug.LogFormat("Windows hash: {0}", Hash.toString(windowsHash));
			//Debug.LogFormat("Linux hash: {0}", Hash.toString(linuxHash));
			//Debug.LogFormat("Mac hash: {0}", Hash.toString(macHash));

			string hashFilePath = MasterBundleHelper.getHashFileName(windowsFilePath);
			File.WriteAllBytes(hashFilePath, hashes);
		}
	}
}
