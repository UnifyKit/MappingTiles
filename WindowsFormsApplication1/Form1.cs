using MappingTiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OpenStreetMapTileSource osmTileSource = new OpenStreetMapTileSource();
            TileLayer tileLayer = new TileLayer(osmTileSource);

            map1.Layers.Add(tileLayer);
        }
    }
}
