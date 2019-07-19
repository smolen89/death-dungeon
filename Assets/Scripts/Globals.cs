// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using UnityEngine;
using System.Collections;

public class Globals
{
	/// <summary>
	/// Globalna kalkulacja odległości.
	/// </summary>
	public static DistanceMetric DefaultMetric = DistanceMetric.Euclidean;

	/// <summary>
	/// Wersja klienta. Wersjonowanie w wersji dev: Minion jak jakiś modół zostanie dodany major 1 będzie dopiero jak będzie to grywalne (przynajmniej demo)
	/// </summary>
	public const string GameVersion = "v0.0.1 proto";

	/// <summary>
	/// Maksymalny poziom Dungeonu.
	/// </summary>
	public const int MaxDepth = 100;

	/// <summary>
	/// Maksymalna ilość dungeonów Licząc miastio
	/// </summary>
	public const int MaxDungeons = 20;
}