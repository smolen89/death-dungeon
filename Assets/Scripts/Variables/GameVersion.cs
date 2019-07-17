// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
[System.Serializable]
public struct GameVersion
{
	public GameVersion( int major = 0, int minion = 0, int revision = 0 )
	{
		this.Major = -1;
		this.Minion = -1;
		this.Revision = -1;
		this.Prefix = "v";
		this.Suffix = string.Empty;
	}

	public int Major;
	public int Minion;
	public int Revision;
	public string Prefix;
	public string Suffix;

	public override string ToString()
	{
		return $"{Prefix} {Major}.{Minion}.{Revision}.{Suffix}";
	}
}