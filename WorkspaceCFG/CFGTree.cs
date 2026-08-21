// ---------------------------------------------------------------------------------------------
// Copyright (c) Bentley Systems, Incorporated. All rights reserved.
// See COPYRIGHT.md in the repository root for full copyright notice
// ---------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WorkspaceCFG.My;
using WorkspaceCFG.My.Resources;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;



namespace WorkspaceCFG
{
    public partial class CFGTree : Form
    {


        [DllImport("shell32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SHGetFileInfo(
            string pszPath,
            uint dwFileAttributes,
            ref SHFILEINFO psfi,
            uint cbFileInfo,
            uint uFlags);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool DestroyIcon(IntPtr hIcon);

        public event TreeViewEventHandler AfterNodeSelect;

        private const uint SHGFI_ICON = 0x100;
        private const uint SHGFI_SMALLICON = 0x1;
        private const uint SHGFI_USEFILEATTRIBUTES = 0x10;
        private const uint FILE_ATTRIBUTE_DIRECTORY = 0x10;
        private const uint FILE_ATTRIBUTE_NORMAL = 0x80;

        private CFGConfiguration _workspace;
        private CFGEnums.ConfigurationType _workspaceType;

        private Dictionary<string, string> _iconKeyCache = new Dictionary<string, string>();

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public int iIcon;
            public uint dwAttributes;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        }

        private void LoadSystemIcons()
        {
            imageList1.Images.Clear();

            // Folder icon
            SHFILEINFO shinfo = new SHFILEINFO();
            IntPtr hImgSmall = SHGetFileInfo("C:\\Windows\\", FILE_ATTRIBUTE_DIRECTORY, ref shinfo,
                (uint)Marshal.SizeOf(shinfo),
                SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES);

            if (shinfo.hIcon != IntPtr.Zero)
            {
                Icon folderIcon = Icon.FromHandle(shinfo.hIcon);
                Bitmap bmp = folderIcon.ToBitmap();
                imageList1.Images.Add("folder", (Bitmap)bmp.Clone());
                bmp.Dispose();
                DestroyIcon(shinfo.hIcon);
            }
            else
            {
                MessageBox.Show("Failed to retrieve folder icon.");
            }

            // File icon
            shinfo = new SHFILEINFO();
            IntPtr hFileIcon = SHGetFileInfo(".txt", FILE_ATTRIBUTE_NORMAL, ref shinfo,
                (uint)Marshal.SizeOf(shinfo),
                SHGFI_ICON | SHGFI_SMALLICON | SHGFI_USEFILEATTRIBUTES);

            if (shinfo.hIcon != IntPtr.Zero)
            {
                using (Icon fileIcon = Icon.FromHandle(shinfo.hIcon))
                {
                    Bitmap bmp = new Bitmap(fileIcon.ToBitmap(), imageList1.ImageSize); // Resize to match
                    imageList1.Images.Add("file", bmp);
                }
                DestroyIcon(shinfo.hIcon);
            }
            else
            {
                MessageBox.Show("Failed to retrieve file icon.");
            }

        }

    public CFGTree(CFGConfiguration _workspaceX,CFGEnums.ConfigurationType inwrktypeX)
        {
            InitializeComponent();
            _workspace = _workspaceX;
            _workspaceType = inwrktypeX;
            object argform = this;
            InterfaceControler.AddForm(ref argform);
        }

