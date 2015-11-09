using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Administration;

namespace Profchat27
{
    public partial class Chatscreen : Form
    {
        public Administrator admin;

        public Chatscreen(Administrator a, List<string> users)
        {
            InitializeComponent();
            this.admin = a;
        }
    }
}
