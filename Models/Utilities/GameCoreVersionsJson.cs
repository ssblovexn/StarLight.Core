﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace StarLight_Core.Models.Utilities;

public class GameCoreVersionsJson
{
    [JsonPropertyName("id")]
    public string Id { get; set; }
    
    [JsonPropertyName("type")]
    public string Type { get; set; }
    
    [JsonPropertyName("mainClass")]
    public string MainClass { get; set; }

    [JsonPropertyName("inheritsFrom")]
    public string InheritsFrom { get; set; }
    
    [JsonPropertyName("releaseTime")]
    public string ReleaseTime { get; set; }

    [JsonPropertyName("time")]
    public string Time { get; set; }
    
    [JsonPropertyName("arguments")]
    public ArgumentsJson Arguments { get; set; }
    
    [JsonPropertyName("minecraftArguments")]
    public ArgumentsJson MinecraftArguments { get; set; }
    
    [JsonPropertyName("javaVersion")]
    public JavaVersionJsonEntity JavaVersion { get; set; } = new JavaVersionJsonEntity
    {
        MajorVersion = 8
    };
}

public class ArgumentsJson
{
    [JsonPropertyName("game")]
    public List<JsonElement> Game { get; set; }

    [JsonPropertyName("jvm")]
    public List<JsonElement> Jvm { get; set; }
}