        private void LoadDirectory(string dirPath, TreeNode parentNode)
        {
            try
            {
                // Add directories
                foreach (string directory in Directory.GetDirectories(dirPath))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(directory);

                    TreeNode dirNode = new TreeNode(dirInfo.Name)
                    {
                        Tag = new NodeData
                        {
                            FullPath = directory,
                            NormalizedPath = NormalizedPath(directory),
                            ObjectId = Guid.NewGuid().ToString(),
                            Exists = true
                        },
                        ImageKey = "folder",
                        SelectedImageKey = "folder",
                        ToolTipText = $"Path: {directory}\nObject ID: {Guid.NewGuid()}"
                    };

                    parentNode.Nodes.Add(dirNode);
                    LoadDirectory(directory, dirNode); // Recursive call
                }

                // Add files
                foreach (string file in Directory.GetFiles(dirPath))
                {
                    FileInfo fileInfo = new FileInfo(file);
                    string ext = fileInfo.Extension.ToLower();

                    // Check if icon for this extension is already cached
                    if (!_iconKeyCache.ContainsKey(ext) && !string.IsNullOrEmpty(ext))
                    {
                        SHFILEINFO shinfo = new SHFILEINFO();
                        string dummyFile = Path.Combine(Path.GetTempPath(), $"dummy{ext}");

                        IntPtr hImg = SHGetFileInfo(dummyFile, FILE_ATTRIBUTE_NORMAL, ref shinfo,
                            (uint)Marshal.SizeOf(shinfo),
                            SHGFI_USEFILEATTRIBUTES | SHGFI_ICON | SHGFI_SMALLICON);

                        if (shinfo.hIcon != IntPtr.Zero)
                        {
                            Icon icon = Icon.FromHandle(shinfo.hIcon);
                            Bitmap bmp = new Bitmap(icon.ToBitmap(), imageList1.ImageSize);
                            string key = $"file_{ext}";
                            if (!imageList1.Images.ContainsKey(key))
                            {
                                imageList1.Images.Add(key, bmp);
                                _iconKeyCache[ext] = key;
                            }
                            bmp.Dispose();
                            icon.Dispose(); // Only dispose after cloning
                            DestroyIcon(shinfo.hIcon);
                        }
                        else
                        {
                            _iconKeyCache[ext] = "file";
                        }
                    }
                    else if (string.IsNullOrEmpty(ext))
                    {
                        // Use a generic icon for files without extension
                        _iconKeyCache[ext] = "file";
                    }

                    TreeNode fileNode = new TreeNode(fileInfo.Name)
                    {
                        Tag = new NodeData
                        {
                            FullPath = file,
                            NormalizedPath = NormalizedPath(file),
                            ObjectId = Guid.NewGuid().ToString(),
                            Exists = true
                        },
                        ImageKey = _iconKeyCache[ext],
                        SelectedImageKey = _iconKeyCache[ext],
                        ToolTipText = $"Path: {file}\nObject ID: {Guid.NewGuid()}"
                    };

                    parentNode.Nodes.Add(fileNode);
                }

            }
            catch (UnauthorizedAccessException)
            {
                TreeNode errorNode = new TreeNode("Access Denied")
                {
                    ImageKey = "file",
                    SelectedImageKey = "file"
                };
                parentNode.Nodes.Add(errorNode);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is NodeData data)
            {
                lblNote.Text = data.GetNote();
                // Raise the event for external listeners
                AfterNodeSelect?.Invoke(sender, e);
            }
        }

        private void CFGTree_Load(object sender, EventArgs e)
        {
            LoadSystemIcons();
            treeView1.ImageList = imageList1;
            treeView1.ShowNodeToolTips = true;

            treeView1.ShowNodeToolTips = false;

            // List your root paths here
            var rootPaths = new List<string>
            {
                _workspace.GetVariable("_USTN_WORKSETROOT").FinalExpansion,
                _workspace.GetVariable("_USTN_WORKSETSROOT").FinalExpansion,
                _workspace.GetVariable("_USTN_CONFIGURATION").FinalExpansion,
                _workspace.GetVariable("MSDIR").FinalExpansion + "config/"
                 // Example additional root
                   // Add as many as you need
            };

            treeView1.Nodes.Clear();

            foreach (var rootPath in rootPaths)
            {
                if (Directory.Exists(rootPath))
                {
                    TreeNode rootNode = new TreeNode(new DirectoryInfo(rootPath).Name)
                    {
                        Tag = new NodeData
                        {
                            FullPath = rootPath,
                            NormalizedPath= NormalizedPath(rootPath),
                            ObjectId = Guid.NewGuid().ToString(),
                            Exists = true
                        },
                        ImageKey = "folder",
                        SelectedImageKey = "folder"
                    };
                    treeView1.Nodes.Add(rootNode);
                    LoadDirectory(rootPath, rootNode);
                    rootNode.Expand();
                }
                else
                {
                    MessageBox.Show($"Root directory does not exist: {rootPath}");
                }
            }

            splitContainer1.SplitterWidth = 8; // Makes the splitter bar wider
        }

