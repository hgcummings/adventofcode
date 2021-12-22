const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const {performance} = require('perf_hooks');

const startTime = performance.now();
async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });

    let expr = /(?<cmd>(on|off)) x=(?<x1>\-?[0-9]+)\.\.(?<x2>\-?[0-9]+),y=(?<y1>\-?[0-9]+)..(?<y2>\-?[0-9]+),z=(?<z1>\-?[0-9]+)..(?<z2>\-?[0-9]+)/;
    let cmds = [];
    for await (const line of rl) {
        const {cmd, x1, x2, y1, y2, z1, z2} = expr.exec(line).groups;
        cmds.push({
            cmd: cmd === "on" ? true : false,
            x1: parseInt(x1, 10),
            x2: parseInt(x2, 10),
            y1: parseInt(y1, 10),
            y2: parseInt(y2, 10),
            z1: parseInt(z1, 10),
            z2: parseInt(z2, 10)
        });
    }

    function isValid(r) {
        return (r.x1 <= r.x2 &&
                r.y1 <= r.y2 &&
                r.z1 <= r.z2);
    }

    function findOverlap(a, b) {
        const overlap = {
            x1: Math.max(a.x1, b.x1), x2: Math.min(a.x2, b.x2),
            y1: Math.max(a.y1, b.y1), y2: Math.min(a.y2, b.y2),
            z1: Math.max(a.z1, b.z1), z2: Math.min(a.z2, b.z2)
        }
        return isValid(overlap) ? overlap : false;
    }

    function split(a, b, includeB) {
        const xRanges = [[a.x1, b.x1-1],[b.x1, b.x2],[b.x2 + 1, a.x2]];
        const yRanges = [[a.y1, b.y1-1],[b.y1, b.y2],[b.y2 + 1, a.y2]];
        const zRanges = [[a.z1, b.z1-1],[b.z1, b.z2],[b.z2 + 1, a.z2]];

        const splitRegions = [];        
        for (let i = 0; i < 3; ++i) {
            for (let j = 0; j < 3; ++j) {
                for (let k = 0; k < 3; ++k) {
                    if ((!includeB) && i === 1 && j === 1 && k === 1) {
                        continue;
                    }

                    const splitRegion = {
                        x1: xRanges[i][0], x2: xRanges[i][1],
                        y1: yRanges[j][0], y2: yRanges[j][1],
                        z1: zRanges[k][0], z2: zRanges[k][1]
                    };
                    if (isValid(splitRegion)) {
                        splitRegions.push(splitRegion);
                    }
                }
            }
        }

        return splitRegions;
    }

    function size(r) {
        return ((r.x2 - r.x1) + 1) * ((r.y2 - r.y1) + 1) * ((r.z2 - r.z1) + 1);
    }

    let onRegions = [];
    for (let ci = 0; ci < cmds.length; ++ci) {
        const sw = cmds[ci].cmd;
        if ((ci+1) % 60 === 0) {
            console.log(`${ci+1} / ${cmds.length}`);
        }
        const cmdRegions = [cmds[ci]];

        let overlapRegions;
        [overlapRegions, onRegions] = _.partition(onRegions, r => findOverlap(r, cmds[ci]));

        while (cmdRegions.length) {
            const cmdRegion = cmdRegions.pop();
            let overlap;
            for (let i = 0; i < overlapRegions.length; ++i) {
                const overlapRegion = overlapRegions[i];
                
                overlap = findOverlap(cmdRegion, overlapRegion);
                if (overlap) {
                    const newOverlapRegions = split(overlapRegion, overlap, sw);
                    overlapRegions.splice(i, 1, ...newOverlapRegions);
                    const newCmdRegions = split(cmdRegion, overlap, false);
                    cmdRegions.push(...newCmdRegions);
                    break;
                }
            }
            if (!overlap) {
                if (sw) {
                    onRegions.push(cmdRegion);
                }
            }
        }

        onRegions.push(...overlapRegions);
    }

    console.log(_.sumBy(onRegions, size));
    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();