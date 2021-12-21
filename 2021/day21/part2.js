let worlds = [
    {
        pos0: 1,
        pos1: 0,
        score0: 0,
        score1: 0,
        instances: 1
    }
];

let rolls = [0,0,0,1,3,6,7,6,3,1];

const wins = [0,0];
while (worlds.length) {
    let newWorlds = [];
    for (world of worlds) {
        for (let i = 3; i <= 9; ++i) {
            const pos0 = (world.pos0 + i) % 10;
            const score0 = world.score0 + pos0 + 1;

            if (score0 >= 21) {
                wins[0] += world.instances * rolls[i];
            } else {
                for (let j = 3; j <= 9; ++j) {
                    const pos1 = (world.pos1 + j) % 10;
                    const score1 = world.score1 + pos1 + 1;
                    const instances =  world.instances * rolls[i] * rolls[j];
    
                    if (score1 >= 21) {
                        wins[1] += instances;
                    } else {
                        newWorlds.push({
                            pos0,
                            pos1,
                            score0,
                            score1,
                            instances
                        });
                    }
                }
            }
        }
    }
    worlds = newWorlds;
    console.log(worlds.length);
    console.log(worlds[worlds.length - 1]);
}


console.log(wins);