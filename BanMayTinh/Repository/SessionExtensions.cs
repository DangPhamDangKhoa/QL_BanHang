using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Runtime.Intrinsics.X86;
using System.Diagnostics.Metrics;

namespace BanMayTinh.Repository
{
	public static class SessionExtensions
	{
		public static void SetJson (this ISession session, string key, object value)
		{
			session.SetString(key, JsonConvert.SerializeObject(value));
		}

		public static T GetJson<T>(this ISession session, string key)
		{
			var sessionData = session.GetString(key);
			return sessionData == null ? default(T) : JsonConvert.DeserializeObject<T>(sessionData);
		}
	}
	
}
