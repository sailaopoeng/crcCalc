using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace crcCalc
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            doConvert();
        }

        private void doConvert()
        {
            if (txtInput.Text.Length > 0)
            {
                // char resultChar = new char();
                string resultStr = "";
                string resultHex = "";
                string formatInput = "";
                string formatInputStr = "";
                //if (chkIsHex.Checked)
                //{
                //hex 
                try
                {
                    byte[] data = FromHex(txtInput.Text);
                    byte crc = crcFromBytes(data);
                    byte[] crcAarray = new byte[1];
                    crcAarray[0] = crc;
                    resultStr = System.Text.Encoding.UTF8.GetString(crcAarray);
                    resultHex = byteArrayToHexString(crcAarray);
                    formatInput = byteArrayToHexString(data);

                    // formatInputStr = System.Text.Encoding.UTF8.GetString(data); //System.Text.Encoding.UTF8.GetString(data);
                     String tmp = Encoding.UTF8.GetString(data);
                    char[] MyChars = tmp.ToCharArray();
                    string sx = new string(MyChars);
                    System.Diagnostics.Debug.WriteLine(sx);
                    // int x = 0;
                    // while (true)
                    // {
                        // for (int i = 0; i < MyChars.Length - x; i++)
                        //     System.Diagnostics.Debug.WriteLine("{0}", MyChars[i]);
                        // x++;
                        // if (x == MyChars.Length)
                            // break;
                        
                    // }
                    
                    var sb = new StringBuilder("new byte[" + data.Length + "] { ");
                    int index = 0;
                    foreach (var b in data)
                    {
                        try
                        {
                            byte[] tmpArray = new byte[1];
                            tmpArray[0] = b;
                            string rr = System.Text.Encoding.UTF8.GetString(tmpArray, 0, 1);
                            char[] MyChars2 = rr.ToCharArray();
                            string s = new string(MyChars2);
                            sb.AppendFormat("{0}-{1},", index, s);
                            System.Diagnostics.Debug.WriteLine(index + " " + s);
                        }
                        catch(Exception ex1)
                        {
                            System.Diagnostics.Debug.WriteLine(ex1);
                        }
                        
                        index++;
                    }
                    sb.Append("}");
                    System.Diagnostics.Debug.WriteLine(sb.ToString());

                    formatInputStr = convertByteArrayWithNull(data);
                    //MessageBox.Show(formatInputStr);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error " + ex);
                }
                /*}
                else
                {
                    resultChar = crcFromString(txtInput.Text);
                    byte[] ba = Encoding.Default.GetBytes(resultChar.ToString());
                    resultStr = byteArrayToHexString(ba);
                }*/
                txtResult.Text = resultStr;
                txtResultHex.Text = resultHex;
                txtInput.Text = formatInput;
                txtResultAscii.Text = formatInputStr;
            }
        }

        private string convertByteArrayWithNull(byte[] data)
        {
            StringBuilder builder = new StringBuilder();
            int lastOffset = 0;
            for(int i = 0; i < data.Length; i++)
            {
                 if(data[i] == 0)
                 {
                    builder.Append(Encoding.UTF8.GetString(data, lastOffset, i - lastOffset));
                    lastOffset = i + 1;
                 }
               // System.Diagnostics.Debug.WriteLine(data.Length + " " + i + " " + lastOffset);
                //System.Diagnostics.Debug.WriteLine(builder.ToString());


            }
            return builder.ToString();
        }

        public static byte[] FromHex(string hex)
        {
            hex = hex.Replace("-", "");
            hex = hex.Replace(" ", "");
            byte[] raw = new byte[hex.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return raw;
        }

        private string byteArrayToHexString(byte[] str)
        {
            var hexString = BitConverter.ToString(str);
            // hexString = hexString.Replace("-", " ");
            return hexString;
        }

        private char crcFromString(string source)
        {
            int result = 0;
            for (int i = 0; i < source.Length; i++)
            {
                result = result ^ (Byte)(Encoding.Default.GetBytes(source.Substring(i, 1))[0]);
            }
            return (Char)result;
        }

        public static byte crcFromBytes(byte[] bytes)
        {
            byte LRC = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                LRC ^= bytes[i];
            }
            return LRC;
        }

        public string ConvertHexToString(string HexValue)
        {
            string StrValue = "";
            while (HexValue.Length > 0)
            {
                StrValue += System.Convert.ToChar(System.Convert.ToUInt32(HexValue.Substring(0, 2), 16)).ToString();
                HexValue = HexValue.Substring(2, HexValue.Length - 2);
            }
            return StrValue;
        }

        public string ConvertStringToHex(string asciiString)
        {
            string hex = "";
            foreach (char c in asciiString)
            {
                int tmp = c;
                hex += String.Format("{0:x2} ", (uint)System.Convert.ToUInt32(tmp.ToString()));
            }
            return hex;
        }


        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtInput_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                doConvert();
            }
        }

        private void txtInput_KeyUp(object sender, KeyEventArgs e)
        {
            
        }
    }
}
