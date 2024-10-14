// See https://aka.ms/new-console-template for more information

using Cadwise.ATM;
using Cadwise.ATM.Exceptions;
using Spectre.Console;


var atm = new Atm();

while (true)
{
    try
    {
        AnsiConsole.Clear();
        var menu = AnsiConsole.Prompt(new SelectionPrompt<string>()
            .Title("Что хотел?")
            .AddChoices([Menu.AddCash, Menu.GetCash, Menu.PrintCash, Menu.Exit]));

        AnsiConsole.Clear();
        switch (menu)
        {
            case Menu.AddCash:
                var denomination = AnsiConsole.Prompt(new SelectionPrompt<int>()
                    .Title("Какие купюры?")
                    .AddChoices(atm.GetCassettes().Select(x => x.BanknoteDenomination)));
                var count = AnsiConsole.Prompt(new TextPrompt<int>("Сколько штук?"));
                var notLoaded = atm.Load(denomination, count);

                if (notLoaded > 0)
                {
                    AnsiConsole.WriteLine($"В банкомат не поместилось {notLoaded} купюр");
                }

                PrintCash(atm);

                var more = AnsiConsole.Prompt(new ConfirmationPrompt("Докинем ещё?") { DefaultValue = true });
                if (more)
                {
                    AnsiConsole.Clear();
                    goto case Menu.AddCash;
                }

                break;
            case Menu.GetCash:
                var amount = AnsiConsole.Prompt(new TextPrompt<int>("Сколько тебе надо?"));
                var exchange = AnsiConsole.Prompt(new ConfirmationPrompt("Мелкими?") { DefaultValue = false });

                var banknots = atm.GetCash(amount, exchange);

                if (banknots.Any())
                {
                    AnsiConsole.Write(new Rows(banknots.Select(x => new Text($"{x.Key}руб - {x.Value}шт"))));
                }
                else
                {
                    AnsiConsole.WriteLine("Не возможно выдать требуемую сумму");
                }

                break;
            case Menu.PrintCash:
                PrintCash(atm);
                break;
            case Menu.Exit:
                Environment.Exit(0);
                break;
        }

        Console.ReadKey();
    }
    catch (BusinessLogicException ex)
    {
        AnsiConsole.Clear();
        AnsiConsole.WriteLine(ex.Message);
        Console.ReadKey();
    }
}

void PrintCash(Atm atm1)
{
    AnsiConsole.Write(new BarChart()
        .Width(60)
        .Label("Купюры в банкомате")
        .CenterLabel()
        .AddItems(atm1.GetCassettes(),
            item => new BarChartItem($"{item.BanknoteDenomination}руб", item.BanknoteCount,
                item.BanknoteCount > 0 ? Color.Green : Color.Red)));
}

public static class Menu
{
    public const string AddCash = "Внести наличные";
    public const string GetCash = "Забрать наличные";
    public const string PrintCash = "Посмотреть купюры";
    public const string Exit = "Выход";
}