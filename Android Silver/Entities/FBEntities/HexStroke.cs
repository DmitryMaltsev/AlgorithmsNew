using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.FBEntities
{
    public class HexStroke
    {
        public byte Length { get; set; }
        public ushort Address { get; set; }
        public byte Command { get; set; }
        public byte CRC { get; set; }
        public byte[] UseData { get; set; }
    }
}
