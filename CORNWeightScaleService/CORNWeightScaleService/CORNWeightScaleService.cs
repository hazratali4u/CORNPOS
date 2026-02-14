using System;
using System.Globalization;
using System.IO.Ports;
using System.Net;
using System.ServiceProcess;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace CORNWeightScaleService
{
    public partial class CORNWeightScaleService : ServiceBase
    {
        private SerialPort _port;
        private string _lastWeight = "0.00";
        private Thread _httpThread;
        private string _buffer = "";

        public CORNWeightScaleService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            StartSerial();
            StartHttpServer();
        }

        protected override void OnStop()
        {
            try
            {
                _port?.Close();
                _port?.Dispose();
                _httpThread?.Abort();
            }
            catch { }
        }

        private void StartSerial()
        {
            try
            {
                int buadRate = 9600;
                int dataBits = 7;
                string parity = "Even";
                string stopBits = "One";
                try
                {
                    buadRate = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["BaudRate"]);
                }
                catch (Exception ex)
                {
                    buadRate = 9600;
                }
                try
                {
                    dataBits = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DataBits"]);
                }                catch (Exception ex)
                {
                    dataBits = 9600;
                }
                try
                {
                    parity = System.Configuration.ConfigurationManager.AppSettings["Parity"].ToString();
                }
                catch (Exception ex)
                {
                    parity = "Even";
                }
                try
                {
                    stopBits = System.Configuration.ConfigurationManager.AppSettings["StopBits"].ToString();
                }
                catch (Exception ex)
                {
                    stopBits = "One";
                }

                Parity parityEnum = Parity.Even;
                if (!Enum.TryParse(parity, true, out parityEnum))
                {
                    parityEnum = Parity.Even;
                }

                StopBits stopBitsEnum = StopBits.One;
                if (!Enum.TryParse(stopBits, true, out stopBitsEnum))
                {
                    stopBitsEnum = StopBits.One;
                }

                _port = new SerialPort
                {
                    PortName = System.Configuration.ConfigurationManager.AppSettings["PortName"].ToString(),
                    BaudRate = buadRate,
                    DataBits = dataBits,
                    Parity = parityEnum,
                    StopBits = stopBitsEnum,
                    Handshake = Handshake.None,
                    Encoding = Encoding.ASCII
                };
                _port.DataReceived += Port_DataReceived;
                _port.Open();
            }
            catch (Exception ex)
            {
                Log(ex.Message, "StartSerial");
            }
        }

        private void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                SerialPort sp = (SerialPort)sender;
                string incoming = sp.ReadExisting();
                if (string.IsNullOrEmpty(incoming))
                    return;
                _buffer += incoming;
                while (_buffer.Contains("\r") || _buffer.Contains("\n"))
                {
                    string line = ExtractLine(ref _buffer);
                    if (string.IsNullOrWhiteSpace(line))
                        continue;
                    line = line.Trim();
                    while (line.Contains("  "))
                        line = line.Replace("  ", " ");
                    string[] parts = line.Split(' ');
                    if (parts.Length > 0)
                    {
                        double w;  // declare variable first
                        if (double.TryParse(parts[0], out w))
                        {
                            _lastWeight = w.ToString("0.00");
                        }
                        else
                        {
                            Log("PARSE FAILED: " + parts[0], "Port_DataReceived");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log("ERROR: " + ex.Message, "Port_DataReceived");
            }
        }

        private void StartHttpServer()
        {
            _httpThread = new Thread(() =>
            {
                try
                {
                    HttpListener listener = new HttpListener();
                    listener.Prefixes.Add("http://localhost:8088/");
                    listener.Start();

                    while (true)
                    {
                        HttpListenerContext ctx = listener.GetContext();
                        HttpListenerResponse resp = ctx.Response;
                        byte[] data = Encoding.UTF8.GetBytes(_lastWeight);
                        resp.ContentType = "text/plain";
                        resp.ContentLength64 = data.Length;
                        resp.OutputStream.Write(data, 0, data.Length);
                        resp.OutputStream.Close();
                    }
                }
                catch { }
            });
            _httpThread.IsBackground = true;
            _httpThread.Start();
        }

        private string ExtractLine(ref string buffer)
        {
            int idxR = buffer.IndexOf('\r');
            int idxN = buffer.IndexOf('\n');

            int idx = -1;
            if (idxR >= 0 && idxN >= 0) idx = Math.Min(idxR, idxN);
            else if (idxR >= 0) idx = idxR;
            else if (idxN >= 0) idx = idxN;

            if (idx < 0) return null;

            string line = buffer.Substring(0, idx);
            buffer = buffer.Substring(idx + 1); // remove processed part
            return line;
        }

        private void Log(string msg,string function)
        {
            System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "\\CORNWeightScaleLog.log",DateTime.Now.ToString() + "- Function: " + function + " Message:" + msg + Environment.NewLine);
        }
    }
}