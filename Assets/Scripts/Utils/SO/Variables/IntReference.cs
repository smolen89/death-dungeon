// Copyright (c) 2019 EG Studio, LLC. All Rights Reserved.
// Create by Ebbi Gebbi.
using System;

	[Serializable]
	public class IntReference
	{
		public bool UseConstant = true;
		public int ConstantValue;
		public IntVariable Variable;

		public IntReference()
		{ }

		public IntReference( int value )
		{
			UseConstant = true;
			ConstantValue = value;
		}

		public int Value
		{
			get { return UseConstant ? ConstantValue : Variable.Value; }
		}

		public static implicit operator int( IntReference reference )
		{
			return reference.Value;
		}
	}
