const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');
const { performance } = require('perf_hooks');

async function processLineByLine() {
    const startTime = performance.now();
    const fileStream = fs.createReadStream(process.argv[2] || 'input.txt');
    
    const rl = readline.createInterface({
        input: fileStream,
        crlfDelay: Infinity
    });
    
    let links = {};
    for await (const line of rl) {
        const [left, right] = line.split("-");
        if (!links[left]) {
            links[left] = [];
        }
        if (!links[right]) {
            links[right] = []
        }
        if (right !== "start" && left !== "end") {
            links[left].push(right);
        }
        if (left !== "start" && right !== "end") {
            links[right].push(left);
        }
    }
    
    console.log(links);
    
    let paths = [];
    const completePaths = [];
    paths.push(["start"]);
    while (paths.length > 0) {
        const newPaths = [];
        while (paths.length) {
            const path = paths.pop();
            if (!links[path[path.length-1]]) {
                continue;
            }
            
            for (let next of links[path[path.length-1]]) {
                if (path.includes(next) && next.toLowerCase() === next) {
                    continue;
                }
                
                const newPath = path.concat(next);
                if (next === "end") {
                    completePaths.push(newPath);
                } else {
                    newPaths.push(newPath);
                }
            }
        }
        paths = newPaths;
    }
    
    console.log(completePaths.length);
    console.log((performance.now() - startTime) / 1000);
}

processLineByLine();