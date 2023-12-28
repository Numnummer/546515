using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sapper.Controllers
{
    public static class StartController
    {
        public static void CreatePlayerForm(Form current)
        {
            // Создаем новую форму для заполнения типа игрока и имени игрока
            Form playerForm = current;
            playerForm.Text = "Player Form";
            playerForm.StartPosition = FormStartPosition.CenterScreen;
            playerForm.Size = new Size(300, 200);

            // Создаем контролы на форме
            Label typeLabel = new Label();
            typeLabel.Text = "Select Player Type:";
            typeLabel.Location = new Point(10, 20);
            typeLabel.AutoSize = true;
            playerForm.Controls.Add(typeLabel);

            ComboBox typeComboBox = new ComboBox();
            typeComboBox.Items.Add("Client");
            typeComboBox.Items.Add("Server");
            typeComboBox.Location = new Point(130, 17);
            typeComboBox.Size = new Size(150, 23);
            playerForm.Controls.Add(typeComboBox);

            Label nameLabel = new Label();
            nameLabel.Text = "Enter Player Name:";
            nameLabel.Location = new Point(10, 60);
            nameLabel.AutoSize = true;
            playerForm.Controls.Add(nameLabel);

            TextBox nameTextBox = new TextBox();
            nameTextBox.Location = new Point(130, 57);
            nameTextBox.Size = new Size(150, 20);
            playerForm.Controls.Add(nameTextBox);

            Button submitButton = new Button();
            submitButton.Text = "Submit";
            submitButton.Location = new Point(130, 100);
            submitButton.Size = new Size(100, 30);
            submitButton.Click += (sender, e) =>
            {
                string playerType = typeComboBox.SelectedItem.ToString();
                string playerName = nameTextBox.Text;

                // Можно использовать полученные значения типа игрока и имени игрока
                // для дальнейших действий, например, сохранения их.

                playerForm.Controls.Clear();
                MapController.Init(current);
            };
            playerForm.Controls.Add(submitButton);
        }
    }
}
