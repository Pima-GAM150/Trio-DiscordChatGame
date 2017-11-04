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
    public async Task SpawnUnit(CommandContext ctx, [Description("Name of unit. See: !listunits")] string unit, [Description("Number of unit")] uint count)
    {
        await ctx.TriggerTypingAsync();

        if (!SpawnEnemy.enemyDictionary.Any())
        {
            await ctx.RespondAsync($"enemyDictionary empty! **The game is broken**:clap::clap:");
            return;
        }
    }

    [Command("listunits")]
    [Aliases("units", "lu")]
    [Description("Lists the spawnable units, their price, and the income rewarded.")]
    public async Task ListUnits(CommandContext ctx)
    {
        await ctx.TriggerTypingAsync();
    }
}