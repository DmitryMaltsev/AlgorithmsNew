using Android_Silver.Entities;

using Microsoft.UI.Xaml.Documents;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Services
{
    public class FileSystemService
    {
        EthernetEntities _ethernetEntities;
        public FileSystemService()
        {
            _ethernetEntities = DIContainer.Resolve<EthernetEntities>();
        }
        public async Task SaveToFileAsync(string fileName, string content)
        {
            try
            {
                string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
                using (StreamWriter writer = new StreamWriter(path))
                {
                    await writer.WriteAsync(content);
                }
            }
            catch (Exception)
            {


            }

        }

        public string ReadFromFile(string fileName)
        {
            try
            {
                string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
                using (StreamReader reader = new StreamReader(path))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string ReadFromCurrentDirectory(string fileName)
        {
            try
            {
                string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
                byte[] fileBytes = File.ReadAllBytes(path);

                // Предполагаем, что файл содержит последовательность int (по 4 байта каждый)
                for (int i = 0; i < fileBytes.Length; i += 4)
                {

                    int value = BitConverter.ToInt32(fileBytes, i);

                }
                return String.Empty;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }


        public void GetUpdaterFromFile()
        {
            var result = ReadFromCurrentDirectory("gold.bin");
        }


        public void GetIPFromFile()
        {
            string ip = ReadFromFile("ConnectIP");
            if (!String.IsNullOrEmpty(ip) && ip.Length > 5)
            {
                _ethernetEntities.ConnectIP = ip;
                string[] ips = _ethernetEntities.ConnectIP.Split(".");
                if (ips.Length == 4)
                {
                    if (byte.TryParse(ips[0], out byte buf))
                    {
                        _ethernetEntities.IP1 = buf;
                    }
                    if (byte.TryParse(ips[1], out buf))
                    {
                        _ethernetEntities.IP2 = buf;
                    }
                    if (byte.TryParse(ips[2], out buf))
                    {
                        _ethernetEntities.IP3 = buf;
                    }
                    if (byte.TryParse(ips[3], out buf))
                    {
                        _ethernetEntities.IP4 = buf;
                    }
                }
            }
        }
    }
}
