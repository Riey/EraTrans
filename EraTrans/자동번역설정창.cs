using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
namespace 에라번역
{
    public partial class 자동번역설정창 : Form
    {
        XmlDocument setting_xml;
        public 자동번역설정창(XmlDocument setting_xml)
        {
            this.setting_xml = setting_xml;
            InitializeComponent();
        }

        private void 자동번역설정창_Load(object sender, EventArgs e)
        {
            id.Text = setting_xml.DocumentElement.SelectSingleNode("NaverID").InnerText??"";
            secret.Text = setting_xml.DocumentElement.SelectSingleNode("NaverSecret").InnerText ?? "";
        }

        private void 저장버튼_Click(object sender, EventArgs e)
        {
            setting_xml.DocumentElement.SelectSingleNode("NaverID").InnerText = id.Text ?? "";
            setting_xml.DocumentElement.SelectSingleNode("NaverSecret").InnerText = secret.Text ?? "";
            Close();
        }
    }
}
