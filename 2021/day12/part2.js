const fs = require('fs');
const readline = require('readline');
const _ = require('lodash');

async function processLineByLine() {
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
                let visitsSmall = path.visitsSmall;
                if (path.includes(next) && next.toLowerCase() === next) {
                    if (visitsSmall) {
                        continue;
                    } else {
                        visitsSmall = true;
                    }
                }
                
                const newPath = path.concat(next);
                newPath.visitsSmall = visitsSmall;
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
}

processLineByLine();