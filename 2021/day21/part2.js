let ArrayKeyedMap = require('array-keyed-map');
let {performance} = require('perf_hooks');

const startTime = performance.now();

let worlds = new ArrayKeyedMap();
worlds.set([1,0,0,0], 1);

let rolls = [0,0,0,1,3,6,7,6,3,1];

const wins = [0,0];
while (worlds.size) {
    let newWorlds = new ArrayKeyedMap();
    for (const [world, instances] of worlds.entries()) {
        for (let i = 3; i <= 9; ++i) {
            const [pos0, pos1, score0, score1] = world;
            const newPos0 = (pos0 + i) % 10;
            const newScore0 = score0 + newPos0 + 1;

            if (newScore0 >= 21) {
                wins[0] += instances * rolls[i];
            } else {
                for (let j = 3; j <= 9; ++j) {
                    const newPos1 = (pos1 + j) % 10;
                    const newScore1 = score1 + newPos1 + 1;
                    const newInstances = instances * rolls[i] * rolls[j];
    
                    if (newScore1 >= 21) {
                        wins[1] += newInstances;
                    } else {
                        const newWorld = [newPos0, newPos1, newScore0, newScore1];
                        if (newWorlds.has(newWorld)) {
                            newWorlds.set(newWorld, newWorlds.get(newWorld) + newInstances);
                        } else {
                            newWorlds.set(newWorld, newInstances);
                        }
                    }
                }
            }
        }
    }
    worlds = newWorlds;
    console.log(newWorlds.size);
}

console.log(performance.now() - startTime);

console.log(wins);