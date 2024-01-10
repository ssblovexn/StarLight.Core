﻿using System.Diagnostics;
using System.Runtime.Intrinsics.Arm;
using StarLight_Core.Enum;
using StarLight_Core.Models.Authentication;
using StarLight_Core.Models.Launch;
using StarLight_Core.Utilities;

namespace StarLight_Core.Launch
{
    public class MinecraftLaunch
    {
        public BaseAccount BaseAccount { get; set; }
        
        public GameWindowConfig GameWindowConfig { get; set; }
        
        public GameCoreConfig GameCoreConfig { get; set; }
        
        public JavaConfig JavaConfig { get; set; }
        
        public MinecraftLaunch(LaunchConfig launchConfig)
        {
            GameWindowConfig = launchConfig.GameWindowConfig;
            GameCoreConfig = launchConfig.GameCoreConfig;
            JavaConfig = launchConfig.JavaConfig;
            BaseAccount = launchConfig.Account.BaseAccount;
        }

        public async Task<LaunchResponse> LaunchAsync(Action<ProgressReport> onProgressChanged)
        {
            var stopwatch = new Stopwatch();
            var process = new Process();
            var progressReport = new ProgressReport();

            progressReport.Description = "检查启动参数";
            progressReport.Percentage = 10;
            onProgressChanged?.Invoke(progressReport);
            
            if (GameCoreConfig == null)
            {
                return new LaunchResponse(LaunchStatus.Failed, stopwatch, process, new Exception("未配置游戏核心信息"));
            }
            if (JavaConfig == null)
            {
                return new LaunchResponse(LaunchStatus.Failed, stopwatch, process, new Exception("未配置Java"));   
            }
            if (BaseAccount == null)
            {
                return new LaunchResponse(LaunchStatus.Failed, stopwatch, process, new Exception("未配置账户信息"));
            }
            if (GameCoreUtil.GetGameCore(GameCoreConfig.Version, GameCoreConfig.Root) == null)
            {
                return new LaunchResponse(LaunchStatus.Failed, stopwatch, process, new Exception("游戏核心不存在或游戏核心已损坏"));
            }
            if (JavaUtil.GetJavaInfo(JavaConfig.JavaPath) == null)
            {
                return new LaunchResponse(LaunchStatus.Failed, stopwatch, process, new Exception("Java 不存在或 Java 已损坏"));
            }
            
            try
            {
                progressReport.Description = "构建启动参数";
                progressReport.Percentage = 20;
                onProgressChanged?.Invoke(progressReport);
                var arguments = new ArgumentsBuildUtil(GameWindowConfig, GameCoreConfig, JavaConfig, BaseAccount).Build();
                
                process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = JavaConfig.JavaPath,
                        Arguments = string.Join(' '.ToString(), arguments),
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        WorkingDirectory = GameCoreConfig.Root
                    },
                    EnableRaisingEvents = true
                };
                
                stopwatch.Start();

                progressReport.Description = "进程启动中...";
                progressReport.Percentage = 99;
                onProgressChanged?.Invoke(progressReport);

                return new LaunchResponse(LaunchStatus.Success, stopwatch, process, new Exception());
            }
            catch (Exception e)
            {
                return new LaunchResponse(LaunchStatus.Failed, stopwatch, process, e);
            }
        }
    }
}

