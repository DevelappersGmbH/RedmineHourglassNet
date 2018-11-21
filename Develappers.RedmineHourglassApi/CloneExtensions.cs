using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Develappers.RedmineHourglassApi
{
    internal static class CloneExtensions
    {
        /// <summary>
        /// Creates a deep clone of an object.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="data">The object to deep clone.</param>
        /// <returns>The clone.</returns>
        public static T DeepClone<T>(this T data) where T : class
        {
            using (var memoryStream = new MemoryStream())
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, data);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return formatter.Deserialize(memoryStream) as T;
            }
        }
    }
}
