namespace Android_Silver.Services
{
    public class MathService
    {
        public MathService()
        {


        }

        public byte CalculateChecksum(byte[] data)
        {
            byte checksum = 0;
            foreach (byte b in data)
            {
                checksum += b;
            }
            return (byte)((0x100 - checksum) & 0xFF);
        }

        public char[] GetHexCharsFromByte(byte data)
        {
            char[] hexChars = new char[2];
            int highVal = (char)(data >> 4);
            int lowVal = (char)(data & 0x0F);
            hexChars[0] = (char)(highVal < 10 ? highVal + '0' : highVal - 10 + 'A');
            hexChars[1] = (char)(lowVal < 10 ? lowVal + '0' : lowVal - 10 + 'A');
            return hexChars;
        }

        public byte GetByteFromHexChar(char char1, char char2)
        {
            byte result = 0;
            int highByte = char1 >= 'A' ? char1 - 'A' + 10 : char1 - '0';
            int lowByte = char2 >= 'A' ? char2 - 'A' + 10 : char2 - '0';
            return (byte)(highByte << 4 | lowByte);
        }
    }
}
