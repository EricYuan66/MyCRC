using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyCRC;

namespace CheckCRC
{
    public partial class Form1 : Form
    {
        private uint _gainBytes;
        private List<TextBox> _tbxList;
        public Form1()
        {
            InitializeComponent();
            GetTbxList();
        }

        public void GetTbxList()
        {
            _tbxList = new List<TextBox> {tbx0, tbx1, tbx2, tbx3, tbx4, tbx5, tbx6, tbx7, tbx8, tbx9};
        }

        private TextBox GetRightTextBox(TextBox thisTbx)
        {
            string thisTbxName = thisTbx.Name;
            int thisTbxNum = thisTbxName[3] - '0';
            return (thisTbxNum + 1) == _tbxList.Count ? null : _tbxList[thisTbxNum + 1];
        }

        private TextBox GetLeftTextBox(TextBox thisTbx)
        {
            string thisTbxName = thisTbx.Name;
            int thisTbxNum = thisTbxName[3] - '0';
            return (thisTbxNum - 1) < 0 ? null : _tbxList[thisTbxNum - 1];
        }

        private void tbx_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox thisTestBox = (TextBox)sender;
            if ((e.KeyChar == 8))//backSpace
            {
                if (thisTestBox.Text.Length == 0)
                {
                    TextBox leftTextBox = GetLeftTextBox(thisTestBox);
                    if (leftTextBox != null)
                    {                    
                        leftTextBox.Focus();
                    }
                }
                return;
            }
            if (thisTestBox.Text.Length >= 1)
            {
                TextBox rightTextBox = GetRightTextBox(thisTestBox);
                if (rightTextBox == null)
                {
                    if (thisTestBox.Text.Length == 2)
                    {
                        e.Handled = true;
                    }
                    return;
                }
                rightTextBox.Focus();
            }
            else
            {
                if ((e.KeyChar < '0') || ((e.KeyChar > '9') && (e.KeyChar < 'A')) || (e.KeyChar > 'F'))
                {
                    e.Handled = true;
                    MessageBox.Show(@"输入大写十六进制数！！");
                }
            }
        }

        #region tbxEvent
        private void tbx0_KeyPress(object sender, KeyPressEventArgs e)
        {
            tbx_KeyPress(sender, e);
        }

        private void tbx1_KeyPress(object sender, KeyPressEventArgs e)
        {
            tbx_KeyPress(sender, e);
        }

        private void tbx2_KeyPress(object sender, KeyPressEventArgs e)
        {
            tbx_KeyPress(sender, e);
        }

        private void tbx3_KeyPress(object sender, KeyPressEventArgs e)
        {
            tbx_KeyPress(sender, e);
        }

        private void tbx4_KeyPress(object sender, KeyPressEventArgs e)
        {
            tbx_KeyPress(sender, e);
        }

        private void tbx5_KeyPress(object sender, KeyPressEventArgs e)
        {
            tbx_KeyPress(sender, e);
        }

        private void tbx6_KeyPress(object sender, KeyPressEventArgs e)
        {
            tbx_KeyPress(sender, e);
        }

        private void tbx7_KeyPress(object sender, KeyPressEventArgs e)
        {
            tbx_KeyPress(sender, e);
        }

        private void tbx8_KeyPress(object sender, KeyPressEventArgs e)
        {
            tbx_KeyPress(sender, e);
        }

        private void tbx9_KeyPress(object sender, KeyPressEventArgs e)
        {
            tbx_KeyPress(sender, e);
        }
        #endregion

        private List<byte> CombineInputStr()
        {
            List<byte> hexs = new List<byte>();
            foreach (TextBox textBox in _tbxList)
            {
                if (!string.IsNullOrEmpty(textBox.Text))
                {
                    hexs.Add(HexstrToint(textBox.Text));
                }
                else
                {
                    break;
                }
            }
            return hexs;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            byte[] inputByteArray = CombineInputStr().ToArray();
            _gainBytes = CRC.crc16_modbus(inputByteArray, (uint)inputByteArray.Length);
            char[] charArray = Convert.ToString(_gainBytes, 16).ToCharArray();
            tbxCRC.Text = charArray[2].ToString().ToUpper() + charArray[3].ToString().ToUpper() + "-" +
                charArray[0].ToString().ToUpper() + charArray[1].ToString().ToUpper();
        }

        private byte HexstrToint(string str)
        {
            byte hexH = 0;
            byte hexL = 0;
            for (int i = 0; i < str.Length; i++)
            {
                char ch = str[i];
                if ((ch >= '0') && (ch <= '9'))
                {
                    if (i == 0)
                    {
                        hexH = Convert.ToByte((ch - '0') * 16);
                    }
                    else
                    {
                        hexL = Convert.ToByte((ch - '0'));
                    }
                }
                else if ((ch >= 'A') && (ch <= 'F'))
                {
                    if (i == 0)
                    {
                        hexH = Convert.ToByte((ch - 'A' + 10) * 16);
                    }
                    else
                    {
                        hexL = Convert.ToByte((ch - 'A' + 10));
                    }
                }
            }
            return Convert.ToByte(hexL + hexH);
        }

    }
}
