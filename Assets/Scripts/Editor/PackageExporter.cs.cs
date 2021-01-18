using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;
using UnityEditor.PackageManager;
using System.Threading;

namespace Editor.Packages
{
    public class Build
    {
        /// <summary>
        /// Main entry point to publish a tarball to disk from command line
        /// Usage: -executeMethod Editor.Packages.StartCommandLineBuild
        ///        -packages com.name.package1,com.name.package2,...
        ///        -outputPath ./Custom/Output/Path
        ///
        /// outputPath is optional and defaults to $PWD/Build
        /// </summary>   
        public static void StartCommandLineBuild() {
            var packageNames = GetPackagesFromCommandline();
            var outputPath = GetOutputPathFromCommandline();
            if(!ExportPackages(outputPath, packageNames))
            {
                Exit(1);
            }
            Debug.Log("Build succeeded.");
            Exit(0);
        }

        private static bool ExportPackages(string outputPath, params string[] packageNames)
        {
            if (packageNames == null || packageNames.Length <= 0 || outputPath == null)
            {
                Debug.Log("Package Names is empty or null or outputPath is null.");
                return false;
            }
            Debug.Log($"Exporting {string.Join(','.ToString(), packageNames)} to {outputPath}");
            // Resolve packages found by the package manager
            var packages = packageNames.Select(x => (name: x, package: ResolvePackage(x)));
            // find requested packages that didn't show up in the package manager
            var missingPackages = packages.Where(x => x.package == null).Select(x => $"'{x.name}'").ToArray();
            if (missingPackages.Length > 0)
            {
                var missingNames = string.Join(", ", missingPackages);
                Debug.LogError($"Some package names could not be resolved: [{missingNames}]");
                return false;
            }
            
            foreach(var item in packages)
            {
                var package = item.package;
                // requesting a packing is happening under the hood and needs to be awaited for
                var packRequest = Client.Pack(package.assetPath, outputPath);
                while (!packRequest.IsCompleted)
                {
                    // for legacy reasons we just wait on the Unity Mono thread
                    Thread.Sleep(16);
                }

                if (packRequest.Error != null)
                {
                    Debug.LogError(packRequest.Error);
                    // error out and exit
                    Exit(1);
                }
                // log tracing infos for CI maintainer
                Debug.Log($"Package '{package.name}' export to '{packRequest.Result.tarballPath}'");
            }
            
            return true;
        }

        private static string[] GetPackagesFromCommandline()
        {
            return  GetCommandlineParameter("-packages")?.Split(',');
        }

        private static string GetOutputPathFromCommandline()
        {
            return GetCommandlineParameter("-outputPath", "./Build/");
        }

        private static string GetCommandlineParameter(string name, string defaultValue = null)
        {
            var args = Environment.GetCommandLineArgs();
            var index = Array.IndexOf(args, name);
            if (index < 0 || index > args.Length - 1)
            {
                return defaultValue;
            }

            return args[index + 1];
        }
            
        private static void Exit(int status)
        {
            if (Application.isBatchMode)
            {
                // immediate exists sometimes confuses the CI logs
                Thread.Sleep(1000);
                EditorApplication.Exit(status);
            }
        }

        private static PackageInfo ResolvePackage(string name)
        {
            // GetAll is the only method to get _any_ registered packages in the Package Manager and is marked internal
            var method = typeof(PackageInfo).GetMethod("GetAll", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            var packageInfos = (PackageInfo[]) method.Invoke(null, new object[] { });
            // resolves the package with a certain name
            return packageInfos.FirstOrDefault(x => x.name == name);
        }
    }
}