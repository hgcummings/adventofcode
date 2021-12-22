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

    function findOverlap(a, b) {
        const o = {
            x1: Math.max(a.x1, b.x1), x2: Math.min(a.x2, b.x2),
            y1: Math.max(a.y1, b.y1), y2: Math.min(a.y2, b.y2),
            z1: Math.max(a.z1, b.z1), z2: Math.min(a.z2, b.z2)
        }
        if (o.x2 < o.x1 || 
            o.y2 < o.y1 || 
            o.z2 < o.z1) {
            return false;
        }
        return o;
    }

    function split(a, b) {
        const xRanges = [[a.x1, b.x1-1],[b.x1, b.x2],[b.x2 + 1, a.x2]];
        const yRanges = [[a.y1, b.y1-1],[b.y1, b.y2],[b.y2 + 1, a.y2]];
        const zRanges = [[a.z1, b.z1-1],[b.z1, b.z2],[b.z2 + 1, a.z2]];
        for (const ranges of [xRanges, yRanges, zRanges]) {
            for (let i = ranges.length - 1; i >= 0; --i) {
                if (ranges[i][1] < ranges[i][0]) {
                    ranges.splice(i,1);
                }
            }
        }

        const splitRegions = [];        
        for (const xRange of xRanges) {
            for (const yRange of yRanges) {
                for (const zRange of zRanges) {
                    splitRegions.push({
                        x1: xRange[0], x2: xRange[1],
                        y1: yRange[0], y2: yRange[1],
                        z1: zRange[0], z2: zRange[1]
                    })
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
        console.log(`${ci+1} / ${cmds.length}`);
        const cmdRegions = [cmds[ci]];
        while (cmdRegions.length) {
            const cmdRegion = cmdRegions.pop();
            let overlap;
            for (let i = 0; i < onRegions.length; ++i) {
                const onRegion = onRegions[i];
                overlap = findOverlap(cmdRegion, onRegion);
                if (overlap) {
                    const newOnRegions = split(onRegion, overlap);
                    if (sw === false) {
                        const index = newOnRegions.findIndex(reg => _.isEqual(reg, overlap));
                        newOnRegions.splice(index, 1);
                    }
                    onRegions.splice(i, 1, ...newOnRegions);
                    const newCmdRegions = split(cmdRegion, overlap);
                    const index = newCmdRegions.findIndex(reg => _.isEqual(reg, overlap));
                    newCmdRegions.splice(index, 1);
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
    }

    console.log(_.sumBy(onRegions, size));
    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();