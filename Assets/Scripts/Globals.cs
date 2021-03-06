﻿// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.

public static class Globals
{
	/// <summary>
	/// Distance global calculations.
	/// </summary>
	public static DistanceMetric DefaultMetric = DistanceMetric.Euclidean;

	public static readonly RD.Util.RND.DotNetRandom DefaultRandom = new RD.Util.RND.DotNetRandom();
	
	/// <summary>
	/// Client Version.
	/// </summary>
	public const string GameVersion = "v0.0.1 proto";

	/// <summary>
	/// Max dungeon depth.
	/// </summary>
	public const int MaxDepth = 100;

	/// <summary>
	/// Max all dungeons with Towns
	/// </summary>
	public const int MaxDungeons = 20;
}