const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const {performance} = require('perf_hooks');

const GIFEncoder = require('gifencoder');
const { createCanvas } = require('canvas');

const encoder = new GIFEncoder(2000, 2000);

encoder.createReadStream().pipe(fs.createWriteStream('animation.gif'));

encoder.start();
encoder.setRepeat(0);
encoder.setDelay(50);
encoder.setQuality(10);

const canvas = createCanvas(2000, 2000);
const ctx = canvas.getContext('2d');

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

    function split(a, b) {
        const xRanges = [[a.x1, b.x1-1],[b.x1, b.x2],[b.x2 + 1, a.x2]];
        const yRanges = [[a.y1, b.y1-1],[b.y1, b.y2],[b.y2 + 1, a.y2]];
        const zRanges = [[a.z1, b.z1-1],[b.z1, b.z2],[b.z2 + 1, a.z2]];

        const splitRegions = [];        
        for (const xRange of xRanges) {
            for (const yRange of yRanges) {
                for (const zRange of zRanges) {
                    const splitRegion = {
                        x1: xRange[0], x2: xRange[1],
                        y1: yRange[0], y2: yRange[1],
                        z1: zRange[0], z2: zRange[1]
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

        
        if (ci >= 20) {
            ctx.fillStyle = '#000000';
            ctx.fillRect(0, 0, 2000,2000);

            const regionsByDistance = [[],[],[],[],[]];

            for (const r of onRegions) {
                let distance = 4;
                for (let d = 0; d < 4; ++d) {
                    if (r.y1 <= (10 * Math.pow(10,d)) && r.y2 >= (-10 * Math.pow(10,d))) {
                        distance = d;
                        break;
                    }
                }
                regionsByDistance[distance].push(r);
            }

            console.log(regionsByDistance.map(a => a.length));

            for (let d = 4; d >= 0; --d) {

                const shade = 255 - (50 * d);
                ctx.fillStyle = `rgb(${shade}, ${shade}, ${shade})`;

                for (const r of regionsByDistance[d]) {
                    ctx.fillRect(Math.round((r.x1 / 100)) + 1000, Math.round((r.z1 / 100)) + 1000,
                        (Math.round((r.x2-r.x1)) / 100) + 1, (Math.round((r.z2-r.z1)) / 100) + 1);
                }
            }
            
            encoder.addFrame(ctx);
        }
    }

    encoder.finish();

    // Find most populous plane
    let x0 = [];
    let y0 = [];
    let z0 = [];
    for (let d = -100000; d <= 100000; ++d){
        x0[d + 100000] = 0;
        y0[d + 100000] = 0;
        z0[d + 100000] = 0;
    }
    for (const r of onRegions) {
        for (let d = -100000; d <= 100000; ++d){
            if (r.x1 <= d && r.x2 >= d) { x0[d + 100000] += 1; }
            if (r.y1 <= d && r.y2 >= d) { y0[d + 100000] += 1; }
            if (r.z1 <= d && r.z2 >= d) { z0[d + 100000] += 1; }
        }
    }
    
    const maxX = _.max(x0);
    const maxY = _.max(y0);
    const maxZ = _.max(z0);
     
    console.log(maxX, maxY, maxZ);
    console.log(x0.indexOf(maxX), y0.indexOf(maxY), z0.indexOf(maxZ));

    // Outputs:
    // 1447 1689 1522
    // 99992 100001 128797 (these are all +100000)
    // So most populous plane is y = 1 

    console.log(_.sumBy(onRegions, size));
    console.log(1323862415207825);
    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();