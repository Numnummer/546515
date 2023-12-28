using GameClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
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
            playerForm.Size = new Size(350, 250);

            // Создаем контролы на форме
            Label typeLabel = new Label();
            typeLabel.Text = "Select Player Type:";
            typeLabel.Location = new Point(10, 20);
            typeLabel.AutoSize = true;
            playerForm.Controls.Add(typeLabel);

            ComboBox typeComboBox = new ComboBox();
            typeComboBox.Items.Add("Client");
            typeComboBox.Items.Add("Server");
            typeComboBox.Location = new Point(150, 17);
            typeComboBox.Size = new Size(150, 23);
            playerForm.Controls.Add(typeComboBox);

            Label nameLabel = new Label();
            nameLabel.Text = "Enter Player Name:";
            nameLabel.Location = new Point(10, 60);
            nameLabel.AutoSize = true;
            playerForm.Controls.Add(nameLabel);

            TextBox nameTextBox = new TextBox();
            nameTextBox.Location = new Point(150, 57);
            nameTextBox.Size = new Size(150, 20);
            playerForm.Controls.Add(nameTextBox);

            Label ipLabel = new Label();
            ipLabel.Text = "Enter IP Address:";
            ipLabel.Location = new Point(10, 100);
            ipLabel.AutoSize = true;
            playerForm.Controls.Add(ipLabel);

            TextBox ipTextBox = new TextBox();
            ipTextBox.Location = new Point(150, 97);
            ipTextBox.Size = new Size(150, 20);
            playerForm.Controls.Add(ipTextBox);

            Button submitButton = new Button();
            submitButton.Text = "Submit";
            submitButton.Location = new Point(150, 140);
            submitButton.Size = new Size(100, 30);
            submitButton.Click += (sender, e) =>
            {
                if (typeComboBox.SelectedItem==null
                    || string.IsNullOrEmpty(nameTextBox.Text)
                    || !IPAddress.TryParse(ipTextBox.Text, out var ip))
                {
                    return;
                }
                var playerType = typeComboBox.SelectedItem.ToString();
                var playerName = nameTextBox.Text;
                //var ip = ipTextBox.Text;

                // Можно использовать полученные значения типа игрока и имени игрока
                // для дальнейших действий, например, сохранения их.

                playerForm.Controls.Clear();
                switch (playerType)
                {
                    case "Client":
                        MapController.InitNetwork(ip, Mode.Client, playerName);
                        break;
                    case "Server":
                        MapController.InitNetwork(ip, Mode.Server, playerName);
                        break;
                    default:
                        break;
                }
                MapController.Init(current);
            };
            playerForm.Controls.Add(submitButton);
        }
    }
}
