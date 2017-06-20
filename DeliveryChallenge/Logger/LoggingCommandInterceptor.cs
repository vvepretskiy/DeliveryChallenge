using System.Data.Common;
using System.Data.Entity.Infrastructure.Interception;
using System.Diagnostics;

namespace DeliveryChallenge.Logger
{
	public class LoggingCommandInterceptor : IDbCommandInterceptor
	{
		public void NonQueryExecuting(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
		{
			Trace.TraceInformation(command.CommandText);
		}

		public void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
		{
			if (interceptionContext.Exception != null)
				Trace.TraceError(command.CommandText + " errored " + interceptionContext.Exception);
		}

		public void ReaderExecuting(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
		{
			Trace.TraceInformation(command.CommandText);
		}

		public void ReaderExecuted(DbCommand command, DbCommandInterceptionContext<DbDataReader> interceptionContext)
		{
			if (interceptionContext.Exception != null)
				Trace.TraceError(command.CommandText + " errored " + interceptionContext.Exception);
		}

		public void ScalarExecuting(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
		{
			Trace.TraceInformation(command.CommandText);
		}

		public void ScalarExecuted(DbCommand command, DbCommandInterceptionContext<object> interceptionContext)
		{
			if (interceptionContext.Exception != null)
				Trace.TraceError(command.CommandText + " errored " + interceptionContext.Exception);
		}
	}
}
