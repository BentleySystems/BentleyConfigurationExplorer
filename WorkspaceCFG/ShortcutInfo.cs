// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System.IO;
using Shell32;

namespace WorkspaceCFG
{

    public class ShortcutInfo
    {

        public string Arguments;
        public string Description;
        public string IconLocation;
        public string ShortCutPath;
        public string Target;
        public string TargetPath;
        public string WorkingDirectory;

        public ShortcutInfo(string shortcutFilePath)
        {
            // Copy file to the temp folder
            string tempFolderPath = Path.GetTempPath();
            string destinationFilePath = Path.Combine(tempFolderPath, Path.GetFileName(shortcutFilePath));

            // Creating a temporary copy of the shortcut since loading it will fail if it location
            // in a start menu or other path requiring elevated privileges.
            File.Copy(shortcutFilePath, destinationFilePath, true);

            var shell = new Shell();
            shortcutFilePath = Path.GetFullPath(destinationFilePath);
            object dir = shell.NameSpace(Path.GetDirectoryName(destinationFilePath));
            var item = ((dynamic)dir).Items().Item(Path.GetFileName(destinationFilePath));
            ShellLinkObject link = (ShellLinkObject)item.GetLink;

            Arguments = link.Arguments;
            Description = link.Description;
            link.GetIconLocation(out IconLocation);
            ShortCutPath = shortcutFilePath;
            TargetPath = link.Path;
            WorkingDirectory = link.WorkingDirectory;

            // Delete the file from the temp folder
            File.Delete(destinationFilePath);
        }

    }
}