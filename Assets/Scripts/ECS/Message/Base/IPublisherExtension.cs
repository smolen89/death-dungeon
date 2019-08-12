using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace RD.ECS.Message
{
	public static class IPublisherExtension
	{
		private static readonly MethodInfo registerMethod = typeof( IPublisher ).GetTypeInfo().GetDeclaredMethod( nameof( IPublisher.RegisterListener ) );

		private static IDisposable RegisterListener( IPublisher publisher, Type type, object target )
		{
			List<IDisposable> registeredEntry = new List<IDisposable>();

			try
			{
				while( type != null )
				{
					foreach( MethodInfo method in type.GetTypeInfo().DeclaredMethods.Where( methodInfo => methodInfo.GetCustomAttribute<RegisterListenerAttribute>( false ) != null && ( methodInfo.IsStatic || target != null ) ) )
					{
						ParameterInfo[] parameters = method.GetParameters();

						if( parameters.Length != 1 || !parameters[ 0 ].ParameterType.IsByRef || method.ReturnType != typeof( void ) )
						{
							throw new NotSupportedException( $"Cant apply {nameof( RegisterListenerAttribute )} to \"{method.Name}\": method is not of type {nameof( ActionIn<IMessage> )}." );
						}

						Type argType = parameters[ 0 ].ParameterType.GetElementType();
						registeredEntry.Add( (IDisposable)registerMethod.MakeGenericMethod( argType ).Invoke(
							publisher,
							new object[]
							{
							method.IsStatic
								? method.CreateDelegate(typeof(ActionIn<>).MakeGenericType(argType))
								: method.CreateDelegate(typeof(ActionIn<>).MakeGenericType(argType),target)
						} ) );
					}
					type = type.GetTypeInfo().BaseType;
				}
			}
			catch
			{
				foreach( var disposable in registeredEntry )
				{
					disposable.Dispose();
				}
				throw;
			}

			return registeredEntry.Count > 1 ? registeredEntry.Merge() : registeredEntry.FirstOrDefault();
		}

		public static IDisposable RegisterListener( this IPublisher publisher, Type type )
		{
			return RegisterListener(
				publisher ?? throw new ArgumentNullException( nameof( publisher ) ),
				type ?? throw new ArgumentNullException( nameof( type ) ),
				null );
		}

		public static IDisposable RegisterListener<T>( this IPublisher publisher ) => RegisterListener( publisher, typeof( T ) );

		public static IDisposable RegisterListener<T>( this IPublisher publisher, T target ) where T : class
		{
			return RegisterListener( publisher, target.GetType(), target );
		}
	}
}