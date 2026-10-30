using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Android_Silver.Entities.MQTT
{
    public class MQTTRoot
    {
        public string name { get; set; }
        public int type { get; set; }
        public string room { get; set; }
        public List<OnOff> on_off { get; set; }
        public List<Range> range { get; set; }

        //public List<Capability> capability { get; set; }
        public List<SensorFloat> sensors_float { get; set; }
        public List<Mode> mode { get; set; }
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

        List<string> options { get; set; }
        public Mode()
        {

            options = new List<string>();
            options.Add("one=1");
            options.Add("two=2");
            options.Add("three=3");
            options.Add("four=4");
            options.Add("five=5");
        }
    }

    public class SensorFloat
    {

        public int type { get; set; }
        public string topic { get; set; }
        public float multiplier { get; set; }
    }


}
