// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.

// http://wiki.unity3d.com/index.php?title=Advanced_CSharp_Messenger
// Delegates used in Messenger.cs.

public delegate void Callback();

public delegate void Callback<T>( T arg1 );

public delegate void Callback<T, U>( T arg1, U arg2 );

public delegate void Callback<T, U, V>( T arg1, U arg2, V arg3 );