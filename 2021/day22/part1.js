const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const {performance} = require('perf_hooks');

const offset = 50;
const limit = 100;

const startTime = performance.now();
async function processLineByLine() {
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let cubes = [];
    for (let x = 0; x <= limit; ++x) {
        cubes[x] = [];
        for (let y = 0; y <= limit; ++y) {
            cubes[x][y] = [];
            for (let z = 0; z <= limit; ++z) {
                cubes[x][y][z] = false;
            }
        }
    }

    let expr = /(?<cmd>(on|off)) x=(?<x1>\-?[0-9]+)\.\.(?<x2>\-?[0-9]+),y=(?<y1>\-?[0-9]+)..(?<y2>\-?[0-9]+),z=(?<z1>\-?[0-9]+)..(?<z2>\-?[0-9]+)/;
    let cmds = [];
    for await (const line of rl) {
        const {cmd, x1, x2, y1, y2, z1, z2} = expr.exec(line).groups;
        cmds.push({
            cmd: cmd === "on" ? true : false,
            x1: parseInt(x1, 10) + offset,
            x2: parseInt(x2, 10) + offset,
            y1: parseInt(y1, 10) + offset,
            y2: parseInt(y2, 10) + offset,
            z1: parseInt(z1, 10) + offset,
            z2: parseInt(z2, 10) + offset
        });
    }

    for (cmd of cmds) {
        if (cmd.x1 < 0 || cmd.y1 < 0 || cmd.z1 < 0 || 
            cmd.x1 > limit || cmd.y1 > limit || cmd.z1 > limit) {
            continue;
        }
        for (let x = cmd.x1; x <= cmd.x2; ++x) {
            for (let y = cmd.y1; y <= cmd.y2; ++y) {
                for (let z = cmd.z1; z <= cmd.z2; ++z) {
                    cubes[x][y][z] = cmd.cmd;
                }
            }
        }
    }

    let count = 0;
    for (let x = 0; x <= limit; ++x) {
        for (let y = 0; y <= limit; ++y) {
            for (let z = 0; z <= limit; ++z) {
                if (cubes[x][y][z]) {
                    count += 1;
                }
            }
        }
    }

    console.log(count);
    console.log(`(Took ${Math.round(performance.now() - startTime) / 1000}s)`);
}

processLineByLine();