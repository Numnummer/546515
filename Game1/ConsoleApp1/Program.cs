using GameClient;
using System.Text;
using System.Threading.Channels;

Console.WriteLine("Введите мод: c или s");
var mode = Console.ReadLine();
Console.WriteLine("Введите имя");
var name = Console.ReadLine();
Player p = null;
if (mode == "c")
{
    p = new Player(Mode.Client, name);
}
if (mode == "s")
{
    p = new Player(Mode.Server, name);
}
if (p==null)
{
    throw new Exception("Неправильный мод");
}

p.BindOnPlayerLeaved(PlayerLeaved);
p.BindOnNewPlayer(NewPlayer);
p.BindOnNewScore(NewScore);


while (true)
{
    var command = Console.ReadLine();
    if (command == "sc")
    {
        _=p.SendScoreAsync("42");
    }
    if (command=="q")
    {
        _=p.EndWorkAsync();
    }
}

void NewPlayer(string name) => Console.WriteLine(name);
void NewScore(string name, string score) => Console.WriteLine(name+' '+score);
void PlayerLeaved(string name) => Console.WriteLine(name);