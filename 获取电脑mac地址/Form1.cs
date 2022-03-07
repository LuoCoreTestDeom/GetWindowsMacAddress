using System.Net.NetworkInformation;

namespace 获取电脑mac地址
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                button1.Enabled = false;
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface ni in interfaces)
                {
                    string macAddress=  BitConverter.ToString(ni.GetPhysicalAddress().GetAddressBytes());
                    textBox1.AppendText("名称：" + ni.Name + Environment.NewLine + ""+ macAddress);
                    textBox1.AppendText( Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string mac;
            string ipv4;
            string ipv6;
            GetActiveIpAndMac2(out mac, out ipv4, out ipv6);
            textBox1.Text = mac;
        }

        /// <summary>
        /// 获取当前激活网络的MAC地址、IPv4地址、IPv6地址 - 方法2
        /// </summary>
        /// <param name="mac">网卡物理地址</param>
        /// <param name="ipv4">IPv4地址</param>
        public static void GetActiveIpAndMac2(out string mac, out string ipv4, out string ipv6)
        {
            mac = "";
            ipv4 = "";
            ipv6 = "";

            //需要引用：System.Net.NetworkInformation
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                UnicastIPAddressInformationCollection allAddress = adapterProperties.UnicastAddresses;
                if (allAddress.Count > 0)
                {
                    if (adapter.OperationalStatus == OperationalStatus.Up)
                    {
                        mac = adapter.GetPhysicalAddress().ToString();
                        foreach (UnicastIPAddressInformation addr in allAddress)
                        {
                            if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                ipv4 = addr.Address.ToString();
                            }
                            if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetworkV6)
                            {
                                ipv6 = addr.Address.ToString();
                            }
                        }

                        if (string.IsNullOrWhiteSpace(mac) ||
                            (string.IsNullOrWhiteSpace(ipv4) && string.IsNullOrWhiteSpace(ipv6)))
                        {
                            mac = "";
                            ipv4 = "";
                            ipv6 = "";
                            continue;
                        }
                        else
                        {
                            if (mac.Length == 12)
                            {
                                mac = string.Format("{0}-{1}-{2}-{3}-{4}-{5}",
                                    mac.Substring(0, 2), mac.Substring(2, 2), mac.Substring(4, 2),
                                    mac.Substring(6, 2), mac.Substring(8, 2), mac.Substring(10, 2));
                            }
                            break;
                        }
                    }
                }
            }
        }
    }
}