const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const { matrix, multiply, column } = require('mathjs');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    const facings = [
        matrix([[1,0,0],[0,1,0],[0,0,1]]),
        matrix([[0,1,0],[-1,0,0],[0,1,0]]),
        matrix([[-1,0,0],[0,-1,0],[0,0,1]]),
        matrix([[0,-1,0],[1,0,0],[0,0,1]]),
        matrix([[0,0,1],[0,1,0],[-1,0,0]]),
        matrix([[0,0,-1],[0,1,0],[1,0,0]])
    ];

    const rotations = [
        matrix([[1,0,0],[0,1,0],[0,0,1]]),
        matrix([[1,0,0],[0,-1,0],[0,0,-1]]),
        matrix([[1,0,0],[0,0,1],[0,-1,0]]),
        matrix([[1,0,0],[0,0,-1],[0,1,0]])
    ];

    const transformations = [];
    for (const facing of facings) {
        for (const rotation of rotations) {
            transformations.push(multiply(facing, rotation));
        }
    }

    const expr = /--- scanner (?<id>[0-9]+) ---/;

    let maps = [];
    let i;
    for await (const line of rl) {
        const result = expr.exec(line);
        if (result && result.groups) {
            i = parseInt(result.groups.id, 10);
            maps[i] = {
                beacons: []
            }
        } else if (line) {
            maps[i].beacons.push(line.split(",").map(c => parseInt(c, 10)));
        }
    }

    function normalise(origin) {
        return beacon => [beacon[0] - origin[0], beacon[1] - origin[1], beacon[2] - origin[2]];
    }

    while (maps.length > 1) {
        console.log("NEW PASS", maps.map(map => map.beacons.length));
        let pair_n = 0;
        for (let i = 0; i < maps.length; ++i) {
            let foundMatch = false;
            for (let j = i + 1; j < maps.length; ++j) {
                console.log("Pair " + (++pair_n));
                const pair = [maps[i], maps[j]];
                let newMap = undefined;
                const as = pair[0].beacons;
                for (const transformation of transformations) {
                    const bs = pair[1].beacons.map(beacon => multiply(beacon, transformation)._data);

                    for (const origin_a of as) {
                        let nas = as.map(normalise(origin_a));
                        let matchingOrigin = undefined;
                        for (const origin_b of bs) {
                            let nbs = bs.map(normalise(origin_b));
                            let matches = 0;
                            for (const a of nas) {
                                for (const b of nbs) {
                                    if (_.isEqual(a, b)) {
                                        matches += 1;
                                    }
                                    if (matches >= 12) {
                                        break;
                                    }
                                }
                                if (matches >= 12) {
                                    break;
                                }
                            }
                            if (matches >= 12) {
                                matchingOrigin = origin_b;
                                break;
                            }
                        }
                        if (matchingOrigin) {
                            // console.log("Found match");
                            newMap = {};
                            // console.log(as.length, bs.length);
                            const allBeacons = as.map(normalise(origin_a)).concat(bs.map(normalise(matchingOrigin)));
                            // console.log(allBeacons.length);
                            // console.log(allBeacons);
                            const uniqueBeacons = _.uniqWith(allBeacons, _.isEqual);
                            // console.log(uniqueBeacons.length);
                            // console.log(uniqueBeacons);
                            newMap.beacons = uniqueBeacons;
                            
                            break;
                        }
                    }
                    if (newMap) {
                        break;
                    }
                }
                if (newMap) {
                    const newMaps = [];
                    for (let k = 0; k < maps.length; ++k) {
                        if (k !== i && k !== j){
                            newMaps.push(maps[k]);
                        }
                    }
                    newMaps.push(newMap);
                    maps = newMaps;
                    foundMatch = true;
                    break;
                }
            }
            if (foundMatch) {
                break;
            }
        }
    }
    
    console.log(maps[0].beacons.length);
}

processLineByLine();