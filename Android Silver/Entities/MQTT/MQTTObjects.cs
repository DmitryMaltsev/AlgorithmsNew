using Android_Silver.Entities.Modes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.MQTT
{
    public class MQTTRoot
    {
        public string name { get; private set; }
        public int type { get; private set; }
        public string room { get; private set; }
        public List<OnOff> on_off { get; private set; }
        public List<Range> range { get; private set; }
        public List<SensorFloat> sensors_float { get; private set; }
        public List<Mode> mode { get; set; }

        public MQTTRoot()
        {
            name = "Тестовый вентилятор";
            type = 28;
            room = "Общая";
            on_off = new List<OnOff>();
            on_off.Add(new OnOff() { cmd_on = "1", cmd_off = "0", topic_cmd = "Vent/Switch/Write", topic_state = "Vent/Switch/Read" });
            range = new List<Range>();
            range.Add(new Range() { type = 4, topic_cmd = "Vent/TempSP/Write", topic_state = "Vent/TempSP/Read", max = 100, min = 17, precision = 1, multiplier = 1 });
            sensors_float = new List<SensorFloat>();
            sensors_float.Add(new SensorFloat() { multiplier = 0.1f, type = 0, topic = "RoomTemp" });
            mode = new List<Mode>();
            Mode mode1 = new Mode();
            mode1.options = new List<string>();
            mode1.options.Add("one=1");
            mode1.options.Add("two=2");
            mode1.options.Add("three=3");
            mode1.options.Add("four=4");
            mode1.options.Add("five=5");
            mode.Add(new Mode());
        }
    }

    public class MyData
    {
        public string server { get; set; }
        public int port { get; set; }
        public int port_tls { get; set; }
        public int websocket { get; set; }
        public string user { get; set; }
        public string password { get; set; }
    }

    public class OnOff
    {
        public string topic_cmd { get; set; }

        public string topic_state { get; set; }
        public string cmd_on { get; set; }
        public string cmd_off { get; set; }
    }

    public class Range
    {
        public int type { get; set; }
        public string topic_cmd { get; set; }
        public string topic_state { get; set; }
        public int max { get; set; }
        public int min { get; set; }
        public int precision { get; set; }
        public int multiplier { get; set; }
    }

    public class Mode
    {
        // public string name { get; set; } 
        public int type { get; set; } = 10;
        public string topic_cmd { get; set; } = "Vent/Mode1/Write";
        public string topic_state { get; set; } = "Vent/Mode1/Read";

        public List<string> options { get; set; }

    }

    public class SensorFloat
    {

        public int type { get; set; }
        public string topic { get; set; }
        public float multiplier { get; set; }
    }


}
