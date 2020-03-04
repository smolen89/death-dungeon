// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.

// http://wiki.unity3d.com/index.php?title=Advanced_CSharp_Messenger
// Delegates used in Messenger.cs.

public delegate void Callback();

public delegate void Callback<in T>( T arg1 );

public delegate void Callback<in T, in T1>( T arg1, T1 arg2 );

public delegate void Callback<in T, in T1, in T2>( T arg1, T1 arg2, T2 arg3 );