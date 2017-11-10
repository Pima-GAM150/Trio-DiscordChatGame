using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class DiscordCommands
{
    [Command("spawn")]
    [Aliases("su", "spawnunit")]
    [Description("Spawns a unit of type if user has enough funds")]
    public async Task SpawnUnit(CommandContext ctx, [Description("Name of unit. See: !listunits")] string unit, [Description("Number of unit")] int count)
    {
        await ctx.TriggerTypingAsync();

        // use this snippet at the top of a method to re-call it on the main thread. will be delayed by a single frame.
        if (!MainThreadQueue.Instance.IsMain())
        {
            MainThreadQueue.Instance.Queue(async () => await SpawnUnit(ctx, unit, count));
            return;
        }

        if (!SpawnEnemy.instance.enemyDictionary.Any())
        {
            await ctx.RespondAsync($"enemyDictionary empty! **The game is broken**:clap::clap:");
            return;
        }
        foreach (var enemy in SpawnEnemy.instance.enemyDictionary)
        {
            if (enemy.key == unit)
            {
                // foreach (var player in SpawnEnemy.enemyIncome)

                SpawnEnemy.instance.instantiateEnemy(unit, ctx.User.Username, count);
                await ctx.RespondAsync($"Spawned {count} Enemies!");
                return;
            }
        }
    }

    [Command("listunits")]
    [Aliases("units", "lu")]
    [Description("Lists the spawnable units, their price, and the income rewarded.")]
    public async Task ListUnits(CommandContext ctx)
    {
        await ctx.TriggerTypingAsync();

        if (!SpawnEnemy.instance.enemyDictionary.Any())
        {
            await ctx.Channel.SendMessageAsync("No Enemies! **Games borken!**");
            return;
        }
        string msg = "";
        foreach (var enemy in SpawnEnemy.instance.enemyDictionary)
        {
            msg += enemy.key;
            msg += "\n";
        }
        await ctx.Channel.SendMessageAsync(msg);
    }
}