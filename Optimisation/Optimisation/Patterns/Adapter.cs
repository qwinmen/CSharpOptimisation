using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace Optimisation.Patterns
{
	/*
	 * Шаблон адаптер - задача выставить наружу несколько методов
	 * и обработать любое обращение от них.
	 * Это может потребоваться если использовалась одна модель данных (JToken), ее сменила другая (JsonDocument),
	 * но от старой модели (JToken) пока нельзя отказаться, следовательно нужен механизм, позволяющий
	 * работать с обоими типами (IJobManagerAdapter)
	 */

	public class Adapter
	{
		private interface IJobManagerAdapter
		{
			[Obsolete("Use EnsureAsync(JsonDocument args)")]
			Task<string> EnsureAsync(JToken args);
			Task<string> EnsureAsync(JsonDocument args);
		}

		//Реализация адаптера:
		private class JobManagerAdapter<TArgs>: IJobManagerAdapter
		{
			private readonly IExtendedBackgroundJobManager _jobManager;

			public JobManagerAdapter(IExtendedBackgroundJobManager jobManager)
			{
				_jobManager = jobManager;
			}

			#region Implementation of IJobManagerAdapter

			private async Task<string> EnsureAsync(TArgs args)
			{
				return await _jobManager.EnsureAsync(args);
			}

			#endregion

			#region Implementation of IJobManagerAdapter

			Task<string> IJobManagerAdapter.EnsureAsync(JToken args)
			{
				return EnsureAsync(args.ToObject<TArgs>());
			}

			Task<string> IJobManagerAdapter.EnsureAsync(JsonDocument args)
			{
				return EnsureAsync(args.ToObject<TArgs>());
			}

			#endregion
		}

		protected IExtendedBackgroundJobManager JobManager;//ioc => LazyGetRequiredService<IExtendedBackgroundJobManager>();

		private static readonly Dictionary<string, Type> JobArgumentsTypeMap = new Dictionary<string, Type>
		{
			{ "PrepareJob.JobName", typeof(int) },
		};

		//Внешняя точка вызова
		public async Task<string> EnsureJob(string jobName, string jobArgumentsTypeName, JsonDocument jobArguments)
		{
			Type jobArgumentsType;
			if (string.IsNullOrEmpty(jobName))
			{
				jobArgumentsType = Type.GetType(jobArgumentsTypeName) ?? throw new InvalidOperationException();
			}
			else
			{
				if (!JobArgumentsTypeMap.TryGetValue(jobName, out jobArgumentsType) || jobArgumentsType == null)
					throw new InvalidOperationException();
			}

			//Использование адаптера:
			Type jobAdapterType = typeof(JobManagerAdapter<>).MakeGenericType(jobArgumentsType);
			var jobManagerAdapter = (IJobManagerAdapter) Activator.CreateInstance(jobAdapterType, JobManager) ?? throw new InvalidOperationException();

			return await jobManagerAdapter.EnsureAsync(jobArguments);
		}
	}

	public interface IExtendedBackgroundJobManager
	{
		Task<string> EnsureAsync<TArgs>(TArgs args);
	}

	public static class JsonExtensions
	{
		public static readonly JsonSerializerOptions DefaultJsonSerializerOptions = new JsonSerializerOptions
		{
			PropertyNameCaseInsensitive = true,
			PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
		};

		public static T ToObject<T>(this JsonDocument document)
		{
			var json = document?.RootElement.GetRawText();
			if (string.IsNullOrEmpty(json)) return default;
			return JsonSerializer.Deserialize<T>(json, DefaultJsonSerializerOptions);
		}
	}
}
