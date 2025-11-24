using Android_Silver.Entities;

using System.Text;

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
                using (StreamReader reader = new StreamReader(path, Encoding.ASCII))
                {
                    string result = reader.ReadToEnd();
                    return result;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public byte[] ReadBytes(string fileName)
        {
                 string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
                byte[] fileBytes = File.ReadAllBytes(path);

            return fileBytes;
        }

        public List<char> ReadFromCurrentDirectory(string fileName)
        {
            List<char> charData = new List<char>();
            try
            {
                string path = Path.Combine(FileSystem.AppDataDirectory, fileName);
                byte[] fileBytes = File.ReadAllBytes(path);
                for (int i = 0; i < fileBytes.Length; i++)
                {

                }
            }
            catch (Exception)
            {

            }

            return charData;

        }

        public string GetUpdaterFromFile()
        {
           return ReadFromFile("gold.hex");
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
