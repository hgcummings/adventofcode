const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const {combinations} = require('combinatorial-generators');
const {performance} = require('perf_hooks');

const monster = {
    "HP": 109,
    "damage": 8,
    "armour": 2
};

function playerWins(player) {
    const playerDamagePerTurn = Math.max(player.damage - monster.armour, 1);
    const monsterDamagePerTurn = Math.max(monster.damage - player.armour, 1);

    return monster.HP / playerDamagePerTurn < player.HP / monsterDamagePerTurn;
}

const startTime = performance.now();
async function processLineByLine() {
    const fileStream = fs.createReadStream('shop.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    const weapons = [];
    const armourOpts = [];
    const rings = [];
    let current;

    const pattern = /(?<name>[A-Z]+( \+[0-9])?)\s*(?<cost>[0-9]+)\s*(?<damage>[0-9]+)\s*(?<armour>[0-9]+)/i
    for await (const line of rl) {
        if (line.startsWith("Weapons:")) {
            current = weapons;
            continue;
        } else if (line.startsWith("Armor:")) {
            current = armourOpts;
            continue;
        } else if (line.startsWith("Rings:")) {
            current = rings;
            continue;
        } else {
            const match = pattern.exec(line);
            if (match) {
                let {name, cost, damage, armour} = match.groups;
                current.push({
                    name,
                    cost: parseInt(cost, 10),
                    damage: parseInt(damage, 10),
                    armour: parseInt(armour, 10)
                });
            }
        }
    }


    const player = {
        "HP": 100
    };
    let maxCost = -Infinity;
    for (let i = 0; i < 3; ++i) {
        for (const combination of combinations(rings, i)) {
            const ringDamage = _.sumBy(combination, ring => ring.damage);
            const ringArmour = _.sumBy(combination, ring => ring.armour);
            const ringCost = _.sumBy(combination, ring => ring.cost);
            for (const weapon of weapons) {
                for (const armourOpt of armourOpts) {
                    player.damage = weapon.damage + ringDamage;
                    player.armour = armourOpt.armour + ringArmour;
                    const cost = ringCost + weapon.cost + armourOpt.cost;
                    if (cost > maxCost && !playerWins(player, cost)) {
                        maxCost = cost;
                    }
                }
            }
        }
    }

    console.log(maxCost);

    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();