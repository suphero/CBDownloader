﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CBDownloader {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class DownloaderSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static DownloaderSettings defaultInstance = ((DownloaderSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new DownloaderSettings())));
        
        public static DownloaderSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("##DownloadPath##")]
        public string DownloadPath {
            get {
                return ((string)(this["DownloadPath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("##PlaylistFilePath##")]
        public string PlaylistFilePath {
            get {
                return ((string)(this["PlaylistFilePath"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("playlist.txt")]
        public string PlaylistFileNameToSave {
            get {
                return ((string)(this["PlaylistFileNameToSave"]));
            }
        }
    }
}
