using System;
using System.IO;

using Xamarin.Forms;
using TransGps.Droid;
using TransGps.Interfaces;

[assembly: Dependency(typeof(AndriodDbPath))]
namespace TransGps.Droid
{
    [Android.Runtime.Preserve(AllMembers = true)]
    class AndriodDbPath : IPath
    {
        public string GetDataBasePath(string fileName)
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), fileName);
        }
    }
}