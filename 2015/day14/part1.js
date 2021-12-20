const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let reindeer = [];
    const pattern = /(?<name>[A-Za-z]+) can fly (?<speed>[0-9]+) km\/s for (?<flyTime>[0-9]+) seconds, but then must rest for (?<restTime>[0-9]+) seconds./
    for await (const line of rl) {
        let {name, speed, flyTime, restTime} = pattern.exec(line).groups;
        reindeer.push({
            name,
            speed: parseInt(speed, 10),
            flyTime: parseInt(flyTime, 10),
            restTime: parseInt(restTime, 10)
        });
    }

    const duration = 2503;
    for (const deer of reindeer) {
        const cycleDuration = deer.flyTime + deer.restTime;
        const cycleDistance = deer.flyTime * deer.speed;
        const wholeCycleDistance = Math.floor(duration / cycleDuration) * cycleDistance;
        const partCycleDistance = Math.min(deer.flyTime, (duration % cycleDuration)) * deer.speed;
        deer.totalDistance = wholeCycleDistance + partCycleDistance;
    }
    
    console.log(_.maxBy(reindeer, deer => deer.totalDistance));
}

processLineByLine();