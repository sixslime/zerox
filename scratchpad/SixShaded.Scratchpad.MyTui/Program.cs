namespace SixShaded.Scratchpad.MyTui;
using Terminal.Gui.Configuration;
using Terminal.Gui.App;
using Terminal.Gui.Drawing;
using Terminal.Gui.ViewBase;
using Terminal.Gui.Views;
using System;
using Terminal.Gui;
using Terminal.Gui.Input;

class Program
{
    static void Main()
    {
        Application.Init();

        var win = new Window()
        {
            Title = "Slider ×2 Example",
            X = 0,
            Y = 1,                 // leave room for menu/status if you add them
            Width = Dim.Fill(),
            Height = Dim.Fill()
        };

        var inWin = new Window()
        {
            Title = "test",
            X = Pos.Center(),
            Y = Pos.Center()-1,                 // leave room for menu/status if you add them
            Width = Dim.Percent(30),
            Height = Dim.Percent(30)
        };

        int value = 50;
        const int min = 0;
        const int max = 100;
        const int barWidth = 30;

        Label bar = new Label()
        {
            Text = MakeBar(value, barWidth),
            X = Pos.Center(),
            Y = 1,
            Width = barWidth + 3
        };

        var dec = new Button() { Text = "<", X = Pos.Center() - 20, Y = 1 };
        var inc = new Button() { Text = ">", X = Pos.Center() + 20, Y = 1 };

        Label result = new Label()
        {
            Text = $"Result: {value * 2}",
            X = Pos.Center(),
            Y = 3
        };

        void UpdateUI()
        {
            bar.Text = MakeBar(value, barWidth);
            result.Text = $"Result: {value * 2}";
            bar.SetNeedsDraw();
            result.SetNeedsDraw();
        }

        dec.Accepting += (s, e) => {
            if (value > min)
            {
                value--;
                UpdateUI();
            }
            e.Handled = true;
        };

        inc.Accepting += (s, e) => {
            if (value < max)
            {
                value++;
                UpdateUI();
            }
            e.Handled = true;
        };

        // Optional: allow clicking the bar to set value by approximate position.
        bar.MouseEvent += (s, e) => {
                // compute fractional position inside bar and map to [min,max]
            if (!e.Flags.HasFlag(MouseFlags.Button1Pressed)) return;
                int localX = e.Position.X - ((bar.Frame.Width - barWidth) / 2);
                if (localX < 0) localX = 0;
                if (localX > barWidth - 1) localX = barWidth - 1;
                value = (int)Math.Round((localX / (double)(barWidth - 1)) * (max - min)) + min;
                UpdateUI();
        };
        bool blink = true;
        inWin.Add(result);
        win.Add(dec, bar, inc, inWin);
        Application.AddTimeout(
        TimeSpan.FromSeconds(0.4), () =>
        {
            if (blink)
            {
                result.Visible = false;
            }
            else
            {
                result.Visible = true;
            }
            blink = !blink;
            inWin.SetNeedsDraw();
            UpdateUI();
            return true;
        });
        Application.Run(win);
        win.Dispose();
        Application.Shutdown();
    }

    static string MakeBar(int v, int width)
    {
        int filled = (v * width) / 100;
        if (filled >= width)
        {
            return "[" + new string('=', width) + $"]";
        }
        else
        {
            int spaces = Math.Max(0, width - filled - 1);
            return "[" + new string('=', filled) + ">" + new string(' ', spaces) + $"]";
        }
    }
}