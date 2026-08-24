using OnlineShopWebApp.Interfaces;
using Newtonsoft.Json;

namespace OnlineShopWebApp.Helpers
{
    public class FileProvider: IFileProvider
    {
        public T? Read<T>(string path)
        {
            if (!File.Exists(path))
            {
                return default;
            }

            try
            {
                var jsonData = File.ReadAllText(path);

                if (string.IsNullOrWhiteSpace(jsonData))
                {
                    return default;
                }

                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };

                return JsonConvert.DeserializeObject<T>(jsonData, settings);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка чтения файла {path}: {ex.Message}");
                return default;
            }
        }

        public void Write<T>(string path, T data)
        {
            try
            {
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto };
                var jsonData = JsonConvert.SerializeObject(data, settings);

                File.WriteAllText(path, jsonData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при записи файла {path}: {ex.Message}");
            }
        }
    }
}