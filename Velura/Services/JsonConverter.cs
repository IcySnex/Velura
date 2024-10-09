using System.Text.Json;
using Microsoft.Extensions.Logging;
using Velura.Services.Abstract;

namespace Velura.Services;

public class JsonConverter : IConverter
{
	readonly ILogger<JsonConverter> logger;
	
	public JsonConverter(
		ILogger<JsonConverter> logger)
	{
		this.logger = logger;
		
		logger.LogInformation("[JsonConverter-.ctor] JsonConverter has been initialized.");
	}


	public string ToString(
		object vlaue)
	{
		logger.LogInformation("[JsonConverter->ToString] Serializing object to string...");
		
		return JsonSerializer.Serialize(vlaue);
	}


	public T ToObject<T>(
		string value) =>
		JsonSerializer.Deserialize<T>(value) ?? throw new NullReferenceException("JsonSerializer.Deserialize<T> returned null.");
	
	public bool TryToObject<T>(
		string value,
		out T? result)
	{
		try
		{
			logger.LogInformation("[JsonConverter-ToObject] Deserializing string to object...");
			
			result = JsonSerializer.Deserialize<T>(value) ?? throw new NullReferenceException("JsonSerializer.Deserialize<T> returned null.");
			return true;
		}
		catch (Exception ex)
		{
			logger.LogError(ex, "[JsonConverter-ToObject] Failed to deseriale string to object: {exception}", ex.Message);
			
			result = default(T);
			return false;
		}
	}
}