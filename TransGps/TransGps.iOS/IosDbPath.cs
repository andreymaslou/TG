using System;
using System.IO;

using TransGps.iOS;
using TransGps.Interfaces;
using Xamarin.Forms;

[assembly:Dependency(typeof(IosDbPath))]
namespace TransGps.iOS
{
    class IosDbPath : IPath
    {
        public string GetDataBasePath(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), fileName);
        }
    }
}