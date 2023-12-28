using Sapper.Controllers;

namespace Game
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            StartController.CreatePlayerForm(this);
        }
    }
}
