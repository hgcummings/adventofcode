const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const { matrix, multiply } = require('mathjs');
const ArrayKeyedMap = require('array-keyed-map');

async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });

    const rotation = [
        // Identity
        matrix([[1,0,0],
                [0,1,0],
                [0,0,1]]),
        // Rotation of x
        matrix([[0,1,0],
                [-1,0,0],
                [0,0,1]]),
        matrix([[0,-1,0],
                [1,0,0],
                [0,0,1]]),
        matrix([[0,0,1],
                [0,1,0],
                [-1,0,0]]),
        matrix([[0,0,-1],
                [0,1,0],
                [1,0,0]]),
        matrix([[-1,0,0],
                [0,-1,0],
                [0,0,1]]),
        matrix([[-1,0,0],
                [0,1,0],
                [0,0,-1]]),
    ];

    const facing = [
        // Identity
        matrix([[1,0,0],
                [0,1,0],
                [0,0,1]]),
        // Rotation around x
        matrix([[1,0,0],
                [0,0,1],
                [0,-1,0]]),
        matrix([[1,0,0],
                [0,0,-1],
                [0,1,0]]),
        matrix([[1,0,0],
                [0,-1,0],
                [0,0,-1]])
    ];

    let transformations = [];
    for (const t1 of rotation) {
        for (const t2 of facing) {
            const t = multiply(t1, t2);
            transformations.push(t);
        }
    }
    transformations = _.uniqBy(transformations, t => JSON.stringify(t._data));

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

    const s = [[0,0,0]];
    function tryMerge(map_a, map_b) {
        const as = map_a.beacons;
        for (const transformation of transformations) {
            const bs = map_b.beacons.map(beacon => multiply(beacon, transformation)._data);

            let counts = new ArrayKeyedMap();
            for (const a of as) {
                for (const b of bs) {
                    const t = [a[0]-b[0], a[1]-b[1], a[2]-b[2]];
                    if (!counts.has(t)) {
                        counts.set(t, 1);
                    } else {
                        const count = counts.get(t);
                        if (count < 11) {
                            counts.set(t, count + 1);
                        } else {
                            s.push(t);
                            const tbs = bs.map(b => [b[0] + t[0], b[1] + t[1], b[2] + t[2]]);
        
                            const allBeacons = tbs.concat(as);
                            const uniqueBeacons = _.uniqWith(allBeacons, _.isEqual);
                            return {
                                beacons: uniqueBeacons
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    while (maps.length > 1) {
        console.log("NEW PASS", maps.map(map => map.beacons.length));
        let pair_n = 0;

        for (let i = maps.length - 1; i > 0; --i) {
            console.log("Pair " + (++pair_n));
            const newMap = tryMerge(maps[0], maps[i]);
            if (newMap) {
                maps.splice(i, 1);
                maps[0] = newMap;
            }
        }
    }

    let maxDistance = -Infinity;
    for (let i = 0; i < s.length - 1; ++i) {
        for (let j = i + 1; j < s.length; ++j) {
            const distance = Math.abs(s[i][0] - s[j][0]) + Math.abs(s[i][1] - s[j][1]) + Math.abs(s[i][2] - s[j][2]);
            if (distance > maxDistance) {
                maxDistance = distance;
            }
        }
    }
    
    console.log(maxDistance);
}

processLineByLine();