        public void RefreshForm()
        {
            // Implement refresh logic or leave empty if not needed
        }

        public void HighlightFiles(List<string> filePaths)
        {
            // Clear all highlights first
            ClearAllNodeHighlights(treeView1.Nodes);

            foreach (string path in filePaths)
            {
                string normalizedPath = NormalizedPath(path);

                // Find the root node whose Tag (NodeData) FullPath is a prefix of the file path
                TreeNode rootNode = treeView1.Nodes
                    .Cast<TreeNode>()
                    .FirstOrDefault(n =>
                        n.Tag is NodeData data &&
                        !string.IsNullOrEmpty(data.FullPath) &&
                        normalizedPath.StartsWith(NormalizedPath(data.FullPath), StringComparison.OrdinalIgnoreCase));

                if (rootNode != null)
                {
                    EnsureAndHighlightFileNodeUnderRoot(rootNode, normalizedPath);
                }
            }
        }

        // Helper: Only search and highlight under the given root node
        private void EnsureAndHighlightFileNodeUnderRoot(TreeNode rootNode, string fullPath)
        {
            string rootPath = NormalizedPath(rootNode.Tag is NodeData d ? d.FullPath : rootNode.Tag as string);

            if (!rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                rootPath += Path.DirectorySeparatorChar;

            if (!fullPath.StartsWith(rootPath, StringComparison.OrdinalIgnoreCase))
                return;

            string relativePath = fullPath.Substring(rootPath.Length);

            TreeNodeCollection currentNodes = rootNode.Nodes;
            TreeNode currentNode = rootNode;
            string cumulativePath = rootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string[] parts = relativePath.Split(new[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string part in parts)
            {
                cumulativePath = Path.Combine(cumulativePath, part);
                string normalizedCumulativePath = NormalizedPath(cumulativePath);

                TreeNode foundNode = currentNodes
                    .Cast<TreeNode>()
                    .FirstOrDefault(n =>
                        (n.Tag is string s && NormalizedPath(s).Equals(normalizedCumulativePath, StringComparison.OrdinalIgnoreCase)) ||
                        (n.Tag is NodeData d && NormalizedPath(d.FullPath).Equals(normalizedCumulativePath, StringComparison.OrdinalIgnoreCase))
                    );

                if (foundNode == null)
                {
                    // Optionally: create missing node, or just break if you only want to highlight existing nodes
                    TreeNode newNode = new TreeNode(part)
                    {
                        Tag = new NodeData  
                        {
                            FullPath = cumulativePath,
                            NormalizedPath = NormalizedPath(cumulativePath),
                            ObjectId = Guid.NewGuid().ToString(),
                            Exists = File.Exists(cumulativePath) || Directory.Exists(cumulativePath)
                        },
                        ImageKey = Directory.Exists(cumulativePath) ? "folder" : "file",
                        SelectedImageKey = Directory.Exists(cumulativePath) ? "folder" : "file"
                    };
                    currentNodes.Add(newNode);
                    currentNode = newNode;
                }
                else
                {
                    currentNode = foundNode;
                }

                if (currentNode.Parent != null)
                    currentNode.Parent.Expand();

                currentNodes = currentNode.Nodes;
            }

            // Highlight and select the final node
            if (currentNode != null)
            {
                currentNode.BackColor = Color.Yellow;
                currentNode.ForeColor = Color.Black;
                currentNode.NodeFont = new Font(treeView1.Font, FontStyle.Bold);
                treeView1.SelectedNode = currentNode;
                currentNode.EnsureVisible();
                RefreshTreeViewScrollBars(currentNode);
            }
        }

        // Collapsing and re-expanding the node's parent forces the TreeView to
        // recompute the horizontal scrollbar range, so wider (e.g. bolded)
        // node text isn't clipped. This avoids recreating the control's
        // window handle (which Scrollable toggling does, and which can
        // re-trigger selection events and cause infinite loops).
        private void RefreshTreeViewScrollBars(TreeNode node)
        {
            TreeNode parent = node?.Parent;
            if (parent == null)
                return;

            treeView1.BeginUpdate();
            try
            {
                parent.Collapse();
                parent.Expand();
            }
            finally
            {
                treeView1.EndUpdate();
            }
        }

        // Utility: Normalize path for consistent comparison
        private string NormalizedPath(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            return Path.GetFullPath(path).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        }

        private void ClearAllNodeHighlights(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                node.BackColor = treeView1.BackColor;
                node.ForeColor = treeView1.ForeColor;
                if (node.Nodes.Count > 0)
                    ClearAllNodeHighlights(node.Nodes);
            }
        }

        private TreeNode[] FindNodeByPath(TreeNodeCollection nodes, string fullPath)
        {
            List<TreeNode> matches = new List<TreeNode>();

            foreach (TreeNode node in nodes)
            {
                if (node.Tag != null && node.Tag.ToString().Equals(fullPath, StringComparison.OrdinalIgnoreCase))
                {
                    matches.Add(node);
                }

                if (node.Nodes.Count > 0)
                {
                    matches.AddRange(FindNodeByPath(node.Nodes, fullPath));
                }
            }

            return matches.ToArray();
        }

        private void EnsureAndHighlightFileNode(string fullPath)
        {
            string[] parts = fullPath.Split(Path.DirectorySeparatorChar);
            TreeNodeCollection currentNodes = treeView1.Nodes;
            TreeNode currentNode = null;
            string cumulativePath = "";
            List<TreeNode> newlyCreatedNodes = new List<TreeNode>();

            foreach (string part in parts)
            {
                if (string.IsNullOrEmpty(part))
                    continue;
                cumulativePath = string.IsNullOrEmpty(cumulativePath) ? part : Path.Combine(cumulativePath, part);
                TreeNode foundNode = currentNodes
                    .Cast<TreeNode>()
                    .FirstOrDefault(n => n.Tag is NodeData d && d.FullPath.Equals(cumulativePath, StringComparison.OrdinalIgnoreCase));

                if (foundNode == null)
                {
                    // Create missing node
                    TreeNode newNode = new TreeNode(part)
                    {
                        Tag = new NodeData
                        {
                            FullPath = cumulativePath,
                            NormalizedPath = NormalizedPath(cumulativePath), 
                            ObjectId = Guid.NewGuid().ToString()
                        },
                        ImageKey = Directory.Exists(cumulativePath) ? "folder" : "file",
                        SelectedImageKey = Directory.Exists(cumulativePath) ? "folder" : "file"
                    };
                    currentNodes.Add(newNode);
                    newlyCreatedNodes.Add(newNode); // Track new node
                    currentNode = newNode;
                }
                else
                {
                    currentNode = foundNode;
                }

                // Expand parent node to make children visible
                if (currentNode.Parent != null)
                    currentNode.Parent.Expand();

                currentNodes = currentNode.Nodes;
            }

            // Highlight and select the final node
            if (currentNode != null)
            {
                currentNode.BackColor = Color.Yellow;
                currentNode.ForeColor = Color.Black;
                currentNode.NodeFont = new Font(treeView1.Font, FontStyle.Bold);
                treeView1.SelectedNode = currentNode;
                currentNode.EnsureVisible();
                RefreshTreeViewScrollBars(currentNode);
            }
        }


        public class NodeData
        {
            public string FullPath { get; set; }
            public string NormalizedPath { get; set; }
            public string ObjectId { get; set; }
            public bool Exists { get; set; }
            public string GetNote()
            {
                return $"Path: {FullPath}\nNormalizedPath: {NormalizedPath}\nObject ID: {ObjectId}";
            }
        }

    }
}
